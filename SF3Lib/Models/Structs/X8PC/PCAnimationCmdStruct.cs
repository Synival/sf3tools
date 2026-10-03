using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationCmdStruct : Struct {
        public readonly int _frameAddr;
        public readonly int _cmdAddr;

        public PCAnimationCmdStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _frameAddr = Address + 0x00; // 2 bytes
            _cmdAddr   = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_frameAddr), displayOrder: 0)]
        [BulkCopy]
        public short Frame {
            get => Data.GetInt16(_frameAddr);
            set => Data.SetInt16(_frameAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_cmdAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public short Command {
            get => Data.GetInt16(_cmdAddr);
            set => Data.SetInt16(_cmdAddr, value);
        }
    }
}
