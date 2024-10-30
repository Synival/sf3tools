using System;
using System.Reflection;

namespace CommonLib.Attributes {
    /// <summary>
    /// An attribute with the name of a "getter" method that fetches the name of the associated property's value.
    /// </summary>
    public class NameGetterAttribute : Attribute {
        public NameGetterAttribute(string methodName) {
            MethodName = methodName;
        }

        /// <summary>
        /// Fetches the property's value's name and all information about it.
        /// </summary>
        /// <param name="containerObj">The object which contains this property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value)</returns>
        public NameAndInfo GetNameAndInfo(object containerObj, int value = 0) {
            var method = containerObj.GetType().GetMethod(
                MethodName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return (NameAndInfo) method.Invoke(containerObj, new object[] { value });
        }

        /// <summary>
        /// Fetches the property's value's name using the fetcher method.
        /// </summary>
        /// <param name="containerObj">The object which contains this property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value).Name</returns>
        public string GetName(object containerObj, int value)
            => GetNameAndInfo(containerObj, value).Name;

        /// <summary>
        /// The name of the method that fetches a NameAndInfo instance.
        /// </summary>
        public string MethodName { get; }
    }
}
