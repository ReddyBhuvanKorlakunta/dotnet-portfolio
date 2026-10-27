using System.Security.Cryptography;
using System.Text;
using EnterprisePortal.Application.Cases;
namespace EnterprisePortal.Infrastructure.Security;
/// <summary>AES-GCM for sensitive fields. Key MUST come from configuration /
/// Azure Key Vault in production. The fallback below is dev-only and is
/// refused at startup when the environment is Production (see Program.cs).</summary>
public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    public EncryptionService(string? keyMaterial)
    {
        if (string.IsNullOrWhiteSpace(keyMaterial))
            throw new InvalidOperationException("Encryption key is not configured. Set Encryption:Key (Key Vault in production).");
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(keyMaterial));
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
        var nonce = all[..AesGcm.NonceByteSizes.MaxSize];
        var tag = all.AsSpan(AesGcm.NonceByteSizes.MaxSize, AesGcm.TagByteSizes.MaxSize).ToArray();
        var cipher = all[(AesGcm.NonceByteSizes.MaxSize + AesGcm.TagByteSizes.MaxSize)..];
        var plain = new byte[cipher.Length];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Decrypt(nonce, cipher, tag, plain);
        return Encoding.UTF8.GetString(plain);
    }
}
