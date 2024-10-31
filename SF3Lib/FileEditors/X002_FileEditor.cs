using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.X002;
using SF3.Types;

namespace SF3.FileEditors {
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor {
        public X002_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
                (ItemTable = new ItemTable(this, 0)),
                (SpellTable = new SpellTable(this, 0)),
                (PresetTable = new PresetTable(this, 0)),
                (LoadingTable = new LoadingTable(this, 0)),
                (StatBoostTable = new StatBoostTable(this, 0)),
                (WeaponRankTable = new WeaponRankTable(this, 0)),
                (AttackResistTable = new AttackResistTable(this, 0)),
                (LoadedOverrideTable = new LoadedOverrideTable(this, 0))
            };

            if (Scenario == ScenarioType.Scenario1)
                tables.Add(WarpTable = new WarpTable(this));

            return tables;
        }

        public override void DestroyTables() {
            ItemTable = null;
            SpellTable = null;
            PresetTable = null;
            LoadingTable = null;
            StatBoostTable = null;
            WeaponRankTable = null;
            AttackResistTable = null;
            LoadedOverrideTable = null;
        }

        [BulkCopyRecurse]
        public ItemTable ItemTable { get; private set; }
        [BulkCopyRecurse]
        public SpellTable SpellTable { get; private set; }
        [BulkCopyRecurse]
        public PresetTable PresetTable { get; private set; }
        [BulkCopyRecurse]
        public LoadingTable LoadingTable { get; private set; }
        [BulkCopyRecurse]
        public StatBoostTable StatBoostTable { get; private set; }
        [BulkCopyRecurse]
        public WeaponRankTable WeaponRankTable { get; private set; }
        [BulkCopyRecurse]
        public AttackResistTable AttackResistTable { get; private set; }
        [BulkCopyRecurse]
        public WarpTable WarpTable { get; private set; }
        [BulkCopyRecurse]
        public LoadedOverrideTable LoadedOverrideTable { get; private set; }
    }
}
