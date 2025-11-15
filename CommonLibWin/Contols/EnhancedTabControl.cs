using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;
using CommonLib.Win.Utils;

namespace CommonLib.Win.Controls {
    /// <summary>
    /// TabControl with enhanced drawing.
    /// </summary>
    public class EnhancedTabControl : TabControl {
        public EnhancedTabControl() : this(TabAlignment.Top) {}

        public EnhancedTabControl(TabAlignment tabAlignment) {
            SuspendLayout();

            Alignment = tabAlignment;
            if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right) {
                SizeMode = TabSizeMode.Fixed;
                ItemSize = new Size(40, 100);
                _needsDisplayRectangleFix = true;
            }
            DrawMode = TabDrawMode.OwnerDrawFixed;
            DrawItem += EnhancedDraw;

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            ResumeLayout();
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                OriginalHighlightedBackColor = HighlightedBackColor;
                OriginalHighlightedForeColor = HighlightedForeColor;

                DarkModeContext = new DarkModeControlContext<EnhancedTabControl>(this);
                DarkModeContext.EnabledChanged += (s, e) => {
                    if (DarkModeContext.Enabled) {
                        CustomPaint = true;
                        HighlightedBackColor = DarkModeColors.HighlightedBackColor;
                        HighlightedForeColor = DarkModeColors.HighlightedForeColor;
                    }
                    else {
                        CustomPaint = false;
                        HighlightedBackColor = OriginalHighlightedBackColor;
                        HighlightedForeColor = OriginalHighlightedForeColor;
                    }
                };
                DarkModeContext.Init();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (CustomPaint) {
                for (int i = 0; i < TabCount; i++) {
                    var bounds = GetTabRect(i);
                    if (!bounds.IntersectsWith(e.ClipRectangle))
                        continue;

                    var drawState = DrawItemState.None;
                    if (SelectedIndex == i)
                        drawState |= DrawItemState.Selected;
                    if (Focused)
                        drawState |= DrawItemState.Focus;

                    EnhancedDraw(this, new DrawItemEventArgs(e.Graphics, Font, e.ClipRectangle, i, drawState));
                }
            }
        }

        private void EnhancedDraw(object sender, DrawItemEventArgs e) {
            var tabPage = TabPages[e.Index];

            Color backColor = BackColor;
            if (e.State.HasFlag(DrawItemState.Selected)) {
                backColor = !e.State.HasFlag(DrawItemState.Focus)
                    ? MathHelpers.Lerp(HighlightedBackColor, BackColor, 0.50f)
                    : HighlightedBackColor;
            }

            var bounds = GetTabRect(e.Index);
 
            StringFormat stringFlags = new StringFormat {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Draw the background, the text, and a focus rectangle.
            // TODO: there is quite a lot of flickering here that needs to be removed!!
            using (var backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, bounds);

            var font = (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right) ? new Font(e.Font.FontFamily, e.Font.Size * 0.85f, e.Font.Style) : e.Font;
            using (var foreBrush = new SolidBrush(e.State.HasFlag(DrawItemState.Selected) ? HighlightedForeColor : tabPage.ForeColor))
                e.Graphics.DrawString(tabPage.Text, font, foreBrush, bounds, stringFlags);
            if (!_customPaint && e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }

        protected override void OnLayout(LayoutEventArgs levent) {
            // This dumb hack is necessary to update the DisplayRectangle after an unknown trigger that fixes the
            // positioning of the tabs. There's probably a better way to do this, but I have not found it...
            if (_needsDisplayRectangleFix && Alignment == TabAlignment.Left && DisplayRectangle.X > ItemSize.Height + 4) {
                var oldDisplayRectangleX = DisplayRectangle.X;
                ClientSize = new Size(ClientSize.Width + 1, Height);
                ClientSize = new Size(ClientSize.Width - 1, Height);
                if (oldDisplayRectangleX != DisplayRectangle.X)
                    _needsDisplayRectangleFix = false;
            }

            base.OnLayout(levent);
        }

        private Color _backColor = SystemColors.Control;
        /// <summary>
        /// Background color for unhighlighted tabs and, if CustomPaint is 'true', all other empty space.
        /// </summary>
        public override Color BackColor {
            get => _backColor;
            set {
                if (_backColor != value) {
                    _backColor = value;
                    Invalidate();
                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }

        private Color _foreColor = SystemColors.ControlText;
        /// <summary>
        /// Text color.
        /// </summary>
        public override Color ForeColor {
            get => _foreColor;
            set {
                if (_foreColor != value) {
                    _foreColor = value;
                    Invalidate();
                    OnForeColorChanged(EventArgs.Empty);
                }
            }
        }

        private Color _highlightedBackColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
        /// <summary>
        /// Back color used for the highlighted menu item.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HighlightedBackColor {
            get => _highlightedBackColor;
            set {
                _highlightedBackColor = value;
                Invalidate();
            }
        }

        private Color _highlightedForeColor = SystemColors.ControlText;
        /// <summary>
        /// Fore/text color used for the highlighted menu item.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HighlightedForeColor {
            get => _highlightedForeColor;
            set {
                _highlightedForeColor = value;
                Invalidate();
            }
        }

        private bool _customPaint = false;
        /// <summary>
        /// When enabled, a custom managed paint method should be used.
        /// </summary>
        public bool CustomPaint {
            get =>_customPaint;
            set {
                if (_customPaint != value) {
                    _customPaint = value;
                    SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, value);
                    Invalidate();
                }
            }
        }

        public Color OriginalHighlightedBackColor { get; set; }
        public Color OriginalHighlightedForeColor { get; set; }

        private bool _needsDisplayRectangleFix = false;
        private DarkModeControlContext<EnhancedTabControl> DarkModeContext { get; set; }
    }
}
