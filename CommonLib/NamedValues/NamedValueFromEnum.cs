using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Named value info for names that are defined from an enum.
    /// </summary>
    public class NamedValueFromEnum<T> : NamedValueInfo, INamedValueInfo where T : Enum {
        public NamedValueFromEnum(int minValue = 0x00, int maxValue = 0xFF, string formatString = "X2")
        : base(minValue, maxValue, formatString, EnumToNamedValueDictionary()) {
        }

        private static Dictionary<int, string> EnumToNamedValueDictionary() {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(x => Convert.ToInt32(x), x => x.ToString());
        }
    };
}
