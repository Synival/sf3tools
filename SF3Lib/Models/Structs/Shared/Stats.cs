using System;
using CommonLib.Attributes;
using CommonLib.Statistics;
using SF3.ByteData;
using SF3.Statistics;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
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
        private readonly int _characterIdAddr;
        private readonly int _characterClassAddr;

        private readonly int _hpPromoteAddr;
        private readonly int _hpCurve1Addr;
        private readonly int _hpCurve5Addr;
        private readonly int _hpCurve10Addr;
        private readonly int _hpCurve12_15Addr;
        private readonly int _hpCurve14_20Addr;
        private readonly int _hpCurve17_30Addr;
        private readonly int _hpCurve30_99Addr;

        private readonly int _mpPromoteAddr;
        private readonly int _mpCurve1Addr;
        private readonly int _mpCurve5Addr;
        private readonly int _mpCurve10Addr;
        private readonly int _mpCurve12_15Addr;
        private readonly int _mpCurve14_20Addr;
        private readonly int _mpCurve17_30Addr;
        private readonly int _mpCurve30_99Addr;

        private readonly int _atkPromoteAddr;
        private readonly int _atkCurve1Addr;
        private readonly int _atkCurve5Addr;
        private readonly int _atkCurve10Addr;
        private readonly int _atkCurve12_15Addr;
        private readonly int _atkCurve14_20Addr;
        private readonly int _atkCurve17_30Addr;
        private readonly int _atkCurve30_99Addr;

        private readonly int _defPromoteAddr;
        private readonly int _defCurve1Addr;
        private readonly int _defCurve5Addr;
        private readonly int _defCurve10Addr;
        private readonly int _defCurve12_15Addr;
        private readonly int _defCurve14_20Addr;
        private readonly int _defCurve17_30Addr;
        private readonly int _defCurve30_99Addr;

        private readonly int _agiPromoteAddr;
        private readonly int _agiCurve1Addr;
        private readonly int _agiCurve5Addr;
        private readonly int _agiCurve10Addr;
        private readonly int _agiCurve12_15Addr;
        private readonly int _agiCurve14_20Addr;
        private readonly int _agiCurve17_30Addr;
        private readonly int _agiCurve30_99Addr;

        private readonly int _s1CharLvAddr;
        private readonly int _s1SpellIdAddr;
        private readonly int _s1SpellLvAddr;

        private readonly int _s2CharLvAddr;
        private readonly int _s2SpellIdAddr;
        private readonly int _s2SpellLvAddr;

        private readonly int _s3CharLvAddr;
        private readonly int _s3SpellIdAddr;
        private readonly int _s3SpellLvAddr;

        private readonly int _s4CharLvAddr;
        private readonly int _s4SpellIdAddr;
        private readonly int _s4SpellLvAddr;

        private readonly int _s5CharLvAddr;
        private readonly int _s5SpellIdAddr;
        private readonly int _s5SpellLvAddr;

        private readonly int _s6CharLvAddr;
        private readonly int _s6SpellIdAddr;
        private readonly int _s6SpellLvAddr;

        private readonly int _s7CharLvAddr;
        private readonly int _s7SpellIdAddr;
        private readonly int _s7SpellLvAddr;

        private readonly int _s8CharLvAddr;
        private readonly int _s8SpellIdAddr;
        private readonly int _s8SpellLvAddr;

        private readonly int _s9CharLvAddr;
        private readonly int _s9SpellIdAddr;
        private readonly int _s9SpellLvAddr;

        private readonly int _s10CharLvAddr;
        private readonly int _s10SpellIdAddr;
        private readonly int _s10SpellLvAddr;

        private readonly int _s11CharLvAddr;
        private readonly int _s11SpellIdAddr;
        private readonly int _s11SpellLvAddr;

        private readonly int _s12CharLvAddr;
        private readonly int _s12SpellIdAddr;
        private readonly int _s12SpellLvAddr;

        private readonly int _weapon1Special1Addr;
        private readonly int _weapon1Special2Addr;
        private readonly int _weapon1Special3Addr;

        private readonly int _weapon2Special1Addr;
        private readonly int _weapon2Special2Addr;
        private readonly int _weapon2Special3Addr;

        private readonly int _weapon3Special1Addr;
        private readonly int _weapon3Special2Addr;
        private readonly int _weapon3Special3Addr;

        private readonly int _weapon4Special1Addr;
        private readonly int _weapon4Special2Addr;
        private readonly int _weapon4Special3Addr;

        private readonly int _baseLuckAddr;
        private readonly int _baseMovAddr;

        private readonly int _baseTurnsAddr;
        private readonly int _baseHPRegenAddr;
        private readonly int _baseMPRegenAddr;

        private readonly int _earthResAddr;
        private readonly int _fireResAddr;
        private readonly int _iceResAddr;
        private readonly int _sparkResAddr;
        private readonly int _windResAddr;
        private readonly int _lightResAddr;
        private readonly int _darkResAddr;
        private readonly int _unknownResAddr;

        private readonly int _slowAddr;
        private readonly int _supportAddr;

        private readonly int _magicBonusIdAddr;
        private readonly int _movementTypeAddr;

        private readonly int _weaponEquipable1Addr;
        private readonly int _weaponEquipable2Addr;
        private readonly int _weaponEquipable3Addr;
        private readonly int _weaponEquipable4Addr;

        private readonly int _accessoryEquipable1Addr;
        private readonly int _accessoryEquipable2Addr;
        private readonly int _accessoryEquipable3Addr;
        private readonly int _accessoryEquipable4Addr;

        public Stats(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x7B) {
            _characterIdAddr         = Address + 0x00;
            _characterClassAddr      = Address + 0x01;
            _hpPromoteAddr           = Address + 0x02;
            _hpCurve1Addr            = Address + 0x03;
            _hpCurve5Addr            = Address + 0x04;
            _hpCurve10Addr           = Address + 0x05;
            _hpCurve12_15Addr        = Address + 0x06;
            _hpCurve14_20Addr        = Address + 0x07;
            _hpCurve17_30Addr        = Address + 0x08;
            _hpCurve30_99Addr        = Address + 0x09;
            _mpPromoteAddr           = Address + 0x0A;
            _mpCurve1Addr            = Address + 0x0B;
            _mpCurve5Addr            = Address + 0x0C;
            _mpCurve10Addr           = Address + 0x0D;
            _mpCurve12_15Addr        = Address + 0x0E;
            _mpCurve14_20Addr        = Address + 0x0F;
            _mpCurve17_30Addr        = Address + 0x10;
            _mpCurve30_99Addr        = Address + 0x11;
            _atkPromoteAddr          = Address + 0x12;
            _atkCurve1Addr           = Address + 0x13;
            _atkCurve5Addr           = Address + 0x14;
            _atkCurve10Addr          = Address + 0x15;
            _atkCurve12_15Addr       = Address + 0x16;
            _atkCurve14_20Addr       = Address + 0x17;
            _atkCurve17_30Addr       = Address + 0x18;
            _atkCurve30_99Addr       = Address + 0x19;
            _defPromoteAddr          = Address + 0x1a;
            _defCurve1Addr           = Address + 0x1b;
            _defCurve5Addr           = Address + 0x1c;
            _defCurve10Addr          = Address + 0x1d;
            _defCurve12_15Addr       = Address + 0x1e;
            _defCurve14_20Addr       = Address + 0x1f;
            _defCurve17_30Addr       = Address + 0x20;
            _defCurve30_99Addr       = Address + 0x21;
            _agiPromoteAddr          = Address + 0x22;
            _agiCurve1Addr           = Address + 0x23;
            _agiCurve5Addr           = Address + 0x24;
            _agiCurve10Addr          = Address + 0x25;
            _agiCurve12_15Addr       = Address + 0x26;
            _agiCurve14_20Addr       = Address + 0x27;
            _agiCurve17_30Addr       = Address + 0x28;
            _agiCurve30_99Addr       = Address + 0x29;

            _s1CharLvAddr            = Address + 0x2a;
            _s1SpellIdAddr           = Address + 0x2b;
            _s1SpellLvAddr           = Address + 0x2c;

            _s2CharLvAddr            = Address + 0x2d;
            _s2SpellIdAddr           = Address + 0x2e;
            _s2SpellLvAddr           = Address + 0x2f;

            _s3CharLvAddr            = Address + 0x30;
            _s3SpellIdAddr           = Address + 0x31;
            _s3SpellLvAddr           = Address + 0x32;

            _s4CharLvAddr            = Address + 0x33;
            _s4SpellIdAddr           = Address + 0x34;
            _s4SpellLvAddr           = Address + 0x35;

            _s5CharLvAddr            = Address + 0x36;
            _s5SpellIdAddr           = Address + 0x37;
            _s5SpellLvAddr           = Address + 0x38;

            _s6CharLvAddr            = Address + 0x39;
            _s6SpellIdAddr           = Address + 0x3a;
            _s6SpellLvAddr           = Address + 0x3b;

            _s7CharLvAddr            = Address + 0x3c;
            _s7SpellIdAddr           = Address + 0x3d;
            _s7SpellLvAddr           = Address + 0x3e;

            _s8CharLvAddr            = Address + 0x3f;
            _s8SpellIdAddr           = Address + 0x40;
            _s8SpellLvAddr           = Address + 0x41;

            _s9CharLvAddr            = Address + 0x42;
            _s9SpellIdAddr           = Address + 0x43;
            _s9SpellLvAddr           = Address + 0x44;

            _s10CharLvAddr           = Address + 0x45;
            _s10SpellIdAddr          = Address + 0x46;
            _s10SpellLvAddr          = Address + 0x47;

            _s11CharLvAddr           = Address + 0x48;
            _s11SpellIdAddr          = Address + 0x49;
            _s11SpellLvAddr          = Address + 0x4a;

            _s12CharLvAddr           = Address + 0x4b;
            _s12SpellIdAddr          = Address + 0x4c;
            _s12SpellLvAddr          = Address + 0x4d;

            _weapon1Special1Addr     = Address + 0x4e;
            _weapon1Special2Addr     = Address + 0x4f;
            _weapon1Special3Addr     = Address + 0x50;
            _weapon2Special1Addr     = Address + 0x51;
            _weapon2Special2Addr     = Address + 0x52;
            _weapon2Special3Addr     = Address + 0x53;
            _weapon3Special1Addr     = Address + 0x54;
            _weapon3Special2Addr     = Address + 0x55;
            _weapon3Special3Addr     = Address + 0x56;
            _weapon4Special1Addr     = Address + 0x57;
            _weapon4Special2Addr     = Address + 0x58;
            _weapon4Special3Addr     = Address + 0x59;

            _baseLuckAddr            = Address + 0x5a;
            _baseMovAddr             = Address + 0x5b;
            _baseTurnsAddr           = Address + 0x5c;
            _baseHPRegenAddr         = Address + 0x5d;
            _baseMPRegenAddr         = Address + 0x5e;

            _earthResAddr            = Address + 0x5f;
            _fireResAddr             = Address + 0x60;
            _iceResAddr              = Address + 0x61;
            _sparkResAddr            = Address + 0x62;
            _windResAddr             = Address + 0x63;
            _lightResAddr            = Address + 0x64;
            _darkResAddr             = Address + 0x65;
            _unknownResAddr          = Address + 0x66;
            _slowAddr                = Address + 0x67;
            _supportAddr             = Address + 0x68;
            _magicBonusIdAddr        = Address + 0x69;
            _movementTypeAddr        = Address + 0x6a;

            _weaponEquipable1Addr    = Address + 0x73;
            _weaponEquipable2Addr    = Address + 0x74;
            _weaponEquipable3Addr    = Address + 0x75;
            _weaponEquipable4Addr    = Address + 0x76;

            _accessoryEquipable1Addr = Address + 0x77;
            _accessoryEquipable2Addr = Address + 0x78;
            _accessoryEquipable3Addr = Address + 0x79;
            _accessoryEquipable4Addr = Address + 0x7a;
        }

        public bool IsPromoted => Data.GetByte(_characterClassAddr) >= 0x20;

        public PromotionLevelType PromotionLevel {
            get {
                var chClass = Data.GetByte(_characterClassAddr);
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

        // ==============================
        // Character and Class
        // ==============================

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2", displayGroup: "CharAndClass")]
        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public int CharacterID {
            get => Data.GetByte(_characterIdAddr);
            set => Data.SetByte(_characterIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_characterClassAddr), displayOrder: 1, displayName: "Class", minWidth: 150, displayFormat: "X2", displayGroup: "CharAndClass")]
        [BulkCopy]
        [NameGetter(NamedValueType.CharacterClass)]
        public int CharacterClass {
            get => Data.GetByte(_characterClassAddr);
            set => Data.SetByte(_characterClassAddr, (byte) value);
        }

        // ==============================
        // Stats
        // ==============================

        [TableViewModelColumn(addressField: nameof(_hpPromoteAddr), displayOrder: 2, displayGroup: "Stats")]
        [BulkCopy]
        public int HPPromote {
            get => Data.GetByte(_hpPromoteAddr);
            set => Data.SetByte(_hpPromoteAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve1Addr), displayOrder: 3, displayName: "HPLv1", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve1 {
            get => Data.GetByte(_hpCurve1Addr);
            set => Data.SetByte(_hpCurve1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve5Addr), displayOrder: 4, displayName: "HPLv5", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve5 {
            get => Data.GetByte(_hpCurve5Addr);
            set => Data.SetByte(_hpCurve5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve10Addr), displayOrder: 5, displayName: "HPLv10", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve10 {
            get => Data.GetByte(_hpCurve10Addr);
            set => Data.SetByte(_hpCurve10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve12_15Addr), displayOrder: 6, displayName: "HPLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve12_15 {
            get => Data.GetByte(_hpCurve12_15Addr);
            set => Data.SetByte(_hpCurve12_15Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve14_20Addr), displayOrder: 7, displayName: "HPLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve14_20 {
            get => Data.GetByte(_hpCurve14_20Addr);
            set => Data.SetByte(_hpCurve14_20Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve17_30Addr), displayOrder: 8, displayName: "HPLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve17_30 {
            get => Data.GetByte(_hpCurve17_30Addr);
            set => Data.SetByte(_hpCurve17_30Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve30_99Addr), displayOrder: 9, displayName: "HPLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public int HPCurve30_99 {
            get => Data.GetByte(_hpCurve30_99Addr);
            set => Data.SetByte(_hpCurve30_99Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_mpPromoteAddr), displayOrder: 9.5f, displayGroup: "Stats")]
        [BulkCopy]
        public int MPPromote {
            get => Data.GetByte(_mpPromoteAddr);
            set => Data.SetByte(_mpPromoteAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve1Addr), displayOrder: 10, displayName: "MPLv1", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve1 {
            get => Data.GetByte(_mpCurve1Addr);
            set => Data.SetByte(_mpCurve1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve5Addr), displayOrder: 11, displayName: "MPLv5", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve5 {
            get => Data.GetByte(_mpCurve5Addr);
            set => Data.SetByte(_mpCurve5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve10Addr), displayOrder: 12, displayName: "MPLv10", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve10 {
            get => Data.GetByte(_mpCurve10Addr);
            set => Data.SetByte(_mpCurve10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve12_15Addr), displayOrder: 13, displayName: "MPLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve12_15 {
            get => Data.GetByte(_mpCurve12_15Addr);
            set => Data.SetByte(_mpCurve12_15Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve14_20Addr), displayOrder: 14, displayName: "MPLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve14_20 {
            get => Data.GetByte(_mpCurve14_20Addr);
            set => Data.SetByte(_mpCurve14_20Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve17_30Addr), displayOrder: 15, displayName: "MPLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve17_30 {
            get => Data.GetByte(_mpCurve17_30Addr);
            set => Data.SetByte(_mpCurve17_30Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve30_99Addr), displayOrder: 16, displayName: "MPLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public int MPCurve30_99 {
            get => Data.GetByte(_mpCurve30_99Addr);
            set => Data.SetByte(_mpCurve30_99Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_atkPromoteAddr), displayOrder: 17, displayGroup: "Stats")]
        [BulkCopy]
        public int AtkPromote {
            get => Data.GetByte(_atkPromoteAddr);
            set => Data.SetByte(_atkPromoteAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve1Addr), displayOrder: 18, displayName: "AtkLv1", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve1 {
            get => Data.GetByte(_atkCurve1Addr);
            set => Data.SetByte(_atkCurve1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve5Addr), displayOrder: 19, displayName: "AtkLv5", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve5 {
            get => Data.GetByte(_atkCurve5Addr);
            set => Data.SetByte(_atkCurve5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve10Addr), displayOrder: 20, displayName: "AtkLv10", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve10 {
            get => Data.GetByte(_atkCurve10Addr);
            set => Data.SetByte(_atkCurve10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve12_15Addr), displayOrder: 21, displayName: "AtkLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve12_15 {
            get => Data.GetByte(_atkCurve12_15Addr);
            set => Data.SetByte(_atkCurve12_15Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve14_20Addr), displayOrder: 22, displayName: "AtkLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve14_20 {
            get => Data.GetByte(_atkCurve14_20Addr);
            set => Data.SetByte(_atkCurve14_20Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve17_30Addr), displayOrder: 23, displayName: "AtkLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve17_30 {
            get => Data.GetByte(_atkCurve17_30Addr);
            set => Data.SetByte(_atkCurve17_30Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve30_99Addr), displayOrder: 24, displayName: "AtkLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public int AtkCurve30_99 {
            get => Data.GetByte(_atkCurve30_99Addr);
            set => Data.SetByte(_atkCurve30_99Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_defPromoteAddr), displayOrder: 25, displayGroup: "Stats")]
        [BulkCopy]
        public int DefPromote {
            get => Data.GetByte(_defPromoteAddr);
            set => Data.SetByte(_defPromoteAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve1Addr), displayOrder: 26, displayName: "DefLv1", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve1 {
            get => Data.GetByte(_defCurve1Addr);
            set => Data.SetByte(_defCurve1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve5Addr), displayOrder: 27, displayName: "DefLv5", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve5 {
            get => Data.GetByte(_defCurve5Addr);
            set => Data.SetByte(_defCurve5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve10Addr), displayOrder: 28, displayName: "DefLv10", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve10 {
            get => Data.GetByte(_defCurve10Addr);
            set => Data.SetByte(_defCurve10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve12_15Addr), displayOrder: 29, displayName: "DefLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve12_15 {
            get => Data.GetByte(_defCurve12_15Addr);
            set => Data.SetByte(_defCurve12_15Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve14_20Addr), displayOrder: 30, displayName: "DefLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve14_20 {
            get => Data.GetByte(_defCurve14_20Addr);
            set => Data.SetByte(_defCurve14_20Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve17_30Addr), displayOrder: 31, displayName: "DefLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve17_30 {
            get => Data.GetByte(_defCurve17_30Addr);
            set => Data.SetByte(_defCurve17_30Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve30_99Addr), displayOrder: 32, displayName: "DefLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public int DefCurve30_99 {
            get => Data.GetByte(_defCurve30_99Addr);
            set => Data.SetByte(_defCurve30_99Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_agiPromoteAddr), displayOrder: 33, displayGroup: "Stats")]
        [BulkCopy]
        public int AgiPromote {
            get => Data.GetByte(_agiPromoteAddr);
            set => Data.SetByte(_agiPromoteAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve1Addr), displayOrder: 34, displayName: "AgiLv1", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve1 {
            get => Data.GetByte(_agiCurve1Addr);
            set => Data.SetByte(_agiCurve1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve5Addr), displayOrder: 35, displayName: "AgiLv5", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve5 {
            get => Data.GetByte(_agiCurve5Addr);
            set => Data.SetByte(_agiCurve5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve10Addr), displayOrder: 36, displayName: "AgiLv10", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve10 {
            get => Data.GetByte(_agiCurve10Addr);
            set => Data.SetByte(_agiCurve10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve12_15Addr), displayOrder: 37, displayName: "AgiLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve12_15 {
            get => Data.GetByte(_agiCurve12_15Addr);
            set => Data.SetByte(_agiCurve12_15Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve14_20Addr), displayOrder: 38, displayName: "AgiLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve14_20 {
            get => Data.GetByte(_agiCurve14_20Addr);
            set => Data.SetByte(_agiCurve14_20Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve17_30Addr), displayOrder: 39, displayName: "AgiLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve17_30 {
            get => Data.GetByte(_agiCurve17_30Addr);
            set => Data.SetByte(_agiCurve17_30Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve30_99Addr), displayOrder: 40, displayName: "AgiLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public int AgiCurve30_99 {
            get => Data.GetByte(_agiCurve30_99Addr);
            set => Data.SetByte(_agiCurve30_99Addr, (byte) value);
        }

        // ==============================
        // Spells
        // ==============================

        [TableViewModelColumn(addressField: nameof(_s1CharLvAddr), displayOrder: 41, displayGroup: "Spells")]
        [BulkCopy]
        public int S1CharLv {
            get => Data.GetByte(_s1CharLvAddr);
            set => Data.SetByte(_s1CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s1SpellIdAddr), displayOrder: 42, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S1SpellID {
            get => Data.GetByte(_s1SpellIdAddr);
            set => Data.SetByte(_s1SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s1SpellLvAddr), displayOrder: 43, displayGroup: "Spells")]
        [BulkCopy]
        public int S1SpellLv {
            get => Data.GetByte(_s1SpellLvAddr);
            set => Data.SetByte(_s1SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s2CharLvAddr), displayOrder: 44, displayGroup: "Spells")]
        [BulkCopy]
        public int S2CharLv {
            get => Data.GetByte(_s2CharLvAddr);
            set => Data.SetByte(_s2CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s2SpellIdAddr), displayOrder: 45, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S2SpellID {
            get => Data.GetByte(_s2SpellIdAddr);
            set => Data.SetByte(_s2SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s2SpellLvAddr), displayOrder: 46, displayGroup: "Spells")]
        [BulkCopy]
        public int S2SpellLv {
            get => Data.GetByte(_s2SpellLvAddr);
            set => Data.SetByte(_s2SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s3CharLvAddr), displayOrder: 47, displayGroup: "Spells")]
        [BulkCopy]
        public int S3CharLv {
            get => Data.GetByte(_s3CharLvAddr);
            set => Data.SetByte(_s3CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s3SpellIdAddr), displayOrder: 48, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S3SpellID {
            get => Data.GetByte(_s3SpellIdAddr);
            set => Data.SetByte(_s3SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s3SpellLvAddr), displayOrder: 49, displayGroup: "Spells")]
        [BulkCopy]
        public int S3SpellLv {
            get => Data.GetByte(_s3SpellLvAddr);
            set => Data.SetByte(_s3SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s4CharLvAddr), displayOrder: 50, displayGroup: "Spells")]
        [BulkCopy]
        public int S4CharLv {
            get => Data.GetByte(_s4CharLvAddr);
            set => Data.SetByte(_s4CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s4SpellIdAddr), displayOrder: 51, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S4SpellID {
            get => Data.GetByte(_s4SpellIdAddr);
            set => Data.SetByte(_s4SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s4SpellLvAddr), displayOrder: 52, displayGroup: "Spells")]
        [BulkCopy]
        public int S4SpellLv {
            get => Data.GetByte(_s4SpellLvAddr);
            set => Data.SetByte(_s4SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s5CharLvAddr), displayOrder: 53, displayGroup: "Spells")]
        [BulkCopy]
        public int S5CharLv {
            get => Data.GetByte(_s5CharLvAddr);
            set => Data.SetByte(_s5CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s5SpellIdAddr), displayOrder: 54, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S5SpellID {
            get => Data.GetByte(_s5SpellIdAddr);
            set => Data.SetByte(_s5SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s5SpellLvAddr), displayOrder: 55, displayGroup: "Spells")]
        [BulkCopy]
        public int S5SpellLv {
            get => Data.GetByte(_s5SpellLvAddr);
            set => Data.SetByte(_s5SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s6CharLvAddr), displayOrder: 56, displayGroup: "Spells")]
        [BulkCopy]
        public int S6CharLv {
            get => Data.GetByte(_s6CharLvAddr);
            set => Data.SetByte(_s6CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s6SpellIdAddr), displayOrder: 57, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S6SpellID {
            get => Data.GetByte(_s6SpellIdAddr);
            set => Data.SetByte(_s6SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s6SpellLvAddr), displayOrder: 58, displayGroup: "Spells")]
        [BulkCopy]
        public int S6SpellLv {
            get => Data.GetByte(_s6SpellLvAddr);
            set => Data.SetByte(_s6SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s7CharLvAddr), displayOrder: 59, displayGroup: "Spells")]
        [BulkCopy]
        public int S7CharLv {
            get => Data.GetByte(_s7CharLvAddr);
            set => Data.SetByte(_s7CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s7SpellIdAddr), displayOrder: 60, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S7SpellID {
            get => Data.GetByte(_s7SpellIdAddr);
            set => Data.SetByte(_s7SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s7SpellLvAddr), displayOrder: 61, displayGroup: "Spells")]
        [BulkCopy]
        public int S7SpellLv {
            get => Data.GetByte(_s7SpellLvAddr);
            set => Data.SetByte(_s7SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s8CharLvAddr), displayOrder: 62, displayGroup: "Spells")]
        [BulkCopy]
        public int S8CharLv {
            get => Data.GetByte(_s8CharLvAddr);
            set => Data.SetByte(_s8CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s8SpellIdAddr), displayOrder: 63, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S8SpellID {
            get => Data.GetByte(_s8SpellIdAddr);
            set => Data.SetByte(_s8SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s8SpellLvAddr), displayOrder: 64, displayGroup: "Spells")]
        [BulkCopy]
        public int S8SpellLv {
            get => Data.GetByte(_s8SpellLvAddr);
            set => Data.SetByte(_s8SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s9CharLvAddr), displayOrder: 65, displayGroup: "Spells")]
        [BulkCopy]
        public int S9CharLv {
            get => Data.GetByte(_s9CharLvAddr);
            set => Data.SetByte(_s9CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s9SpellIdAddr), displayOrder: 66, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S9SpellID {
            get => Data.GetByte(_s9SpellIdAddr);
            set => Data.SetByte(_s9SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s9SpellLvAddr), displayOrder: 67, displayGroup: "Spells")]
        [BulkCopy]
        public int S9SpellLv {
            get => Data.GetByte(_s9SpellLvAddr);
            set => Data.SetByte(_s9SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s10CharLvAddr), displayOrder: 68, displayGroup: "Spells")]
        [BulkCopy]
        public int S10CharLv {
            get => Data.GetByte(_s10CharLvAddr);
            set => Data.SetByte(_s10CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s10SpellIdAddr), displayOrder: 69, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S10SpellID {
            get => Data.GetByte(_s10SpellIdAddr);
            set => Data.SetByte(_s10SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s10SpellLvAddr), displayOrder: 70, displayGroup: "Spells")]
        [BulkCopy]
        public int S10SpellLv {
            get => Data.GetByte(_s10SpellLvAddr);
            set => Data.SetByte(_s10SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s11CharLvAddr), displayOrder: 71, displayGroup: "Spells")]
        [BulkCopy]
        public int S11CharLv {
            get => Data.GetByte(_s11CharLvAddr);
            set => Data.SetByte(_s11CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s11SpellIdAddr), displayOrder: 72, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S11SpellID {
            get => Data.GetByte(_s11SpellIdAddr);
            set => Data.SetByte(_s11SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s11SpellLvAddr), displayOrder: 73, displayGroup: "Spells")]
        [BulkCopy]
        public int S11SpellLv {
            get => Data.GetByte(_s11SpellLvAddr);
            set => Data.SetByte(_s11SpellLvAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s12CharLvAddr), displayOrder: 74, displayGroup: "Spells")]
        [BulkCopy]
        public int S12CharLv {
            get => Data.GetByte(_s12CharLvAddr);
            set => Data.SetByte(_s12CharLvAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s12SpellIdAddr), displayOrder: 75, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int S12SpellID {
            get => Data.GetByte(_s12SpellIdAddr);
            set => Data.SetByte(_s12SpellIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_s12SpellLvAddr), displayOrder: 76, displayGroup: "Spells")]
        [BulkCopy]
        public int S12SpellLv {
            get => Data.GetByte(_s12SpellLvAddr);
            set => Data.SetByte(_s12SpellLvAddr, (byte) value);
        }

        // ==============================
        // Specials
        // ==============================

        [TableViewModelColumn(addressField: nameof(_weapon1Special1Addr), displayOrder: 77, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special1 {
            get => Data.GetByte(_weapon1Special1Addr);
            set => Data.SetByte(_weapon1Special1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1Special2Addr), displayOrder: 78, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special2 {
            get => Data.GetByte(_weapon1Special2Addr);
            set => Data.SetByte(_weapon1Special2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1Special3Addr), displayOrder: 79, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon1Special3 {
            get => Data.GetByte(_weapon1Special3Addr);
            set => Data.SetByte(_weapon1Special3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special1Addr), displayOrder: 80, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special1 {
            get => Data.GetByte(_weapon2Special1Addr);
            set => Data.SetByte(_weapon2Special1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special2Addr), displayOrder: 81, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special2 {
            get => Data.GetByte(_weapon2Special2Addr);
            set => Data.SetByte(_weapon2Special2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special3Addr), displayOrder: 82, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon2Special3 {
            get => Data.GetByte(_weapon2Special3Addr);
            set => Data.SetByte(_weapon2Special3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special1Addr), displayOrder: 83, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special1 {
            get => Data.GetByte(_weapon3Special1Addr);
            set => Data.SetByte(_weapon3Special1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special2Addr), displayOrder: 84, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special2 {
            get => Data.GetByte(_weapon3Special2Addr);
            set => Data.SetByte(_weapon3Special2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special3Addr), displayOrder: 85, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon3Special3 {
            get => Data.GetByte(_weapon3Special3Addr);
            set => Data.SetByte(_weapon3Special3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special1Addr), displayOrder: 86, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special1 {
            get => Data.GetByte(_weapon4Special1Addr);
            set => Data.SetByte(_weapon4Special1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special2Addr), displayOrder: 87, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special2 {
            get => Data.GetByte(_weapon4Special2Addr);
            set => Data.SetByte(_weapon4Special2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special3Addr), displayOrder: 88, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Weapon4Special3 {
            get => Data.GetByte(_weapon4Special3Addr);
            set => Data.SetByte(_weapon4Special3Addr, (byte) value);
        }

        // ==============================
        // Stats 2
        // ==============================

        [TableViewModelColumn(addressField: nameof(_baseLuckAddr), displayOrder: 89, displayName: "Luck", displayGroup: "Stats2")]
        [BulkCopy]
        public int BaseLuck {
            get => Data.GetByte(_baseLuckAddr);
            set => Data.SetByte(_baseLuckAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_baseMovAddr), displayOrder: 90, displayName: "Mov", displayGroup: "Stats2")]
        [BulkCopy]
        public int BaseMov {
            get => Data.GetByte(_baseMovAddr);
            set => Data.SetByte(_baseMovAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_baseTurnsAddr), displayOrder: 91, displayName: "Turns", displayGroup: "Stats2")]
        [BulkCopy]
        public int BaseTurns {
            get => Data.GetByte(_baseTurnsAddr);
            set => Data.SetByte(_baseTurnsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_baseHPRegenAddr), displayOrder: 92, displayName: "HPRegen", displayGroup: "Stats2")]
        [BulkCopy]
        public int BaseHPRegen {
            get => Data.GetByte(_baseHPRegenAddr);
            set => Data.SetByte(_baseHPRegenAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_baseMPRegenAddr), displayOrder: 93, displayName: "MPRegen", displayGroup: "Stats2")]
        [BulkCopy]
        public int BaseMPRegen {
            get => Data.GetByte(_baseMPRegenAddr);
            set => Data.SetByte(_baseMPRegenAddr, (byte) value);
        }

        // ==============================
        // Magic Resistance
        // ==============================

        [TableViewModelColumn(addressField: nameof(_earthResAddr), displayOrder: 94, displayGroup: "MagicRes")]
        [BulkCopy]
        public int EarthRes {
            get => (sbyte) Data.GetByte(_earthResAddr);
            set => Data.SetByte(_earthResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_fireResAddr), displayOrder: 95, displayGroup: "MagicRes")]
        [BulkCopy]
        public int FireRes {
            get => (sbyte) Data.GetByte(_fireResAddr);
            set => Data.SetByte(_fireResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_iceResAddr), displayOrder: 96, displayGroup: "MagicRes")]
        [BulkCopy]
        public int IceRes {
            get => (sbyte) Data.GetByte(_iceResAddr);
            set => Data.SetByte(_iceResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sparkResAddr), displayOrder: 97, displayGroup: "MagicRes")]
        [BulkCopy]
        public int SparkRes {
            get => (sbyte) Data.GetByte(_sparkResAddr);
            set => Data.SetByte(_sparkResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_windResAddr), displayOrder: 98, displayGroup: "MagicRes")]
        [BulkCopy]
        public int WindRes {
            get => (sbyte) Data.GetByte(_windResAddr);
            set => Data.SetByte(_windResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lightResAddr), displayOrder: 99, displayGroup: "MagicRes")]
        [BulkCopy]
        public int LightRes {
            get => (sbyte) Data.GetByte(_lightResAddr);
            set => Data.SetByte(_lightResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_darkResAddr), displayOrder: 100, displayGroup: "MagicRes")]
        [BulkCopy]
        public int DarkRes {
            get => (sbyte) Data.GetByte(_darkResAddr);
            set => Data.SetByte(_darkResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknownResAddr), displayOrder: 101, displayGroup: "MagicRes")]
        [BulkCopy]
        public int UnknownRes {
            get => (sbyte) Data.GetByte(_unknownResAddr);
            set => Data.SetByte(_unknownResAddr, (byte) value);
        }

        // ==============================
        // Stats 2 (2/2)
        // ==============================

        [TableViewModelColumn(addressField: nameof(_slowAddr), displayOrder: 102, displayName: "SlowPlus", displayGroup: "Stats2")]
        [BulkCopy]
        public int Slow {
            get => Data.GetByte(_slowAddr);
            set => Data.SetByte(_slowAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_supportAddr), displayOrder: 102, displayName: "SupportPlus", displayGroup: "Stats2")]
        [BulkCopy]
        public int Support {
            get => Data.GetByte(_supportAddr);
            set => Data.SetByte(_supportAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_magicBonusIdAddr), displayOrder: 103, displayFormat: "X2", minWidth: 120, displayGroup: "Stats2")]
        [BulkCopy]
        [NameGetter(NamedValueType.MagicBonus)]
        public int MagicBonusID {
            get => Data.GetByte(_magicBonusIdAddr);
            set => Data.SetByte(_magicBonusIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_movementTypeAddr), displayOrder: 104, displayFormat: "X2", minWidth: 100, displayGroup: "Stats2")]
        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public int MovementType {
            get => Data.GetByte(_movementTypeAddr);
            set => Data.SetByte(_movementTypeAddr, (byte) value);
        }

        // ==============================
        // Equipment
        // ==============================

        [TableViewModelColumn(addressField: nameof(_weaponEquipable1Addr), displayOrder: 105, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable1 {
            get => Data.GetByte(_weaponEquipable1Addr);
            set => Data.SetByte(_weaponEquipable1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable2Addr), displayOrder: 106, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable2 {
            get => Data.GetByte(_weaponEquipable2Addr);
            set => Data.SetByte(_weaponEquipable2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable3Addr), displayOrder: 107, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable3 {
            get => Data.GetByte(_weaponEquipable3Addr);
            set => Data.SetByte(_weaponEquipable3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable4Addr), displayOrder: 108, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int WeaponEquipable4 {
            get => Data.GetByte(_weaponEquipable4Addr);
            set => Data.SetByte(_weaponEquipable4Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable1Addr), displayOrder: 109, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable1 {
            get => Data.GetByte(_accessoryEquipable1Addr);
            set => Data.SetByte(_accessoryEquipable1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable2Addr), displayOrder: 110, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable2 {
            get => Data.GetByte(_accessoryEquipable2Addr);
            set => Data.SetByte(_accessoryEquipable2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable3Addr), displayOrder: 111, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable3 {
            get => Data.GetByte(_accessoryEquipable3Addr);
            set => Data.SetByte(_accessoryEquipable3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable4Addr), displayOrder: 112, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public int AccessoryEquipable4 {
            get => Data.GetByte(_accessoryEquipable4Addr);
            set => Data.SetByte(_accessoryEquipable4Addr, (byte) value);
        }

        // ==============================
        // Curve Calc 1
        // ==============================

        [TableViewModelColumn(addressField: null, displayOrder: 200, displayGroup: "CurveCalc1")] public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 201, displayGroup: "CurveCalc1")] public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 202, displayGroup: "CurveCalc1")] public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 203, displayGroup: "CurveCalc1")] public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 204, displayGroup: "CurveCalc1")] public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 205, displayGroup: "CurveCalc1")] public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 210, displayGroup: "CurveCalc1")] public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 211, displayGroup: "CurveCalc1")] public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 212, displayGroup: "CurveCalc1")] public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 213, displayGroup: "CurveCalc1")] public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 214, displayGroup: "CurveCalc1")] public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 215, displayGroup: "CurveCalc1")] public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 220, displayGroup: "CurveCalc1")] public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 221, displayGroup: "CurveCalc1")] public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 222, displayGroup: "CurveCalc1")] public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 223, displayGroup: "CurveCalc1")] public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 224, displayGroup: "CurveCalc1")] public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 225, displayGroup: "CurveCalc1")] public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 230, displayGroup: "CurveCalc1")] public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 231, displayGroup: "CurveCalc1")] public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 232, displayGroup: "CurveCalc1")] public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 233, displayGroup: "CurveCalc1")] public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 234, displayGroup: "CurveCalc1")] public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 235, displayGroup: "CurveCalc1")] public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 240, displayGroup: "CurveCalc1")] public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 241, displayGroup: "CurveCalc1")] public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 242, displayGroup: "CurveCalc1")] public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 243, displayGroup: "CurveCalc1")] public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 244, displayGroup: "CurveCalc1")] public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 245, displayGroup: "CurveCalc1")] public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);

        // ==============================
        // Curve Calc 2
        // ==============================

        [TableViewModelColumn(addressField: null, displayOrder: 300, displayGroup: "CurveCalc2")] public string HP1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 301, displayGroup: "CurveCalc2")] public string HP2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 302, displayGroup: "CurveCalc2")] public string HP3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 303, displayGroup: "CurveCalc2")] public string HP4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 304, displayGroup: "CurveCalc2")] public string HP5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 305, displayGroup: "CurveCalc2")] public string HP6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

    }
}
