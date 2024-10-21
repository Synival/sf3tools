namespace SF3.Values
{
    public class WeaponTypeValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "WeaponTypes.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for WeaponType that can be bound to an ObjectListView.
    /// </summary>
    public class WeaponTypeValue : NamedValueFromResource<WeaponTypeValue, WeaponTypeValueResourceInfo>
    {
        public WeaponTypeValue(int value) : base(value)
        {
        }
    }
}
