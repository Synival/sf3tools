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
        public static readonly Dictionary<int, string> ValueNamesS1 = GetValueNameDictionaryFromXML("Resources/S1/Spells.xml");
        public static readonly Dictionary<int, string> ValueNamesS2 = GetValueNameDictionaryFromXML("Resources/S2/Spells.xml");
        public static readonly Dictionary<int, string> ValueNamesS3 = GetValueNameDictionaryFromXML("Resources/S3/Spells.xml");
        public static readonly Dictionary<int, string> ValueNamesPD = GetValueNameDictionaryFromXML("Resources/PD/Spells.xml");

        private static readonly Dictionary<NamedValue, string> _ComboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpellValue(value));

        public override Dictionary<NamedValue, string> ComboBoxValues => _ComboBoxValues;

        // TODO: determine which scenario to use!
        public SpellValue(int value) : base(HexValueWithName(value, ValueNamesS3), value)
        {
        }
    }
}
