using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class LightPosition : Struct {
        public LightPosition(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Pitch", displayFormat: "X4")]
        public ushort Pitch {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Yaw", displayFormat: "X4")]
        public ushort Yaw {
            get => (ushort) Data.GetWord(Address + 2);
            set => Data.SetWord(Address + 2, value);
        }
    }
}
