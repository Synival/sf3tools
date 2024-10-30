using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Tables.X002;
using SF3.Types;

namespace SF3.FileEditors {
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor {
        public X002_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
                (ItemTable = new ItemTable(this)),
                (SpellTable = new SpellTable(this)),
                (PresetTable = new PresetTable(this)),
                (LoadTable = new LoadTable(this)),
                (StatTable = new StatTable(this)),
                (WeaponRankTable = new WeaponRankTable(this)),
                (AttackResistTable = new AttackResistTable(this)),
                (LoadedOverrideTable = new LoadedOverrideTable(this))
            };

            if (Scenario == ScenarioType.Scenario1)
                tables.Add(WarpTable = new WarpTable(this));

            return tables;
        }

        public override void DestroyTables() {
            ItemTable = null;
            SpellTable = null;
            PresetTable = null;
            LoadTable = null;
            StatTable = null;
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
        public LoadTable LoadTable { get; private set; }
        [BulkCopyRecurse]
        public StatTable StatTable { get; private set; }
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
