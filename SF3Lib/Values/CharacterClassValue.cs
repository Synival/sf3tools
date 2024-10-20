using System.Collections.Generic;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    public class CharacterClassValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "CharacterClasses.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
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
