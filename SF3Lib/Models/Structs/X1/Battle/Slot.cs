using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct {
        private readonly int _battleAddrBase;

        private readonly int _enemyIDAddr;
        private readonly int _xAddr;
        private readonly int _zAddr;

        private readonly int _itemOverrideAddr;
        private readonly int _dropDisableAddr;
        private readonly int _unknown0x09Addr;
        private readonly int _characterPlusAddr;
        private readonly int _spawnTypeAddr;
        private readonly int _eventCallAddr;
        private readonly int _unknown0x0EAddr;
        private readonly int _creepUpWhenOutOfRangeAddr;
        private readonly int _facingIsBossAddr;
        private readonly int _respawnCountAddr;
        private readonly int _teamIdAddr;

        private readonly int _cond1ZoneAddr;
        private readonly int _cond1MvmtAddr;
        private readonly int _cond1UnknownAddr;
        private readonly int _cond1AIIndexAddr;

        private readonly int _cond2ZoneAddr;
        private readonly int _cond2MvmtAddr;
        private readonly int _cond2UnknownAddr;
        private readonly int _cond2AIIndexAddr;

        private readonly int _cond3ZoneAddr;
        private readonly int _cond3MvmtAddr;
        private readonly int _cond3UnknownAddr;
        private readonly int _cond3AIIndexAddr;

        private readonly int _cond4ZoneAddr;
        private readonly int _cond4MvmtAddr;
        private readonly int _cond4UnknownAddr;
        private readonly int _cond4AIIndexAddr;

        private readonly int _ai1TagAddr;
        private readonly int _ai1TypeAddr;
        private readonly int _ai1AggrAddr;

        private readonly int _ai2TagAddr;
        private readonly int _ai2TypeAddr;
        private readonly int _ai2AggrAddr;

        private readonly int _ai3TagAddr;
        private readonly int _ai3TypeAddr;
        private readonly int _ai3AggrAddr;

        private readonly int _ai4TagAddr;
        private readonly int _ai4TypeAddr;
        private readonly int _ai4AggrAddr;

        private readonly int _paddingAddr;

        private readonly int _flagsAddr;
        private readonly int _flagTieInAddr;

        public Slot(IByteData data, int id, string name, int address, ScenarioType scenario, Slot prevSlot)
        : base(data, id, name, address, 0x34) {
            Scenario = scenario;
            PrevSlot = prevSlot;
            _battleAddrBase = GetBattleAddrBase(scenario);

            _enemyIDAddr            = Address + 0x00; // 2 bytes  
            _xAddr                  = Address + 0x02; // 2 bytes
            _zAddr                  = Address + 0x04; // 2 bytes
            _itemOverrideAddr       = Address + 0x06; // 2 bytes
            _dropDisableAddr        = Address + 0x08; // 1 byte
            _unknown0x09Addr        = Address + 0x09; // 1 byte
            _eventCallAddr          = Address + 0x0A; // 2 bytes
            _characterPlusAddr      = Address + 0x0C; // 1 byte
            _spawnTypeAddr          = Address + 0x0D; // 1 byte
            _unknown0x0EAddr        = Address + 0x0E; // 1 byte
            _creepUpWhenOutOfRangeAddr = Address + 0x0F; // 1 byte
            _facingIsBossAddr       = Address + 0x10; // 1 byte
            _teamIdAddr             = Address + 0x11; // 1 byte
            _respawnCountAddr       = Address + 0x12; // 1 byte
            _cond1ZoneAddr          = Address + 0x13; // 1 byte
            _cond1MvmtAddr          = Address + 0x14; // 1 byte
            _cond1UnknownAddr       = Address + 0x15; // 1 byte
            _cond1AIIndexAddr       = Address + 0x16; // 1 byte
            _cond2ZoneAddr          = Address + 0x17; // 1 byte
            _cond2MvmtAddr          = Address + 0x18; // 1 byte
            _cond2UnknownAddr       = Address + 0x19; // 1 byte
            _cond2AIIndexAddr       = Address + 0x1A; // 1 byte
            _cond3ZoneAddr          = Address + 0x1B; // 1 byte
            _cond3MvmtAddr          = Address + 0x1C; // 1 byte
            _cond3UnknownAddr       = Address + 0x1D; // 1 byte
            _cond3AIIndexAddr       = Address + 0x1E; // 1 byte
            _cond4ZoneAddr          = Address + 0x1F; // 1 byte
            _cond4MvmtAddr          = Address + 0x20; // 1 byte
            _cond4UnknownAddr       = Address + 0x21; // 1 byte
            _cond4AIIndexAddr       = Address + 0x22; // 1 byte
            _ai1TagAddr             = Address + 0x23; // 1 byte
            _ai1TypeAddr            = Address + 0x24; // 1 byte
            _ai1AggrAddr            = Address + 0x25; // 1 byte
            _ai2TagAddr             = Address + 0x26; // 1 byte
            _ai2TypeAddr            = Address + 0x27; // 1 byte
            _ai2AggrAddr            = Address + 0x28; // 1 byte
            _ai3TagAddr             = Address + 0x29; // 1 byte
            _ai3TypeAddr            = Address + 0x2A; // 1 byte
            _ai3AggrAddr            = Address + 0x2B; // 1 byte
            _ai4TagAddr             = Address + 0x2C; // 1 byte
            _ai4TypeAddr            = Address + 0x2D; // 1 byte
            _ai4AggrAddr            = Address + 0x2E; // 1 byte
            _paddingAddr            = Address + 0x2F; // 1 byte
            _flagsAddr              = Address + 0x30; // 2 bytes
            _flagTieInAddr          = Address + 0x32; // 2 bytes
        }

        private int GetBattleAddrBase(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    return 0x0602f6f0;
                case ScenarioType.Scenario2:
                    return 0x06030970;
                case ScenarioType.Scenario3:
                    return 0x060312A0;
                case ScenarioType.PremiumDisk:
                    return 0x06030980;
                default:
                    return 0;
            }
        }

        public ScenarioType Scenario { get; }
        public Slot PrevSlot { get; }

        // ------------------------------------------------------------------------------------------------------------
        // Page 1
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_enemyIDAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2", displayGroup: "Metadata")]
        [BulkCopy]
        [NameGetter(NamedValueType.MonsterForSlot)]
        public int EnemyID {
            get => Data.GetWord(_enemyIDAddr);
            set => Data.SetWord(_enemyIDAddr, value);
        }

        public bool IsEnemy =>
            EnemyID >= 0x01 && EnemyID < 0x8000 && EnemyID != 0x5B;

        public int BattleIDEnemyCounter
            => PrevSlot == null ? 0x80 : PrevSlot.BattleIDEnemyCounter + (PrevSlot.IsEnemy ? 1 : 0);

        public int BattleID =>
            IsEnemy ? BattleIDEnemyCounter : (EnemyID == 0x5B) ? CharacterPlus : -1;

        [TableViewModelColumn(addressField: null, displayOrder: 0.7f, displayName: "Battle ID", displayGroup: "Metadata", displayFormat: "X2")]
        public string BattleIDStr =>
            (BattleID < 0) ? "--" : BattleID.ToString("X2");

        public int BattleAddress =>
            (BattleID < 0) ? 0 : (BattleID - 0x80) * 0xB0 + _battleAddrBase;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8f, displayName: "Battle Address", displayGroup: "Metadata", isPointer: true)]
        public string BattleAddressStr =>
            (BattleID < 0) ? "--" : BattleAddress.ToString("X6");

        [TableViewModelColumn(addressField: nameof(_xAddr), displayOrder: 1, displayGroup: "Page1", minWidth: 60)]
        [BulkCopy]
        public int X {
            get => Data.GetWord(_xAddr);
            set => Data.SetWord(_xAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 2, displayGroup: "Page1", minWidth: 60)]
        [BulkCopy]
        public int Z {
            get => Data.GetWord(_zAddr);
            set => Data.SetWord(_zAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemOverrideAddr), displayOrder: 3, minWidth: 150, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemOverride {
            get => Data.GetWord(_itemOverrideAddr);
            set => Data.SetWord(_itemOverrideAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropDisableAddr), displayOrder: 4, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int DropDisable {
            get => Data.GetByte(_dropDisableAddr);
            set => Data.SetByte(_dropDisableAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x09Addr), displayOrder: 5, displayName: "+0x09 (probably drop rate override)", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int Unknown0x09 {
            get => Data.GetByte(_unknown0x09Addr);
            set => Data.SetByte(_unknown0x09Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_eventCallAddr), displayOrder: 6, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int EventCall {
            get => Data.GetWord(_eventCallAddr);
            set => Data.SetWord(_eventCallAddr, value);
        }

        public NamedValueType? CharacterPlusType
            => (EnemyID == 0x5B) ? NamedValueType.Character : (NamedValueType?) null;

        [TableViewModelColumn(addressField: nameof(_characterPlusAddr), displayOrder: 7, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(CharacterPlusType))]
        public int CharacterPlus {
            get => Data.GetByte(_characterPlusAddr);
            set => Data.SetByte(_characterPlusAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spawnTypeAddr), displayOrder: 8, displayFormat: "X2", minWidth: 230, displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpawnType)]
        public int SpawnType {
            get => Data.GetByte(_spawnTypeAddr);
            set => Data.SetByte(_spawnTypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0EAddr), displayOrder: 9, displayName: "+0x0E", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int Unknown0x0E {
            get => Data.GetByte(_unknown0x0EAddr);
            set => Data.SetByte(_unknown0x0EAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 2
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_creepUpWhenOutOfRangeAddr), displayOrder: 10, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int CreepUpWhenOutOfRange {
            get => Data.GetByte(_creepUpWhenOutOfRangeAddr);
            set => Data.SetByte(_creepUpWhenOutOfRangeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 11, displayName: "Facing/IsBoss", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int FacingIsBoss {
            get => Data.GetByte(_facingIsBossAddr);
            set => Data.SetByte(_facingIsBossAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 11.5f, minWidth: 80, displayGroup: "Page2")]
        public SlotFacingType Facing {
            get => (SlotFacingType) (FacingIsBoss & 0xE0);
            set => FacingIsBoss = (FacingIsBoss & ~0xE0) | ((int) value & 0xE0);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 12, displayGroup: "Page2")]
        public bool IsBoss {
            get => Data.GetBit(_facingIsBossAddr, 5);
            set => Data.SetBit(_facingIsBossAddr, 5, value);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 12.5f, displayName: "Unknown Flag 0x40", displayGroup: "Page2")]
        [BulkCopy]
        public bool UnknownFacingFlag0x40 {
            get => Data.GetBit(_facingIsBossAddr, 3);
            set => Data.SetBit(_facingIsBossAddr, 3, value);
        }

        [TableViewModelColumn(addressField: nameof(_teamIdAddr), displayOrder: 13, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int TeamID {
            get => Data.GetByte(_teamIdAddr);
            set => Data.SetByte(_teamIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_respawnCountAddr), displayOrder: 14, displayName: "RespawnCount?", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int RespawnCount {
            get => Data.GetByte(_respawnCountAddr);
            set => Data.SetByte(_respawnCountAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 3
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond1ZoneAddr), displayOrder: 15, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1Zone {
            get => Data.GetByte(_cond1ZoneAddr);
            set => Data.SetByte(_cond1ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond1MvmtAddr), displayOrder: 16, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1Mvmt {
            get => Data.GetByte(_cond1MvmtAddr);
            set => Data.SetByte(_cond1MvmtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond1UnknownAddr), displayOrder: 17, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1Unknown {
            get => Data.GetByte(_cond1UnknownAddr);
            set => Data.SetByte(_cond1UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond1AIIndexAddr), displayOrder: 18, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1AIIndex {
            get => Data.GetByte(_cond1AIIndexAddr);
            set => Data.SetByte(_cond1AIIndexAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond2ZoneAddr), displayOrder: 19, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2Zone {
            get => Data.GetByte(_cond2ZoneAddr);
            set => Data.SetByte(_cond2ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2MvmtAddr), displayOrder: 20, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2Mvmt {
            get => Data.GetByte(_cond2MvmtAddr);
            set => Data.SetByte(_cond2MvmtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2UnknownAddr), displayOrder: 21, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2Unknown {
            get => Data.GetByte(_cond2UnknownAddr);
            set => Data.SetByte(_cond2UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2AIIndexAddr), displayOrder: 22, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2AIIndex {
            get => Data.GetByte(_cond2AIIndexAddr);
            set => Data.SetByte(_cond2AIIndexAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond3ZoneAddr), displayOrder: 23, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3Zone {
            get => Data.GetByte(_cond3ZoneAddr);
            set => Data.SetByte(_cond3ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3MvmtAddr), displayOrder: 24, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3Mvmt {
            get => Data.GetByte(_cond3MvmtAddr);
            set => Data.SetByte(_cond3MvmtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3UnknownAddr), displayOrder: 25, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3Unknown {
            get => Data.GetByte(_cond3UnknownAddr);
            set => Data.SetByte(_cond3UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3AIIndexAddr), displayOrder: 26, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3AIIndex {
            get => Data.GetByte(_cond3AIIndexAddr);
            set => Data.SetByte(_cond3AIIndexAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond4ZoneAddr), displayOrder: 27, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4Zone {
            get => Data.GetByte(_cond4ZoneAddr);
            set => Data.SetByte(_cond4ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4MvmtAddr), displayOrder: 28, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4Mvmt {
            get => Data.GetByte(_cond4MvmtAddr);
            set => Data.SetByte(_cond4MvmtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4UnknownAddr), displayOrder: 29, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4Unknown {
            get => Data.GetByte(_cond4UnknownAddr);
            set => Data.SetByte(_cond4UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4AIIndexAddr), displayOrder: 30, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4AIIndex {
            get => Data.GetByte(_cond4AIIndexAddr);
            set => Data.SetByte(_cond4AIIndexAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 4
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_ai1TagAddr), displayOrder: 31, displayFormat: "X2", displayGroup: "Page4", minWidth: 100)]
        [NameGetter(NamedValueType.AITargetType)]
        [BulkCopy]
        public int AI1Tag {
            get => Data.GetByte(_ai1TagAddr);
            set => Data.SetByte(_ai1TagAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai1TypeAddr), displayOrder: 32, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI1Type {
            get => Data.GetByte(_ai1TypeAddr);
            set => Data.SetByte(_ai1TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai1AggrAddr), displayOrder: 33, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI1Aggr {
            get => Data.GetByte(_ai1AggrAddr);
            set => Data.SetByte(_ai1AggrAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_ai2TagAddr), displayOrder: 34, displayFormat: "X2", displayGroup: "Page4", minWidth: 100)]
        [NameGetter(NamedValueType.AITargetType)]
        [BulkCopy]
        public int AI2Tag {
            get => Data.GetByte(_ai2TagAddr);
            set => Data.SetByte(_ai2TagAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai2TypeAddr), displayOrder: 35, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI2Type {
            get => Data.GetByte(_ai2TypeAddr);
            set => Data.SetByte(_ai2TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai2AggrAddr), displayOrder: 36, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI2Aggr {
            get => Data.GetByte(_ai2AggrAddr);
            set => Data.SetByte(_ai2AggrAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_ai3TagAddr), displayOrder: 37, displayFormat: "X2", displayGroup: "Page4", minWidth: 100)]
        [NameGetter(NamedValueType.AITargetType)]
        [BulkCopy]
        public int AI3Tag {
            get => Data.GetByte(_ai3TagAddr);
            set => Data.SetByte(_ai3TagAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai3TypeAddr), displayOrder: 38, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI3Type {
            get => Data.GetByte(_ai3TypeAddr);
            set => Data.SetByte(_ai3TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai3AggrAddr), displayOrder: 39, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI3Aggr {
            get => Data.GetByte(_ai3AggrAddr);
            set => Data.SetByte(_ai3AggrAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_ai4TagAddr), displayOrder: 40, displayFormat: "X2", displayGroup: "Page4", minWidth: 100)]
        [NameGetter(NamedValueType.AITargetType)]
        [BulkCopy]
        public int AI4Tag {
            get => Data.GetByte(_ai4TagAddr);
            set => Data.SetByte(_ai4TagAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai4TypeAddr), displayOrder: 41, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI4Type {
            get => Data.GetByte(_ai4TypeAddr);
            set => Data.SetByte(_ai4TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_ai4AggrAddr), displayOrder: 42, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AI4Aggr {
            get => Data.GetByte(_ai4AggrAddr);
            set => Data.SetByte(_ai4AggrAddr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [BulkCopy]
        public int Padding {
            get => Data.GetByte(_paddingAddr);
            set => Data.SetByte(_paddingAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 5
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45, displayName: "EnemyFlags", displayFormat: "X4", displayGroup: "Page5")]
        [BulkCopy]
        public ushort Flags {
            get => (ushort) Data.GetWord(_flagsAddr);
            set => Data.SetWord(_flagsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.1f, displayGroup: "Page5")]
        public bool DontMove {
            get => (Flags & 0x0002) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0002) : (Flags & ~0x0002));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.12f, displayGroup: "Page5")]
        public bool DontMoveIfFlagOff {
            get => (Flags & 0x0008) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0008) : (Flags & ~0x0008));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.15f, displayGroup: "Page5")]
        public bool PrioritizeTargetSpecified {
            get => (Flags & 0x0010) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0010) : (Flags & ~0x0010));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.2f, displayGroup: "Page5")]
        public bool NoTurn {
            get => (Flags & 0x0040) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0040) : (Flags & ~0x0040));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.3f, displayGroup: "Page5")]
        public bool DontGetMoreAggroWhenHurt {
            get => (Flags & 0x0080) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0080) : (Flags & ~0x0080));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.32f, displayGroup: "Page5")]
        public bool UnknownBattleIDFlag0x0200 {
            get => (Flags & 0x0200) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0200) : (Flags & ~0x0200));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.33f, displayGroup: "Page5")]
        public bool UnknownBattleIDFlag0x0400 {
            get => (Flags & 0x0400) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0400) : (Flags & ~0x0400));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.34f, displayGroup: "Page5")]
        public bool UnknownBattleIDFlag0x1000 {
            get => (Flags & 0x1000) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x1000) : (Flags & ~0x1000));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.4f, displayGroup: "Page5")]
        public bool CreepTowardsMoveTarget {
            get => (Flags & 0x4000) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x4000) : (Flags & ~0x4000));
        }

        public bool IsBarrel => EnemyID == 0x5F;

        public NamedValueType? FlagOrBattleIDType {
            get {
                bool hasFlag = IsBarrel || DontMoveIfFlagOff;
                bool hasBattleID = PrioritizeTargetSpecified || UnknownBattleIDFlag0x0200 || UnknownBattleIDFlag0x0400 || UnknownBattleIDFlag0x1000;

                if (hasFlag && !hasBattleID)
                    return NamedValueType.GameFlag;
                else if (!hasFlag && hasBattleID)
                    return NamedValueType.Character;
                else
                    return null;
            }
        }

        [TableViewModelColumn(addressField: nameof(_flagTieInAddr), displayOrder: 46, displayName: "Flag / Battle ID", displayFormat: "X3", minWidth: 200, displayGroup: "Page5")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(FlagOrBattleIDType))]
        public int FlagOrBattleID {
            get => Data.GetWord(_flagTieInAddr);
            set => Data.SetWord(_flagTieInAddr, value);
        }
    }
}
