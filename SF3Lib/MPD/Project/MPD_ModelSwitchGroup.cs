using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_ModelSwitchGroup : IMPD_ModelSwitchGroup {
        public MPD_ModelSwitchGroup(IMPD_ModelSwitchGroup original) {
            Flag = original.Flag;
            if (original.ModelInstancesVisibleWhenOff != null)
                ModelInstancesVisibleWhenOff = original.ModelInstancesVisibleWhenOff.ToArray();
            if (original.ModelInstancesVisibleWhenOn != null)
                ModelInstancesVisibleWhenOn = original.ModelInstancesVisibleWhenOn.ToArray();
        }

        public static MPD_ModelSwitchGroup FromJToken(JToken token) => new MPD_ModelSwitchGroup(token);
        private MPD_ModelSwitchGroup(JToken token) {
            var jObject = (JObject) token;

            Flag = (int) jObject["Flag"];
            ModelInstancesVisibleWhenOff = jObject.GetValueIfExists("ModelInstancesVisibleWhenOff", t => ((JArray) t).Select(x => (int) x).ToArray());
            ModelInstancesVisibleWhenOn  = jObject.GetValueIfExists("ModelInstancesVisibleWhenOn",  t => ((JArray) t).Select(x => (int) x).ToArray());
        }

        public int Flag { get; set; }
        public IReadOnlyList<int> ModelInstancesVisibleWhenOff { get; }
        public IReadOnlyList<int> ModelInstancesVisibleWhenOn { get; }
        public bool StateInEditor { get; set; }
    }
}
