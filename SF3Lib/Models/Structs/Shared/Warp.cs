using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawEditors;

namespace SF3.Models.Structs.Shared {
    public class Warp : Struct {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        public Warp(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            unknown1 = Address;
            unknown2 = Address + 1;
            type     = Address + 2;
            map      = Address + 3;
        }

        [BulkCopy]
        public int WarpUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int WarpUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int WarpType {
            get => Editor.GetByte(type);
            set => Editor.SetByte(type, (byte) value);
        }

        [BulkCopy]
        public int WarpMap {
            get => Editor.GetByte(map);
            set => Editor.SetByte(map, (byte) value);
        }
    }
}
