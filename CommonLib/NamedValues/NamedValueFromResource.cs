using System.Collections.Generic;
using CommonLib.Extensions;
using static CommonLib.Utils.ControlUtils;
using static CommonLib.Utils.ResourceUtils;
using static CommonLib.Utils.Utils;

namespace CommonLib.NamedValues {
    public interface INamedValueFromResourceInfo : INamedValueInfo {
        string ResourceName { get; }
        Dictionary<NamedValue, string> ComboBoxValues { get; set; }
    };

    public class NamedValueFromResourceInfo : NamedValueInfo, INamedValueFromResourceInfo {
        public NamedValueFromResourceInfo(string resourceName, int minValue = 0x00, int maxValue = 0xFF, string formatString = "X2")
        : base(minValue, maxValue, formatString, GetValueNameDictionaryFromXML("Resources/" + resourceName))
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; }
        public Dictionary<NamedValue, string> ComboBoxValues { get; set; } = null;
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public abstract class NamedValueFromResource<TResourceInfo> : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new() {
        public NamedValueFromResource(int value)
        : base(
            NameOrNull(value, ResourceInfo.Values),
            value.ToStringHex(ResourceInfo.FormatString, ""),
            value
        ) {
        }

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public override int MinValue => ResourceInfo.MinValue;
        public override int MaxValue => ResourceInfo.MaxValue;

        public override Dictionary<int, string> PossibleValues => ResourceInfo.Values;

        public override Dictionary<NamedValue, string> ComboBoxValues {
            get {
                if (ResourceInfo.ComboBoxValues == null)
                    ResourceInfo.ComboBoxValues = MakeNamedValueComboBoxValues(this);
                return ResourceInfo.ComboBoxValues;
            }
        }
    }
}
