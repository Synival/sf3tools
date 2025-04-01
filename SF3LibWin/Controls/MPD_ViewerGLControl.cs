using System;
using System.ComponentModel;
using System.Windows.Forms;
using OpenTK.GLControl;
using SF3.Models.Files.MPD;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl : GLControl {
        public MPD_ViewerGLControl() {
            InitializeComponent();

            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();
            Disposed += (s, a) => {
                _timer?.Dispose();
                _timer = null;
            };

            InitRendering();
            InitKeyboardControls();
            InitMouseControls();
            InitEditing();
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

        public void RunCmdKeyEvent(object sender, ref Message msg, Keys keyData, ref bool wasProcessed)
            => CmdKey?.Invoke(sender, ref msg, keyData, ref wasProcessed);

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private long _startTimeInMs = 0;
        private long _lastTimeInMs = 0;
        private float _lastDeltaInMs = 0.0f;

        private void IncrementFrame() {
            if (!Visible)
                return;

            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds() - _startTimeInMs;
            if (_startTimeInMs == 0) {
                _startTimeInMs = now;
                _lastTimeInMs = 0;
                now = 0;
                _lastDeltaInMs = now - _lastDeltaInMs;
            }

            // Frame delta is an average of this frame and the last to reduce some jittering.
            var deltaInMs = now - _lastTimeInMs;
            FrameTick?.Invoke(this, (_lastDeltaInMs + deltaInMs) / 2);

            _lastTimeInMs = now;
            _lastDeltaInMs = deltaInMs;
        }

        private void OnTileModified(object sender, EventArgs e)
            => TileModified?.Invoke(sender, e);

        private void AttachListeners(IMPD_File mpdFile) {
            for (var x = 0; x < mpdFile.Tiles.GetLength(0); x++)
                for (var y = 0; y < mpdFile.Tiles.GetLength(1); y++)
                    mpdFile.Tiles[x, y].Modified += OnTileModified;
        }

        private void DetachListeners(IMPD_File mpdFile) {
            for (var x = 0; x < mpdFile.Tiles.GetLength(0); x++)
                for (var y = 0; y < mpdFile.Tiles.GetLength(1); y++)
                    mpdFile.Tiles[x, y].Modified -= OnTileModified;
        }

        private IMPD_File _mpdFile = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD_File MPD_File {
            get => _mpdFile;
            set {
                if (_mpdFile == value)
                    return;

                if (_mpdFile != null)
                    DetachListeners(_mpdFile);

                _mpdFile = value;
                if (_mpdFile != null) {
                    AttachListeners(_mpdFile);
                    _surfaceModel?.Invalidate();
                }

                UpdateLightPosition();
                UpdateLightingTexture();
                Invalidate();
            }
        }

        public event EventHandler RightDoubleClick;
        public event EventHandler MiddleDoubleClick;
        public event FrameTickEventHandler FrameTick;
        public event CmdKeyEventHandler CmdKey;
        public event EventHandler TileModified;

        private Timer _timer = null;
    }
}
