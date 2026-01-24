using System;
using SF3.Models.Files.KAO;
using SF3.Models.Structs.KAO;

namespace SF3.Win.Views.KAO {
    public class KAO_View : ArrayView<FaceChunk, FaceChunkView> {
        public KAO_View(string name, IKAO_File file) : base(
            name, file?.FaceChunkTable?.Rows, nameof(FaceChunk.Name),
            new FaceChunkView("Faces", null, file.NameGetterContext)
        ) {
            File = file;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedChunk = (FaceChunk) DropdownList.SelectedValue;
            ElementView.Chunk = selectedChunk;
        }

        private IKAO_File _file = null;
        public IKAO_File File {
            get => _file;
            set {
                if (_file != value) {
                    _file = value;
                    Elements = value?.FaceChunkTable?.Rows;
                }
            }
        }
    }
}
