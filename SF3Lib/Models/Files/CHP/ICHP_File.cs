using System.Collections.Generic;
using SF3.Models.Files.CHR;

namespace SF3.Models.Files.CHP {
    public interface ICHP_File : IScenarioTableFile {
        Dictionary<int, ICHR_File> CHR_EntriesByOffset { get; }
    }
}
