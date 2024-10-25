using SF3.Types;

namespace SF3.Values {
    public class SpellValueResourceInfo : NamedValueFromResourceForScenariosInfo {
        public SpellValueResourceInfo() : base("Spells.xml") {
        }
    }

    /// <summary>
    /// Named value for Spell ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpellValue : NamedValueFromResourceForScenarios<SpellValueResourceInfo> {
        public SpellValue(ScenarioType scenario, int value) : base(scenario, value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new SpellValue(Scenario, value);
    }
}
