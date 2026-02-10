using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public interface IDAT_File : IGameTableFile {
        DAT_FileType FileType { get; }
        Table<FixedSizeTextureStructBase> TextureTable { get; }
        int TextureViewerScale { get; }
    }
}
