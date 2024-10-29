using System;
using System.Collections.Generic;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Concrete type with both a Name (string) and a Value (int).
    /// Used when values need both names to look up and must be identifiable as a specific type.
    /// </summary>
    public abstract class NamedValue : IComparable, IComparable<NamedValue> {
        public NamedValue(string name, string valueStr, int value) {
            Name = name;
            ValueStr = valueStr;
            NameOrValueStr = name ?? valueStr;
            FullName = valueStr + ((name != null) ? (": " + name) : "");
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

        public override string ToString() => FullName;

        /// <summary>
        /// Name of the value, or 'null' if it doesn't have one.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Combination of value + name.
        /// </summary>
        public string ValueStr { get; }

        /// <summary>
        /// The name of the value or, if there is no name, the value as a string.
        /// </summary>
        public string NameOrValueStr { get; }

        /// <summary>
        /// Full combination of value + name.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Value represented.
        /// </summary>
        public int Value { get; }

        public static bool operator <(NamedValue lhs, NamedValue rhs) => lhs.Value < rhs.Value;
        public static bool operator >(NamedValue lhs, NamedValue rhs) => lhs.Value > rhs.Value;
        public static bool operator ==(NamedValue lhs, NamedValue rhs) => lhs.Value == rhs.Value;
        public static bool operator !=(NamedValue lhs, NamedValue rhs) => lhs.Value != rhs.Value;

        public static bool operator <(NamedValue lhs, int rhs) => lhs.Value < rhs;
        public static bool operator >(NamedValue lhs, int rhs) => lhs.Value > rhs;
        public static bool operator ==(NamedValue lhs, int rhs) => lhs.Value == rhs;
        public static bool operator !=(NamedValue lhs, int rhs) => lhs.Value != rhs;

        public static bool operator <(int lhs, NamedValue rhs) => lhs < rhs.Value;
        public static bool operator >(int lhs, NamedValue rhs) => lhs > rhs.Value;
        public static bool operator ==(int lhs, NamedValue rhs) => lhs == rhs.Value;
        public static bool operator !=(int lhs, NamedValue rhs) => lhs != rhs.Value;

        public override bool Equals(object rhs) =>
            rhs is NamedValue rhsNV ? Value == rhsNV.Value :
            rhs is int rhsInt ? Value == rhsInt :
            base.Equals(rhs);

        public override int GetHashCode() => Value;

        public int CompareTo(object rhs) =>
            rhs is NamedValue rhsNV ? CompareTo(rhsNV) :
            rhs is int rhsInt ? CompareTo(rhsInt) :
            throw new NotImplementedException();

        public int CompareTo(NamedValue rhs) => Value < rhs.Value ? -1 : Value == rhs.Value ? 0 : 1;
        public int CompareTo(int rhs) => Value < rhs ? -1 : Value == rhs ? 0 : 1;

        public static implicit operator int(NamedValue nv) => nv.Value;
    }
}
