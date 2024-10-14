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

namespace SF3.X002_Editor.Forms
{
    public partial class frmX002_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.20";

        private ScenarioType _scenario = (ScenarioType) (-1); // uninitialized value

        private ScenarioType Scenario
        {
            get => _scenario;
            set
            {
                _scenario = value;
                tsmiScenario_Scenario1.Checked = (_scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (_scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (_scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (_scenario == ScenarioType.PremiumDisk);
            }
        }

        private ItemList _itemList;
        private SpellList _spellList;
        private PresetList _presetList;
        private LoadList _loadList;
        private StatList _statList;
        private WeaponRankList _weaponRankList;
        private AttackResistList _attackResistList;
        private WarpList _warpList;
        private MusicOverrideList _musicOverrideList;

        private List<ObjectListView> _objectListViews;

        public frmX002_Editor()
        {
            InitializeComponent();

            BaseTitle = this.Text;
            tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
            _objectListViews = Utils.GetAllObjectsOfTypeInFields<ObjectListView>(this, false);

            UpdateTitle();
        }

        private bool Initialize()
        {
            tsmiFile_SaveAs.Enabled = true;
            var fileEditor = FileEditor as IX002_FileEditor;

            _itemList = new ItemList(fileEditor);
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load " + _itemList.ResourceFile);
                return false;
            }

            _spellList = new SpellList(fileEditor);
            if (!_spellList.Load())
            {
                MessageBox.Show("Could not load " + _spellList.ResourceFile);
                return false;
            }

            _presetList = new PresetList(fileEditor);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load " + _presetList.ResourceFile);
                return false;
            }

            _loadList = new LoadList(fileEditor);
            if (!_loadList.Load())
            {
                MessageBox.Show("Could not load " + _loadList.ResourceFile);
                return false;
            }

            _statList = new StatList(fileEditor);
            if (!_statList.Load())
            {
                MessageBox.Show("Could not load " + _statList.ResourceFile);
                return false;
            }

            _weaponRankList = new WeaponRankList(fileEditor);
            if (!_weaponRankList.Load())
            {
                MessageBox.Show("Could not load " + _weaponRankList.ResourceFile);
                return false;
            }

            _attackResistList = new AttackResistList(fileEditor);
            if (!_attackResistList.Load())
            {
                MessageBox.Show("Could not load " + _attackResistList.ResourceFile);
                return false;
            }

            _warpList = new WarpList(fileEditor);
            if (Scenario == ScenarioType.Scenario1 && !_warpList.Load())
            {
                MessageBox.Show("Could not load " + _warpList.ResourceFile);
                return false;
            }

            _musicOverrideList = new MusicOverrideList(fileEditor);
            if (!_musicOverrideList.Load())
            {
                MessageBox.Show("Could not load " + _musicOverrideList.ResourceFile);
                return false;
            }

            _objectListViews.ForEach(x => x.ClearObjects());

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

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 scn3 data (X002.bin)|X002.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                CloseFile();
                FileEditor = new X002_FileEditor(Scenario);
                FileEditor.TitleChanged += (obj, args) => UpdateTitle();

                if (FileEditor.LoadFile(openfile.FileName))
                {
                    try
                    {
                        Initialize();
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        //wrong file was selected
                        CloseFile();
                        MessageBox.Show("Failed to read file:\n" +
                                        "    " + openfile.FileName);
                    }
                    catch (FileEditorReadException)
                    {
                        //wrong file was selected
                        CloseFile();
                        MessageBox.Show("Data appears corrupt or invalid:\n" +
                                        "    " + openfile.FileName + "\n\n" +
                                        "Is this the correct type of file?");
                    }
                }
                else
                {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                }
            }
        }

        public override void CloseFile()
        {
            base.CloseFile();
            _objectListViews.ForEach(x => x.ClearObjects());
            tsmiFile_SaveAs.Enabled = false;
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e)
        {
            if (FileEditor == null)
            {
                return;
            }

            _objectListViews.ForEach(x => x.FinishCellEdit());

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x002 (.bin)|X002.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

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

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;
    }
}
