using System.Collections.Generic;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Files.X8PC {
    public interface IPCAttackAnimBoneKeyframesTableContainer {
        IReadOnlyList<PCBoneKeyframesTable> AttackAnimBoneKeyframesTables { get; }
    }
}
