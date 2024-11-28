using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;
using SF3.Types;

namespace SF3.Models.Structs.X019 {
    public class Monster : Struct {
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

        public Monster(IRawData data, int id, string name, int address)
        : base(data, id, name, address, 0x4C) {
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

        public int SpriteID { get; }

        [BulkCopy]
        public int MaxHP {
            get => Data.GetWord(maxHP);
            set => Data.SetWord(maxHP, value);
        }

        [BulkCopy]
        public int MaxMP {
            get => Data.GetByte(maxMP);
            set => Data.SetByte(maxMP, (byte) value);
        }

        [BulkCopy]
        public int Level {
            get => Data.GetByte(level);
            set => Data.SetByte(level, (byte) value);
        }

        [BulkCopy]
        public int Attack {
            get => Data.GetByte(attack);
            set => Data.SetByte(attack, (byte) value);
        }

        [BulkCopy]
        public int Defense {
            get => Data.GetByte(defense);
            set => Data.SetByte(defense, (byte) value);
        }

        [BulkCopy]
        public int Agility {
            get => Data.GetByte(agility);
            set => Data.SetByte(agility, (byte) value);
        }

        [BulkCopy]
        public int Mov {
            get => Data.GetByte(mov);
            set => Data.SetByte(mov, (byte) value);
        }

        [BulkCopy]
        public int Luck {
            get => Data.GetByte(luck);
            set => Data.SetByte(luck, (byte) value);
        }

        [BulkCopy]
        public int Turns {
            get => Data.GetByte(turns);
            set => Data.SetByte(turns, (byte) value);
        }

        [BulkCopy]
        public int HPRegen {
            get => Data.GetByte(hpRegen);
            set => Data.SetByte(hpRegen, (byte) value);
        }

        [BulkCopy]
        public int MPRegen {
            get => Data.GetByte(mpRegen);
            set => Data.SetByte(mpRegen, (byte) value);
        }

        [BulkCopy]
        public int EarthRes {
            get => (sbyte) Data.GetByte(earthRes);
            set => Data.SetByte(earthRes, (byte) value);
        }

        [BulkCopy]
        public int FireRes {
            get => (sbyte) Data.GetByte(fireRes);
            set => Data.SetByte(fireRes, (byte) value);
        }

        [BulkCopy]
        public int IceRes {
            get => (sbyte) Data.GetByte(iceRes);
            set => Data.SetByte(iceRes, (byte) value);
        }

        [BulkCopy]
        public int SparkRes {
            get => (sbyte) Data.GetByte(sparkRes);
            set => Data.SetByte(sparkRes, (byte) value);
        }

        [BulkCopy]
        public int WindRes {
            get => (sbyte) Data.GetByte(windRes);
            set => Data.SetByte(windRes, (byte) value);
        }

        [BulkCopy]
        public int LightRes {
            get => (sbyte) Data.GetByte(lightRes);
            set => Data.SetByte(lightRes, (byte) value);
        }

        [BulkCopy]
        public int DarkRes {
            get => (sbyte) Data.GetByte(darkRes);
            set => Data.SetByte(darkRes, (byte) value);
        }

        [BulkCopy]
        public int UnusedRes {
            get => (sbyte) Data.GetByte(unusedRes);
            set => Data.SetByte(unusedRes, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell1 {
            get => Data.GetByte(spell1);
            set => Data.SetByte(spell1, (byte) value);
        }

        [BulkCopy]
        public int Spell1Level {
            get => Data.GetByte(spell1Level);
            set => Data.SetByte(spell1Level, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell2 {
            get => Data.GetByte(spell2);
            set => Data.SetByte(spell2, (byte) value);
        }

        [BulkCopy]
        public int Spell2Level {
            get => Data.GetByte(spell2Level);
            set => Data.SetByte(spell2Level, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell3 {
            get => Data.GetByte(spell3);
            set => Data.SetByte(spell3, (byte) value);
        }

        [BulkCopy]
        public int Spell3Level {
            get => Data.GetByte(spell3Level);
            set => Data.SetByte(spell3Level, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell4 {
            get => Data.GetByte(spell4);
            set => Data.SetByte(spell4, (byte) value);
        }

        [BulkCopy]
        public int Spell4Level {
            get => Data.GetByte(spell4Level);
            set => Data.SetByte(spell4Level, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Weapon {
            get => Data.GetWord(equippedWeapon);
            set => Data.SetWord(equippedWeapon, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Accessory {
            get => Data.GetWord(equippedAccessory);
            set => Data.SetWord(equippedAccessory, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot1 {
            get => Data.GetWord(itemSlot1);
            set => Data.SetWord(itemSlot1, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot2 {
            get => Data.GetWord(itemSlot2);
            set => Data.SetWord(itemSlot2, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot3 {
            get => Data.GetWord(itemSlot3);
            set => Data.SetWord(itemSlot3, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot4 {
            get => Data.GetWord(itemSlot4);
            set => Data.SetWord(itemSlot4, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special1 {
            get => Data.GetByte(enemySpecial1);
            set => Data.SetByte(enemySpecial1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special2 {
            get => Data.GetByte(enemySpecial2);
            set => Data.SetByte(enemySpecial2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special3 {
            get => Data.GetByte(enemySpecial3);
            set => Data.SetByte(enemySpecial3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special4 {
            get => Data.GetByte(enemySpecial4);
            set => Data.SetByte(enemySpecial4, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special5 {
            get => Data.GetByte(enemySpecial5);
            set => Data.SetByte(enemySpecial5, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special6 {
            get => Data.GetByte(enemySpecial6);
            set => Data.SetByte(enemySpecial6, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special7 {
            get => Data.GetByte(enemySpecial7);
            set => Data.SetByte(enemySpecial7, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special8 {
            get => Data.GetByte(enemySpecial8);
            set => Data.SetByte(enemySpecial8, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special9 {
            get => Data.GetByte(enemySpecial9);
            set => Data.SetByte(enemySpecial9, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special10 {
            get => Data.GetByte(enemySpecial10);
            set => Data.SetByte(enemySpecial10, (byte) value);
        }

        [BulkCopy]
        public int Unknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int Unknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int Unknown3 {
            get => Data.GetByte(unknown3);
            set => Data.SetByte(unknown3, (byte) value);
        }

        public bool CantSeeStatus {
            get => Data.GetBit(protections, 4);
            set => Data.SetBit(protections, 4, value);
        }

        [BulkCopy]
        public int Protections {
            get => Data.GetByte(protections);
            set => Data.SetByte(protections, (byte) value);
        }

        [BulkCopy]
        public int ExpIs5 {
            get => Data.GetByte(expIs5);
            set => Data.SetByte(expIs5, (byte) value);
        }

        [BulkCopy]
        public int Unknown6 {
            get => Data.GetByte(unknown6);
            set => Data.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int Gold {
            get => Data.GetWord(gold);
            set => Data.SetWord(gold, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Drop {
            get => Data.GetWord(drop);
            set => Data.SetWord(drop, value);
        }

        [BulkCopy]
        public int Unknown7 {
            get => Data.GetByte(unknown7);
            set => Data.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Droprate)]
        public int Droprate {
            get => Data.GetByte(droprate);
            set => Data.SetByte(droprate, (byte) value);
        }

        [BulkCopy]
        public int SlowPlus {
            get => Data.GetByte(slowPlus);
            set => Data.SetByte(slowPlus, (byte) value);
        }

        [BulkCopy]
        public int SupportPlus {
            get => Data.GetByte(supportPlus);
            set => Data.SetByte(supportPlus, (byte) value);
        }

        [BulkCopy]
        public int MagicType {
            get => Data.GetByte(magicType);
            set => Data.SetByte(magicType, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public int MovementType {
            get => Data.GetByte(movementType);
            set => Data.SetByte(movementType, (byte) value);
        }

        [BulkCopy]
        public int Unknown11 {
            get => Data.GetByte(unknown11);
            set => Data.SetByte(unknown11, (byte) value);
        }

        [BulkCopy]
        public int Unknown12 {
            get => Data.GetByte(unknown12);
            set => Data.SetByte(unknown12, (byte) value);
        }

        [BulkCopy]
        public int Unknown13 {
            get => Data.GetByte(unknown13);
            set => Data.SetByte(unknown13, (byte) value);
        }

        [BulkCopy]
        public int Unknown14 {
            get => Data.GetByte(unknown14);
            set => Data.SetByte(unknown14, (byte) value);
        }

        [BulkCopy]
        public int Unknown15 {
            get => Data.GetByte(unknown15);
            set => Data.SetByte(unknown15, (byte) value);
        }

        [BulkCopy]
        public int Unknown16 {
            get => Data.GetByte(unknown16);
            set => Data.SetByte(unknown16, (byte) value);
        }

        [BulkCopy]
        public int Unknown17 {
            get => Data.GetByte(unknown17);
            set => Data.SetByte(unknown17, (byte) value);
        }

        [BulkCopy]
        public int Unknown18 {
            get => Data.GetByte(unknown18);
            set => Data.SetByte(unknown18, (byte) value);
        }

        [BulkCopy]
        public int Unknown19 {
            get => Data.GetByte(unknown19);
            set => Data.SetByte(unknown19, (byte) value);
        }

        [BulkCopy]
        public int Unknown20 {
            get => Data.GetByte(unknown20);
            set => Data.SetByte(unknown20, (byte) value);
        }
    }
}
