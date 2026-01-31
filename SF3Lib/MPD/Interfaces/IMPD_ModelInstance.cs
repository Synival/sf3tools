using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for an instance of an IMPD_Model.
    /// </summary>
    public interface IMPD_ModelInstance {
        MPD_CollectionType Collection { get; }
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

        /// <summary>
        /// Number of levels of detail. This is always 8 except for, like, 3 times, and they're dumb.
        /// Mostly under-utilized.
        /// </summary>
        int LevelsOfDetail { get; set; }
    }
}
