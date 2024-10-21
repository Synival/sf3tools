namespace SF3.Values
{
    public class StatTypeValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "StatTypes.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for +stat up type that can be bound to an ObjectListView.
    /// </summary>
    public class StatTypeValue : NamedValueFromResource<StatTypeValue, StatTypeValueResourceInfo>
    {
        public StatTypeValue(int value) : base(value)
        {
        }
    }
}
