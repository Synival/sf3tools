using System.Windows.Forms;
using SF3.Models.Files.DAT;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;

namespace SF3.Win.Views.DAT {
    public class DAT_View : TabView {
        public DAT_View(string name, IDAT_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (Model.TextureTable != null) {
                CreateChild(new TextureDataTableView<FixedSizeTextureStructBase, Table<FixedSizeTextureStructBase>>(
                    "Textures", Model.TextureTable, Model.NameGetterContext, Model.TextureViewerScale));
            }

            return Control;
        }

        public IDAT_File Model { get; }
    }
}
