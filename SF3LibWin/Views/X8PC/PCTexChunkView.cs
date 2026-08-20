using System.Windows.Forms;
using SF3.Models.Files.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCTexChunkView : TabView {
        public PCTexChunkView(string name, IX8PC_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model?.TexDefChunkHeader != null)
                CreateChild(new DataModelView("Header", Model.TexDefChunkHeader, ngc));
            if (Model?.TextureTable != null)
                CreateChild(new PCTextureTableView("Textures", Model.TextureTable, ngc));

            return Control;
        }

        public IX8PC_File Model { get; }
    }
}
