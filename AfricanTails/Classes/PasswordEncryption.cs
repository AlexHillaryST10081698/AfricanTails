using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AfricanTails.Classes
{
    internal class PasswordEncryption
    {
        public string HashedPassword(string password)
        {
            SHA1CryptoServiceProvider sHaiHash = new SHA1CryptoServiceProvider();
            byte[] hashPassword_bytes = Encoding.UTF8.GetBytes(password);
            byte[] salt_bytes = sHaiHash.ComputeHash(hashPassword_bytes);
            return Convert.ToBase64String(salt_bytes);

        }
    }
}
