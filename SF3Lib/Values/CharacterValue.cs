using SF3.Types;

namespace SF3.Values
{
    public class CharacterValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Characters.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for Character ID's that can be bound to an ObjectListView.
    /// </summary>
    public class CharacterValue : NamedValueFromResourceForScenarios<CharacterValue, CharacterValueResourceInfo>
    {
        public CharacterValue(ScenarioType scenario, int value) : base(scenario, value)
        {
        }
    }
}
