using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Win.Extensions;

namespace SF3.Win.Controls {

    /// <summary>
    /// This exists because of many layers of stupid.
    /// In short, Winforms provides no way to know when a Control is no longer visible because it's parent (like a tab) has changed.
    /// The least-worst hack is to continuously check the "Visible" property to know if this is actually visible or not.
    /// The alternative is to set up an elaborate system of events to *hopefully* inform child controls property.
    /// Just don't make 1,000 of these things at once, okay?
    /// </summary>
    public class EnhancedObjectListView : ObjectListView {
        public static void PushCachedOLV(string key, EnhancedObjectListView olv) {
            if (!_cachedOLVControls.ContainsKey(key))
                _cachedOLVControls.Add(key, new Stack<EnhancedObjectListView>());
            _cachedOLVControls[key].Push(olv);
        }

        public static EnhancedObjectListView PopCachedOLV(string key) {
            if (!_cachedOLVControls.ContainsKey(key))
                return null;
            var stack = _cachedOLVControls[key];
            if (stack.Count == 0)
                return null;

            var olv = stack.Pop();
            olv.Show();
            return olv;
        }

        private static Dictionary<string, Stack<EnhancedObjectListView>> _cachedOLVControls = new Dictionary<string, Stack<EnhancedObjectListView>>();

        public EnhancedObjectListView() {
            _timer.Interval = 100;
            _timer.Tick += CheckForVisibility;
            _timer.Start();
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (Items.Count == 0 || !Visible || Parent == null || _wasVisible)
                return;

            // If we weren't visible before, refresh.
            this.RefreshAllItems();
            _wasVisible = true;
            _timer.Start();
        }

        /// <summary>
        /// Checks to see if we're visible. This is on a timer tick because there are no events to detect for this.
        /// (This seems oddly deliberate!!!)
        /// </summary>
        private void CheckForVisibility(object sender, EventArgs args) {
            if (Parent == null || !Visible) {
                _wasVisible = false;
                _timer.Stop();
            }
        }

        private bool _wasVisible = true;
        private Timer _timer = new Timer();
    }
}
