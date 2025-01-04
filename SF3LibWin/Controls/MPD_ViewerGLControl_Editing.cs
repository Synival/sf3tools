using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void UpdateTilePosition() {
            // Changing the tile is locked while rotating around a tile.
            if ((_mouseButtons & c_MouseMiddleRight) == c_MouseMiddleRight)
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
            if (_tilePos == pos)
                return;

            _tilePos = pos;
            _surfaceEditor.UpdateTileModel(MPD_File, _world, _tilePos);

            Invalidate();
        }

        private Point? _tilePos = null;
    }
}
