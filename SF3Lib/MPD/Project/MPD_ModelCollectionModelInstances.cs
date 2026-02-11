using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_ModelCollectionModelInstances : IEnumerableWithLength<IMPD_ModelInstance> {
        public MPD_ModelCollectionModelInstances() {
            _instances = new List<MPD_ModelInstance>();
        }

        public MPD_ModelCollectionModelInstances(IEnumerableWithLength<IMPD_ModelInstance> original) {
            _instances = original.Select(x => new MPD_ModelInstance(x)).ToList();
        }

        private List<MPD_ModelInstance> _instances;

        public int Length => _instances.Count;
        public IEnumerator<IMPD_ModelInstance> GetEnumerator() => _instances.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
