using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.Helpers
{
    public class StringHelper
    {
        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static Random random = new Random();

        public static string GetRandomString(int length = 8)
        {
            return new string(
                Enumerable.Repeat(Chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        public static string GetRandomString(DateTime date)
        {
            return $"{date.ToString("yyyyMMdd")}{GetRandomString()}";
        }
    }
}