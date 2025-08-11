using Microsoft.AspNetCore.DataProtection;

namespace FairShare.Application.Helpers;

public class EncryptionHelper
{
    private static readonly IDataProtector _protector;

    static EncryptionHelper()
    {
        var provider = DataProtectionProvider.Create("MyApp");
        _protector = provider.CreateProtector("PasswordProtector");
    }

    public static string Encrypt(string plainText)
    {
        return _protector.Protect(plainText);
    }

    public static string Decrypt(string encryptedText)
    {
        return _protector.Unprotect(encryptedText);
    }
}
