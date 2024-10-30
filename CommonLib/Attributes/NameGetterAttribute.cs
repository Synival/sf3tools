using System;
using System.Reflection;
using CommonLib.NamedValues;

namespace CommonLib.Attributes {
    /// <summary>
    /// An attribute with the name of a "getter" method that fetches the name of the associated property's value.
    /// </summary>
    public class NameGetterAttribute : Attribute {
        /// <summary>
        /// Creates a NameGetterAttribute with an optional parameter that is passed to the INameGetterContext
        /// when fetching names and info.
        /// </summary>
        /// <param name="parameter">Optional parameter used when fetching names and info.</param>
        public NameGetterAttribute(object parameter) {
            Parameter = parameter;
        }

        /// <summary>
        /// Fetches the property's value's name and all information about it.
        /// </summary>
        /// <param name="context">The object that will provide the name for this value.</param>
        /// <param name="value">The value for whose name we're trying to fetch.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value)</returns>
        public NameAndInfo GetNameAndInfo(INameGetterContext context, int value)
            => context.GetNameAndInfo(value, Parameter);

        /// <summary>
        /// Fetches the property's value's name using the fetcher method.
        /// </summary>
        /// <param name="context">The object that will provide the name for this value.</param>
        /// <param name="value">The value for whose name we're trying to fetch.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value).Name</returns>
        public string GetName(INameGetterContext context, int value)
            => GetNameAndInfo(context, value).Name;

        /// <summary>
        /// Fetches the property's value's info using the fetcher method.
        /// </summary>
        /// <param name="context">The object that will provide the info for this value.</param>
        /// <param name="value">The value for whose name we're trying to fetch.</param>
        /// <returns>The return value of: containerObj.<MethodName>(value).Info</returns>
        public INamedValueInfo GetInfo(INameGetterContext context, int value)
            => GetNameAndInfo(context, value).Info;

        /// <summary>
        /// The parameter used when fetching names and info.
        /// </summary>
        public object Parameter { get; }
    }
}
