using SF3.Models.Tables.KAO;

namespace SF3.Models.Files.KAO {
    public interface IKAO_File : IGameTableFile {
        FaceChunkTable FaceChunkTable { get; }
    }
}
