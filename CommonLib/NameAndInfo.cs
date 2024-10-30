namespace CommonLib {
    /// <summary>
    /// Contained for a value, its name, and all information about the value.
    /// </summary>
    public class NameAndInfo {
        public NameAndInfo(int value, INamedValueInfo info) {
            Value = value;
            Name = info.Values.TryGetValue(value, out string name) ? name : null;
            Info = info;
        }

        public int Value { get; }
        public string Name { get; }
        public INamedValueInfo Info { get; }
    };
}
