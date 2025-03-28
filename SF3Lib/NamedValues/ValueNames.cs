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
        public static readonly NamedValueFromEnum<EventActionInspectType> EventActionInspectInfo
            = new NamedValueFromEnum<EventActionInspectType>();
        public static readonly NamedValueFromEnum<EventActionInspectFlags> EventActionInspectFlagsInfo
            = new NamedValueFromEnum<EventActionInspectFlags>();
        public static readonly NamedValueFromEnum<EventActionNpcTalkType> EventActionNpcTalkInfo
            = new NamedValueFromEnum<EventActionNpcTalkType>();
        public static readonly NamedValueFromEnum<EventTriggerDirectionType> EventTriggerDirectionInfo
            = new NamedValueFromEnum<EventTriggerDirectionType>(maxValue: 0x0F);
        public static readonly NamedValueFromEnum<EventTriggerInspectType> EventTriggerInspectTypeInfo
            = new NamedValueFromEnum<EventTriggerInspectType>(maxValue: 0xFF);
        public static readonly NamedValueFromEnum<EventTriggerMoveOnTileType> EventTriggerMoveOnTileInfo
            = new NamedValueFromEnum<EventTriggerMoveOnTileType>(maxValue: 0x0F);
        public static readonly NamedValueFromEnum<EventTriggerType> EventTriggerTypeInfo
            = new NamedValueFromEnum<EventTriggerType>(maxValue: 0x0F);
        public static readonly NamedValueFromEnum<EventTriggerUseItemType> EventTriggerUseItemTypeInfo
            = new NamedValueFromEnum<EventTriggerUseItemType>(maxValue: 0x0F);
        public static readonly NamedValueFromEnum<EventTriggerWarpSoundType> EventTriggerWarpSoundInfo
            = new NamedValueFromEnum<EventTriggerWarpSoundType>(maxValue: 0x0F);
        public static readonly NamedValueFromResourceForScenariosInfo FileIndexInfo
            = new NamedValueFromResourceForScenariosInfo("FileIndexes.xml", formatString: "X4");
        public static readonly NamedValueFromResourceInfo FriendshipBonusTypeInfo
            = new NamedValueFromResourceInfo("FriendshipBonusTypeList.xml");
        public static readonly NamedValueFromResourceInfo GameFlagInfo
            = new NamedValueFromResourceInfo("GameFlags.xml");
        public static readonly NamedValueFromResourceForScenariosInfo ItemInfo
            = new NamedValueFromResourceForScenariosInfo("Items.xml");
        public static readonly NamedValueFromResourceForScenariosInfo LoadInfo
            = new NamedValueFromResourceForScenariosInfo("LoadList.xml");
        public static readonly NamedValueFromResourceForScenariosInfo MonsterInfo
            = new NamedValueFromResourceForScenariosInfo("Monsters.xml");
        public static readonly MonsterForSlotInfo MonsterForSlotInfo
            = new MonsterForSlotInfo();
        public static readonly NamedValueFromEnum<MovementType> MovementTypeInfo
            = new NamedValueFromEnum<MovementType>();
        public static readonly NamedValueFromResourceInfo SexInfo
            = new NamedValueFromResourceInfo("Sexes.xml");
        public static readonly NamedValueFromEnum<SpawnType> SpawnTypeInfo
            = new NamedValueFromEnum<SpawnType>();
        public static readonly NamedValueFromResourceInfo SpecialEffectInfo
            = new NamedValueFromResourceInfo("SpecialEffects.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpecialInfo
            = new NamedValueFromResourceForScenariosInfo("Specials.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpellInfo
            = new NamedValueFromResourceForScenariosInfo("Spells.xml");
        public static readonly NamedValueFromResourceInfo SpellTargetInfo
            = new NamedValueFromResourceInfo("SpellTargets.xml");
        public static readonly NamedValueFromResourceForScenariosInfo SpriteInfo
            = new NamedValueFromResourceForScenariosInfo("Sprites.xml");
        public static readonly NamedValueFromResourceInfo StatTypeInfo
            = new NamedValueFromResourceInfo("StatTypes.xml");
        public static readonly NamedValueFromResourceForScenariosInfo WeaponSpellInfo
            = new NamedValueFromResourceForScenariosInfo("WeaponSpells.xml");
        public static readonly NamedValueFromResourceInfo WeaponTypeInfo
            = new NamedValueFromResourceInfo("WeaponTypes.xml");
    }
}
