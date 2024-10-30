using CommonLib;
using CommonLib.NamedValues;
using SF3.Types;
using SF3.Values;

namespace SF3 {
    public static class ValueNames {
        private static readonly NamedValueFromResourceForScenariosInfo CharacterInfo
            = new NamedValueFromResourceForScenariosInfo("Characters.xml");
        private static readonly NamedValueFromResourceInfo CharacterClassInfo
            = new NamedValueFromResourceInfo("CharacterClasses.xml");
        private static readonly NamedValueFromResourceInfo DroprateInfo
            = new NamedValueFromResourceInfo("DroprateList.xml");
        private static readonly NamedValueFromResourceInfo EffectiveTypeInfo
            = new NamedValueFromResourceInfo("EffectiveTypes.xml");
        private static readonly NamedValueFromResourceForScenariosInfo ItemInfo
            = new NamedValueFromResourceForScenariosInfo("Items.xml");
        private static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        private static readonly NamedValueFromResourceInfo WeaponTypeInfo
            = new NamedValueFromResourceInfo("WeaponTypes.xml");

        public static NameAndInfo GetCharacterName(ScenarioType scenario, int value)
            => new NameAndInfo(value, CharacterInfo.Info[scenario]);
        public static NameAndInfo GetCharacterClassName(int value)
            => new NameAndInfo(value, CharacterClassInfo);
        public static NameAndInfo GetDroprateName(int value)
            => new NameAndInfo(value, DroprateInfo);
        public static NameAndInfo GetEffectiveTypeName(int value)
            => new NameAndInfo(value, EffectiveTypeInfo);
        public static NameAndInfo GetItemName(ScenarioType scenario, int value)
            => new NameAndInfo(value, ItemInfo.Info[scenario]);
        public static NameAndInfo GetSexName(int value)
            => new NameAndInfo(value, SexInfo);
        public static NameAndInfo GetWeaponTypeName(int value)
            => new NameAndInfo(value, WeaponTypeInfo);
    }
}
