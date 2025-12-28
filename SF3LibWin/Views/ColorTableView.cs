using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
    public class ColorTableView : ControlSpaceView {
        public ColorTableView(string name, ColorTable table, INameGetterContext nameGetterContext) : base(name) {
            TableView   = new TableView("Table", table, nameGetterContext, typeof(Models.Structs.Shared.Color));
            PaletteView = new PaletteView("Texture");
            Table       = table;

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
            var colors = Table?.Select(x => x.ColorABGR1555)?.ToArray() ?? [0x0000];
            PaletteView.SetColors(colors);
        }

        public readonly TableView TableView = null;
        public readonly PaletteView PaletteView = null;

        public ColorTable Table {
            get => (ColorTable) TableView.Table;
            set {
                if (TableView.Table != value) {
                    TableView.Table = value;
                    UpdateBitmap();
                }
            }
        }

        public Bitmap PaletteBitmap => PaletteView.PaletteBitmap;
    }
}
