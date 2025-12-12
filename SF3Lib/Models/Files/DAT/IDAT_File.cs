using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public interface IDAT_File : IScenarioTableFile {
        DAT_FileType FileType { get; }
        Table<TextureStructBase> TextureTable { get; }
        int TextureViewerScale { get; }
    }
}
