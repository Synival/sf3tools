using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class MovableModel : Struct {
        private readonly int _pdataOffsetAddress;
        private readonly int _positionXAddress;
        private readonly int _positionYAddress;
        private readonly int _positionZAddress;
        private readonly int _angleXAddress;
        private readonly int _angleYAddress;
        private readonly int _angleZAddress;
        private readonly int _scaleXAddress;
        private readonly int _scaleYAddress;
        private readonly int _scaleZAddress;

        public MovableModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x1C) {
            _pdataOffsetAddress     = Address + 0x00; // 4 bytes
            _positionXAddress = Address + 0x04; // 2 bytes
            _positionYAddress = Address + 0x06; // 2 bytes
            _positionZAddress = Address + 0x08; // 2 bytes
            _angleXAddress    = Address + 0x0A; // 2 bytes
            _angleYAddress    = Address + 0x0C; // 2 bytes
            _angleZAddress    = Address + 0x0E; // 2 bytes
            _scaleXAddress    = Address + 0x10; // 4 bytes
            _scaleYAddress    = Address + 0x14; // 4 bytes
            _scaleZAddress    = Address + 0x18; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayName: "PData Offset", isPointer: true)]
        public uint PDataOffset {
            get => (uint) Data.GetDouble(_pdataOffsetAddress);
            set => Data.SetDouble(_pdataOffsetAddress, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 8)]
        public short PositionX {
            get => (short) Data.GetWord(_positionXAddress);
            set => Data.SetWord(_positionXAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 9)]
        public short PositionY {
            get => (short) Data.GetWord(_positionYAddress);
            set => Data.SetWord(_positionYAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 10)]
        public short PositionZ {
            get => (short) Data.GetWord(_positionZAddress);
            set => Data.SetWord(_positionZAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 11)]
        public float AngleX {
            get => Data.GetCompressedFIXED(_angleXAddress).Float;
            set => Data.SetCompressedFIXED(_angleXAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12)]
        public float AngleY {
            get => Data.GetCompressedFIXED(_angleYAddress).Float;
            set => Data.SetCompressedFIXED(_angleYAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13)]
        public float AngleZ {
            get => Data.GetCompressedFIXED(_angleZAddress).Float;
            set => Data.SetCompressedFIXED(_angleZAddress, new CompressedFIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14)]
        public float ScaleX {
            get => Data.GetFIXED(_scaleXAddress).Float;
            set => Data.SetFIXED(_scaleXAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15)]
        public float ScaleY {
            get => Data.GetFIXED(_scaleYAddress).Float;
            set => Data.SetFIXED(_scaleYAddress, new FIXED(value, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16)]
        public float ScaleZ {
            get => Data.GetFIXED(_scaleZAddress).Float;
            set => Data.SetFIXED(_scaleZAddress, new FIXED(value, 0));
        }
    }
}
