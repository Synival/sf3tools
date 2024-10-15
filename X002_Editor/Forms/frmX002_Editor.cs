using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X002_Editor.Models.Spells;
using SF3.X002_Editor.Models.Presets;
using SF3.X002_Editor.Models.Items;
using SF3.X002_Editor.Models.Loading;
using SF3.X002_Editor.Models.StatBoost;
using SF3.X002_Editor.Models.WeaponRank;
using SF3.X002_Editor.Models.AttackResist;
using SF3.X002_Editor.Models.Warps;
using SF3.X002_Editor.Models.MusicOverride;
using BrightIdeasSoftware;
using SF3.Types;
using SF3.Exceptions;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Models;

namespace SF3.X002_Editor.Forms
{
    public partial class frmX002_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.20";

        private ItemList _itemList;
        private SpellList _spellList;
        private PresetList _presetList;
        private LoadList _loadList;
        private StatList _statList;
        private WeaponRankList _weaponRankList;
        private AttackResistList _attackResistList;
        private WarpList _warpList;
        private MusicOverrideList _musicOverrideList;

        public frmX002_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            tsmiHelp_Version.Text = "Version " + Version;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        private void tabMain_Click(object sender, EventArgs e)
        {
            olvSpells.ClearObjects();
            if (_spellList != null)
            {
                olvSpells.AddObjects(_spellList.Models);
            }

            olvStatBoost.ClearObjects();
            if (_statList != null)
            {
                olvStatBoost.AddObjects(_statList.Models);
            }
        }

        protected override string FileDialogFilter => "SF3 scn3 data (X002.bin)|X002.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X002_FileEditor(Scenario);

        protected override bool LoadOpenedFile()
        {
            var fileEditor = FileEditor as IX002_FileEditor;

            _itemList = new ItemList(fileEditor);
            _spellList = new SpellList(fileEditor);
            _presetList = new PresetList(fileEditor);
            _loadList = new LoadList(fileEditor);
            _statList = new StatList(fileEditor);
            _weaponRankList = new WeaponRankList(fileEditor);
            _attackResistList = new AttackResistList(fileEditor);
            _musicOverrideList = new MusicOverrideList(fileEditor);
            _warpList = new WarpList(fileEditor);

            var loadLists = new List<IModelArray>()
            {
                _itemList,
                _spellList,
                _presetList,
                _loadList,
                _statList,
                _weaponRankList,
                _attackResistList,
                _musicOverrideList,
            };

            if (Scenario == ScenarioType.Scenario1)
            {
                loadLists.Add(_warpList);
            }

            foreach (var list in loadLists)
            {
                if (!list.Load())
                {
                    MessageBox.Show("Could not load " + list.ResourceFile);
                    return false;
                }
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            olvItems.AddObjects(_itemList.Models);
            olvSpells.AddObjects(_spellList.Models);
            olvPreset.AddObjects(_presetList.Models);
            olvLoaded.AddObjects(_loadList.Models);
            olvStatBoost.AddObjects(_statList.Models);
            olvWeaponRankAttack.AddObjects(_weaponRankList.Models);
            olvAttackResist.AddObjects(_attackResistList.Models);
            olvLoadedOverride.AddObjects(_musicOverrideList.Models);

            if (Scenario == ScenarioType.Scenario1)
            {
                olvWarpTable.AddObjects(_warpList.Models);
            }

            return true;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;
    }
}
