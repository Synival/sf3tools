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

        // TODO: Use resources file?
        public static readonly Dictionary<int, string> ValueNames = new Dictionary<int, string> {
            {0x00, "-"},
            {0x01, "Item"},
            {0x0A, "Dagger"},
            {0x0B, "Sword"},
            {0x0C, "Rapier"},
            {0x0D, "Blade"},
            {0x0E, "Katana"},
            {0x0F, "Wing"},
            {0x14, "Lance"},
            {0x15, "Spear"},
            {0x16, "Halberd"},
            {0x1E, "Axe"},
            {0x1F, "Tomahawk"},
            {0x20, "Mace"},
            {0x21, "Anchor"},
            {0x32, "Claw"},
            {0x33, "Glove"},
            {0x34, "Knuckle"},
            {0x3C, "Rod"},
            {0x3D, "Wand"},
            {0x3E, "Ankh"},
            {0x3F, "Elbesem Staff"},
            {0x46, "Bow"},
            {0x47, "Crossbow"},
            {0x48, "Shell"},
            {0x4A, "Shuriken"},
            {0x50, "Beak"},
            {0x51, "Horn"},
            {0x52, "Talon"},
            {0x5A, "Whip"},
            {0x7A, "Bracer"},
            {0x7B, "Circlet/Tiara"},
            {0x7C, "Ninja Bowl"},
            {0x81, "Ring/Accessory"},
        };

        private static readonly Dictionary<NamedValue, string> _comboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new WeaponTypeValue(value));

        public WeaponTypeValue(int value) : base(NameOrHexValue(value, ValueNames), HexValueWithName(value, ValueNames), value)
        {
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues;
    }
}
