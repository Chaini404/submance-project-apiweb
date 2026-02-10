using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; // Necesario para la encriptación


namespace Submance.Infrastructure.Security
{
    public class PasswordHasher
    {
        // Método para crear el Hash (Encriptar)
        public string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Método para verificar si la contraseña coincide
        public bool Verify(string password, string hash)
        {
            string nuevoHash = Hash(password);
            return nuevoHash == hash;
        }
    }
}