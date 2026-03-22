using System;
using System.Collections;
using System.Collections.Generic;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Model : IMPD_Model {
        public MPD_Model(MPD_CollectionType collection, int modelID, int levelsOfDetail, ISGL_Model actualModel) {
            Collection     = collection;
            ModelID        = modelID;
            LevelsOfDetail = levelsOfDetail;
            _actualModel   = new SGL_Model(actualModel);

            ModelLoDs      = new ModelRetriever(this);
        }

        public MPD_Model(IMPD_Model original) {
            Collection     = original.Collection;
            ModelID        = original.ModelID;
            LevelsOfDetail = original.LevelsOfDetail;
            _actualModel   = new SGL_Model(original.ModelLoDs[0]);

            ModelLoDs      = new ModelRetriever(this);
        }

        public static MPD_Model FromJToken(JToken token, MPD_CollectionType collection) => new MPD_Model(token, collection);
        private MPD_Model(JToken token, MPD_CollectionType collection) {
            Collection = collection;

            var jObject = (JObject) token;

            ModelID      = (int) jObject["ID"];
            _actualModel = SGL_Model.FromJObject(jObject);
            LevelsOfDetail = Collection.IsHeaderModelCollection() ? 1 : (int) jObject["LevelsOfDetail"];

            ModelLoDs = new ModelRetriever(this);
        }

        public MPD_CollectionType Collection { get; }
        public int ModelID { get; }

        public int LevelsOfDetail { get; }

        private SGL_Model _actualModel { get; }

        private class ModelRetriever : IReadOnlyList<IMPD_ModelLoD> {
            public ModelRetriever(MPD_Model modelWithLoD) {
                ModelWithLoD = modelWithLoD;
            }

            public IMPD_ModelLoD this[int index] {
                get {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    if (!_models.ContainsKey(index))
                        _models[index] = new MPD_ModelLoD(ModelWithLoD._actualModel, ModelWithLoD.Collection, ModelWithLoD.ModelID, index);
                    return _models[index];
                }
            }

            public int Count => ModelWithLoD.LevelsOfDetail;

            public IMPD_ModelLoD[] AsArray() => throw new System.NotImplementedException();

            public IEnumerator<IMPD_ModelLoD> GetEnumerator() {
                var levelsOfDetail = Count;
                for (int i = 0; i < levelsOfDetail; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public MPD_Model ModelWithLoD { get; }
            private Dictionary<int, IMPD_ModelLoD> _models = new Dictionary<int, IMPD_ModelLoD>();
        }

        public IReadOnlyList<IMPD_ModelLoD> ModelLoDs { get; }
    }
}
