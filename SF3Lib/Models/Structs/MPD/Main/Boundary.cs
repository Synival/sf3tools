using CommonLib.Attributes;
using CommonLib.Geometry;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Main {
    public class Boundary : Struct, IRectangleShort {
        private readonly int _x1Addr;
        private readonly int _y1Addr;
        private readonly int _x2Addr;
        private readonly int _y2Addr;

        public Boundary(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x8) {
            _x1Addr = address + 0x00; // 2 bytes
            _y1Addr = address + 0x02; // 2 bytes
            _x2Addr = address + 0x04; // 2 bytes
            _y2Addr = address + 0x06; // 2 bytes

            _p1 = new MockPoint(data, _x1Addr, _y1Addr);
            _p2 = new MockPoint(data, _x2Addr, _y2Addr);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_x1Addr), displayOrder: 0, minWidth: 60)]
        public short X1 {
            get => Data.GetInt16(_x1Addr);
            set => Data.SetInt16(_x1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_y1Addr), displayOrder: 1, minWidth: 60)]
        public short Y1 {
            get => Data.GetInt16(_y1Addr);
            set => Data.SetInt16(_y1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_x2Addr), displayOrder: 2, minWidth: 60)]
        public short X2 {
            get => Data.GetInt16(_x2Addr);
            set => Data.SetInt16(_x2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_y2Addr), displayOrder: 3, minWidth: 60)]
        public short Y2 {
            get => Data.GetInt16(_y2Addr);
            set => Data.SetInt16(_y2Addr, value);
        }

        private struct MockPoint : IPointShort {
            public MockPoint(IByteData data, int offsetX, int offsetY) {
                _data    = data;
                _offsetX = offsetX;
                _offsetY = offsetY;
            }

            public short X {
                get => _data.GetInt16(_offsetX);
                set => _data.SetInt16(_offsetX, value);
            }

            public short Y {
                get => _data.GetInt16(_offsetY);
                set => _data.SetInt16(_offsetY, value);
            }

            private readonly IByteData _data;
            private readonly int _offsetX;
            private readonly int _offsetY;
        }

        private MockPoint _p1;
        private MockPoint _p2;

        public IPointShort P1 { get => new PointShort(_p1); set { _p1.X = value.X; _p1.Y = value.Y; } }
        public IPointShort P2 { get => new PointShort(_p2); set { _p2.X = value.X; _p2.Y = value.Y; } }

        IPointBase<short> IRectangleBase<short, int>.P1 { get => new PointShort(_p1); set { _p1.X = value.X; _p1.Y = value.Y; } }
        IPointBase<short> IRectangleBase<short, int>.P2 { get => new PointShort(_p1); set { _p2.X = value.X; _p2.Y = value.Y; } }

        public int Width { get => X2 - X1; set => X2 = (short) (X1 + value); }
        public int Height { get => Y2 - Y1; set => Y2 = (short) (Y1 + value); }
    }
}
