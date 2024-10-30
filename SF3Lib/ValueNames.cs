using CommonLib;
using CommonLib.NamedValues;
using SF3.Types;
using SF3.Values;

namespace SF3 {
    public static class ValueNames {
        private static readonly NamedValueFromResourceInfo CharacterClassInfo
            = new NamedValueFromResourceInfo("CharacterClasses.xml");
        private static readonly NamedValueFromResourceForScenariosInfo ItemInfo
            = new NamedValueFromResourceForScenariosInfo("Items.xml");
        private static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        private static readonly NamedValueFromResourceInfo WeaponTypeInfo
            = new NamedValueFromResourceInfo("WeaponTypes.xml");

        public static NameAndInfo GetCharacterClassName(int value)
            => new NameAndInfo(value, CharacterClassInfo);
        public static NameAndInfo GetItemName(ScenarioType scenario, int value)
            => new NameAndInfo(value, ItemInfo.Info[scenario]);
        public static NameAndInfo GetSexName(int value)
            => new NameAndInfo(value, SexInfo);
        public static NameAndInfo GetWeaponTypeName(int value)
            => new NameAndInfo(value, WeaponTypeInfo);
    }
}
