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
        Dictionary<int, string> ValueNames { get; }
    };

    public class NamedValueFromResourceInfo : INamedValueFromResourceInfo
    {
        public NamedValueFromResourceInfo(string resourceName)
        {
            ResourceName = resourceName;
            ValueNames = GetValueNameDictionaryFromXML("Resources/" + ResourceName);
        }

        public string ResourceName { get; }
        public virtual int MinValue => 0x00;
        public virtual int MaxValue => 0xFF;
        public Dictionary<int, string> ValueNames { get; }
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public class NamedValueFromResource<TSelf, TResourceInfo> : NamedValue
        where TSelf : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new()
    {
        public NamedValueFromResource(int value) : base(NameOrHexValue(value, ValueNames), HexValueWithName(value, ValueNames), value)
        {
        }

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();
        public static readonly int MinValue = ResourceInfo.MinValue;
        public static readonly int MaxValue = ResourceInfo.MaxValue;
        public static Dictionary<int, string> ValueNames => ResourceInfo.ValueNames;

        private static readonly Dictionary<NamedValue, string> _comboBoxValues = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => (TSelf)Activator.CreateInstance(typeof(TSelf), value));

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues;
    }
}
