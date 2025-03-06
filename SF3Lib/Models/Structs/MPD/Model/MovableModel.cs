using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class MovableModel : ModelBase {
        public MovableModel(IByteData data, int id, string name, int address, ModelCollectionType collectionType)
        : base(data, id, name, address, 0x04, 0x1C, collectionType) {
        }

        public override bool AlwaysFacesCamera {
            get => false;
            set {}
        }
    }
}
