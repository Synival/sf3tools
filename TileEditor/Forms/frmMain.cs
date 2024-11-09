using System;
using System.IO;
using System.Windows.Forms;
using STHAEditor.Models;
using STHAEditor.Models.Items;
using STHAEditor.Models.Presets;

namespace STHAEditor.Forms {

    public partial class frmMain : Form {
        //Used to append to state names to stop program loading states from older versions
        private readonly string Version = "v3";

        public frmMain() {
            InitializeComponent();
            frmMonsterEditor_Resize(this, new EventArgs());
        }

        private bool initialise() {
            saveAsToolStripMenuItem.Enabled = true;
            if (!ItemList.loadItemList()) {
                _ = MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            if (!PresetList.loadPresetList()) {
                _ = MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            olvItems.ClearObjects();
            objectListView1.ClearObjects();

            olvItems.AddObjects(PresetList.getPresetList());
            objectListView1.AddObjects(ItemList.getItemList());

            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            var openfile = new OpenFileDialog {
                Filter = "SF3 data File (*.bin)|*.bin|" + "All Files (*.*)|*.*"
            };
            if (openfile.ShowDialog() == DialogResult.OK) {
                if (FileEditor.loadFile(openfile.FileName)) {
                    try {
                        _ = initialise();
                    }
                    catch (System.Reflection.TargetInvocationException) {
                        //wrong x1 file was selected
                    }
                }
                else {
                    _=MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                }
            }
        }

        private void frmMonsterEditor_Resize(object sender, EventArgs e) {
            var newsize = ClientSize;
            newsize.Height -= 24;
            tabMain.Size = newsize;
            olvItems.Size = tabItems.ClientSize;
            objectListView1.Size = tabPage1.ClientSize;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            olvItems.FinishCellEdit();
            objectListView1.FinishCellEdit();
            var savefile = new SaveFileDialog {
                Filter = "Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*",
                FileName = Path.GetFileName(FileEditor.Filename)
            };
            if (savefile.ShowDialog() == DialogResult.OK)
                _ = FileEditor.saveFile(savefile.FileName);
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) {
            if (e.Column.AspectToStringFormat == "{0:X}") {
                var control = (NumericUpDown)e.Control;
                control.Hexadecimal = true;
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e) => frmMonsterEditor_Resize(this, new EventArgs());

        public static class Globals {
            public static int scenario = 1;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) => Globals.scenario = 1;
        private void toolStripMenuItem2_Click(object sender, EventArgs e) => Globals.scenario = 2;
        private void toolStripMenuItem3_Click(object sender, EventArgs e) => Globals.scenario = 3;
        private void toolStripMenuItem4_Click(object sender, EventArgs e) => Globals.scenario = 4;
        private void toolStripMenuItem5_Click(object sender, EventArgs e) => Globals.scenario = 5;
    }
}
