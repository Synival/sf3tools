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

        public MPD_ModelCollectionModelInstances(IEnumerableWithLength<IMPD_ModelInstance> original, IMPD_ModelCollection newCollection) {
            _instances = original.Select(x => new MPD_ModelInstance(x, newCollection)).ToList();
        }

        private List<MPD_ModelInstance> _instances;

        public int Count => _instances.Count;
        public IEnumerator<IMPD_ModelInstance> GetEnumerator() => _instances.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
