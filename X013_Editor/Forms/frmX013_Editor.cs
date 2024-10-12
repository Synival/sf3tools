using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X013_Editor.Models.Spells;
using SF3.X013_Editor.Models.Presets;
using SF3.X013_Editor.Models.Items;
using SF3.X013_Editor.Models.Stats;
using SF3.X013_Editor.Models.Soulmate;
using SF3.X013_Editor.Models.Soulfail;
using SF3.X013_Editor.Models.MagicBonus;
using SF3.X013_Editor.Models.CritMod;
using SF3.X013_Editor.Models.Critrate;
using SF3.X013_Editor.Models.SpecialChance;
using SF3.X013_Editor.Models.ExpLimit;
using SF3.X013_Editor.Models.HealExp;
using SF3.X013_Editor.Models.WeaponSpellRank;
using SF3.X013_Editor.Models.StatusEffects;
using BrightIdeasSoftware;
using SF3.Types;
using SF3.Exceptions;

namespace SF3.X013_Editor.Forms
{
    public partial class frmX013_Editor : Form
    {
        // Used to display version in the application
        private string Version = "0.17";

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
        private StatsList _statsList;
        private SoulmateList _soulmateList;
        private SoulfailList _soulfailList;
        private MagicBonusList _magicBonusList;
        private CritModList _critModList;
        private CritrateList _critrateList;
        private SpecialChanceList _specialChanceList;
        private ExpLimitList _expLimitList;
        private HealExpList _healExpList;
        private WeaponSpellRankList _weaponSpellRankList;
        private StatusEffectList _statusEffectList;

        private IX013_FileEditor _fileEditor;

        public frmX013_Editor()
        {
            InitializeComponent();
            this.tsmiHelp_Version.Text = "Version " + Version;
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
                MessageBox.Show("Could not load Resources/characters.xml.");
                return false;
            }

            _presetList = new PresetList(_fileEditor);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/ExpList.xml.");
                return false;
            }

            _statsList = new StatsList(_fileEditor);
            if (!_statsList.Load())
            {
                MessageBox.Show("Could not load Resources/X013StatList.xml.");
                return false;
            }

            _soulmateList = new SoulmateList(_fileEditor);
            if (!_soulmateList.Load())
            {
                MessageBox.Show("Could not load Resources/SoulmateList.xml.");
                return false;
            }

            _soulfailList = new SoulfailList(_fileEditor);
            if (!_soulfailList.Load())
            {
                MessageBox.Show("Could not load Resources/Soulfail.xml.");
                return false;
            }

            _magicBonusList = new MagicBonusList(_fileEditor);
            if (!_magicBonusList.Load())
            {
                MessageBox.Show("Could not load Resources/MagicBonus.xml.");
                return false;
            }

            _critModList = new CritModList(_fileEditor);
            if (!_critModList.Load())
            {
                MessageBox.Show("Could not load Resources/CritModList.xml.");
                return false;
            }

            _critrateList = new CritrateList(_fileEditor);
            if (!_critrateList.Load())
            {
                MessageBox.Show("Could not load Resources/CritrateList.xml.");
                return false;
            }

            _specialChanceList = new SpecialChanceList(_fileEditor);
            if (!_specialChanceList.Load())
            {
                MessageBox.Show("Could not load Resources/SpecialChanceList.xml.");
                return false;
            }

            _expLimitList = new ExpLimitList(_fileEditor);
            if (!_expLimitList.Load())
            {
                MessageBox.Show("Could not load Resources/ExpLimitList.xml.");
                return false;
            }

            _healExpList = new HealExpList(_fileEditor);
            if (!_healExpList.Load())
            {
                MessageBox.Show("Could not load Resources/HealExpList.xml.");
                return false;
            }

            _weaponSpellRankList = new WeaponSpellRankList(_fileEditor);
            if (!_weaponSpellRankList.Load())
            {
                MessageBox.Show("Could not load Resources/WeaponSpellRankListList.xml.");
                return false;
            }

            _statusEffectList = new StatusEffectList(_fileEditor);
            if (!_statusEffectList.Load())
            {
                MessageBox.Show("Could not load Resources/StatusGroupList.xml.");
                return false;
            }

            olvSpecials.ClearObjects();
            olvFriendshipExp.ClearObjects();
            olvSupportType.ClearObjects();
            olvSupportStats.ClearObjects();
            olvSoulmate.ClearObjects();
            olvSoulmateChanceFail.ClearObjects();
            olvMagicBonus.ClearObjects();
            olvCritVantages.ClearObjects();
            olvCritCounterRate.ClearObjects();
            olvSpecialChance.ClearObjects();
            olvExpLimit.ClearObjects();
            olvHealExp.ClearObjects();
            olvWeaponSpellRank.ClearObjects();
            olvStatusGroups.ClearObjects();

            olvSpecials.AddObjects(_itemList.Models);
            olvFriendshipExp.AddObjects(_presetList.Models);
            olvSupportType.AddObjects(_spellList.Models);
            olvSupportStats.AddObjects(_statsList.Models);
            olvSoulmate.AddObjects(_soulmateList.Models);
            olvSoulmateChanceFail.AddObjects(_soulfailList.Models);
            olvMagicBonus.AddObjects(_magicBonusList.Models);
            olvCritVantages.AddObjects(_critModList.Models);
            olvCritCounterRate.AddObjects(_critrateList.Models);
            olvSpecialChance.AddObjects(_specialChanceList.Models);
            olvExpLimit.AddObjects(_expLimitList.Models);
            olvHealExp.AddObjects(_healExpList.Models);
            olvWeaponSpellRank.AddObjects(_weaponSpellRankList.Models);
            olvStatusGroups.AddObjects(_statusEffectList.Models);

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 scn3 data (X013.bin)|X013.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new X013_FileEditor(Scenario);
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

            olvSpecials.FinishCellEdit();
            olvFriendshipExp.FinishCellEdit();
            olvSupportType.FinishCellEdit();
            olvSupportStats.FinishCellEdit();
            olvSoulmate.FinishCellEdit();
            olvSoulmateChanceFail.FinishCellEdit();
            olvMagicBonus.FinishCellEdit();
            olvCritVantages.FinishCellEdit();
            olvCritCounterRate.FinishCellEdit();
            olvSpecialChance.FinishCellEdit();
            olvExpLimit.FinishCellEdit();
            olvHealExp.FinishCellEdit();
            olvWeaponSpellRank.FinishCellEdit();
            olvStatusGroups.FinishCellEdit();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x013 (.bin)|X013.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;
    }
}
