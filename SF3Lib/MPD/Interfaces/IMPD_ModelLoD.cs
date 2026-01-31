using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for an ISGL_Model with additional data necessary for MPD files.
    /// </summary>
    public interface IMPD_ModelLoD : ISGL_Model {
        /// <summary>
        /// ID of the model this belongs to.
        /// </summary>
        int ModelID { get; }

        /// <summary>
        /// Level-of-detail index for the model.
        /// </summary>
        int LevelOfDetail { get; }

        /// <summary>
        /// Collection or chunk this model belongs to.
        /// </summary>
        MPD_CollectionType Collection { get; }
    }
}
