using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Structs.X8PC {
    public class PCTexture : TextureStructBase {
        private readonly int _widthAddr;
        private readonly int _heightAddr;
        private readonly int _dataOffsetAddr;
        private readonly int _compressedDataSizeAddr;

        public PCTexture(IByteData data, IByteArray texData, int id, string name, int address)
        : base(data, texData, id, name, address, 0x08, CommonLib.Types.TexturePixelFormat.ABGR1555, false, false, CommonLib.Types.IndexedColorUpdateStrategy.DontUpdate) {
            _widthAddr              = Address + 0x00; // 2 bytes
            _heightAddr             = Address + 0x02; // 2 bytes
            _dataOffsetAddr         = Address + 0x04; // 2 bytes
            _compressedDataSizeAddr = Address + 0x06; // 2 bytes

            LoadImageData();
        }

        public override bool HasImage => true;
        public override bool CanLoadImage => true;

        [TableViewModelColumn(addressField: nameof(_dataOffsetAddr), displayOrder: 0.00f, displayFormat: "X2")]
        [BulkCopy]
        public ushort StoredDataOffset {
            get => Data.GetUInt16(_dataOffsetAddr);
            set => Data.SetUInt16(_dataOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_compressedDataSizeAddr), displayOrder: 0.01f, displayFormat: "X2")]
        [BulkCopy]
        public ushort CompressedDataSize {
            get => Data.GetUInt16(_compressedDataSizeAddr);
            set => Data.SetUInt16(_compressedDataSizeAddr, value);
        }

        protected override int StructWidth {
            get => Data.GetUInt16(_widthAddr);
            set => Data.SetUInt16(_widthAddr, (ushort) value);
        }

        protected override int StructHeight {
            get => Data.GetUInt16(_heightAddr);
            set => Data.SetUInt16(_heightAddr, (ushort) value);
        }

        protected override int StructImageDataOffset {
            get => StoredDataOffset << 3;
            set => StoredDataOffset = (ushort) (value >> 3);
        }

        protected override IPalette StructPalette {
            get => null;
            set {}
        }

        protected override void OnImageUpdated() {}
    }
}
