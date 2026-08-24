using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllMPD_PDatasTable : Table<MPD_PDataStruct> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllMPD_PDatasTable(string name, IEnumerable<MPD_PDataTable> modelTables) : base(null, name, 0) {
            Models = modelTables
                .SelectMany(x => x)
                .Where(x => x.LevelOfDetail == 0)
                .OrderBy(x => x.ID).ToArray();
        }

        public static AllMPD_PDatasTable Create(string name, IEnumerable<MPD_PDataTable> modelTables)
            => Create(() => new AllMPD_PDatasTable(name, modelTables));

        public override bool Load() {
            _rows = Models.OrderBy(x => x.Collection).ThenBy(x => x.ID).ToArray();
            return true;
        }

        public MPD_PDataStruct[] Models { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
