using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class VertexModel : Struct {
        public VertexModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, minWidth: 75)]
        public float X {
            get => Data.GetFIXED(Address + 0x00).Float;
            set => Data.SetFIXED(Address + 0x00, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, minWidth: 75)]
        public float Y {
            get => Data.GetFIXED(Address + 0x04).Float;
            set => Data.SetFIXED(Address + 0x04, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, minWidth: 75)]
        public float Z {
            get => Data.GetFIXED(Address + 0x08).Float;
            set => Data.SetFIXED(Address + 0x08, new FIXED(value, 0));
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
