using CommonLib.Attributes;
using CommonLib.SGL;
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
        [TableViewModelColumn(addressField: nameof(_pitchAddr), displayName: "Pitch")]
        public float Pitch {
            get => Data.GetCompressedFIXED(_pitchAddr).Float * 180.0f;
            set => Data.SetCompressedFIXED(_pitchAddr, new CompressedFIXED(value / 180.0f, 0));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_yawAddr), displayName: "Yaw")]
        public float Yaw {
            get => Data.GetCompressedFIXED(_yawAddr).Float * 180.0f;
            set => Data.SetCompressedFIXED(_yawAddr, new CompressedFIXED(value / 180.0f, 0));
        }
    }
}
