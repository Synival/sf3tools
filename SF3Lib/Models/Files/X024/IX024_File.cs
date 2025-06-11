using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X024 {
    public interface IX024_File : IScenarioTableFile {
        BlacksmithTable BlacksmithTable { get; }
    }
}
