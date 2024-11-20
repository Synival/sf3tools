using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Attributes;
using SF3.Tables;
using SF3.Win.Extensions;

namespace SF3.Win.Views {
    public class TableView : ViewBase, ITableView {
        private static int s_controlIndex = 1;

        public TableView(string name, Table table, INameGetterContext nameGetterContext)
        : base(name) {
            Table = table;
            NameGetterContext = nameGetterContext;
        }

        public override Control Create() {
            if (Table == null)
                return null;

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

            var lvcNameBase = "lvcTableView" + s_controlIndex;

            foreach (var kv in props) {
                var prop = kv.Key;
                var attr = kv.Value;

                var lvc = new OLVColumn(lvcNameBase + prop.Name, prop.Name);
                if (hexFont == null)
                    hexFont = new Font("Courier New", Control.DefaultFont.Size);

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
            try {
                olv.AddObjects(Table.RowObjs);
            }
            catch {
                olv.ClearObjects();
                throw;
            }
            ((System.ComponentModel.ISupportInitialize) olv).EndInit();
            olv.ResumeLayout();

            Control = olv;
            OLVControl = olv;
            return olv;
        }

        public override void Destroy() {
            base.Destroy();
            OLVControl = null;
        }

        public Table Table { get; }
        public INameGetterContext NameGetterContext { get; }

        private ObjectListView OLVControl { get; set; } = null;
    }
}
