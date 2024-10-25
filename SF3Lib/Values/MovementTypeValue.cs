namespace SF3.Values {
    public class MovementTypeValueResourceInfo : NamedValueFromResourceInfo {
        public MovementTypeValueResourceInfo() : base("MovementTypes.xml") {
        }
    }

    /// <summary>
    /// Named value for movement type that can be bound to an ObjectListView.
    /// </summary>
    public class MovementTypeValue : NamedValueFromResource<MovementTypeValueResourceInfo> {
        public MovementTypeValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new MovementTypeValue(value);
    }
}
