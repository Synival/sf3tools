using SF3.Models.Tables.Shared;

namespace SF3.Models.Files.X027 {
    public interface IX027_File : IScenarioTableFile {
        BlacksmithTable[] BlacksmithTables { get; }
    }
}
