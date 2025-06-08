using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class VertexModel : Struct {
        private readonly int _xAddr;
        private readonly int _yAddr;
        private readonly int _zAddr;

        public VertexModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
            _xAddr = Address + 0x00; // 4 bytes
            _yAddr = Address + 0x04; // 4 bytes
            _zAddr = Address + 0x08; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_xAddr), displayOrder: 0, minWidth: 75)]
        public float X {
            get => Data.GetFIXED(_xAddr).Float;
            set => Data.SetFIXED(_xAddr, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_yAddr), displayOrder: 1, minWidth: 75)]
        public float Y {
            get => Data.GetFIXED(_yAddr).Float;
            set => Data.SetFIXED(_yAddr, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 2, minWidth: 75)]
        public float Z {
            get => Data.GetFIXED(_zAddr).Float;
            set => Data.SetFIXED(_zAddr, new FIXED(value, 0));
        }

        public VECTOR Vector {
            get => new VECTOR(X, Y, Z);
            set {
                X = value.X.Float;
                Y = value.Y.Float;
                Z = value.Z.Float;
            }
        }
    }
}
