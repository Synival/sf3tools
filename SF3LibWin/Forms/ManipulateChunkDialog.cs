using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Structs.MPD;
using SF3.Win.Types;

namespace SF3.Win.Forms {
    public partial class ManipulateChunkDialog : Form {
        public ManipulateChunkDialog(ManipulateChunkDialogType type, ChunkHeader[] chunks) {
            Type = type;
            Chunks = chunks;

            InitializeComponent();

            DialogResult = DialogResult.Cancel;
            bool onlyExisting = false;

            switch (Type) {
                case ManipulateChunkDialogType.ExportChunk:
                    Text = "Export Chunk...";
                    btnAction.Text = "Export...";
                    onlyExisting = true;
                    break;

                case ManipulateChunkDialogType.ImportChunk:
                    Text = "Import Chunk...";
                    btnAction.Text = "Import...";
                    break;

                case ManipulateChunkDialogType.DeleteChunk:
                    Text = "Delete Chunk...";
                    btnAction.Text = "Delete";
                    labelWhichData.Hide();
                    rbCompressed.Hide();
                    rbUncompressed.Hide();
                    onlyExisting = true;
                    break;

                default:
                    throw new ArgumentException(nameof(type));
            }

            var chunkDropdown = chunks
                .Where(x => !onlyExisting || x.Exists)
                .ToDictionary(x => x.ID, x => {
                    var name = (x.Exists) ? $"{x.ChunkType.ToString()} ({x.CompressionType.ToString()})" : "--";
                    return $"{x.ID}: {name}";
                });

            cbChunk.DataSource    = new BindingSource(chunkDropdown, null);
            cbChunk.DisplayMember = "Value";
            cbChunk.ValueMember   = "Key";

            if (chunkDropdown.Count == 0)
                btnAction.Enabled = false;

            cbChunk.SelectedValueChanged += (s, e) => {
                UpdateSelectedChunk();
                UpdateDataRadioButtons();
            };

            UpdateSelectedChunk();
            UpdateDataRadioButtons();
        }

        private void UpdateDataRadioButtons() {
            if (SelectedChunk == null || Type == ManipulateChunkDialogType.DeleteChunk) {
                rbCompressed.Enabled = false;
                rbUncompressed.Enabled = false;
            }
            else if (Type == ManipulateChunkDialogType.ImportChunk) {
                rbCompressed.Enabled = true;
                rbUncompressed.Enabled = true;
            }
            else if (Type == ManipulateChunkDialogType.ExportChunk) {
                if (SelectedChunk.CompressionType == SF3.Types.CompressionType.Compressed) {
                    rbCompressed.Enabled = true;
                    rbUncompressed.Enabled = true;
                }
                else {
                    rbCompressed.Enabled = false;
                    rbUncompressed.Enabled = true;
                    rbUncompressed.Checked = true;
                }
            }
        }

        private void UpdateSelectedChunk() {
            var item = (KeyValuePair<int, string>?) cbChunk.SelectedItem;
            SelectedChunk = (item != null) ? Chunks[item.Value.Key] : null;
        }

        private void btnAction_Click(object sender, EventArgs e) {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => Close();

        public ManipulateChunkDialogType Type { get; }
        public ChunkHeader[] Chunks { get; }

        public bool Uncompressed => rbUncompressed.Checked;
        public ChunkHeader SelectedChunk { get; private set; } = null;
    }
}
