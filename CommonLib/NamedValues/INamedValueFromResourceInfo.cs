namespace CommonLib.NamedValues {
    /// <summary>
    /// Interface for value info for names that come from a resource.
    /// </summary>
    public interface INamedValueFromResourceInfo : INamedValueInfo {
        string ResourceName { get; }
    };
}
