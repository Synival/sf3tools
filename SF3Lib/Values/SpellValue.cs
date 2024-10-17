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

        // TODO: Use resources file?
        // TODO: These definitely change between scenarios, so somehow it will need to be scenario-specific, likely tied to the SF3FileEditor.
        public static readonly Dictionary<int, string> ValueNames = new Dictionary<int, string> {
            {0x00, "-"},
            {0x01, "Blaze"},
            {0x02, "Freeze"},
            {0x03, "Spark"},
            {0x04, "Tornado"},
            {0x05, "Soul Steal"},
            {0x06, "Inferno"},
            {0x07, "Support"},
            {0x08, "Slow"},
            {0x09, "Shining"},
            {0x0A, "Resist"},
            {0x0B, "Attack"},
            {0x0C, "Dispel"},
            {0x0D, "Confuse"},
            {0x0E, "Sleep"},
            {0x0F, "Charm"},
            {0x10, "Drain"},
            {0x11, "Heal"},
            {0x12, "Aura"},
            {0x13, "Chant"},
            {0x14, "Antidote"},
            {0x15, "Egress"},
            {0x16, "*Heal"},
            {0x17, "*Restore"},
            {0x18, "*Cure"},
            {0x19, "*Egress"},
            {0x1A, "*Raise 1"},
            {0x1B, "*Raise 2"},
            {0x1C, "*Raise 3"},
            {0x1D, "*Level"},
            {0x1E, "Fire Breath"},
            {0x1F, "Blizzard Breath"},
            {0x20, "Thunder Breath"},
            {0x21, "Negative Flash"},
            {0x22, "Demon Breath"},
            {0x23, "Bubble Breath"},
            {0x24, "Acid Breath"},
            {0x25, "Chaos"},
            {0x26, "Phoenix"},
            {0x27, "Wendigo"},
            {0x28, "Tiamat"},
            {0x29, "Thanatos"},
            {0x2A, "Berserk"},
            {0x2B, "*Meat"},
            {0x2C, "*Orb"},
            {0x2D, "X-Laser"},
            {0x2E, "*Hammer"},
            {0x2F, "*Boomerang"},
            {0x30, "Panic"},
            {0x31, "Tremor"},
            {0x32, "Rockfall"},
            {0x33, "Blood Cane"},
            {0x34, "Thor"},
            {0x35, "Zephyrus"},
            {0x36, "Golem"},
            {0x37, "Hell Dragon"},
            {0x38, "Bubble Breath"},
            {0x39, "Black Breath"},
            {0x3A, "Captured by Whip"},
            {0x3B, "Fake Charm"},
            {0x3C, "Electric Discharge"},
            {0x3D, "Negative Bolt"},
            {0x3E, "Proserpina"},
            {0x3F, "Demon King"},
            {0x40, "Penko"},
            {0x41, "Penn"},
            {0x42, "Bombardment"},
            {0x43, "Wind Storm"},
            {0x44, "Keppyoudo"},
            {0x45, "Blizzard"},
            {0x46, "Blessing"},
            {0x47, "Chaos Gate"},
            {0x48, "Negative Flash"},
            {0x49, "Blessing"},
            {0x4A, "???"},
        };

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpellValue(value));

        public SpellValue(int value) : base(HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
