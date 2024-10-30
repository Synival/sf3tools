using System;
using System.Collections.Generic;
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
                { NamedValueType.MovementType,        v => new NameAndInfo(v, ValueNames.MovementTypeInfo) },
                { NamedValueType.Sex,                 v => new NameAndInfo(v, ValueNames.SexInfo) },
                { NamedValueType.Special,             v => new NameAndInfo(v, ValueNames.SpecialInfo.Info[Scenario]) },
                { NamedValueType.Spell,               v => new NameAndInfo(v, ValueNames.SpellInfo.Info[Scenario]) },
                { NamedValueType.SpellTarget,         v => new NameAndInfo(v, ValueNames.StatTypeInfo) },
                { NamedValueType.StatType,            v => new NameAndInfo(v, ValueNames.StatTypeInfo) },
                { NamedValueType.WeaponType,          v => new NameAndInfo(v, ValueNames.WeaponTypeInfo) },
            };
        }

        public NameAndInfo GetNameAndInfo(int value, object parameter) => _nameGetters[(NamedValueType) parameter](value);

        public ScenarioType Scenario { get; }

        private delegate NameAndInfo NameGetter(int value);

        private readonly Dictionary<NamedValueType, NameGetter> _nameGetters;
    }
}
