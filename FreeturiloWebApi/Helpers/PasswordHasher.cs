using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers
{
    public class PasswordHasher
    {
        /// <summary>
        /// Generates hash of password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
