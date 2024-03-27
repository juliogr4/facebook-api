using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace facebook_api.Service.Hash
{
    public class PasswordHashing : IPasswordHashing
    {
        private readonly char delimiter = ';';
        private readonly int saltSize = 128 / 8;
        private readonly int hashSize = 256 / 8;
        private readonly int iterationCount = 100000;
        private readonly KeyDerivationPrf hashAlgorithm = KeyDerivationPrf.HMACSHA256;

        public string GenerateHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, hashAlgorithm, iterationCount, hashSize);
            return string.Join(delimiter, Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public bool VerifyHash(string newPassword, string hashedPassword)
        {
            string[] elements = hashedPassword.Split(';');
            byte[] hashDB = Convert.FromBase64String(elements[0]);
            byte[] saltDB = Convert.FromBase64String(elements[1]);

            byte[] hash = KeyDerivation.Pbkdf2(newPassword, saltDB, hashAlgorithm, iterationCount, hashSize);
            return CryptographicOperations.FixedTimeEquals(hash, hashDB);
        }
    }
}
