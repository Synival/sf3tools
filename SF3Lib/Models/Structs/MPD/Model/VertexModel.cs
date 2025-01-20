using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class VertexModel : Struct {
        public VertexModel(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x0C) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, minWidth: 75)]
        public FIXED X {
            get => Data.GetFIXED(Address + 0x00);
            set => Data.SetFIXED(Address + 0x00, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, minWidth: 75)]
        public FIXED Y {
            get => Data.GetFIXED(Address + 0x04);
            set => Data.SetFIXED(Address + 0x04, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, minWidth: 75)]
        public FIXED Z {
            get => Data.GetFIXED(Address + 0x08);
            set => Data.SetFIXED(Address + 0x08, value);
        }
    }
}
