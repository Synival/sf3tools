using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_ModelCollectionModels : IReadOnlyList<IMPD_Model> {
        public MPD_ModelCollectionModels() {
            _models = new List<MPD_Model>();
        }

        public MPD_ModelCollectionModels(IReadOnlyList<IMPD_Model> original) {
            _models = original.Select(x => new MPD_Model(x)).ToList();
        }

        private List<MPD_Model> _models;

        public int Count => _models.Count;
        public IMPD_Model this[int index] => _models[index];
        public IEnumerator<IMPD_Model> GetEnumerator() => _models.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _models.GetEnumerator();
    }
}
