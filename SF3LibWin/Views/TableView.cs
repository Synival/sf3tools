using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Tables;
using SF3.Win.Controls;
using SF3.Win.Utils;

namespace SF3.Win.Views {
    public class TableView : ViewBase, ITableView {
        private static int s_controlIndex = 1;

        public TableView(string name, ITable table, INameGetterContext nameGetterContext, Type modelType = null, string[] displayGroups = null)
        : base(name) {
            Table = table;
            NameGetterContext = nameGetterContext;
            ModelType = modelType ?? ((Table == null) ? null : Table.RowObjs.GetType().GetElementType()!);
            DisplayGroups = displayGroups;
        }

        private string GetCacheKey()
            => "TableView_" + ModelType.FullName + "_" + NameGetterContext.Name + "_" + (DisplayGroups != null ? ("_" + string.Join("_", DisplayGroups)) : "");

        private EnhancedObjectListView PopCachedOLV()
            => ObjectListViewUtils.PopCachedOLV(GetCacheKey());

        private void PushCachedOLV()
            => ObjectListViewUtils.PushCachedOLV(GetCacheKey(), OLVControl);

        private EnhancedObjectListView GetOLV() {
            var olv = PopCachedOLV();
            if (olv == null) {
                var vm = ModelType.GetTableViewModel();

                var lvcColumns = new List<OLVColumn>();
                var lvcNameBase = "lvcTableView" + s_controlIndex;

                foreach (var kv in vm.Properties) {
                    var prop = kv.Key;
                    var attr = kv.Value;

                    if (DisplayGroups != null)
                        if (!DisplayGroups.Contains(attr.DisplayGroup))
                            continue;

                    var lvc = new OLVColumn(lvcNameBase + prop.Name, prop.Name);
                    lvc.IsEditable           = attr.GetColumnIsEditable(prop);
                    lvc.Text                 = attr.GetColumnText(prop);
                    lvc.AspectToStringFormat = attr.GetColumnAspectToStringFormat();
                    lvc.Width                = attr.GetColumnWidth();

                    lvcColumns.Add(lvc);
                }

                olv = new EnhancedObjectListView(NameGetterContext);
                olv.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize) olv).BeginInit();
                olv.AllowColumnReorder = true;
                olv.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
                olv.FullRowSelect = true;
                olv.HasCollapsibleGroups = false;
                olv.HideSelection = false;
                olv.MenuLabelGroupBy = "";
                olv.Name = "olvTableView" + (s_controlIndex++);
                olv.ShowGroups = false;
                olv.TabIndex = 1;
                olv.UseAlternatingBackColors = true;
                olv.UseCompatibleStateImageBehavior = false;
                olv.View = View.Details;
                olv.AddColumns(lvcColumns);
            }
            else {
                olv.SuspendLayout();
            }

            try {
                if (Table != null) {
                    olv.AddObjects(Table.RowObjs);
                    UpdateColumnVisibility(olv);
                }
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

        private ITable _table = null;

        public ITable Table {
            get => _table;
            set {
                if (IsCreated) {
                    if (_table != null)
                        OLVControl.ClearObjects();
                    if (value != null) {
                        OLVControl.AddObjects(value.RowObjs);
                        UpdateColumnVisibility(OLVControl);
                    }
                }
                _table = value;
            }
        }

        private void UpdateColumnVisibility(ObjectListView olv) {
            var objs = olv.Objects;
            if (objs == null)
                return;

            var vm = ModelType.GetTableViewModel();
            var vmProperties = vm.Properties.ToDictionary(x => x.Key.Name, x => x.Value);

            var rows = objs.Cast<IStruct>().ToArray();
            var columns = olv.AllColumns.ToArray();

            foreach (var col in columns) {
                var vmCol = vmProperties.TryGetValue(col.AspectName, out var vmColValue) ? vmColValue : null;
                col.IsVisible = vmCol != null && rows.Any(x => vmCol.GetVisibility(x));
            }
        }

        public Type ModelType { get; }
        public INameGetterContext NameGetterContext { get; }
        public EnhancedObjectListView OLVControl { get; private set; } = null;
        public string[] DisplayGroups { get; }
    }
}
