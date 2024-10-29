using System;
using CommonLib.NamedValues;
using SF3.Types;
using SF3.Values;
using static CommonLib.Utils.Utils;

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

        public static string GetCharacterClassName(int value)
            => NameOrNull(value, CharacterClassInfo.PossibleValues);
        public static string GetItemName(ScenarioType scenario, int value)
            => NameOrNull(value, ItemInfo.PossibleValues[scenario]);
        public static string GetSexName(int value)
            => NameOrNull(value, SexInfo.PossibleValues);
        public static string GetWeaponTypeName(int value)
            => NameOrNull(value, WeaponTypeInfo.PossibleValues);
    }
}
