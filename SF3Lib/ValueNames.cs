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
        private static readonly NamedValueFromResourceInfo ElementInfo
            = new NamedValueFromResourceInfo("Elements.xml");
        private static readonly NamedValueFromResourceForScenariosInfo FileIndexInfo
            = new NamedValueFromResourceForScenariosInfo("FileIndexes.xml", formatString: "X4");
        private static readonly NamedValueFromResourceInfo FriendshipBonusTypeInfo
            = new NamedValueFromResourceInfo("FriendshipBonusTypeList.xml");
        private static readonly NamedValueFromResourceForScenariosInfo ItemInfo
            = new NamedValueFromResourceForScenariosInfo("Items.xml");
        private static readonly NamedValueFromResourceInfo MovementTypeInfo
            = new NamedValueFromResourceInfo("MovementTypes.xml");
        private static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        private static readonly NamedValueFromResourceForScenariosInfo SpecialInfo
            = new NamedValueFromResourceForScenariosInfo("Specials.xml");
        private static readonly NamedValueFromResourceForScenariosInfo SpellInfo
            = new NamedValueFromResourceForScenariosInfo("Spells.xml");
        private static readonly NamedValueFromResourceInfo SpellTargetInfo
            = new NamedValueFromResourceInfo("SpellTargets.xml");
        private static readonly NamedValueFromResourceInfo StatTypeInfo
            = new NamedValueFromResourceInfo("StatTypes.xml");
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
        public static NameAndInfo GetElementName(int value)
            => new NameAndInfo(value, ElementInfo);
        public static NameAndInfo GetFileIndexName(ScenarioType scenario, int value)
            => new NameAndInfo(value, FileIndexInfo.Info[scenario]);
        public static NameAndInfo GetFriendshipBonusTypeName(int value)
            => new NameAndInfo(value, FriendshipBonusTypeInfo);
        public static NameAndInfo GetItemName(ScenarioType scenario, int value)
            => new NameAndInfo(value, ItemInfo.Info[scenario]);
        public static NameAndInfo GetMovementTypeName(int value)
            => new NameAndInfo(value, MovementTypeInfo);
        public static NameAndInfo GetSexName(int value)
            => new NameAndInfo(value, SexInfo);
        public static NameAndInfo GetSpecialName(ScenarioType scenario, int value)
            => new NameAndInfo(value, SpecialInfo.Info[scenario]);
        public static NameAndInfo GetSpellName(ScenarioType scenario, int value)
            => new NameAndInfo(value, SpellInfo.Info[scenario]);
        public static NameAndInfo GetSpellTargetName(int value)
            => new NameAndInfo(value, StatTypeInfo);
        public static NameAndInfo GetStatTypeName(int value)
            => new NameAndInfo(value, StatTypeInfo);
        public static NameAndInfo GetWeaponTypeName(int value)
            => new NameAndInfo(value, WeaponTypeInfo);
    }
}
