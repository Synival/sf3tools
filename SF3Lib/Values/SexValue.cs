namespace SF3.Values
{
    public class SexValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Sexes.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
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
