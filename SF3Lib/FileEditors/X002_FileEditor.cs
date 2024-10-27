using System.Collections.Generic;
using SF3.Tables;
using SF3.Tables.Shared;
using SF3.Tables.X002;
using SF3.Types;

namespace SF3.FileEditors {
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor {
        public X002_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
                (ItemList = new ItemTable(this)),
                (SpellList = new SpellTable(this)),
                (PresetList = new PresetTable(this)),
                (LoadList = new LoadTable(this)),
                (StatList = new StatTable(this)),
                (WeaponRankList = new WeaponRankTable(this)),
                (AttackResistList = new AttackResistTable(this)),
                (LoadedOverrideList = new LoadedOverrideTable(this))
            };

            if (Scenario == ScenarioType.Scenario1)
                tables.Add(WarpList = new WarpTable(this));

            return tables;
        }

        public override void DestroyTables() {
            ItemList = null;
            SpellList = null;
            PresetList = null;
            LoadList = null;
            StatList = null;
            WeaponRankList = null;
            AttackResistList = null;
            LoadedOverrideList = null;
        }

        public ItemTable ItemList { get; private set; }
        public SpellTable SpellList { get; private set; }
        public PresetTable PresetList { get; private set; }
        public LoadTable LoadList { get; private set; }
        public StatTable StatList { get; private set; }
        public WeaponRankTable WeaponRankList { get; private set; }
        public AttackResistTable AttackResistList { get; private set; }
        public WarpTable WarpList { get; private set; }
        public LoadedOverrideTable LoadedOverrideList { get; private set; }
    }
}
