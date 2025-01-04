using System;
using System.Windows.Forms;
using OpenTK.GLControl;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl : GLControl {
        public MPD_ViewerGLControl() {
            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();

            InitializeComponent();
            InitToolstrip();

            Disposed += (s, a) => {
                OnDisposeRendering();
                _timer?.Dispose();
                _timer = null;
            };
        }

        protected override void WndProc(ref Message m) {
            const int WM_RBUTTONDBLCLK   = 0x0206;
            const int WM_NCMBUTTONDBLCLK = 0x0209;

            switch (m.Msg) {
                case WM_RBUTTONDBLCLK:
                    OnRightDoubleClick(EventArgs.Empty);
                    break;

                case WM_NCMBUTTONDBLCLK:
                    OnMiddleDoubleClick(EventArgs.Empty);
                    break;
            }

            base.WndProc(ref m);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            OnLoadRendering();
            _timer.Start();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            OnResizeRendering();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            OnPaintRendering();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Focus();
            _mouseButtons |= e.Button;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            _mouseButtons &= ~e.Button;
        }

        // TODO: temporary click function!! remove this when there's an actual 'edit' panel
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            if (_tilePos == null)
                return;

            System.Diagnostics.Debug.WriteLine("Tile: " + _tilePos.ToString());
            System.Diagnostics.Debug.Write(_surfaceModel.TileDebugText[_tilePos.Value.X, _tilePos.Value.Y]);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            MoveCameraForward(e.Delta / -50 * GetShiftFactor());
        }

        protected void OnRightDoubleClick(EventArgs e)
            => LookAtCurrentTileTarget();

        protected void OnMiddleDoubleClick(EventArgs e)
            => PanToCurrentTileTarget();

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            OnMouseMoveControls(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            _lastMousePos = null;
            _mouseButtons = MouseButtons.None;

            if (_mousePos != null)
                UpdateMousePosition(null);
            else
                UpdateTilePosition(null);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = true;
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) keyData & 0xFFFF)] = true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            _keysPressed.Clear();
            _mouseButtons = MouseButtons.None;
            _lastMousePos = null;
        }

        private void OnFrameTick() {
            OnFrameTickKeys();
            OnFrameTickRendering();
        }

        private void InitToolstrip() {
            var state = AppState.RetrieveAppState();
            _drawWireframe = state.ViewerDrawWireframe;
            _drawHelp      = state.ViewerDrawHelp;
            _drawNormals   = state.ViewerDrawNormals;
        }

        private void IncrementFrame() {
            if (!Visible)
                return;
            _frame = (_frame + 1) % 3600;
            OnFrameTick();
        }

        public IMPD_File Model { get; set; }

        private int _frame = 0;
        private Timer _timer = null;
    }
}
