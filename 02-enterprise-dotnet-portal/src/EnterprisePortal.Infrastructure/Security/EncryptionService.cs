using System.Security.Cryptography;
using System.Text;
using EnterprisePortal.Application.Cases;
namespace EnterprisePortal.Infrastructure.Security;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    public EncryptionService(string? keyMaterial)
    {
        if (string.IsNullOrWhiteSpace(keyMaterial))
            throw new InvalidOperationException("Encryption key is not configured. Set Encryption:Key (Key Vault in production).");
        _key = Convert.FromBase64String(keyMaterial);
        if (_key.Length != 32) throw new ArgumentException("Encryption key must contain 32 random bytes.");
    }
    public string Encrypt(string plain)
    {
        var nonce = RandomNumberGenerator.GetBytes(AesGcm.NonceByteSizes.MaxSize);
        var plainBytes = Encoding.UTF8.GetBytes(plain);
        var cipher = new byte[plainBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Encrypt(nonce, plainBytes, cipher, tag);
        return Convert.ToBase64String(nonce.Concat(tag).Concat(cipher).ToArray());
    }
    public string Decrypt(string cipherText)
    {
        var all = Convert.FromBase64String(cipherText);
        if (all.Length < 28) throw new CryptographicException("Invalid encrypted value.");
        var nonce = all[..AesGcm.NonceByteSizes.MaxSize];
        var tag = all.AsSpan(AesGcm.NonceByteSizes.MaxSize, AesGcm.TagByteSizes.MaxSize).ToArray();
        var cipher = all[(AesGcm.NonceByteSizes.MaxSize + AesGcm.TagByteSizes.MaxSize)..];
        var plain = new byte[cipher.Length];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Decrypt(nonce, cipher, tag, plain);
        return Encoding.UTF8.GetString(plain);
    }
}
