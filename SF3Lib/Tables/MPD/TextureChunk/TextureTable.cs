using SF3.FileEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IByteEditor fileEditor, int address, int textureCount, int startId) : base(fileEditor, address) {
            MaxSize = textureCount;
            StartID = startId;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Texture(FileEditor, StartID + id, "Texture" + (StartID + id), address));

        public override int? MaxSize { get; }

        public int StartID { get; }
    }
}
