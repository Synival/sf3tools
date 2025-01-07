using System;
using System.ComponentModel;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitEditing() {
            Click += (s, e) => OnClickEditing();
        }

        private void OnClickEditing() {
            var tile = (_tilePos == null) ? null : MPD_File.Tiles[_tilePos.Value.X, _tilePos.Value.Y];
            TilePropertiesControl.Tile = tile;
        }

        private void UpdateTilePosition() {
            // Don't allow changing tiles while the mouse is down.
            if (_mouseButtons != 0)
                return;

            if (_mousePos == null) {
                UpdateTilePosition(null);
                return;
            }

            var pixel = new byte[3];
            using (_selectFramebuffer.Use(FramebufferTarget.ReadFramebuffer))
                GL.ReadPixels(_mousePos.Value.X, Height - _mousePos.Value.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);

            if (pixel[2] == 255)
                UpdateTilePosition(null);
            else {
                UpdateTilePosition(new Point(
                    (int) Math.Round(pixel[0] / (255.0f / SurfaceModelAllResources.WidthInTiles)),
                    (int) Math.Round(pixel[1] / (255.0f / SurfaceModelAllResources.HeightInTiles))
                ));
            }
        }

        private void UpdateTilePosition(Point? pos) {
            // All invalid tile values should be 'null'.
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X > 63 || pos.Value.Y > 63))
                pos = null;

            // Early exit if no change is necessary.
            if (_tilePos == pos)
                return;

            _tilePos = pos;
            _surfaceEditor.UpdateTileModel(MPD_File, _world, _tilePos);

            Invalidate();
        }

        private Point? _tilePos = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TilePropertiesControl TilePropertiesControl { get; set; } = null;
    }
}
