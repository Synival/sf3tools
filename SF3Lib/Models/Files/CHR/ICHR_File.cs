using SF3.Models.Tables.CHR;

namespace SF3.Models.Files.CHR {
    public interface ICHR_File : IScenarioTableFile {
        bool IsCHP { get; }
        SpriteTable SpriteTable { get; }
    }
}
