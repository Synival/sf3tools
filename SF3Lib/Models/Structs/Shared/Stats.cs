using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class Stats : Struct {
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

        public bool IsPromoted => Data.GetUInt8(_characterClassAddr) >= 0x20;

        public PromotionLevelType PromotionLevel {
            get {
                var chClass = Data.GetUInt8(_characterClassAddr);
                return chClass < 0x20 ? PromotionLevelType.Unpromoted :
                       chClass < 0x48 ? PromotionLevelType.Promotion1 :
                                        PromotionLevelType.Promotion2;
            }
        }

        // ==============================
        // Character and Class
        // ==============================

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2", displayGroup: "CharAndClass")]
        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public byte CharacterID {
            get => Data.GetUInt8(_characterIdAddr);
            set => Data.SetUInt8(_characterIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_characterClassAddr), displayOrder: 1, displayName: "Class", minWidth: 150, displayFormat: "X2", displayGroup: "CharAndClass")]
        [BulkCopy]
        [NameGetter(NamedValueType.CharacterClass)]
        public byte CharacterClass {
            get => Data.GetUInt8(_characterClassAddr);
            set => Data.SetUInt8(_characterClassAddr, value);
        }

        // ==============================
        // Stats
        // ==============================

        [TableViewModelColumn(addressField: nameof(_hpPromoteAddr), displayOrder: 2, displayGroup: "Stats")]
        [BulkCopy]
        public byte HPPromote {
            get => Data.GetUInt8(_hpPromoteAddr);
            set => Data.SetUInt8(_hpPromoteAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve1Addr), displayOrder: 3, displayName: "HPLv1", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve1 {
            get => Data.GetUInt8(_hpCurve1Addr);
            set => Data.SetUInt8(_hpCurve1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve5Addr), displayOrder: 4, displayName: "HPLv5", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve5 {
            get => Data.GetUInt8(_hpCurve5Addr);
            set => Data.SetUInt8(_hpCurve5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve10Addr), displayOrder: 5, displayName: "HPLv10", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve10 {
            get => Data.GetUInt8(_hpCurve10Addr);
            set => Data.SetUInt8(_hpCurve10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve12_15Addr), displayOrder: 6, displayName: "HPLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve12_15 {
            get => Data.GetUInt8(_hpCurve12_15Addr);
            set => Data.SetUInt8(_hpCurve12_15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve14_20Addr), displayOrder: 7, displayName: "HPLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve14_20 {
            get => Data.GetUInt8(_hpCurve14_20Addr);
            set => Data.SetUInt8(_hpCurve14_20Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve17_30Addr), displayOrder: 8, displayName: "HPLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve17_30 {
            get => Data.GetUInt8(_hpCurve17_30Addr);
            set => Data.SetUInt8(_hpCurve17_30Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpCurve30_99Addr), displayOrder: 9, displayName: "HPLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public byte HPCurve30_99 {
            get => Data.GetUInt8(_hpCurve30_99Addr);
            set => Data.SetUInt8(_hpCurve30_99Addr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_mpPromoteAddr), displayOrder: 9.5f, displayGroup: "Stats")]
        [BulkCopy]
        public byte MPPromote {
            get => Data.GetUInt8(_mpPromoteAddr);
            set => Data.SetUInt8(_mpPromoteAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve1Addr), displayOrder: 10, displayName: "MPLv1", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve1 {
            get => Data.GetUInt8(_mpCurve1Addr);
            set => Data.SetUInt8(_mpCurve1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve5Addr), displayOrder: 11, displayName: "MPLv5", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve5 {
            get => Data.GetUInt8(_mpCurve5Addr);
            set => Data.SetUInt8(_mpCurve5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve10Addr), displayOrder: 12, displayName: "MPLv10", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve10 {
            get => Data.GetUInt8(_mpCurve10Addr);
            set => Data.SetUInt8(_mpCurve10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve12_15Addr), displayOrder: 13, displayName: "MPLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve12_15 {
            get => Data.GetUInt8(_mpCurve12_15Addr);
            set => Data.SetUInt8(_mpCurve12_15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve14_20Addr), displayOrder: 14, displayName: "MPLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve14_20 {
            get => Data.GetUInt8(_mpCurve14_20Addr);
            set => Data.SetUInt8(_mpCurve14_20Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve17_30Addr), displayOrder: 15, displayName: "MPLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve17_30 {
            get => Data.GetUInt8(_mpCurve17_30Addr);
            set => Data.SetUInt8(_mpCurve17_30Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpCurve30_99Addr), displayOrder: 16, displayName: "MPLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public byte MPCurve30_99 {
            get => Data.GetUInt8(_mpCurve30_99Addr);
            set => Data.SetUInt8(_mpCurve30_99Addr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_atkPromoteAddr), displayOrder: 17, displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkPromote {
            get => Data.GetUInt8(_atkPromoteAddr);
            set => Data.SetUInt8(_atkPromoteAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve1Addr), displayOrder: 18, displayName: "AtkLv1", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve1 {
            get => Data.GetUInt8(_atkCurve1Addr);
            set => Data.SetUInt8(_atkCurve1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve5Addr), displayOrder: 19, displayName: "AtkLv5", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve5 {
            get => Data.GetUInt8(_atkCurve5Addr);
            set => Data.SetUInt8(_atkCurve5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve10Addr), displayOrder: 20, displayName: "AtkLv10", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve10 {
            get => Data.GetUInt8(_atkCurve10Addr);
            set => Data.SetUInt8(_atkCurve10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve12_15Addr), displayOrder: 21, displayName: "AtkLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve12_15 {
            get => Data.GetUInt8(_atkCurve12_15Addr);
            set => Data.SetUInt8(_atkCurve12_15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve14_20Addr), displayOrder: 22, displayName: "AtkLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve14_20 {
            get => Data.GetUInt8(_atkCurve14_20Addr);
            set => Data.SetUInt8(_atkCurve14_20Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve17_30Addr), displayOrder: 23, displayName: "AtkLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve17_30 {
            get => Data.GetUInt8(_atkCurve17_30Addr);
            set => Data.SetUInt8(_atkCurve17_30Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_atkCurve30_99Addr), displayOrder: 24, displayName: "AtkLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public byte AtkCurve30_99 {
            get => Data.GetUInt8(_atkCurve30_99Addr);
            set => Data.SetUInt8(_atkCurve30_99Addr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_defPromoteAddr), displayOrder: 25, displayGroup: "Stats")]
        [BulkCopy]
        public byte DefPromote {
            get => Data.GetUInt8(_defPromoteAddr);
            set => Data.SetUInt8(_defPromoteAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve1Addr), displayOrder: 26, displayName: "DefLv1", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve1 {
            get => Data.GetUInt8(_defCurve1Addr);
            set => Data.SetUInt8(_defCurve1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve5Addr), displayOrder: 27, displayName: "DefLv5", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve5 {
            get => Data.GetUInt8(_defCurve5Addr);
            set => Data.SetUInt8(_defCurve5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve10Addr), displayOrder: 28, displayName: "DefLv10", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve10 {
            get => Data.GetUInt8(_defCurve10Addr);
            set => Data.SetUInt8(_defCurve10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve12_15Addr), displayOrder: 29, displayName: "DefLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve12_15 {
            get => Data.GetUInt8(_defCurve12_15Addr);
            set => Data.SetUInt8(_defCurve12_15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve14_20Addr), displayOrder: 30, displayName: "DefLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve14_20 {
            get => Data.GetUInt8(_defCurve14_20Addr);
            set => Data.SetUInt8(_defCurve14_20Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve17_30Addr), displayOrder: 31, displayName: "DefLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve17_30 {
            get => Data.GetUInt8(_defCurve17_30Addr);
            set => Data.SetUInt8(_defCurve17_30Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defCurve30_99Addr), displayOrder: 32, displayName: "DefLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public byte DefCurve30_99 {
            get => Data.GetUInt8(_defCurve30_99Addr);
            set => Data.SetUInt8(_defCurve30_99Addr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_agiPromoteAddr), displayOrder: 33, displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiPromote {
            get => Data.GetUInt8(_agiPromoteAddr);
            set => Data.SetUInt8(_agiPromoteAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve1Addr), displayOrder: 34, displayName: "AgiLv1", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve1 {
            get => Data.GetUInt8(_agiCurve1Addr);
            set => Data.SetUInt8(_agiCurve1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve5Addr), displayOrder: 35, displayName: "AgiLv5", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve5 {
            get => Data.GetUInt8(_agiCurve5Addr);
            set => Data.SetUInt8(_agiCurve5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve10Addr), displayOrder: 36, displayName: "AgiLv10", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve10 {
            get => Data.GetUInt8(_agiCurve10Addr);
            set => Data.SetUInt8(_agiCurve10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve12_15Addr), displayOrder: 37, displayName: "AgiLv12/15", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve12_15 {
            get => Data.GetUInt8(_agiCurve12_15Addr);
            set => Data.SetUInt8(_agiCurve12_15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve14_20Addr), displayOrder: 38, displayName: "AgiLv14/20", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve14_20 {
            get => Data.GetUInt8(_agiCurve14_20Addr);
            set => Data.SetUInt8(_agiCurve14_20Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve17_30Addr), displayOrder: 39, displayName: "AgiLv17/30", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve17_30 {
            get => Data.GetUInt8(_agiCurve17_30Addr);
            set => Data.SetUInt8(_agiCurve17_30Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agiCurve30_99Addr), displayOrder: 40, displayName: "AgiLv30/99", displayGroup: "Stats")]
        [BulkCopy]
        public byte AgiCurve30_99 {
            get => Data.GetUInt8(_agiCurve30_99Addr);
            set => Data.SetUInt8(_agiCurve30_99Addr, value);
        }

        // ==============================
        // Spells
        // ==============================

        [TableViewModelColumn(addressField: nameof(_s1CharLvAddr), displayOrder: 41, displayGroup: "Spells")]
        [BulkCopy]
        public byte S1CharLv {
            get => Data.GetUInt8(_s1CharLvAddr);
            set => Data.SetUInt8(_s1CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s1SpellIdAddr), displayOrder: 42, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S1SpellID {
            get => Data.GetUInt8(_s1SpellIdAddr);
            set => Data.SetUInt8(_s1SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s1SpellLvAddr), displayOrder: 43, displayGroup: "Spells")]
        [BulkCopy]
        public byte S1SpellLv {
            get => Data.GetUInt8(_s1SpellLvAddr);
            set => Data.SetUInt8(_s1SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s2CharLvAddr), displayOrder: 44, displayGroup: "Spells")]
        [BulkCopy]
        public byte S2CharLv {
            get => Data.GetUInt8(_s2CharLvAddr);
            set => Data.SetUInt8(_s2CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s2SpellIdAddr), displayOrder: 45, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S2SpellID {
            get => Data.GetUInt8(_s2SpellIdAddr);
            set => Data.SetUInt8(_s2SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s2SpellLvAddr), displayOrder: 46, displayGroup: "Spells")]
        [BulkCopy]
        public byte S2SpellLv {
            get => Data.GetUInt8(_s2SpellLvAddr);
            set => Data.SetUInt8(_s2SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s3CharLvAddr), displayOrder: 47, displayGroup: "Spells")]
        [BulkCopy]
        public byte S3CharLv {
            get => Data.GetUInt8(_s3CharLvAddr);
            set => Data.SetUInt8(_s3CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s3SpellIdAddr), displayOrder: 48, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S3SpellID {
            get => Data.GetUInt8(_s3SpellIdAddr);
            set => Data.SetUInt8(_s3SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s3SpellLvAddr), displayOrder: 49, displayGroup: "Spells")]
        [BulkCopy]
        public byte S3SpellLv {
            get => Data.GetUInt8(_s3SpellLvAddr);
            set => Data.SetUInt8(_s3SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s4CharLvAddr), displayOrder: 50, displayGroup: "Spells")]
        [BulkCopy]
        public byte S4CharLv {
            get => Data.GetUInt8(_s4CharLvAddr);
            set => Data.SetUInt8(_s4CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s4SpellIdAddr), displayOrder: 51, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S4SpellID {
            get => Data.GetUInt8(_s4SpellIdAddr);
            set => Data.SetUInt8(_s4SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s4SpellLvAddr), displayOrder: 52, displayGroup: "Spells")]
        [BulkCopy]
        public byte S4SpellLv {
            get => Data.GetUInt8(_s4SpellLvAddr);
            set => Data.SetUInt8(_s4SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s5CharLvAddr), displayOrder: 53, displayGroup: "Spells")]
        [BulkCopy]
        public byte S5CharLv {
            get => Data.GetUInt8(_s5CharLvAddr);
            set => Data.SetUInt8(_s5CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s5SpellIdAddr), displayOrder: 54, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S5SpellID {
            get => Data.GetUInt8(_s5SpellIdAddr);
            set => Data.SetUInt8(_s5SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s5SpellLvAddr), displayOrder: 55, displayGroup: "Spells")]
        [BulkCopy]
        public byte S5SpellLv {
            get => Data.GetUInt8(_s5SpellLvAddr);
            set => Data.SetUInt8(_s5SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s6CharLvAddr), displayOrder: 56, displayGroup: "Spells")]
        [BulkCopy]
        public byte S6CharLv {
            get => Data.GetUInt8(_s6CharLvAddr);
            set => Data.SetUInt8(_s6CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s6SpellIdAddr), displayOrder: 57, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S6SpellID {
            get => Data.GetUInt8(_s6SpellIdAddr);
            set => Data.SetUInt8(_s6SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s6SpellLvAddr), displayOrder: 58, displayGroup: "Spells")]
        [BulkCopy]
        public byte S6SpellLv {
            get => Data.GetUInt8(_s6SpellLvAddr);
            set => Data.SetUInt8(_s6SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s7CharLvAddr), displayOrder: 59, displayGroup: "Spells")]
        [BulkCopy]
        public byte S7CharLv {
            get => Data.GetUInt8(_s7CharLvAddr);
            set => Data.SetUInt8(_s7CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s7SpellIdAddr), displayOrder: 60, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S7SpellID {
            get => Data.GetUInt8(_s7SpellIdAddr);
            set => Data.SetUInt8(_s7SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s7SpellLvAddr), displayOrder: 61, displayGroup: "Spells")]
        [BulkCopy]
        public byte S7SpellLv {
            get => Data.GetUInt8(_s7SpellLvAddr);
            set => Data.SetUInt8(_s7SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s8CharLvAddr), displayOrder: 62, displayGroup: "Spells")]
        [BulkCopy]
        public byte S8CharLv {
            get => Data.GetUInt8(_s8CharLvAddr);
            set => Data.SetUInt8(_s8CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s8SpellIdAddr), displayOrder: 63, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S8SpellID {
            get => Data.GetUInt8(_s8SpellIdAddr);
            set => Data.SetUInt8(_s8SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s8SpellLvAddr), displayOrder: 64, displayGroup: "Spells")]
        [BulkCopy]
        public byte S8SpellLv {
            get => Data.GetUInt8(_s8SpellLvAddr);
            set => Data.SetUInt8(_s8SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s9CharLvAddr), displayOrder: 65, displayGroup: "Spells")]
        [BulkCopy]
        public byte S9CharLv {
            get => Data.GetUInt8(_s9CharLvAddr);
            set => Data.SetUInt8(_s9CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s9SpellIdAddr), displayOrder: 66, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S9SpellID {
            get => Data.GetUInt8(_s9SpellIdAddr);
            set => Data.SetUInt8(_s9SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s9SpellLvAddr), displayOrder: 67, displayGroup: "Spells")]
        [BulkCopy]
        public byte S9SpellLv {
            get => Data.GetUInt8(_s9SpellLvAddr);
            set => Data.SetUInt8(_s9SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s10CharLvAddr), displayOrder: 68, displayGroup: "Spells")]
        [BulkCopy]
        public byte S10CharLv {
            get => Data.GetUInt8(_s10CharLvAddr);
            set => Data.SetUInt8(_s10CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s10SpellIdAddr), displayOrder: 69, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S10SpellID {
            get => Data.GetUInt8(_s10SpellIdAddr);
            set => Data.SetUInt8(_s10SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s10SpellLvAddr), displayOrder: 70, displayGroup: "Spells")]
        [BulkCopy]
        public byte S10SpellLv {
            get => Data.GetUInt8(_s10SpellLvAddr);
            set => Data.SetUInt8(_s10SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s11CharLvAddr), displayOrder: 71, displayGroup: "Spells")]
        [BulkCopy]
        public byte S11CharLv {
            get => Data.GetUInt8(_s11CharLvAddr);
            set => Data.SetUInt8(_s11CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s11SpellIdAddr), displayOrder: 72, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S11SpellID {
            get => Data.GetUInt8(_s11SpellIdAddr);
            set => Data.SetUInt8(_s11SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s11SpellLvAddr), displayOrder: 73, displayGroup: "Spells")]
        [BulkCopy]
        public byte S11SpellLv {
            get => Data.GetUInt8(_s11SpellLvAddr);
            set => Data.SetUInt8(_s11SpellLvAddr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_s12CharLvAddr), displayOrder: 74, displayGroup: "Spells")]
        [BulkCopy]
        public byte S12CharLv {
            get => Data.GetUInt8(_s12CharLvAddr);
            set => Data.SetUInt8(_s12CharLvAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s12SpellIdAddr), displayOrder: 75, displayFormat: "X2", minWidth: 120, displayGroup: "Spells")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte S12SpellID {
            get => Data.GetUInt8(_s12SpellIdAddr);
            set => Data.SetUInt8(_s12SpellIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_s12SpellLvAddr), displayOrder: 76, displayGroup: "Spells")]
        [BulkCopy]
        public byte S12SpellLv {
            get => Data.GetUInt8(_s12SpellLvAddr);
            set => Data.SetUInt8(_s12SpellLvAddr, value);
        }

        // ==============================
        // Specials
        // ==============================

        [TableViewModelColumn(addressField: nameof(_weapon1Special1Addr), displayOrder: 77, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon1Special1 {
            get => Data.GetUInt8(_weapon1Special1Addr);
            set => Data.SetUInt8(_weapon1Special1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1Special2Addr), displayOrder: 78, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon1Special2 {
            get => Data.GetUInt8(_weapon1Special2Addr);
            set => Data.SetUInt8(_weapon1Special2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon1Special3Addr), displayOrder: 79, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon1Special3 {
            get => Data.GetUInt8(_weapon1Special3Addr);
            set => Data.SetUInt8(_weapon1Special3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special1Addr), displayOrder: 80, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon2Special1 {
            get => Data.GetUInt8(_weapon2Special1Addr);
            set => Data.SetUInt8(_weapon2Special1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special2Addr), displayOrder: 81, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon2Special2 {
            get => Data.GetUInt8(_weapon2Special2Addr);
            set => Data.SetUInt8(_weapon2Special2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon2Special3Addr), displayOrder: 82, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon2Special3 {
            get => Data.GetUInt8(_weapon2Special3Addr);
            set => Data.SetUInt8(_weapon2Special3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special1Addr), displayOrder: 83, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon3Special1 {
            get => Data.GetUInt8(_weapon3Special1Addr);
            set => Data.SetUInt8(_weapon3Special1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special2Addr), displayOrder: 84, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon3Special2 {
            get => Data.GetUInt8(_weapon3Special2Addr);
            set => Data.SetUInt8(_weapon3Special2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon3Special3Addr), displayOrder: 85, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon3Special3 {
            get => Data.GetUInt8(_weapon3Special3Addr);
            set => Data.SetUInt8(_weapon3Special3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special1Addr), displayOrder: 86, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon4Special1 {
            get => Data.GetUInt8(_weapon4Special1Addr);
            set => Data.SetUInt8(_weapon4Special1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special2Addr), displayOrder: 87, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon4Special2 {
            get => Data.GetUInt8(_weapon4Special2Addr);
            set => Data.SetUInt8(_weapon4Special2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weapon4Special3Addr), displayOrder: 88, displayFormat: "X2", minWidth: 150, displayGroup: "Specials")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Weapon4Special3 {
            get => Data.GetUInt8(_weapon4Special3Addr);
            set => Data.SetUInt8(_weapon4Special3Addr, value);
        }

        // ==============================
        // Stats 2
        // ==============================

        [TableViewModelColumn(addressField: nameof(_baseLuckAddr), displayOrder: 89, displayName: "Luck", displayGroup: "Stats2")]
        [BulkCopy]
        public byte BaseLuck {
            get => Data.GetUInt8(_baseLuckAddr);
            set => Data.SetUInt8(_baseLuckAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_baseMovAddr), displayOrder: 90, displayName: "Mov", displayGroup: "Stats2")]
        [BulkCopy]
        public byte BaseMov {
            get => Data.GetUInt8(_baseMovAddr);
            set => Data.SetUInt8(_baseMovAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_baseTurnsAddr), displayOrder: 91, displayName: "Turns", displayGroup: "Stats2")]
        [BulkCopy]
        public byte BaseTurns {
            get => Data.GetUInt8(_baseTurnsAddr);
            set => Data.SetUInt8(_baseTurnsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_baseHPRegenAddr), displayOrder: 92, displayName: "HPRegen", displayGroup: "Stats2")]
        [BulkCopy]
        public byte BaseHPRegen {
            get => Data.GetUInt8(_baseHPRegenAddr);
            set => Data.SetUInt8(_baseHPRegenAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_baseMPRegenAddr), displayOrder: 93, displayName: "MPRegen", displayGroup: "Stats2")]
        [BulkCopy]
        public byte BaseMPRegen {
            get => Data.GetUInt8(_baseMPRegenAddr);
            set => Data.SetUInt8(_baseMPRegenAddr, value);
        }

        // ==============================
        // Magic Resistance
        // ==============================

        [TableViewModelColumn(addressField: nameof(_earthResAddr), displayOrder: 94, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte EarthRes {
            get => Data.GetInt8(_earthResAddr);
            set => Data.SetInt8(_earthResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fireResAddr), displayOrder: 95, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte FireRes {
            get => Data.GetInt8(_fireResAddr);
            set => Data.SetInt8(_fireResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_iceResAddr), displayOrder: 96, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte IceRes {
            get => Data.GetInt8(_iceResAddr);
            set => Data.SetInt8(_iceResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sparkResAddr), displayOrder: 97, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte SparkRes {
            get => Data.GetInt8(_sparkResAddr);
            set => Data.SetInt8(_sparkResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_windResAddr), displayOrder: 98, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte WindRes {
            get => Data.GetInt8(_windResAddr);
            set => Data.SetInt8(_windResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lightResAddr), displayOrder: 99, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte LightRes {
            get => Data.GetInt8(_lightResAddr);
            set => Data.SetInt8(_lightResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_darkResAddr), displayOrder: 100, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte DarkRes {
            get => Data.GetInt8(_darkResAddr);
            set => Data.SetInt8(_darkResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknownResAddr), displayOrder: 101, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte UnknownRes {
            get => Data.GetInt8(_unknownResAddr);
            set => Data.SetInt8(_unknownResAddr, value);
        }

        // ==============================
        // Stats 2 (2/2)
        // ==============================

        [TableViewModelColumn(addressField: nameof(_slowAddr), displayOrder: 102, displayName: "SlowPlus", displayGroup: "Stats2")]
        [BulkCopy]
        public byte Slow {
            get => Data.GetUInt8(_slowAddr);
            set => Data.SetUInt8(_slowAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_supportAddr), displayOrder: 102, displayName: "SupportPlus", displayGroup: "Stats2")]
        [BulkCopy]
        public byte Support {
            get => Data.GetUInt8(_supportAddr);
            set => Data.SetUInt8(_supportAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_magicBonusIdAddr), displayOrder: 103, displayFormat: "X2", minWidth: 120, displayGroup: "Stats2")]
        [BulkCopy]
        [NameGetter(NamedValueType.MagicBonus)]
        public byte MagicBonusID {
            get => Data.GetUInt8(_magicBonusIdAddr);
            set => Data.SetUInt8(_magicBonusIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_movementTypeAddr), displayOrder: 104, displayFormat: "X2", minWidth: 100, displayGroup: "Stats2")]
        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public byte MovementType {
            get => Data.GetUInt8(_movementTypeAddr);
            set => Data.SetUInt8(_movementTypeAddr, value);
        }

        // ==============================
        // Equipment
        // ==============================

        [TableViewModelColumn(addressField: nameof(_weaponEquipable1Addr), displayOrder: 105, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte WeaponEquipable1 {
            get => Data.GetUInt8(_weaponEquipable1Addr);
            set => Data.SetUInt8(_weaponEquipable1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable2Addr), displayOrder: 106, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte WeaponEquipable2 {
            get => Data.GetUInt8(_weaponEquipable2Addr);
            set => Data.SetUInt8(_weaponEquipable2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable3Addr), displayOrder: 107, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte WeaponEquipable3 {
            get => Data.GetUInt8(_weaponEquipable3Addr);
            set => Data.SetUInt8(_weaponEquipable3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponEquipable4Addr), displayOrder: 108, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte WeaponEquipable4 {
            get => Data.GetUInt8(_weaponEquipable4Addr);
            set => Data.SetUInt8(_weaponEquipable4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable1Addr), displayOrder: 109, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte AccessoryEquipable1 {
            get => Data.GetUInt8(_accessoryEquipable1Addr);
            set => Data.SetUInt8(_accessoryEquipable1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable2Addr), displayOrder: 110, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte AccessoryEquipable2 {
            get => Data.GetUInt8(_accessoryEquipable2Addr);
            set => Data.SetUInt8(_accessoryEquipable2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable3Addr), displayOrder: 111, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte AccessoryEquipable3 {
            get => Data.GetUInt8(_accessoryEquipable3Addr);
            set => Data.SetUInt8(_accessoryEquipable3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryEquipable4Addr), displayOrder: 112, displayFormat: "X2", minWidth: 120, displayGroup: "Equipment")]
        [BulkCopy]
        [NameGetter(NamedValueType.WeaponType)]
        public byte AccessoryEquipable4 {
            get => Data.GetUInt8(_accessoryEquipable4Addr);
            set => Data.SetUInt8(_accessoryEquipable4Addr, value);
        }
    }
}
