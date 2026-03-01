using SF3.ByteData;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class HeaderModelInstance : ModelInstanceBase {
        public HeaderModelInstance(IByteData data, IMPD_ModelCollection collection, int id, string name, int address)
        : base(data, collection, id, name, address, 0x04, 0x1C) {
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

        public override int LevelsOfDetail { get => 1; set {} }
    }
}
