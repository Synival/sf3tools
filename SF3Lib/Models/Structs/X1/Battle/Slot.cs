using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct {
        private readonly int _battleAddrBase;

        private readonly int _enemyIDAddr;
        private readonly int _xAddr;
        private readonly int _yAddr;
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
        private readonly int _condition1ZoneAddr;
        private readonly int _condition1MovementAddr;
        private readonly int _condition1UnknownAddr;
        private readonly int _condition1AIIndexAddr;
        private readonly int _condition2ZoneAddr;
        private readonly int _condition2MovementAddr;
        private readonly int _condition2UnknownAddr;
        private readonly int _condition2AIIndexAddr;
        private readonly int _condition3ZoneAddr;
        private readonly int _condition3MovementAddr;
        private readonly int _condition3UnknownAddr;
        private readonly int _condition3AIIndexAddr;
        private readonly int _condition4ZoneAddr;
        private readonly int _condition4MovementAddr;
        private readonly int _condition4UnknownAddr;
        private readonly int _condition4AIIndex;
        private readonly int _aiTag1Addr;
        private readonly int _aiType1Addr;
        private readonly int _aiAggr1Addr;
        private readonly int _aiTag2Addr;
        private readonly int _aiType2Addr;
        private readonly int _aiAggr2Addr;
        private readonly int _aiTag3Addr;
        private readonly int _aiType3Addr;
        private readonly int _aiAggr3Addr;
        private readonly int _aiTag4Addr;
        private readonly int _aiType4Addr;
        private readonly int _aiAggr4Addr;
        private readonly int _paddingAddr;
        private readonly int _enemyFlagsAddr;
        private readonly int _flagTieInAddr;

        public Slot(IByteData data, int id, string name, int address, ScenarioType scenario, Slot prevSlot)
        : base(data, id, name, address, 0x34) {
            Scenario = scenario;
            PrevSlot = prevSlot;
            _battleAddrBase = GetBattleAddrBase(scenario);

            _enemyIDAddr            = Address + 0x00; // 2 bytes  
            _xAddr                  = Address + 0x02; // 2 bytes
            _yAddr                  = Address + 0x04; // 2 bytes
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
            _condition1ZoneAddr     = Address + 0x13; // 1 byte
            _condition1MovementAddr = Address + 0x14; // 1 byte
            _condition1UnknownAddr  = Address + 0x15; // 1 byte
            _condition1AIIndexAddr  = Address + 0x16; // 1 byte
            _condition2ZoneAddr     = Address + 0x17; // 1 byte
            _condition2MovementAddr = Address + 0x18; // 1 byte
            _condition2UnknownAddr  = Address + 0x19; // 1 byte
            _condition2AIIndexAddr  = Address + 0x1A; // 1 byte
            _condition3ZoneAddr     = Address + 0x1B; // 1 byte
            _condition3MovementAddr = Address + 0x1C; // 1 byte
            _condition3UnknownAddr  = Address + 0x1D; // 1 byte
            _condition3AIIndexAddr  = Address + 0x1E; // 1 byte
            _condition4ZoneAddr     = Address + 0x1F; // 1 byte
            _condition4MovementAddr = Address + 0x20; // 1 byte
            _condition4UnknownAddr  = Address + 0x21; // 1 byte
            _condition4AIIndex      = Address + 0x22; // 1 byte
            _aiTag1Addr             = Address + 0x23; // 1 byte
            _aiType1Addr            = Address + 0x24; // 1 byte
            _aiAggr1Addr            = Address + 0x25; // 1 byte
            _aiTag2Addr             = Address + 0x26; // 1 byte
            _aiType2Addr            = Address + 0x27; // 1 byte
            _aiAggr2Addr            = Address + 0x28; // 1 byte
            _aiTag3Addr             = Address + 0x29; // 1 byte
            _aiType3Addr            = Address + 0x2A; // 1 byte
            _aiAggr3Addr            = Address + 0x2B; // 1 byte
            _aiTag4Addr             = Address + 0x2C; // 1 byte
            _aiType4Addr            = Address + 0x2D; // 1 byte
            _aiAggr4Addr            = Address + 0x2E; // 1 byte
            _paddingAddr            = Address + 0x2F; // 1 byte
            _enemyFlagsAddr         = Address + 0x30; // 2 bytes
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

        [TableViewModelColumn(displayOrder: 0, minWidth: 150, displayGroup: "Page1")]
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

        [TableViewModelColumn(displayOrder: 0.7f, displayName: "Battle ID", displayGroup: "Page1", displayFormat: "X2")]
        public string BattleIDStr =>
            (BattleID < 0) ? "--" : BattleID.ToString("X2");

        public int BattleAddress =>
            (BattleID < 0) ? 0 : (BattleID - 0x80) * 0xB0 + _battleAddrBase;

        [TableViewModelColumn(displayOrder: 0.8f, displayName: "Battle Address", displayGroup: "Page1", isPointer: true)]
        public string BattleAddressStr =>
            (BattleID < 0) ? "--" : BattleAddress.ToString("X6");

        [TableViewModelColumn(displayOrder: 1, displayGroup: "Page1")]
        [BulkCopy]
        public int EnemyX {
            get => Data.GetWord(_xAddr);
            set => Data.SetWord(_xAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayGroup: "Page1")]
        [BulkCopy]
        public int EnemyY {
            get => Data.GetWord(_yAddr);
            set => Data.SetWord(_yAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, minWidth: 150, displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemOverride {
            get => Data.GetWord(_itemOverrideAddr);
            set => Data.SetWord(_itemOverrideAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int DropDisable {
            get => Data.GetByte(_dropDisableAddr);
            set => Data.SetByte(_dropDisableAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "+0x09 (probably drop rate override)", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int Unknown0x09 {
            get => Data.GetByte(_unknown0x09Addr);
            set => Data.SetByte(_unknown0x09Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int EventCall {
            get => Data.GetWord(_eventCallAddr);
            set => Data.SetWord(_eventCallAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.CharacterPlus, nameof(EnemyID))]
        public int CharacterPlus {
            get => Data.GetByte(_characterPlusAddr);
            set => Data.SetByte(_characterPlusAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X2", minWidth: 230, displayGroup: "Page1")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpawnType)]
        public int SpawnType {
            get => Data.GetByte(_spawnTypeAddr);
            set => Data.SetByte(_spawnTypeAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "+0x0E", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int Unknown0x0E {
            get => Data.GetByte(_unknown0x0EAddr);
            set => Data.SetByte(_unknown0x0EAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 2
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(displayOrder: 10, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int CreepUpWhenOutOfRange {
            get => Data.GetByte(_creepUpWhenOutOfRangeAddr);
            set => Data.SetByte(_creepUpWhenOutOfRangeAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "Facing/IsBoss", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int FacingIsBoss {
            get => Data.GetByte(_facingIsBossAddr);
            set => Data.SetByte(_facingIsBossAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11.5f, minWidth: 80, displayGroup: "Page2")]
        public SlotFacingType Facing {
            get => (SlotFacingType) (FacingIsBoss & 0xE0);
            set => FacingIsBoss = (FacingIsBoss & ~0xE0) | ((int) value & 0xE0);
        }

        [TableViewModelColumn(displayOrder: 12, displayGroup: "Page2")]
        public bool IsBoss {
            get => Data.GetBit(_facingIsBossAddr, 5);
            set => Data.SetBit(_facingIsBossAddr, 5, value);
        }

        [TableViewModelColumn(displayOrder: 13, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int TeamID {
            get => Data.GetByte(_teamIdAddr);
            set => Data.SetByte(_teamIdAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 14, displayName: "RespawnCount?", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int RespawnCount {
            get => Data.GetByte(_respawnCountAddr);
            set => Data.SetByte(_respawnCountAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 3
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(displayOrder: 15, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition1Zone {
            get => Data.GetByte(_condition1ZoneAddr);
            set => Data.SetByte(_condition1ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 16, displayName: "Condition1Movement?", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition1Movement {
            get => Data.GetByte(_condition1MovementAddr);
            set => Data.SetByte(_condition1MovementAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 17, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition1Unknown {
            get => Data.GetByte(_condition1UnknownAddr);
            set => Data.SetByte(_condition1UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 18, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition1AIIndex {
            get => Data.GetByte(_condition1AIIndexAddr);
            set => Data.SetByte(_condition1AIIndexAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 19, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition2Zone {
            get => Data.GetByte(_condition2ZoneAddr);
            set => Data.SetByte(_condition2ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 20, displayName: "Condition2Movement?", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition2Movement {
            get => Data.GetByte(_condition2MovementAddr);
            set => Data.SetByte(_condition2MovementAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 21, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition2Unknown {
            get => Data.GetByte(_condition2UnknownAddr);
            set => Data.SetByte(_condition2UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 22, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition2AIIndex {
            get => Data.GetByte(_condition2AIIndexAddr);
            set => Data.SetByte(_condition2AIIndexAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 23, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition3Zone {
            get => Data.GetByte(_condition3ZoneAddr);
            set => Data.SetByte(_condition3ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 24, displayName: "Condition3Movement?", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition3Movement {
            get => Data.GetByte(_condition3MovementAddr);
            set => Data.SetByte(_condition3MovementAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 25, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition3Unknown {
            get => Data.GetByte(_condition3UnknownAddr);
            set => Data.SetByte(_condition3UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 26, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition3AIIndex {
            get => Data.GetByte(_condition3AIIndexAddr);
            set => Data.SetByte(_condition3AIIndexAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 27, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition4Zone {
            get => Data.GetByte(_condition4ZoneAddr);
            set => Data.SetByte(_condition4ZoneAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 28, displayName: "Condition4Movement?", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition4Movement {
            get => Data.GetByte(_condition4MovementAddr);
            set => Data.SetByte(_condition4MovementAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 29, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition4Unknown {
            get => Data.GetByte(_condition4UnknownAddr);
            set => Data.SetByte(_condition4UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 30, displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Condition4AIIndex {
            get => Data.GetByte(_condition4AIIndex);
            set => Data.SetByte(_condition4AIIndex, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 4
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(displayOrder: 31, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AITag1 {
            get => Data.GetByte(_aiTag1Addr);
            set => Data.SetByte(_aiTag1Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 32, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIType1 {
            get => Data.GetByte(_aiType1Addr);
            set => Data.SetByte(_aiType1Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 33, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIAggr1 {
            get => Data.GetByte(_aiAggr1Addr);
            set => Data.SetByte(_aiAggr1Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 34, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AITag2 {
            get => Data.GetByte(_aiTag2Addr);
            set => Data.SetByte(_aiTag2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 35, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIType2 {
            get => Data.GetByte(_aiType2Addr);
            set => Data.SetByte(_aiType2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 36, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIAggr2 {
            get => Data.GetByte(_aiAggr2Addr);
            set => Data.SetByte(_aiAggr2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 37, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AITag3 {
            get => Data.GetByte(_aiTag3Addr);
            set => Data.SetByte(_aiTag3Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 38, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIType3 {
            get => Data.GetByte(_aiType3Addr);
            set => Data.SetByte(_aiType3Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 39, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIAggr3 {
            get => Data.GetByte(_aiAggr3Addr);
            set => Data.SetByte(_aiAggr3Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 40, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AITag4 {
            get => Data.GetByte(_aiTag4Addr);
            set => Data.SetByte(_aiTag4Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 41, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIType4 {
            get => Data.GetByte(_aiType4Addr);
            set => Data.SetByte(_aiType4Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 42, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int AIAggr4 {
            get => Data.GetByte(_aiAggr4Addr);
            set => Data.SetByte(_aiAggr4Addr, (byte) value);
        }

        [BulkCopy]
        public int Padding {
            get => Data.GetByte(_paddingAddr);
            set => Data.SetByte(_paddingAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 45, displayName: "EnemyFlags", displayFormat: "X4", displayGroup: "Page4")]
        [BulkCopy]
        public ushort EnemyFlags {
            get => (ushort) Data.GetWord(_enemyFlagsAddr);
            set => Data.SetWord(_enemyFlagsAddr, value);
        }

        [TableViewModelColumn(displayOrder: 45.1f, displayGroup: "Page4")]
        public bool DontMove {
            get => (EnemyFlags & 0x0002) != 0;
            set => EnemyFlags = (ushort) (value ? (EnemyFlags | 0x0002) : (EnemyFlags & ~0x0002));
        }

        [TableViewModelColumn(displayOrder: 45.2f, displayGroup: "Page4")]
        public bool NoTurn {
            get => (EnemyFlags & 0x0040) != 0;
            set => EnemyFlags = (ushort) (value ? (EnemyFlags | 0x0040) : (EnemyFlags & ~0x0040));
        }

        [TableViewModelColumn(displayOrder: 45.3f, displayGroup: "Page4")]
        public bool DontGetMoreAggroWhenHurt {
            get => (EnemyFlags & 0x0080) != 0;
            set => EnemyFlags = (ushort) (value ? (EnemyFlags | 0x0080) : (EnemyFlags & ~0x0080));
        }

        [TableViewModelColumn(displayOrder: 46, displayFormat: "X4", displayGroup: "Page4")]
        [BulkCopy]
        public int FlagTieIn {
            get => Data.GetWord(_flagTieInAddr);
            set => Data.SetWord(_flagTieInAddr, value);
        }
    }
}
