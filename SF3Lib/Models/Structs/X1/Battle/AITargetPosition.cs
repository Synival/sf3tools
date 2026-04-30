using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class AITargetPosition : XYCoord {
        public AITargetPosition(IByteData data, int id, string name, int address) : base(data, id, name, address) {
        }
    }
}
