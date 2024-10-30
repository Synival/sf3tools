namespace CommonLib {
    /// <summary>
    /// Contained for a value, its name, and all information about the value.
    /// </summary>
    public class NameAndInfo {
        public NameAndInfo(int value, INamedValueInfo info) {
            Value = value;
            Name = info.Values.TryGetValue(value, out string name) ? name : null;
            NameOrValueStr = (Name != null) ? Name : Value.ToString(Info.FormatString);
            Info = info;
        }

        public int Value { get; }
        public string Name { get; }
        public string NameOrValueStr { get; }
        public INamedValueInfo Info { get; }
    };
}
