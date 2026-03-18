using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    public class DarkModeGroupBox : GroupBox {
        public DarkModeGroupBox() {
            _defaultCursor = Cursor;
        }

        protected override void OnClick(EventArgs e) {
            if (!_isMouseOverTitle) {
                base.OnClick(e);
                return;
            }

            if (!IsCollapsed) {
                _uncollapseHeight = Height;
                Height = Font.Height + 3;
                AdjustLowerControlsBy(Height - _uncollapseHeight);
            }
            else {
                AdjustLowerControlsBy(_uncollapseHeight - Height);
                Height = _uncollapseHeight;
            }

            IsCollapsed ^= true;
        }

        private void AdjustLowerControlsBy(int amount) {
            Parent.SuspendLayout();
            foreach (var controlObj in Parent.Controls) {
                var control = controlObj as Control;
                if (control == this || control == null || control.Parent != Parent || control.Location.Y <= Location.Y)
                    continue;
                control.Location = new Point(control.Location.X, control.Location.Y + amount);
            }
            Parent.ResumeLayout();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseEnter(e);
            _isMouseOverTitle = IsPositionOverTitle(e.Location);
            Cursor = _isMouseOverTitle ? Cursors.Hand : _defaultCursor;
        }

        private bool IsPositionOverTitle(Point point) {
            if (point.Y >= Font.Height)
                return false;
            var textWidth = TextRenderer.MeasureText(Text, Font).Width;
            return (point.X >= 4 && point.X < textWidth);
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeGroupBox>(this);
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = DefaultBackColor;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (!DarkModeContext.Enabled) {
                base.OnPaint(e);
                return;
            }

            using (Brush textBrush = new SolidBrush(Enabled ? ForeColor : DarkModeColors.DisabledColor))
            using (Brush borderBrush = new SolidBrush(DarkModeColors.BorderColor))
            using (Pen borderPen = new Pen(borderBrush)) {
                var g = e.Graphics;
                SizeF strSize = g.MeasureString(Text, Font);
                Rectangle rect = new Rectangle(
                    ClientRectangle.X,
                    ClientRectangle.Y + (int)(strSize.Height / 2),
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - (int)(strSize.Height / 2) - 1
                );

                // Clear text and border
                g.Clear(BackColor);

                // Draw text
                g.DrawString(Text, Font, textBrush, Padding.Left, 0);

                // Drawing Border
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X,  rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X,  rect.Y), new Point(rect.X + Padding.Left, rect.Y));
                g.DrawLine(borderPen, new Point(rect.X + Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCollapsed { get; set; } = false;

        private DarkModeControlContext<DarkModeGroupBox> DarkModeContext { get; set; }

        private Cursor _defaultCursor;
        private bool _isMouseOverTitle;
        private int _uncollapseHeight = 0;
    }
}
