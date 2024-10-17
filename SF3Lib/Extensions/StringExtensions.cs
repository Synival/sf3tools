using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(this string str, string indentString)
        {
            return str = string.Join("\n", str.Split('\n').Select(x => (x == "") ? "" : (indentString + x)));
        }
    }
}
