using SF3.Models.Tables;
using SF3.Models.Tables.X012;

namespace SF3.Models.Files.X012 {
    public interface IX012_File : IScenarioTableFile {
        ClassTargetPriorityTable[] ClassTargetPriorityTables { get; }
        UnknownUInt8Table[] UnknownPriorityTables { get; }
    }
}
