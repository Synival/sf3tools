using System.Windows.Forms;
using SF3.Models.Files.DAT;
using SF3.Models.Structs.DAT;

namespace SF3.Win.Views.DAT {
    public class DAT_View : TabView {
        public DAT_View(string name, IDAT_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (Model.TextureTable != null)
                CreateChild(new TextureTableView<TextureModelBase>("Textures", Model.NameGetterContext, Model.TextureTable, Model.TextureViewerScale));

            return Control;
        }

        public IDAT_File Model { get; }
    }
}
