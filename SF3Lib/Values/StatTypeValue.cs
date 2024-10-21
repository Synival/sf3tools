namespace SF3.Values
{
    public class StatTypeValueResourceInfo : NamedValueFromResourceInfo
    {
        public StatTypeValueResourceInfo() : base("StatTypes.xml")
        {
        }
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
