using System.Security.Cryptography;
using System.Text.Json.Serialization;
using EnterprisePortal.Application.Cases;
using EnterprisePortal.Infrastructure.Data;
using EnterprisePortal.Infrastructure.Security;
using EnterprisePortal.Web.Auth;
using EnterprisePortal.Web.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
var demo = builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Demo:Enabled");
if (!demo && builder.Configuration["Storage:Provider"] != "SqlServer") throw new InvalidOperationException("Non-demo hosting requires SQL Server.");
var key = builder.Configuration["Encryption:Key"];
if (string.IsNullOrWhiteSpace(key)) throw new InvalidOperationException("Set Encryption__Key to a base64 encoded 32-byte random key.");
var password = builder.Configuration["Auth:Password"];
if (string.IsNullOrWhiteSpace(password) || password.Length < 14) throw new InvalidOperationException("Set Auth__Password to at least 14 characters.");
if (!demo) throw new InvalidOperationException("Production release requires agency identity provider integration and security review. This portfolio host is demo-only.");
builder.Services.AddSingleton(new DemoCredentials(password));
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key));
builder.Services.AddSingleton<StoreGate>();
builder.Services.AddScoped<ICaseStorage>(sp =>
{
#if SQLSERVER
    if (builder.Configuration["Storage:Provider"] == "SqlServer") return new SqlServerStorage(builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("SQL connection required."), sp.GetRequiredService<IEncryptionService>());
#endif
    if (builder.Configuration["Storage:Provider"] == "SqlServer") throw new InvalidOperationException("Build with -p:UseSqlServer=true.");
    return new DemoFileStorage(builder.Configuration["Demo:DataPath"] ?? "App_Data/cases.enc", sp.GetRequiredService<IEncryptionService>());
});
builder.Services.AddScoped<CaseRepository>();
builder.Services.AddScoped<ICaseRepository>(sp => sp.GetRequiredService<CaseRepository>());
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CaseRepository>());
builder.Services.AddSingleton<IReferenceGenerator, ReferenceGenerator>();
builder.Services.AddScoped<CaseService>();
builder.Services.AddControllersWithViews(o => { o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); }).AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddAntiforgery(o => { o.HeaderName = "X-CSRF-TOKEN"; o.Cookie.SameSite = SameSiteMode.Strict; });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.LoginPath = "/Auth/Login"; o.AccessDeniedPath = "/Auth/Denied"; o.ExpireTimeSpan = TimeSpan.FromMinutes(20); o.SlidingExpiration = false;
    o.Cookie.HttpOnly = true; o.Cookie.SameSite = SameSiteMode.Strict; o.Cookie.SecurePolicy = demo ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
    o.Events.OnRedirectToLogin = c => { if (c.Request.Path.StartsWithSegments("/api")) c.Response.StatusCode = 401; else c.Response.Redirect(c.RedirectUri); return Task.CompletedTask; };
    o.Events.OnRedirectToAccessDenied = c => { c.Response.StatusCode = 403; return Task.CompletedTask; };
});
builder.Services.AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
builder.Services.AddRateLimiter(o => { o.RejectionStatusCode = 429; o.AddFixedWindowLimiter("login", p => { p.PermitLimit = 10; p.Window = TimeSpan.FromMinutes(1); p.QueueLimit = 0; }); });
var app = builder.Build();
app.UseExceptionHandler("/Home/Error");
app.UseMiddleware<RequestIdMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();
if (!demo) { app.UseHsts(); app.UseHttpsRedirection(); }
app.UseStaticFiles(); app.UseRouting(); app.UseRateLimiter(); app.UseAuthentication(); app.UseAuthorization();
app.MapGet("/health/live", () => Results.Ok(new { status = "live" })).AllowAnonymous();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
public partial class Program { }
