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

namespace SF3.X002_Editor.Forms
{
    public partial class frmX002_Editor : Form
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

        private IX002_FileEditor _fileEditor;

        public frmX002_Editor()
        {
            InitializeComponent();
            tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
        }

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;

            _itemList = new ItemList(_fileEditor);
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            _spellList = new SpellList(_fileEditor);
            if (!_spellList.Load())
            {
                MessageBox.Show("Could not load Resources/spellList.xml.");
                return false;
            }

            _presetList = new PresetList(_fileEditor);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            _loadList = new LoadList(_fileEditor);
            if (!_loadList.Load())
            {
                MessageBox.Show("Could not load Resources/loadList.xml.");
                return false;
            }

            _statList = new StatList(_fileEditor);
            if (!_statList.Load())
            {
                MessageBox.Show("Could not load Resources/X002StatList.xml.");
                return false;
            }

            _weaponRankList = new WeaponRankList(_fileEditor);
            if (!_weaponRankList.Load())
            {
                MessageBox.Show("Could not load Resources/WeaponRankList.xml.");
                return false;
            }

            _attackResistList = new AttackResistList(_fileEditor);
            if (!_attackResistList.Load())
            {
                MessageBox.Show("Could not load Resources/AttackResistList.xml.");
                return false;
            }

            _warpList = new WarpList(_fileEditor);
            if (Scenario == ScenarioType.Scenario1 && !_warpList.Load())
            {
                MessageBox.Show("Could not load Resources/X002Warp.xml.");
                return false;
            }

            _musicOverrideList = new MusicOverrideList(_fileEditor);
            if (!_musicOverrideList.Load())
            {
                MessageBox.Show("Could not load Resources/MusicOverrideList.xml.");
                return false;
            }

            olvItems.ClearObjects();
            olvSpells.ClearObjects();
            olvPreset.ClearObjects();
            olvLoaded.ClearObjects();
            olvStatBoost.ClearObjects();
            olvWeaponRankAttack.ClearObjects();
            olvAttackResist.ClearObjects();
            olvWarpTable.ClearObjects();
            olvLoadedOverride.ClearObjects();

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
                _fileEditor = new X002_FileEditor(Scenario);
                if (_fileEditor.LoadFile(openfile.FileName))
                {
                    try
                    {
                        initialise();
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        //wrong file was selected
                        MessageBox.Show("Failed to read file:\n" +
                                        "    " + openfile.FileName);
                    }
                    catch (FileEditorReadException)
                    {
                        //wrong file was selected
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

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e)
        {
            if (_fileEditor == null)
            {
                return;
            }

            olvItems.FinishCellEdit();
            olvSpells.FinishCellEdit();
            olvPreset.FinishCellEdit();
            olvLoaded.FinishCellEdit();
            olvStatBoost.FinishCellEdit();
            olvWeaponRankAttack.FinishCellEdit();
            olvAttackResist.FinishCellEdit();
            olvWarpTable.FinishCellEdit();
            olvLoadedOverride.FinishCellEdit();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x002 (.bin)|X002.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(_fileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
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
