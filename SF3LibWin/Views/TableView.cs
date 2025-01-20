using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.Models.Tables;
using SF3.Win.Extensions;

namespace SF3.Win.Views {
    public class TableView : ViewBase, ITableView {
        private static int s_controlIndex = 1;

        public TableView(string name, IBaseTable table, INameGetterContext nameGetterContext)
        : base(name) {
            Table = table;
            NameGetterContext = nameGetterContext;
            ModelType = (Table == null) ? null : Table.RowObjs.GetType().GetElementType()!;
        }

        /// <summary>
        /// This exists because of many layers of stupid.
        /// In short, Winforms provides no way to know when a Control is no longer visible because it's parent (like a tab) has changed.
        /// The least-worst hack is to continuously check the "Visible" property to know if this is actually visible or not.
        /// The alternative is to set up an elaborate system of events to *hopefully* inform child controls property.
        /// Just don't make 1,000 of these things at once, okay?
        /// </summary>
        private class EnhancedObjectListView : ObjectListView {
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

        private static Dictionary<string, Stack<EnhancedObjectListView>> _cachedOLVControls = new Dictionary<string, Stack<EnhancedObjectListView>>();

        private ObjectListView PopCachedOLV() {
            var key = ModelType.AssemblyQualifiedName;
            if (!_cachedOLVControls.ContainsKey(key))
                return null;
            var stack = _cachedOLVControls[key];
            if (stack.Count == 0)
                return null;

            var olv = stack.Pop();
            olv.Show();
            return olv;
        }

        private void PushCachedOLV() {
            var key = ModelType.AssemblyQualifiedName;
            if (!_cachedOLVControls.ContainsKey(key))
                _cachedOLVControls.Add(key, new Stack<EnhancedObjectListView>());
            _cachedOLVControls[key].Push((EnhancedObjectListView) OLVControl);
        }

        private ObjectListView GetOLV() {
            var olv = PopCachedOLV();
            if (olv == null) {
                var vm = ModelType.GetTableViewModel();

                var lvcColumns = new List<OLVColumn>();
                Font hexFont = null;

                var lvcNameBase = "lvcTableView" + s_controlIndex;

                foreach (var kv in vm.Properties) {
                    var prop = kv.Key;
                    var attr = kv.Value;

                    var lvc = new OLVColumn(lvcNameBase + prop.Name, prop.Name);
                    if (hexFont == null)
                        hexFont = new Font("Courier New", Control.DefaultFont.Size);

                    lvc.IsEditable = !attr.IsReadOnly && prop.GetSetMethod() != null;
                    lvc.Text = attr.DisplayName ?? prop.Name;

                    if (attr.DisplayFormat != null && attr.DisplayFormat != string.Empty)
                        lvc.AspectToStringFormat = "{0:" + attr.DisplayFormat + "}";
                    else if (attr.IsPointer)
                        lvc.AspectToStringFormat = "{0:X6}";
                    else
                        lvc.AspectToStringFormat = "";

                    lvc.Width = Math.Max(30, attr.MinWidth);
                    lvcColumns.Add(lvc);
                }

                olv = new EnhancedObjectListView();
                olv.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize) olv).BeginInit();
                olv.AllowColumnReorder = true;
                olv.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
                olv.FullRowSelect = true;
                olv.GridLines = true;
                olv.HasCollapsibleGroups = false;
                olv.HideSelection = false;
                olv.MenuLabelGroupBy = "";
                olv.Name = "olvTableView" + (s_controlIndex++);
                olv.ShowGroups = false;
                olv.TabIndex = 1;
                olv.UseAlternatingBackColors = true;
                olv.UseCompatibleStateImageBehavior = false;
                olv.View = View.Details;
                olv.AllColumns.AddRange(lvcColumns);
                olv.Columns.AddRange(lvcColumns.ToArray());
                olv.Enhance(NameGetterContext);
            }
            else {
                olv.SuspendLayout();
            }

            try {
                olv.AddObjects(Table.RowObjs);
            }
            catch {
                olv.ClearObjects();
                throw;
            }
            ((System.ComponentModel.ISupportInitialize) olv).EndInit();
            olv.ResumeLayout();

            return olv;
        }

        public override Control Create() {
            if (Table == null)
                return null;

            OLVControl = GetOLV();
            Control = OLVControl;
            return Control;
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (OLVControl != null) {
                OLVControl.ClearObjects();
                OLVControl.Parent = null;
                PushCachedOLV();
                OLVControl = null;
                Control = null;
            }
            base.Destroy();
        }

        public override void RefreshContent() {
            if (!IsCreated || OLVControl == null)
                return;

            OLVControl.RefreshAllItems();
        }

        public IBaseTable Table { get; }
        public Type ModelType { get; }
        public INameGetterContext NameGetterContext { get; }
        public ObjectListView OLVControl { get; private set; } = null;
    }
}
