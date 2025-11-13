using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using CommonLib.ViewModels;
using CommonLib.Win.DarkMode;
using CommonLib.Win.Utils;
using SF3.Models.Structs;
using SF3.Win.Views;

namespace SF3.Win.Controls {
    /// <summary>
    /// This exists because of many layers of stupid.
    /// In short, Winforms provides no way to know when a Control is no longer visible because it's parent (like a tab) has changed.
    /// The least-worst hack is to continuously check the "Visible" property to know if this is actually visible or not.
    /// The alternative is to set up an elaborate system of events to *hopefully* inform child controls property.
    /// Just don't make 1,000 of these things at once, okay?
    /// </summary>
    public partial class EnhancedObjectListView : ObjectListView {
        public delegate INameGetterContext NameGetterContextFetcherHandler();

        public EnhancedObjectListView(INameGetterContext nameGetterContext) : this(() => nameGetterContext) {}

        public EnhancedObjectListView(NameGetterContextFetcherHandler nameGetterContextFetcher) {
            SuspendLayout();

            HeaderUsesThemes = false;
            HeaderFormatStyle = new HeaderFormatStyle();
            var olvStyle = HeaderFormatStyle;
            olvStyle.SetFont(DefaultFont);

            CellToolTipGetter = GetCellTooltip;
            HighlightBackgroundColor = SystemColors.Highlight;
            UnfocusedHighlightBackgroundColor = MathHelpers.Lerp(HighlightBackgroundColor, SystemColors.Window, 0.50f);

            NameGetterContextFetcher = nameGetterContextFetcher;
            OwnerDraw = true;
            GridLines = false;
            DefaultRenderer = EnhancedOLVRenderer.Instance;
            CellEditStarting += (s, e) => EnhanceOlvCellEditControl(e);

            HighlightBackgroundColor = SystemColors.Highlight;
            HighlightForegroundColor = SystemColors.HighlightText;
            UnfocusedHighlightBackgroundColor = MathHelpers.Lerp(HighlightBackgroundColor, BackColor, 0.50f);
            UnfocusedHighlightForegroundColor = SystemColors.HighlightText;
            HeaderBackgroundColor = Color.FromArgb(0xF0, 0xF0, 0xF0);

            _timer.Interval = 100;
            _timer.Tick += CheckForVisibility;
            _timer.Start();

            ResumeLayout();
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);

            if (DarkModeContext == null) {
                OriginalAltLineColor1 = AlternateRowBackColor;
                OriginalAltLineColor2 = AlternateRowBackColor2;
                OriginalReadOnlyFieldColor = ReadOnlyFieldColor;
                OriginalHighlightBackgroundColor = HighlightBackgroundColor;
                OriginalHighlightForegroundColor = HighlightForegroundColor;
                OriginalUnfocusedHighlightBackgroundColor = UnfocusedHighlightBackgroundColor;
                OriginalUnfocusedHighlightForegroundColor = UnfocusedHighlightForegroundColor;
                OriginalHeaderBackColor = HeaderBackgroundColor;
                OriginalHeaderSelectedColor = HeaderSelectedColor;
                OriginalHeaderHoverColor = HeaderHoverColor;

                DarkModeContext = new DarkModeControlContext<EnhancedObjectListView>(this);
                DarkModeContext.EnabledChanged += (s, e) => {
                    if (DarkModeContext.Enabled) {
                        AlternateRowBackColor  = Color.FromArgb(0x30, 0x30, 0x00);
                        AlternateRowBackColor2 = Color.FromArgb(0x00, 0x00, 0x30);
                        ReadOnlyFieldColor = DarkModeColors.HighlightedDisabledColor;
                        HighlightBackgroundColor = DarkModeColors.HighlightedBackColor;
                        HighlightForegroundColor = DarkModeColors.HighlightedForeColor;
                        UnfocusedHighlightBackgroundColor = DarkModeColors.HalfHighlightedBackColor;
                        UnfocusedHighlightForegroundColor = DarkModeColors.HighlightedForeColor;
                        HeaderBackgroundColor = DarkModeColors.BackColor;
                        HeaderSelectedColor = DarkModeColors.HighlightedBackColor;
                        HeaderHoverColor  = DarkModeColors.HalfHighlightedBackColor;
                    }
                    else {
                        AlternateRowBackColor  = OriginalAltLineColor1;
                        AlternateRowBackColor2 = OriginalAltLineColor2;
                        ReadOnlyFieldColor = OriginalReadOnlyFieldColor;
                        HighlightBackgroundColor = OriginalHighlightBackgroundColor;
                        HighlightForegroundColor = OriginalHighlightForegroundColor;
                        UnfocusedHighlightBackgroundColor = OriginalUnfocusedHighlightBackgroundColor;
                        UnfocusedHighlightForegroundColor = OriginalUnfocusedHighlightForegroundColor;
                        HeaderBackgroundColor = OriginalHeaderBackColor;
                        HeaderSelectedColor = OriginalHeaderSelectedColor;
                        HeaderHoverColor  = OriginalHeaderHoverColor;
                    }
                    RefreshAllItems();
                };
                DarkModeContext.Init();
            }
        }

        protected override void OnCreateControl() {
            // To render the non-client area of the header -- which we need to do to render dark mode -- we need to
            // subclass the window handle to extend the WM_PAINT procedure.
            SubClassHeader();
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

        protected override void PostProcessOneRow(int rowIndex, int displayIndex, OLVListItem olvi) {
            base.PostProcessOneRow(rowIndex, displayIndex, olvi);
            if (UseAlternatingBackColors) {
                // OLV is silly and needs its alternate color setting corrected if specified.
                if (displayIndex % 3 == 1)
                    olvi.BackColor = (AlternateRowBackColor2 == Color.Empty) ? Color.LightBlue : AlternateRowBackColor2;
                else if (displayIndex % 2 == 1)
                    olvi.BackColor = (AlternateRowBackColor == Color.Empty) ? Color.LemonChiffon : AlternateRowBackColor;
                else
                    olvi.BackColor = BackColor;
            }
        }

        protected override Control MakeDefaultCellEditor(OLVColumn column) {
            var tb = new DarkModeTextBox();
            if (column.AutoCompleteEditor)
                ConfigureAutoComplete(tb, column);
            return tb;
        }

        /// <summary>
        /// Runs RefreshItem() on all OLVListItem's in the property Items.
        /// </summary>
        /// <param name="olv">The ObjectListView to refresh.</param>
        public void RefreshAllItems() {
            foreach (var item in Items)
                RefreshItem(item as OLVListItem);
        }

        public void AddColumn(OLVColumn lvc) {
            lvc.HeaderFormatStyle = new HeaderFormatStyle();
            var lvcStyle = lvc.HeaderFormatStyle;
            lvcStyle.SetFont(DefaultFont);
            lvcStyle.SetForeColor((lvc?.IsEditable ?? true) ? ForeColor : _readOnlyFieldColor);
            lvcStyle.Normal.BackColor  = _headerBackgroundColor;
            lvcStyle.Hot.BackColor     = _headerHoverColor;
            lvcStyle.Pressed.BackColor = _headerSelectedColor;

            var headerTextWidth = TextRenderer.MeasureText(lvc.Text, lvc.HeaderFont).Width + 8;
            var aspectTextSample = string.Format(lvc.AspectToStringFormat ?? "", 0);
            var aspectTextWidth = TextRenderer.MeasureText(aspectTextSample, EnhancedOLVRenderer.HexFont).Width + 4;
            lvc.Width = Math.Max(Math.Max(headerTextWidth, aspectTextWidth), lvc.Width);

            // TODO: maybe put this in the columns? this is a bit extreme!!!
            if (lvc.AspectToStringFormat == "{0:X}")
                lvc.AspectToStringFormat = "{0:X2}";

            // Add a hook to each AspectGetter that will check for a named value.
            // If a name exists, hijack the AspectToStringConverter to use the name instead.
            // If no name exists, use the standard AspectToStringConverter.
            // (It would be nice if we could set one single AspectToStringConverter to check for this,
            // but alas, it only takes one paramter (value) and that's not enough to check for a name.)
            var oldGetter = lvc.AspectGetter;
            lvc.AspectGetter = obj => {
                AspectToStringConverterDelegate converter = null;

                var nameContext = ((EnhancedObjectListView) lvc.ListView).NameGetterContext;
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
                return (oldGetter != null) ? oldGetter(obj) : lvc.GetAspectByName(obj);
            };

            AllColumns.Add(lvc);
            Columns.Add(lvc);
        }

        public void AddColumns(IEnumerable<OLVColumn> lvcs) {
            foreach (var lvc in lvcs)
                AddColumn(lvc);
        }

        private string GetCellTooltip(OLVColumn column, object modelObject) {
            Type modelType = modelObject.GetType();
            Struct modelStruct = modelObject as Struct;
            PropertyInfo modelProp = null;
            TableViewModelColumn propAttr = null;
            string propName = column.AspectName;
            bool isMultiRow = true;

            // For data tables, the model is an abstraction of the property as a row, not a column.
            // Fetch the data directly from the 'ModelProperty'.
            if (modelObject is ModelProperty modelProperty) {
                isMultiRow  = false;
                modelObject = modelProperty.Model;
                modelType   = modelObject.GetType();
                modelStruct = modelObject as Struct;
                modelProp   = modelProperty.PropertyInfo;
                propAttr    = modelProperty.VMColumn;
                propName    = modelProperty.Name;
            }
            // Otherwise, assume 'modelObject' really is the model. Fetch the property and its metadata.
            else {
                // Get the property. Allow for failure in case there is any ambiguity. If the property doesn't exist,
                // fail silently.
                try {
                    modelProp = modelType.GetProperty(column.AspectName);

                    var attrs = modelProp?.GetCustomAttributes(typeof(TableViewModelColumnAttribute), true) ?? [];
                    if (attrs != null && attrs.Length == 1)
                        propAttr = ((TableViewModelColumnAttribute) attrs[0]).Column;
                }
                catch {
                }
            }

            // If there's no property, let's not bother showing anything.
            if (modelStruct == null)
                return null;

            // We can build a tooltip. Make a list of lines to add, starting with the property name.
            var fieldName = (modelStruct != null ? $"{modelStruct.Name}." : "") + (propAttr != null ? propAttr.DisplayName ?? propName : propName) + "";
            var lines = new List<string>() { fieldName };

            if (isMultiRow && modelStruct != null) {
                lines.Add("Row Info:");
                lines.Add($"  ID: 0x{modelStruct.ID:X2}");
                lines.Add($"  File Address: 0x{modelStruct.Address:X4}");
            }

            // Add addresses.
            if (modelProp != null) {
                if (propAttr?.AddressField != null) {
                    var field = modelType.GetField(propAttr.AddressField, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
                    if (field != null) {
                        var valueObj = field.GetValue(modelObject);
                        if (valueObj != null) {
                            var value = Convert.ToInt32(valueObj);
                            lines.Add("Property Info:");
                            if (isMultiRow && modelStruct != null)
                                lines.Add($"  Offset in Row: {CommonLib.Utils.ValueUtils.SignedHexStr(value - modelStruct.Address, "X2")}");
                            lines.Add($"  File Address: 0x{value:X4}");
                        }
                    }
                }
            }

            return string.Join("\n", lines);
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

        /// <summary>
        /// Adds some extra functionality to the Control created when editing an ObjectListView cell.
        /// </summary>
        /// <param name="e">Arguments from the OlvCellEditControl event.</param>
        private void EnhanceOlvCellEditControl(CellEditEventArgs e) {
            // Enhance ComboBox's so values are updated any time the dropdown is closed, unless from hitting "escape".
            if (e.Control is ComboBox) {
                var cb = e.Control as ComboBox;
                cb.KeyDown += (s, e2) => {
                    if (e2.KeyCode == Keys.Escape)
                        cb.SelectedValue = e.Column.GetValue(e.RowObject);
                };
                cb.DropDownClosed += (s, e2) => {
                    e.Column.PutValue(e.RowObject, cb.SelectedValue);
                    RefreshItem(e.ListViewItem);
                };

                // Auto-expand the ComboBox when opened.
                // This needs to happen at a specific point: when the data is populated, and the correct item is selected.
                // Normally we can wait for a SelectedIndexChanged event, but if the selected index is changed to 0,
                // no change took place and the event will not trigger. In that case, just trigger on GotFocus.
                var eValueIsNonZero = (e.Value is bool eBool) ? eBool : (e.Value as int? != 0);
                if (eValueIsNonZero) {
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
                control.Font = EnhancedOLVRenderer.GetCellFont(e.Column.AspectToStringFormat ?? "");

                // Ensure that strings displayed in hex format are edited in hex format.
                if (e.Column.AspectToStringFormat?.StartsWith("{0:X") == true)
                    control.Hexadecimal = true;
            }
        }

        private void ForAllHeaderStyles(Action<HeaderFormatStyle, OLVColumn> func) {
            func(HeaderFormatStyle, null);
            foreach (var lvc in AllColumns)
                if (lvc.HeaderFormatStyle != null)
                    func(lvc.HeaderFormatStyle, lvc);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NameGetterContextFetcherHandler NameGetterContextFetcher { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public INameGetterContext NameGetterContext {
            get => NameGetterContextFetcher?.Invoke();
            set => NameGetterContextFetcher = (value == null) ? null : (() => value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color AlternateRowBackColor2 { get; set; } = Color.Empty;

        private Color _readOnlyFieldColor = Color.FromArgb(0x60, 0x60, 0x60);
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ReadOnlyFieldColor {
            get => _readOnlyFieldColor;
            set {
                if (_readOnlyFieldColor != value) {
                    _readOnlyFieldColor = value;
                    ForAllHeaderStyles((style, lvc) => style.SetForeColor((lvc?.IsEditable ?? true) ? ForeColor : value));
                }
            }
        }

        private Color _headerBackgroundColor = Color.Empty;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HeaderBackgroundColor {
            get => _headerBackgroundColor;
            set {
                if (_headerBackgroundColor != value) {
                    _headerBackgroundColor = value;
                    ForAllHeaderStyles((style, column) => style.Normal.BackColor = value);
                }
            }
        }

        private Color _headerHoverColor = Color.Empty;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HeaderHoverColor {
            get => _headerHoverColor;
            set {
                if (_headerHoverColor != value) {
                    _headerHoverColor = value;
                    ForAllHeaderStyles((style, column) => style.Hot.BackColor = value);
                }
            }
        }

        private Color _headerSelectedColor = Color.Empty;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HeaderSelectedColor {
            get => _headerSelectedColor;
            set {
                if (_headerSelectedColor != value) {
                    _headerSelectedColor = value;
                    ForAllHeaderStyles((style, column) => style.Pressed.BackColor = value);
                }
            }
        }

        public Color OriginalAltLineColor1 { get; private set; }
        public Color OriginalAltLineColor2 { get; private set; }
        public Color OriginalReadOnlyFieldColor { get; private set; }
        public Color OriginalHighlightBackgroundColor { get; private set; }
        public Color OriginalHighlightForegroundColor { get; private set; }
        public Color OriginalUnfocusedHighlightBackgroundColor { get; private set; }
        public Color OriginalUnfocusedHighlightForegroundColor { get; private set; }
        public Color OriginalHeaderBackColor { get; private set; }
        public Color OriginalHeaderSelectedColor { get; private set; }
        public Color OriginalHeaderHoverColor { get; private set; }

        private bool _wasVisible = true;
        private Timer _timer = new Timer();
        private DarkModeControlContext<EnhancedObjectListView> DarkModeContext { get; set; }
    }
}
