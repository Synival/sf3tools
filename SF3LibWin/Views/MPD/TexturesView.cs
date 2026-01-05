using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class TexturesView : TabView {
        public TexturesView(string name, IMPD_File model) : base(name) {
            var allTables = (model.TextureChunks == null)
                ? new List<TextureTable>()
                : model.TextureChunks.Where(x => x != null).Select(x => x.TextureTable).ToList();
            AllTexturesTable = AllTexturesTable.Create("AllTextures", allTables);
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TextureTableView("Textures", AllTexturesTable, ngc));
            if (Model.Animations != null) {
                CreateChild(new AnimationTableView("Animations", Model.Animations, ngc));
                CreateChild(new AnimationFramesView("Animation Frames", Model, ngc));
            }

            if (Model.TexturePaletteColorTable != null)
                CreateChild(new ColorTableView("Texture Palette", Model.TexturePaletteColorTable, Model.NameGetterContext));

            return Control;
        }

        public IMPD_File Model { get; }
        public AllTexturesTable AllTexturesTable { get; }
    }
}
