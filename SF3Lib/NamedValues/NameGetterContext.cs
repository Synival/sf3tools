using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.NamedValues {
    /// <summary>
    /// Context for the NameGetter for SF3 values.
    /// </summary>
    public class NameGetterContext : INameGetterContext {
        private delegate NameAndInfo NameAndInfoGetter(object obj, PropertyInfo property, int value, object[] parameters);
        private delegate bool CanGetNameChecker(object obj, PropertyInfo property, int value, object[] parameters);
        private delegate bool CanGetInfoChecker(object obj, PropertyInfo property, object[] parameters);

        private static readonly CanGetNameChecker AlwaysCanGetName = new CanGetNameChecker((o, p, v, r) => true);
        private static readonly CanGetInfoChecker AlwaysCanGetInfo = new CanGetInfoChecker((o, p, r) => true);

        private struct SubMethods {
            public SubMethods(NameAndInfoGetter getNameAndInfo, CanGetNameChecker canGetName = null, CanGetInfoChecker canGetInfo = null) {
                GetNameAndInfo = getNameAndInfo;
                CanGetName     = canGetName ?? AlwaysCanGetName;
                CanGetInfo     = canGetInfo ?? AlwaysCanGetInfo;
            }

            public NameAndInfoGetter GetNameAndInfo { get; }
            public CanGetNameChecker CanGetName { get; }
            public CanGetInfoChecker CanGetInfo { get; }
        };

        private readonly Dictionary<NamedValueType, SubMethods> _nameGetters;

        public NameGetterContext(ScenarioType scenario) {
            Scenario = scenario;

            _nameGetters = new Dictionary<NamedValueType, SubMethods>() {
                { NamedValueType.ActorScript,
                    new SubMethods(
                        (o, p, v, a) => GetActorScriptNameAndInfo(o, p, v, a),
                        AlwaysCanGetName,
                        AlwaysCanGetInfo
                    ) },

                { NamedValueType.BattleSceneById,     new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.BattleSceneByIdInfo)) },
                { NamedValueType.Character,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.CharacterInfo.Info[Scenario])) },
                { NamedValueType.CharacterClass,      new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.CharacterClassInfo)) },
                { NamedValueType.Droprate,            new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.DroprateInfo)) },
                { NamedValueType.EffectiveType,       new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EffectiveTypeInfo)) },
                { NamedValueType.Element,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.ElementInfo)) },
                { NamedValueType.EventActionInspect,  new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventActionInspectInfo)) },
                { NamedValueType.EventActionInspectFlags, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventActionInspectFlagsInfo)) },
                { NamedValueType.EventActionNpcTalk,  new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventActionNpcTalkInfo)) },
                { NamedValueType.EventTriggerDirection, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerDirectionInfo)) },
                { NamedValueType.EventTriggerInspectType, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerInspectTypeInfo)) },
                { NamedValueType.EventTriggerMoveOnTileType, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerMoveOnTileInfo)) },
                { NamedValueType.EventTriggerType,    new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerTypeInfo)) },
                { NamedValueType.EventTriggerUseItemType, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerUseItemTypeInfo)) },
                { NamedValueType.EventTriggerWarpSound, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EventTriggerWarpSoundInfo)) },
                { NamedValueType.FileIndex,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.FileIndexInfo.Info[Scenario])) },
                { NamedValueType.FileIndexWithFFFFFFFF, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.FileIndexWithFFFFFFFFInfo.Info[Scenario])) },
                { NamedValueType.FriendshipBonusType, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.FriendshipBonusTypeInfo)) },
                { NamedValueType.GameFlag,            new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.GameFlagInfo)) },
                { NamedValueType.InteractDirectionBehavior, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.InteractDirectionBehaviorInfo)) },
                { NamedValueType.Item,                new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.ItemInfo.Info[Scenario])) },
                { NamedValueType.Load,                new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.LoadInfo.Info[Scenario])) },
                { NamedValueType.MagicBonus,          new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MagicBonusInfo.Info[Scenario])) },
                { NamedValueType.ModelDirection,      new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.ModelDirection)) },
                { NamedValueType.Monster,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MonsterInfo.Info[Scenario])) },
                { NamedValueType.MonsterForSlot,      new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MonsterForSlotInfo.Info[Scenario])) },
                { NamedValueType.MovementType,        new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MovementTypeInfo)) },
                { NamedValueType.Music,               new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MusicInfo.Info[Scenario])) },
                { NamedValueType.Sex,                 new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SexInfo)) },
                { NamedValueType.SpawnType,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpawnTypeInfo)) },
                { NamedValueType.Special,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpecialInfo.Info[Scenario])) },
                { NamedValueType.SpecialEffect,       new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpecialEffectInfo)) },
                { NamedValueType.SpecialType,         new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpecialTypeInfo)) },
                { NamedValueType.Spell,               new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpellInfo.Info[Scenario])) },
                { NamedValueType.SpellTarget,         new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpellTargetInfo)) },

                { NamedValueType.Sprite,
                    new SubMethods((o, p, v, a) => {
                        return
                            // Characters
                            (v < 0x3C) ? _nameGetters[NamedValueType.Character].GetNameAndInfo(o, p, v, a) :
                            // Enemies
                            (v >= 0xC8 && v < 0x186) ? _nameGetters[NamedValueType.Monster].GetNameAndInfo(o, p, v - 0xC8, a) :
                            // Sprites
                            new NameAndInfo(v, ValueNames.SpriteInfo.Info[Scenario]);
                    })
                },

                { NamedValueType.StatType,            new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.StatTypeInfo)) },
                { NamedValueType.WeaponSpell,         new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.WeaponSpellInfo.Info[Scenario])) },
                { NamedValueType.WeaponType,          new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.WeaponTypeInfo)) },

                { NamedValueType.ConditionalType,
                    new SubMethods(
                        (o, p, v, a) => GetConditionalNameAndInfo(o, p, v, a),
                        (o, p, v, a) => CanGetConditionalName(o, p, v, a),
                        (o, p, a)    => CanGetConditionalInfo(o, p, a)
                    ) },
            };
        }

        public string GetName(object obj, PropertyInfo property, object value, params object[] parameters)
            => _nameGetters[(NamedValueType) parameters[0]].GetNameAndInfo(obj, property, Convert.ToInt32(value), parameters).Name;

        public INamedValueInfo GetInfo(object obj, PropertyInfo property, params object[] parameters)
            => _nameGetters[(NamedValueType) parameters[0]].GetNameAndInfo(obj, property, 0, parameters).Info;

        private T GetParameterAsT<T>(object[] parameters, int index, T defaultValue = null) where T : class
            => (parameters.Length > index) ? (T) parameters[index] : defaultValue;

        private T? GetParameterAsTOrNull<T>(object[] parameters, int index) where T : struct
            => (parameters.Length > index) ? (T?) parameters[index] : null;

        private PropertyInfo GetParameterReferencedProperty(object obj, object[] parameters, int index)
            => (parameters.Length > index) ? obj.GetType().GetProperty((string) parameters[index], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) : null;

        private object GetReferencedPropertyValue(object obj, object[] parameters, int index, object defaultValue = null)
            => GetParameterReferencedProperty(obj, parameters, index)?.GetValue(obj) ?? defaultValue;

        private T GetReferencedPropertyValueAsT<T>(object obj, object[] parameters, int index, T defaultValue = null) where T : class
            => (T) (GetReferencedPropertyValue(obj, parameters, index, defaultValue) ?? defaultValue);

        private T? GetReferencedPropertyValueAsTOrNull<T>(object obj, object[] parameters, int index) where T : struct
            => (T?) GetReferencedPropertyValue(obj, parameters, index);

        private int? GetReferencedPropertyValueAsIntOrNull(object obj, object[] parameters, int index)
            => ObjectToIntOrNull(GetReferencedPropertyValue(obj, parameters, index));

        private int? ObjectToIntOrNull(object value) {
            if (value == null)
                return null;

            var propertyValueType = value.GetType();
            return (propertyValueType == typeof(uint) || propertyValueType == typeof(uint?))
                ? (int) Convert.ToUInt32(value) : Convert.ToInt32(value);
        }

        private bool? GetReferencedPropertyValueAsBoolOrNull(object obj, object[] parameters, int index) {
            var value = GetReferencedPropertyValueAsIntOrNull(obj, parameters, index);
            return value.HasValue ? (value.Value == 0 ? false : true) : (bool?) null;
        }

        private T? GetReferencedPropertyValueAsEnumOrNull<T>(object obj, object[] parameters, int index) where T : struct, Enum {
            var intValue = GetReferencedPropertyValueAsIntOrNull(obj, parameters, index);
            return (intValue == null) ? (T?) null : (T) (object) intValue.Value;
        }

        public bool CanGetName(object obj, PropertyInfo property, object value, params object[] parameters) {
            var valueType = (NamedValueType) parameters[0];
            if (!_nameGetters.ContainsKey(valueType))
                return false;

            var valueInt = ObjectToIntOrNull(value);
            if (valueInt == null)
                return false;

            return _nameGetters[valueType].CanGetName(obj, property, valueInt.Value, parameters);
        }

        public bool CanGetInfo(object obj, PropertyInfo property, params object[] parameters) {
            var valueType = (NamedValueType) parameters[0];
            if (!_nameGetters.ContainsKey((NamedValueType) parameters[0]))
                return false;
            return _nameGetters[valueType].CanGetInfo(obj, property, parameters);
        }

        private NameAndInfo GetActorScriptNameAndInfo(object obj, PropertyInfo property, int value, object[] parameters) {
            var scripts = GetReferencedPropertyValueAsT<Dictionary<uint, ActorScript>>(obj, parameters, 1);

            // TODO: this is really aggressive!! cache it somehow???
            var possibleValues = scripts.ToDictionary(x => (int) x.Key, x => (x.Value.ScriptName == "") ? "(unnamed)" : x.Value.ScriptName);
            var namedValueInfo = new NamedValueInfo(0, 0, "X8", possibleValues);

            return new NameAndInfo(value, namedValueInfo);
        }

        private (NamedValueType, object[]) GetConditionalTypeAndParameters(object obj, object[] parameters) {
            var type = GetReferencedPropertyValueAsEnumOrNull<NamedValueType>(obj, parameters, 1);
            if (type == null || _nameGetters.ContainsKey(type.Value) == false)
                return (default, null);

            var restParameters = parameters.Skip(2).ToArray();
            var newParameters = new List<object> { type.Value };
            newParameters.AddRange(restParameters);

            return (type.Value, newParameters.ToArray());
        }

        private NameAndInfo GetConditionalNameAndInfo(object obj, PropertyInfo property, int value, object[] parameters) {
            var args = GetConditionalTypeAndParameters(obj, parameters);
            return args.Item2 == null ?  null : _nameGetters[args.Item1].GetNameAndInfo(obj, property, value, args.Item2);
        }

        private bool CanGetConditionalName(object obj, PropertyInfo property, int value, object[] parameters) {
            var args = GetConditionalTypeAndParameters(obj, parameters);
            return args.Item2 == null ? false : _nameGetters[args.Item1].CanGetName(obj, property, value, args.Item2);
        }

        private bool CanGetConditionalInfo(object obj, PropertyInfo property, object[] parameters) {
            var args = GetConditionalTypeAndParameters(obj, parameters);
            return args.Item2 == null ? false : _nameGetters[args.Item1].CanGetInfo(obj, property, args.Item2);
        }

        public ScenarioType Scenario { get; }
        public string Name => Scenario.ToString();
    }
}
