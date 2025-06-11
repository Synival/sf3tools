using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X023 {
    public interface IX023_File : IScenarioTableFile {
        BlacksmithTable BlacksmithTable { get; }
    }
}
