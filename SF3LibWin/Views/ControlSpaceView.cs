using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public class ControlSpaceView : ViewBase, IControlSpaceView {
        public ControlSpaceView(string name) : base(name) { }

        public override Control Create() {
            _childViews = new List<IView>();

            var container = new Control(null, Name);
            container.Padding = new Padding();
            container.Margin = new Padding();

            Control = container;
            return Control;
        }

        public void DestroyChildViews() {
            if (!IsCreated || _childViews == null || _childViews.Count == 0)
                return;

            if (_childViews != null) {
                foreach (var c in _childViews)
                    c.Destroy();
                _childViews.Clear();
            }
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();
            DestroyChildViews();
            _childViews = null;

            base.Destroy();
        }

        public void CreateChild(IView childView, Action<Control> onCreate, bool autoFill = true) {
            if (childView == null)
                return;

            var childControl = childView.Create();
            if (childControl == null) {
                onCreate?.Invoke(null);
                return;
            }

            Control.SuspendLayout();
            if (autoFill)
                childControl.Dock = DockStyle.Fill;
            Control.Controls.Add(childControl);
            Control.ResumeLayout();

            _childViews.Add(childView);
            onCreate?.Invoke(childControl);
        }

        public bool RemoveChild(IView child) {
            if (!_childViews.Contains(child))
                return false;

            child.Destroy();
            _childViews.Remove(child);

            return true;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            foreach (var child in ChildViews)
                child.RefreshContent();
        }

        private List<IView> _childViews = null;
        public IEnumerable<IView> ChildViews => _childViews;
    }
}
