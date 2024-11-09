using CommonLib.NamedValues;

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
        public static readonly MonsterForSlotInfo MonsterForSlotInfo
            = new MonsterForSlotInfo();
        public static readonly NamedValueFromResourceInfo MovementTypeInfo
            = new NamedValueFromResourceInfo("MovementTypes.xml");
        public static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        public static readonly NamedValueFromResourceInfo SpecialEffectInfo
            = new NamedValueFromResourceInfo("SpecialEffects.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpecialInfo
            = new NamedValueFromResourceForScenariosInfo("Specials.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpellInfo
            = new NamedValueFromResourceForScenariosInfo("Spells.xml");
        public static readonly NamedValueFromResourceInfo SpellTargetInfo
            = new NamedValueFromResourceInfo("SpellTargets.xml");
        public static readonly NamedValueFromResourceInfo StatTypeInfo
            = new NamedValueFromResourceInfo("StatTypes.xml");
        public static readonly NamedValueFromResourceForScenariosInfo WeaponSpellInfo
            = new NamedValueFromResourceForScenariosInfo("WeaponSpells.xml");
        public static readonly NamedValueFromResourceInfo WeaponTypeInfo
            = new NamedValueFromResourceInfo("WeaponTypes.xml");
    }
}
