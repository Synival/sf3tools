using CommonLib.NamedValues;
namespace SF3.Values {
    public class SexValueResourceInfo : NamedValueFromResourceInfo {
        public SexValueResourceInfo() : base("Sexes.xml") {
        }
    }

    /// <summary>
    /// Named value for sex that can be bound to an ObjectListView.
    /// </summary>
    public class SexValue : NamedValueFromResource<SexValueResourceInfo> {
        public SexValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new SexValue(value);
    }
}
