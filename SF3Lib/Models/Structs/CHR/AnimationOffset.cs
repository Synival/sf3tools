﻿using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class AnimationOffset : Struct {
        private readonly int _offsetAddr;

        public AnimationOffset(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _offsetAddr = Address;
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f)]
        public AnimationType AnimationType => (AnimationType) ID;

        [TableViewModelColumn(addressField: nameof(_offsetAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public uint Offset {
            get => (uint) Data.GetDouble(_offsetAddr);
            set => Data.SetDouble(_offsetAddr, (int) value);
        }
    }
}
