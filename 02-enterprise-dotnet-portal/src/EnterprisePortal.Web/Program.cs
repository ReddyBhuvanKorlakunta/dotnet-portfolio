using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnterprisePortal.Application.Cases;
using EnterprisePortal.Infrastructure.Data;
using EnterprisePortal.Infrastructure.Security;
using EnterprisePortal.Web.Auth;
using EnterprisePortal.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// --- Configuration, fail fast in Production if secrets are missing ---
var conn = builder.Configuration.GetConnectionString("Default");
var encKey = builder.Configuration["Encryption:Key"];
var jwtKey = builder.Configuration["Jwt:Key"];
if (env.IsProduction() && (string.IsNullOrWhiteSpace(conn) || string.IsNullOrWhiteSpace(encKey) || string.IsNullOrWhiteSpace(jwtKey)))
    throw new InvalidOperationException("Production requires ConnectionStrings:Default, Encryption:Key and Jwt:Key (Key Vault). Refusing to start with dev defaults.");
if (string.IsNullOrWhiteSpace(encKey)) encKey = "dev-only-change-me";
if (string.IsNullOrWhiteSpace(jwtKey)) jwtKey = "dev-only-change-me-jwt-key-must-be-at-least-32-chars";

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn ?? "Server=(localdb)\mssqllocaldb;Database=EnterprisePortal;Trusted_Connection=True;"));
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IReferenceGenerator, ReferenceGenerator>();
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(encKey));
builder.Services.AddScoped<CaseService>();
builder.Services.AddSingleton(new JwtSettings(jwtKey, builder.Configuration["Jwt:Issuer"] ?? "EnterprisePortal"));
builder.Services.AddScoped<AuthService>();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false, ValidateAudience = false, ClockSkew = TimeSpan.FromMinutes(2),
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseMiddleware<RequestIdMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();
if (!env.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
else app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
// Seed dummy data only on explicit flag, never on a normal production start:
//   dotnet run -- seed
if (args.Contains("seed"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    if (!await db.Cases.AnyAsync())
    {
        db.Cases.AddRange(SeedData.Cases());
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded dummy cases.");
    }
    return;
}
app.Run();
public partial class Program { } // for integration tests
