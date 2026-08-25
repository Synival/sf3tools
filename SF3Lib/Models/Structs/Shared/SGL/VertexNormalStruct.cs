using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared.SGL {
    public class VertexNormalStruct : Struct {
        private readonly int _xAddr;
        private readonly int _yAddr;
        private readonly int _zAddr;

        public VertexNormalStruct(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x06) {
            _xAddr = Address + 0x00; // 2 bytes
            _yAddr = Address + 0x02; // 2 bytes
            _zAddr = Address + 0x04; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_xAddr), displayOrder: 0, minWidth: 75)]
        public float X {
            get => Data.GetCompressedFIXED(_xAddr).Float;
            set => Data.SetCompressedFIXED(_xAddr, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_yAddr), displayOrder: 1, minWidth: 75)]
        public float Y {
            get => Data.GetCompressedFIXED(_yAddr).Float;
            set => Data.SetCompressedFIXED(_yAddr, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 2, minWidth: 75)]
        public float Z {
            get => Data.GetCompressedFIXED(_zAddr).Float;
            set => Data.SetCompressedFIXED(_zAddr, new CompressedFIXED(value, 0));
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
