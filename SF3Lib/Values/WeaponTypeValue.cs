using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value for WeaponType that can be bound to an ObjectListView.
    /// </summary>
    public class WeaponTypeValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        public static readonly Dictionary<int, string> ValueNames = GetValueNameDictionaryFromXML("Resources/WeaponTypes.xml");

        private static readonly Dictionary<NamedValue, string> _comboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new WeaponTypeValue(value));

        public WeaponTypeValue(int value) : base(NameOrHexValue(value, ValueNames), HexValueWithName(value, ValueNames), value)
        {
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues;
    }
}
