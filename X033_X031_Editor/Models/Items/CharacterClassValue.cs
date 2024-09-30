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
            {0x08, "Archer Knight (U)"},
            {0x09, "Penguin (U)"},
            {0x0D, "Kunoichi (U)"},
            {0x0E, "Ranger (U)"},
            {0x0F, "Shaman (U)"},
            {0x12, "Penkichi (U)"},
            {0x20, "Swordsman (P)"},
            {0x21, "Cavalier (P)"},
            {0x22, "Gladiator (P)"},
            {0x23, "Cleric (P)"},
            {0x24, "Wizard (P)"},
            {0x25, "Sniper (P)"},
            {0x26, "Master Monk (P)"},
            {0x27, "Birdknight (P)"},
            {0x28, "Bow Knight (P)"},
            {0x29, "Emperor Penguin (P)"},
            {0x2A, "Steam Knight (P)"},
            {0x2B, "Werewolf (P)"},
            {0x2C, "Ninja (P)"},
            {0x2D, "Ninja (Female) (P)"},
            {0x2E, "Commando (P)"},
            {0x2F, "Summoner (P)"},
            {0x30, "Robot (P)"},
            {0x31, "Metal Gunner (P)"},
            {0x32, "Samba Penguin (P)"},
            {0x33, "Tamer (P)"},
            {0x34, "Holy Child (P)"},
            {0x35, "Commander (P)"},
            {0x36, "Princess (P)"},
            {0x37, "Dragonman (P)"},
            {0x38, "Unicorn (P)"},
            {0x39, "Dragon (P)"},
            {0x3A, "Gladiator (Leon) (P)"},
            {0x3B, "Pegasus Knight (P)"},
            {0x3C, "Scholar Penguin (P)"},
            {0x3D, "Samurai (P)"},
            {0x3E, "Fairy (P)"},
            {0x3F, "Witch (P)"},
            {0x40, "Ogre Rider (P)"},
            {0x41, "Master (P)"},
        };

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = Utils.MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new CharacterClassValue(value));

        public CharacterClassValue(int value) : base(Utils.HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
