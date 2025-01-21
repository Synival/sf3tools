using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class PolygonModel : Struct {
        private readonly int _normalXAddr;
        private readonly int _normalYAddr;
        private readonly int _normalZAddr;
        private readonly int _vertex1Addr;
        private readonly int _vertex2Addr;
        private readonly int _vertex3Addr;
        private readonly int _vertex4Addr;

        public PolygonModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x14) {
            _normalXAddr = Address + 0x00; // 4 bytes
            _normalYAddr = Address + 0x04; // 4 bytes
            _normalZAddr = Address + 0x08; // 4 bytes
            _vertex1Addr = Address + 0x0C; // 2 bytes
            _vertex2Addr = Address + 0x0E; // 2 bytes
            _vertex3Addr = Address + 0x10; // 2 bytes
            _vertex4Addr = Address + 0x12; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, minWidth: 75)]
        public FIXED NormalX {
            get => Data.GetFIXED(_normalXAddr);
            set => Data.SetFIXED(_normalXAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, minWidth: 75)]
        public FIXED NormalY {
            get => Data.GetFIXED(_normalYAddr);
            set => Data.SetFIXED(_normalYAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, minWidth: 75)]
        public FIXED NormalZ {
            get => Data.GetFIXED(_normalZAddr);
            set => Data.SetFIXED(_normalZAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, displayFormat: "X4")]
        public ushort Vertex1 {
            get => (ushort) Data.GetWord(_vertex1Addr);
            set => Data.SetWord(_vertex1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, displayFormat: "X4")]
        public ushort Vertex2 {
            get => (ushort) Data.GetWord(_vertex2Addr);
            set => Data.SetWord(_vertex2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5, displayFormat: "X4")]
        public ushort Vertex3 {
            get => (ushort) Data.GetWord(_vertex3Addr);
            set => Data.SetWord(_vertex3Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 6, displayFormat: "X4")]
        public ushort Vertex4 {
            get => (ushort) Data.GetWord(_vertex4Addr);
            set => Data.SetWord(_vertex4Addr, value);
        }
    }
}
