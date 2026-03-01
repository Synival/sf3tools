using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelInstance : IMPD_ModelInstance {
        public MPD_ModelInstance() {
            OnlyVisibleFromDirection = ModelDirectionType.Unset;
        }

        public MPD_ModelInstance(IMPD_ModelInstance original, IMPD_ModelCollection newCollection) {
            Collection = newCollection;
            ID        = original.ID;
            ModelID   = original.ModelID;
            PositionX = original.PositionX;
            PositionY = original.PositionY;
            PositionZ = original.PositionZ;
            AngleX    = original.AngleX;
            AngleY    = original.AngleY;
            AngleZ    = original.AngleZ;
            ScaleX    = original.ScaleX;
            ScaleY    = original.ScaleY;
            ScaleZ    = original.ScaleZ;
            Tag       = original.Tag;
            Flags     = original.Flags;
            LevelsOfDetail = original.LevelsOfDetail;
        }

        public static MPD_ModelInstance FromJToken(JToken token, IMPD_ModelCollection collection) => new MPD_ModelInstance(token, collection);
        private MPD_ModelInstance(JToken token, IMPD_ModelCollection collection) {
            Collection = collection;

            var jObject = (JObject) token;

            ID        =    (int) jObject["ID"];
            ModelID   =    (int) jObject["ModelID"];
            PositionX =  (short) jObject["PositionX"];
            PositionY =  (short) jObject["PositionY"];
            PositionZ =  (short) jObject["PositionZ"];
            AngleX    =  (float) jObject["AngleX"];
            AngleY    =  (float) jObject["AngleY"];
            AngleZ    =  (float) jObject["AngleZ"];
            ScaleX    =  (float) jObject["ScaleX"];
            ScaleY    =  (float) jObject["ScaleY"];
            ScaleZ    =  (float) jObject["ScaleZ"];

            if (!Collection.IsHeaderModelCollection()) {
                Tag   = (ushort) jObject["Tag"];
                Flags = (ushort) jObject["Flags"];
                LevelsOfDetail = (int) jObject["LevelsOfDetail"];
            }
        }

        public IMPD_ModelLoD GetModel(int lod)
            => Collection.GetModel(ModelID, lod);

        public IMPD_ModelCollection Collection { get; set; }
        public int ID { get; set; }
        public int ModelID { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public short PositionZ { get; set; }
        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public ushort Tag { get; set; }
        public ushort Flags { get; set; }

        public bool AlwaysFacesCamera {
            get => (Flags & 0x08) == 0x08;
            set => Flags = (ushort) (Flags & ~0x08 | (value ? 0x08 : 0x00));
        }

        public ModelDirectionType OnlyVisibleFromDirection {
            get => (Flags & 0x10) == 0x10 ? (ModelDirectionType) (Flags & 0x07) : ModelDirectionType.Unset;
            set => Flags = (ushort) (Flags & 0x07 | (((short) value & 0x07) == (short) ModelDirectionType.Unset ? 0 : (ushort) value & 0x07));
        }

        public int LevelsOfDetail { get; set; }
    }
}
