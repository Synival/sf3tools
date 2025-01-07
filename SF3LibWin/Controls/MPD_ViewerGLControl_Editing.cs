using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitEditing() {
            MouseDown += (s, e) => OnMouseDownEditing(e);
        }

        private void OnMouseDownEditing(MouseEventArgs e) {
            if (e.Button != MouseButtons.Left)
                return;

            _tileSelectedPos = _tileHoverPos;
            var tile = (_tileSelectedPos == null) ? null : MPD_File.Tiles[_tileSelectedPos.Value.X, _tileSelectedPos.Value.Y];
            TilePropertiesControl.Tile = tile;
            _tileSelectedNeedsUpdate = true;
            Invalidate();
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
                    (int) Math.Round(pixel[0] / (255.0f / SurfaceModelResources.WidthInTiles)),
                    (int) Math.Round(pixel[1] / (255.0f / SurfaceModelResources.HeightInTiles))
                ));
            }
        }

        private void UpdateTilePosition(Point? pos) {
            // All invalid tile values should be 'null'.
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X > 63 || pos.Value.Y > 63))
                pos = null;

            // Early exit if no change is necessary.
            if (_tileHoverPos == pos)
                return;

            _tileHoverPos = pos;
            _surfaceEditor.UpdateTileHoverModel(MPD_File, _world, _tileHoverPos);

            Invalidate();
        }

        private Point? _tileHoverPos = null;
        private Point? _tileSelectedPos = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TilePropertiesControl TilePropertiesControl { get; set; } = null;
    }
}
