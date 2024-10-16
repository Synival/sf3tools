using SF3.Models;
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
        }

        public override IEnumerable<IModelArray> MakeModelArrays()
        {
            var modelArrays = new List<IModelArray>()
            {
                (ItemList = new ItemList(this)),
                (SpellList = new SpellList(this)),
                (PresetList = new PresetList(this)),
                (LoadList = new LoadList(this)),
                (StatList = new StatList(this)),
                (WeaponRankList = new WeaponRankList(this)),
                (AttackResistList = new AttackResistList(this)),
                (MusicOverrideList = new MusicOverrideList(this))
            };

            if (Scenario == ScenarioType.Scenario1)
            {
                modelArrays.Add(WarpList = new WarpList(this));
            }

            return modelArrays;
        }

        public ItemList ItemList { get; private set; }
        public SpellList SpellList { get; private set; }
        public PresetList PresetList { get; private set; }
        public LoadList LoadList { get; private set; }
        public StatList StatList { get; private set; }
        public WeaponRankList WeaponRankList { get; private set; }
        public AttackResistList AttackResistList { get; private set; }
        public WarpList WarpList { get; private set; }
        public MusicOverrideList MusicOverrideList { get; private set; }
    }
}
