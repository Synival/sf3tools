using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class Animation : Struct {
        public Animation(IByteData data, int id, string name, int address, AnimationType animationType) : base(data, id, name, address, 0 /* abstract */) {
            AnimationType = animationType;
        }

        [TableViewModelColumn(displayOrder: 0)]
        public AnimationType AnimationType { get; }
    }
}
