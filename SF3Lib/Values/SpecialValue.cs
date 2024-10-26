using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.Values {
    public class SpecialValueResourceInfo : NamedValueFromResourceForScenariosInfo {
        public SpecialValueResourceInfo() : base("Specials.xml") {
        }
    }

    /// <summary>
    /// Named value for Special Attack ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpecialValue : NamedValueFromResourceForScenarios<SpecialValueResourceInfo> {
        public SpecialValue(ScenarioType scenario, int value) : base(scenario, value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new SpecialValue(Scenario, value);
    }
}
