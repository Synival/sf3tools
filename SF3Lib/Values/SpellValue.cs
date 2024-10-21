using SF3.Types;

namespace SF3.Values
{
    public class SpellValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Spells.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for Spell ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpellValue : NamedValueFromResourceForScenarios<SpellValue, SpellValueResourceInfo>
    {
        public SpellValue(ScenarioType scenario, int value) : base(scenario, value)
        {
        }
    }
}
