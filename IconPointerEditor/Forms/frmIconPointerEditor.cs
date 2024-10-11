using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.IconPointerEditor.Models.ItemIcons;
using SF3.IconPointerEditor.Models.SpellIcons;
using BrightIdeasSoftware;
using SF3.Types;
using SF3.Exceptions;

namespace SF3.IconPointerEditor.Forms
{
    public partial class frmIconPointerEditor : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "007";

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

        public bool _x026 = false;

        private bool X026
        {
            get => _x026;
            set
            {
                _x026 = value;
                tsmiHelp_X026Toggle.Checked = _x026;
                updateText();
            }
        }

        private SpellIconList _itemList;
        private ItemIconList _presetList;

        private IIconPointerFileEditor _fileEditor;

        public frmIconPointerEditor()
        {
            InitializeComponent();
            Scenario = ScenarioType.Scenario1;
            X026 = false;
        }

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;

            _itemList = new SpellIconList(_fileEditor);
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            _presetList = new ItemIconList(_fileEditor);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            olvItemIcons.ClearObjects();
            olvSpellIcons.ClearObjects();

            olvItemIcons.AddObjects(_presetList.Models);
            olvSpellIcons.AddObjects(_itemList.Models);

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X011*.bin)|X011*.bin|SF3 data (X021*.bin)|X021*.bin|SF3 data (X026*.bin)|X026*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new IconPointerFileEditor(Scenario, X026);
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

            olvItemIcons.FinishCellEdit();
            olvSpellIcons.FinishCellEdit();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 X011* (.bin)|X011.bin|Sf3 X021* (.bin)|X021.bin|Sf3 X026* (.bin)|X026.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column.AspectToStringFormat == "{0:X}")
            {
                NumericUpDown control = (NumericUpDown)e.Control;
                control.Hexadecimal = true;
            }
            else if (e.Value is SpellIcon)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_itemList.Models);
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }
            else if (e.Value is ItemIcon)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_presetList.Models);
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }

            Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);
        }

        private void olvCellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Value is SpellIcon)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                SpellIcon value = (SpellIcon)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is ItemIcon)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                ItemIcon value = (ItemIcon)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
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

        private void tsmiHelp_X026Toggle_Click(object sender, EventArgs e) => X026 = !X026;

        private void updateText()
        {
            //this.toolStripMenuItem12.Text = "Current Loading info. Map: " + Globals.maps + " Scenario " + Globals.scn + " MapType: " + Globals.battle + " debug: " + Globals.debug;
            //this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + Globals.fileName + "|          Current Loading info: Scenario: " + Globals.scn + " | Map: " + Globals.maps + " | MapType: " + Globals.battle + " | debug: " + Globals.debug;
            this.Text = "Sf3 Icon pointer Editor" + "          " + "X026 mode: " + (X026 ? "On" : "Off");
        }
    }
}
