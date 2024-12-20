using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using SF3.FileEditors;
using static SF3.Editor.Utils.ControlUtils;

namespace SF3.Editor.Extensions {
    public static class ObjectListViewExtensions {
        /// <summary>
        /// Runs RefreshItem() on all OLVListItem's in the property Items.
        /// </summary>
        /// <param name="olv">The ObjectListView to refresh.</param>
        public static void RefreshAllItems(this ObjectListView olv) {
            foreach (var item in olv.Items) {
                var olvItem = item as OLVListItem;
                olv.RefreshItem(olvItem);
            }
        }

        /// <summary>
        /// Renderer that will display hex values will a special font.
        /// </summary>
        private class HexRenderer : BaseRenderer {
            public static Font HexFont { get; private set; }
            private static bool _useHexFont = false;

            public HexRenderer() {
            }

            public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
                var lvc = ((ObjectListView) e.Item.ListView).GetColumn(e.ColumnIndex);
                _ = lvc.AspectGetter(rowObject);
                _useHexFont = lvc.AspectToStringConverter == null && (lvc.AspectToStringFormat?.StartsWith("{0:X") == true);
                return base.RenderSubItem(e, g, cellBounds, rowObject);
            }

            public override void Render(Graphics g, Rectangle r) {
                if (_useHexFont) {
                    if (HexFont == null)
                        HexFont = new Font("Lucida Sans Typewriter", Font.Size);
                    this.Font = HexFont;
                }
                base.Render(g, r);
            }
        }

        private static HexRenderer GlobalHexRenderer = new HexRenderer();

        /// <summary>
        /// Applies some neat extensions to the ObjectListView.
        /// </summary>
        /// <param name="olv">The ObjectListView to enhance.</param>
        /// <param name="fileEditorFetcher">The function that fetchers the current FileEditor associated for this ObjectListView.</param>
        public static void Enhance(this ObjectListView olv, FileEditorFetcher fileEditorFetcher) {
            olv.SetFileEditorFetcher(fileEditorFetcher);
            olv.OwnerDraw = true;
            olv.DefaultRenderer = GlobalHexRenderer;
            foreach (var lvc in olv.AllColumns)
                lvc.Enhance();
        }

        /// <summary>
        /// Adds some extra functionality to a column of an ObjectListView.
        /// </summary>
        /// <param name="lvc">The ObjectListView column to enhance.</param>
        public static void Enhance(this OLVColumn lvc) {
            // TODO: maybe put this in the columns? this is a bit extreme!!!
            if (lvc.AspectToStringFormat == "{0:X}")
                lvc.AspectToStringFormat = "{0:X2}";

            // Add a hook to each AspectGetter that will check for a named value.
            // If a name exists, hijack the AspectToStringConverter to use the name instead.
            // If no name exists, use the standard AspectToStringConverter.
            // (It would be nice if we could set one single AspectToStringConverter to check for this,
            // but alas, it only takes one paramter (value) and that's not enough to check for a name.)
            lvc.AspectGetter = obj => {
                AspectToStringConverterDelegate converter = null;

                var nameContext = ((ObjectListView) lvc.ListView).GetFileEditor()?.NameContext;
                if (nameContext != null) {
                    var property = obj.GetType().GetProperty(lvc.AspectName);
                    if (property != null) {
                        var attr = property.GetCustomAttribute<NameGetterAttribute>();
                        if (attr != null) {
                            var value = property.GetValue(obj);
                            if (nameContext.CanGetName(obj, property, value, attr.Parameters))
                                converter = v => obj.GetPropertyValueName(property, nameContext, value) ?? string.Format(lvc.AspectToStringFormat, lvc.GetAspectByName(obj));
                        }
                    }
                }

                lvc.AspectToStringConverter = converter;
                return lvc.GetAspectByName(obj);
            };
        }

        /// <summary>
        /// Adds some extra functionality to the Control created when editing an ObjectListView cell.
        /// </summary>
        /// <param name="olv">The ObjectListView whose control should be modified.</param>
        /// <param name="e">Arguments from the OlvCellEditControl event.</param>
        public static void EnhanceOlvCellEditControl(this ObjectListView olv, CellEditEventArgs e) {
            // Enhance ComboBox's so values are updated any time the dropdown is closed, unless from hitting "escape".
            if (e.Control is ComboBox) {
                var cb = e.Control as ComboBox;
                cb.KeyDown += (s, e2) => {
                    if (e2.KeyCode == Keys.Escape)
                        cb.SelectedValue = e.Column.GetValue(e.RowObject);
                };
                cb.DropDownClosed += (s, e2) => {
                    e.Column.PutValue(e.RowObject, cb.SelectedValue);
                    olv.RefreshItem(e.ListViewItem);
                };
            }
            else if (e.Control is NumericUpDown control) {
                // Ensure that strings displayed in hex format are edited in hex format.
                if (e.Column.AspectToStringFormat?.StartsWith("{0:X") == true) {
                    control.Font = HexRenderer.HexFont;
                    control.Hexadecimal = true;
                }
            }
        }

        /// <summary>
        /// Function to use for each EditorCreatorDelegate we're hijacking.
        /// Creates a combo box instead of the standard control if a named value is present.
        /// </summary>
        /// <param name="obj">The object bound to the ObjectListView row.</param>
        /// <param name="model">The column of the OLV.</param>
        /// <param name="value">The value fetched from the column.</param>
        /// <param name="oldDelegate">The EditorCreatorDelegate we're replacing to use as a fallback.</param>
        /// <returns>The control to use when editing - a ComboBox for named values, otherwise the return value of 'oldDelegate'.</returns>
        private static Control NamedValueEditorCreator(object obj, OLVColumn model, object value, EditorCreatorDelegate oldDelegate) {
            if (Globals.UseDropdowns) {
                var nameContext = ((ObjectListView) model.ListView).GetFileEditor()?.NameContext;
                if (nameContext != null) {
                    var property = obj.GetType().GetProperty(model.AspectName);
                    var attr = property.GetCustomAttribute<NameGetterAttribute>();
                    if (attr != null && nameContext.CanGetInfo(obj, property, attr.Parameters)) {
                        var intValue = (int) property.GetValue(obj);
                        if (nameContext.CanGetName(obj, property, intValue, attr.Parameters))
                            return MakeNamedValueComboBox(nameContext.GetInfo(obj, property, attr.Parameters), intValue);
                    }
                }
            }

            return oldDelegate(obj, model, value);
        }

        private class Int32UpDown : NumericUpDown
        {
            public Int32UpDown() {
                this.DecimalPlaces = 0;
                this.Minimum = int.MinValue;
                this.Maximum = int.MaxValue;
            }

            new public int Value {
                get { return decimal.ToInt32(base.Value); }
                set { base.Value = new decimal(value); }
            }
        }

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterNamedValues() {
            /// BIG HACK to get existing editor delegates.
            var creatorMapField = ObjectListView.EditorRegistry.GetType().GetField(
                "creatorMap", BindingFlags.NonPublic | BindingFlags.Instance);
            var creatorMap = (Dictionary<Type, EditorCreatorDelegate>) creatorMapField.GetValue(ObjectListView.EditorRegistry);

            ObjectListView.EditorRegistry.Register(typeof(int), typeof(Int32UpDown));

            var typesToHijack = new Type[] {
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(ushort),
                typeof(uint),
                typeof(ulong)
            };

            foreach (var type in typesToHijack) {
                var creator = creatorMap[type];
                ObjectListView.EditorRegistry.Register(type, (obj, model, value)
                    => NamedValueEditorCreator(obj, model, value, creator));
            }
        }

        private static Dictionary<ObjectListView, FileEditorFetcher> _olvFileEditorFetchers = new Dictionary<ObjectListView, FileEditorFetcher>();
        public delegate IFileEditor FileEditorFetcher();

        public static void SetFileEditorFetcher(this ObjectListView olv, FileEditorFetcher fetcher)
            => _olvFileEditorFetchers[olv] = fetcher;

        public static FileEditorFetcher GetFileEditorFetcher(this ObjectListView olv)
            => _olvFileEditorFetchers.TryGetValue(olv, out FileEditorFetcher fetcher) ? fetcher : null;

        public static IFileEditor GetFileEditor(this ObjectListView olv)
            => (olv.GetFileEditorFetcher() is var fetcher) ? fetcher() : null;
    }
}
