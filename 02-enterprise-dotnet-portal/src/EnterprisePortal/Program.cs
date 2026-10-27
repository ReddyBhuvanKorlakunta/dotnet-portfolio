using EnterprisePortal.Data;
using EnterprisePortal.Middleware;
using EnterprisePortal.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddDataProtection(); // tokens/cookies; keys to Key Vault in prod
builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<AuditLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.Run();
