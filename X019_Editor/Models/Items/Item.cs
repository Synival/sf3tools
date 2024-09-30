using static SF3.X019_Editor.Forms.frmMain;

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

        public Item(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x0000000C; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x0000000C; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000eb0; //scn3
            }
            else if (Globals.scenario == 4)
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

        public int ID
        {
            get
            {
                return index;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public int SpriteID
        {
            get
            {
                return spriteID;
            }
        }

        public int MaxHP
        {
            get
            {
                return FileEditor.getWord(maxHP);
            }
            set
            {
                FileEditor.setWord(maxHP, value);
            }
        }

        public int MaxMP
        {
            get
            {
                return FileEditor.getByte(maxMP);
            }
            set
            {
                FileEditor.setByte(maxMP, (byte)value);
            }
        }
        public int Level
        {
            get
            {
                return FileEditor.getByte(level);
            }
            set
            {
                FileEditor.setByte(level, (byte)value);
            }
        }
        public int Attack
        {
            get
            {
                return FileEditor.getByte(attack);
            }
            set
            {
                FileEditor.setByte(attack, (byte)value);
            }
        }
        public int Defense
        {
            get
            {
                return FileEditor.getByte(defense);
            }
            set
            {
                FileEditor.setByte(defense, (byte)value);
            }
        }
        public int Agility
        {
            get
            {
                return FileEditor.getByte(agility);
            }
            set
            {
                FileEditor.setByte(agility, (byte)value);
            }
        }

        public int Mov
        {
            get
            {
                return FileEditor.getByte(mov);
            }
            set
            {
                FileEditor.setByte(mov, (byte)value);
            }
        }

        public int Luck
        {
            get
            {
                return FileEditor.getByte(luck);
            }
            set
            {
                FileEditor.setByte(luck, (byte)value);
            }
        }

        public int Turns
        {
            get
            {
                return FileEditor.getByte(turns);
            }
            set
            {
                FileEditor.setByte(turns, (byte)value);
            }
        }

        public int HPRegen
        {
            get
            {
                return FileEditor.getByte(hpRegen);
            }
            set
            {
                FileEditor.setByte(hpRegen, (byte)value);
            }
        }

        public int MPRegen
        {
            get
            {
                return FileEditor.getByte(mpRegen);
            }
            set
            {
                FileEditor.setByte(mpRegen, (byte)value);
            }
        }

        public int EarthRes
        {
            get
            {
                return FileEditor.getByte(earthRes);
            }
            set
            {
                FileEditor.setByte(earthRes, (byte)value);
            }
        }

        public int FireRes
        {
            get
            {
                return FileEditor.getByte(fireRes);
            }
            set
            {
                FileEditor.setByte(fireRes, (byte)value);
            }
        }

        public int IceRes
        {
            get
            {
                return FileEditor.getByte(iceRes);
            }
            set
            {
                FileEditor.setByte(iceRes, (byte)value);
            }
        }

        public int SparkRes
        {
            get
            {
                return FileEditor.getByte(sparkRes);
            }
            set
            {
                FileEditor.setByte(sparkRes, (byte)value);
            }
        }

        public int WindRes
        {
            get
            {
                return FileEditor.getByte(windRes);
            }
            set
            {
                FileEditor.setByte(windRes, (byte)value);
            }
        }

        public int LightRes
        {
            get
            {
                return FileEditor.getByte(lightRes);
            }
            set
            {
                FileEditor.setByte(lightRes, (byte)value);
            }
        }

        public int DarkRes
        {
            get
            {
                return FileEditor.getByte(darkRes);
            }
            set
            {
                FileEditor.setByte(darkRes, (byte)value);
            }
        }

        public int UnusedRes
        {
            get
            {
                return FileEditor.getByte(unusedRes);
            }
            set
            {
                FileEditor.setByte(unusedRes, (byte)value);
            }
        }

        public int Spell1
        {
            get
            {
                return FileEditor.getByte(spell1);
            }
            set
            {
                FileEditor.setByte(spell1, (byte)value);
            }
        }

        public int Spell1Level
        {
            get
            {
                return FileEditor.getByte(spell1Level);
            }
            set
            {
                FileEditor.setByte(spell1Level, (byte)value);
            }
        }

        public int Spell2
        {
            get
            {
                return FileEditor.getByte(spell2);
            }
            set
            {
                FileEditor.setByte(spell2, (byte)value);
            }
        }

        public int Spell2Level
        {
            get
            {
                return FileEditor.getByte(spell2Level);
            }
            set
            {
                FileEditor.setByte(spell2Level, (byte)value);
            }
        }

        public int Spell3
        {
            get
            {
                return FileEditor.getByte(spell3);
            }
            set
            {
                FileEditor.setByte(spell3, (byte)value);
            }
        }

        public int Spell3Level
        {
            get
            {
                return FileEditor.getByte(spell3Level);
            }
            set
            {
                FileEditor.setByte(spell3Level, (byte)value);
            }
        }

        public int Spell4
        {
            get
            {
                return FileEditor.getByte(spell4);
            }
            set
            {
                FileEditor.setByte(spell4, (byte)value);
            }
        }

        public int Spell4Level
        {
            get
            {
                return FileEditor.getByte(spell4Level);
            }
            set
            {
                FileEditor.setByte(spell4Level, (byte)value);
            }
        }

        public int Weapon
        {
            get
            {
                return FileEditor.getWord(equippedWeapon);
            }
            set
            {
                FileEditor.setWord(equippedWeapon, value);
            }
        }

        public int Accessory
        {
            get
            {
                return FileEditor.getWord(equippedAccessory);
            }
            set
            {
                FileEditor.setWord(equippedAccessory, value);
            }
        }

        public int ItemSlot1
        {
            get
            {
                return FileEditor.getWord(itemSlot1);
            }
            set
            {
                FileEditor.setWord(itemSlot1, value);
            }
        }

        public int ItemSlot2
        {
            get
            {
                return FileEditor.getWord(itemSlot2);
            }
            set
            {
                FileEditor.setWord(itemSlot2, value);
            }
        }

        public int ItemSlot3
        {
            get
            {
                return FileEditor.getWord(itemSlot3);
            }
            set
            {
                FileEditor.setWord(itemSlot3, value);
            }
        }

        public int ItemSlot4
        {
            get
            {
                return FileEditor.getWord(itemSlot4);
            }
            set
            {
                FileEditor.setWord(itemSlot4, value);
            }
        }

        public int Special1
        {
            get
            {
                return FileEditor.getByte(enemySpecial1);
            }
            set
            {
                FileEditor.setByte(enemySpecial1, (byte)value);
            }
        }

        public int Special2
        {
            get
            {
                return FileEditor.getByte(enemySpecial2);
            }
            set
            {
                FileEditor.setByte(enemySpecial2, (byte)value);
            }
        }
        public int Special3
        {
            get
            {
                return FileEditor.getByte(enemySpecial3);
            }
            set
            {
                FileEditor.setByte(enemySpecial3, (byte)value);
            }
        }
        public int Special4
        {
            get
            {
                return FileEditor.getByte(enemySpecial4);
            }
            set
            {
                FileEditor.setByte(enemySpecial4, (byte)value);
            }
        }
        public int Special5
        {
            get
            {
                return FileEditor.getByte(enemySpecial5);
            }
            set
            {
                FileEditor.setByte(enemySpecial5, (byte)value);
            }
        }
        public int Special6
        {
            get
            {
                return FileEditor.getByte(enemySpecial6);
            }
            set
            {
                FileEditor.setByte(enemySpecial6, (byte)value);
            }
        }
        public int Special7
        {
            get
            {
                return FileEditor.getByte(enemySpecial7);
            }
            set
            {
                FileEditor.setByte(enemySpecial7, (byte)value);
            }
        }
        public int Special8
        {
            get
            {
                return FileEditor.getByte(enemySpecial8);
            }
            set
            {
                FileEditor.setByte(enemySpecial8, (byte)value);
            }
        }
        public int Special9
        {
            get
            {
                return FileEditor.getByte(enemySpecial9);
            }
            set
            {
                FileEditor.setByte(enemySpecial9, (byte)value);
            }
        }
        public int Special10
        {
            get
            {
                return FileEditor.getByte(enemySpecial10);
            }
            set
            {
                FileEditor.setByte(enemySpecial10, (byte)value);
            }
        }

        public int Unknown1
        {
            get
            {
                return FileEditor.getByte(unknown1);
            }
            set
            {
                FileEditor.setByte(unknown1, (byte)value);
            }
        }

        public int Unknown2
        {
            get
            {
                return FileEditor.getByte(unknown2);
            }
            set
            {
                FileEditor.setByte(unknown2, (byte)value);
            }
        }
        public int Unknown3
        {
            get
            {
                return FileEditor.getByte(unknown3);
            }
            set
            {
                FileEditor.setByte(unknown3, (byte)value);
            }
        }
        public int Unknown4
        {
            get
            {
                return FileEditor.getByte(unknown4);
            }
            set
            {
                FileEditor.setByte(unknown4, (byte)value);
            }
        }
        public int Unknown5
        {
            get
            {
                return FileEditor.getByte(unknown5);
            }
            set
            {
                FileEditor.setByte(unknown5, (byte)value);
            }
        }
        public int Unknown6
        {
            get
            {
                return FileEditor.getByte(unknown6);
            }
            set
            {
                FileEditor.setByte(unknown6, (byte)value);
            }
        }

        public int Gold
        {
            get
            {
                return FileEditor.getWord(gold);
            }
            set
            {
                FileEditor.setWord(gold, value);
            }
        }

        public int Drop
        {
            get
            {
                return FileEditor.getWord(drop);
            }
            set
            {
                FileEditor.setWord(drop, value);
            }
        }

        public int Unknown7
        {
            get
            {
                return FileEditor.getByte(unknown7);
            }
            set
            {
                FileEditor.setByte(unknown7, (byte)value);
            }
        }
        public int Unknown8
        {
            get
            {
                return FileEditor.getByte(unknown8);
            }
            set
            {
                FileEditor.setByte(unknown8, (byte)value);
            }
        }
        public int Unknown9
        {
            get
            {
                return FileEditor.getByte(unknown9);
            }
            set
            {
                FileEditor.setByte(unknown9, (byte)value);
            }
        }
        public int Unknown10
        {
            get
            {
                return FileEditor.getByte(unknown10);
            }
            set
            {
                FileEditor.setByte(unknown10, (byte)value);
            }
        }

        public int MagicType
        {
            get
            {
                return FileEditor.getByte(magicType);
            }
            set
            {
                FileEditor.setByte(magicType, (byte)value);
            }
        }

        public int MovementType
        {
            get
            {
                return FileEditor.getByte(movementType);
            }
            set
            {
                FileEditor.setByte(movementType, (byte)value);
            }
        }

        public int Unknown11
        {
            get
            {
                return FileEditor.getByte(unknown11);
            }
            set
            {
                FileEditor.setByte(unknown11, (byte)value);
            }
        }

        public int Unknown12
        {
            get
            {
                return FileEditor.getByte(unknown12);
            }
            set
            {
                FileEditor.setByte(unknown12, (byte)value);
            }
        }

        public int Unknown13
        {
            get
            {
                return FileEditor.getByte(unknown13);
            }
            set
            {
                FileEditor.setByte(unknown13, (byte)value);
            }
        }

        public int Unknown14
        {
            get
            {
                return FileEditor.getByte(unknown14);
            }
            set
            {
                FileEditor.setByte(unknown14, (byte)value);
            }
        }

        public int Unknown15
        {
            get
            {
                return FileEditor.getByte(unknown15);
            }
            set
            {
                FileEditor.setByte(unknown15, (byte)value);
            }
        }

        public int Unknown16
        {
            get
            {
                return FileEditor.getByte(unknown16);
            }
            set
            {
                FileEditor.setByte(unknown16, (byte)value);
            }
        }

        public int Unknown17
        {
            get
            {
                return FileEditor.getByte(unknown17);
            }
            set
            {
                FileEditor.setByte(unknown17, (byte)value);
            }
        }

        public int Unknown18
        {
            get
            {
                return FileEditor.getByte(unknown18);
            }
            set
            {
                FileEditor.setByte(unknown18, (byte)value);
            }
        }

        public int Unknown19
        {
            get
            {
                return FileEditor.getByte(unknown19);
            }
            set
            {
                FileEditor.setByte(unknown19, (byte)value);
            }
        }

        public int Unknown20
        {
            get
            {
                return FileEditor.getByte(unknown20);
            }
            set
            {
                FileEditor.setByte(unknown20, (byte)value);
            }
        }

        public int Address
        {
            get
            {
                return (address);
            }
        }

    }
}
