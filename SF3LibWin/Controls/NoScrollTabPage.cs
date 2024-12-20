using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public class NoScrollTabPage : TabPage {
        public NoScrollTabPage(string text) : base(text) { }
        protected override Point ScrollToControl(Control control) => DisplayRectangle.Location;
    }
}
