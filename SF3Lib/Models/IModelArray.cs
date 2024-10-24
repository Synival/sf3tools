using SF3.Types;

namespace SF3.Models {
    /// <summary>
    /// Interface for any collection of SF3 models that can be modified.
    /// </summary>
    public interface IModelArray {
        /// <summary>
        /// Loads models from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        bool Load();

        /// <summary>
        /// The Scenario the contained models belong to.
        /// </summary>
        ScenarioType Scenario { get; }

        /// <summary>
        /// Resets all loaded data.
        /// </summary>
        /// <returns>'true' when a reset has occurred or nothing was loaded.</returns>
        bool Reset();

        /// <summary>
        /// Is 'true' when a successful Load() has occurred.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// The XML file to load for this resource.
        /// </summary>
        string ResourceFile { get; }

        /// <summary>
        /// An mutable array of models.
        /// </summary>
        object[] ModelObjs { get; }
    }

    /// <summary>
    /// Interface for a specific collection of SF3 models that can be modified.
    /// </summary>
    public interface IModelArray<T> : IModelArray {
        /// <summary>
        /// A mutable array of models of type T.
        /// </summary>
        T[] Models { get; }
    }
}
