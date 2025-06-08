using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class LightPosition : Struct {
        private readonly int _pitchAddr;
        private readonly int _yawAddr;

        public LightPosition(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _pitchAddr = Address + 0x00; // 2 bytes
            _yawAddr   = Address + 0x02; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_pitchAddr), displayName: "Pitch", displayFormat: "X4")]
        public ushort Pitch {
            get => (ushort) Data.GetWord(_pitchAddr);
            set => Data.SetWord(_pitchAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_yawAddr), displayName: "Yaw", displayFormat: "X4")]
        public ushort Yaw {
            get => (ushort) Data.GetWord(_yawAddr);
            set => Data.SetWord(_yawAddr, value);
        }
    }
}
