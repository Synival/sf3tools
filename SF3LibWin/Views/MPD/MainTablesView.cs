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

            if (Model.OffsetUnknown1Table != null)
                CreateChild(new TableView("Unknown 1",  Model.OffsetUnknown1Table, ngc));

            if (Model.OffsetModelSwitchGroupsTable != null)
                CreateChild(new TableView("Model Switch Groups",  Model.OffsetModelSwitchGroupsTable, ngc));

            if (Model.TextureAnimations != null)
                CreateChild(new TextureAnimationsView("Texture Animations", Model.TextureAnimations, ngc));

            for (var i = 0; i < Model.PaletteTables.Length; i++)
                if (Model.PaletteTables[i] != null)
                    CreateChild(new ColorTableView("Texture Palette " + (i + 1).ToString(), Model.PaletteTables[i], Model.NameGetterContext));

            if (Model.IndexedTextureTable != null)
                CreateChild(new TableView("Indexed Textures",  Model.IndexedTextureTable, ngc));

            if (Model.TextureAnimationsAlt != null)
                CreateChild(new TableView("Texture Animations (Alt)", Model.TextureAnimationsAlt, ngc));

            if (Model.OffsetScrollScreenAnimationTable != null)
                CreateChild(new TableView("Scroll Screen Animation",  Model.OffsetScrollScreenAnimationTable, ngc));

            if (Model.BoundariesTable != null)
                CreateChild(new TableView("Boundaries", Model.BoundariesTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
