﻿using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SpecialAnimationAssignment : Struct {
        private readonly int _specialIdAddr;
        private readonly int _subAnimationAddr;
        private readonly int _animationIdAddr;

        public SpecialAnimationAssignment(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _specialIdAddr    = Address + 0x00; // 2 bytes
            _subAnimationAddr = Address + 0x02; // 1 byte
            _animationIdAddr  = Address + 0x03; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_specialIdAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int SpecialId {
            get => Data.GetWord(_specialIdAddr);
            set => Data.SetWord(_specialIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_subAnimationAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte SubAnimation {
            get => (byte) Data.GetByte(_subAnimationAddr);
            set => Data.SetByte(_subAnimationAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_animationIdAddr), displayOrder: 2, minWidth: 200, displayFormat: "X2")]
        [NameGetter(NamedValueType.SpecialAnimation)]
        [BulkCopy]
        public int AnimationId {
            get => Data.GetByte(_animationIdAddr);
            set => Data.SetByte(_animationIdAddr, (byte) value);
        }
    }
}
