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
            _modelArrays.Add(ItemList = new ItemList(this));
            _modelArrays.Add(SpellList = new SpellList(this));
            _modelArrays.Add(PresetList = new PresetList(this));
            _modelArrays.Add(LoadList = new LoadList(this));
            _modelArrays.Add(StatList = new StatList(this));
            _modelArrays.Add(WeaponRankList = new WeaponRankList(this));
            _modelArrays.Add(AttackResistList = new AttackResistList(this));
            _modelArrays.Add(MusicOverrideList = new MusicOverrideList(this));

            if (Scenario == ScenarioType.Scenario1)
            {
                _modelArrays.Add(WarpList = new WarpList(this));
            }
        }

        public override bool LoadFile(string filename)
        {
            if (!base.LoadFile(filename))
            {
                return false;
            }

            foreach (var model in _modelArrays)
            {
                if (!model.Load())
                {
                    return false;
                }
            }

            return true;
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

        private List<IModelArray> _modelArrays = new List<IModelArray>();
    }
}
