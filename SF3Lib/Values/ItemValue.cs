using SF3.Types;

namespace SF3.Values
{
    public class ItemValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Items.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for Item ID's that can be bound to an ObjectListView.
    /// </summary>
    public class ItemValue : NamedValueFromResourceForScenarios<ItemValue, ItemValueResourceInfo>
    {
        public ItemValue(ScenarioType scenario, int value) : base(scenario, value)
        {
        }
    }
}
