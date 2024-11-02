using System.Collections.Generic;
using System.Reflection;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    /// <summary>
    /// Context for the NameGetter for SF3 values.
    /// </summary>
    public class NameGetterContext : INameGetterContext {
        public NameGetterContext(ScenarioType scenario) {
            Scenario = scenario;

            _nameGetters = new Dictionary<NamedValueType, NameGetter>() {
                { NamedValueType.Character,           v => new NameAndInfo(v, ValueNames.CharacterInfo.Info[Scenario]) },
                { NamedValueType.CharacterClass,      v => new NameAndInfo(v, ValueNames.CharacterClassInfo) },
                { NamedValueType.Droprate,            v => new NameAndInfo(v, ValueNames.DroprateInfo) },
                { NamedValueType.EffectiveType,       v => new NameAndInfo(v, ValueNames.EffectiveTypeInfo) },
                { NamedValueType.Element,             v => new NameAndInfo(v, ValueNames.ElementInfo) },
                { NamedValueType.FileIndex,           v => new NameAndInfo(v, ValueNames.FileIndexInfo.Info[Scenario]) },
                { NamedValueType.FriendshipBonusType, v => new NameAndInfo(v, ValueNames.FriendshipBonusTypeInfo) },
                { NamedValueType.Item,                v => new NameAndInfo(v, ValueNames.ItemInfo.Info[Scenario]) },
                { NamedValueType.Monster,             v => new NameAndInfo(v, ValueNames.MonsterInfo.Info[Scenario]) },
                { NamedValueType.MonsterForSlot,      v => new NameAndInfo(v, ValueNames.MonsterForSlotInfo.Info[Scenario]) },
                { NamedValueType.MovementType,        v => new NameAndInfo(v, ValueNames.MovementTypeInfo) },
                { NamedValueType.Sex,                 v => new NameAndInfo(v, ValueNames.SexInfo) },
                { NamedValueType.Special,             v => new NameAndInfo(v, ValueNames.SpecialInfo.Info[Scenario]) },
                { NamedValueType.Spell,               v => new NameAndInfo(v, ValueNames.SpellInfo.Info[Scenario]) },
                { NamedValueType.SpellTarget,         v => new NameAndInfo(v, ValueNames.StatTypeInfo) },
                { NamedValueType.StatType,            v => new NameAndInfo(v, ValueNames.StatTypeInfo) },
                { NamedValueType.WeaponType,          v => new NameAndInfo(v, ValueNames.WeaponTypeInfo) },
            };
        }

        public string GetName(object obj, PropertyInfo property, int value, params object[] parameters) => _nameGetters[(NamedValueType) parameters[0]](value).Name;
        public INamedValueInfo GetInfo(object obj, PropertyInfo property, params object[] parameters) => _nameGetters[(NamedValueType) parameters[0]](0).Info;

        public bool CanGetName(object obj, PropertyInfo property, int value, params object[] parameters) => _nameGetters.ContainsKey((NamedValueType) parameters[0]);
        public bool CanGetInfo(object obj, PropertyInfo property, params object[] parameters) => _nameGetters.ContainsKey((NamedValueType) parameters[0]);

        public ScenarioType Scenario { get; }

        private delegate NameAndInfo NameGetter(int value);

        private readonly Dictionary<NamedValueType, NameGetter> _nameGetters;
    }
}
