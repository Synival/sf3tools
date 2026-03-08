using System.Windows.Forms;
using SF3.Models.Files.DAT;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;

namespace SF3.Win.Views.DAT {
    public class DAT_View : TabView {
        public DAT_View(string name, IDAT_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (Model.TextureTable != null)
                CreateChild(new TextureView("Spritesheet", Model.Spritesheet, 1));

            if (Model.TextureTable != null) {
                CreateChild(new TextureDataTableView<DAT_FileTextureBase, Table<DAT_FileTextureBase>>(
                    "Textures", Model.TextureTable, Model.NameGetterContext, Model.TextureViewerScale));
            }

            return Control;
        }

        public IDAT_File Model { get; }
    }
}
