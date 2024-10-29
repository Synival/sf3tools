using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models {
    public class Monster : IModel {
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
        private readonly int protections;
        private readonly int expIs5;
        private readonly int unknown6;
        private readonly int gold;
        private readonly int drop;
        private readonly int unknown7;
        private readonly int droprate;
        private readonly int slowPlus;
        private readonly int supportPlus;
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

        public Monster(IByteEditor editor, int id, string name, int address, ScenarioType scenario, bool isX044) {
            Editor   = editor;
            ID       = id;
            Name     = name;
            Address  = address;
            Size     = 0x4C;

            Scenario = scenario;

            maxHP             = Address; // 2 bytes
            maxMP             = Address + 2;
            level             = Address + 3;
            attack            = Address + 4;
            defense           = Address + 5;
            agility           = Address + 6;
            mov               = Address + 7;
            luck              = Address + 8;
            turns             = Address + 9;
            hpRegen           = Address + 10;
            mpRegen           = Address + 11;
            earthRes          = Address + 12;
            fireRes           = Address + 13;
            iceRes            = Address + 14;
            sparkRes          = Address + 15;
            windRes           = Address + 16;
            lightRes          = Address + 17;
            darkRes           = Address + 18;
            unusedRes         = Address + 19;
            spell1            = Address + 20;
            spell1Level       = Address + 21;
            spell2            = Address + 22;
            spell2Level       = Address + 23;
            spell3            = Address + 24;
            spell3Level       = Address + 25;
            spell4            = Address + 26;
            spell4Level       = Address + 27;
            equippedWeapon    = Address + 28; // 2 byte
            equippedAccessory = Address + 30; // 2 byte
            itemSlot1         = Address + 32; // 2 byte
            itemSlot2         = Address + 34; // 2 byte
            itemSlot3         = Address + 36; // 2 byte
            itemSlot4         = Address + 38; // 2 byte
            enemySpecial1     = Address + 40;
            enemySpecial2     = Address + 41;
            enemySpecial3     = Address + 42;
            enemySpecial4     = Address + 43; // ?
            enemySpecial5     = Address + 44; // ?
            enemySpecial6     = Address + 45; // ?
            enemySpecial7     = Address + 46; // ?
            enemySpecial8     = Address + 47; // ?
            enemySpecial9     = Address + 48; // ?
            enemySpecial10    = Address + 49; // ?
            unknown1          = Address + 50;
            unknown2          = Address + 51;
            unknown3          = Address + 52;
            protections       = Address + 53; // protections? 8 = no crit? 0a = damage immunity?
            expIs5            = Address + 54; // exp = 5
            unknown6          = Address + 55;
            gold              = Address + 56; // 2 byte
            drop              = Address + 58; // 2 byte
            unknown7          = Address + 60;
            droprate          = Address + 61; // droprate/drops items when attacked. Set E for thief rules
            slowPlus          = Address + 62; // slow bonus?
            supportPlus       = Address + 63; // support bonus?
            magicType         = Address + 64;
            movementType      = Address + 65;
            unknown11         = Address + 66; // heal when damaged when set?
            unknown12         = Address + 67;
            unknown13         = Address + 68; // what to do on turn1?. 0 = atk. 1 = spell. 4 = use weapon?
            unknown14         = Address + 69; // what to do on turn2?
            unknown15         = Address + 70; // what to do on turn3?
            unknown16         = Address + 71; // what to do on turn4?
            unknown17         = Address + 72; // what to do on turn5?
            unknown18         = Address + 73; // what to do on turn6?
            unknown19         = Address + 74;
            unknown20         = Address + 75;
            SpriteID          = id + 200;
        }

        private string GetItemName(int value) => ValueNames.GetItemName(Scenario, value);

        public IByteEditor Editor { get; }
        public ScenarioType Scenario { get; }
        public int ID { get; }
        public string Name { get; }
        public int Address { get; }
        public int Size { get; }

        public int SpriteID { get; }

        public int MaxHP {
            get => Editor.GetWord(maxHP);
            set => Editor.SetWord(maxHP, value);
        }

        public int MaxMP {
            get => Editor.GetByte(maxMP);
            set => Editor.SetByte(maxMP, (byte) value);
        }

        public int Level {
            get => Editor.GetByte(level);
            set => Editor.SetByte(level, (byte) value);
        }

        public int Attack {
            get => Editor.GetByte(attack);
            set => Editor.SetByte(attack, (byte) value);
        }

        public int Defense {
            get => Editor.GetByte(defense);
            set => Editor.SetByte(defense, (byte) value);
        }
        public int Agility {
            get => Editor.GetByte(agility);
            set => Editor.SetByte(agility, (byte) value);
        }

        public int Mov {
            get => Editor.GetByte(mov);
            set => Editor.SetByte(mov, (byte) value);
        }

        public int Luck {
            get => Editor.GetByte(luck);
            set => Editor.SetByte(luck, (byte) value);
        }

        public int Turns {
            get => Editor.GetByte(turns);
            set => Editor.SetByte(turns, (byte) value);
        }

        public int HPRegen {
            get => Editor.GetByte(hpRegen);
            set => Editor.SetByte(hpRegen, (byte) value);
        }

        public int MPRegen {
            get => Editor.GetByte(mpRegen);
            set => Editor.SetByte(mpRegen, (byte) value);
        }

        public int EarthRes {
            get => Editor.GetByte(earthRes);
            set => Editor.SetByte(earthRes, (byte) value);
        }

        public int FireRes {
            get => Editor.GetByte(fireRes);
            set => Editor.SetByte(fireRes, (byte) value);
        }

        public int IceRes {
            get => Editor.GetByte(iceRes);
            set => Editor.SetByte(iceRes, (byte) value);
        }

        public int SparkRes {
            get => Editor.GetByte(sparkRes);
            set => Editor.SetByte(sparkRes, (byte) value);
        }

        public int WindRes {
            get => Editor.GetByte(windRes);
            set => Editor.SetByte(windRes, (byte) value);
        }

        public int LightRes {
            get => Editor.GetByte(lightRes);
            set => Editor.SetByte(lightRes, (byte) value);
        }

        public int DarkRes {
            get => Editor.GetByte(darkRes);
            set => Editor.SetByte(darkRes, (byte) value);
        }

        public int UnusedRes {
            get => Editor.GetByte(unusedRes);
            set => Editor.SetByte(unusedRes, (byte) value);
        }

        public SpellValue Spell1 {
            get => new SpellValue(Scenario, Editor.GetByte(spell1));
            set => Editor.SetByte(spell1, (byte) value);
        }

        public int Spell1Level {
            get => Editor.GetByte(spell1Level);
            set => Editor.SetByte(spell1Level, (byte) value);
        }

        public SpellValue Spell2 {
            get => new SpellValue(Scenario, Editor.GetByte(spell2));
            set => Editor.SetByte(spell2, (byte) value);
        }

        public int Spell2Level {
            get => Editor.GetByte(spell2Level);
            set => Editor.SetByte(spell2Level, (byte) value);
        }

        public SpellValue Spell3 {
            get => new SpellValue(Scenario, Editor.GetByte(spell3));
            set => Editor.SetByte(spell3, (byte) value);
        }

        public int Spell3Level {
            get => Editor.GetByte(spell3Level);
            set => Editor.SetByte(spell3Level, (byte) value);
        }

        public SpellValue Spell4 {
            get => new SpellValue(Scenario, Editor.GetByte(spell4));
            set => Editor.SetByte(spell4, (byte) value);
        }

        public int Spell4Level {
            get => Editor.GetByte(spell4Level);
            set => Editor.SetByte(spell4Level, (byte) value);
        }

        [NameGetter(nameof(GetItemName))]
        public int Weapon {
            get => Editor.GetWord(equippedWeapon);
            set => Editor.SetWord(equippedWeapon, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int Accessory {
            get => Editor.GetWord(equippedAccessory);
            set => Editor.SetWord(equippedAccessory, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int ItemSlot1 {
            get => Editor.GetWord(itemSlot1);
            set => Editor.SetWord(itemSlot1, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int ItemSlot2 {
            get => Editor.GetWord(itemSlot2);
            set => Editor.SetWord(itemSlot2, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int ItemSlot3 {
            get => Editor.GetWord(itemSlot3);
            set => Editor.SetWord(itemSlot3, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int ItemSlot4 {
            get => Editor.GetWord(itemSlot4);
            set => Editor.SetWord(itemSlot4, value);
        }

        public SpecialValue Special1 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial1));
            set => Editor.SetByte(enemySpecial1, (byte) value);
        }

        public SpecialValue Special2 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial2));
            set => Editor.SetByte(enemySpecial2, (byte) value);
        }

        public SpecialValue Special3 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial3));
            set => Editor.SetByte(enemySpecial3, (byte) value);
        }

        public SpecialValue Special4 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial4));
            set => Editor.SetByte(enemySpecial4, (byte) value);
        }

        public SpecialValue Special5 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial5));
            set => Editor.SetByte(enemySpecial5, (byte) value);
        }

        public SpecialValue Special6 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial6));
            set => Editor.SetByte(enemySpecial6, (byte) value);
        }

        public SpecialValue Special7 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial7));
            set => Editor.SetByte(enemySpecial7, (byte) value);
        }

        public SpecialValue Special8 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial8));
            set => Editor.SetByte(enemySpecial8, (byte) value);
        }

        public SpecialValue Special9 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial9));
            set => Editor.SetByte(enemySpecial9, (byte) value);
        }

        public SpecialValue Special10 {
            get => new SpecialValue(Scenario, Editor.GetByte(enemySpecial10));
            set => Editor.SetByte(enemySpecial10, (byte) value);
        }

        public int Unknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        public int Unknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        public int Unknown3 {
            get => Editor.GetByte(unknown3);
            set => Editor.SetByte(unknown3, (byte) value);
        }

        public bool CantSeeStatus {
            get => Editor.GetBit(protections, 4);
            set => Editor.SetBit(protections, 4, value);
        }

        public int Protections {
            get => Editor.GetByte(protections);
            set => Editor.SetByte(protections, (byte) value);
        }

        public int ExpIs5 {
            get => Editor.GetByte(expIs5);
            set => Editor.SetByte(expIs5, (byte) value);
        }

        public int Unknown6 {
            get => Editor.GetByte(unknown6);
            set => Editor.SetByte(unknown6, (byte) value);
        }

        public int Gold {
            get => Editor.GetWord(gold);
            set => Editor.SetWord(gold, value);
        }

        [NameGetter(nameof(GetItemName))]
        public int Drop {
            get => Editor.GetWord(drop);
            set => Editor.SetWord(drop, value);
        }

        public int Unknown7 {
            get => Editor.GetByte(unknown7);
            set => Editor.SetByte(unknown7, (byte) value);
        }

        public DroprateValue Droprate {
            get => new DroprateValue(Editor.GetByte(droprate));
            set => Editor.SetByte(droprate, (byte) value);
        }

        public int SlowPlus {
            get => Editor.GetByte(slowPlus);
            set => Editor.SetByte(slowPlus, (byte) value);
        }

        public int SupportPlus {
            get => Editor.GetByte(supportPlus);
            set => Editor.SetByte(supportPlus, (byte) value);
        }

        public int MagicType {
            get => Editor.GetByte(magicType);
            set => Editor.SetByte(magicType, (byte) value);
        }

        public MovementTypeValue MovementType {
            get => new MovementTypeValue(Editor.GetByte(movementType));
            set => Editor.SetByte(movementType, (byte) value);
        }

        public int Unknown11 {
            get => Editor.GetByte(unknown11);
            set => Editor.SetByte(unknown11, (byte) value);
        }

        public int Unknown12 {
            get => Editor.GetByte(unknown12);
            set => Editor.SetByte(unknown12, (byte) value);
        }

        public int Unknown13 {
            get => Editor.GetByte(unknown13);
            set => Editor.SetByte(unknown13, (byte) value);
        }

        public int Unknown14 {
            get => Editor.GetByte(unknown14);
            set => Editor.SetByte(unknown14, (byte) value);
        }

        public int Unknown15 {
            get => Editor.GetByte(unknown15);
            set => Editor.SetByte(unknown15, (byte) value);
        }

        public int Unknown16 {
            get => Editor.GetByte(unknown16);
            set => Editor.SetByte(unknown16, (byte) value);
        }

        public int Unknown17 {
            get => Editor.GetByte(unknown17);
            set => Editor.SetByte(unknown17, (byte) value);
        }

        public int Unknown18 {
            get => Editor.GetByte(unknown18);
            set => Editor.SetByte(unknown18, (byte) value);
        }

        public int Unknown19 {
            get => Editor.GetByte(unknown19);
            set => Editor.SetByte(unknown19, (byte) value);
        }

        public int Unknown20 {
            get => Editor.GetByte(unknown20);
            set => Editor.SetByte(unknown20, (byte) value);
        }
    }
}
