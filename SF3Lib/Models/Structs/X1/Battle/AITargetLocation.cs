using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class AITargetLocation : XYCoord {
        public AITargetLocation(IByteData data, int id, string name, int address) : base(data, id, name, address) {
        }
    }
}
