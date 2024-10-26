using System.Collections.Generic;
using static CommonLib.Utils.ControlUtils;
using static CommonLib.Utils.ResourceUtils;
using static CommonLib.Utils.Utils;

namespace CommonLib.NamedValues {
    public interface INamedValueFromResourceInfo {
        string ResourceName { get; }
        int MinValue { get; }
        int MaxValue { get; }
        string FormatString { get; }
        Dictionary<int, string> PossibleValues { get; }
        Dictionary<NamedValue, string> ComboBoxValues { get; set; }
    };

    public class NamedValueFromResourceInfo : INamedValueFromResourceInfo {
        public NamedValueFromResourceInfo(string resourceName) {
            ResourceName = resourceName;
            PossibleValues = GetValueNameDictionaryFromXML("Resources/" + ResourceName);
        }

        public string ResourceName { get; }
        public virtual int MinValue => 0x00;
        public virtual int MaxValue => 0xFF;
        public virtual string FormatString => "X2";
        public Dictionary<int, string> PossibleValues { get; }
        public Dictionary<NamedValue, string> ComboBoxValues { get; set; } = null;
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public abstract class NamedValueFromResource<TResourceInfo> : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new() {
        public NamedValueFromResource(int value)
        : base(
            NameOrHexValue(value, ResourceInfo.PossibleValues, ResourceInfo.FormatString),
            HexValueWithName(value, ResourceInfo.PossibleValues, ResourceInfo.FormatString),
            value
        ) {
        }

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public override int MinValue => ResourceInfo.MinValue;
        public override int MaxValue => ResourceInfo.MaxValue;

        public override Dictionary<int, string> PossibleValues => ResourceInfo.PossibleValues;

        public override Dictionary<NamedValue, string> ComboBoxValues {
            get {
                if (ResourceInfo.ComboBoxValues == null)
                    ResourceInfo.ComboBoxValues = MakeNamedValueComboBoxValues(this);
                return ResourceInfo.ComboBoxValues;
            }
        }
    }
}
