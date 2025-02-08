using System;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimationsView : ControlSpaceView {
        public TextureAnimationsView(string name, TextureAnimationTable model, INameGetterContext nameGetterContext) : base(name) {
            Model       = model;
            TableView   = new TableView("Animations", model, nameGetterContext);
            TextureView = new TextureView("Texture");

            _timer = new Timer() { Interval = 250 };
            _timer.Tick += AdvanceFrame;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnTextureChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var anim = (TextureAnimationModel) item?.RowObject;

            var frame = anim?.Frames?.FirstOrDefault();
            TextureView.Image = frame?.Texture?.CreateBitmapARGB1555() ?? null;

            _timer.Stop();

            if (frame != null) {
                _currentAnimation = anim;
                _currentFrameNum = 0;
                _timer.Interval = (int) _currentAnimation.Frames[_currentFrameNum].Duration * 1000 / 30;
                _timer.Start();
            }
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            _timer.Stop();
            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnTextureChanged;

            TableView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        private void AdvanceFrame(object sender, EventArgs eventArgs) {
            if (_currentAnimation == null)
                return;
            _currentFrameNum = (_currentFrameNum + 1) % _currentAnimation.NumFrames;
            var currentFrame = _currentAnimation.Frames[_currentFrameNum];
            TextureView.Image = currentFrame.Texture?.CreateBitmapARGB1555();
            _timer.Interval = (int) currentFrame.Duration * 1000 / 30;
        }

        public TextureAnimationTable Model { get; }
        public TableView TableView { get; private set; }
        public TextureView TextureView { get; private set; }

        private TextureAnimationModel _currentAnimation = null;
        private int _currentFrameNum = 0;
        private Timer _timer = null;
    }
}