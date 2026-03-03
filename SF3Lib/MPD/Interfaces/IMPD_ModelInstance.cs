using SF3.Types;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for an instance of an IMPD_Model.
    /// </summary>
    public interface IMPD_ModelInstance {
        IMPD_ModelCollection Collection { get; }
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

        /// <summary>
        /// Fetches the model referenced by this instance at a specific level of detail.
        /// </summary>
        /// <param name="lod">Level of detail to fetch.</param>
        /// <returns>An existing IMPD_ModelLoD if found, otherwise 'null'.</returns>
        IMPD_ModelLoD GetModel(int lod);

        /// <summary>
        /// Bounding cube of this model in world space, taking scale and rotation into account.
        /// Position is ignored; bounding cube is as if the model is placed at (0, 0, 0).
        /// </summary>
        BoundingCube BoundingCube { get; }
    }
}
