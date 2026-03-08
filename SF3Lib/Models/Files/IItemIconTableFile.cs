using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Models.Files {
    public interface IItemIconTableFile {
        ScenarioType Scenario { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
