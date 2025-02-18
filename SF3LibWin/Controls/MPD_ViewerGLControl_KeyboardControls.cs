using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitKeyboardControls() {
            KeyDown           += (s, e) => OnKeyDownKeyboardControls(e);
            KeyUp             += (s, e) => OnKeyUpKeyboardControls(e);
            CmdKey            += (object s, ref Message msg, Keys k, ref bool wp) => OnCmdKeyKeyboardControls(k, ref wp);
            LostFocus         += (s, e) => OnLostFocusKeyboardControls();
            FrameTick         += (s, e) => OnFrameTickKeyboardControls();
        }

        private void OnKeyDownKeyboardControls(KeyEventArgs e) {
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = true;
        }

        private void OnKeyUpKeyboardControls(KeyEventArgs e) =>
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = false;

        private void OnCmdKeyKeyboardControls(Keys keyData, ref bool wasProcessed) {
            if (wasProcessed || !Enabled || !Visible)
                return;

            var keyPressed = (Keys) ((int) keyData & 0xFFFF);
            switch (keyPressed) {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    if (keyData.HasFlag(Keys.Control)) {
                        ChangeFocusedTile(keyPressed);
                        wasProcessed = true;
                        return;
                    }

                    _keysPressed[keyPressed] = true;
                    wasProcessed = true;
                break;
            }
        }

        public bool ChangeFocusedTile(Keys keyData) {
            var keyPressed = (Keys) ((int) keyData & 0xFFFF);
            switch (keyPressed) {
                case Keys.Up:
                    return ChangeFocusedTile(0, 1);
                case Keys.Down:
                    return ChangeFocusedTile(0, -1);
                case Keys.Left:
                    return ChangeFocusedTile(-1, 0);
                case Keys.Right:
                    return ChangeFocusedTile(1, 0);
                default:
                    return false;
            }
        }

        public bool ChangeFocusedTile(int xDir, int yDir) {
            var currentTile = TilePropertiesControl.Tile;
            if (currentTile == null)
                return false;

            var newTileX = currentTile.X + xDir;
            var newTileY = currentTile.Y + yDir;
            if (newTileX < 0 || newTileY < 0 || newTileX > 63 || newTileY > 63)
                return false;

            SelectTile(new Point(newTileX, newTileY));
            return true;
        }

        private void OnLostFocusKeyboardControls() {
            _keysPressed.Clear();
        }

        private void OnFrameTickKeyboardControls() {
            var keysDown = _keysPressed.Where(x => x.Value == true).Select(x => x.Key).ToHashSet();

            var moveUpKey    = keysDown.Contains(Keys.Subtract) || keysDown.Contains(Keys.R);
            var moveDownKey  = keysDown.Contains(Keys.Add) || keysDown.Contains(Keys.F);
            var moveLeftKey  = keysDown.Contains(Keys.End) || keysDown.Contains(Keys.NumPad1) || keysDown.Contains(Keys.A);
            var moveRightKey = keysDown.Contains(Keys.PageDown) || keysDown.Contains(Keys.NumPad3) || keysDown.Contains(Keys.D);

            int moveX = (moveLeftKey ? -1 : 0) + (moveRightKey ? 1 : 0);
            int moveY = (moveUpKey ? 1 : 0) + (moveDownKey ? -1 : 0);
            int moveZ = (keysDown.Contains(Keys.W) ? -1 : 0) + (keysDown.Contains(Keys.S) ? 1 : 0);

            var shiftFactor = GetShiftFactor();

            if (moveX != 0 || moveY != 0 || moveZ != 0) {
                var move = new Vector3(moveX, moveY * 0.5f, moveZ * 2.5f)
                    * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                    * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                Position += move * 0.25f * shiftFactor;
                Invalidate();
            }

            var rotateLeftKey  = keysDown.Contains(Keys.Left)  || keysDown.Contains(Keys.NumPad4);
            var rotateRightKey = keysDown.Contains(Keys.Right) || keysDown.Contains(Keys.NumPad6);

            int rotateYaw = (rotateLeftKey ? 1 : 0) + (rotateRightKey ? -1 : 0);
            if (rotateYaw != 0) {
                Yaw += rotateYaw * 1.0f * shiftFactor;
                Invalidate();
            }

            var rotateUpKey   = keysDown.Contains(Keys.Up)   || keysDown.Contains(Keys.NumPad8);
            var rotateDownKey = keysDown.Contains(Keys.Down) || keysDown.Contains(Keys.NumPad2);

            int rotatePitch = (rotateUpKey ? -1 : 0) + (rotateDownKey ? 1 : 0);
            if (rotatePitch != 0) {
                var oldPitch = Pitch;
                Pitch += rotatePitch * 0.5f * shiftFactor;
                if (oldPitch != Pitch)
                    Invalidate();
            }

            // Keys to adjust lighting
            if (MPD_File?.LightPosition != null) {
                if (keysDown.Contains(Keys.Oemcomma)) {
                    MPD_File.LightPosition.Pitch -= (ushort) (0x080 * shiftFactor);
                    UpdateShaderLighting();
                    Invalidate();
                }
                else if (keysDown.Contains(Keys.OemPeriod)) {
                    MPD_File.LightPosition.Pitch += (ushort) (0x080 * shiftFactor);
                    UpdateShaderLighting();
                    Invalidate();
                }

                if (keysDown.Contains(Keys.OemOpenBrackets)) {
                    MPD_File.LightPosition.Yaw -= (ushort) (0x080 * shiftFactor);
                    UpdateShaderLighting();
                    Invalidate();
                }
                else if (keysDown.Contains(Keys.OemCloseBrackets)) {
                    MPD_File.LightPosition.Yaw += (ushort) (0x080 * shiftFactor);
                    UpdateShaderLighting();
                    Invalidate();
                }
            }
        }

        private bool KeyIsDown(Keys keyCode)
            => Enabled && Focused && Visible && _keysPressed.ContainsKey(keyCode) && _keysPressed[keyCode];

        private float GetShiftFactor()
            => _keysPressed.TryGetValue(Keys.ShiftKey, out bool on) ? (on ? 3 : 1) : 1;

        private Dictionary<Keys, bool> _keysPressed = [];
    }
}
