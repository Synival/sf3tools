using SF3.FileEditors;
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

namespace SF3.X002_Editor.FileEditors
{
    public interface IX002_FileEditor : ISF3FileEditor
    {
        ItemList ItemList { get; }
        SpellList SpellList { get; }
        PresetList PresetList { get; }
        LoadList LoadList { get; }
        StatList StatList { get; }
        WeaponRankList WeaponRankList { get; }
        AttackResistList AttackResistList { get; }
        WarpList WarpList { get; }
        MusicOverrideList MusicOverrideList { get; }
    }
}
