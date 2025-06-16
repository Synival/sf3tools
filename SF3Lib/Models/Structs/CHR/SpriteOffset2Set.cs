using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.CHR {
    public class SpriteOffset2Set : Struct, IEnumerable<int> {
        private readonly int _offset01Addr;
        private readonly int _offset02Addr;
        private readonly int _offset03Addr;
        private readonly int _offset04Addr;
        private readonly int _offset05Addr;
        private readonly int _offset06Addr;
        private readonly int _offset07Addr;
        private readonly int _offset08Addr;
        private readonly int _offset09Addr;
        private readonly int _offset10Addr;
        private readonly int _offset11Addr;
        private readonly int _offset12Addr;
        private readonly int _offset13Addr;
        private readonly int _offset14Addr;
        private readonly int _offset15Addr;
        private readonly int _offset16Addr;

        public SpriteOffset2Set(IByteData data, int id, string name, int address, int dataOffset) : base(data, id, name, address, 0x40) {
            DataOffset = dataOffset;

            _offset01Addr = Address + 0x00; // 4 bytes
            _offset02Addr = Address + 0x04; // 4 bytes
            _offset03Addr = Address + 0x08; // 4 bytes
            _offset04Addr = Address + 0x0C; // 4 bytes
            _offset05Addr = Address + 0x10; // 4 bytes
            _offset06Addr = Address + 0x14; // 4 bytes
            _offset07Addr = Address + 0x18; // 4 bytes
            _offset08Addr = Address + 0x1C; // 4 bytes
            _offset09Addr = Address + 0x20; // 4 bytes
            _offset10Addr = Address + 0x24; // 4 bytes
            _offset11Addr = Address + 0x28; // 4 bytes
            _offset12Addr = Address + 0x2C; // 4 bytes
            _offset13Addr = Address + 0x30; // 4 bytes
            _offset14Addr = Address + 0x34; // 4 bytes
            _offset15Addr = Address + 0x38; // 4 bytes
            _offset16Addr = Address + 0x3C; // 4 bytes
        }

        public int DataOffset { get; }

        public int this[int index] {
            get => (index >= 0 && index < 16) ? Data.GetDouble(Address + index * 0x04) : throw new ArgumentOutOfRangeException(nameof(index));
            set {
                if (index >= 0 && index < 16)
                    Data.SetDouble(Address + index * 0x04, value);
                else
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public IEnumerable<int> GetOffsets() {
            var offsets = new int[16];
            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = Data.GetDouble(Address + i * 0x04);
            return offsets;
        }

        public IEnumerator<int> GetEnumerator() => GetOffsets().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetOffsets().GetEnumerator();

        [TableViewModelColumn(addressField: nameof(_offset01Addr), displayOrder: 0, isPointer: true)]
        [BulkCopy]
        public int Offset01 {
            get => Data.GetDouble(_offset01Addr);
            set => Data.SetDouble(_offset01Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset02Addr), displayOrder: 1, isPointer: true)]
        [BulkCopy]
        public int Offset02 {
            get => Data.GetDouble(_offset02Addr);
            set => Data.SetDouble(_offset02Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset03Addr), displayOrder: 2, isPointer: true)]
        [BulkCopy]
        public int Offset03 {
            get => Data.GetDouble(_offset03Addr);
            set => Data.SetDouble(_offset03Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset04Addr), displayOrder: 3, isPointer: true)]
        [BulkCopy]
        public int Offset04 {
            get => Data.GetDouble(_offset04Addr);
            set => Data.SetDouble(_offset04Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset05Addr), displayOrder: 4, isPointer: true)]
        [BulkCopy]
        public int Offset05 {
            get => Data.GetDouble(_offset05Addr);
            set => Data.SetDouble(_offset05Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset06Addr), displayOrder: 5, isPointer: true)]
        [BulkCopy]
        public int Offset06 {
            get => Data.GetDouble(_offset06Addr);
            set => Data.SetDouble(_offset06Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset07Addr), displayOrder: 6, isPointer: true)]
        [BulkCopy]
        public int Offset07 {
            get => Data.GetDouble(_offset07Addr);
            set => Data.SetDouble(_offset07Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset08Addr), displayOrder: 7, isPointer: true)]
        [BulkCopy]
        public int Offset08 {
            get => Data.GetDouble(_offset08Addr);
            set => Data.SetDouble(_offset08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset09Addr), displayOrder: 8, isPointer: true)]
        [BulkCopy]
        public int Offset09 {
            get => Data.GetDouble(_offset09Addr);
            set => Data.SetDouble(_offset09Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset10Addr), displayOrder: 9, isPointer: true)]
        [BulkCopy]
        public int Offset10 {
            get => Data.GetDouble(_offset10Addr);
            set => Data.SetDouble(_offset10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset11Addr), displayOrder: 10, isPointer: true)]
        [BulkCopy]
        public int Offset11 {
            get => Data.GetDouble(_offset11Addr);
            set => Data.SetDouble(_offset11Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset12Addr), displayOrder: 11, isPointer: true)]
        [BulkCopy]
        public int Offset12 {
            get => Data.GetDouble(_offset12Addr);
            set => Data.SetDouble(_offset12Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset13Addr), displayOrder: 12, isPointer: true)]
        [BulkCopy]
        public int Offset13 {
            get => Data.GetDouble(_offset13Addr);
            set => Data.SetDouble(_offset13Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset14Addr), displayOrder: 13, isPointer: true)]
        [BulkCopy]
        public int Offset14 {
            get => Data.GetDouble(_offset14Addr);
            set => Data.SetDouble(_offset14Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset15Addr), displayOrder: 14, isPointer: true)]
        [BulkCopy]
        public int Offset15 {
            get => Data.GetDouble(_offset15Addr);
            set => Data.SetDouble(_offset15Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_offset16Addr), displayOrder: 15, isPointer: true)]
        [BulkCopy]
        public int Offset16 {
            get => Data.GetDouble(_offset16Addr);
            set => Data.SetDouble(_offset16Addr, value);
        }
    }
}
