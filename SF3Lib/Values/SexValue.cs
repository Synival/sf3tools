namespace SF3.Values
{
    public class SexValueResourceInfo : NamedValueFromResourceInfo
    {
        public SexValueResourceInfo() : base("Sexes.xml")
        {
        }
    }

    /// <summary>
    /// Named value for Sex that can be bound to an ObjectListView.
    /// </summary>
    public class SexValue : NamedValueFromResource<SexValue, SexValueResourceInfo>
    {
        public SexValue(int value) : base(value)
        {
        }
    }
}
