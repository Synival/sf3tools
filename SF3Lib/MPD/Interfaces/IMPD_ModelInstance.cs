using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for an instance of an IMPD_Model.
    /// </summary>
    public interface IMPD_ModelInstance : ISGL_ModelInstance {
        IMPD_ModelCollection Collection { get; }
        ushort Tag { get; set; }
        ushort Flags { get; set; }
        ModelDirectionType OnlyVisibleFromDirection { get; set; }

        /// <summary>
        /// Fetches the model referenced by this instance at a specific level of detail.
        /// </summary>
        /// <param name="lod">Level of detail to fetch.</param>
        /// <returns>An existing IMPD_ModelLoD if found, otherwise 'null'.</returns>
        new IMPD_ModelLoD GetModel(int lod);
    }
}
