using SF3.Models.Files.X8PC;
using SF3.Models.Tables.X8AN;

namespace SF3.Models.Files.X8AN {
    public interface IX8AN_File : IScenarioTableFile, IPCAttackAnimBoneKeyframesTableContainer {
        X8ANAttackAnimChunkTable AttackAnimChunkTable { get; }
    }
}
