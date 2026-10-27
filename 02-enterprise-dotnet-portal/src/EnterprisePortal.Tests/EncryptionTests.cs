using EnterprisePortal.Infrastructure.Security;
namespace EnterprisePortal.Tests;
public class EncryptionTests
{
    [Fact] public void Roundtrip_Preserves_Text()
    {
        var s = new EncryptionService("test-key-do-not-use-in-prod");
        var cipher = s.Encrypt("sensitive note");
        Assert.Equal("sensitive note", s.Decrypt(cipher));
        Assert.NotEqual("sensitive note", cipher);
    }
    [Fact] public void Tampered_Cipher_Fails()
    {
        var s = new EncryptionService("test-key-do-not-use-in-prod");
        var cipher = s.Encrypt("hello");
        var tampered = cipher[..^4] + "AAAA";
        Assert.ThrowsAny<Exception>(() => s.Decrypt(tampered));
    }
}
