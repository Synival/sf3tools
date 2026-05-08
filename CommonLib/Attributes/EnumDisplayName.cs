using System;

namespace CommonLib.Attributes {
    public class EnumDisplayNameAttribute : Attribute {
        public EnumDisplayNameAttribute(string displayName) {
            DisplayName = displayName;
        }

        public readonly string DisplayName;
    }
}
