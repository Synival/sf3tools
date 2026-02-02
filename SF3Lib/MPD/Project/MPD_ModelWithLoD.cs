using System;
using System.Collections;
using System.Collections.Generic;
using CommonLib;
using CommonLib.SGL;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelWithLoD : IMPD_ModelWithLoD {
        public MPD_ModelWithLoD(MPD_CollectionType collection, int modelID, int levelsOfDetail, ISGL_Model actualModel) {
            Collection     = collection;
            ModelID        = modelID;
            LevelsOfDetail = levelsOfDetail;
            _actualModel   = new SGL_Model(actualModel);
            Models         = new ModelRetriever(this);
        }

        public MPD_ModelWithLoD(IMPD_ModelWithLoD original) {
            Collection     = original.Collection;
            ModelID        = original.ModelID;
            LevelsOfDetail = original.LevelsOfDetail;
            _actualModel   = new SGL_Model(original.Models[0]);
            Models         = new ModelRetriever(this);
        }

        public MPD_CollectionType Collection { get; }
        public int ModelID { get; }

        public int LevelsOfDetail { get; }

        private SGL_Model _actualModel { get; }

        private class ModelRetriever : IIndexedEnumerableWithLength<IMPD_Model> {
            public ModelRetriever(MPD_ModelWithLoD modelWithLoD) {
                ModelWithLoD = modelWithLoD;
            }

            public IMPD_Model this[int index] {
                get {
                    if (index < 0 || index >= Length)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    if (!_models.ContainsKey(index))
                        _models[index] = new MPD_ModelLoD(ModelWithLoD._actualModel, ModelWithLoD.Collection, ModelWithLoD.ModelID, index);
                    return _models[index];
                }
            }

            public int Length => ModelWithLoD.LevelsOfDetail;

            public IMPD_Model[] AsArray() => throw new System.NotImplementedException();

            public IEnumerator<IMPD_Model> GetEnumerator() {
                var levelsOfDetail = Length;
                for (int i = 0; i < levelsOfDetail; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public MPD_ModelWithLoD ModelWithLoD { get; }
            private Dictionary<int, IMPD_Model> _models = new Dictionary<int, IMPD_Model>();
        }

        public IIndexedEnumerableWithLength<IMPD_Model> Models { get; }
    }
}
