using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Extensions;
using SF3.Types;

namespace SF3.Win.Views.MPD {
    public class MainTablesView : TabView {
        public MainTablesView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new DataModelView("Header", Model.MPDHeader, ngc, displayGroups: ["Metadata", "Main"]));
            CreateChild(new DataModelView("Map Flags", Model.Flags, ngc, displayGroups: ["Flags"]));

            if (Model.LightPaletteColorTable != null)
                CreateChild(new ColorTableView("Light Palette", Model.LightPaletteColorTable, Model.NameGetterContext));

            if (Model.LightPosition != null)
                CreateChild(new DataModelView("Light Position", Model.LightPosition, ngc));

            if (Model.Unknown1Table != null)
                CreateChild(new TableView("Unknown 1", Model.Unknown1Table, ngc));

            if (Model.PaletteAdjustment != null)
                CreateChild(new DataModelView("Palette Adjustment" + (Model.PaletteAdjustment.IsTruncated ? " (Truncated)" : ""), Model.PaletteAdjustment, ngc));

            if (Model.UnreferencedDataAfterPaletteAdjustmentTable != null)
                CreateChild(new TableView("Junk After Palette Adjustment", Model.UnreferencedDataAfterPaletteAdjustmentTable, ngc));

            if (Model.ModelSwitchGroupsTable != null)
                CreateChild(new ModelSwitchGroupTableView("Model Switch Groups", Model, Model.ModelSwitchGroupsTable, ngc));

            if (Model.Animations != null) {
                CreateChild(new AnimationTableView("Animations", Model.Animations, ngc));
                CreateChild(new AnimationFramesView("Animation Frames", Model, ngc));
            }

            if (Model.Unknown2Table != null)
                CreateChild(new TableView("Unknown 2", Model.Unknown2Table, ngc));

            if (Model.Unknown3Table != null)
                CreateChild(new TableView("Unknown 3 (Ship2)", Model.Unknown3Table, ngc));

            if (Model.Unknown4Table != null)
                CreateChild(new TableView("Unknown 4 (Prototype)", Model.Unknown4Table, ngc));

            if (Model.GradientTable != null) {
                if (Model.GradientTable.Length == 1)
                    CreateChild(new DataModelView("Gradient" + (Model.GradientTable[0].IsDummiedOut ? " (Dummied Out)" : ""), Model.GradientTable[0], ngc));
                else
                    CreateChild(new TableView("Gradients", Model.GradientTable, ngc));
            }

            if (Model.GroundAnimationTable != null)
                CreateChild(new TableView("Ground Animation", Model.GroundAnimationTable, ngc));

            if (Model.ModelCollections != null) {
                foreach (var models in Model.ModelCollections.Values) {
                    if (models.IsHeaderModelCollection() && models is ModelChunk fileModels) {
                        var name = (models.Collection == MPD_CollectionType.Chest ? "Chest Models" : models.Collection == MPD_CollectionType.LockedChest ? "Locked Chest Models" : "Barrel Models");
                        if (models.IsUnreferenced)
                            name += " (Unreferenced)";
                        CreateChild(new ModelChunkView(name, Model, fileModels));
                    }
                }
            }

            if (Model.GroundPaletteColorTable != null)
                CreateChild(new ColorTableView("Ground Palette", Model.GroundPaletteColorTable, Model.NameGetterContext));

            if (Model.SkyPaletteColorTable != null)
                CreateChild(new ColorTableView("Sky Palette", Model.SkyPaletteColorTable, Model.NameGetterContext));

            if (Model.TexturePaletteColorTable != null)
                CreateChild(new ColorTableView("Texture Palette", Model.TexturePaletteColorTable, Model.NameGetterContext));

            if (Model.IndexedTextureTable != null)
                CreateChild(new IndexedTextureTableView("Indexed Textures",  Model.IndexedTextureTable, ngc, Model.ModelCollections[MPD_CollectionType.Primary]));

            if (Model.IgnoredTextureTable != null) {
                var name = "Ignored Textures" + (Model.IgnoredTextureTable.IsDummiedOut ? " (Dummied Out)" : "");
                CreateChild(new IgnoredTextureTableView(name, Model.IgnoredTextureTable, ngc, Model.ModelCollections[MPD_CollectionType.Primary]));
            }

            if (Model.BoundariesTable != null)
                CreateChild(new TableView("Boundaries", Model.BoundariesTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
