using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class MovableModel : ModelBase {
        public MovableModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04, 0x1C) {
        }
    }
}
