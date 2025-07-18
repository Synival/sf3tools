using SF3.Models.Tables.CHR;
using SF3.Sprites;

namespace SF3.Models.Files.CHR {
    public interface ICHR_File : IScenarioTableFile {
        CHR_Def ToCHR_Def();

        uint DataOffset { get; }
        SpriteTable SpriteTable { get; }
    }
}
