using System;
using System.Collections.Generic;
using System.Reflection;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    /// <summary>
    /// Context for the NameGetter for SF3 values.
    /// </summary>
    public class NameGetterContext : INameGetterContext {
        private delegate NameAndInfo NameAndInfoGetter(object obj, PropertyInfo property, int value, object[] parameters);
        private delegate bool CanGetNameChecker(object obj, PropertyInfo property, int value, object[] parameters);
        private delegate bool CanGetInfoChecker(object obj, PropertyInfo property, object[] parameters);

        private static readonly CanGetNameChecker AlwaysCanGetName = new CanGetNameChecker((o, p, v, r) => {
            return true;
        });

        private static readonly CanGetInfoChecker AlwaysCanGetInfo = new CanGetInfoChecker((o, p, r) => {
            return true;
        });

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
                { NamedValueType.Character,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.CharacterInfo.Info[Scenario])) },

                { NamedValueType.CharacterPlus,
                    new SubMethods(
                        (o, p, v, a) => new NameAndInfo(v, ValueNames.CharacterInfo.Info[Scenario]),
                        (o, p, v, a) => CanGetCharacterPlusValue(o, p, v, a),
                        (o, p, a)    => CanGetCharacterPlusValue(o, p, 0, a)
                    ) },

                { NamedValueType.CharacterClass,      new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.CharacterClassInfo)) },
                { NamedValueType.Droprate,            new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.DroprateInfo)) },
                { NamedValueType.EffectiveType,       new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.EffectiveTypeInfo)) },
                { NamedValueType.Element,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.ElementInfo)) },

                { NamedValueType.EventParameter,
                    new SubMethods(
                        (o, p, v, a) => GetEventParameterValue(o, p, v, a),
                        (o, p, v, a) => CanGetEventParameterValue(o, p, v, a),
                        (o, p, a)    => CanGetEventParameterValue(o, p, 0, a)
                    ) },

                { NamedValueType.FileIndex,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.FileIndexInfo.Info[Scenario])) },
                { NamedValueType.FriendshipBonusType, new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.FriendshipBonusTypeInfo)) },
                { NamedValueType.GameFlag,            new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.GameFlagInfo)) },

                { NamedValueType.GameFlagOrValue,
                    new SubMethods(
                        (o, p, v, a) => GetGameFlagValue(o, p, v, a),
                        (o, p, v, a) => CanGetGameFlagValue(o, p, v, a),
                        (o, p, a)    => CanGetGameFlagValue(o, p, 0, a)
                    ) },

                { NamedValueType.Item,                new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.ItemInfo.Info[Scenario])) },
                { NamedValueType.Load,                new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.LoadInfo.Info[Scenario])) },
                { NamedValueType.Monster,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MonsterInfo.Info[Scenario])) },
                { NamedValueType.MonsterForSlot,      new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MonsterForSlotInfo.Info[Scenario])) },
                { NamedValueType.MovementType,        new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.MovementTypeInfo)) },
                { NamedValueType.Sex,                 new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SexInfo)) },
                { NamedValueType.SpawnType,           new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpawnTypeInfo)) },
                { NamedValueType.Special,             new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpecialInfo.Info[Scenario])) },
                { NamedValueType.SpecialEffect,       new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.SpecialEffectInfo)) },

                { NamedValueType.SpecialElement,
                    new SubMethods(
                        (o, p, v, a) => GetSpecialElementValue(o, p, v, a),
                        (o, p, v, a) => CanGetSpecialElementValue(o, p, v, a),
                        (o, p, a)    => CanGetSpecialElementValue(o, p, 0, a)
                    ) },

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

                { NamedValueType.StatUpValueType,
                    new SubMethods(
                        (o, p, v, a) => GetStatUpValue(o, p, v, a),
                        (o, p, v, a) => CanGetStatUpValue(o, p, v, a),
                        (o, p, a)    => CanGetStatUpValue(o, p, 0, a)
                    ) },

                { NamedValueType.WeaponSpell,         new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.WeaponSpellInfo.Info[Scenario])) },
                { NamedValueType.WeaponType,          new SubMethods((o, p, v, a) => new NameAndInfo(v, ValueNames.WeaponTypeInfo)) },
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
            => (parameters.Length > index) ? obj.GetType().GetProperty((string) parameters[index]) : null;

        private object GetReferencedPropertyValue(object obj, object[] parameters, int index, object defaultValue = null)
            => GetParameterReferencedProperty(obj, parameters, index)?.GetValue(obj) ?? defaultValue;

        private T GetReferencedPropertyValueAsT<T>(object obj, object[] parameters, int index, T defaultValue = null) where T : class
            => (T) (GetReferencedPropertyValue(obj, parameters, index, defaultValue) ?? defaultValue);

        private T? GetReferencedPropertyValueAsTOrNull<T>(object obj, object[] parameters, int index) where T : struct
            => (T?) GetReferencedPropertyValue(obj, parameters, index);

        private int? GetReferencedPropertyValueAsIntOrNull(object obj, object[] parameters, int index) {
            var propertyValue = GetReferencedPropertyValue(obj, parameters, index);
            if (propertyValue == null)
                return null;

            var propertyValueType = propertyValue.GetType();
            return (propertyValueType == typeof(uint) || propertyValueType == typeof(uint?))
                ? (int) Convert.ToUInt32(propertyValue) : Convert.ToInt32(propertyValue);
        }

        private bool? GetReferencedPropertyValueAsBoolOrNull(object obj, object[] parameters, int index) {
            var value = GetReferencedPropertyValueAsIntOrNull(obj, parameters, index);
            return value.HasValue ? (value.Value == 0 ? false : true) : (bool?) null;
        }

        private T GetReferencedPropertyValueAsEnumOrNull<T>(object obj, object[] parameters, int index) where T : Enum {
            var intValue = GetReferencedPropertyValueAsIntOrNull(obj, parameters, index);
            return (intValue == null) ? (T) (object) null : (T) (object) intValue.Value;
        }

        public bool CanGetName(object obj, PropertyInfo property, object value, params object[] parameters) {
            var valueType = (NamedValueType) parameters[0];
            if (!_nameGetters.ContainsKey(valueType))
                return false;

            var valueInt = value.GetType() == typeof(uint) ? (int) Convert.ToUInt32(value) : Convert.ToInt32(value);
            return _nameGetters[valueType].CanGetName(obj, property, valueInt, parameters);
        }

        public bool CanGetInfo(object obj, PropertyInfo property, params object[] parameters) {
            var valueType = (NamedValueType) parameters[0];
            if (!_nameGetters.ContainsKey((NamedValueType) parameters[0]))
                return false;
            return _nameGetters[valueType].CanGetInfo(obj, property, parameters);
        }

        public ScenarioType Scenario { get; }

        private NameAndInfo GetStatUpValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var statUpType = GetReferencedPropertyValueAsEnumOrNull<StatUpType>(obj, parameters, 1);
            switch (statUpType) {
                case StatUpType.Special:
                    return _nameGetters[NamedValueType.Special].GetNameAndInfo(obj, property, value, parameters);
                case StatUpType.Spell:
                    return _nameGetters[NamedValueType.WeaponSpell].GetNameAndInfo(obj, property, value, parameters);
                default:
                    return null;
            }
        }

        private bool CanGetStatUpValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var statUpType = GetReferencedPropertyValueAsEnumOrNull<StatUpType>(obj, parameters, 1);
            switch (statUpType) {
                case StatUpType.Special:
                case StatUpType.Spell:
                    return true;
                default:
                    return false;
            }
        }

        private bool CanGetCharacterPlusValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var enemyID = GetReferencedPropertyValueAsIntOrNull(obj, parameters, 1);
            return enemyID == 0x5B; // magic number indicated "Character Placeholder"
        }

        private NameAndInfo GetEventParameterValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var inspectActionType = GetReferencedPropertyValueAsIntOrNull(obj, parameters, 1);
            switch (inspectActionType) {
                case 0x100:
                case 0x101:
                    return _nameGetters[NamedValueType.Item].GetNameAndInfo(obj, property, value, parameters);
                default:
                    return null;
            }
        }

        private bool CanGetEventParameterValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var inspectActionType = GetReferencedPropertyValueAsIntOrNull(obj, parameters, 1);
            switch (inspectActionType) {
                case 0x100:
                case 0x101:
                    return true;
                default:
                    return false;
            }
        }

        private NameAndInfo GetSpecialElementValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var damageCalcType = GetReferencedPropertyValueAsIntOrNull(obj, parameters, 1);
            switch (damageCalcType) {
                case 100:
                    return _nameGetters[NamedValueType.Element].GetNameAndInfo(obj, property, value, parameters);
                default:
                    return null;
            }
        }

        private bool CanGetSpecialElementValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var damageCalcType = GetReferencedPropertyValueAsIntOrNull(obj, parameters, 1);
            switch (damageCalcType) {
                case 100:
                    return true;
                default:
                    return false;
            }
        }

        private NameAndInfo GetGameFlagValue(object obj, PropertyInfo property, int value, object[] parameters) {
            var isGameFlagValue = GetReferencedPropertyValueAsBoolOrNull(obj, parameters, 1);
            return (isGameFlagValue == true) ? _nameGetters[NamedValueType.GameFlag].GetNameAndInfo(obj, property, value, parameters) : null;
        }

        private bool CanGetGameFlagValue(object obj, PropertyInfo property, int value, object[] parameters)
            => GetReferencedPropertyValueAsBoolOrNull(obj, parameters, 1) ?? false;

        public string Name => Scenario.ToString();
    }
}
