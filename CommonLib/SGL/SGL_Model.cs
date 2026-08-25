using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;

namespace CommonLib.SGL {
    public class SGL_Model : ISGL_Model {
        public SGL_Model(int modelCollectionId, int modelId, int levelOfDetail) {
            ModelCollectionID = modelCollectionId;
            ModelID           = modelId;
            LevelOfDetail     = levelOfDetail;
            Vertices          = new VECTOR[0];
            Faces             = new ISGL_ModelFace[0];
            VertexNormals     = new VECTOR[0];
        }

        public SGL_Model(ISGL_Model original)
        : this(original, original.ModelCollectionID, original.ModelID, original.LevelOfDetail)
        {}

        public SGL_Model(ISGL_Model original, int modelCollectionId, int modelId, int levelOfDetail) {
            ModelCollectionID = modelCollectionId;
            ModelID       = modelId;
            LevelOfDetail = levelOfDetail;

            if (original.Vertices != null)
                Vertices = original.Vertices.Select(x => new VECTOR(x)).ToArray();
            if (original.Faces != null)
                Faces = original.Faces.Select(x => (ISGL_ModelFace) (new SGL_ModelFace(x))).ToArray();
            if (original.VertexNormals != null)
                VertexNormals = original.VertexNormals.Select(x => new VECTOR(x)).ToArray();
        }

        public SGL_Model(int modelId, int levelOfDetail, IEnumerable<VECTOR> vertices, IEnumerable<ISGL_ModelFace> faces, IEnumerable<VECTOR> vertexNormals) {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            ModelID       = modelId;
            LevelOfDetail = levelOfDetail;
            Vertices      = vertices.ToArray();
            Faces         = faces.ToArray();
            VertexNormals = vertexNormals?.ToArray();
        }

        public static SGL_Model FromJToken(JToken token) => new SGL_Model((JObject) token);
        public static SGL_Model FromJObject(JObject jObject) => new SGL_Model(jObject);
        public static SGL_Model FromJObject(JObject jObject, int modelCollectionId, int modelId, int levelOfDetail)
            => new SGL_Model(jObject, modelCollectionId, modelId, levelOfDetail);

        private SGL_Model(JObject jObject) :
        this(
            jObject,
            (int?) jObject.GetValueIfExists("ModelCollectionID", t => (JValue) t) ?? 0,
            (int?) jObject.GetValueIfExists("ModelID",           t => (JValue) t) ?? 0,
            (int?) jObject.GetValueIfExists("LevelOfDetail",     t => (JValue) t) ?? 0
        ) {
        }

        private SGL_Model(JObject jObject, int modelCollectionId, int modelId, int levelOfDetail) {
            ModelCollectionID = modelCollectionId;
            ModelID           = modelId;
            LevelOfDetail     = levelOfDetail;

            Vertices = jObject.GetValueIfExists("Vertices", t => ((JArray) t).Select(x => VECTOR.FromJToken(x)).ToArray());
            Faces    = jObject.GetValueIfExists("Faces",    t => ((JArray) t).Select(x => (ISGL_ModelFace) SGL_ModelFace.FromJToken(x)).ToArray());
            VertexNormals = jObject.GetValueIfExists("VertexNormals", t => ((JArray) t).Select(x => VECTOR.FromJToken(x)).ToArray());
        }

        public int ModelCollectionID { get; }
        public int ModelID { get; }
        public int LevelOfDetail { get; }

        public IReadOnlyList<VECTOR> Vertices { get; }
        public IReadOnlyList<ISGL_ModelFace> Faces { get; }
        public IReadOnlyList<VECTOR> VertexNormals { get; }
    }
}
