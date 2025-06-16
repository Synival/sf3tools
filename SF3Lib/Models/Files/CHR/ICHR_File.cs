using System.Collections.Generic;
using SF3.Models.Tables.CHR;

namespace SF3.Models.Files.CHR {
    public interface ICHR_File : IScenarioTableFile {
        bool IsCHP { get; }
        SpriteTable SpriteTable { get; }
        SpriteOffset1SetTable SpriteOffset1SetTable { get; }
        Dictionary<int, SpriteFrameTable> SpriteFrameTablesByFileAddr { get; }
        SpriteOffset2SetTable SpriteOffset2SetTable { get; }
        Dictionary<int, SpriteOffset2SubTable> SpriteOffset2SubTablesByFileAddr { get; }
    }
}
