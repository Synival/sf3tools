using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value for Sex that can be bound to an ObjectListView.
    /// </summary>
    public class SexValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        // TODO: Use resources file?
        public static readonly Dictionary<int, string> ValueNames = new Dictionary<int, string> {
            {0x01, "Male"},
            {0x02, "Female"},
        };

        private static readonly Dictionary<NamedValue, string> _ComboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SexValue(value));

        public override Dictionary<NamedValue, string> ComboBoxValues => _ComboBoxValues;

        public SexValue(int value) : base(HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
