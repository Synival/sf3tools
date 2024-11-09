using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SF3.Models.MPD.TextureChunk;
using SF3.MPDEditor.Extensions;

namespace SF3.MPDEditor.Controls {
    public partial class BattleMapControl : UserControl {
        public BattleMapControl() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (FullImage != null)
                e.Graphics.DrawImage(FullImage, 0, 0);
        }

        public Dictionary<int, Image> UniqueImages { get; private set; } = null;
        public Image[,] Images { get; private set; } = null;
        public byte[,] Flags { get; private set; } = null;
        public Image FullImage { get; private set; } = null;

        public void UpdateTextures(ushort[,] textureData, TextureChunk[] textureChunks) {
            UniqueImages = new Dictionary<int, Image>();
            Images = new Image[textureData.GetLength(1), textureData.GetLength(0)];
            Flags = new byte[textureData.GetLength(1), textureData.GetLength(0)];

            for (int y = 0; y < textureData.GetLength(1); y++) {
                for (int x = 0; x < textureData.GetLength(0); x++) {
                    var textureId = textureData[x, y] & 0xFF;
                    if (!UniqueImages.ContainsKey(textureId)) {
                        var texture = textureChunks
                            .Select(a => a.TextureTable)
                            .SelectMany(a => a.Rows)
                            .FirstOrDefault(a => a.ID == textureId);

                        var image = texture?.GetImage();
                        UniqueImages.Add(textureId, image);
                    }

                    Images[x, y] = UniqueImages[textureId];
                    Flags[x, y] = (byte) ((textureData[x, y] >> 8) & 0xFF);
                }
            }

            FullImage = new Bitmap(24 * 64, 24 * 64);
            using (var graphics = Graphics.FromImage(FullImage)) {
                for (int y = 0; y < Images.GetLength(1); y++) {
                    for (int x = 0; x < Images.GetLength(0); x++) {
                        if (Images[x, y] != null) {
                            graphics.DrawImage(Images[x, y], x * 24, y * 24, 24, 24);
                        }
                    }
                }
            }

            Invalidate();
        }
    }
}
