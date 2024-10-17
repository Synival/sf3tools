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
    }
}
