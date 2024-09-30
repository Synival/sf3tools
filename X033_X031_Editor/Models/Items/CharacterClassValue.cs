using SF3.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SF3.X033_X031_Editor.Models.Items
{
    /// <summary>
    /// Named value for CharacterClass that can be bound to an ObjectListView.
    /// </summary>
    public class CharacterClassValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        // TODO: Use resources file?
        public static readonly Dictionary<int, string> ValueNames = new Dictionary<int, string> {
            {0x00, "Soldier (U)"},
            {0x01, "Knight (U)"},
            {0x02, "Warrior (U)"},
            {0x03, "Priest (U)"},
            {0x04, "Magician (U)"},
            {0x05, "Archer (U)"},
            {0x06, "Monk (U)"},
            {0x07, "Birdsoldier (U)"},
            {0x09, "Penguin (U)"},
            {0x20, "Swordsman (P)"},
            {0x21, "Cavalier (P)"},
            {0x22, "Gladiator (P)"},
            {0x23, "Cleric (P)"},
            {0x24, "Wizard (P)"},
            {0x25, "Marksman (P)"},
            {0x26, "Master Monk (P)"},
            {0x27, "Birdknight (P)"},
            {0x28, "Archer Knight (P)"},
            {0x29, "Emperor Penguin (P)"},
            {0x2A, "Steam Knight (P)"},
            {0x2B, "Werewolf (P)"},
            {0x2C, "Ninja (P)"},
        };

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = Utils.MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new CharacterClassValue(value));

        public CharacterClassValue(int value) : base(Utils.HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
