using System;
using CommonLib.Attributes;
using CommonLib.Statistics;
using SF3.Models.Structs;
using SF3.ByteData;
using SF3.Statistics;
using SF3.Types;

namespace SF3.Models.Structs.X033_X031 {
    public class Stats : Struct {
        /// <summary>
        /// When enabled, GetAverageStatGrowthPerLevelAsPercent() will show the "growthValue" in its output
        /// </summary>
        public static bool DebugGrowthValues { get; set; } = false;

        public enum PromotionLevelType {
            Unpromoted = 0,
            Promotion1 = 1,
            Promotion2 = 2,
        }

        //starting stat table
        private readonly int character;
        private readonly int characterClass;
        private readonly int hpPromote;
        private readonly int hpCurve1;
        private readonly int hpCurve5;
        private readonly int hpCurve10;
        private readonly int hpCurve12_15;
        private readonly int hpCurve14_20;
        private readonly int hpCurve17_30;
        private readonly int hpCurve30_99;
        private readonly int mpPromote;
        private readonly int mpCurve1;
        private readonly int mpCurve5;
        private readonly int mpCurve10;
        private readonly int mpCurve12_15;
        private readonly int mpCurve14_20;
        private readonly int mpCurve17_30;
        private readonly int mpCurve30_99;
        private readonly int atkPromote;
        private readonly int atkCurve1;
        private readonly int atkCurve5;
        private readonly int atkCurve10;
        private readonly int atkCurve12_15;
        private readonly int atkCurve14_20;
        private readonly int atkCurve17_30;
        private readonly int atkCurve30_99;
        private readonly int defPromote;
        private readonly int defCurve1;
        private readonly int defCurve5;
        private readonly int defCurve10;
        private readonly int defCurve12_15;
        private readonly int defCurve14_20;
        private readonly int defCurve17_30;
        private readonly int defCurve30_99;
        private readonly int agiPromote;
        private readonly int agiCurve1;
        private readonly int agiCurve5;
        private readonly int agiCurve10;
        private readonly int agiCurve12_15;
        private readonly int agiCurve14_20;
        private readonly int agiCurve17_30;
        private readonly int agiCurve30_99;

        private readonly int s1LearnedAt;
        private readonly int s1LearnedID;
        private readonly int s1LearnedLevel;
        private readonly int s2LearnedAt;
        private readonly int s2LearnedID;
        private readonly int s2LearnedLevel;
        private readonly int s3LearnedAt;
        private readonly int s3LearnedID;
        private readonly int s3LearnedLevel;
        private readonly int s4LearnedAt;
        private readonly int s4LearnedID;
        private readonly int s4LearnedLevel;
        private readonly int s5LearnedAt;
        private readonly int s5LearnedID;
        private readonly int s5LearnedLevel;
        private readonly int s6LearnedAt;
        private readonly int s6LearnedID;
        private readonly int s6LearnedLevel;
        private readonly int s7LearnedAt;
        private readonly int s7LearnedID;
        private readonly int s7LearnedLevel;
        private readonly int s8LearnedAt;
        private readonly int s8LearnedID;
        private readonly int s8LearnedLevel;
        private readonly int s9LearnedAt;
        private readonly int s9LearnedID;
        private readonly int s9LearnedLevel;
        private readonly int s10LearnedAt;
        private readonly int s10LearnedID;
        private readonly int s10LearnedLevel;
        private readonly int s11LearnedAt;
        private readonly int s11LearnedID;
        private readonly int s11LearnedLevel;
        private readonly int s12LearnedAt;
        private readonly int s12LearnedID;
        private readonly int s12LearnedLevel;

        private readonly int weapon1Special1;
        private readonly int weapon1Special2;
        private readonly int weapon1Special3;
        private readonly int weapon2Special1;
        private readonly int weapon2Special2;
        private readonly int weapon2Special3;
        private readonly int weapon3Special1;
        private readonly int weapon3Special2;
        private readonly int weapon3Special3;
        private readonly int weapon4Special1;
        private readonly int weapon4Special2;
        private readonly int weapon4Special3;
        private readonly int baseLuck;
        private readonly int baseMov;

        private readonly int baseTurns;
        private readonly int baseHPRegen;
        private readonly int baseMPRegen;

        private readonly int earthRes;
        private readonly int fireRes;
        private readonly int iceRes;
        private readonly int sparkRes;
        private readonly int windRes;
        private readonly int lightRes;
        private readonly int darkRes;
        private readonly int unknownRes;
        private readonly int slow;
        private readonly int support;

        private readonly int magicBonus;
        private readonly int movementType;

        private readonly int weaponEquipable1;
        private readonly int weaponEquipable2;
        private readonly int weaponEquipable3;
        private readonly int weaponEquipable4;
        private readonly int accessoryEquipable1;
        private readonly int accessoryEquipable2;
        private readonly int accessoryEquipable3;
        private readonly int accessoryEquipable4;

        public Stats(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x7B) {
            character           = Address + 0x00;
            characterClass      = Address + 0x01;
            hpPromote           = Address + 0x02;
            hpCurve1            = Address + 0x03;
            hpCurve5            = Address + 0x04;
            hpCurve10           = Address + 0x05;
            hpCurve12_15        = Address + 0x06;
            hpCurve14_20        = Address + 0x07;
            hpCurve17_30        = Address + 0x08;
            hpCurve30_99        = Address + 0x09;
            mpPromote           = Address + 0x0A;
            mpCurve1            = Address + 0x0B;
            mpCurve5            = Address + 0x0C;
            mpCurve10           = Address + 0x0D;
            mpCurve12_15        = Address + 0x0E;
            mpCurve14_20        = Address + 0x0F;
            mpCurve17_30        = Address + 0x10;
            mpCurve30_99        = Address + 0x11;
            atkPromote          = Address + 0x12;
            atkCurve1           = Address + 0x13;
            atkCurve5           = Address + 0x14;
            atkCurve10          = Address + 0x15;
            atkCurve12_15       = Address + 0x16;
            atkCurve14_20       = Address + 0x17;
            atkCurve17_30       = Address + 0x18;
            atkCurve30_99       = Address + 0x19;
            defPromote          = Address + 0x1a;
            defCurve1           = Address + 0x1b;
            defCurve5           = Address + 0x1c;
            defCurve10          = Address + 0x1d;
            defCurve12_15       = Address + 0x1e;
            defCurve14_20       = Address + 0x1f;
            defCurve17_30       = Address + 0x20;
            defCurve30_99       = Address + 0x21;
            agiPromote          = Address + 0x22;
            agiCurve1           = Address + 0x23;
            agiCurve5           = Address + 0x24;
            agiCurve10          = Address + 0x25;
            agiCurve12_15       = Address + 0x26;
            agiCurve14_20       = Address + 0x27;
            agiCurve17_30       = Address + 0x28;
            agiCurve30_99       = Address + 0x29;

            s1LearnedAt         = Address + 0x2a;
            s1LearnedID         = Address + 0x2b;
            s1LearnedLevel      = Address + 0x2c;
            s2LearnedAt         = Address + 0x2d;
            s2LearnedID         = Address + 0x2e;
            s2LearnedLevel      = Address + 0x2f;
            s3LearnedAt         = Address + 0x30;
            s3LearnedID         = Address + 0x31;
            s3LearnedLevel      = Address + 0x32;
            s4LearnedAt         = Address + 0x33;
            s4LearnedID         = Address + 0x34;
            s4LearnedLevel      = Address + 0x35;
            s5LearnedAt         = Address + 0x36;
            s5LearnedID         = Address + 0x37;
            s5LearnedLevel      = Address + 0x38;
            s6LearnedAt         = Address + 0x39;
            s6LearnedID         = Address + 0x3a;
            s6LearnedLevel      = Address + 0x3b;
            s7LearnedAt         = Address + 0x3c;
            s7LearnedID         = Address + 0x3d;
            s7LearnedLevel      = Address + 0x3e;
            s8LearnedAt         = Address + 0x3f;
            s8LearnedID         = Address + 0x40;
            s8LearnedLevel      = Address + 0x41;
            s9LearnedAt         = Address + 0x42;
            s9LearnedID         = Address + 0x43;
            s9LearnedLevel      = Address + 0x44;
            s10LearnedAt        = Address + 0x45;
            s10LearnedID        = Address + 0x46;
            s10LearnedLevel     = Address + 0x47;
            s11LearnedAt        = Address + 0x48;
            s11LearnedID        = Address + 0x49;
            s11LearnedLevel     = Address + 0x4a;
            s12LearnedAt        = Address + 0x4b;
            s12LearnedID        = Address + 0x4c;
            s12LearnedLevel     = Address + 0x4d;

            weapon1Special1     = Address + 0x4e;
            weapon1Special2     = Address + 0x4f;
            weapon1Special3     = Address + 0x50;
            weapon2Special1     = Address + 0x51;
            weapon2Special2     = Address + 0x52;
            weapon2Special3     = Address + 0x53;
            weapon3Special1     = Address + 0x54;
            weapon3Special2     = Address + 0x55;
            weapon3Special3     = Address + 0x56;
            weapon4Special1     = Address + 0x57;
            weapon4Special2     = Address + 0x58;
            weapon4Special3     = Address + 0x59;

            baseLuck            = Address + 0x5a;
            baseMov             = Address + 0x5b;
            baseTurns           = Address + 0x5c;
            baseHPRegen         = Address + 0x5d;
            baseMPRegen         = Address + 0x5e;

            earthRes            = Address + 0x5f;
            fireRes             = Address + 0x60;
            iceRes              = Address + 0x61;
            sparkRes            = Address + 0x62;
            windRes             = Address + 0x63;
            lightRes            = Address + 0x64;
            darkRes             = Address + 0x65;
            unknownRes          = Address + 0x66;
            slow                = Address + 0x67;
            support             = Address + 0x68;
            magicBonus          = Address + 0x69;
            movementType        = Address + 0x6a;

            weaponEquipable1    = Address + 0x73;
            weaponEquipable2    = Address + 0x74;
            weaponEquipable3    = Address + 0x75;
            weaponEquipable4    = Address + 0x76;

            accessoryEquipable1 = Address + 0x77;
            accessoryEquipable2 = Address + 0x78;
            accessoryEquipable3 = Address + 0x79;
            accessoryEquipable4 = Address + 0x7a;
        }

        public bool IsPromoted => Data.GetByte(characterClass) >= 0x20;

        public PromotionLevelType PromotionLevel {
            get {
                var chClass = Data.GetByte(characterClass);
                return chClass < 0x20 ? PromotionLevelType.Unpromoted :
                       chClass < 0x48 ? PromotionLevelType.Promotion1 :
                                        PromotionLevelType.Promotion2;
            }
        }

        public ValueRange<int> GetStatGrowthRange(StatType stat, int groupIndex) {
            switch (stat) {
                case StatType.HP:
                    switch (groupIndex) {
                        case 0:
                            return new ValueRange<int>(HPCurve1, HPCurve5);
                        case 1:
                            return new ValueRange<int>(HPCurve5, HPCurve10);
                        case 2:
                            return new ValueRange<int>(HPCurve10, HPCurve12_15);
                        case 3:
                            return new ValueRange<int>(HPCurve12_15, HPCurve14_20);
                        case 4:
                            return new ValueRange<int>(HPCurve14_20, HPCurve17_30);
                        case 5:
                            return new ValueRange<int>(HPCurve17_30, HPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.MP:
                    switch (groupIndex) {
                        case 0:
                            return new ValueRange<int>(MPCurve1, MPCurve5);
                        case 1:
                            return new ValueRange<int>(MPCurve5, MPCurve10);
                        case 2:
                            return new ValueRange<int>(MPCurve10, MPCurve12_15);
                        case 3:
                            return new ValueRange<int>(MPCurve12_15, MPCurve14_20);
                        case 4:
                            return new ValueRange<int>(MPCurve14_20, MPCurve17_30);
                        case 5:
                            return new ValueRange<int>(MPCurve17_30, MPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Atk:
                    switch (groupIndex) {
                        case 0:
                            return new ValueRange<int>(AtkCurve1, AtkCurve5);
                        case 1:
                            return new ValueRange<int>(AtkCurve5, AtkCurve10);
                        case 2:
                            return new ValueRange<int>(AtkCurve10, AtkCurve12_15);
                        case 3:
                            return new ValueRange<int>(AtkCurve12_15, AtkCurve14_20);
                        case 4:
                            return new ValueRange<int>(AtkCurve14_20, AtkCurve17_30);
                        case 5:
                            return new ValueRange<int>(AtkCurve17_30, AtkCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Def:
                    switch (groupIndex) {
                        case 0:
                            return new ValueRange<int>(DefCurve1, DefCurve5);
                        case 1:
                            return new ValueRange<int>(DefCurve5, DefCurve10);
                        case 2:
                            return new ValueRange<int>(DefCurve10, DefCurve12_15);
                        case 3:
                            return new ValueRange<int>(DefCurve12_15, DefCurve14_20);
                        case 4:
                            return new ValueRange<int>(DefCurve14_20, DefCurve17_30);
                        case 5:
                            return new ValueRange<int>(DefCurve17_30, DefCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Agi:
                    switch (groupIndex) {
                        case 0:
                            return new ValueRange<int>(AgiCurve1, AgiCurve5);
                        case 1:
                            return new ValueRange<int>(AgiCurve5, AgiCurve10);
                        case 2:
                            return new ValueRange<int>(AgiCurve10, AgiCurve12_15);
                        case 3:
                            return new ValueRange<int>(AgiCurve12_15, AgiCurve14_20);
                        case 4:
                            return new ValueRange<int>(AgiCurve14_20, AgiCurve17_30);
                        case 5:
                            return new ValueRange<int>(AgiCurve17_30, AgiCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double GetAverageStatGrowthPerLevel(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[IsPromoted][groupIndex].Range.Range);
            return GrowthStats.GetAverageStatGrowthPerLevel(growthValue);
        }

        public string GetAverageStatGrowthPerLevelAsPercent(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[IsPromoted][groupIndex].Range.Range);
            return (DebugGrowthValues ? string.Format("{0:x}", growthValue) + " || " : "") +
                    GrowthStats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        [BulkCopy]
        public int CharacterID {
            get => Data.GetByte(character);
            set => Data.SetByte(character, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.CharacterClass)]
        public int CharacterClass {
            get => Data.GetByte(characterClass);
            set => Data.SetByte(characterClass, (byte) value);
        }

        [BulkCopy]
        public int HPPromote {
            get => Data.GetByte(hpPromote);
            set => Data.SetByte(hpPromote, (byte) value);
        }

        [BulkCopy]
        public int HPCurve1 {
            get => Data.GetByte(hpCurve1);
            set => Data.SetByte(hpCurve1, (byte) value);
        }

        [BulkCopy]
        public int HPCurve5 {
            get => Data.GetByte(hpCurve5);
            set => Data.SetByte(hpCurve5, (byte) value);
        }

        [BulkCopy]
        public int HPCurve10 {
            get => Data.GetByte(hpCurve10);
            set => Data.SetByte(hpCurve10, (byte) value);
        }

        [BulkCopy]
        public int HPCurve12_15 {
            get => Data.GetByte(hpCurve12_15);
            set => Data.SetByte(hpCurve12_15, (byte) value);
        }

        [BulkCopy]
        public int HPCurve14_20 {
            get => Data.GetByte(hpCurve14_20);
            set => Data.SetByte(hpCurve14_20, (byte) value);
        }

        [BulkCopy]
        public int HPCurve17_30 {
            get => Data.GetByte(hpCurve17_30);
            set => Data.SetByte(hpCurve17_30, (byte) value);
        }

        [BulkCopy]
        public int HPCurve30_99 {
            get => Data.GetByte(hpCurve30_99);
            set => Data.SetByte(hpCurve30_99, (byte) value);
        }

        public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [BulkCopy]
        public int MPPromote {
            get => Data.GetByte(mpPromote);
            set => Data.SetByte(mpPromote, (byte) value);
        }

        [BulkCopy]
        public int MPCurve1 {
            get => Data.GetByte(mpCurve1);
            set => Data.SetByte(mpCurve1, (byte) value);
        }

        [BulkCopy]
        public int MPCurve5 {
            get => Data.GetByte(mpCurve5);
            set => Data.SetByte(mpCurve5, (byte) value);
        }

        [BulkCopy]
        public int MPCurve10 {
            get => Data.GetByte(mpCurve10);
            set => Data.SetByte(mpCurve10, (byte) value);
        }

        [BulkCopy]
        public int MPCurve12_15 {
            get => Data.GetByte(mpCurve12_15);
            set => Data.SetByte(mpCurve12_15, (byte) value);
        }

        [BulkCopy]
        public int MPCurve14_20 {
            get => Data.GetByte(mpCurve14_20);
            set => Data.SetByte(mpCurve14_20, (byte) value);
        }

        [BulkCopy]
        public int MPCurve17_30 {
            get => Data.GetByte(mpCurve17_30);
            set => Data.SetByte(mpCurve17_30, (byte) value);
        }

        [BulkCopy]
        public int MPCurve30_99 {
            get => Data.GetByte(mpCurve30_99);
            set => Data.SetByte(mpCurve30_99, (byte) value);
        }

        public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [BulkCopy]
        public int AtkPromote {
            get => Data.GetByte(atkPromote);
            set => Data.SetByte(atkPromote, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve1 {
            get => Data.GetByte(atkCurve1);
            set => Data.SetByte(atkCurve1, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve5 {
            get => Data.GetByte(atkCurve5);
            set => Data.SetByte(atkCurve5, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve10 {
            get => Data.GetByte(atkCurve10);
            set => Data.SetByte(atkCurve10, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve12_15 {
            get => Data.GetByte(atkCurve12_15);
            set => Data.SetByte(atkCurve12_15, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve14_20 {
            get => Data.GetByte(atkCurve14_20);
            set => Data.SetByte(atkCurve14_20, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve17_30 {
            get => Data.GetByte(atkCurve17_30);
            set => Data.SetByte(atkCurve17_30, (byte) value);
        }

        [BulkCopy]
        public int AtkCurve30_99 {
            get => Data.GetByte(atkCurve30_99);
            set => Data.SetByte(atkCurve30_99, (byte) value);
        }

        public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [BulkCopy]
        public int DefPromote {
            get => Data.GetByte(defPromote);
            set => Data.SetByte(defPromote, (byte) value);
        }

        [BulkCopy]
        public int DefCurve1 {
            get => Data.GetByte(defCurve1);
            set => Data.SetByte(defCurve1, (byte) value);
        }

        [BulkCopy]
        public int DefCurve5 {
            get => Data.GetByte(defCurve5);
            set => Data.SetByte(defCurve5, (byte) value);
        }

        [BulkCopy]
        public int DefCurve10 {
            get => Data.GetByte(defCurve10);
            set => Data.SetByte(defCurve10, (byte) value);
        }

        [BulkCopy]
        public int DefCurve12_15 {
            get => Data.GetByte(defCurve12_15);
            set => Data.SetByte(defCurve12_15, (byte) value);
        }

        [BulkCopy]
        public int DefCurve14_20 {
            get => Data.GetByte(defCurve14_20);
            set => Data.SetByte(defCurve14_20, (byte) value);
        }

        [BulkCopy]
        public int DefCurve17_30 {
            get => Data.GetByte(defCurve17_30);
            set => Data.SetByte(defCurve17_30, (byte) value);
        }

        [BulkCopy]
        public int DefCurve30_99 {
            get => Data.GetByte(defCurve30_99);
            set => Data.SetByte(defCurve30_99, (byte) value);
        }

        public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [BulkCopy]
        public int AgiPromote {
            get => Data.GetByte(agiPromote);
            set => Data.SetByte(agiPromote, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve1 {
            get => Data.GetByte(agiCurve1);
            set => Data.SetByte(agiCurve1, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve5 {
            get => Data.GetByte(agiCurve5);
            set => Data.SetByte(agiCurve5, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve10 {
            get => Data.GetByte(agiCurve10);
            set => Data.SetByte(agiCurve10, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve12_15 {
            get => Data.GetByte(agiCurve12_15);
            set => Data.SetByte(agiCurve12_15, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve14_20 {
            get => Data.GetByte(agiCurve14_20);
            set => Data.SetByte(agiCurve14_20, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve17_30 {
            get => Data.GetByte(agiCurve17_30);
            set => Data.SetByte(agiCurve17_30, (byte) value);
        }

        [BulkCopy]
        public int AgiCurve30_99 {
            get => Data.GetByte(agiCurve30_99);
            set => Data.SetByte(agiCurve30_99, (byte) value);
        }

        public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);

        [BulkCopy]
        public int S1LearnedAt {
            get => Data.GetByte(s1LearnedAt);
            set => Data.SetByte(s1LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S1LearnedLevel {
            get => Data.GetByte(s1LearnedLevel);
            set => Data.SetByte(s1LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S1LearnedID {
            get => Data.GetByte(s1LearnedID);
            set => Data.SetByte(s1LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S2LearnedAt {
            get => Data.GetByte(s2LearnedAt);
            set => Data.SetByte(s2LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S2LearnedLevel {
            get => Data.GetByte(s2LearnedLevel);
            set => Data.SetByte(s2LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S2LearnedID {
            get => Data.GetByte(s2LearnedID);
            set => Data.SetByte(s2LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S3LearnedAt {
            get => Data.GetByte(s3LearnedAt);
            set => Data.SetByte(s3LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S3LearnedLevel {
            get => Data.GetByte(s3LearnedLevel);
            set => Data.SetByte(s3LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S3LearnedID {
            get => Data.GetByte(s3LearnedID);
            set => Data.SetByte(s3LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S4LearnedAt {
            get => Data.GetByte(s4LearnedAt);
            set => Data.SetByte(s4LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S4LearnedLevel {
            get => Data.GetByte(s4LearnedLevel);
            set => Data.SetByte(s4LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S4LearnedID {
            get => Data.GetByte(s4LearnedID);
            set => Data.SetByte(s4LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S5LearnedAt {
            get => Data.GetByte(s5LearnedAt);
            set => Data.SetByte(s5LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S5LearnedLevel {
            get => Data.GetByte(s5LearnedLevel);
            set => Data.SetByte(s5LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S5LearnedID {
            get => Data.GetByte(s5LearnedID);
            set => Data.SetByte(s5LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S6LearnedAt {
            get => Data.GetByte(s6LearnedAt);
            set => Data.SetByte(s6LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S6LearnedLevel {
            get => Data.GetByte(s6LearnedLevel);
            set => Data.SetByte(s6LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S6LearnedID {
            get => Data.GetByte(s6LearnedID);
            set => Data.SetByte(s6LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S7LearnedAt {
            get => Data.GetByte(s7LearnedAt);
            set => Data.SetByte(s7LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S7LearnedLevel {
            get => Data.GetByte(s7LearnedLevel);
            set => Data.SetByte(s7LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S7LearnedID {
            get => Data.GetByte(s7LearnedID);
            set => Data.SetByte(s7LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S8LearnedAt {
            get => Data.GetByte(s8LearnedAt);
            set => Data.SetByte(s8LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S8LearnedLevel {
            get => Data.GetByte(s8LearnedLevel);
            set => Data.SetByte(s8LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S8LearnedID {
            get => Data.GetByte(s8LearnedID);
            set => Data.SetByte(s8LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S9LearnedAt {
            get => Data.GetByte(s9LearnedAt);
            set => Data.SetByte(s9LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S9LearnedLevel {
            get => Data.GetByte(s9LearnedLevel);
            set => Data.SetByte(s9LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S9LearnedID {
            get => Data.GetByte(s9LearnedID);
            set => Data.SetByte(s9LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S10LearnedAt {
            get => Data.GetByte(s10LearnedAt);
            set => Data.SetByte(s10LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S10LearnedLevel {
            get => Data.GetByte(s10LearnedLevel);
            set => Data.SetByte(s10LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S10LearnedID {
            get => Data.GetByte(s10LearnedID);
            set => Data.SetByte(s10LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S11LearnedAt {
            get => Data.GetByte(s11LearnedAt);
            set => Data.SetByte(s11LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S11LearnedLevel {
            get => Data.GetByte(s11LearnedLevel);
            set => Data.SetByte(s11LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S11LearnedID {
            get => Data.GetByte(s11LearnedID);
            set => Data.SetByte(s11LearnedID, (byte) value);
        }

        [BulkCopy]
        public int S12LearnedAt {
            get => Data.GetByte(s12LearnedAt);
            set => Data.SetByte(s12LearnedAt, (byte) value);
        }

        [BulkCopy]
        public int S12LearnedLevel {
            get => Data.GetByte(s12LearnedLevel);
            set => Data.SetByte(s12LearnedLevel, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S12LearnedID {
            get => Data.GetByte(s12LearnedID);
            set => Data.SetByte(s12LearnedID, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special1 {
            get => Data.GetByte(weapon1Special1);
            set => Data.SetByte(weapon1Special1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special2 {
            get => Data.GetByte(weapon1Special2);
            set => Data.SetByte(weapon1Special2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special3 {
            get => Data.GetByte(weapon1Special3);
            set => Data.SetByte(weapon1Special3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special1 {
            get => Data.GetByte(weapon2Special1);
            set => Data.SetByte(weapon2Special1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special2 {
            get => Data.GetByte(weapon2Special2);
            set => Data.SetByte(weapon2Special2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special3 {
            get => Data.GetByte(weapon2Special3);
            set => Data.SetByte(weapon2Special3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special1 {
            get => Data.GetByte(weapon3Special1);
            set => Data.SetByte(weapon3Special1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special2 {
            get => Data.GetByte(weapon3Special2);
            set => Data.SetByte(weapon3Special2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special3 {
            get => Data.GetByte(weapon3Special3);
            set => Data.SetByte(weapon3Special3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special1 {
            get => Data.GetByte(weapon4Special1);
            set => Data.SetByte(weapon4Special1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special2 {
            get => Data.GetByte(weapon4Special2);
            set => Data.SetByte(weapon4Special2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special3 {
            get => Data.GetByte(weapon4Special3);
            set => Data.SetByte(weapon4Special3, (byte) value);
        }

        [BulkCopy]
        public int BaseLuck {
            get => Data.GetByte(baseLuck);
            set => Data.SetByte(baseLuck, (byte) value);
        }

        [BulkCopy]
        public int BaseMov {
            get => Data.GetByte(baseMov);
            set => Data.SetByte(baseMov, (byte) value);
        }

        [BulkCopy]
        public int BaseTurns {
            get => Data.GetByte(baseTurns);
            set => Data.SetByte(baseTurns, (byte) value);
        }

        [BulkCopy]
        public int BaseHPRegen {
            get => Data.GetByte(baseHPRegen);
            set => Data.SetByte(baseHPRegen, (byte) value);
        }

        [BulkCopy]
        public int BaseMPRegen {
            get => Data.GetByte(baseMPRegen);
            set => Data.SetByte(baseMPRegen, (byte) value);
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
        public int UnknownRes {
            get => (sbyte) Data.GetByte(unknownRes);
            set => Data.SetByte(unknownRes, (byte) value);
        }

        [BulkCopy]
        public int Slow {
            get => Data.GetByte(slow);
            set => Data.SetByte(slow, (byte) value);
        }

        [BulkCopy]
        public int Support {
            get => Data.GetByte(support);
            set => Data.SetByte(support, (byte) value);
        }

        [BulkCopy]
        public int MagicBonus {
            get => Data.GetByte(magicBonus);
            set => Data.SetByte(magicBonus, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public int MovementType {
            get => Data.GetByte(movementType);
            set => Data.SetByte(movementType, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable1 {
            get => Data.GetByte(weaponEquipable1);
            set => Data.SetByte(weaponEquipable1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable2 {
            get => Data.GetByte(weaponEquipable2);
            set => Data.SetByte(weaponEquipable2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable3 {
            get => Data.GetByte(weaponEquipable3);
            set => Data.SetByte(weaponEquipable3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable4 {
            get => Data.GetByte(weaponEquipable4);
            set => Data.SetByte(weaponEquipable4, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable1 {
            get => Data.GetByte(accessoryEquipable1);
            set => Data.SetByte(accessoryEquipable1, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable2 {
            get => Data.GetByte(accessoryEquipable2);
            set => Data.SetByte(accessoryEquipable2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable3 {
            get => Data.GetByte(accessoryEquipable3);
            set => Data.SetByte(accessoryEquipable3, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable4 {
            get => Data.GetByte(accessoryEquipable4);
            set => Data.SetByte(accessoryEquipable4, (byte) value);
        }
    }
}
