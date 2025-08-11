// File Path: Application/Services/EncryptionService.cs
using System.Security.Cryptography;
using System.Text;

public class EncryptionService
{
    // ⭐ ለምሳሌ ያህል ብቻ ሃርድ ኮድ የተደረገ ቁልፍ እና IV። በምርት ላይ (production) አትጠቀምበት ⭐
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("a2b1c4e6f8d1e2a3c5d7b9f1e2a3b4c5"); // 32 bytes for AES-256
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("a2b1c4e6f8d1e2a3"); // 16 bytes

    public static string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}