using System.Windows.Forms;
using SF3.Models.Files.KAO;
using SF3.Models.Structs.KAO;
using SF3.Models.Tables.KAO;

namespace SF3.Win.Views.KAO {
    public class KAO_View : TabView {
        public KAO_View(string name, IKAO_File file) : base(name) {
            File = file;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = File.NameGetterContext;
            CreateChild(new TextureDataTableView<FaceChunk, FaceChunkTable>("Faces", File.FaceChunkTable, ngc));

            return Control;
        }

        public IKAO_File File { get; }
    }
}
