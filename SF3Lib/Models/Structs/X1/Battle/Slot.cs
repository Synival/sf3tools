using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct {
        private readonly int _enemyIDAddr;
        private readonly int _xAddr;
        private readonly int _yAddr;
        private readonly int _itemOverrideAddr;
        private readonly int _dropDisableAddr;
        private readonly int _unknown0x09Addr;
        private readonly int _characterPlusAddr;
        private readonly int _spawnConditionAddr;
        private readonly int _eventCallAddr;
        private readonly int _unknown0x0EAddr;
        private readonly int _unknown0x0FAddr;
        private readonly int _facingIsBossAddr;
        private readonly int _unknown0x12Addr;
        private readonly int _controlTypeAddr;
        private readonly int _spawnZone1Addr;
        private readonly int _spawnZone1ConditionAddr;
        private readonly int _spawnZone2Addr;
        private readonly int _spawnZone2ConditionAddr;
        private readonly int _strictAIAddr;
        private readonly int _unknown0x18Addr;
        private readonly int _unknown0x19Addr;
        private readonly int _unknown0x1AAddr;
        private readonly int _unknown0x1BAddr;
        private readonly int _unknown0x1CAddr;
        private readonly int _unknown0x1DAddr;
        private readonly int _unknown0x1EAddr;
        private readonly int _noTurnSkipAddr;
        private readonly int _unknown0x20Addr;
        private readonly int _unknown0x21Addr;
        private readonly int _unknown0x22Addr;
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
        private readonly int _unknown0x2FAddr;
        private readonly int _unknown0x30Addr;
        private readonly int _unknown0x31Addr;
        private readonly int _flagsAddr;

        public Slot(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x34) {
            _enemyIDAddr        = Address + 0x00; // 2 bytes  
            _xAddr              = Address + 0x02; // 2 bytes
            _yAddr              = Address + 0x04; // 2 bytes
            _itemOverrideAddr   = Address + 0x06; // 2 bytes
            _dropDisableAddr    = Address + 0x08; // 1 byte
            _unknown0x09Addr    = Address + 0x09; // 1 byte
            _eventCallAddr      = Address + 0x0A; // 2 bytes
            _characterPlusAddr  = Address + 0x0C; // 1 byte
            _spawnConditionAddr = Address + 0x0D; // 1 byte
            _unknown0x0EAddr    = Address + 0x0E; // 1 byte
            _unknown0x0FAddr    = Address + 0x0F; // 1 byte
            _facingIsBossAddr   = Address + 0x10; // 1 byte
            _controlTypeAddr    = Address + 0x11; // 1 byte
            _unknown0x12Addr    = Address + 0x12; // 1 byte
            _spawnZone1Addr       = Address + 0x13; // 1 byte
            _spawnZone1ConditionAddr       = Address + 0x14; // 1 byte
            _spawnZone2Addr       = Address + 0x15; // 1 byte
            _spawnZone2ConditionAddr       = Address + 0x16; // 1 byte
            _strictAIAddr       = Address + 0x17; // 1 byte
            _unknown0x18Addr    = Address + 0x18; // 1 byte
            _unknown0x19Addr    = Address + 0x19; // 1 byte
            _unknown0x1AAddr    = Address + 0x1A; // 1 byte
            _unknown0x1BAddr    = Address + 0x1B; // 1 byte
            _unknown0x1CAddr    = Address + 0x1C; // 1 byte
            _unknown0x1DAddr    = Address + 0x1D; // 1 byte
            _unknown0x1EAddr    = Address + 0x1E; // 1 byte
            _noTurnSkipAddr     = Address + 0x1F; // 1 byte
            _unknown0x20Addr    = Address + 0x20; // 1 byte
            _unknown0x21Addr    = Address + 0x21; // 1 byte
            _unknown0x22Addr    = Address + 0x22; // 1 byte
            _aiTag1Addr         = Address + 0x23; // 1 byte
            _aiType1Addr        = Address + 0x24; // 1 byte
            _aiAggr1Addr        = Address + 0x25; // 1 byte
            _aiTag2Addr         = Address + 0x26; // 1 byte
            _aiType2Addr        = Address + 0x27; // 1 byte
            _aiAggr2Addr        = Address + 0x28; // 1 byte
            _aiTag3Addr         = Address + 0x29; // 1 byte
            _aiType3Addr        = Address + 0x2A; // 1 byte
            _aiAggr3Addr        = Address + 0x2B; // 1 byte
            _aiTag4Addr         = Address + 0x2C; // 1 byte
            _aiType4Addr        = Address + 0x2D; // 1 byte
            _aiAggr4Addr        = Address + 0x2E; // 1 byte
            _unknown0x2FAddr    = Address + 0x2F; // 1 byte
            _unknown0x30Addr    = Address + 0x30; // 1 byte
            _unknown0x31Addr    = Address + 0x31; // 1 byte
            _flagsAddr          = Address + 0x32; // 2 bytes
        }

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

        [TableViewModelColumn(displayOrder: 8, displayName: "SpawnCondition?", displayFormat: "X2", displayGroup: "Page1")]
        [BulkCopy]
        public int SpawnCondition {
            get => Data.GetByte(_spawnConditionAddr);
            set => Data.SetByte(_spawnConditionAddr, (byte) value);
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

        [TableViewModelColumn(displayOrder: 10, displayName: "+0x0F", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x0F {
            get => Data.GetByte(_unknown0x0FAddr);
            set => Data.SetByte(_unknown0x0FAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "Facing/IsBoss", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int FacingIsBoss {
            get => Data.GetByte(_facingIsBossAddr);
            set => Data.SetByte(_facingIsBossAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 12, displayGroup: "Page2")]
        public bool IsBoss {
            get => Data.GetBit(_facingIsBossAddr, 5);
            set => Data.SetBit(_facingIsBossAddr, 5, value);
        }

        [TableViewModelColumn(displayOrder: 13, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int ControlType {
            get => Data.GetByte(_controlTypeAddr);
            set => Data.SetByte(_controlTypeAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 14, displayName: "+0x12", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x12 {
            get => Data.GetByte(_unknown0x12Addr);
            set => Data.SetByte(_unknown0x12Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 15, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int SpawnZone1 {
            get => Data.GetByte(_spawnZone1Addr);
            set => Data.SetByte(_spawnZone1Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 16, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int SpawnZone1Condition {
            get => Data.GetByte(_spawnZone1ConditionAddr);
            set => Data.SetByte(_spawnZone1ConditionAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 17, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int SpawnZone2 {
            get => Data.GetByte(_spawnZone2Addr);
            set => Data.SetByte(_spawnZone2Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 18, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int SpawnZone2Condition {
            get => Data.GetByte(_spawnZone2ConditionAddr);
            set => Data.SetByte(_spawnZone2ConditionAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 19, displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int StrictAI {
            get => Data.GetByte(_strictAIAddr);
            set => Data.SetByte(_strictAIAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 20, displayName: "+0x18", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x18 {
            get => Data.GetByte(_unknown0x18Addr);
            set => Data.SetByte(_unknown0x18Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 21, displayName: "+0x19", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x19 {
            get => Data.GetByte(_unknown0x19Addr);
            set => Data.SetByte(_unknown0x19Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 22, displayName: "+0x1A", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x1A {
            get => Data.GetByte(_unknown0x1AAddr);
            set => Data.SetByte(_unknown0x1AAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 23, displayName: "+0x1B", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x1B {
            get => Data.GetByte(_unknown0x1BAddr);
            set => Data.SetByte(_unknown0x1BAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 24, displayName: "+0x1C", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x1C {
            get => Data.GetByte(_unknown0x1CAddr);
            set => Data.SetByte(_unknown0x1CAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 25, displayName: "+0x1D", displayFormat: "X2", displayGroup: "Page2")]
        [BulkCopy]
        public int Unknown0x1D {
            get => Data.GetByte(_unknown0x1DAddr);
            set => Data.SetByte(_unknown0x1DAddr, (byte) value);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Page 3
        // ------------------------------------------------------------------------------------------------------------

        [TableViewModelColumn(displayOrder: 26, displayName: "+0x1E", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Unknown0x1E {
            get => Data.GetByte(_unknown0x1EAddr);
            set => Data.SetByte(_unknown0x1EAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 27, displayName: "!TurnSkip", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int NoTurnSkip {
            get => Data.GetByte(_noTurnSkipAddr);
            set => Data.SetByte(_noTurnSkipAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 28, displayName: "+0x20", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Unknown0x20 {
            get => Data.GetByte(_unknown0x20Addr);
            set => Data.SetByte(_unknown0x20Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 29, displayName: "+0x21", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Unknown0x21 {
            get => Data.GetByte(_unknown0x21Addr);
            set => Data.SetByte(_unknown0x21Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 30, displayName: "+0x22", displayFormat: "X2", displayGroup: "Page3")]
        [BulkCopy]
        public int Unknown0x22 {
            get => Data.GetByte(_unknown0x22Addr);
            set => Data.SetByte(_unknown0x22Addr, (byte) value);
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

        [TableViewModelColumn(displayOrder: 43, displayName: "+0x2F", displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int Unknown0x2F {
            get => Data.GetByte(_unknown0x2FAddr);
            set => Data.SetByte(_unknown0x2FAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 44, displayName: "+0x30", displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int Unknown0x30 {
            get => Data.GetByte(_unknown0x30Addr);
            set => Data.SetByte(_unknown0x30Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 45, displayName: "+0x31", displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int Unknown0x31 {
            get => Data.GetByte(_unknown0x31Addr);
            set => Data.SetByte(_unknown0x31Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 46, displayFormat: "X2", displayGroup: "Page4")]
        [BulkCopy]
        public int Flags {
            get => Data.GetWord(_flagsAddr);
            set => Data.SetWord(_flagsAddr, value);
        }
    }
}
