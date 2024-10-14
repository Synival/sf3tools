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
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;

namespace SF3.IconPointerEditor.Forms
{
    public partial class frmIconPointerEditor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.09";

        public bool _x026 = false;

        private bool X026
        {
            get => _x026;
            set
            {
                _x026 = value;
                tsmiHelp_X026Toggle.Checked = _x026;
                UpdateTitle();
            }
        }

        private SpellIconList _spellIconList;
        private ItemIconList _itemIconList;

        public frmIconPointerEditor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
            X026 = false;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FinalizeForm();
        }

        protected override string OpenFileDialogFilter => "SF3 data (X011*.bin)|X011*.bin|SF3 data (X021*.bin)|X021*.bin|SF3 data (X026*.bin)|X026*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new IconPointerFileEditor(Scenario, X026);

        protected override bool LoadOpenedFile()
        {
            var fileEditor = FileEditor as IIconPointerFileEditor;

            _spellIconList = new SpellIconList(fileEditor);
            if (!_spellIconList.Load())
            {
                MessageBox.Show("Could not load " + _spellIconList.ResourceFile);
                return false;
            }

            _itemIconList = new ItemIconList(fileEditor);
            if (!_itemIconList.Load())
            {
                MessageBox.Show("Could not load " + _itemIconList.ResourceFile);
                return false;
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            olvItemIcons.AddObjects(_itemIconList.Models);
            olvSpellIcons.AddObjects(_spellIconList.Models);

            tsmiFile_SaveAs.Enabled = true;
            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();

        public override void CloseFile()
        {
            base.CloseFile();
            tsmiFile_SaveAs.Enabled = false;
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e)
        {
            if (FileEditor == null)
            {
                return;
            }

            ObjectListViews.ForEach(x => x.FinishCellEdit());

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 X011* (.bin)|X011.bin|Sf3 X021* (.bin)|X021.bin|Sf3 X026* (.bin)|X026.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        private void tsmiHelp_X026Toggle_Click(object sender, EventArgs e) => X026 = !X026;
    }
}
