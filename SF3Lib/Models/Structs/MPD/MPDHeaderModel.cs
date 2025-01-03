using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class MPDHeaderModel : Struct {
        private readonly int unknown1Address;             // int16  Unknown. Might be map id.
        private readonly int unknown2Address;             // int16  Always zero 0x0000
        private readonly int offset1Address;              // int32  Always 0x0c. Pointer to 0x20 unknown int16. See (#header-offset-1).
        private readonly int offset2Address;              // int32  Always 0x4c. pointer to a single unknown int32. See (#header-offset-2).
        private readonly int offset3Address;              // int32  Always 0x50. pointer to 0x20 unknown int16s at the start of the file. Mostly zero or 0x8000. (#header-offset-3)
        private readonly int unknown3Address;             // int16  Unknown small value. maybe some count?
        private readonly int unknown4Address;             // int16  Always zero
        private readonly int offset4Address;              // int32  Always 0x90. Pointer to unknown structure. See (#header-offset-4)
        private readonly int offsetTextureAnimationsAddress;  // int32  Offset to list of texture groups. See (#texture-groups)
        private readonly int offset6Address;              // int32  Pointer to unknown list.
        private readonly int offset7Address;              // int32  Pointer to unknown list.
        private readonly int offsetMesh1Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh2Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int offsetMesh3Address;          // int32  Pointer to list of 2 movable/interactable mesh. may be null.
        private readonly int const1Address;               // int32  Const 0x8000b334
        private readonly int const2Address;               // int32  Const 0x4ccc0000
        private readonly int offsetTextureAnimAltAddress; // int32  Pointer to a list of texture indices. These textures are the same images as the "real" texture animations, but these textures are from the normal texture block (and doesn't seems to be used). See (#texture-animation-alternatives)
        private readonly int offsetPal1Address;           // int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int offsetPal2Address;           // int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int offsetPal3Address; // (scn3+pd) int32  Pointer to 256 rgb16 colors. May be null.
        private readonly int unknown5Address;             // int32  Unknown small value, may be negative.
        private readonly int const3Address;               // int32  Const 0xc000
        private readonly int unknown6Address;             // int32  Unknown. Lower 16 bits often null. May me FIXED.
        private readonly int unknown7Address;             // int32  Unknown. Small value in upper int16, 0x0000 in lower int16. May me FIXED.
        private readonly int offset12Address;             // int32  Pointer to unknown list of exactly 8 uint16 in two block with 4 uint16 each.

        public MPDHeaderModel(IByteData data, int id, string name, int address, bool hasPalette3)
        : base(data, id, name, address, 0x58) {
            unknown1Address             = Address;        // 2 bytes
            unknown2Address             = Address + 0x02; // 2 bytes
            offset1Address              = Address + 0x04; // 4 bytes
            offset2Address              = Address + 0x08; // 4 bytes
            offset3Address              = Address + 0x0C; // 4 bytes
            unknown3Address             = Address + 0x10; // 2 bytes
            unknown4Address             = Address + 0x12; // 2 bytes
            offset4Address              = Address + 0x14; // 4 bytes
            offsetTextureAnimationsAddress = Address + 0x18; // 4 bytes
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

            int address2;
            if (hasPalette3) {
                offsetPal3Address = Address + 0x44; // 4 bytes
                address2      = Address + 0x48;
            }
            else {
                offsetPal3Address = -1;
                address2 = Address + 0x44;
            }

            unknown5Address             = address2; // 4 bytes
            const3Address               = address2 + 0x04; // 4 bytes
            unknown6Address             = address2 + 0x08; // 4 bytes
            unknown7Address             = address2 + 0x0C; // 4 bytes
            offset12Address             = address2 + 0x10; // 4 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        public int Unknown1 {
            get => Data.GetWord(unknown1Address);
            set => Data.SetWord(unknown1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        public int Unknown2 {
            get => Data.GetDouble(unknown2Address);
            set => Data.SetDouble(unknown2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, isPointer: true)]
        public int OffsetLightPal {
            get => Data.GetDouble(offset1Address);
            set => Data.SetDouble(offset1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, isPointer: true)]
        public int OffsetLightDir {
            get => Data.GetDouble(offset2Address);
            set => Data.SetDouble(offset2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 4, isPointer: true)]
        public int Offset3 {
            get => Data.GetDouble(offset3Address);
            set => Data.SetDouble(offset3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        public int Unknown3 {
            get => Data.GetWord(unknown3Address);
            set => Data.SetWord(unknown3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 6, displayFormat: "X2")]
        public int Unknown4 {
            get => Data.GetWord(unknown4Address);
            set => Data.SetWord(unknown4Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 7, isPointer: true)]
        public int Offset4 {
            get => Data.GetDouble(offset4Address);
            set => Data.SetDouble(offset4Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 8, isPointer: true)]
        public int OffsetTextureAnimations {
            get => Data.GetDouble(offsetTextureAnimationsAddress);
            set => Data.SetDouble(offsetTextureAnimationsAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 9, isPointer: true)]
        public int Offset6 {
            get => Data.GetDouble(offset6Address);
            set => Data.SetDouble(offset6Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 10, isPointer: true)]
        public int Offset7 {
            get => Data.GetDouble(offset7Address);
            set => Data.SetDouble(offset7Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 11, isPointer: true)]
        public int OffsetMesh1 {
            get => Data.GetDouble(offsetMesh1Address);
            set => Data.SetDouble(offsetMesh1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 12, isPointer: true)]
        public int OffsetMesh2 {
            get => Data.GetDouble(offsetMesh2Address);
            set => Data.SetDouble(offsetMesh2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 13, isPointer: true)]
        public int OffsetMesh3 {
            get => Data.GetDouble(offsetMesh3Address);
            set => Data.SetDouble(offsetMesh3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 14, displayFormat: "X8")]
        public int Const1 {
            get => Data.GetDouble(const1Address);
            set => Data.SetDouble(const1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 15, displayFormat: "X8")]
        public int Const2 {
            get => Data.GetDouble(const2Address);
            set => Data.SetDouble(const2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 16, isPointer: true)]
        public int OffsetTextureAnimAlt {
            get => Data.GetDouble(offsetTextureAnimAltAddress);
            set => Data.SetDouble(offsetTextureAnimAltAddress, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 17, isPointer: true)]
        public int OffsetPal1 {
            get => Data.GetDouble(offsetPal1Address);
            set => Data.SetDouble(offsetPal1Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 18, isPointer: true)]
        public int OffsetPal2 {
            get => Data.GetDouble(offsetPal2Address);
            set => Data.SetDouble(offsetPal2Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 19, isPointer: true)]
        public int OffsetPal3 {
            get => offsetPal3Address >= 0 ? Data.GetDouble(offsetPal3Address) : 0;
            set {
                if (offsetPal3Address >= 0)
                    Data.SetDouble(offsetPal3Address, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 20, displayFormat: "X4")]
        public int Unknown5 {
            get => Data.GetDouble(unknown5Address);
            set => Data.SetDouble(unknown5Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 21, displayFormat: "X4")]
        public int Const3 {
            get => Data.GetDouble(const3Address);
            set => Data.SetDouble(const3Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 22, displayFormat: "X8")]
        public int Unknown6 {
            get => Data.GetDouble(unknown6Address);
            set => Data.SetDouble(unknown6Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 23, displayFormat: "X8")]
        public int Unknown7 {
            get => Data.GetDouble(unknown7Address);
            set => Data.SetDouble(unknown7Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 24, isPointer: true)]
        public int Offset12 {
            get => Data.GetDouble(offset12Address);
            set => Data.SetDouble(offset12Address, value);
        }
    }
}
