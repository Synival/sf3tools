using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Files.X8PC {
    public interface IX8PC_File : IScenarioTableFile {
        PCHeader Header { get; }
        PCTexDefChunkHeader TexDefChunkHeader { get; }
        PCTextureTable TextureTable { get; }
    }
}
