using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class TilePropertiesControl : UserControl {
        public TilePropertiesControl() {
            InitializeComponent();
            UpdateControls();
        }

        private void UpdateControls() {
            if (_tile == null) {
                labelTileEdited.Text = "Tile: (no tile)";
                labelRealCoordinates.Text = "";
            }
            else {
                labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";
            }
        }

        public Tile _tile = null;

        public Tile Tile {
            get => _tile;
            set {
                if (value == _tile)
                    return;

                _tile = value;
                UpdateControls();
            }
        }
    }
}
