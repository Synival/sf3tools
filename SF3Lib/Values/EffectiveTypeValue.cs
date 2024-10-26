using CommonLib.NamedValues;

namespace SF3.Values {
    public class EffectiveTypeValueResourceInfo : NamedValueFromResourceInfo {
        public EffectiveTypeValueResourceInfo() : base("EffectiveTypes.xml") {
        }
    }

    /// <summary>
    /// Named value for EffectiveType that can be bound to an ObjectListView.
    /// </summary>
    public class EffectiveTypeValue : NamedValueFromResource<EffectiveTypeValueResourceInfo> {
        public EffectiveTypeValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new EffectiveTypeValue(value);
    }
}
