using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public interface IDAT_File : IGameTableFile {
        DAT_FileType FileType { get; }
        Table<DAT_FileTextureBase> TextureTable { get; }
        int TextureViewerScale { get; }
        TexturesAsSpritesheet Spritesheet { get; }
    }
}
