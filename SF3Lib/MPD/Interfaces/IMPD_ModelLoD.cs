using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for an ISGL_Model with additional data necessary for MPD files.
    /// </summary>
    public interface IMPD_ModelLoD : ISGL_Model {
        /// <summary>
        /// Collection or chunk this model belongs to.
        /// </summary>
        MPD_CollectionType Collection { get; }
    }
}
