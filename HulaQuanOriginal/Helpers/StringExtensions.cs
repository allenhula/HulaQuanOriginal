using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HulaQuanOriginal.Helpers
{
    public static class StringExtensions
    {
        public static string AppendSuffix(this string originalString, string suffix)
        {
            var lastPoint = originalString.LastIndexOf('.');
            var strBuilder = new StringBuilder();
            strBuilder.Append(originalString.Substring(0, lastPoint));
            strBuilder.Append(suffix);
            strBuilder.Append(originalString.Substring(lastPoint));
            return strBuilder.ToString();
        }
    }
}