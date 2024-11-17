using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using static SF3.Win.Utils.ControlUtils;

namespace SF3.Win.Extensions {
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
            private static readonly Font _defaultFont = Control.DefaultFont;
            private static readonly Font _hexFont = new Font("Courier New", Control.DefaultFont.Size);

            private Font _currentRenderFont = Control.DefaultFont;
            private Color _currentRenderColor = Color.Black;

            public static Font GetCellFont(string formatString)
                => formatString.StartsWith("{0:X") ? _hexFont : _defaultFont;

            public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
                var lvc = ((ObjectListView) e.Item.ListView).GetColumn(e.ColumnIndex);
                _ = lvc.AspectGetter(rowObject);

                // If an AspectToStringConverter was supplied, this is probably a named value. Just use the default font.
                var formatString = (lvc.AspectToStringConverter == null) ? (lvc.AspectToStringFormat ?? "") : "";
                _currentRenderFont = GetCellFont(formatString);
                _currentRenderColor = lvc.IsEditable ? Color.Black : _readOnlyColor;

                return base.RenderSubItem(e, g, cellBounds, rowObject);
            }

            protected override Color GetForegroundColor()
                => _currentRenderColor;

            public override void Render(Graphics g, Rectangle r) {
                Font = _currentRenderFont;
                base.Render(g, r);
            }
        }

        private static HexRenderer GlobalHexRenderer = new HexRenderer();

        /// <summary>
        /// Applies some neat extensions to the ObjectListView.
        /// </summary>
        /// <param name="olv">The ObjectListView to enhance.</param>
        /// <param name="nameGetterContext">Name getter context used for display and editing named values.</param>
        public static void Enhance(this ObjectListView olv, INameGetterContext nameGetterContext)
            => Enhance(olv, () => nameGetterContext);

        private static readonly Color _headerBackColor = Color.FromArgb(244, 244, 244);
        private static readonly Color _readOnlyColor = Color.FromArgb(96, 96, 96);

        /// <summary>
        /// Applies some neat extensions to the ObjectListView.
        /// </summary>
        /// <param name="olv">The ObjectListView to enhance.</param>
        /// <param name="nameGetterContextFetcher">Callback function for getting the NameGetterContext associated for this ObjectListView.</param>
        public static void Enhance(this ObjectListView olv, NameGetterContextFetcher nameGetterContextFetcher) {
            var hexFont = new Font("Courier New", Control.DefaultFont.Size);
            olv.HeaderUsesThemes = false;

            olv.HeaderFormatStyle = new HeaderFormatStyle();
            var olvStyle = olv.HeaderFormatStyle;
            olvStyle.SetFont(Control.DefaultFont);
            olvStyle.Normal.BackColor = _headerBackColor;

            // Make sure the column can fit its text.
            foreach (var lvc in olv.AllColumns) {
                if (!lvc.IsEditable) {
                    lvc.HeaderFormatStyle = new HeaderFormatStyle();
                    var lvcStyle = lvc.HeaderFormatStyle;
                    lvcStyle.SetFont(Control.DefaultFont);
                    lvcStyle.SetForeColor(_readOnlyColor);
                    lvcStyle.Normal.BackColor = _headerBackColor;
                }

                var headerTextWidth = TextRenderer.MeasureText(lvc.Text, lvc.HeaderFont).Width + 8;
                var aspectTextSample = string.Format(lvc.AspectToStringFormat ?? "", 0);
                var aspectTextWidth = TextRenderer.MeasureText(aspectTextSample, hexFont).Width + 4;
                lvc.Width = Math.Max(Math.Max(headerTextWidth, aspectTextWidth), lvc.Width);
            }

            olv.SetNameGetterContextFetcher(nameGetterContextFetcher);
            olv.OwnerDraw = true;
            olv.DefaultRenderer = GlobalHexRenderer;
            olv.CellEditStarting += (s, e) => olv.EnhanceOlvCellEditControl(e);

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

                var nameContext = ((ObjectListView) lvc.ListView).GetNameGetterContext();
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

                // Auto-expand the ComboBox when opened.
                // This needs to happen at a specific point: when the data is populated, and the correct item is selected.
                // Normally we can wait for a SelectedIndexChanged event, but if the selected index is changed to 0,
                // no change took place and the event will not trigger. In that case, just trigger on GotFocus.
                if ((int) e.Value != 0) {
                    void selectedIndexChangedFunc(object sender, EventArgs args) {
                        cb.DroppedDown = true;
                        cb.SelectedIndexChanged -= selectedIndexChangedFunc;
                    };
                    cb.SelectedIndexChanged += selectedIndexChangedFunc;
                }
                else {
                    void selectedIndexChangedFunc(object sender, EventArgs args) {
                        cb.DroppedDown = true;
                        cb.GotFocus -= selectedIndexChangedFunc;
                    };
                    cb.GotFocus += selectedIndexChangedFunc;
                }
            }
            else if (e.Control is NumericUpDown control) {
                control.Font = HexRenderer.GetCellFont(e.Column.AspectToStringFormat ?? "");

                // Ensure that strings displayed in hex format are edited in hex format.
                if (e.Column.AspectToStringFormat?.StartsWith("{0:X") == true)
                    control.Hexadecimal = true;
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
                var nameContext = ((ObjectListView) model.ListView).GetNameGetterContext();
                if (nameContext != null) {
                    var property = obj.GetType().GetProperty(model.AspectName);
                    if (property != null) {
                        var attr = property.GetCustomAttribute<NameGetterAttribute>();
                        if (attr != null && nameContext.CanGetInfo(obj, property, attr.Parameters)) {
                            var intValue = (int) property.GetValue(obj);
                            if (nameContext.CanGetName(obj, property, intValue, attr.Parameters))
                                return MakeNamedValueComboBox(nameContext.GetInfo(obj, property, attr.Parameters), intValue);
                        }
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

        public delegate INameGetterContext NameGetterContextFetcher();
        private static Dictionary<ObjectListView, NameGetterContextFetcher> _olvNameGetterContextFetchers = new Dictionary<ObjectListView, NameGetterContextFetcher>();

        public static void SetNameGetterContextFetcher(this ObjectListView olv, NameGetterContextFetcher fetcher)
            => _olvNameGetterContextFetchers[olv] = fetcher;

        public static NameGetterContextFetcher GetNameGetterContextFetcher(this ObjectListView olv)
            => _olvNameGetterContextFetchers.TryGetValue(olv, out NameGetterContextFetcher fetcher) ? fetcher : null;

        public static INameGetterContext GetNameGetterContext(this ObjectListView olv)
            => (olv.GetNameGetterContextFetcher() is var fetcher) ? fetcher() : null;
    }
}
