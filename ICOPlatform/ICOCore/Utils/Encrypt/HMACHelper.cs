using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Utils.Encrypt
{
    public class HMACHelper
    {
        public static string CreateToken_HMAC_SHA256(string secret, string message)
        {
            secret = secret ?? "";
            //var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static string ComputeHash_HMAC_SHA256(string secret, string message)
        {
            var key = Encoding.UTF8.GetBytes(secret.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }
    }
}
