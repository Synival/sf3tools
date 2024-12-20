﻿using System.Collections.Generic;
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

        public override void Destroy() {
            Control?.Hide();

            if (_childViews != null) {
                foreach (var c in _childViews)
                    c.Destroy();
                _childViews.Clear();
                _childViews = null;
            }

            base.Destroy();
        }

        public Control CreateChild(IView childView, bool autoFill = true) {
            if (childView == null)
                return null;

            var childControl = childView.Create();
            if (childControl == null)
                return null;

            Control.SuspendLayout();
            if (autoFill)
                childControl.Dock = DockStyle.Fill;
            Control.Controls.Add(childControl);
            Control.ResumeLayout();

            _childViews.Add(childView);
            return childControl;
        }

        public override void RefreshContent() {
            foreach (var child in ChildViews)
                child.RefreshContent();
        }

        private List<IView> _childViews = null;
        public IEnumerable<IView> ChildViews => _childViews;
    }
}
