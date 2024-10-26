using CommonLib.NamedValues;

namespace SF3.Values {
    public class ElementValueResourceInfo : NamedValueFromResourceInfo {
        public ElementValueResourceInfo() : base("Elements.xml") {
        }
    }

    /// <summary>
    /// Named value for Element that can be bound to an ObjectListView.
    /// </summary>
    public class ElementValue : NamedValueFromResource<ElementValueResourceInfo> {
        public ElementValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new ElementValue(value);
    }
}
