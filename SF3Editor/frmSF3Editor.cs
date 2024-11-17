using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.BulkOperations;
using SF3.Editors.MPD;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;
using SF3.Win.Extensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        const string c_mpdPath = "";

        public frmSF3Editor() {
            SuspendLayout();
            InitializeComponent();

            var mpdEditor = MPD_Editor.Create(
                new ByteEditor(File.ReadAllBytes(c_mpdPath)),
                new NameGetterContext(ScenarioType.Scenario1),
                ScenarioType.Scenario1);

            var table = mpdEditor.ChunkHeader;
            var columnType = table.Rows.GetType().GetElementType()!;
            var columnProperties = columnType
                .GetProperties()
                .OrderBy(x => x.DeclaringType == columnType ? 1 : 0)
                .ToList();

            var props = columnProperties
                .Select(x => new { Property = x, Attributes = x.GetCustomAttributes(typeof(DataMetadataAttribute), true) })
                .Where(x => x.Attributes.Length == 1)
                .OrderBy(x => ((DataMetadataAttribute) x.Attributes[0]).DisplayOrder)
                .ToDictionary(x => x.Property, x => (DataMetadataAttribute) x.Attributes[0]);

            var lvcColumns = new List<OLVColumn>();
            Font? hexFont = null;

            foreach (var kv in props) {
                var prop = kv.Key;
                var attr = kv.Value;

                if (attr.EditorCapabilities == EditorCapabilities.Hidden)
                    continue;

                var lvc = new OLVColumn("lvcTest" + prop.Name, prop.Name);
                if (hexFont == null)
                    hexFont = new Font("Courier New", DefaultFont.Size);

                if (attr.EditorCapabilities == EditorCapabilities.Auto)
                    lvc.IsEditable = prop.GetSetMethod() != null;

                lvc.Text = attr.DisplayName ?? prop.Name;
                if (attr.DisplayFormat != null && attr.DisplayFormat != string.Empty)
                    lvc.AspectToStringFormat = "{0:" + attr.DisplayFormat + "}";
                else if (attr.IsPointer)
                    lvc.AspectToStringFormat = "{0:X6}";
                else
                    lvc.AspectToStringFormat = "";

                var headerTextWidth = TextRenderer.MeasureText(lvc.Text, DefaultFont).Width + 8;
                var aspectTextSample = string.Format(lvc.AspectToStringFormat, 0);
                var aspectTextWidth = TextRenderer.MeasureText(aspectTextSample, hexFont).Width + 4;
                lvc.Width = Math.Max(Math.Max(headerTextWidth, aspectTextWidth), attr.MinWidth);

                lvcColumns.Add(lvc);
            }

            var olv = new ObjectListView();
            ((System.ComponentModel.ISupportInitialize) olv).BeginInit();
            olv.AllowColumnReorder = true;
            olv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            olv.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olv.FullRowSelect = true;
            olv.GridLines = true;
            olv.HasCollapsibleGroups = false;
            olv.HideSelection = false;
            olv.Location = new System.Drawing.Point(3, 3);
            olv.MenuLabelGroupBy = "";
            olv.Name = "TestOLV";
            olv.ShowGroups = false;
            olv.Size = masterEditorControl1.Size;
            olv.TabIndex = 1;
            olv.UseAlternatingBackColors = true;
            olv.UseCompatibleStateImageBehavior = false;
            olv.View = System.Windows.Forms.View.Details;
            olv.AllColumns.AddRange(lvcColumns);
            olv.Columns.AddRange(lvcColumns.ToArray());
            ((System.ComponentModel.ISupportInitialize) olv).EndInit();

            masterEditorControl1.Controls.Add(olv);
            ResumeLayout();

            olv.Enhance(() => mpdEditor);
            olv.AddObjects(table.Rows);
        }
    }
}
