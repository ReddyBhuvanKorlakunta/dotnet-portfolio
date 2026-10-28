using System.Security.Cryptography;
namespace EnterprisePortal.Web.Auth;

public sealed class DemoCredentials
{
    private readonly byte[] salt = RandomNumberGenerator.GetBytes(32);
    private readonly byte[] hash;
    public DemoCredentials(string password) => hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 210000, HashAlgorithmName.SHA256, 32);
    public bool Verify(string password) => CryptographicOperations.FixedTimeEquals(hash, Rfc2898DeriveBytes.Pbkdf2(password, salt, 210000, HashAlgorithmName.SHA256, 32));
}
