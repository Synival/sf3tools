using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class SurfaceMap3DControl : UserControl {
        public SurfaceMap3DControl() {
            InitializeComponent();
        }

        public void UpdateTextures(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks) {
            // TODO: actual work!
        }
    }
}
