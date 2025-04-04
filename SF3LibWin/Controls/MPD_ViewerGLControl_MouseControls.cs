using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Mathematics;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private const MouseButtons c_MouseMiddleRight = MouseButtons.Middle | MouseButtons.Right;

        private void InitMouseControls() {
            MouseMove         += (s, e) => OnMouseMoveMouseControls(e);
            MouseEnter        += (s, e) => OnMouseEnterMouseControls();
            MouseLeave        += (s, e) => OnMouseLeaveMouseControls();
            MouseDown         += (s, e) => OnMouseDownMouseControls(e);
            MouseUp           += (s, e) => OnMouseUpMouseControls(e);
            MouseWheel        += (s, e) => OnMouseWheelMouseControls(e);
            RightDoubleClick  += (s, e) => OnRightDoubleClickMouseControls();
            MiddleDoubleClick += (s, e) => OnMiddleDoubleClickMouseControls();
            LostFocus         += (s, e) => OnLostFocusMouseControls();

            UpdateCursor();
        }

        private void OnMouseMoveMouseControls(MouseEventArgs e) {
            var dragMouseButtons = _mouseButtons;
            if (CursorMode == ViewerCursorMode.Navigate && _mouseButtons.HasFlag(MouseButtons.Left))
                dragMouseButtons |= MouseButtons.Middle;

            if (_lastMousePos.HasValue && (_lastMousePos != e.Location)) {
                var shiftFactor = GetShiftFactor();
                var deltaX = (e.X - _lastMousePos.Value.X) * shiftFactor;
                var deltaY = (e.Y - _lastMousePos.Value.Y) * shiftFactor;

                // For middle+right drag, rotate around, keeping the tile over the mouse in place.
                if ((dragMouseButtons & c_MouseMiddleRight) == c_MouseMiddleRight && _tileHoverPos.HasValue) {
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
                else if ((dragMouseButtons & MouseButtons.Middle) != 0) {
                    Position += new Vector3(deltaX / -40.0f, deltaY / 40.0f, 0)
                        * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                        * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                    Invalidate();
                }
                // For middle drag, look around.
                else if ((dragMouseButtons & MouseButtons.Right) != 0) {
                    Yaw   += deltaX / -10.0f;
                    Pitch += deltaY / -10.0f;
                    Invalidate();
                }
            }

            UpdateMousePosition(e);
        }

        private void OnMouseEnterMouseControls() {
            _mouseIn = true;
            _lastMousePos = null;
            _mouseButtons = MouseButtons.None;

            if (_mousePos != null)
                UpdateMousePosition((Point?) null);
            else
                UpdateTilePosition(null);
        }

        private void OnMouseLeaveMouseControls() {
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

        private void OnMouseDownMouseControls(MouseEventArgs e)
            => _mouseButtons |= e.Button;

        private void OnMouseUpMouseControls(MouseEventArgs e) {
            if (_mouseButtons == 0)
                return;

            _mouseButtons &= ~e.Button;
            if (_mouseButtons == 0) {
                if (_mouseIn == false)
                    OnMouseLeaveMouseControls();
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

        private void OnMouseWheelMouseControls(MouseEventArgs e)
            => MoveCameraForward(e.Delta / -50 * GetShiftFactor());

        private void OnRightDoubleClickMouseControls()
            => LookAtCurrentTileTarget();

        private void OnMiddleDoubleClickMouseControls()
            => PanToCurrentTileTarget();

        private void OnLostFocusMouseControls() {
            _mouseButtons = MouseButtons.None;
            _lastMousePos = null;
        }

        private void UpdateMousePosition(Point? pos) {
            if (_mousePos == pos)
                return;
            _mousePos = pos;

            if (_mouseButtons == 0)
                UpdateTilePosition();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ViewerCursorMode CursorMode {
            get {
                switch (AppState.ViewerCursorMode) {
                    case 0:
                        return ViewerCursorMode.Select;
                    case 1:
                        return ViewerCursorMode.Navigate;
                    default:
                        return ViewerCursorMode.Select;
                }
            }
            set {
                if (AppState.ViewerCursorMode != (int) value) {
                    AppState.ViewerCursorMode = (int) value;
                    AppState.Serialize();
                    UpdateCursor();
                    CursorModeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void UpdateCursor() {
            var cursorMode = CursorMode;
            Cursor = (cursorMode == ViewerCursorMode.Select) ? Cursors.Arrow : Cursors.SizeAll;
        }

        private bool _mouseIn = false;
        private Point? _lastMousePos = null;
        private Point? _mousePos = null;

        private MouseButtons _mouseButtons = MouseButtons.None;

        public event EventHandler CursorModeChanged;
    }
}
