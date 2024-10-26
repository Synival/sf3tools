using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.X019.Monster {
    public class Monster {
        private readonly IX019_FileEditor _fileEditor;

        private readonly int maxHP;
        private readonly int maxMP;
        private readonly int level;
        private readonly int attack;
        private readonly int defense;
        private readonly int agility;
        private readonly int mov;
        private readonly int luck;
        private readonly int turns;
        private readonly int hpRegen;
        private readonly int mpRegen;
        private readonly int earthRes;
        private readonly int fireRes;
        private readonly int iceRes;
        private readonly int sparkRes;
        private readonly int windRes;
        private readonly int lightRes;
        private readonly int darkRes;
        private readonly int unusedRes;
        private readonly int spell1;
        private readonly int spell1Level;
        private readonly int spell2;
        private readonly int spell2Level;
        private readonly int spell3;
        private readonly int spell3Level;
        private readonly int spell4;
        private readonly int spell4Level;
        private readonly int equippedWeapon;
        private readonly int equippedAccessory;
        private readonly int itemSlot1;
        private readonly int itemSlot2;
        private readonly int itemSlot3;
        private readonly int itemSlot4;
        private readonly int enemySpecial1;
        private readonly int enemySpecial2;
        private readonly int enemySpecial3;
        private readonly int enemySpecial4; //?
        private readonly int enemySpecial5; //?
        private readonly int enemySpecial6; //?
        private readonly int enemySpecial7; //?
        private readonly int enemySpecial8; //?
        private readonly int enemySpecial9; //?
        private readonly int enemySpecial10; //?
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int unknown3;
        private readonly int unknown4;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int gold;
        private readonly int drop;
        private readonly int unknown7;
        private readonly int unknown8;
        private readonly int unknown9;
        private readonly int unknown10;
        private readonly int magicType;
        private readonly int movementType;
        private readonly int unknown11;
        private readonly int unknown12;
        private readonly int unknown13;
        private readonly int unknown14;
        private readonly int unknown15;
        private readonly int unknown16;
        private readonly int unknown17;
        private readonly int unknown18;
        private readonly int unknown19;
        private readonly int unknown20;
        private readonly int offset;

        public Monster(IX019_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x0000000C; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x0000000C; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000eb0; //scn3
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = _fileEditor.IsX044 ? 0x00007e40 : 0x00000eb0; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 76);
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
            SpriteID = id + 200;

            Address = offset + (id * 0x4C);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int ID { get; }
        public string Name { get; }

        public int SpriteID { get; }

        public int MaxHP {
            get => _fileEditor.GetWord(maxHP);
            set => _fileEditor.SetWord(maxHP, value);
        }

        public int MaxMP {
            get => _fileEditor.GetByte(maxMP);
            set => _fileEditor.SetByte(maxMP, (byte) value);
        }
        public int Level {
            get => _fileEditor.GetByte(level);
            set => _fileEditor.SetByte(level, (byte) value);
        }
        public int Attack {
            get => _fileEditor.GetByte(attack);
            set => _fileEditor.SetByte(attack, (byte) value);
        }
        public int Defense {
            get => _fileEditor.GetByte(defense);
            set => _fileEditor.SetByte(defense, (byte) value);
        }
        public int Agility {
            get => _fileEditor.GetByte(agility);
            set => _fileEditor.SetByte(agility, (byte) value);
        }

        public int Mov {
            get => _fileEditor.GetByte(mov);
            set => _fileEditor.SetByte(mov, (byte) value);
        }

        public int Luck {
            get => _fileEditor.GetByte(luck);
            set => _fileEditor.SetByte(luck, (byte) value);
        }

        public int Turns {
            get => _fileEditor.GetByte(turns);
            set => _fileEditor.SetByte(turns, (byte) value);
        }

        public int HPRegen {
            get => _fileEditor.GetByte(hpRegen);
            set => _fileEditor.SetByte(hpRegen, (byte) value);
        }

        public int MPRegen {
            get => _fileEditor.GetByte(mpRegen);
            set => _fileEditor.SetByte(mpRegen, (byte) value);
        }

        public int EarthRes {
            get => _fileEditor.GetByte(earthRes);
            set => _fileEditor.SetByte(earthRes, (byte) value);
        }

        public int FireRes {
            get => _fileEditor.GetByte(fireRes);
            set => _fileEditor.SetByte(fireRes, (byte) value);
        }

        public int IceRes {
            get => _fileEditor.GetByte(iceRes);
            set => _fileEditor.SetByte(iceRes, (byte) value);
        }

        public int SparkRes {
            get => _fileEditor.GetByte(sparkRes);
            set => _fileEditor.SetByte(sparkRes, (byte) value);
        }

        public int WindRes {
            get => _fileEditor.GetByte(windRes);
            set => _fileEditor.SetByte(windRes, (byte) value);
        }

        public int LightRes {
            get => _fileEditor.GetByte(lightRes);
            set => _fileEditor.SetByte(lightRes, (byte) value);
        }

        public int DarkRes {
            get => _fileEditor.GetByte(darkRes);
            set => _fileEditor.SetByte(darkRes, (byte) value);
        }

        public int UnusedRes {
            get => _fileEditor.GetByte(unusedRes);
            set => _fileEditor.SetByte(unusedRes, (byte) value);
        }

        public SpellValue Spell1 {
            get => new SpellValue(Scenario, _fileEditor.GetByte(spell1));
            set => _fileEditor.SetByte(spell1, (byte) value.Value);
        }

        public int Spell1Level {
            get => _fileEditor.GetByte(spell1Level);
            set => _fileEditor.SetByte(spell1Level, (byte) value);
        }

        public SpellValue Spell2 {
            get => new SpellValue(Scenario, _fileEditor.GetByte(spell2));
            set => _fileEditor.SetByte(spell2, (byte) value.Value);
        }

        public int Spell2Level {
            get => _fileEditor.GetByte(spell2Level);
            set => _fileEditor.SetByte(spell2Level, (byte) value);
        }

        public SpellValue Spell3 {
            get => new SpellValue(Scenario, _fileEditor.GetByte(spell3));
            set => _fileEditor.SetByte(spell3, (byte) value.Value);
        }

        public int Spell3Level {
            get => _fileEditor.GetByte(spell3Level);
            set => _fileEditor.SetByte(spell3Level, (byte) value);
        }

        public SpellValue Spell4 {
            get => new SpellValue(Scenario, _fileEditor.GetByte(spell4));
            set => _fileEditor.SetByte(spell4, (byte) value.Value);
        }

        public int Spell4Level {
            get => _fileEditor.GetByte(spell4Level);
            set => _fileEditor.SetByte(spell4Level, (byte) value);
        }

        public ItemValue Weapon {
            get => new ItemValue(Scenario, _fileEditor.GetWord(equippedWeapon));
            set => _fileEditor.SetWord(equippedWeapon, value.Value);
        }

        public ItemValue Accessory {
            get => new ItemValue(Scenario, _fileEditor.GetWord(equippedAccessory));
            set => _fileEditor.SetWord(equippedAccessory, value.Value);
        }

        public ItemValue ItemSlot1 {
            get => new ItemValue(Scenario, _fileEditor.GetWord(itemSlot1));
            set => _fileEditor.SetWord(itemSlot1, value.Value);
        }

        public ItemValue ItemSlot2 {
            get => new ItemValue(Scenario, _fileEditor.GetWord(itemSlot2));
            set => _fileEditor.SetWord(itemSlot2, value.Value);
        }

        public ItemValue ItemSlot3 {
            get => new ItemValue(Scenario, _fileEditor.GetWord(itemSlot3));
            set => _fileEditor.SetWord(itemSlot3, value.Value);
        }

        public ItemValue ItemSlot4 {
            get => new ItemValue(Scenario, _fileEditor.GetWord(itemSlot4));
            set => _fileEditor.SetWord(itemSlot4, value.Value);
        }

        public SpecialValue Special1 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial1));
            set => _fileEditor.SetByte(enemySpecial1, (byte) value.Value);
        }

        public SpecialValue Special2 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial2));
            set => _fileEditor.SetByte(enemySpecial2, (byte) value.Value);
        }

        public SpecialValue Special3 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial3));
            set => _fileEditor.SetByte(enemySpecial3, (byte) value.Value);
        }

        public SpecialValue Special4 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial4));
            set => _fileEditor.SetByte(enemySpecial4, (byte) value.Value);
        }

        public SpecialValue Special5 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial5));
            set => _fileEditor.SetByte(enemySpecial5, (byte) value.Value);
        }

        public SpecialValue Special6 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial6));
            set => _fileEditor.SetByte(enemySpecial6, (byte) value.Value);
        }

        public SpecialValue Special7 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial7));
            set => _fileEditor.SetByte(enemySpecial7, (byte) value.Value);
        }

        public SpecialValue Special8 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial8));
            set => _fileEditor.SetByte(enemySpecial8, (byte) value.Value);
        }

        public SpecialValue Special9 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial9));
            set => _fileEditor.SetByte(enemySpecial9, (byte) value.Value);
        }

        public SpecialValue Special10 {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(enemySpecial10));
            set => _fileEditor.SetByte(enemySpecial10, (byte) value.Value);
        }

        public int Unknown1 {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte) value);
        }

        public int Unknown2 {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte) value);
        }

        public int Unknown3 {
            get => _fileEditor.GetByte(unknown3);
            set => _fileEditor.SetByte(unknown3, (byte) value);
        }

        public int Unknown4 {
            get => _fileEditor.GetByte(unknown4);
            set => _fileEditor.SetByte(unknown4, (byte) value);
        }

        public int Unknown5 {
            get => _fileEditor.GetByte(unknown5);
            set => _fileEditor.SetByte(unknown5, (byte) value);
        }

        public int Unknown6 {
            get => _fileEditor.GetByte(unknown6);
            set => _fileEditor.SetByte(unknown6, (byte) value);
        }

        public int Gold {
            get => _fileEditor.GetWord(gold);
            set => _fileEditor.SetWord(gold, value);
        }

        public ItemValue Drop {
            get => new ItemValue(Scenario, _fileEditor.GetWord(drop));
            set => _fileEditor.SetWord(drop, value.Value);
        }

        public int Unknown7 {
            get => _fileEditor.GetByte(unknown7);
            set => _fileEditor.SetByte(unknown7, (byte) value);
        }

        public int Unknown8 {
            get => _fileEditor.GetByte(unknown8);
            set => _fileEditor.SetByte(unknown8, (byte) value);
        }

        public int Unknown9 {
            get => _fileEditor.GetByte(unknown9);
            set => _fileEditor.SetByte(unknown9, (byte) value);
        }

        public int Unknown10 {
            get => _fileEditor.GetByte(unknown10);
            set => _fileEditor.SetByte(unknown10, (byte) value);
        }

        public int MagicType {
            get => _fileEditor.GetByte(magicType);
            set => _fileEditor.SetByte(magicType, (byte) value);
        }

        public MovementTypeValue MovementType {
            get => new MovementTypeValue(_fileEditor.GetByte(movementType));
            set => _fileEditor.SetByte(movementType, (byte) value.Value);
        }

        public int Unknown11 {
            get => _fileEditor.GetByte(unknown11);
            set => _fileEditor.SetByte(unknown11, (byte) value);
        }

        public int Unknown12 {
            get => _fileEditor.GetByte(unknown12);
            set => _fileEditor.SetByte(unknown12, (byte) value);
        }

        public int Unknown13 {
            get => _fileEditor.GetByte(unknown13);
            set => _fileEditor.SetByte(unknown13, (byte) value);
        }

        public int Unknown14 {
            get => _fileEditor.GetByte(unknown14);
            set => _fileEditor.SetByte(unknown14, (byte) value);
        }

        public int Unknown15 {
            get => _fileEditor.GetByte(unknown15);
            set => _fileEditor.SetByte(unknown15, (byte) value);
        }

        public int Unknown16 {
            get => _fileEditor.GetByte(unknown16);
            set => _fileEditor.SetByte(unknown16, (byte) value);
        }

        public int Unknown17 {
            get => _fileEditor.GetByte(unknown17);
            set => _fileEditor.SetByte(unknown17, (byte) value);
        }

        public int Unknown18 {
            get => _fileEditor.GetByte(unknown18);
            set => _fileEditor.SetByte(unknown18, (byte) value);
        }

        public int Unknown19 {
            get => _fileEditor.GetByte(unknown19);
            set => _fileEditor.SetByte(unknown19, (byte) value);
        }

        public int Unknown20 {
            get => _fileEditor.GetByte(unknown20);
            set => _fileEditor.SetByte(unknown20, (byte) value);
        }

        public int Address { get; }
    }
}
