using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class MainTablesView : TabView {
        public MainTablesView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Header", Model.MPDHeader, ngc));

            if (Model.LightPalette != null)
                CreateChild(new ColorTableView("Light Palette", Model.LightPalette, Model.NameGetterContext));

            if (Model.LightPositionTable != null)
                CreateChild(new TableView("Light Position", Model.LightPositionTable, ngc));

            for (var i = 0; i < Model.TexturePalettes.Length; i++)
                if (Model.TexturePalettes[i] != null)
                    CreateChild(new ColorTableView("Texture Palette " + (i + 1).ToString(), Model.TexturePalettes[i], Model.NameGetterContext));

            if (Model.Offset3Table != null)
                CreateChild(new TableView("Offset 3 Table",  Model.Offset3Table, ngc));

            if (Model.Offset4Table != null)
                CreateChild(new TableView("Offset 4 Table",  Model.Offset4Table, ngc));

            if (Model.TextureAnimations != null)
                CreateChild(new TextureAnimationsView("Texture Animations", Model.TextureAnimations, ngc));

            if (Model.TextureAnimationsAlt != null)
                CreateChild(new TableView("Texture Animations (Alt)", Model.TextureAnimationsAlt, ngc));

            if (Model.Offset7Table != null)
                CreateChild(new TableView("Offset 7 Table",  Model.Offset7Table, ngc));

            if (Model.BoundariesTable != null)
                CreateChild(new TableView("Boundaries", Model.BoundariesTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
