using Isopoh.Cryptography.Argon2;

namespace UrlShortener.Service
{
    public class PasswordHashing
    {
        public static string HashPassword(string password)
        {
            return Argon2.Hash(password);
        }

        public static bool VerifyPassword(string hash, string password)
        {
            return Argon2.Verify(hash, password);
        }
    }
}
