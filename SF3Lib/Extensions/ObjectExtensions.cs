using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SF3.Extensions {
    public static partial class ObjectExtensions {
        /// <summary>
        /// Gets all values of all fields with type T.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="obj">The object whose fields should be searched.</param>
        /// <param name="inherit">If 'true', values for fields with base classes will be returned.</param>
        /// <returns></returns>
        public static List<T> GetAllObjectsOfTypeInFields<T>(this object obj, bool inherit = true) where T : class {
            var fields = obj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                (inherit ? 0 : BindingFlags.DeclaredOnly)
            );

            return fields
                .Where(x => x.FieldType == typeof(T))
                .Select(x => x.GetValue(obj) as T)
                .ToList();
        }

        /// <summary>
        /// Returns a hex value for the string if possible. Otherwise returns ToString().
        /// </summary>
        /// <param name="obj">Object to ToString() or return a hex value for.</param>
        /// <returns>The value of 'obj' formatted, if possible, with 'formatString' or "X2".
        /// Otherwise returns obj.ToString().</returns>
        public static string ToStringHex(this object obj, string formatString = null) {
            switch (obj) {
                case null:
                    throw new ArgumentNullException(nameof(obj));
                case byte v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case sbyte v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case ushort v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case short v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case uint v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case int v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case ulong v:
                    return "0x" + v.ToString(formatString ?? "X2");
                case long v:
                    return "0x" + v.ToString(formatString ?? "X2");
                default:
                    return obj.ToString();
            }
        }
    }
}
