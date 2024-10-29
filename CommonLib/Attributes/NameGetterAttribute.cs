using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommonLib.Attributes {
    /// <summary>
    /// An attribute with the name of a "getter" method that fetches the name of the associated property's value.
    /// </summary>
    public class NameGetterAttribute : Attribute {
        public NameGetterAttribute(string methodName) {
            MethodName = methodName;
        }

        /// <summary>
        /// Fetches the property's value's name using the fetcher method.
        /// </summary>
        /// <param name="containerObj">The object which contains this property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value)</returns>
        public string GetName(object containerObj, int value) {
            var method = containerObj.GetType().GetMethod(
                MethodName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return (string) method.Invoke(containerObj, new object[] { value });
        }

        public string MethodName { get; }
    }
}
