using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class TexturesView : TabView {
        public TexturesView(string name, IMPD_File model) : base(name) {
            var allTables = (model.TextureCollections == null)
                ? new List<TextureTable>()
                : model.TextureCollections.Where(x => x != null).Select(x => x.TextureTable).ToList();
            AllTexturesTable = AllTexturesTable.Create("AllTextures", allTables);
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TextureTableView("Textures", AllTexturesTable, ngc));
            if (Model.TextureAnimations != null) {
                CreateChild(new TextureAnimationsView("Animations", Model.TextureAnimations, ngc));
                CreateChild(new TextureAnimFramesView("Animation Frames", Model, ngc));
            }

            var palettes = Model.PaletteTables
                .Select((p, i) => new { Index = i, Palette = p })
                .Where(x => x.Palette != null)
                .GroupBy(x => x.Palette.Address)
                .ToDictionary(x => x.Key, x => x.ToArray());

            foreach (var palette in palettes) {
                var indices = palette.Value.Select(x => (x.Index + 1).ToString()).ToArray();
                var name = ((palette.Value.Length == 1) ? "Palette " : "Palettes ") + string.Join('+', indices);
                CreateChild(new ColorTableView(name, palette.Value[0].Palette, Model.NameGetterContext));
            }

            return Control;
        }

        public IMPD_File Model { get; }
        public AllTexturesTable AllTexturesTable { get; }
    }
}
