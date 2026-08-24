using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelInstance : SGL_ModelInstanceBase, IMPD_ModelInstance {
        public MPD_ModelInstance() : base() {
            OnlyVisibleFromDirection = ModelDirectionType.Unset;
        }

        public MPD_ModelInstance(IMPD_ModelInstance original, IMPD_ModelCollection newCollection) : base(original) {
            Collection = newCollection;
            Tag        = original.Tag;
            Flags      = original.Flags;
        }

        public static MPD_ModelInstance FromJToken(JToken token, IMPD_ModelCollection collection) => new MPD_ModelInstance(token, collection);
        protected MPD_ModelInstance(JToken token, IMPD_ModelCollection collection) : base(token) {
            Collection = collection;

            var jObject = (JObject) token;
            if (!Collection.IsHeaderModelCollection()) {
                Tag   = (ushort) jObject["Tag"];
                Flags = (ushort) jObject["Flags"];
                LevelsOfDetail = (int) jObject["LevelsOfDetail"];
            }
        }

        public override ISGL_Model GetModel(int lod)
            => Collection.GetModel(ModelID, lod);
        IMPD_ModelLoD IMPD_ModelInstance.GetModel(int lod)
            => Collection.GetModel(ModelID, lod);

        public IMPD_ModelCollection Collection { get; set; }

        public override int ModelCollectionID {
            get => (int) Collection.Collection;
            set {}
        }

        public ushort Tag { get; set; }
        public ushort Flags { get; set; }

        public override bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) (Flags & ~0x08 | (value ? 0x08 : 0x00));
        }

        public ModelDirectionType OnlyVisibleFromDirection {
            get => (Flags & 0x10) == 0x10 ? (ModelDirectionType) (Flags & 0x07) : ModelDirectionType.Unset;
            set {
                Flags = (value < 0x00 || (ushort) value > 0x07)
                    ? (ushort) (Flags & ~0x17)
                    : (ushort) (Flags | 0x10 | ((ushort) value & 0x07));
            }
        }
    }
}
