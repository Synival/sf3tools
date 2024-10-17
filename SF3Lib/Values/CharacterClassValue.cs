using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static SF3.Utils.Utils;

namespace SF3.Values
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
            {0x00, "Swordsman (U)"},
            {0x01, "Knight (U)"},
            {0x02, "Warrior (U)"},
            {0x03, "Priest (U)"},
            {0x04, "Magician (U)"},
            {0x05, "Archer (U)"},
            {0x06, "Monk (U)"},
            {0x07, "Birdsoldier (U)"},
            {0x08, "Ranger (U)"},
            {0x09, "Penguin (M) (U)"},
            {0x0A, "Steam Soldier (U)"},
            {0x0B, "Werewolf (U)"},
            {0x0C, "Ninja Tyro (U)"},
            {0x0D, "Kunoichi (U)"},
            {0x0E, "Elven Ranger (U)"},
            {0x0F, "Shaman (U)"},

            {0x10, "Mecha Bot (U)"},
            {0x11, "Metal Driver (U)"},
            {0x12, "Penguin (F) (U)"},
            {0x13, "Trainer (U)"},
            {0x14, "Child (U)"},
            {0x15, "Officer (U)"},
            {0x16, "Royal Youth (U)"},
            {0x17, "Dragon Child (U)"},
            {0x18, "Unicorn Filly (U)"},
            {0x19, "Dragon (U)"},
            {0x1A, "Lion Soldier (U)"},
            {0x1B, "Pegasus (U)"},
            {0x1C, "Penguin (S) (U)"},
            {0x1D, "Samurai Tyro (U)"},
            {0x1E, "Sprite (U)"},
            {0x1F, "Harridan (U)"},

            {0x20, "Hero (P1)"},
            {0x21, "Cavalier (P1)"},
            {0x22, "Gladiator (P1)"},
            {0x23, "Cleric (P1)"},
            {0x24, "Wizard (P1)"},
            {0x25, "Sniper (P1)"},
            {0x26, "Master Monk (P1)"},
            {0x27, "Birdknight (P1)"},
            {0x28, "Bow Knight (P1)"},
            {0x29, "Dynamo (P1)"},
            {0x2A, "Steam Knight (P1)"},
            {0x2B, "Wolf Baron (P1)"},
            {0x2C, "Ninja (P1)"},
            {0x2D, "Ninja (F) (P1)"},
            {0x2E, "Striker (P1)"},
            {0x2F, "Summoner (P1)"},

            {0x30, "Mecha Soldier (P1)"},
            {0x31, "Gunner (P1)"},
            {0x32, "Diva (P1)"},
            {0x33, "Beast Tamer (P1)"},
            {0x34, "Godchild (P1)"},
            {0x35, "Commander (P1)"},
            {0x36, "Princess (P1)"},
            {0x37, "Dragonman (P1)"},
            {0x38, "Unicorn (P1)"},
            {0x39, "White Dragon (P1)"},
            {0x3A, "Lion Warrior (P1)"},
            {0x3B, "Pegasus Knight (P1)"},
            {0x3C, "Scholar (P1)"},
            {0x3D, "Samurai (P1)"},
            {0x3E, "Fairy (P1)"},
            {0x3F, "Witch (P1)"},

            {0x40, "Ogre Rider (P1)"},
            {0x41, "Master (P1)"},
            {0x42, "Rider (P1)"},
            {0x43, "Monk (?) (P1)"},
            {0x44, "1-228 (P1)"},
            {0x45, "1-229 (P1)"},
            {0x46, "1-230 (P1)"},
            {0x47, "1-231 (P1)"},
            {0x48, "Champion (P2)"},
            {0x49, "Paladin (P2)"},
            {0x4A, "High Wizard (P2)"},
            {0x4B, "Bishop (P2)"},
            {0x4C, "Bow Master (P2)"},
            {0x4D, "Baron (P2)"},
            {0x4E, "Fist Master (P2)"},
            {0x4F, "Wing Lord (P2)"},

            {0x50, "Saint (P2)"},
            {0x51, "Buster Knight (P2)"},
            {0x52, "Emperor (P2)"},
            {0x53, "Steam Baron (P2)"},
            {0x54, "Berserker (P2)"},
            {0x55, "Master Ninja (P2)"},
            {0x56, "Commando (P2)"},
            {0x57, "Sorceress (P2)"},
            {0x58, "Innovator (P2)"},
            {0x59, "Mecha God (P2)"},
            {0x5A, "Magic Knight (P2)"},
            {0x5B, "Buster (P2)"},
            {0x5C, "Queen (P2)"},
            {0x5D, "Beast Master (P2)"},
            {0x5E, "General (P2)"},
            {0x5F, "Light Princess (P2)"},

            {0x60, "Dragon Master (P2)"},
            {0x61, "Unicorn Queen (P2)"},
            {0x62, "Sacred Dragon (P2)"},
            {0x63, "Lion King (P2)"},
            {0x64, "Pegasus Lord (P2)"},
            {0x65, "Professor (P2)"},
            {0x66, "Taisho (P2)"},
            {0x67, "Light Fairy (P2)"},
            {0x68, "Enchantress (P2)"},
            {0x69, "Mad Rider (P2)"},
            {0x6A, "Hermit (P2)"},
        };

        public static readonly Dictionary<NamedValue, string> ComboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new CharacterClassValue(value));

        public CharacterClassValue(int value) : base(HexValueWithName(value, ValueNames), value)
        {
        }
    }
}
