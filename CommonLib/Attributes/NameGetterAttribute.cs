using System;

namespace CommonLib.Attributes {
    /// <summary>
    /// An attribute with the name of a "getter" method that fetches the name of the associated property's value.
    /// </summary>
    public class NameGetterAttribute : Attribute {
        /// <summary>
        /// Creates a NameGetterAttribute with optional parameters that is passed to the INameGetterContext
        /// when fetching names and info.
        /// </summary>
        /// <param name="parameters">Optional parameters used when fetching names and info.</param>
        public NameGetterAttribute(params object[] parameters) {
            Parameters = parameters;
        }

        /// <summary>
        /// The parameter used when fetching names and info.
        /// </summary>
        public object[] Parameters { get; }
    }
}
