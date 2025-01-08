using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class TexturesView : TabView {
        public TexturesView(string name, IMPD_File model) : base(name) {
            var allTables = model.TextureCollections.Where(x => x != null).Select(x => x.TextureTable).ToList();
            AllTexturesTable = AllTexturesTable.Create(allTables);
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

            return Control;
        }

        public IMPD_File Model { get; }
        public AllTexturesTable AllTexturesTable { get; }
    }
}
