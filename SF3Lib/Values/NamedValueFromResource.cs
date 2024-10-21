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
        Dictionary<int, string> PossibleValues { get; }
        Dictionary<NamedValue, string> ComboBoxValues { get; set; }
    };

    public class NamedValueFromResourceInfo : INamedValueFromResourceInfo
    {
        public NamedValueFromResourceInfo(string resourceName)
        {
            ResourceName = resourceName;
            PossibleValues = GetValueNameDictionaryFromXML("Resources/" + ResourceName);
        }

        public string ResourceName { get; }
        public virtual int MinValue => 0x00;
        public virtual int MaxValue => 0xFF;
        public Dictionary<int, string> PossibleValues { get; }
        public Dictionary<NamedValue, string> ComboBoxValues { get; set; } = null;
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public abstract class NamedValueFromResource<TResourceInfo> : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new()
    {
        public NamedValueFromResource(int value)
        : base(
            NameOrHexValue(value, ResourceInfo.PossibleValues),
            HexValueWithName(value, ResourceInfo.PossibleValues),
            value
        )
        {
        }

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public override int MinValue => ResourceInfo.MinValue;
        public override int MaxValue => ResourceInfo.MaxValue;

        public override Dictionary<int, string> PossibleValues => ResourceInfo.PossibleValues;

        public override Dictionary<NamedValue, string> ComboBoxValues
        {
            get
            {
                if (ResourceInfo.ComboBoxValues == null)
                {
                    ResourceInfo.ComboBoxValues = MakeNamedValueComboBoxValues(this);
                }
                return ResourceInfo.ComboBoxValues;
            }
        }
    }
}
