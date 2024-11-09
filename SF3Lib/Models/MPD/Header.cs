using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models.MPD {
    public class Header : Model {
        private readonly int unknown1Address;             // 0x00  int16  Unknown. Might be map id.
        private readonly int unknown2Address;             // 0x02  int16  Always zero 0x0000
        private readonly int offset1Address;              // 0x04  int32  Always 0x0c. Pointer to 0x20 unknown int16. See (#header-offset-1).
        private readonly int offset2Address;              // 0x08  int32  Always 0x4c. pointer to a single unknown int32. See (#header-offset-2).
        private readonly int offset3Address;              // 0x0C  int32  Always 0x50. pointer to 0x20 unknown int16s at the start of the file. Mostly zero or 0x8000. (#header-offset-3)
        private readonly int unknown3Address;             // 0x10  int16  Unknown small value. maybe some count?
        private readonly int unknown4Address;             // 0x12  int16  Always zero
        private readonly int offset4Address;              // 0x14  int32  Always 0x90. Pointer to unknown structure. See (#header-offset-4)
        private readonly int offsetTextureGroupsAddress;  // 0x18  int32  Offset to list of texture groups. See (#texture-groups)
        private readonly int offset6Address;              // 0x1C  int32  Pointer to unknown list.
        private readonly int offset7Address;              // 0x20  int32  Pointer to unknown list.
        private readonly int offsetMesh1Address;          // 0x24  int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh2Address;          // 0x28  int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh3Address;          // 0x2C  int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int const1Address;               // 0x30  int32  Const 0x8000b334
        private readonly int const2Address;               // 0x34  int32  Const 0x4ccc0000
        private readonly int offsetTextureAnimAltAddress; // 0x38  int32  Pointer to a list of texture indices. These textures are the same images as the "real" texture animations, but these textures are from the normal texture block (and doesn't seems to be used). See (#texture-animation-alternatives)
        private readonly int offsetPal1Address;           // 0x3C  int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int offsetPal2Address;           // 0x40  int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int unknown5Address;             // 0x44  int32  Unknown small value, may be negative.
        private readonly int const3Address;               // 0x48  int32  Const 0xc000
        private readonly int unknown6Address;             // 0x4C  int32  Unknown. Lower 16 bits often null. May me FIXED.
        private readonly int unknown7Address;             // 0x50  int32  Unknown. Small value in upper int16, 0x0000 in lower int16. May me FIXED.
        private readonly int offset12Address;             // 0x54  int32  Pointer to unknown list of exactly 8 uint16 in two block with 4 uint16 each.

        public Header(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x58) {
            unknown1Address             = Address;        // 2 bytes
            unknown2Address             = Address + 0x02; // 2 bytes
            offset1Address              = Address + 0x04; // 4 bytes
            offset2Address              = Address + 0x08; // 4 bytes
            offset3Address              = Address + 0x0C; // 4 bytes
            unknown3Address             = Address + 0x10; // 2 bytes
            unknown4Address             = Address + 0x12; // 2 bytes
            offset4Address              = Address + 0x14; // 4 bytes
            offsetTextureGroupsAddress  = Address + 0x18; // 4 bytes
            offset6Address              = Address + 0x1C; // 4 bytes
            offset7Address              = Address + 0x20; // 4 bytes
            offsetMesh1Address          = Address + 0x24; // 4 bytes
            offsetMesh2Address          = Address + 0x28; // 4 bytes
            offsetMesh3Address          = Address + 0x2C; // 4 bytes
            const1Address               = Address + 0x30; // 4 bytes
            const2Address               = Address + 0x34; // 4 bytes
            offsetTextureAnimAltAddress = Address + 0x38; // 4 bytes
            offsetPal1Address           = Address + 0x3C; // 4 bytes
            offsetPal2Address           = Address + 0x40; // 4 bytes
            unknown5Address             = Address + 0x44; // 4 bytes
            const3Address               = Address + 0x48; // 4 bytes
            unknown6Address             = Address + 0x4C; // 4 bytes
            unknown7Address             = Address + 0x50; // 4 bytes
            offset12Address             = Address + 0x54; // 4 bytes
        }

        [BulkCopy]
        public int Unknown1 {
            get => Editor.GetWord(unknown1Address);
            set => Editor.SetWord(unknown1Address, value);
        }

        [BulkCopy]
        public int Unknown2 {
            get => Editor.GetDouble(unknown2Address);
            set => Editor.SetDouble(unknown2Address, value);
        }

        [BulkCopy]
        public int Offset1 {
            get => Editor.GetDouble(offset1Address);
            set => Editor.SetDouble(offset1Address, value);
        }

        [BulkCopy]
        public int Offset2 {
            get => Editor.GetDouble(offset2Address);
            set => Editor.SetDouble(offset2Address, value);
        }

        [BulkCopy]
        public int Offset3 {
            get => Editor.GetDouble(offset3Address);
            set => Editor.SetDouble(offset3Address, value);
        }

        [BulkCopy]
        public int Unknown3 {
            get => Editor.GetWord(unknown3Address);
            set => Editor.SetWord(unknown3Address, value);
        }

        [BulkCopy]
        public int Unknown4 {
            get => Editor.GetWord(unknown4Address);
            set => Editor.SetWord(unknown4Address, value);
        }

        [BulkCopy]
        public int Offset4 {
            get => Editor.GetDouble(offset4Address);
            set => Editor.SetDouble(offset4Address, value);
        }

        [BulkCopy]
        public int OffsetTextureGroups {
            get => Editor.GetDouble(offsetTextureGroupsAddress);
            set => Editor.SetDouble(offsetTextureGroupsAddress, value);
        }

        [BulkCopy]
        public int Offset6 {
            get => Editor.GetDouble(offset6Address);
            set => Editor.SetDouble(offset6Address, value);
        }

        [BulkCopy]
        public int Offset7 {
            get => Editor.GetDouble(offset7Address);
            set => Editor.SetDouble(offset7Address, value);
        }

        [BulkCopy]
        public int OffsetMesh1 {
            get => Editor.GetDouble(offsetMesh1Address);
            set => Editor.SetDouble(offsetMesh1Address, value);
        }

        [BulkCopy]
        public int OffsetMesh2 {
            get => Editor.GetDouble(offsetMesh2Address);
            set => Editor.SetDouble(offsetMesh2Address, value);
        }

        [BulkCopy]
        public int OffsetMesh3 {
            get => Editor.GetDouble(offsetMesh3Address);
            set => Editor.SetDouble(offsetMesh3Address, value);
        }

        [BulkCopy]
        public int Const1 {
            get => Editor.GetDouble(const1Address);
            set => Editor.SetDouble(const1Address, value);
        }

        [BulkCopy]
        public int Const2 {
            get => Editor.GetDouble(const2Address);
            set => Editor.SetDouble(const2Address, value);
        }

        [BulkCopy]
        public int OffsetTextureAnimAlt {
            get => Editor.GetDouble(offsetTextureAnimAltAddress);
            set => Editor.SetDouble(offsetTextureAnimAltAddress, value);
        }

        [BulkCopy]
        public int OffsetPal1 {
            get => Editor.GetDouble(offsetPal1Address);
            set => Editor.SetDouble(offsetPal1Address, value);
        }

        [BulkCopy]
        public int OffsetPal2 {
            get => Editor.GetDouble(offsetPal2Address);
            set => Editor.SetDouble(offsetPal2Address, value);
        }

        [BulkCopy]
        public int Unknown5 {
            get => Editor.GetDouble(unknown5Address);
            set => Editor.SetDouble(unknown5Address, value);
        }

        [BulkCopy]
        public int Const3 {
            get => Editor.GetDouble(const3Address);
            set => Editor.SetDouble(const3Address, value);
        }

        [BulkCopy]
        public int Unknown6 {
            get => Editor.GetDouble(unknown6Address);
            set => Editor.SetDouble(unknown6Address, value);
        }

        [BulkCopy]
        public int Unknown7 {
            get => Editor.GetDouble(unknown7Address);
            set => Editor.SetDouble(unknown7Address, value);
        }

        [BulkCopy]
        public int Offset12 {
            get => Editor.GetDouble(offset12Address);
            set => Editor.SetDouble(offset12Address, value);
        }
    }
}
