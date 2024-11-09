using static CommonLib.Utils.Utils;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Contained for a value, its name, and all information about the value.
    /// </summary>
    public class NameAndInfo {
        public NameAndInfo(int value, INamedValueInfo info) {
            Value = value;
            Name = NameOrNull(value, info.Values);
            NameOrValueStr = Name ?? value.ToString(info.FormatString);
            Info = info;
        }

        public int Value { get; }
        public string Name { get; }
        public string NameOrValueStr { get; }
        public INamedValueInfo Info { get; }
    };
}
