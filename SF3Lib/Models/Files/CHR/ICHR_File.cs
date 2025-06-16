using System.Collections.Generic;
using SF3.Models.Tables.CHR;

namespace SF3.Models.Files.CHR {
    public interface ICHR_File : IScenarioTableFile {
        bool IsCHP { get; }
        SpriteTable SpriteTable { get; }
        FrameDataOffsetsTable FrameDataOffsetsTable { get; }
        Dictionary<int, FrameTable> FrameTablesByFileAddr { get; }
        AnimationOffsetsTable AnimationOffsetsTable { get; }
        Dictionary<int, AnimationFrameTable> AnimationFrameTablesByAddr { get; }
    }
}
