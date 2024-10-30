using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    public static class ValueNames {
        public static readonly NamedValueFromResourceForScenariosInfo CharacterInfo
            = new NamedValueFromResourceForScenariosInfo("Characters.xml");
        public static readonly NamedValueFromResourceInfo CharacterClassInfo
            = new NamedValueFromResourceInfo("CharacterClasses.xml");
        public static readonly NamedValueFromResourceInfo DroprateInfo
            = new NamedValueFromResourceInfo("DroprateList.xml");
        public static readonly NamedValueFromResourceInfo EffectiveTypeInfo
            = new NamedValueFromResourceInfo("EffectiveTypes.xml");
        public static readonly NamedValueFromResourceInfo ElementInfo
            = new NamedValueFromResourceInfo("Elements.xml");
        public static readonly NamedValueFromResourceForScenariosInfo FileIndexInfo
            = new NamedValueFromResourceForScenariosInfo("FileIndexes.xml", formatString: "X4");
        public static readonly NamedValueFromResourceInfo FriendshipBonusTypeInfo
            = new NamedValueFromResourceInfo("FriendshipBonusTypeList.xml");
        public static readonly NamedValueFromResourceForScenariosInfo ItemInfo
            = new NamedValueFromResourceForScenariosInfo("Items.xml");
        public static readonly NamedValueFromResourceForScenariosInfo MonsterInfo
            = new NamedValueFromResourceForScenariosInfo("Monsters.xml");
        public static readonly NamedValueFromResourceInfo MovementTypeInfo
            = new NamedValueFromResourceInfo("MovementTypes.xml");
        public static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpecialInfo
            = new NamedValueFromResourceForScenariosInfo("Specials.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpellInfo
            = new NamedValueFromResourceForScenariosInfo("Spells.xml");
        public static readonly NamedValueFromResourceInfo SpellTargetInfo
            = new NamedValueFromResourceInfo("SpellTargets.xml");
        public static readonly NamedValueFromResourceInfo StatTypeInfo
            = new NamedValueFromResourceInfo("StatTypes.xml");
        public static readonly NamedValueFromResourceInfo WeaponTypeInfo
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
        public static NameAndInfo GetMonsterName(ScenarioType scenario, int value)
            => new NameAndInfo(value, MonsterInfo.Info[scenario]);
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
