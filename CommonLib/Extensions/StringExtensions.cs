using System.ComponentModel;
using System.Linq;

namespace CommonLib.Extensions {
    public static class StringExtensions {
        public static string Indent(this string str, string indentString)
            => str = string.Join("\n", str.Split('\n').Select(x => x == "" ? "" : indentString + x));

        /// <summary>
        /// Converts a string to type T as using the Int32Converter.
        /// Accepts a string as a series of decimal numbers or a hex string in format "0xNN...".
        /// </summary>
        /// <typeparam name="T">The type to convert 'str' into.</typeparam>
        /// <param name="str">The string to convert to T.</param>
        /// <returns>A number of type T converted from 'str'.</returns>
        public static T ConvertFromDecOrHex<T>(this string str)
            => (T) new Int32Converter().ConvertFromString(str);
    }
}
