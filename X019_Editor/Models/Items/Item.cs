using SF3.Editor;
using SF3.Types;

namespace SF3.X019_Editor.Models.Items
{
    public class Item
    {
        private int maxHP;
        private int maxMP;
        private int level;
        private int attack;
        private int defense;
        private int agility;
        private int mov;
        private int luck;
        private int turns;
        private int hpRegen;
        private int mpRegen;
        private int earthRes;
        private int fireRes;
        private int iceRes;
        private int sparkRes;
        private int windRes;
        private int lightRes;
        private int darkRes;
        private int unusedRes;
        private int spell1;
        private int spell1Level;
        private int spell2;
        private int spell2Level;
        private int spell3;
        private int spell3Level;
        private int spell4;
        private int spell4Level;
        private int equippedWeapon;
        private int equippedAccessory;
        private int itemSlot1;
        private int itemSlot2;
        private int itemSlot3;
        private int itemSlot4;
        private int enemySpecial1;
        private int enemySpecial2;
        private int enemySpecial3;
        private int enemySpecial4; //?
        private int enemySpecial5; //?
        private int enemySpecial6; //?
        private int enemySpecial7; //?
        private int enemySpecial8; //?
        private int enemySpecial9; //?
        private int enemySpecial10; //?
        private int unknown1;
        private int unknown2;
        private int unknown3;
        private int unknown4;
        private int unknown5;
        private int unknown6;
        private int gold;
        private int drop;
        private int unknown7;
        private int unknown8;
        private int unknown9;
        private int unknown10;
        private int magicType;
        private int movementType;
        private int unknown11;
        private int unknown12;
        private int unknown13;
        private int unknown14;
        private int unknown15;
        private int unknown16;
        private int unknown17;
        private int unknown18;
        private int unknown19;
        private int unknown20;
        private int spriteID;

        private int address;
        private int offset;

        private int index;
        private string name;

        public Item(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x0000000C; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000000C; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000eb0; //scn3
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000eb0; //pd
            }
            else
                offset = 0x00000eb0; //X044 pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 76);
            maxHP = start; //2 bytes
            maxMP = start + 2; //1 byte
            level = start + 3; //1 byte
            attack = start + 4; //1 byte
            defense = start + 5;
            agility = start + 6;
            mov = start + 7;
            luck = start + 8;
            turns = start + 9;
            hpRegen = start + 10;
            mpRegen = start + 11;
            earthRes = start + 12;
            fireRes = start + 13;
            iceRes = start + 14;
            sparkRes = start + 15;
            windRes = start + 16;
            lightRes = start + 17;
            darkRes = start + 18;
            unusedRes = start + 19;
            spell1 = start + 20;
            spell1Level = start + 21;
            spell2 = start + 22;
            spell2Level = start + 23;
            spell3 = start + 24;
            spell3Level = start + 25;
            spell4 = start + 26;
            spell4Level = start + 27;
            equippedWeapon = start + 28; //2 byte
            equippedAccessory = start + 30; //2 byte
            itemSlot1 = start + 32; //2 byte
            itemSlot2 = start + 34; //2 byte
            itemSlot3 = start + 36; //2 byte
            itemSlot4 = start + 38; //2 byte
            enemySpecial1 = start + 40;
            enemySpecial2 = start + 41;
            enemySpecial3 = start + 42;
            enemySpecial4 = start + 43; //?
            enemySpecial5 = start + 44; //?
            enemySpecial6 = start + 45; //?
            enemySpecial7 = start + 46; //?
            enemySpecial8 = start + 47; //?
            enemySpecial9 = start + 48; //?
            enemySpecial10 = start + 49; //?
            unknown1 = start + 50;
            unknown2 = start + 51;
            unknown3 = start + 52;
            unknown4 = start + 53; //protections? 8 = no crit? 0a = damage immunity?
            unknown5 = start + 54; //exp = 5
            unknown6 = start + 55;
            gold = start + 56; //2 byte
            drop = start + 58; //2 byte
            unknown7 = start + 60;
            unknown8 = start + 61; //droprate/drops items when attacked. Set E for thief rules
            unknown9 = start + 62; //slow bonus?
            unknown10 = start + 63; //support bonus?
            magicType = start + 64;
            movementType = start + 65;
            unknown11 = start + 66; //heal when damaged when set?
            unknown12 = start + 67;
            unknown13 = start + 68; //what to do on turn1?. 0 = atk. 1 = spell. 4 = use weapon?
            unknown14 = start + 69; //what to do on turn2?
            unknown15 = start + 70; //what to do on turn3?
            unknown16 = start + 71; //what to do on turn4?
            unknown17 = start + 72; //what to do on turn5?
            unknown18 = start + 73; //what to do on turn6?
            unknown19 = start + 74;
            unknown20 = start + 75;
            spriteID = id + 200;

            address = offset + (id * 0x4C);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int ID => index;
        public string Name => name;

        public int SpriteID => spriteID;

        public int MaxHP
        {
            get => FileEditor.GetWord(maxHP);
            set => FileEditor.SetWord(maxHP, value);
        }

        public int MaxMP
        {
            get => FileEditor.GetByte(maxMP);
            set => FileEditor.SetByte(maxMP, (byte)value);
        }
        public int Level
        {
            get => FileEditor.GetByte(level);
            set => FileEditor.SetByte(level, (byte)value);
        }
        public int Attack
        {
            get => FileEditor.GetByte(attack);
            set => FileEditor.SetByte(attack, (byte)value);
        }
        public int Defense
        {
            get => FileEditor.GetByte(defense);
            set => FileEditor.SetByte(defense, (byte)value);
        }
        public int Agility
        {
            get => FileEditor.GetByte(agility);
            set => FileEditor.SetByte(agility, (byte)value);
        }

        public int Mov
        {
            get => FileEditor.GetByte(mov);
            set => FileEditor.SetByte(mov, (byte)value);
        }

        public int Luck
        {
            get => FileEditor.GetByte(luck);
            set => FileEditor.SetByte(luck, (byte)value);
        }

        public int Turns
        {
            get => FileEditor.GetByte(turns);
            set => FileEditor.SetByte(turns, (byte)value);
        }

        public int HPRegen
        {
            get => FileEditor.GetByte(hpRegen);
            set => FileEditor.SetByte(hpRegen, (byte)value);
        }

        public int MPRegen
        {
            get => FileEditor.GetByte(mpRegen);
            set => FileEditor.SetByte(mpRegen, (byte)value);
        }

        public int EarthRes
        {
            get => FileEditor.GetByte(earthRes);
            set => FileEditor.SetByte(earthRes, (byte)value);
        }

        public int FireRes
        {
            get => FileEditor.GetByte(fireRes);
            set => FileEditor.SetByte(fireRes, (byte)value);
        }

        public int IceRes
        {
            get => FileEditor.GetByte(iceRes);
            set => FileEditor.SetByte(iceRes, (byte)value);
        }

        public int SparkRes
        {
            get => FileEditor.GetByte(sparkRes);
            set => FileEditor.SetByte(sparkRes, (byte)value);
        }

        public int WindRes
        {
            get => FileEditor.GetByte(windRes);
            set => FileEditor.SetByte(windRes, (byte)value);
        }

        public int LightRes
        {
            get => FileEditor.GetByte(lightRes);
            set => FileEditor.SetByte(lightRes, (byte)value);
        }

        public int DarkRes
        {
            get => FileEditor.GetByte(darkRes);
            set => FileEditor.SetByte(darkRes, (byte)value);
        }

        public int UnusedRes
        {
            get => FileEditor.GetByte(unusedRes);
            set => FileEditor.SetByte(unusedRes, (byte)value);
        }

        public int Spell1
        {
            get => FileEditor.GetByte(spell1);
            set => FileEditor.SetByte(spell1, (byte)value);
        }

        public int Spell1Level
        {
            get => FileEditor.GetByte(spell1Level);
            set => FileEditor.SetByte(spell1Level, (byte)value);
        }

        public int Spell2
        {
            get => FileEditor.GetByte(spell2);
            set => FileEditor.SetByte(spell2, (byte)value);
        }

        public int Spell2Level
        {
            get => FileEditor.GetByte(spell2Level);
            set => FileEditor.SetByte(spell2Level, (byte)value);
        }

        public int Spell3
        {
            get => FileEditor.GetByte(spell3);
            set => FileEditor.SetByte(spell3, (byte)value);
        }

        public int Spell3Level
        {
            get => FileEditor.GetByte(spell3Level);
            set => FileEditor.SetByte(spell3Level, (byte)value);
        }

        public int Spell4
        {
            get => FileEditor.GetByte(spell4);
            set => FileEditor.SetByte(spell4, (byte)value);
        }

        public int Spell4Level
        {
            get => FileEditor.GetByte(spell4Level);
            set => FileEditor.SetByte(spell4Level, (byte)value);
        }

        public int Weapon
        {
            get => FileEditor.GetWord(equippedWeapon);
            set => FileEditor.SetWord(equippedWeapon, value);
        }

        public int Accessory
        {
            get => FileEditor.GetWord(equippedAccessory);
            set => FileEditor.SetWord(equippedAccessory, value);
        }

        public int ItemSlot1
        {
            get => FileEditor.GetWord(itemSlot1);
            set => FileEditor.SetWord(itemSlot1, value);
        }

        public int ItemSlot2
        {
            get => FileEditor.GetWord(itemSlot2);
            set => FileEditor.SetWord(itemSlot2, value);
        }

        public int ItemSlot3
        {
            get => FileEditor.GetWord(itemSlot3);
            set => FileEditor.SetWord(itemSlot3, value);
        }

        public int ItemSlot4
        {
            get => FileEditor.GetWord(itemSlot4);
            set => FileEditor.SetWord(itemSlot4, value);
        }

        public int Special1
        {
            get => FileEditor.GetByte(enemySpecial1);
            set => FileEditor.SetByte(enemySpecial1, (byte)value);
        }

        public int Special2
        {
            get => FileEditor.GetByte(enemySpecial2);
            set => FileEditor.SetByte(enemySpecial2, (byte)value);
        }
        public int Special3
        {
            get => FileEditor.GetByte(enemySpecial3);
            set => FileEditor.SetByte(enemySpecial3, (byte)value);
        }
        public int Special4
        {
            get => FileEditor.GetByte(enemySpecial4);
            set => FileEditor.SetByte(enemySpecial4, (byte)value);
        }
        public int Special5
        {
            get => FileEditor.GetByte(enemySpecial5);
            set => FileEditor.SetByte(enemySpecial5, (byte)value);
        }
        public int Special6
        {
            get => FileEditor.GetByte(enemySpecial6);
            set => FileEditor.SetByte(enemySpecial6, (byte)value);
        }
        public int Special7
        {
            get => FileEditor.GetByte(enemySpecial7);
            set => FileEditor.SetByte(enemySpecial7, (byte)value);
        }
        public int Special8
        {
            get => FileEditor.GetByte(enemySpecial8);
            set => FileEditor.SetByte(enemySpecial8, (byte)value);
        }
        public int Special9
        {
            get => FileEditor.GetByte(enemySpecial9);
            set => FileEditor.SetByte(enemySpecial9, (byte)value);
        }
        public int Special10
        {
            get => FileEditor.GetByte(enemySpecial10);
            set => FileEditor.SetByte(enemySpecial10, (byte)value);
        }

        public int Unknown1
        {
            get => FileEditor.GetByte(unknown1);
            set => FileEditor.SetByte(unknown1, (byte)value);
        }

        public int Unknown2
        {
            get => FileEditor.GetByte(unknown2);
            set => FileEditor.SetByte(unknown2, (byte)value);
        }
        public int Unknown3
        {
            get => FileEditor.GetByte(unknown3);
            set => FileEditor.SetByte(unknown3, (byte)value);
        }
        public int Unknown4
        {
            get => FileEditor.GetByte(unknown4);
            set => FileEditor.SetByte(unknown4, (byte)value);
        }
        public int Unknown5
        {
            get => FileEditor.GetByte(unknown5);
            set => FileEditor.SetByte(unknown5, (byte)value);
        }
        public int Unknown6
        {
            get => FileEditor.GetByte(unknown6);
            set => FileEditor.SetByte(unknown6, (byte)value);
        }

        public int Gold
        {
            get => FileEditor.GetWord(gold);
            set => FileEditor.SetWord(gold, value);
        }

        public int Drop
        {
            get => FileEditor.GetWord(drop);
            set => FileEditor.SetWord(drop, value);
        }

        public int Unknown7
        {
            get => FileEditor.GetByte(unknown7);
            set => FileEditor.SetByte(unknown7, (byte)value);
        }
        public int Unknown8
        {
            get => FileEditor.GetByte(unknown8);
            set => FileEditor.SetByte(unknown8, (byte)value);
        }
        public int Unknown9
        {
            get => FileEditor.GetByte(unknown9);
            set => FileEditor.SetByte(unknown9, (byte)value);
        }
        public int Unknown10
        {
            get => FileEditor.GetByte(unknown10);
            set => FileEditor.SetByte(unknown10, (byte)value);
        }

        public int MagicType
        {
            get => FileEditor.GetByte(magicType);
            set => FileEditor.SetByte(magicType, (byte)value);
        }

        public int MovementType
        {
            get => FileEditor.GetByte(movementType);
            set => FileEditor.SetByte(movementType, (byte)value);
        }

        public int Unknown11
        {
            get => FileEditor.GetByte(unknown11);
            set => FileEditor.SetByte(unknown11, (byte)value);
        }

        public int Unknown12
        {
            get => FileEditor.GetByte(unknown12);
            set => FileEditor.SetByte(unknown12, (byte)value);
        }

        public int Unknown13
        {
            get => FileEditor.GetByte(unknown13);
            set => FileEditor.SetByte(unknown13, (byte)value);
        }

        public int Unknown14
        {
            get => FileEditor.GetByte(unknown14);
            set => FileEditor.SetByte(unknown14, (byte)value);
        }

        public int Unknown15
        {
            get => FileEditor.GetByte(unknown15);
            set => FileEditor.SetByte(unknown15, (byte)value);
        }

        public int Unknown16
        {
            get => FileEditor.GetByte(unknown16);
            set => FileEditor.SetByte(unknown16, (byte)value);
        }

        public int Unknown17
        {
            get => FileEditor.GetByte(unknown17);
            set => FileEditor.SetByte(unknown17, (byte)value);
        }

        public int Unknown18
        {
            get => FileEditor.GetByte(unknown18);
            set => FileEditor.SetByte(unknown18, (byte)value);
        }

        public int Unknown19
        {
            get => FileEditor.GetByte(unknown19);
            set => FileEditor.SetByte(unknown19, (byte)value);
        }

        public int Unknown20
        {
            get => FileEditor.GetByte(unknown20);
            set => FileEditor.SetByte(unknown20, (byte)value);
        }

        public int Address => (address);
    }
}
