using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationCmdStruct : Struct {
        public readonly int _frameAddr;
        public readonly int _cmdAddr;

        public PCAnimationCmdStruct(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _frameAddr = Address + 0x00; // 2 bytes
            _cmdAddr   = Address + 0x02; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_frameAddr), displayOrder: 0)]
        [BulkCopy]
        public short Frame {
            get => Data.GetInt16(_frameAddr);
            set => Data.SetInt16(_frameAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_cmdAddr), displayOrder: 1, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.PCAnimationCmdType)]
        [BulkCopy]
        public ushort CommandRaw {
            get => Data.GetUInt16(_cmdAddr);
            set => Data.SetUInt16(_cmdAddr, value);
        }

        public PCAnimationCmdType Command {
            get => (PCAnimationCmdType) CommandRaw;
            set => CommandRaw = (ushort) value;
        }

        public bool IsSfx => (CommandRaw & 0xff00) == 0xff00;

        [TableViewModelColumn(addressField: nameof(_cmdAddr), displayOrder: 1, displayFormat: "X2")]
        public byte? Sfx {
            get => IsSfx ? (byte?) (CommandRaw & 0x00ff) : null;
            set {
                if (IsSfx && value.HasValue)
                    CommandRaw = (ushort) (0xff00 | value.Value);
            }
        }
    }
}
