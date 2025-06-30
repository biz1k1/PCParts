using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Storage.Common.Helpers
{
    public static class HashHelper
    {
        public static string ComputeSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Преобразуем в hex-строку
                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}
