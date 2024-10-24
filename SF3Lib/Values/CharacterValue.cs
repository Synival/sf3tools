using SF3.Types;

namespace SF3.Values {
    public class CharacterValueResourceInfo : NamedValueFromResourceForScenariosInfo {
        public CharacterValueResourceInfo() : base("Characters.xml") {
        }
    }

    /// <summary>
    /// Named value for Character ID's that can be bound to an ObjectListView.
    /// </summary>
    public class CharacterValue : NamedValueFromResourceForScenarios<CharacterValueResourceInfo> {
        public CharacterValue(ScenarioType scenario, int value) : base(scenario, value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new CharacterValue(Scenario, value);
    }
}
