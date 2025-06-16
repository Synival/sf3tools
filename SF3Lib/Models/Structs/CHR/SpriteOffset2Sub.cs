using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.CHR {
    public class SpriteOffset2Sub : Struct {
        private readonly int _frameIdAddr;
        private readonly int _durationAddr;

        public SpriteOffset2Sub(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x4) {
            _frameIdAddr  = Address + 0x00; // 2 bytes
            _durationAddr = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_frameIdAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int FrameID {
            get => Data.GetWord(_frameIdAddr);
            set => Data.SetWord(_frameIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_durationAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Duration {
            get => Data.GetWord(_durationAddr);
            set => Data.SetWord(_durationAddr, value);
        }
    }
}
