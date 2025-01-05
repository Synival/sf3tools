using System;
using System.ComponentModel;
using System.Windows.Forms;
using OpenTK.GLControl;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl : GLControl {
        public MPD_ViewerGLControl() {
            InitializeComponent();

            // Horrible hack to make this work in Design mode.
            if (!AppState.Initialized())
                return;

            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();
            Disposed += (s, a) => {
                _timer?.Dispose();
                _timer = null;
            };

            InitToolstrip();
            InitRendering();
            InitControls();
        }

        protected override void WndProc(ref Message m) {
            const int WM_RBUTTONDBLCLK   = 0x0206;
            const int WM_NCMBUTTONDBLCLK = 0x0209;

            switch (m.Msg) {
                case WM_RBUTTONDBLCLK:
                    RightDoubleClick?.Invoke(this, EventArgs.Empty);
                    break;

                case WM_NCMBUTTONDBLCLK:
                    MiddleDoubleClick?.Invoke(this, EventArgs.Empty);
                    break;
            }

            base.WndProc(ref m);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            _timer?.Start();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Focus();
        }

        // TODO: temporary click function!! remove this when there's an actual 'edit' panel
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            if (_tilePos == null)
                return;

            System.Diagnostics.Debug.WriteLine("Tile: " + _tilePos.ToString());
            System.Diagnostics.Debug.Write(_surfaceModel.TileDebugText[_tilePos.Value.X, _tilePos.Value.Y]);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
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
            FrameTick?.Invoke(this, EventArgs.Empty);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD_File MPD_File { get; set; } = null;

        public delegate void CmdKeyEventHandler(object sender, ref Message msg, Keys keyData, ref bool wasProcessed);

        public event EventHandler RightDoubleClick;
        public event EventHandler MiddleDoubleClick;
        public event EventHandler FrameTick;
        public event CmdKeyEventHandler CmdKey;

        private int _frame = 0;
        private Timer _timer = null;
    }
}
