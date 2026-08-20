using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCTexDefChunkHeader : Struct {
        private readonly int _texDefsOffsetAddr;
        private readonly int _numTexturesAddr;
        private readonly int _decompressedSize;
        private readonly int _unknown0x0cAddr;
        private readonly int _unknown0x10Addr;

        public PCTexDefChunkHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            _texDefsOffsetAddr = Address + 0x00; // 4 bytes
            _numTexturesAddr   = Address + 0x04; // 4 bytes
            _decompressedSize  = Address + 0x08; // 4 bytes
            _unknown0x0cAddr   = Address + 0x0C; // 4 bytes
            _unknown0x10Addr   = Address + 0x10; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_texDefsOffsetAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public uint TexDefsOffset {
            get => Data.GetUInt32(_texDefsOffsetAddr);
            set => Data.SetUInt32(_texDefsOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numTexturesAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public uint NumTextures {
            get => Data.GetUInt32(_numTexturesAddr);
            set => Data.SetUInt32(_numTexturesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_decompressedSize), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public uint DecompressedSize {
            get => Data.GetUInt32(_decompressedSize);
            set => Data.SetUInt32(_decompressedSize, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0cAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public uint Unknown0x0C {
            get => Data.GetUInt32(_unknown0x0cAddr);
            set => Data.SetUInt32(_unknown0x0cAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x10Addr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public uint Unknown0x10 {
            get => Data.GetUInt32(_unknown0x10Addr);
            set => Data.SetUInt32(_unknown0x10Addr, value);
        }
    }
}
