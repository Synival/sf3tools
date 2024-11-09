using SF3.FileEditors;
using SF3.Models.MPD.TextureChunk;

namespace SF3.Tables.MPD.TextureChunk {
    public class TextureTable : Table<Texture> {
        public TextureTable(IByteEditor fileEditor, int address, int textureCount) : base(fileEditor, address) {
            MaxSize = textureCount;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new Texture(FileEditor, id, "Header", address));

        public override int? MaxSize { get; }
    }
}
