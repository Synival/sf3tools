using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_ModelCollectionModelsWithLoD : IEnumerableWithLength<IMPD_ModelWithLoD> {
        public MPD_ModelCollectionModelsWithLoD(IEnumerableWithLength<IMPD_ModelWithLoD> original) {
            _models = original.Select(x => new MPD_ModelWithLoD(x)).ToList();
        }

        private List<MPD_ModelWithLoD> _models;

        public int Length => _models.Count;
        public IEnumerator<IMPD_ModelWithLoD> GetEnumerator() => _models.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _models.GetEnumerator();
    }
}
