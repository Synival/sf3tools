using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class MapMoveCoordFlags : Struct {
        public MapMoveCoordFlags(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2")]
        public byte RawValue {
            get => Data.GetUInt8(Address);
            set => Data.SetUInt8(Address, value);
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 100)]
        public MapMoveTeamType Teams {
            get => (MapMoveTeamType) ((RawValue & 0xC0) >> 6);
            set => RawValue = (byte) (RawValue & ~0xC0 | (((int) value & 0x03) << 6 ));
        }

        [TableViewModelColumn(displayOrder: 2)]
        public bool AnyEventID {
            get => (RawValue & 0x20) == 0x20;
            set => RawValue = (byte) (RawValue & ~0x20 | (value ? 0x20 : 0x00));
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        public byte EventID {
            get => (byte) (0x10 | (RawValue & 0x0F));
            set => RawValue = (byte) (RawValue & ~0x0F | (value & 0x0F));
        }
    }
}
