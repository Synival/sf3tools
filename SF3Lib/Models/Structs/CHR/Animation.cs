using SF3.ByteData;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, int animIndex) : base(data, id, name, address, 0 /* abstract */) {
            AnimIndex = animIndex;
        }

        public int AnimIndex { get; }
    }
}
