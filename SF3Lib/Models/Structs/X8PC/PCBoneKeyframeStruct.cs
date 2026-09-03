using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCBoneKeyframeStruct : Struct {
        private readonly int _numPosKeyframesAddr;
        private readonly int _numRotKeyframesAddr;
        private readonly int _numScaleKeyframesAddr;

        private readonly int _posFramesOffsetAddr;
        private readonly int _rotFramesOffsetAddr;
        private readonly int _scaleFramesOffsetAddr;

        private readonly int _posXPtrAddr;
        private readonly int _posYPtrAddr;
        private readonly int _posZPtrAddr;

        private readonly int _rotXPtrAddr;
        private readonly int _rotYPtrAddr;
        private readonly int _rotZPtrAddr;
        private readonly int _rotWPtrAddr;

        private readonly int _scaleXPtrAddr;
        private readonly int _scaleYPtrAddr;
        private readonly int _scaleZPtrAddr;

        public PCBoneKeyframeStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x40) {
            _numPosKeyframesAddr   = Address + 0x00; // 4 bytes
            _numRotKeyframesAddr   = Address + 0x04; // 4 bytes
            _numScaleKeyframesAddr = Address + 0x08; // 4 bytes

            _posFramesOffsetAddr   = Address + 0x0C; // 4 bytes
            _rotFramesOffsetAddr   = Address + 0x10; // 4 bytes
            _scaleFramesOffsetAddr = Address + 0x14; // 4 bytes

            _posXPtrAddr           = Address + 0x18; // 4 bytes
            _posYPtrAddr           = Address + 0x1C; // 4 bytes
            _posZPtrAddr           = Address + 0x20; // 4 bytes

            _rotXPtrAddr           = Address + 0x24; // 4 bytes
            _rotYPtrAddr           = Address + 0x28; // 4 bytes
            _rotZPtrAddr           = Address + 0x2C; // 4 bytes
            _rotWPtrAddr           = Address + 0x30; // 4 bytes

            _scaleXPtrAddr         = Address + 0x34; // 4 bytes
            _scaleYPtrAddr         = Address + 0x38; // 4 bytes
            _scaleZPtrAddr         = Address + 0x3C; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_numPosKeyframesAddr), displayOrder: 0)]
        [BulkCopy]
        public uint NumPosKeyFrames {
            get => Data.GetUInt32(_numPosKeyframesAddr);
            set => Data.SetUInt32(_numPosKeyframesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numRotKeyframesAddr), displayOrder: 1)]
        [BulkCopy]
        public uint NumRotKeyFrames {
            get => Data.GetUInt32(_numRotKeyframesAddr);
            set => Data.SetUInt32(_numRotKeyframesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numScaleKeyframesAddr), displayOrder: 2)]
        [BulkCopy]
        public uint NumScaleKeyFrames {
            get => Data.GetUInt32(_numScaleKeyframesAddr);
            set => Data.SetUInt32(_numScaleKeyframesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posFramesOffsetAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public uint PosFramesOffset {
            get => Data.GetUInt32(_posFramesOffsetAddr);
            set => Data.SetUInt32(_posFramesOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rotFramesOffsetAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public uint RotFramesOffset {
            get => Data.GetUInt32(_rotFramesOffsetAddr);
            set => Data.SetUInt32(_rotFramesOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleFramesOffsetAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public uint ScaleFramesOffset {
            get => Data.GetUInt32(_scaleFramesOffsetAddr);
            set => Data.SetUInt32(_scaleFramesOffsetAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posXPtrAddr), displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public uint PosXPtr {
            get => Data.GetUInt32(_posXPtrAddr);
            set => Data.SetUInt32(_posXPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posYPtrAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public uint PosYPtr {
            get => Data.GetUInt32(_posYPtrAddr);
            set => Data.SetUInt32(_posYPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_posZPtrAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public uint PosZPtr {
            get => Data.GetUInt32(_posZPtrAddr);
            set => Data.SetUInt32(_posZPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rotXPtrAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public uint RotXPtr {
            get => Data.GetUInt32(_rotXPtrAddr);
            set => Data.SetUInt32(_rotXPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rotYPtrAddr), displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public uint RotYPtr {
            get => Data.GetUInt32(_rotYPtrAddr);
            set => Data.SetUInt32(_rotYPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rotZPtrAddr), displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public uint RotZPtr {
            get => Data.GetUInt32(_rotZPtrAddr);
            set => Data.SetUInt32(_rotZPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_rotWPtrAddr), displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public uint RotWPtr {
            get => Data.GetUInt32(_rotWPtrAddr);
            set => Data.SetUInt32(_rotWPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleXPtrAddr), displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public uint ScaleXPtr {
            get => Data.GetUInt32(_scaleXPtrAddr);
            set => Data.SetUInt32(_scaleXPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleYPtrAddr), displayOrder: 14, displayFormat: "X2")]
        [BulkCopy]
        public uint ScaleYPtr {
            get => Data.GetUInt32(_scaleYPtrAddr);
            set => Data.SetUInt32(_scaleYPtrAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_scaleZPtrAddr), displayOrder: 15, displayFormat: "X2")]
        [BulkCopy]
        public uint ScaleZPtr {
            get => Data.GetUInt32(_scaleZPtrAddr);
            set => Data.SetUInt32(_scaleZPtrAddr, value);
        }
    }
}
