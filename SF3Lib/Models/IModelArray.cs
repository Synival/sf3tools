using SF3.Types;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SF3.Models
{
    public interface IModelArray
    {
        /// <summary>
        /// Loads models from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        bool Load();

        /// <summary>
        /// The Scenario the contained models belong to.
        /// </summary>
        ScenarioType Scenario { get; }

/*
        /// <summary>
        /// Resets all loaded data.
        /// </summary>
        /// <returns>'true' when a reset has occurred or nothing was loaded.</returns>
        bool Reset();

        /// <summary>
        /// Is 'true' when a successful Load() has occurred.
        /// </summary>
        bool IsLoaded { get; }
*/
    }

    public interface IModelArray<T> : IModelArray
    {
        /// <summary>
        /// A mutable array of models of type T.
        /// </summary>
        T[] Models { get; }
    }
}
