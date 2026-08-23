using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;

namespace CommonLib.SGL {
    public class SGL_Model : ISGL_Model {
        public SGL_Model(int modelId, int levelOfDetail = 0) {
            Vertices = new VECTOR[0];
            Faces    = new ISGL_ModelFace[0];
            ModelID  = modelId;
            LevelOfDetail = levelOfDetail;
        }

        public SGL_Model(ISGL_Model original)
        : this(original, original.ModelID, original.LevelOfDetail)
        {}

        public SGL_Model(ISGL_Model original, int modelId, int levelOfDetail = 0) {
            ModelID       = modelId;
            LevelOfDetail = levelOfDetail;

            if (original.Vertices != null) {
                Vertices = original.Vertices.Select(x => new VECTOR(x)).ToArray();

                var left   = Vertices.Select(x => x.X.Float).Min();
                var right  = Vertices.Select(x => x.X.Float).Max();
                var top    = Vertices.Select(x => x.Y.Float).Min();
                var bottom = Vertices.Select(x => x.Y.Float).Max();
                var front  = Vertices.Select(x => x.Z.Float).Min();
                var back   = Vertices.Select(x => x.Z.Float).Max();
            }
            if (original.Faces != null)
                Faces = original.Faces.Select(x => (ISGL_ModelFace) (new SGL_ModelFace(x))).ToArray();
        }

        public SGL_Model(int modelId, int levelOfDetail, IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            ModelID       = modelId;
            LevelOfDetail = levelOfDetail;
            Vertices      = vertices.ToArray();
            Faces         = faces.ToArray();
        }

        public static SGL_Model FromJToken(JToken token) => new SGL_Model((JObject) token);
        public static SGL_Model FromJObject(JObject jObject) => new SGL_Model(jObject);
        public static SGL_Model FromJObject(JObject jObject, int modelId, int levelOfDetail) => new SGL_Model(jObject, modelId, levelOfDetail);

        private SGL_Model(JObject jObject) :
        this(
            jObject,
            (int?) jObject.GetValueIfExists("ModelID",        t => (JValue) t) ?? 0,
            (int?) jObject.GetValueIfExists("LevelOfDetail",  t => (JValue) t) ?? 0
        ) {
        }

        private SGL_Model(JObject jObject, int modelId, int levelOfDetail) {
            Vertices      = jObject.GetValueIfExists("Vertices", t => ((JArray) t).Select(x => VECTOR.FromJToken(x)).ToArray());
            Faces         = jObject.GetValueIfExists("Faces",    t => ((JArray) t).Select(x => (ISGL_ModelFace) SGL_ModelFace.FromJToken(x)).ToArray());
            ModelID       = modelId;
            LevelOfDetail = levelOfDetail;
        }

        public IReadOnlyList<VECTOR> Vertices { get; }
        public IReadOnlyList<ISGL_ModelFace> Faces { get; }
        public int ModelID { get; }
        public int LevelOfDetail { get; }
    }
}
