using System.Collections.Generic;
using SF3.CHR;
using SF3.Models.Files.CHR;

namespace SF3.Models.Files.CHP {
    public interface ICHP_File : IScenarioTableFile {
        CHP_Def ToCHP_Def();

        Dictionary<int, ICHR_File> CHR_EntriesByOffset { get; }
    }
}
