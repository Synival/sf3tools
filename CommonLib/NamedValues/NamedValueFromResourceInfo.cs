using System.Collections.Generic;
using static CommonLib.Utils.ResourceUtils;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Named value info for names that come from a resource.
    /// </summary>
    public class NamedValueFromResourceInfo : NamedValueInfo, INamedValueFromResourceInfo {
        public NamedValueFromResourceInfo(string resourceName, int minValue = 0x00, int maxValue = 0xFF, string formatString = "X2")
        : base(minValue, maxValue, formatString, GetValueNameDictionaryFromXML("Resources/" + resourceName))
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; }
    };
}
