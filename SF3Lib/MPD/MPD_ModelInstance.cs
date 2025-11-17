using SF3.Types;

namespace SF3.MPD {
    public abstract class MPD_ModelInstance : IMPD_ModelInstance {
        public MPD_ModelInstance() {}

        public ModelCollectionType CollectionType { get; set; }
        public int ModelIndex { get; set; }
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
        public bool AlwaysFacesCamera { get; set; }
        public ModelDirectionType OnlyVisibleFromDirection { get; set; }
    }
}
