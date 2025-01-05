using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private const MouseButtons c_MouseMiddleRight = MouseButtons.Middle | MouseButtons.Right;

        private void InitControls() {
            MouseMove         += (s, e) => OnMouseMoveControls(e);
            MouseEnter        += (s, e) => OnMouseEnterControls();
            MouseLeave        += (s, e) => OnMouseLeaveControls();
            MouseDown         += (s, e) => OnMouseDownControls(e);
            MouseUp           += (s, e) => OnMouseUpControls(e);
            MouseWheel        += (s, e) => OnMouseWheelControls(e);
            RightDoubleClick  += (s, e) => OnRightDoubleClickControls();
            MiddleDoubleClick += (s, e) => OnMiddleDoubleClickControls();
            KeyDown           += (s, e) => OnKeyDownControls(e);
            KeyUp             += (s, e) => OnKeyUpControls(e);
            CmdKey            += (object s, ref Message msg, Keys k, ref bool wp) => OnCmdKeyControls(k, ref wp);
            LostFocus         += (s, e) => OnLostFocusControls();
            FrameTick         += (s, e) => OnFrameTickControls();
        }

        private void OnMouseMoveControls(MouseEventArgs e) {
            if (_lastMousePos.HasValue && (_lastMousePos != e.Location)) {
                var shiftFactor = GetShiftFactor();
                var deltaX = (e.X - _lastMousePos.Value.X) * shiftFactor;
                var deltaY = (e.Y - _lastMousePos.Value.Y) * shiftFactor;

                // For middle+right drag, rotate around, keeping the tile over the mouse in place.
                if ((_mouseButtons & c_MouseMiddleRight) == c_MouseMiddleRight && _tilePos.HasValue) {
                    // TODO: SIMPLYIFY ALL THIS, AND IMPROVE IT!!!

                    var targetDist = GetCurrentTileTargetAndDistance().Value;
                    var distVec = new Vector3(0, 0, targetDist.Distance);

                    var distForward = -distVec
                        * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                        * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                    var targetBackward = targetDist.Target - distForward;
                    var targetBackwardOffset = Position - targetBackward;

                    var deltaYaw   = deltaX / -10.0f;
                    var deltaPitch = deltaY / -10.0f;
                    Yaw   += deltaYaw;
                    Pitch += deltaPitch;

                    Position = targetDist.Target +
                        distVec
                            * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                            * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw))
                        + new Vector3(targetBackwardOffset.X, 0, targetBackwardOffset.Z)
                            // TODO: for some reason, applying pitch is really bogus here, so for now we don't have it :(
                            * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(deltaYaw))
                        + new Vector3(0, targetBackwardOffset.Y, 0);

                    Invalidate();
                }
                // For middle drag, pan around.
                else if ((_mouseButtons & MouseButtons.Middle) != 0) {
                    Position += new Vector3(deltaX / -40.0f, deltaY / 40.0f, 0)
                        * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                        * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                    Invalidate();
                }
                // For middle drag, look around.
                else if ((_mouseButtons & MouseButtons.Right) != 0) {
                    Yaw   += deltaX / -10.0f;
                    Pitch += deltaY / -10.0f;
                    Invalidate();
                }
            }

            UpdateMousePosition(e);
        }

        private void OnMouseEnterControls() {
            _mouseIn = true;
            _lastMousePos = null;
            _mouseButtons = MouseButtons.None;

            if (_mousePos != null)
                UpdateMousePosition((Point?) null);
            else
                UpdateTilePosition(null);
        }

        private void OnMouseLeaveControls() {
            _mouseIn = false;
            if (_mouseButtons != MouseButtons.None)
                return;

            _lastMousePos = null;
            _mouseButtons = MouseButtons.None;

            if (_mousePos != null)
                UpdateMousePosition((Point?) null);
            else
                UpdateTilePosition(null);
        }

        private void OnMouseDownControls(MouseEventArgs e)
            => _mouseButtons |= e.Button;
        private void OnMouseUpControls(MouseEventArgs e) {
            if (_mouseButtons == 0)
                return;

            _mouseButtons &= ~e.Button;
            if (_mouseButtons == 0) {
                if (_mouseIn == false)
                    OnMouseLeaveControls();
                else {
                    UpdateMousePosition(e);
                    UpdateTilePosition();
                }
            }
        }

        private void UpdateMousePosition(MouseEventArgs e) {
            if (e.X < 0 || e.Y < 0 || e.X >= Width || e.Y >= Height)
                UpdateMousePosition((Point?) null);
            else
                UpdateMousePosition(e.Location);
            _lastMousePos = e.Location;
        }

        private void OnMouseWheelControls(MouseEventArgs e)
            => MoveCameraForward(e.Delta / -50 * GetShiftFactor());

        private void OnRightDoubleClickControls()
            => LookAtCurrentTileTarget();

        private void OnMiddleDoubleClickControls()
            => PanToCurrentTileTarget();

        private void OnKeyDownControls(KeyEventArgs e) {
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = true;
        }

        private void OnKeyUpControls(KeyEventArgs e) =>
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = false;

        private void OnCmdKeyControls(Keys keyData, ref bool wasProcessed) {
            if (wasProcessed || !Enabled || !Focused || !Visible)
                return;

            var keyPressed = (Keys) ((int) keyData & 0xFFFF);
            switch (keyPressed) {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    _keysPressed[keyPressed] = true;
                    wasProcessed = true;
                break;
            }
        }

        private void OnLostFocusControls() {
            _keysPressed.Clear();
            _mouseButtons = MouseButtons.None;
            _lastMousePos = null;
        }

        private void OnFrameTickControls() {
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
        }

        private bool KeyIsDown(Keys keyCode)
            => Enabled && Focused && Visible && _keysPressed.ContainsKey(keyCode) && _keysPressed[keyCode];

        private float GetShiftFactor()
            => _keysPressed.TryGetValue(Keys.ShiftKey, out bool on) ? (on ? 3 : 1) : 1;

        private void UpdateMousePosition(Point? pos) {
            if (_mousePos == pos)
                return;
            _mousePos = pos;

            if (_mouseButtons == 0)
                UpdateTilePosition();
        }

        private bool _mouseIn = false;
        private Point? _lastMousePos = null;
        private Point? _mousePos = null;

        private MouseButtons _mouseButtons = MouseButtons.None;
        private Dictionary<Keys, bool> _keysPressed = [];
    }
}
