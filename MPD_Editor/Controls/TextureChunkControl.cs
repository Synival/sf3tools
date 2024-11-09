using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;

namespace SF3.X1_Editor.Controls {
    public partial class TextureChunkControl : UserControl {
        public TextureChunkControl() {
            InitializeComponent();
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        public TabControl Tabs => this.tabMain;

        public TabPage TabHeader => this.tabHeader;

        public ObjectListView OLVHeader => this.olvHeader;
    }
}