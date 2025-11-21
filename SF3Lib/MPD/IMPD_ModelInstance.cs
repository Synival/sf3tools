using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_ModelInstance {
        ModelCollectionType CollectionType { get; }
        int ID { get; }
        int ModelID { get; set; }
        short PositionX { get; set; }
        short PositionY { get; set; }
        short PositionZ { get; set; }
        float AngleX { get; set; }
        float AngleY { get; set; }
        float AngleZ { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }
        float ScaleZ { get; set; }
        ushort Tag { get; set; }
        ushort Flags { get; set; }
        bool AlwaysFacesCamera { get; set; }
        ModelDirectionType OnlyVisibleFromDirection { get; set; }
    }
}
