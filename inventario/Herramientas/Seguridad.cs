using System.Security.Cryptography;
using System.Text;

namespace inventario.Herramientas
{
    public class Seguridad
    {
        // 1. Generar un Salt aleatorio
        public static string GenerarSalt(int tamano = 32)
        {
            var buffer = new byte[tamano];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        // 2. Cifrar Contraseña + Salt con SHA-256
        public static string GenerarHash(string password, string salt)
        {
            string combinado = password + salt;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinado));

                // Convertir el array de bytes a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
