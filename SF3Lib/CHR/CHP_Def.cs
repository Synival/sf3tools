using System.Collections.Generic;
using SF3.Models.Files.CHP;

namespace SF3.CHR {
    public class CHP_Def {
        public Dictionary<int, CHR_Def> CHRsByOffset;

        public ICHP_File ToCHP_File() {
            // TODO: deserialize!
            return null;
        }
    }
}
