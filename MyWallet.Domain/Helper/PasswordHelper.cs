using System.Security.Cryptography;
using System.Text;

namespace MyWallet.Domain.Helper
{
    public class PasswordHelper
    {
        public static string HashPasswordThrice(string password)
        {
            using var sha256 = SHA256.Create();
            var firstHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var secondHash = sha256.ComputeHash(firstHash);
            var thirdHash = sha256.ComputeHash(secondHash);
            return Convert.ToBase64String(secondHash);
        }
    }
}
