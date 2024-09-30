using SF3.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SF3.Editor.Values
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

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = Utils.MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SexValue(value));

        public SexValue(int value) : base(Utils.HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
