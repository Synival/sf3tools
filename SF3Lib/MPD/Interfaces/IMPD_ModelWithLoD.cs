using CommonLib;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract representation of model that can be expressed at multiple levels-of-detail.
    /// </summary>
    public interface IMPD_ModelWithLoD {
        /// <summary>
        /// ID of the model this belongs to.
        /// </summary>
        int ModelID { get; }

        /// <summary>
        /// Number of levels-of-detail for this model.
        /// </summary>
        int LevelsOfDetail { get; }

        /// <summary>
        /// Collection or chunk this model belongs to.
        /// </summary>
        MPD_CollectionType Collection { get; }

        /// <summary>
        /// Set of models used for reach level-of-detail.
        /// </summary>
        IIndexedEnumerableWithLength<IMPD_Model> Models { get; }
    }
}
