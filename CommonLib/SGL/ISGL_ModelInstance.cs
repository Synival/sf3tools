using System.Numerics;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace CommonLib.SGL {
    /// <summary>
    /// Interface for an instance of an ISGL_Model.
    /// </summary>
    public interface ISGL_ModelInstance {
        /// <summary>
        /// Fetches the model referenced by this instance at a specific level of detail.
        /// </summary>
        /// <param name="lod">Level of detail to fetch.</param>
        /// <returns>An existing IMPD_ModelLoD if found, otherwise 'null'.</returns>
        ISGL_Model GetModel(int lod);

        int ModelCollectionID { get; }
        int ModelID { get; set; }
        int ModelInstanceID { get; }

        short PositionX { get; set; }
        short PositionY { get; set; }
        short PositionZ { get; set; }

        float AngleX { get; set; }
        float AngleY { get; set; }
        float AngleZ { get; set; }

        float ScaleX { get; set; }
        float ScaleY { get; set; }
        float ScaleZ { get; set; }

        /// <summary>
        /// Optional matrix as an addition to explicit Pos, Rot, Scale transformations. Should be considered *after* those transformations.
        /// </summary>
        Matrix4x4? Matrix { get; }

        bool AlwaysFacesCamera { get; set; }

        /// <summary>
        /// Number of levels of detail. This is always 8 except for, like, 3 times, and they're dumb.
        /// Mostly under-utilized.
        /// </summary>
        int LevelsOfDetail { get; set; }

        /// <summary>
        /// Bounding box of this model in world space, taking scale and rotation into account.
        /// Position is ignored; bounding box is as if the model is placed at (0, 0, 0).
        /// </summary>
        BoundingBox BoundingBox { get; }
    }
}
