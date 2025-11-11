using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.Utils;
using CommonLib.ViewModels;
using SF3.Models.Structs;
using SF3.Win.Extensions;
using SF3.Win.Views;

namespace SF3.Win.Controls {
    /// <summary>
    /// This exists because of many layers of stupid.
    /// In short, Winforms provides no way to know when a Control is no longer visible because it's parent (like a tab) has changed.
    /// The least-worst hack is to continuously check the "Visible" property to know if this is actually visible or not.
    /// The alternative is to set up an elaborate system of events to *hopefully* inform child controls property.
    /// Just don't make 1,000 of these things at once, okay?
    /// </summary>
    public class EnhancedObjectListView : ObjectListView {
        private static readonly Color _headerBackColor = Color.FromArgb(244, 244, 244);
        private static Dictionary<string, Stack<EnhancedObjectListView>> _cachedOLVControls = new Dictionary<string, Stack<EnhancedObjectListView>>();

        public EnhancedObjectListView(INameGetterContext nameGetterContext) : this(() => nameGetterContext) {}

        public EnhancedObjectListView(NameGetterContextFetcherHandler nameGetterContextFetcher) {
            SuspendLayout();

            CellToolTipGetter = GetCellTooltip;
            HighlightBackgroundColor = SystemColors.Highlight;
            UnfocusedHighlightBackgroundColor = Utils.MathHelpers.Lerp(HighlightBackgroundColor, SystemColors.Window, 0.50f);

            HeaderUsesThemes = false;
            HeaderFormatStyle = new HeaderFormatStyle();
            var olvStyle = HeaderFormatStyle;
            olvStyle.SetFont(DefaultFont);
            olvStyle.Normal.BackColor = _headerBackColor;

            NameGetterContextFetcher = nameGetterContextFetcher;
            OwnerDraw = true;
            DefaultRenderer = EnhancedOLVRenderer.Instance;
            CellEditStarting += (s, e) => this.EnhanceOlvCellEditControl(e);

            _timer.Interval = 100;
            _timer.Tick += CheckForVisibility;
            _timer.Start();

            ResumeLayout();
        }

        public delegate INameGetterContext NameGetterContextFetcherHandler();

        public static void PushCachedOLV(string key, EnhancedObjectListView olv) {
            if (!_cachedOLVControls.ContainsKey(key))
                _cachedOLVControls.Add(key, new Stack<EnhancedObjectListView>());
            _cachedOLVControls[key].Push(olv);
        }

        public static EnhancedObjectListView PopCachedOLV(string key) {
            if (!_cachedOLVControls.ContainsKey(key))
                return null;
            var stack = _cachedOLVControls[key];
            if (stack.Count == 0)
                return null;

            var olv = stack.Pop();
            olv.Show();
            return olv;
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
                                lines.Add($"  Offset in Row: {ValueUtils.SignedHexStr(value - modelStruct.Address, "X2")}");
                            lines.Add($"  File Address: 0x{value:X4}");
                        }
                    }
                }
            }

            return string.Join("\n", lines);
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

        /// <summary>
        /// Runs RefreshItem() on all OLVListItem's in the property Items.
        /// </summary>
        /// <param name="olv">The ObjectListView to refresh.</param>
        public void RefreshAllItems() {
            foreach (var item in Items)
                RefreshItem(item as OLVListItem);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NameGetterContextFetcherHandler NameGetterContextFetcher { get; set; }

        public INameGetterContext NameGetterContext {
            get => NameGetterContextFetcher?.Invoke();
            set => NameGetterContextFetcher = (value == null) ? null : (() => value);
        }

        private bool _wasVisible = true;
        private Timer _timer = new Timer();
    }
}
