using System;
using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.CHR {
    public class AnimationOffsets : Struct, IEnumerable<uint> {
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

        public AnimationOffsets(IByteData data, int id, string name, int address, uint dataOffset) : base(data, id, name, address, 0x40) {
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

        public uint DataOffset { get; }

        public uint this[int index] {
            get => (index >= 0 && index < 16) ? (uint) Data.GetDouble(Address + index * 0x04) : throw new ArgumentOutOfRangeException(nameof(index));
            set {
                if (index >= 0 && index < 16)
                    Data.SetDouble(Address + index * 0x04, (int) value);
                else
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public IEnumerable<uint> GetOffsets() {
            var offsets = new uint[16];
            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = (uint) Data.GetDouble(Address + i * 0x04);
            return offsets;
        }

        public IEnumerator<uint> GetEnumerator() => GetOffsets().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetOffsets().GetEnumerator();

        // Gigantic table of properties so it's visible to the ObjectListView (eeeeew)
        [TableViewModelColumn(addressField: nameof(_offset01Addr), displayOrder:  0, isPointer: true)] [BulkCopy] public uint Offset01 { get => (uint) Data.GetDouble(_offset01Addr); set => Data.SetDouble(_offset01Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset02Addr), displayOrder:  1, isPointer: true)] [BulkCopy] public uint Offset02 { get => (uint) Data.GetDouble(_offset02Addr); set => Data.SetDouble(_offset02Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset03Addr), displayOrder:  2, isPointer: true)] [BulkCopy] public uint Offset03 { get => (uint) Data.GetDouble(_offset03Addr); set => Data.SetDouble(_offset03Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset04Addr), displayOrder:  3, isPointer: true)] [BulkCopy] public uint Offset04 { get => (uint) Data.GetDouble(_offset04Addr); set => Data.SetDouble(_offset04Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset05Addr), displayOrder:  4, isPointer: true)] [BulkCopy] public uint Offset05 { get => (uint) Data.GetDouble(_offset05Addr); set => Data.SetDouble(_offset05Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset06Addr), displayOrder:  5, isPointer: true)] [BulkCopy] public uint Offset06 { get => (uint) Data.GetDouble(_offset06Addr); set => Data.SetDouble(_offset06Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset07Addr), displayOrder:  6, isPointer: true)] [BulkCopy] public uint Offset07 { get => (uint) Data.GetDouble(_offset07Addr); set => Data.SetDouble(_offset07Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset08Addr), displayOrder:  7, isPointer: true)] [BulkCopy] public uint Offset08 { get => (uint) Data.GetDouble(_offset08Addr); set => Data.SetDouble(_offset08Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset09Addr), displayOrder:  8, isPointer: true)] [BulkCopy] public uint Offset09 { get => (uint) Data.GetDouble(_offset09Addr); set => Data.SetDouble(_offset09Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset10Addr), displayOrder:  9, isPointer: true)] [BulkCopy] public uint Offset10 { get => (uint) Data.GetDouble(_offset10Addr); set => Data.SetDouble(_offset10Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset11Addr), displayOrder: 10, isPointer: true)] [BulkCopy] public uint Offset11 { get => (uint) Data.GetDouble(_offset11Addr); set => Data.SetDouble(_offset11Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset12Addr), displayOrder: 11, isPointer: true)] [BulkCopy] public uint Offset12 { get => (uint) Data.GetDouble(_offset12Addr); set => Data.SetDouble(_offset12Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset13Addr), displayOrder: 12, isPointer: true)] [BulkCopy] public uint Offset13 { get => (uint) Data.GetDouble(_offset13Addr); set => Data.SetDouble(_offset13Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset14Addr), displayOrder: 13, isPointer: true)] [BulkCopy] public uint Offset14 { get => (uint) Data.GetDouble(_offset14Addr); set => Data.SetDouble(_offset14Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset15Addr), displayOrder: 14, isPointer: true)] [BulkCopy] public uint Offset15 { get => (uint) Data.GetDouble(_offset15Addr); set => Data.SetDouble(_offset15Addr, (int) value); }
        [TableViewModelColumn(addressField: nameof(_offset16Addr), displayOrder: 15, isPointer: true)] [BulkCopy] public uint Offset16 { get => (uint) Data.GetDouble(_offset16Addr); set => Data.SetDouble(_offset16Addr, (int) value); }
    }
}
