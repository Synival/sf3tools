using System;
using System.Collections.Generic;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    public interface INamedValueFromResourceInfo
    {
        string ResourceName { get; }
        int MinValue { get; }
        int MaxValue { get; }
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public class NamedValueFromResource<TSelf, TResourceInfo> : NamedValue
        where TSelf : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new()
    {
        private static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public static readonly int MinValue = ResourceInfo.MinValue;
        public static readonly int MaxValue = ResourceInfo.MaxValue;

        public static readonly Dictionary<int, string> ValueNames = GetValueNameDictionaryFromXML("Resources/" + ResourceInfo.ResourceName);

        private static readonly Dictionary<NamedValue, string> _comboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => (TSelf)Activator.CreateInstance(typeof(TSelf), value));

        public NamedValueFromResource(int value) : base(NameOrHexValue(value, ValueNames), HexValueWithName(value, ValueNames), value)
        {
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues;
    }
}
