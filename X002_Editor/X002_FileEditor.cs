using SF3.Types;
using SF3.X002_Editor.Models.AttackResist;
using SF3.X002_Editor.Models.Items;
using SF3.X002_Editor.Models.Loading;
using SF3.X002_Editor.Models.MusicOverride;
using SF3.X002_Editor.Models.Presets;
using SF3.X002_Editor.Models.Spells;
using SF3.X002_Editor.Models.StatBoost;
using SF3.X002_Editor.Models.Warps;
using SF3.X002_Editor.Models.WeaponRank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X002_Editor
{
    public class X002_FileEditor : SF3FileEditor, IX002_FileEditor
    {
        public X002_FileEditor(ScenarioType scenario) : base(scenario)
        {
            ItemList = new ItemList(this);
            SpellList = new SpellList(this);
            PresetList = new PresetList(this);
            LoadList = new LoadList(this);
            StatList = new StatList(this);
            WeaponRankList = new WeaponRankList(this);
            AttackResistList = new AttackResistList(this);
            WarpList = new WarpList(this);
            MusicOverrideList = new MusicOverrideList(this);
        }

        public ItemList ItemList { get; }
        public SpellList SpellList { get; }
        public PresetList PresetList { get; }
        public LoadList LoadList { get; }
        public StatList StatList { get; }
        public WeaponRankList WeaponRankList { get; }
        public AttackResistList AttackResistList { get; }
        public WarpList WarpList { get; }
        public MusicOverrideList MusicOverrideList { get; }
    }
}
