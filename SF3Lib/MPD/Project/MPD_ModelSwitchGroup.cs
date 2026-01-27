using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_ModelSwitchGroup : IMPD_ModelSwitchGroup {
        public MPD_ModelSwitchGroup(IMPD_ModelSwitchGroup original) {
            Flag = original.Flag;
            if (original.ModelInstancesVisibleWhenOff != null)
                ModelInstancesVisibleWhenOff = original.ModelInstancesVisibleWhenOff.ToArray().ToEnumerableWithLength();
            if (original.ModelInstancesVisibleWhenOn != null)
                ModelInstancesVisibleWhenOn = original.ModelInstancesVisibleWhenOn.ToArray().ToEnumerableWithLength();
        }

        public int Flag { get; set; }
        public IEnumerableWithLength<int> ModelInstancesVisibleWhenOff { get; }
        public IEnumerableWithLength<int> ModelInstancesVisibleWhenOn { get; }
        public bool StateInEditor { get; set; }
    }
}
