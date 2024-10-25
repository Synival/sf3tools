using System;
using System.Collections.Generic;

namespace SF3 {
    /// <summary>
    /// Concrete type with both a Name (string) and a Value (int).
    /// Used when values need both names to look up and must be identifiable as a specific type.
    /// </summary>
    public abstract class NamedValue : IComparable, IComparable<NamedValue> {
        public NamedValue(string name, string valueName, int value) {
            _name = name;
            _valueName = valueName;
            _value = value;
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

        private readonly string _name;
        private readonly string _valueName;
        private readonly int _value;

        /// <summary>
        /// The name of the value.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Combination of value + name.
        /// </summary>
        public string ValueName => _valueName;

        /// <summary>
        /// Value represented.
        /// </summary>
        public int Value => _value;

        static public bool operator <(NamedValue lhs, NamedValue rhs) => lhs.Value < rhs.Value;
        static public bool operator >(NamedValue lhs, NamedValue rhs) => lhs.Value > rhs.Value;
        static public bool operator ==(NamedValue lhs, NamedValue rhs) => lhs.Value == rhs.Value;
        static public bool operator !=(NamedValue lhs, NamedValue rhs) => lhs.Value != rhs.Value;
        public override bool Equals(object rhs) => (rhs is NamedValue) ? this.Value == (rhs as NamedValue).Value : base.Equals(rhs);
        public override int GetHashCode() => this.Value;

        public int CompareTo(object rhs) => (rhs is NamedValue) ? CompareTo(rhs as NamedValue) : throw new NotImplementedException();
        public int CompareTo(NamedValue rhs) => (this.Value < rhs.Value) ? -1 : (this.Value == rhs.Value) ? 0 : 1;
    }
}
