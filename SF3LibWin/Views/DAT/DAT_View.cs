using System.Windows.Forms;
using CommonLib.Win.Utils;
using SF3.Models.Files.DAT;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Win.App;

namespace SF3.Win.Views.DAT {
    public class DAT_View : TabView {
        public DAT_View(string name, IDAT_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            if (Model.TextureTable != null) {
                var textureView = new TextureView("Spritesheet", Model.Spritesheet, 1);

                textureView.ImagePreImport += (s, e) =>
                    Model.Spritesheet.MinimalChangesWhenSetting = AppState.Get().MinimalChangesWhenImportingSpritesheets;

                textureView.ImageImported += (s, e) => {
                    // TODO: There's a more elegant want to do this for sure!
                    if (Model is ITEM_CG_File) {
                        MessageUtils.InfoMessage(
                            "Contents successfully replaced.\r\n\r\n" +
                            "Please update the icon offset tables in X011.BIN, X021.BIN, X026.BIN, and X032.BIN.\r\n\r\n" +
                            "This can be done easily by opening each file and performing the menu action:\r\n\r\n" +
                            "    Icon Offsets -> Assign from Active Icons"
                        );
                    }
                };
                CreateChild(textureView);
            }

            if (Model.TextureTable != null) {
                CreateChild(new TextureDataTableView<DAT_FileTextureBase, Table<DAT_FileTextureBase>>(
                    "Textures", Model.TextureTable, Model.NameGetterContext, Model.TextureViewerScale));
            }

            return Control;
        }

        public IDAT_File Model { get; }
    }
}
