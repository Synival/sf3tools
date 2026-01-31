using System;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Model : SGL_Model, IMPD_Model {
        public MPD_Model(IMPD_Model original) : base(original) {
            ModelID       = original.ModelID;
            LevelOfDetail = original.LevelOfDetail;
            Collection    = original.Collection;
        }

        public static MPD_Model FromJToken(JToken token) => new MPD_Model(token);
        private MPD_Model(JToken token) {
        }

        public int ModelID { get; }
        public int LevelOfDetail { get; }
        public MPD_CollectionType Collection { get; }
    }
}
