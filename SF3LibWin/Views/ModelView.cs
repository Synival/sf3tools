using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using CommonLib.ViewModels;
using SF3.Win.Extensions;
using static CommonLib.Extensions.TypeExtensions;

namespace SF3.Win.Views {
    public class ModelView : ViewBase {
        private static int s_controlIndex = 1;

        public ModelView(string name, object model, INameGetterContext nameGetterContext, Type modelType = null)
        : base(name) {
            Model = model;
            NameGetterContext = nameGetterContext;
            ModelType = modelType ?? ((model == null) ? null : model.GetType()!);
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

            public override void EditSubItem(OLVListItem item, int subItemIndex) {
                // Make sure we can't edit the actual value, not just the column.
                if (item == null || ((ModelProperty) item.RowObject).IsReadOnly)
                    return;
                base.EditSubItem(item, subItemIndex);
            }

            public override void Sort(OLVColumn columnToSort, SortOrder order) {
                // Prevent sorting of "Value" column (it crashes at the moment).
                if (columnToSort?.AspectName != "Value")
                    base.Sort(columnToSort, order);
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
                var lvcColumns = new List<OLVColumn>();
                var lvcNameBase = "lvcModelView" + s_controlIndex;

                // "Name" column
                {
                    var lvc = new OLVColumn(lvcNameBase + "Name", "Name");
                    lvc.IsEditable = false;
                    lvc.Text       = "Name";

                    var maxWidth = Math.Max(30, TextRenderer.MeasureText(lvc.Text, lvc.HeaderFont).Width + 8);
                    foreach (var mp in _modelProperties)
                        maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(mp.Name, lvc.HeaderFont).Width + 8);
                    lvc.Width = (int) (maxWidth * 0.80);

                    lvcColumns.Add(lvc);
                }

                // "Is Read-Only" column
                {
                    var lvc = new OLVColumn(lvcNameBase + "IsReadOnly", "IsReadOnly");
                    lvc.IsEditable = false;
                    lvc.Text       = "Is Read-Only";
                    lvcColumns.Add(lvc);
                }

                // "Value" column
                {
                    var lvc = new OLVColumn(lvcNameBase + "Value", "Value");
                    lvc.IsEditable = true;
                    lvc.Text       = "Value";
                    lvc.Width      = _modelProperties.Max(x => x.Width);

                    // AspectToStringFormat is used after fetching fetching the aspect with AspectGetter().
                    // We can take advantage of that by setting the AspectToStringFormat for the specific value.
                    // (It would be nice if there was a string converter that took the actual row object, not just the value.)
                    lvc.AspectGetter = (row) => {
                        lvc.AspectToStringFormat = ((ModelProperty) row).AspectToStringFormat;
                        return lvc.GetAspectByName(row);
                    };

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
                olv.Name = "olvModelView" + (s_controlIndex++);
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
                if (Model != null)
                    olv.AddObjects(_modelProperties);
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
            if (ModelType == null)
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

        /// <summary>
        /// Item row representing an individual property with its own view model.
        /// </summary>
        private class ModelProperty {
            public ModelProperty(object model, PropertyInfo propertyInfo, TableViewModelColumn vmColumn) {
                Model                = model;
                PropertyInfo         = propertyInfo;
                VMColumn             = vmColumn;
                IsReadOnly           = !vmColumn.GetColumnIsEditable(propertyInfo);
                Name                 = vmColumn.GetColumnText(propertyInfo);
                AspectToStringFormat = vmColumn.GetColumnAspectToStringFormat(propertyInfo);
                Width                = vmColumn.GetColumnWidth();
            }

            public object Model { get; }
            public PropertyInfo PropertyInfo { get; }
            public TableViewModelColumn VMColumn { get; }

            public bool IsReadOnly { get; }
            public string Name { get; }
            public string AspectToStringFormat { get; }
            public int Width { get; }

            public object Value {
                get => PropertyInfo.GetValue(Model, null);
                set {
                    if (!IsReadOnly)
                        PropertyInfo.SetValue(Model, value);
                }
            }
        };

        private object _model = null;

        public object Model {
            get => _model;
            set {
                _modelProperties = null;

                // Update the property list when a new model is selected.
                if (value != null) {
                    var vm = value.GetType().GetTableViewModel();
                    _modelProperties = vm.Properties
                        .Where(x => x.Value.DisplayOrder >= 0 || x.Key.Name == "Address")
                        .Select(x => new ModelProperty(value, x.Key, x.Value)).ToList();
                }

                if (IsCreated) {
                    if (_model != null)
                        OLVControl.ClearObjects();
                    if (value != null)
                        OLVControl.AddObject(_modelProperties);
                }
                _model = value;
            }
        }

        private List<ModelProperty> _modelProperties;

        public Type ModelType { get; }
        public INameGetterContext NameGetterContext { get; }
        public ObjectListView OLVControl { get; private set; } = null;
    }
}
