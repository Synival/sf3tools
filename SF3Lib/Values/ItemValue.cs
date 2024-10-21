using SF3.Types;

namespace SF3.Values
{
    public class ItemValueResourceInfo : NamedValueFromResourceForScenariosInfo
    {
        public ItemValueResourceInfo() : base("Items.xml")
        {
        }
    }

    /// <summary>
    /// Named value for Item ID's that can be bound to an ObjectListView.
    /// </summary>
    public class ItemValue : NamedValueFromResourceForScenarios<ItemValueResourceInfo>
    {
        public ItemValue(ScenarioType scenario, int value) : base(scenario, value)
        {
        }

        public override NamedValue MakeRelatedValue(int value) => new ItemValue(Scenario, value);
    }
}
