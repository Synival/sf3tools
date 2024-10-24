﻿using System.Linq;

namespace SF3.Extensions {
    public static class StringExtensions {
        public static string Indent(this string str, string indentString)
            => str = string.Join("\n", str.Split('\n').Select(x => (x == "") ? "" : (indentString + x)));
    }
}
