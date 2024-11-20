using System;
using System.Collections.Generic;
using System.Text;
using CommonLib.Attributes;
using SF3.Attributes;
using SF3.RawEditors;

namespace SF3.Models.MPD {
    public class Offset4Model : Model {
        private readonly int _value1Address;
        private readonly int _value2Address;
        private readonly int _value3Address;
        private readonly int _value4Address;

        public Offset4Model(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x10) {
            _value1Address = Address + 0x00;  // 4 bytes
            _value2Address = Address + 0x04;  // 4 bytes
            _value3Address = Address + 0x08;  // 4 bytes
            _value4Address = Address + 0x0C;  // 4 bytes
        }

        [BulkCopy]
        [ViewModelData(displayName: "Value 1", displayOrder: 0, displayFormat: "X8")]
        public uint Value1 {
            get => (uint) Editor.GetDouble(_value1Address);
            set => Editor.SetDouble(_value1Address, (int) value);
        }

        [BulkCopy]
        [ViewModelData(displayName: "Value 2", displayOrder: 1, isPointer: true)]
        public uint Value2 {
            get => (uint) Editor.GetDouble(_value2Address);
            set {
                // Don't allow setting if this is out of range.
                if (Value1 != 0xFFFF_FFFF)
                    Editor.SetDouble(_value2Address, (int) value);
            }
        }

        [BulkCopy]
        [ViewModelData(displayName: "Value 3", displayOrder: 2, isPointer: true)]
        public uint Value3 {
            get => (uint) Editor.GetDouble(_value3Address);
            set {
                // Don't allow setting if this is out of range.
                if (Value1 != 0xFFFF_FFFF)
                    Editor.SetDouble(_value3Address, (int) value);
            }
        }

        [BulkCopy]
        [ViewModelData(displayName: "Value 4", displayOrder: 3, displayFormat: "X8")]
        public uint Value4 {
            get => (uint) Editor.GetDouble(_value4Address);
            set {
                // Don't allow setting if this is out of range.
                if (Value1 != 0xFFFF_FFFF)
                    Editor.SetDouble(_value4Address, (int) value);
            }
        }
    }
}
