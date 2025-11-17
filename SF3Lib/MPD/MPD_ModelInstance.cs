using SF3.Types;

namespace SF3.MPD {
    public class MPD_ModelInstance : IMPD_ModelInstance {
        public MPD_ModelInstance() {
            OnlyVisibleFromDirection = ModelDirectionType.Unset;
        }

        public MPD_ModelInstance(IMPD_ModelInstance mi) {
            CollectionType = mi.CollectionType;
            ID = mi.ID;
            ModelID = mi.ModelID;
            PositionX = mi.PositionX;
            PositionY = mi.PositionY;
            PositionZ = mi.PositionZ;
            AngleX = mi.AngleX;
            AngleY = mi.AngleY;
            AngleZ = mi.AngleZ;
            ScaleX = mi.ScaleX;
            ScaleY = mi.ScaleY;
            ScaleZ = mi.ScaleZ;
            Tag = mi.Tag;
            AlwaysFacesCamera = mi.AlwaysFacesCamera;
            OnlyVisibleFromDirection = mi.OnlyVisibleFromDirection;
        }

        public ModelCollectionType CollectionType { get; set; }
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
        public bool AlwaysFacesCamera { get; set; }
        public ModelDirectionType OnlyVisibleFromDirection { get; set; }
    }
}
