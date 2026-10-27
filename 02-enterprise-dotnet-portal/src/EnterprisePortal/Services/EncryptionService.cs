using System.Security.Cryptography;
using System.Text;

namespace EnterprisePortal.Services;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

/// <summary>
/// AES-GCM for sensitive fields at rest. Demo key only — in production the
/// key comes from configuration / Azure Key Vault and is never committed.
/// GCM gives confidentiality + integrity; ECB is never used.
/// </summary>
public class EncryptionService : IEncryptionService
{
    // DEV ONLY placeholder. Replace with Key Vault / config in production.
    private static readonly byte[] Key = SHA256.HashData(Encoding.UTF8.GetBytes("dev-only-change-me"));

    public string Encrypt(string plainText)
    {
        var nonce = RandomNumberGenerator.GetBytes(AesGcm.NonceByteSizes.MaxSize);
        var plain = Encoding.UTF8.GetBytes(plainText);
        var cipher = new byte[plain.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        using var aes = new AesGcm(Key, tag.Length);
        aes.Encrypt(nonce, plain, cipher, tag);
        return Convert.ToBase64String(nonce.Concat(tag).Concat(cipher).ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var all = Convert.FromBase64String(cipherText);
        var nonce = all[..AesGcm.NonceByteSizes.MaxSize];
        var tag = all.AsSpan(AesGcm.NonceByteSizes.MaxSize, AesGcm.TagByteSizes.MaxSize).ToArray();
        var cipher = all[(AesGcm.NonceByteSizes.MaxSize + AesGcm.TagByteSizes.MaxSize)..];
        var plain = new byte[cipher.Length];
        using var aes = new AesGcm(Key, tag.Length);
        aes.Decrypt(nonce, cipher, tag, plain);
        return Encoding.UTF8.GetString(plain);
    }
}
