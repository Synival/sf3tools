using System;
using System.Collections;
using System.Collections.Generic;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Model : IMPD_Model {
        public MPD_Model(MPD_CollectionType collection, int modelId, int levelsOfDetail, ISGL_Model actualModel) {
            _actualModel   = new SGL_Model(actualModel, (int) collection, modelId, levelsOfDetail);
            ModelLoDs      = new ModelRetriever(this);
        }

        public MPD_Model(IMPD_Model original) {
            _actualModel   = new SGL_Model(original.ModelLoDs[0], (int) original.Collection, original.ModelID, original.LevelsOfDetail);
            ModelLoDs      = new ModelRetriever(this);
        }

        public MPD_Model(IMPD_Model original, int modelId, int levelsOfDetail) {
            _actualModel   = new SGL_Model(original.ModelLoDs[0], (int) original.Collection, modelId, levelsOfDetail);
            ModelLoDs      = new ModelRetriever(this);
        }

        public static MPD_Model FromJToken(JToken token, MPD_CollectionType collection) => new MPD_Model(token, collection);
        private MPD_Model(JToken token, MPD_CollectionType collection) {
            var jObject        = (JObject) token;
            var modelId        = (int) jObject["ID"];
            var levelsOfDetail = collection.IsHeaderModelCollection() ? 1 : (int) jObject["LevelsOfDetail"];
            _actualModel       = SGL_Model.FromJObject(jObject, (int) collection, modelId, levelsOfDetail);
            ModelLoDs          = new ModelRetriever(this);
        }

        public MPD_CollectionType Collection => (MPD_CollectionType) _actualModel.ModelCollectionID;
        public int ModelID => _actualModel.ModelID;
        public int LevelsOfDetail => _actualModel.LevelOfDetail;

        private readonly SGL_Model _actualModel;

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
