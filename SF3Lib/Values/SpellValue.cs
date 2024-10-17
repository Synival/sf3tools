using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value for Spell ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpellValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        // TODO: These definitely change between scenarios, so somehow it will need to be scenario-specific, likely tied to the SF3FileEditor.
        public static readonly Dictionary<int, string> ValueNames = GetValueNameDictionaryFromXML("Resources/S3/Spells.xml");

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpellValue(value));

        public SpellValue(int value) : base(HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
