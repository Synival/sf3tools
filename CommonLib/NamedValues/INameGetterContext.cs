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
        /// Fetches the name of a value and information about it.
        /// </summary>
        /// <param name="value">The value whose name we want to fetch.</param>
        /// <param name="parameter">Optional parameter assigned to the NameGetterAttribute.</param>
        /// <returns>Both the name of the value (if one exists) and information about the named value.</returns>
        NameAndInfo GetNameAndInfo(int value, object parameter);
    }
}
