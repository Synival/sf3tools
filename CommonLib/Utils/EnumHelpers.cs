using System;

namespace CommonLib.Utils {
    public static class EnumHelpers {
        /// <summary>
        /// Returns the name of the enum if it's defined or an alternative string retrieved with a function.
        /// </summary>
        /// <typeparam name="T">Type of the enum whose value you want to retrieve.</typeparam>
        /// <param name="value">Value to retrieve a name from.</param>
        /// <param name="alternate">Function to use if the value in the requested enum T is not defined.</param>
        /// <returns>The name of the enum value if it is defined or a result of alternate(value).</returns>
        public static string EnumNameOr<T>(T value, Func<T, string> alternate) where T : Enum
            => Enum.IsDefined(typeof(T), value) ? value.ToString() : alternate(value);

        /// <summary>
        /// Gets the attribute of an enum value, if available.
        /// </summary>
        /// <typeparam name="T">Type of the Enum</typeparam>
        /// <param name="val">Value of the Enum</param>
        /// <returns>An attribute if found, or null.</returns>
        public static T GetAttributeOfType<T>(this Enum val) where T : Attribute
        {
            var type = val.GetType();
            var memberInfo = type.GetMember(val.ToString());
            var attributes = (memberInfo.Length > 0) ? memberInfo[0].GetCustomAttributes(typeof(T), false) : null;
            return (attributes?.Length > 0) ? (T) attributes[0] : null;
        }
    } 
}
