namespace SF3.Values
{
    public class CharacterClassValueResourceInfo : NamedValueFromResourceInfo
    {
        public CharacterClassValueResourceInfo() : base("CharacterClasses.xml")
        {
        }
    }

    /// <summary>
    /// Named value for CharacterClass that can be bound to an ObjectListView.
    /// </summary>
    public class CharacterClassValue : NamedValueFromResource<CharacterClassValue, CharacterClassValueResourceInfo>
    {
        public CharacterClassValue(int value) : base(value)
        {
        }
    }
}
