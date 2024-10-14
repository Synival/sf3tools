using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X019_Editor.Models.Monsters;
using BrightIdeasSoftware;
using SF3.Types;
using SF3.Exceptions;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;

namespace SF3.X019_Editor.Forms
{
    public partial class frmX019_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.12";

        private MonsterList _monsterList;

        public frmX019_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            this.tsmiHelp_Version.Text = "Version " + Version;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
                tsmiScenario_PremiumDiskX044.Checked = (Scenario == ScenarioType.Other);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FinalizeForm();
        }

        private bool Initialize()
        {
            tsmiFile_SaveAs.Enabled = true;
            var fileEditor = FileEditor as IX019_FileEditor;

            _monsterList = new MonsterList(fileEditor);
            if (!_monsterList.Load())
            {
                MessageBox.Show("Could not load " + _monsterList.ResourceFile);
                return false;
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            olvMonsterTab1.AddObjects(_monsterList.Models);
            olvMonsterTab2.AddObjects(_monsterList.Models);
            olvMonsterTab3.AddObjects(_monsterList.Models);
            olvMonsterTab4.AddObjects(_monsterList.Models);
            olvMonsterTab5.AddObjects(_monsterList.Models);

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X019.bin)|X019.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                CloseFile();
                FileEditor = new X019_FileEditor(Scenario);
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
            savefile.Filter = "Sf3 X019 (.bin)|X019.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
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

        private void tsmiScenario_PremiumDiskX044_Click(object sender, EventArgs e) => Scenario = ScenarioType.Other;
    }
}
