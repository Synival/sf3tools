using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Tables.MPD;

namespace SF3.Win.Views {
    public class ColorTableView : ControlSpaceView {
        public ColorTableView(string name, ColorTable table, INameGetterContext nameGetterContext) : base(name) {
            Table     = table;
            TableView = new TableView("Table", table, nameGetterContext);
            PaletteView = new PaletteView("Texture");

            PaletteView.ImportPalette += (s, colors) => {
                int max = Math.Min(Table.Length, colors.Length);
                for (int i = 0; i < max; i++)
                    Table[i].ColorABGR1555 = colors[i];
                UpdateBitmap();
            };
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            CreateChild(TableView, (c) => {
                var olv = (ObjectListView) c;
                olv.CellEditFinishing += (s, e) => {
                    if (!e.Cancel) {
                        e.Control.LostFocus += (s, e) => UpdateBitmap();
                    }
                };
            });

            CreateChild(PaletteView, (c) => {
                c.Dock = DockStyle.Right;
                UpdateBitmap();
            }, false);

            return control;
        }

        private void UpdateBitmap() {
            var colors = Table.Select(x => x.ColorABGR1555).ToArray();
            PaletteView.SetColors(colors);
        }

        public readonly ColorTable Table = null;
        public readonly TableView TableView = null;
        public readonly PaletteView PaletteView = null;

        public Bitmap PaletteBitmap => PaletteView.PaletteBitmap;

    }
}
