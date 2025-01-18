using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class Boundaries : Struct {
        private readonly int _cameraX1Addr;
        private readonly int _cameraY1Addr;
        private readonly int _cameraX2Addr;
        private readonly int _cameraY2Addr;

        private readonly int _battleX1Addr;
        private readonly int _battleY1Addr;
        private readonly int _battleX2Addr;
        private readonly int _battleY2Addr;

        public Boundaries(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _cameraX1Addr = address + 0x00; // 2 bytes
            _cameraY1Addr = address + 0x02; // 2 bytes
            _cameraX2Addr = address + 0x04; // 2 bytes
            _cameraY2Addr = address + 0x06; // 2 bytes

            _battleX1Addr = address + 0x08; // 2 bytes
            _battleY1Addr = address + 0x0A; // 2 bytes
            _battleX2Addr = address + 0x0C; // 2 bytes
            _battleY2Addr = address + 0x0E; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "CameraX1", displayOrder: 0)]
        public short CameraX1 {
            get => (short) Data.GetWord(_cameraX1Addr);
            set => Data.SetWord(_cameraX1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "CameraY1", displayOrder: 1)]
        public short CameraY1 {
            get => (short) Data.GetWord(_cameraY1Addr);
            set => Data.SetWord(_cameraY1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "CameraX2", displayOrder: 2)]
        public short CameraX2 {
            get => (short) Data.GetWord(_cameraX2Addr);
            set => Data.SetWord(_cameraX2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "CameraY2", displayOrder: 3)]
        public short CameraY2 {
            get => (short) Data.GetWord(_cameraY2Addr);
            set => Data.SetWord(_cameraY2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "BattleX1", displayOrder: 4)]
        public short BattleX1 {
            get => (short) Data.GetWord(_battleX1Addr);
            set => Data.SetWord(_battleX1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "BattleY1", displayOrder: 5)]
        public short BattleY1 {
            get => (short) Data.GetWord(_battleY1Addr);
            set => Data.SetWord(_battleY1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "BattleX2", displayOrder: 6)]
        public short BattleX2 {
            get => (short) Data.GetWord(_battleX2Addr);
            set => Data.SetWord(_battleX2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "BattleY2", displayOrder: 7)]
        public short BattleY2 {
            get => (short) Data.GetWord(_battleY2Addr);
            set => Data.SetWord(_battleY2Addr, value);
        }
    }
}
