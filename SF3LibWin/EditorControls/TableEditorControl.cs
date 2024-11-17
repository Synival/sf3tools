using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Attributes;
using SF3.Tables;
using SF3.Tables.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.EditorControls {
    public class TableEditorControl : ITableEditorControl {
        private static int s_controlIndex = 1;

        public TableEditorControl(Table table, INameGetterContext nameGetterContext) {
            Table = table;
            NameGetterContext = nameGetterContext;
        }

        public Control Create() {

            var columnType = Table.RowObjs.GetType().GetElementType()!;
            var columnProperties = columnType
                .GetProperties()
                .OrderBy(x => x.DeclaringType == columnType ? 1 : 0)
                .ToList();

            var props = columnProperties
                .Select(x => new { Property = x, Attributes = x.GetCustomAttributes(typeof(MetadataAttribute), true) })
                .Where(x => x.Attributes.Length == 1)
                .OrderBy(x => ((MetadataAttribute) x.Attributes[0]).DisplayOrder)
                .ToDictionary(x => x.Property, x => (MetadataAttribute) x.Attributes[0]);

            var lvcColumns = new List<OLVColumn>();
            Font hexFont = null;

            var lvcNameBase = "lvcTableEditorControl" + s_controlIndex;

            foreach (var kv in props) {
                var prop = kv.Key;
                var attr = kv.Value;

                if (attr.EditorCapabilities == EditorCapabilities.Hidden)
                    continue;

                var lvc = new OLVColumn(lvcNameBase + prop.Name, prop.Name);
                if (hexFont == null)
                    hexFont = new Font("Courier New", Control.DefaultFont.Size);

                if (attr.EditorCapabilities == EditorCapabilities.Auto)
                    lvc.IsEditable = prop.GetSetMethod() != null;

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

            var olv = new ObjectListView();
            ((System.ComponentModel.ISupportInitialize) olv).BeginInit();
            olv.AllowColumnReorder = true;
            olv.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olv.FullRowSelect = true;
            olv.GridLines = true;
            olv.HasCollapsibleGroups = false;
            olv.HideSelection = false;
            olv.MenuLabelGroupBy = "";
            olv.Name = "olvTableEditorControl" + (s_controlIndex++);
            olv.ShowGroups = false;
            olv.TabIndex = 1;
            olv.UseAlternatingBackColors = true;
            olv.UseCompatibleStateImageBehavior = false;
            olv.View = View.Details;
            olv.AllColumns.AddRange(lvcColumns);
            olv.Columns.AddRange(lvcColumns.ToArray());
            olv.Enhance(NameGetterContext);
            olv.AddObjects(Table.RowObjs);
            ((System.ComponentModel.ISupportInitialize) olv).EndInit();

            OLVControl = olv;
            return olv;
        }

        public void Destroy() {
            if (Control == null)
                return;
            Control.Dispose();
            Control = null;
            OLVControl = null;
        }

        public void Dispose() {
            Destroy();
        }

        public Table Table { get; }
        public INameGetterContext NameGetterContext { get; }

        private ObjectListView OLVControl { get; set; } = null;

        public Control Control { get; private set; } = null;
    }
}
