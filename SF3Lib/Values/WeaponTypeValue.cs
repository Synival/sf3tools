namespace SF3.Values
{
    public class WeaponTypeValueResourceInfo : NamedValueFromResourceInfo
    {
        public WeaponTypeValueResourceInfo() : base("WeaponTypes.xml")
        {
        }
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
