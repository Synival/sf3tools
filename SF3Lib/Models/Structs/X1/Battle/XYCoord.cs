using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class XYCoord : Struct {
        private readonly int _xAddr;
        private readonly int _zAddr;

        public XYCoord(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _xAddr = Address + 0x00; // 2 bytes
            _zAddr = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_xAddr), displayOrder: 0, minWidth: 60)]
        [BulkCopy]
        public int X {
            get => Data.GetWord(_xAddr);
            set => Data.SetWord(_xAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 1, minWidth: 60)]
        [BulkCopy]
        public int Z {
            get => Data.GetWord(_zAddr);
            set => Data.SetWord(_zAddr, value);
        }
    }
}
