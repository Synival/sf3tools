using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct {
        private readonly int _unknown0x00Addr;
        private readonly int _numSlotsAddr;
        private readonly int _unknown0x02Addr;
        private readonly int _numSpawnZonesAddr;
        private readonly int _unknown0x04Addr;
        private readonly int _numAITargetsAddr;
        private readonly int _unknown0x06Addr;
        private readonly int _numScriptedMovementsAddr;
        private readonly int _unknown0x08Addr;
        private readonly int _unknown0x09Addr;

        public BattleHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0A) {
            _unknown0x00Addr          = Address + 0x00; // 1 byte
            _numSlotsAddr             = Address + 0x01; // 1 byte
            _unknown0x02Addr          = Address + 0x02; // 1 byte
            _numSpawnZonesAddr        = Address + 0x03; // 1 byte
            _unknown0x04Addr          = Address + 0x04; // 1 byte
            _numAITargetsAddr         = Address + 0x05; // 1 byte
            _unknown0x06Addr          = Address + 0x06; // 1 byte
            _numScriptedMovementsAddr = Address + 0x07; // 1 byte
            _unknown0x08Addr          = Address + 0x08; // 1 byte
            _unknown0x09Addr          = Address + 0x09; // 1 byte
        }

        [TableViewModelColumn(displayName: "+0x00", displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x00 {
            get => Data.GetByte(_unknown0x00Addr);
            set => Data.SetByte(_unknown0x00Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int NumSlots {
            get => Data.GetByte(_numSlotsAddr);
            set => Data.SetByte(_numSlotsAddr, (byte) value);
        }

        [TableViewModelColumn(displayName: "+0x02", displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x02 {
            get => Data.GetByte(_unknown0x02Addr);
            set => Data.SetByte(_unknown0x02Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int NumSpawnZones {
            get => Data.GetByte(_numSpawnZonesAddr);
            set => Data.SetByte(_numSpawnZonesAddr, (byte) value);
        }

        [TableViewModelColumn(displayName: "+0x04", displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x04 {
            get => Data.GetByte(_unknown0x04Addr);
            set => Data.SetByte(_unknown0x04Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int NumAITargets {
            get => Data.GetByte(_numAITargetsAddr);
            set => Data.SetByte(_numAITargetsAddr, (byte) value);
        }

        [TableViewModelColumn(displayName: "+0x06", displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x06 {
            get => Data.GetByte(_unknown0x06Addr);
            set => Data.SetByte(_unknown0x06Addr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int NumScriptedMovements {
            get => Data.GetByte(_numScriptedMovementsAddr);
            set => Data.SetByte(_numScriptedMovementsAddr, (byte) value);
        }

        [TableViewModelColumn(displayName: "+0x08", displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x08 {
            get => Data.GetByte(_unknown0x08Addr);
            set => Data.SetByte(_unknown0x08Addr, (byte) value);
        }

        [TableViewModelColumn(displayName: "+0x09", displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x09 {
            get => Data.GetByte(_unknown0x09Addr);
            set => Data.SetByte(_unknown0x09Addr, (byte) value);
        }
    }
}
