using SF3.Types;

namespace SF3.Values
{
    public class SpecialValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Specials.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for Special Attack ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpecialValue : NamedValueFromResourceForScenarios<SpecialValue, SpecialValueResourceInfo>
    {
        public SpecialValue(ScenarioType scenario, int value) : base(scenario, value)
        {
        }
    }
}
