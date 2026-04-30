using SF3.Models.Tables.X007;

namespace SF3.Models.Files.X007 {
    public interface IX007_File : IScenarioTableFile {
        CHPSectorSizesTable[] CHPSectorSizesTables { get; }
    }
}
