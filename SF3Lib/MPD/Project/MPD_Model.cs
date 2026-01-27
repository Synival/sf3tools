using CommonLib.SGL;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Model : SGL_Model, IMPD_Model {
        public MPD_Model(IMPD_Model original) : base(original) {
            ModelID       = original.ModelID;
            LevelOfDetail = original.LevelOfDetail;
            Collection    = original.Collection;
        }

        public int ModelID { get; }
        public int LevelOfDetail { get; }
        public MPD_CollectionType Collection { get; }
    }
}
