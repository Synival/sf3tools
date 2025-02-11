﻿using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class Offset4Model : Struct {
        private readonly int _unknown1Address;
        private readonly int _pointer1Address;
        private readonly int _pointer2Address;
        private readonly int _unknown2Address;

        public Offset4Model(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _unknown1Address = Address + 0x00;  // 4 bytes
            _pointer1Address = Address + 0x04;  // 4 bytes
            _pointer2Address = Address + 0x08;  // 4 bytes
            _unknown2Address = Address + 0x0C;  // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown1", displayOrder: 0, displayFormat: "X8")]
        public uint Unknown1 {
            get => (uint) Data.GetDouble(_unknown1Address);
            set => Data.SetDouble(_unknown1Address, (int) value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt16Table1", displayOrder: 1, isPointer: true)]
        public uint UInt16Table1 {
            get => (uint) Data.GetDouble(_pointer1Address);
            set {
                // Don't allow setting if this is out of range.
                if (Unknown1 != 0xFFFF_FFFF)
                    Data.SetDouble(_pointer1Address, (int) value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "UInt16Table2", displayOrder: 2, isPointer: true)]
        public uint UInt16Table2 {
            get => (uint) Data.GetDouble(_pointer2Address);
            set {
                // Don't allow setting if this is out of range.
                if (Unknown1 != 0xFFFF_FFFF)
                    Data.SetDouble(_pointer2Address, (int) value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Unknown2", displayOrder: 3, displayFormat: "X8")]
        public uint Unknown2 {
            get => (uint) Data.GetDouble(_unknown2Address);
            set {
                // Don't allow setting if this is out of range.
                if (Unknown1 != 0xFFFF_FFFF)
                    Data.SetDouble(_unknown2Address, (int) value);
            }
        }
    }
}
