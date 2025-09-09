using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllPDatasTable : Table<PDataModel> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllPDatasTable(string name, IEnumerable<PDataTable> modelTables) : base(null, name, 0) {
            Models = modelTables
                .SelectMany(x => x)
                .Where(x => x.Index == 0)
                .OrderBy(x => x.ID).ToArray();
        }

        public static AllPDatasTable Create(string name, IEnumerable<PDataTable> modelTables)
            => CreateBase(() => new AllPDatasTable(name, modelTables));

        public override bool Load() {
            _rows = Models.OrderBy(x => x.Collection).ThenBy(x => x.ID).ToArray();
            return true;
        }

        public PDataModel[] Models { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
