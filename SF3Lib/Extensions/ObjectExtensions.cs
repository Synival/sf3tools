using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Extensions
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Gets all values of all fields with type T.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="obj">The object whose fields should be searched.</param>
        /// <param name="inherit">If 'true', values for fields with base classes will be returned.</param>
        /// <returns></returns>
        public static List<T> GetAllObjectsOfTypeInFields<T>(this object obj, bool inherit = true) where T : class
        {
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
        /// <returns></returns>
        public static string ToStringHex(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            else if (obj is byte)
            {
                return "0x" + ((byte) obj).ToString("X2");
            }
            else if (obj is sbyte)
            {
                return "0x" + ((sbyte) obj).ToString("X2");
            }
            else if (obj is ushort)
            {
                return "0x" + ((ushort) obj).ToString("X2");
            }
            else if (obj is short)
            {
                return "0x" + ((short) obj).ToString("X2");
            }
            else if (obj is uint)
            {
                return "0x" + ((uint) obj).ToString("X2");
            }
            else if (obj is int)
            {
                return "0x" + ((int) obj).ToString("X2");
            }
            else if (obj is ulong)
            {
                return "0x" + ((ulong) obj).ToString("X2");
            }
            else if (obj is long)
            {
                return "0x" + ((long) obj).ToString("X2");
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
