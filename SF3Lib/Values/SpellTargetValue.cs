namespace SF3.Values {
    public class SpellTargetValueResourceInfo : NamedValueFromResourceInfo {
        public SpellTargetValueResourceInfo() : base("SpellTargets.xml") {
        }
    }

    /// <summary>
    /// Named value for SpellTarget that can be bound to an ObjectListView.
    /// </summary>
    public class SpellTargetValue : NamedValueFromResource<SpellTargetValueResourceInfo> {
        public SpellTargetValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new SpellTargetValue(value);
    }
}
