using CommonLib.NamedValues;

namespace SF3.Values {
    public class DroprateValueResourceInfo : NamedValueFromResourceInfo {
        public DroprateValueResourceInfo() : base("DroprateList.xml") {
        }
    }

    /// <summary>
    /// Named value for DroprateValue that can be bound to an ObjectListView.
    /// </summary>
    public class DroprateValue : NamedValueFromResource<DroprateValueResourceInfo> {
        public DroprateValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new DroprateValue(value);
    }
}
