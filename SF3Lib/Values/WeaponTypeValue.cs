namespace SF3.Values {
    public class WeaponTypeValueResourceInfo : NamedValueFromResourceInfo {
        public WeaponTypeValueResourceInfo() : base("WeaponTypes.xml") {
        }
    }

    /// <summary>
    /// Named value for WeaponType that can be bound to an ObjectListView.
    /// </summary>
    public class WeaponTypeValue : NamedValueFromResource<WeaponTypeValueResourceInfo> {
        public WeaponTypeValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new WeaponTypeValue(value);
    }
}
