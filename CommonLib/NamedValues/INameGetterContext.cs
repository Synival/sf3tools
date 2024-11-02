using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.NamedValues
{
    /// <summary>
    /// Interface for the context passed to functions like NameGetterAttribute.GetName(value, context).
    /// The actual work of retrieving the named value is done here.
    /// </summary>
    public interface INameGetterContext
    {
        /// <summary>
        /// Fetches the name of a value.
        /// </summary>
        /// <param name="value">The value whose name we want to fetch.</param>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>The name of the value (if one exists).</returns>
        string GetName(int value, params object[] parameters);

        /// <summary>
        /// Fetches the name of a value and information about it.
        /// </summary>
        /// <param name="value">The value whose name we want to fetch.</param>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>Both the name of the value (if one exists) and information about the named value.</returns>
        NameAndInfo GetNameAndInfo(int value, params object[] parameters);

        /// <summary>
        /// Returns 'true' if GetNameAndInfo() is safe to use with this value and parameter.
        /// The conditions on whether or not this is safe depends on the implementation.
        /// </summary>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>'true' if GetNameAndInfo() can be used.</returns>
        bool CanGetNameAndInfo(int value, params object[] parameters);
    }
}
