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

/*

*/

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

            //lvcAction1.AspectToStringConverter = getActionName;
            //lvcAction2.AspectToStringConverter = getActionName;
            //lvcAction3.AspectToStringConverter = getActionName;
            //lvcAction4.AspectToStringConverter = getActionName;
            //lvcAction5.AspectToStringConverter = getActionName;
            //lvcAction6.AspectToStringConverter = getActionName;
            //lvcAction7.AspectToStringConverter = getActionName;
            //lvcAction8.AspectToStringConverter = getActionName;

            //lvcItemStatType1.AspectToStringConverter += getStatTypeName;
            //lvcItemStatType2.AspectToStringConverter += getStatTypeName;
            //lvcItemStatType3.AspectToStringConverter += getStatTypeName;

            //lvcCharacterItem1.AspectToStringConverter += getItemName;
            //lvcCharacterItem2.AspectToStringConverter += getItemName;
            //lvcCharacterItem3.AspectToStringConverter += getItemName;
            //lvcCharacterItem4.AspectToStringConverter += getItemName;
            //lvcCharacterItem5.AspectToStringConverter += getItemName;
            //lvcCharacterItem6.AspectToStringConverter += getItemName;
            //lvcCharacterItem7.AspectToStringConverter += getItemName;
            //lvcCharacterItem8.AspectToStringConverter += getItemName;

            //lvcItem.AspectToStringConverter += getItemName;

            //lvcBlacksmithItem.AspectToStringConverter += getItemName;
            //lvcStoreItem.AspectToStringConverter += getItemName;

            //lvcSpellType.AspectToStringConverter += getSpellName;
            //lvcSpellClass.AspectToStringConverter += getClassName;

            //lvcStoreItemType.AspectToStringConverter += getStoreItemTypeName;

            //Block the putter events for columns that use comboboxes
            //we handle this in the cell edit finishing event to make things a TON easier
            /*lvcItemStatType1.AspectPutter += blocker;
            lvcItemStatType2.AspectPutter += blocker;
            lvcItemStatType3.AspectPutter += blocker;
            lvcAction1.AspectPutter += blocker;
            lvcAction2.AspectPutter += blocker;
            lvcAction3.AspectPutter += blocker;
            lvcAction4.AspectPutter += blocker;
            lvcAction5.AspectPutter += blocker;
            lvcAction6.AspectPutter += blocker;
            lvcAction7.AspectPutter += blocker;
            lvcAction8.AspectPutter += blocker;
            lvcCharacterItem1.AspectPutter += blocker;
            lvcCharacterItem2.AspectPutter += blocker;
            lvcCharacterItem3.AspectPutter += blocker;
            lvcCharacterItem4.AspectPutter += blocker;
            lvcCharacterItem5.AspectPutter += blocker;
            lvcCharacterItem6.AspectPutter += blocker;
            lvcCharacterItem7.AspectPutter += blocker;
            lvcCharacterItem8.AspectPutter += blocker;

            lvcItem.AspectPutter += blocker;
            lvcBlacksmithItem.AspectPutter += blocker;
            lvcStoreItem.AspectPutter += blocker;

            lvcSpellClass.AspectPutter += blocker;
            lvcSpellType.AspectPutter += blocker;
            lvcStoreItemType.AspectPutter += blocker;*/
        }

        private void blocker(object target, object newvalue) { }

        /*private string getActionName(object target)
        {
            return ((Action)target).Name;
        }*/
        /*private string getStatTypeName(object target)
        {
            return ((StatType)target).Name;
        }*/
        private string getItemName(object target)
        {
            return ((Item)target).Name;
        }
        private string getSpellName(object target)
        {
            return ((Spell)target).SpellName;
        }
        private string getPresetName(object target)
        {
            return ((Preset)target).PresetName;
        }
        private string getLoadName(object target)
        {
            return ((Loading)target).LoadName;
        }
        private string getStatName(object target)
        {
            return ((StatBoost)target).StatName;
        }
        private string getWeaponRankName(object target)
        {
            return ((WeaponRank)target).WeaponRankName;
        }

        private string getAttackResist(object target)
        {
            return ((AttackResist)target).AttackResistName;
        }

        private string getMusicOverride(object target)
        {
            return ((MusicOverride)target).MusicOverrideName;
        }

        /*
        private string getStoreItemTypeName(object target)
        {
            return ((StoreItemType)target).Name;
        }*/

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;
            /*if (!StoreItemTypeList.loadStoreItemTypeList()) {
                MessageBox.Show("Could not load Resources/storeitemtypes.xml.");
                return false;
            }
            if (!SpellList.loadSpellList()) {
                MessageBox.Show("Could not load Resources/spells.xml.");
                return false;
            }
            if (!CharacterClassList.loadCharacterClassList()) {
                MessageBox.Show("Could not load Resources/classes.xml.");
                return false;
            }*/
            /*if (!ActionList.loadActionList()) {
                MessageBox.Show("Could not load Resources/actions.xml.");
                return false;
            }*/
            /*if (!StatTypeList.loadStatTypeList()) {
                MessageBox.Show("Could not load Resources/stattypes.xml.");
                return false;
            }*/
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

            //BlacksmithList.loadBlacksmithList();
            //StoreItemList.loadStoreItemList();
            //SpellEntryList.loadSpellEntryList();

            //olvBlacksmith.ClearObjects();
            //olvCharacters.ClearObjects();
            //olvMonsters.ClearObjects();

            olvItems.ClearObjects();
            olvSpells.ClearObjects();
            olvPreset.ClearObjects();
            olvLoaded.ClearObjects();
            olvStatBoost.ClearObjects();
            olvWeaponRankAttack.ClearObjects();
            olvAttackResist.ClearObjects();
            olvWarpTable.ClearObjects();
            olvLoadedOverride.ClearObjects();

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

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

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());
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

            //olvBlacksmith.FinishCellEdit();
            //olvMonsters.FinishCellEdit();
            //olvCharacters.FinishCellEdit();
            olvItems.FinishCellEdit();
            olvSpells.FinishCellEdit();
            olvPreset.FinishCellEdit();
            olvLoaded.FinishCellEdit();
            olvStatBoost.FinishCellEdit();
            olvWeaponRankAttack.FinishCellEdit();
            olvAttackResist.FinishCellEdit();
            olvWarpTable.FinishCellEdit();
            olvLoadedOverride.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x002 (.bin)|X002.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);
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

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario1;
        }

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario2;
        }

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario3;
        }

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.PremiumDisk;
        }
    }
}
