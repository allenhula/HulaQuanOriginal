using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HulaQuanOriginal.Helpers
{
    public class PasswordHelper
    {
        public static string EncodePassword(string password, string key)
        {
            var pwdInBytes = Encoding.UTF8.GetBytes(password);
            var hmac256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(hmac256.ComputeHash(pwdInBytes));
        }
    }
}