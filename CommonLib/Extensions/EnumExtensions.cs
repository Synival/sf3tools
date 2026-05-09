using System;
using System.Linq;
using CommonLib.Attributes;

namespace CommonLib.Extensions {
    public static class EnumExtensions {
        public static TAttr GetAttribute<TEnum, TAttr>(this TEnum e) where TEnum : Enum where TAttr : Attribute {
            var enumType = typeof(TEnum);
            var valueString = e.ToString();

            var memberInfos = enumType.GetMember(valueString);
            var valueMemberInfo = memberInfos.FirstOrDefault(x => x.DeclaringType == enumType);
            if (valueMemberInfo == null)
                return null;

            return (TAttr) Attribute.GetCustomAttribute(valueMemberInfo, typeof(EnumDisplayNameAttribute));
        }

        public static string GetDisplayName<T>(this T e) where T : Enum
            => e.GetAttribute<T, EnumDisplayNameAttribute>()?.DisplayName ?? e.ToString();
    }
}
