using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using EnterprisePortal.Web.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace EnterprisePortal.Web.Controllers;

public sealed class LoginModel
{
    [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = "";
    [Required, StringLength(200)] public string Password { get; set; } = "";
    public string? ReturnUrl { get; set; }
}
public class AuthController(DemoCredentials credentials) : Controller
{
    [AllowAnonymous, HttpGet] public IActionResult Login(string? returnUrl) => View(new LoginModel { ReturnUrl = returnUrl });
    [AllowAnonymous, HttpPost, EnableRateLimiting("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var validPassword = credentials.Verify(model.Password);
        var role = model.Email switch { "worker@example.test" => "CaseWorker", "auditor@example.test" => "Auditor", _ => null };
        if (!validPassword || role is null) { ModelState.AddModelError("", "Email or password is incorrect."); return View(model); }
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Email), new Claim(ClaimTypes.Role, role) }, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return LocalRedirect(Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl! : "/");
    }
    [HttpPost] public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(); return RedirectToAction(nameof(Login)); }
    public IActionResult Denied() => StatusCode(403);
}
