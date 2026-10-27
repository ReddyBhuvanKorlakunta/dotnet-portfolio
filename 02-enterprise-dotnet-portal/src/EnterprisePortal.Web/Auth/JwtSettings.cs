using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace EnterprisePortal.Web.Auth;
public record JwtSettings(string Key, string Issuer);
/// <summary>Dev-only token issuer. Production replaces this with Entra ID /
/// Azure AD — the API only trusts the JWT bearer scheme, so swapping the
/// issuer does not change controllers, services or tests. Demo accounts are
/// local-dev only and must never exist in production configuration.</summary>
public class AuthService(JwtSettings settings)
{
    // Dev-only demo accounts. Passwords are NOT stored here in production.
    private static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        ["admin@example.test"] = ("Admin123!", "Admin"),
        ["worker@example.test"] = ("Worker123!", "CaseWorker"),
    };
    public string? Login(string email, string password, out string role)
    {
        role = "";
        if (!Users.TryGetValue(email, out var u) || u.Password != password) return null;
        role = u.Role;
        var claims = new[] { new Claim(ClaimTypes.Name, email), new Claim(ClaimTypes.Role, role) };
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(settings.Issuer, null, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
public record LoginRequest(string Email, string Password);
