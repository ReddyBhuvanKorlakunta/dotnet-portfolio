using EnterprisePortal.Web.Auth;
using Microsoft.AspNetCore.Mvc;
namespace EnterprisePortal.Web.Controllers;
[ApiController, Route("api/v1/auth")]
public class AuthController(AuthService auth) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var token = auth.Login(req.Email, req.Password, out var role);
        return token is null ? Unauthorized(new { error = "Invalid credentials." }) : Ok(new { token, role });
    }
}
