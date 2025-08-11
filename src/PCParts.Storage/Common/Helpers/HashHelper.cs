using System.Security.Cryptography;
using System.Text;

namespace PCParts.Storage.Common.Helpers;

public static class HashHelper
{
    public static string ComputeSha256(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(inputBytes);

            // Преобразуем в hex-строку
            var builder = new StringBuilder();
            foreach (var b in hashBytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }
    }
}
