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
        /// Fetches the info for a named value.
        /// </summary>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>The information associated with the named value.</returns>
        INamedValueInfo GetInfo(params object[] parameters);

        /// <summary>
        /// Returns 'true' if GetName() is safe to use with this value and these parameters.
        /// The conditions on whether or not this is safe depends on the implementation.
        /// </summary>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>'true' if GetName() can be used.</returns>
        bool CanGetName(int value, params object[] parameters);

        /// <summary>
        /// Returns 'true' if GetInfo() is safe to use with these parameters.
        /// The conditions on whether or not this is safe depends on the implementation.
        /// </summary>
        /// <param name="parameters">Optional parameters assigned to the NameGetterAttribute.</param>
        /// <returns>'true' if GetInfo() can be used.</returns>
        bool CanGetInfo(params object[] parameters);
    }
}
