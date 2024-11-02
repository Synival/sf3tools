using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using static CommonLib.Utils.Utils;

namespace CommonLib.Extensions {
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
        public static string ToStringHex(this object obj, string formatString = null, string prefix = "0x") {
            switch (obj) {
                case null:
                    throw new ArgumentNullException(nameof(obj));
                case byte v:
                    return prefix + v.ToString(formatString ?? "X2");
                case sbyte v:
                    return prefix + v.ToString(formatString ?? "X2");
                case ushort v:
                    return prefix + v.ToString(formatString ?? "X2");
                case short v:
                    return prefix + v.ToString(formatString ?? "X2");
                case uint v:
                    return prefix + v.ToString(formatString ?? "X2");
                case int v:
                    return prefix + v.ToString(formatString ?? "X2");
                case ulong v:
                    return prefix + v.ToString(formatString ?? "X2");
                case long v:
                    return prefix + v.ToString(formatString ?? "X2");
                default:
                    return obj.ToString();
            }
        }

        /// <summary>
        /// Returns a nicely formatted named value for an object's property if a NamedValueAttribute is present.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="obj">The object that contains the proeprty with the named value.</param>
        /// <param name="property">The property that has the named value.</param>
        /// <param name="context">The context which knows how to fetch names and does the work for it.</param>
        /// <returns>A nicely formatted value + name or 'null' if no NamedValueAttribute was supplied.</returns>
        public static string GetPropertyValueName(this object obj, PropertyInfo property, INameGetterContext context) {
            if (obj == null || property == null || context == null)
                return null;

            var attr = property.GetCustomAttribute<NameGetterAttribute>();
            if (attr == null)
                return null;

            object value = property.GetValue(obj);
            var hexValue = value.ToStringHex(null, "");
            var intValue = Convert.ToInt32(hexValue, 16);

            if (!context.CanGetInfo(obj, property, attr.Parameters) || !context.CanGetName(obj, property, intValue, attr.Parameters))
                return null;

            return GetFullName(
                intValue,
                context.GetName(obj, property, intValue, attr.Parameters),
                context.GetInfo(obj, property, attr.Parameters).FormatString
            );
        }

        /// <summary>
        /// Returns a nicely formatted named value for an object's property if a NamedValueAttribute is present.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="obj">The object that contains the proeprty with the named value.</param>
        /// <param name="property">The property that has the named value.</param>
        /// <param name="context">The context which knows how to fetch names and does the work for it.</param>
        /// <param name="value">A specific value to get a name for.</param>
        /// <returns>A nicely formatted value + name or 'null' if no NamedValueAttribute was supplied.</returns>
        public static string GetPropertyValueName(this object obj, PropertyInfo property, INameGetterContext context, object value) {
            if (obj == null || property == null || context == null)
                return null;

            var attr = property.GetCustomAttribute<NameGetterAttribute>();
            if (attr == null)
                return null;

            var hexValue = value.ToStringHex(null, "");
            var intValue = Convert.ToInt32(hexValue, 16);

            if (!context.CanGetInfo(obj, property, attr.Parameters) || !context.CanGetName(obj, property, intValue, attr.Parameters))
                return null;

            return GetFullName(
                intValue,
                context.GetName(obj, property, intValue, attr.Parameters),
                context.GetInfo(obj, property, attr.Parameters).FormatString
            );
        }
    }
}
