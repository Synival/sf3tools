using System;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct, IActor {
        private readonly int _battleAddrEnemyBase;
        private readonly int _battleAddrPlayerBase;

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
        private readonly int _defaultAIIndex;
        private readonly int _facingIsBossAddr;
        private readonly int _respawnCountAddr;
        private readonly int _teamIdAddr;

        private readonly int _cond1ZoneAddr;
        private readonly int _cond1TypeAddr;
        private readonly int _cond1AIIndex1Addr;
        private readonly int _cond1AIIndex2Addr;

        private readonly int _cond2ZoneAddr;
        private readonly int _cond2TypeAddr;
        private readonly int _cond2AIIndex1Addr;
        private readonly int _cond2AIIndex2Addr;

        private readonly int _cond3ZoneAddr;
        private readonly int _cond3TypeAddr;
        private readonly int _cond3AIIndex1Addr;
        private readonly int _cond3AIIndex2Addr;

        private readonly int _cond4ZoneAddr;
        private readonly int _cond4TypeAddr;
        private readonly int _cond4AIIndex1Addr;
        private readonly int _cond4AIIndex2Addr;

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

        public Slot(IByteData data, int id, string name, int address, ScenarioType scenario, Slot prevSlot, BattleMapPointerTable battles, MapLeaderType mapLeader)
        : base(data, id, name, address, 0x34) {
            Scenario  = scenario;
            _prevSlot = prevSlot;
            Battles   = battles;
            MapLeader = mapLeader;

            _battleAddrEnemyBase  = GetBattleAddrBase(scenario);
            _battleAddrPlayerBase = _battleAddrEnemyBase + 0x3C * 0xB0;

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
            _defaultAIIndex         = Address + 0x0F; // 1 byte
            _facingIsBossAddr       = Address + 0x10; // 1 byte
            _teamIdAddr             = Address + 0x11; // 1 byte
            _respawnCountAddr       = Address + 0x12; // 1 byte
            _cond1ZoneAddr          = Address + 0x13; // 1 byte
            _cond1TypeAddr          = Address + 0x14; // 1 byte
            _cond1AIIndex1Addr      = Address + 0x15; // 1 byte
            _cond1AIIndex2Addr      = Address + 0x16; // 1 byte
            _cond2ZoneAddr          = Address + 0x17; // 1 byte
            _cond2TypeAddr          = Address + 0x18; // 1 byte
            _cond2AIIndex1Addr      = Address + 0x19; // 1 byte
            _cond2AIIndex2Addr      = Address + 0x1A; // 1 byte
            _cond3ZoneAddr          = Address + 0x1B; // 1 byte
            _cond3TypeAddr          = Address + 0x1C; // 1 byte
            _cond3AIIndex1Addr      = Address + 0x1D; // 1 byte
            _cond3AIIndex2Addr      = Address + 0x1E; // 1 byte
            _cond4ZoneAddr          = Address + 0x1F; // 1 byte
            _cond4TypeAddr          = Address + 0x20; // 1 byte
            _cond4AIIndex1Addr      = Address + 0x21; // 1 byte
            _cond4AIIndex2Addr      = Address + 0x22; // 1 byte
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
        public BattleMapPointerTable Battles { get; }
        public MapLeaderType MapLeader { get; }

        public Slot _prevSlot;
        public Slot PrevSlot {
            get {
                if (_prevSlot != null)
                    return _prevSlot;
                for (int i = (int) MapLeader - 1; i >= 0; i--) {
                    var battleMap = Battles[i].BattleMap;
                    if (battleMap != null)
                        return battleMap.SlotTable.Rows[battleMap.SlotTable.Size - 1];
                }
                return null;
            }
        }

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

        public int SpriteID {
            get => IsEnemy ? EnemyID + 0xC8 : (EnemyID == 0x5B) ? CharacterPlus : -1;
            set {}
        }

        public int BattleID =>
            IsEnemy ? BattleIDEnemyCounter : (EnemyID == 0x5B) ? CharacterPlus : -1;

        [TableViewModelColumn(addressField: null, displayOrder: 0.5f, displayName: nameof(SpriteID), displayGroup: "Metadata", displayFormat: "X2")]
        public string SpriteIDStr =>
            (SpriteID < 0) ? "--" : SpriteID.ToString("X2");

        [TableViewModelColumn(addressField: null, displayOrder: 0.7f, displayName: nameof(BattleID), displayGroup: "Metadata", displayFormat: "X2")]
        public string BattleIDStr =>
            (BattleID < 0) ? "--" : BattleID.ToString("X2");

        public int BattleAddress {
            get {
                var battleId = BattleID;
                return (battleId < 0) ? 0 : (battleId % 0x80 * 0xB0 + (battleId >= 0x80 ? _battleAddrEnemyBase : _battleAddrPlayerBase));
            }
        }

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

        [TableViewModelColumn(addressField: nameof(_defaultAIIndex), displayOrder: 10, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int DefaultAIIndex {
            get => Data.GetByte(_defaultAIIndex);
            set => Data.SetByte(_defaultAIIndex, (byte) value);
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

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 12.5f, displayGroup: "Page2")]
        [BulkCopy]
        public bool IgnoreConditions {
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

        [TableViewModelColumn(addressField: nameof(_cond1TypeAddr), displayOrder: 16, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1Type {
            get => Data.GetByte(_cond1TypeAddr);
            set => Data.SetByte(_cond1TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond1AIIndex1Addr), displayOrder: 17, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1AIIndex1 {
            get => Data.GetByte(_cond1AIIndex1Addr);
            set => Data.SetByte(_cond1AIIndex1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond1AIIndex2Addr), displayOrder: 18, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond1AIIndex2 {
            get => Data.GetByte(_cond1AIIndex2Addr);
            set => Data.SetByte(_cond1AIIndex2Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond2ZoneAddr), displayOrder: 19, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2Zone {
            get => Data.GetByte(_cond2ZoneAddr);
            set => Data.SetByte(_cond2ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2TypeAddr), displayOrder: 20, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2Type {
            get => Data.GetByte(_cond2TypeAddr);
            set => Data.SetByte(_cond2TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2AIIndex1Addr), displayOrder: 21, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2AIIndex1 {
            get => Data.GetByte(_cond2AIIndex1Addr);
            set => Data.SetByte(_cond2AIIndex1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond2AIIndex2Addr), displayOrder: 22, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond2AIIndex2 {
            get => Data.GetByte(_cond2AIIndex2Addr);
            set => Data.SetByte(_cond2AIIndex2Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond3ZoneAddr), displayOrder: 23, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3Zone {
            get => Data.GetByte(_cond3ZoneAddr);
            set => Data.SetByte(_cond3ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3TypeAddr), displayOrder: 24, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3Type {
            get => Data.GetByte(_cond3TypeAddr);
            set => Data.SetByte(_cond3TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3AIIndex1Addr), displayOrder: 25, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3AIIndex1 {
            get => Data.GetByte(_cond3AIIndex1Addr);
            set => Data.SetByte(_cond3AIIndex1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond3AIIndex2Addr), displayOrder: 26, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond3AIIndex2 {
            get => Data.GetByte(_cond3AIIndex2Addr);
            set => Data.SetByte(_cond3AIIndex2Addr, (byte) value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_cond4ZoneAddr), displayOrder: 27, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4Zone {
            get => Data.GetByte(_cond4ZoneAddr);
            set => Data.SetByte(_cond4ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4TypeAddr), displayOrder: 28, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4Type {
            get => Data.GetByte(_cond4TypeAddr);
            set => Data.SetByte(_cond4TypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4AIIndex1Addr), displayOrder: 29, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4AIIndex1 {
            get => Data.GetByte(_cond4AIIndex1Addr);
            set => Data.SetByte(_cond4AIIndex1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cond4AIIndex2Addr), displayOrder: 30, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Cond4AIIndex2 {
            get => Data.GetByte(_cond4AIIndex2Addr);
            set => Data.SetByte(_cond4AIIndex2Addr, (byte) value);
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
        public bool PrioritizeHealing {
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
                bool hasBattleID = PrioritizeTargetSpecified || PrioritizeHealing || UnknownBattleIDFlag0x0400 || UnknownBattleIDFlag0x1000;

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

        public bool HasActorY => false;
        public float ActorX { get => X * 32 + 16; set => X = (int) Math.Round((value - 16) / 32); }
        public float ActorY { get => 0; set {} }
        public float ActorZ { get => Z * 32 + 16; set => Z = (int) Math.Round((value - 16) / 32); }

        public float ActorDirection {
            get {
                switch (Facing) {
                    case SlotFacingType.South:     return -180.0f;
                    case SlotFacingType.Southwest: return -135.0f;
                    case SlotFacingType.West:      return  -90.0f;
                    case SlotFacingType.Northwest: return  -45.0f;
                    case SlotFacingType.North:     return    0.0f;
                    case SlotFacingType.Northeast: return   45.0f;
                    case SlotFacingType.East:      return   90.0f;
                    case SlotFacingType.Southeast: return  135.0f;
                    default:                       return    0.0f;
                }
            }
            set {
                value = MathHelpers.ActualMod(value + 180.0f, 360.0f) - 180.0f;
                     if (value < -157.5f) Facing = SlotFacingType.South;
                else if (value < -112.5f) Facing = SlotFacingType.Southwest;
                else if (value <  -67.5f) Facing = SlotFacingType.West;
                else if (value <  -22.5f) Facing = SlotFacingType.Northwest;
                else if (value <   22.5f) Facing = SlotFacingType.North;
                else if (value <   67.5f) Facing = SlotFacingType.Northeast;
                else if (value <  112.5f) Facing = SlotFacingType.East;
                else if (value <  157.5f) Facing = SlotFacingType.Southeast;
                else                      Facing = SlotFacingType.South;
            }
        }
    }
}
