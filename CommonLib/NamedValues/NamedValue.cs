using System;
using System.Collections.Generic;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Concrete type with both a Name (string) and a Value (int).
    /// Used when values need both names to look up and must be identifiable as a specific type.
    /// </summary>
    public abstract class NamedValue : IComparable, IComparable<NamedValue> {
        public NamedValue(string name, string valueName, int value) {
            Name = name;
            ValueName = valueName;
            Value = value;
        }

        /// <summary>
        /// Factory function for making another value using the same settings as the current value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract NamedValue MakeRelatedValue(int value);

        /// <summary>
        /// The minimum value that related values can be.
        /// </summary>
        public abstract int MinValue { get; }

        /// <summary>
        /// The maximum value that related values can be.
        /// </summary>
        public abstract int MaxValue { get; }

        /// <summary>
        /// Returns a dictionary with possible values and names for possible related values.
        /// </summary>
        public abstract Dictionary<int, string> PossibleValues { get; }

        /// <summary>
        /// Returns a dictionary with possible values and names for display and modifying the value in a ComboBox.
        /// </summary>
        public abstract Dictionary<NamedValue, string> ComboBoxValues { get; }

        public override string ToString() => ValueName;

        /// <summary>
        /// The name of the value.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Combination of value + name.
        /// </summary>
        public string ValueName { get; }

        /// <summary>
        /// Value represented.
        /// </summary>
        public int Value { get; }

        public static bool operator <(NamedValue lhs, NamedValue rhs) => lhs.Value < rhs.Value;
        public static bool operator >(NamedValue lhs, NamedValue rhs) => lhs.Value > rhs.Value;
        public static bool operator ==(NamedValue lhs, NamedValue rhs) => lhs.Value == rhs.Value;
        public static bool operator !=(NamedValue lhs, NamedValue rhs) => lhs.Value != rhs.Value;
        public override bool Equals(object rhs) => rhs is NamedValue ? Value == (rhs as NamedValue).Value : base.Equals(rhs);
        public override int GetHashCode() => Value;

        public int CompareTo(object rhs) => rhs is NamedValue ? CompareTo(rhs as NamedValue) : throw new NotImplementedException();
        public int CompareTo(NamedValue rhs) => Value < rhs.Value ? -1 : Value == rhs.Value ? 0 : 1;
    }
}
