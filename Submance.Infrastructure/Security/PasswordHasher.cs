using System.Security.Cryptography;
using System.Text;
using Submance.Application.Interfaces.Security;

namespace Submance.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            string computedHash = Hash(inputPassword);
            return computedHash == passwordHash;
        }
    }
}