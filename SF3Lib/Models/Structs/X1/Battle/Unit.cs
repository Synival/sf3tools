using System;
using CommonLib.Attributes;
using CommonLib.Utils;
using SF3.Actors;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Unit : Struct, IActor {
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

        private readonly int _aiCond1Addr;
        private readonly int _aiCond2Addr;
        private readonly int _aiCond3Addr;
        private readonly int _aiCond4Addr;

        private readonly int _aiOrder1Addr;
        private readonly int _aiOrder2Addr;
        private readonly int _aiOrder3Addr;
        private readonly int _aiOrder4Addr;

        private readonly int _paddingAddr;

        private readonly int _flagsAddr;
        private readonly int _flagTieInAddr;

        public Unit(IByteData data, int id, string name, int address, ScenarioType scenario, Unit prevUnit, BattleHeader battleHeader, MapLeaderType mapLeader)
        : base(data, id, name, address, 0x34) {
            Scenario     = scenario;
            _prevUnit    = prevUnit;
            BattleHeader = battleHeader;
            MapLeader    = mapLeader;

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
            _aiCond1Addr            = Address + 0x13; // 4 bytes
            _aiCond2Addr            = Address + 0x17; // 4 bytes
            _aiCond3Addr            = Address + 0x1B; // 4 bytes
            _aiCond4Addr            = Address + 0x1F; // 4 bytes
            _aiOrder1Addr           = Address + 0x23; // 3 bytes
            _aiOrder2Addr           = Address + 0x26; // 3 bytes
            _aiOrder3Addr           = Address + 0x29; // 3 bytes
            _aiOrder4Addr           = Address + 0x2C; // 3 bytes
            _paddingAddr            = Address + 0x2F; // 1 byte
            _flagsAddr              = Address + 0x30; // 2 bytes
            _flagTieInAddr          = Address + 0x32; // 2 bytes

            Conditions = new UnitAICondition[] {
                new UnitAICondition(Data, 0, "Condition1", _aiCond1Addr),
                new UnitAICondition(Data, 1, "Condition2", _aiCond2Addr),
                new UnitAICondition(Data, 2, "Condition3", _aiCond3Addr),
                new UnitAICondition(Data, 3, "Condition4", _aiCond4Addr),
            };

            Orders = new UnitAIOrder[] {
                new UnitAIOrder(Data, 0, "Order1", _aiOrder1Addr),
                new UnitAIOrder(Data, 1, "Order2", _aiOrder2Addr),
                new UnitAIOrder(Data, 2, "Order3", _aiOrder3Addr),
                new UnitAIOrder(Data, 3, "Order4", _aiOrder4Addr),
            };
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
        public BattleHeader BattleHeader { get; }
        public MapLeaderType MapLeader { get; }

        public Unit _prevUnit;
        public Unit PrevUnit {
            get {
                if (_prevUnit != null)
                    return _prevUnit;
                for (int i = (int) MapLeader - 1; i >= 0; i--) {
                    var battleMap = BattleHeader.MapPointerTable[i].BattleMap;
                    if (battleMap != null)
                        return battleMap.UnitTable.Rows[battleMap.UnitTable.Size - 1];
                }
                return null;
            }
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 1
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_enemyIDAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2", displayGroup: "Metadata")]
        [BulkCopy]
        [NameGetter(NamedValueType.MonsterForUnit)]
        public ushort EnemyID {
            get => Data.GetUInt16(_enemyIDAddr);
            set => Data.SetUInt16(_enemyIDAddr, value);
        }

        public bool IsEnemy
            => EnemyID >= 0x01 && EnemyID < 0x8000 && EnemyID != 0x5B;

        public int BattleIDEnemyCounter
            => PrevUnit == null ? 0x80 : PrevUnit.BattleIDEnemyCounter + (PrevUnit.IsEnemy ? 1 : 0);

        public ushort SpriteID {
            get => (ushort) (IsEnemy ? EnemyID + 0xC8 : (EnemyID == 0x5B) ? CharacterPlus : -1);
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
        public short X {
            get => Data.GetInt16(_xAddr);
            set => Data.SetInt16(_xAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 2, displayGroup: "Page1", minWidth: 60)]
        [BulkCopy]
        public short Z {
            get => Data.GetInt16(_zAddr);
            set => Data.SetInt16(_zAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemOverrideAddr), displayOrder: 3, minWidth: 150, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort ItemOverride {
            get => Data.GetUInt16(_itemOverrideAddr);
            set => Data.SetUInt16(_itemOverrideAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropDisableAddr), displayOrder: 4, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int DropDisable {
            get => Data.GetUInt8(_dropDisableAddr);
            set => Data.SetUInt8(_dropDisableAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x09Addr), displayOrder: 5, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int DropRate {
            get => Data.GetUInt8(_unknown0x09Addr);
            set => Data.SetUInt8(_unknown0x09Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_eventCallAddr), displayOrder: 6, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public ushort EventCall {
            get => Data.GetUInt16(_eventCallAddr);
            set => Data.SetUInt16(_eventCallAddr, value);
        }

        public NamedValueType? CharacterPlusType
            => (EnemyID == 0x5B) ? NamedValueType.Character : (NamedValueType?) null;

        [TableViewModelColumn(addressField: nameof(_characterPlusAddr), displayOrder: 7, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(CharacterPlusType))]
        public int CharacterPlus {
            get => Data.GetUInt8(_characterPlusAddr);
            set => Data.SetUInt8(_characterPlusAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spawnTypeAddr), displayOrder: 8, displayFormat: "X2", minWidth: 230, displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpawnType)]
        public int SpawnType {
            get => (Data.GetUInt8(_spawnTypeAddr) & 0x7F);
            set => Data.SetUInt8(_spawnTypeAddr, (byte) ((Data.GetUInt8(_spawnTypeAddr) & 0x80) | value));
        }

        [TableViewModelColumn(addressField: nameof(_spawnTypeAddr), displayOrder: 8.1f, displayGroup: "Page1")]
        [BulkCopy]
        public bool CanSpawnNearby {
            get => (Data.GetUInt8(_spawnTypeAddr) & 0x80) == 0x80;
            set => Data.SetUInt8(_spawnTypeAddr, (byte) ((Data.GetUInt8(_spawnTypeAddr) & 0x7F) | (value ? 0x80 : 0x00)));
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0EAddr), displayOrder: 9, displayName: "+0x0E", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int Unknown0x0E {
            get => Data.GetUInt8(_unknown0x0EAddr);
            set => Data.SetUInt8(_unknown0x0EAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 2
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_defaultAIIndex), displayOrder: 10, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int DefaultAIIndex {
            get => Data.GetUInt8(_defaultAIIndex);
            set => Data.SetUInt8(_defaultAIIndex, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 11, displayName: "Facing/IsBoss", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int FacingIsBoss {
            get => Data.GetUInt8(_facingIsBossAddr);
            set => Data.SetUInt8(_facingIsBossAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_facingIsBossAddr), displayOrder: 11.5f, minWidth: 80, displayGroup: "Page2")]
        public UnitFacingType Facing {
            get => (UnitFacingType) (FacingIsBoss & 0xE0);
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
            get => Data.GetUInt8(_teamIdAddr);
            set => Data.SetUInt8(_teamIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_respawnCountAddr), displayOrder: 14, displayName: "RespawnCount?", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int RespawnCount {
            get => Data.GetUInt8(_respawnCountAddr);
            set => Data.SetUInt8(_respawnCountAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 3
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_aiCond1Addr), displayOrder: 15, displayFormat: "X8", displayGroup: "Page3")]
        [BulkCopy]
        public uint AICondition1 {
            get => Data.GetUInt32(_aiCond1Addr);
            set => Data.SetUInt32(_aiCond1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_aiCond2Addr), displayOrder: 16, displayFormat: "X8", displayGroup: "Page3")]
        [BulkCopy]
        public uint AICondition2 {
            get => Data.GetUInt32(_aiCond2Addr);
            set => Data.SetUInt32(_aiCond2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_aiCond3Addr), displayOrder: 17, displayFormat: "X8", displayGroup: "Page3")]
        [BulkCopy]
        public uint AICondition3 {
            get => Data.GetUInt32(_aiCond3Addr);
            set => Data.SetUInt32(_aiCond3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_aiCond4Addr), displayOrder: 18, displayFormat: "X8", displayGroup: "Page3")]
        [BulkCopy]
        public uint AICondition4 {
            get => Data.GetUInt32(_aiCond4Addr);
            set => Data.SetUInt32(_aiCond4Addr, value);
        }

        // -----------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_aiOrder1Addr), displayOrder: 19, displayFormat: "X6", displayGroup: "Page3")]
        [BulkCopy]
        public uint AIOrder1 {
            get => Data.GetData(_aiOrder1Addr, 3);
            set => Data.SetData(_aiOrder1Addr, value, 3);
        }

        [TableViewModelColumn(addressField: nameof(_aiOrder2Addr), displayOrder: 20, displayFormat: "X6", displayGroup: "Page3")]
        [BulkCopy]
        public uint AIOrder2 {
            get => Data.GetData(_aiOrder2Addr, 3);
            set => Data.SetData(_aiOrder2Addr, value, 3);
        }

        [TableViewModelColumn(addressField: nameof(_aiOrder3Addr), displayOrder: 21, displayFormat: "X6", displayGroup: "Page3")]
        [BulkCopy]
        public uint AIOrder3 {
            get => Data.GetData(_aiOrder3Addr, 3);
            set => Data.SetData(_aiOrder3Addr, value, 3);
        }

        [TableViewModelColumn(addressField: nameof(_aiOrder4Addr), displayOrder: 22, displayFormat: "X6", displayGroup: "Page3")]
        [BulkCopy]
        public uint AIOrder4 {
            get => Data.GetData(_aiOrder4Addr, 3);
            set => Data.SetData(_aiOrder4Addr, value, 3);
        }

        // -----------------------------------------------------------------------------------------------------------

        [BulkCopy]
        public int Padding {
            get => Data.GetUInt8(_paddingAddr);
            set => Data.SetUInt8(_paddingAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 4
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45, displayName: "EnemyFlags", displayFormat: "X4", displayGroup: "Page4")]
        [BulkCopy]
        public ushort Flags {
            get => Data.GetUInt16(_flagsAddr);
            set => Data.SetUInt16(_flagsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.01f, displayGroup: "Page4")]
        public bool DontSeekBestTarget {
            get => (Flags & 0x0001) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0001) : (Flags & ~0x0001));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.1f, displayGroup: "Page4")]
        public bool DontMove {
            get => (Flags & 0x0002) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0002) : (Flags & ~0x0002));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.11f, displayName: nameof(PrioritizeLastAttackerUnlessAIBit0x20Set) + " (Unused)", displayGroup: "Page4")]
        public bool PrioritizeLastAttackerUnlessAIBit0x20Set {
            get => (Flags & 0x0004) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0004) : (Flags & ~0x0004));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.12f, displayGroup: "Page4")]
        public bool DontMoveIfFlagOff {
            get => (Flags & 0x0008) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0008) : (Flags & ~0x0008));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.15f, displayGroup: "Page4")]
        public bool PrioritizeFlagTarget {
            get => (Flags & 0x0010) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0010) : (Flags & ~0x0010));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayName: nameof(UnknownFlag0x0020) + " (Unused)", displayOrder: 45.16f, displayGroup: "Page4")]
        public bool UnknownFlag0x0020 {
            get => (Flags & 0x0020) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0020) : (Flags & ~0x0020));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.2f, displayGroup: "Page4")]
        public bool NoTurn {
            get => (Flags & 0x0040) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0040) : (Flags & ~0x0040));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.3f, displayGroup: "Page4")]
        public bool DontGetMoreAggroWhenHurt {
            get => (Flags & 0x0080) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0080) : (Flags & ~0x0080));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayName: nameof(DontSpawnPlayerCharacterIfDead) + " (Unused)", displayOrder: 45.31f, displayGroup: "Page4")]
        public bool DontSpawnPlayerCharacterIfDead {
            get => (Flags & 0x0100) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0100) : (Flags & ~0x0100));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.32f, displayGroup: "Page4")]
        public bool PrioritizeChoiceThenHealing {
            get => (Flags & 0x0200) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0200) : (Flags & ~0x0200));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.33f, displayGroup: "Page4")]
        public bool PrioritizeLastAttackerThenFlagTarget {
            get => (Flags & 0x0400) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0400) : (Flags & ~0x0400));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.34f, displayGroup: "Page4")]
        public bool AttackBestTargetIfLocationOrPathIsUnreachable {
            get => (Flags & 0x0800) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x0800) : (Flags & ~0x0800));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.35f, displayGroup: "Page4")]
        public bool DontInitBaseStats {
            get => (Flags & 0x1000) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x1000) : (Flags & ~0x1000));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayName: nameof(UnknownFlag0x0020) + " (Unused)", displayOrder: 45.16f, displayGroup: "Page4")]
        public bool UnknownFlag0x2000 {
            get => (Flags & 0x2000) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x2000) : (Flags & ~0x0020));
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 45.4f, displayGroup: "Page4")]
        public bool CreepTowardsMoveTarget {
            get => (Flags & 0x4000) != 0;
            set => Flags = (ushort) (value ? (Flags | 0x4000) : (Flags & ~0x4000));
        }

        public bool IsBarrel => EnemyID == 0x5F;

        public NamedValueType? FlagOrBattleIDType {
            get {
                bool hasFlag = IsBarrel || DontMoveIfFlagOff;
                bool hasBattleID = PrioritizeFlagTarget || PrioritizeChoiceThenHealing || PrioritizeLastAttackerThenFlagTarget || DontInitBaseStats;

                if (hasFlag && !hasBattleID)
                    return NamedValueType.GameFlag;
                else if (!hasFlag && hasBattleID)
                    return NamedValueType.Character;
                else
                    return null;
            }
        }

        [TableViewModelColumn(addressField: nameof(_flagTieInAddr), displayOrder: 46, displayName: "Flag / Battle ID", displayFormat: "X3", minWidth: 200, displayGroup: "Page4")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(FlagOrBattleIDType))]
        public ushort FlagOrBattleID {
            get => Data.GetUInt16(_flagTieInAddr);
            set => Data.SetUInt16(_flagTieInAddr, value);
        }

        public bool HasActorY => false;
        public float ActorX { get => X * 32 + 16; set => X = (short) Math.Round((value - 16) / 32); }
        public float ActorY { get => 0; set {} }
        public float ActorZ { get => Z * 32 + 16; set => Z = (short) Math.Round((value - 16) / 32); }

        public float ActorDirection {
            get {
                switch (Facing) {
                    case UnitFacingType.South:     return -180.0f;
                    case UnitFacingType.Southwest: return -135.0f;
                    case UnitFacingType.West:      return  -90.0f;
                    case UnitFacingType.Northwest: return  -45.0f;
                    case UnitFacingType.North:     return    0.0f;
                    case UnitFacingType.Northeast: return   45.0f;
                    case UnitFacingType.East:      return   90.0f;
                    case UnitFacingType.Southeast: return  135.0f;
                    default:                       return    0.0f;
                }
            }
            set {
                value = MathHelpers.ActualMod(value + 180.0f, 360.0f) - 180.0f;
                     if (value < -157.5f) Facing = UnitFacingType.South;
                else if (value < -112.5f) Facing = UnitFacingType.Southwest;
                else if (value <  -67.5f) Facing = UnitFacingType.West;
                else if (value <  -22.5f) Facing = UnitFacingType.Northwest;
                else if (value <   22.5f) Facing = UnitFacingType.North;
                else if (value <   67.5f) Facing = UnitFacingType.Northeast;
                else if (value <  112.5f) Facing = UnitFacingType.East;
                else if (value <  157.5f) Facing = UnitFacingType.Southeast;
                else                      Facing = UnitFacingType.South;
            }
        }

        public readonly UnitAICondition[] Conditions;
        public readonly UnitAIOrder[] Orders;
    }
}
