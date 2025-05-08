using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Town {
    public class Npc : Struct {
        private readonly int _spriteIDAddr;
        private readonly int _flagAddr;
        private readonly int _scriptOffsetAddr;
        private readonly int _xPosAddr;
        private readonly int _yPosAddr;
        private readonly int _zPosAddr;
        private readonly int _directionAddr;
        private readonly int _unusedDirection0x16Addr;

        public Npc(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _spriteIDAddr      = Address + 0x00; // 2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            _flagAddr          = Address + 0x02; // 2 bytes
            _scriptOffsetAddr  = Address + 0x04; // 4 bytes
            _xPosAddr          = Address + 0x08; // 4 bytes
            _yPosAddr          = Address + 0x0C; // 4 bytes
            _zPosAddr          = Address + 0x10; // 4 bytes
            _directionAddr     = Address + 0x14; // 2 bytes (upper word)
            _unusedDirection0x16Addr = Address + 0x016; // 2 bytes (lower word, not read)
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X3", minWidth: 200)]
        [BulkCopy]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID {
            get => Data.GetWord(_spriteIDAddr);
            set => Data.SetWord(_spriteIDAddr, value);
        }

        [BulkCopy]
        public int FlagCheckedWithValue {
            get => Data.GetWord(_flagAddr);
            set => Data.SetWord(_flagAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1.0f, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int FlagChecked {
            get => FlagCheckedWithValue & 0x0FFF;
            set => FlagCheckedWithValue = (value == 0xFFF ? 0xFFFF : (FlagCheckedWithValue & ~0xFFF) | (value & 0x0FFF));
        }

        [TableViewModelColumn(displayOrder: 1.1f)]
        public bool FlagExpectedValue {
            get => (FlagCheckedWithValue & 0x1000) != 0;
            set => FlagCheckedWithValue = (FlagCheckedWithValue == 0xFFFF ? 0xFFFF : (value ? (FlagCheckedWithValue | ~0x1000) : (FlagCheckedWithValue & ~0x1000)));
        }

        [TableViewModelColumn(displayOrder: 2, isPointer: true, minWidth: 120, displayFormat: "X8")]
        [BulkCopy]
        public int ScriptOffset {
            get => Data.GetDouble(_scriptOffsetAddr);
            set => Data.SetDouble(_scriptOffsetAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "xPos", displayFormat: "X8")]
        [BulkCopy]
        public int XPos {
            get => Data.GetDouble(_xPosAddr);
            set => Data.SetDouble(_xPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "yPos", displayFormat: "X8")]
        [BulkCopy]
        public int YPos {
            get => Data.GetDouble(_yPosAddr);
            set => Data.SetDouble(_yPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "zPos", displayFormat: "X8")]
        [BulkCopy]
        public int ZPos {
            get => Data.GetDouble(_zPosAddr);
            set => Data.SetDouble(_zPosAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "xPos (dec)")]
        [BulkCopy]
        public float XPosDec {
            get => XPos / 65536.0f;
            set => XPos = (int) value * 65536;
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "yPos (dec)")]
        [BulkCopy]
        public float YPosDec {
            get => YPos / 65536.0f;
            set => YPos = (int) value * 65536;
        }

        [TableViewModelColumn(displayOrder: 8, displayName: "zPos (dec)")]
        [BulkCopy]
        public float ZPosDec {
            get => ZPos / 65536.0f;
            set => ZPos = (int) value * 65536;
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "Direction", displayFormat: "X4")]
        [BulkCopy]
        public int Direction {
            get => Data.GetWord(_directionAddr);
            set => Data.SetWord(_directionAddr, value);
        }

        [TableViewModelColumn(displayOrder: 10, displayName: "UnusedDirection (+0x16)", displayFormat: "X4")]
        [BulkCopy]
        public int UnusedDirection0x16 {
            get => Data.GetWord(_unusedDirection0x16Addr);
            set => Data.SetWord(_unusedDirection0x16Addr, value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "Interactable Tie-in", displayFormat: "X2")]
        public int? InteractableTieIn {
            get {
                var spriteId = SpriteID;
                return (spriteId > 0x0f && spriteId != 0xffff) ? (ID + 0x3D) : (int?) null;
            }
        }
    }
}
