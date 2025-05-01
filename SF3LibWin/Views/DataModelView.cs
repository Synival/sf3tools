using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using CommonLib.ViewModels;
using SF3.Win.Controls;
using SF3.Win.Extensions;
using static CommonLib.Extensions.TypeExtensions;

namespace SF3.Win.Views {
    public class DataModelView : ViewBase {
        private static int s_controlIndex = 1;

        public DataModelView(string name, object model, INameGetterContext nameGetterContext, Type modelType = null, string[] displayGroups = null)
        : base(name) {
            DisplayGroups = displayGroups;
            Model = model;
            NameGetterContext = nameGetterContext;
            ModelType = modelType ?? ((model == null) ? null : model.GetType()!);
        }

        private class ModelObjectListView : EnhancedObjectListView {
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
        }

        private string GetCacheKey()
            => "ModelView_" + ModelType.FullName + "_" + NameGetterContext.Name + "_" + (DisplayGroups != null ? ("_" + string.Join("_", DisplayGroups)) : "");

        private EnhancedObjectListView PopCachedOLV()
            => EnhancedObjectListView.PopCachedOLV(GetCacheKey());

        private void PushCachedOLV()
            => EnhancedObjectListView.PushCachedOLV(GetCacheKey(), OLVControl);

        private EnhancedObjectListView GetOLV() {
            var olv = PopCachedOLV();
            if (olv == null) {
                var lvcColumns = new List<OLVColumn>();
                var lvcNameBase = "lvcModelView" + s_controlIndex;

                // "Index" column
                {
                    var lvc = new OLVColumn(lvcNameBase + "Index", "Index");
                    lvc.IsEditable = false;
                    lvc.Text       = "Index";
                    lvc.Width      = 50;
                    lvcColumns.Add(lvc);
                }

                // "Name" column
                {
                    var lvc = new OLVColumn(lvcNameBase + "Name", "Name");
                    lvc.IsEditable = false;
                    lvc.Text       = "Name";

                    var maxWidth = Math.Max(30, TextRenderer.MeasureText(lvc.Text, lvc.HeaderFont).Width + 8);
                    foreach (var mp in _modelProperties)
                        maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(mp.Name, lvc.HeaderFont).Width + 8);
                    lvc.Width = (int) (maxWidth * 0.85);

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

                olv = new ModelObjectListView();
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
            public ModelProperty(object model, PropertyInfo propertyInfo, TableViewModelColumn vmColumn, int index) {
                Model                = model;
                PropertyInfo         = propertyInfo;
                VMColumn             = vmColumn;
                Index                = index;
                IsReadOnly           = !vmColumn.GetColumnIsEditable(propertyInfo);
                Name                 = vmColumn.GetColumnText(propertyInfo);
                AspectToStringFormat = vmColumn.GetColumnAspectToStringFormat();
                Width                = vmColumn.GetColumnWidth();
            }

            public object Model { get; }
            public PropertyInfo PropertyInfo { get; }
            public TableViewModelColumn VMColumn { get; }
            public int Index { get; }

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
                        .Where(x => x.Value.DisplayOrder >= 0 || (x.Key.Name == "Address" && (int) x.Key.GetValue(value) >= 0))
                        .Where(x => x.Value.GetVisibility(value))
                        .Where(x => DisplayGroups == null || DisplayGroups.Contains(x.Value.DisplayGroup))
                        .Select((x, i) => new ModelProperty(value, x.Key, x.Value, i + 1)).ToList();
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
        public EnhancedObjectListView OLVControl { get; private set; } = null;
        public string[] DisplayGroups { get; }
    }
}
