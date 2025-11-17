using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class MovableModel : ModelInstanceBase {
        public MovableModel(IByteData data, int id, string name, int address, ModelCollectionType collectionType)
        : base(data, id, name, address, 0x04, 0x1C, collectionType) {
        }

        public override ushort Tag {
            get => 0;
            set {}
        }

        public override ushort Flags {
            get => 0;
            set {}
        }

        public override bool AlwaysFacesCamera {
            get => false;
            set {}
        }

        public override ModelDirectionType OnlyVisibleFromDirection {
            get => ModelDirectionType.Unset;
            set {}
        }
    }
}
