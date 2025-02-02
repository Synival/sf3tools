using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SF3.Win.Types;

namespace SF3.Win.Forms {
    public partial class ManipulateChunkDialog : Form {
        public ManipulateChunkDialog(ManipulateChunkDialogType type, Dictionary<int, string> chunks) {
            Type = type;
            Chunks = chunks;

            InitializeComponent();

            switch (Type) {
                case ManipulateChunkDialogType.ExportChunk:
                    Text = "Export Chunk...";
                    btnAction.Text = "Export...";
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
                    break;

                default:
                    throw new ArgumentException(nameof(type));
            }

            cbChunk.DataSource    = new BindingSource(chunks, null);
            cbChunk.DisplayMember = "Value";
            cbChunk.ValueMember   = "Key";
        }

        private void btnCancel_Click(object sender, EventArgs e)
            => Close();

        private void btnAction_Click(object sender, EventArgs e) {

        }

        public ManipulateChunkDialogType Type { get; }
        private Dictionary<int, string> Chunks { get; }
    }
}
