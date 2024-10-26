using System.Collections.Generic;
using SF3.Models;
using SF3.Models.X002.AttackResist;
using SF3.Models.X002.Items;
using SF3.Models.X002.LoadedOverride;
using SF3.Models.X002.Loading;
using SF3.Models.X002.Preset;
using SF3.Models.X002.Spells;
using SF3.Models.X002.StatBoost;
using SF3.Models.X002.Warp;
using SF3.Models.X002.WeaponRank;
using SF3.Types;

namespace SF3.FileEditors {
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor {
        public X002_FileEditor(ScenarioType scenario) : base(scenario) {
        }

        public override IEnumerable<ITable> MakeTables() {
            var modelArrays = new List<ITable>() {
                (ItemList = new ItemList(this)),
                (SpellList = new SpellList(this)),
                (PresetList = new PresetList(this)),
                (LoadList = new LoadList(this)),
                (StatList = new StatList(this)),
                (WeaponRankList = new WeaponRankList(this)),
                (AttackResistList = new AttackResistList(this)),
                (LoadedOverrideList = new LoadedOverrideList(this))
            };

            if (Scenario == ScenarioType.Scenario1)
                modelArrays.Add(WarpList = new WarpList(this));

            return modelArrays;
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

        public ItemList ItemList { get; private set; }
        public SpellList SpellList { get; private set; }
        public PresetList PresetList { get; private set; }
        public LoadList LoadList { get; private set; }
        public StatList StatList { get; private set; }
        public WeaponRankList WeaponRankList { get; private set; }
        public AttackResistList AttackResistList { get; private set; }
        public WarpList WarpList { get; private set; }
        public LoadedOverrideList LoadedOverrideList { get; private set; }
    }
}
