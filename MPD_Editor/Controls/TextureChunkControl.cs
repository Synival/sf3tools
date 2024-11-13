using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.MPDEditor.Controls;

namespace SF3.X1_Editor.Controls {
    public partial class TextureChunkControl : UserControl {
        public TextureChunkControl() {
            InitializeComponent();
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        public TabControl Tabs => this.tabMain;

        public TabPage TabHeader => this.tabHeader;
        public TabPage TabTextures => this.tabTextures;

        public ObjectListView OLVHeader => this.olvHeader;
        public ObjectListView OLVTextures => this.olvTextures;

        public TextureControl TextureControl => this.textureControl;
    }
}
