using System;
using System.Collections.Generic;
using System.Text;

namespace SF3
{
    /// <summary>
    /// Concrete type with both a Name (string) and a Value (int).
    /// Used when values need both names to look up and must be identifiable as a specific type.
    /// </summary>
    public class NamedValue : IComparable, IComparable<NamedValue>
    {
        public NamedValue(string name, int value)
        {
            _name = name;
            _value = value;
        }

        public override string ToString() => Name;

        public string Name => _name;
        public int Value => _value;

        private readonly string _name;
        private readonly int _value;

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
