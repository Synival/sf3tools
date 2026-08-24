using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllMPD_ModelInstancesTable : Table<MPD_ModelInstance> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllMPD_ModelInstancesTable(string name, IEnumerable<MPD_ModelInstanceTable> modelTables) : base(null, name, 0) {
            ModelInstances = modelTables
                .Where(x => x != null)
                .SelectMany(x => x.Rows)
                .OrderBy(x => x.Collection.Collection)
                .OrderBy(x => x.ID)
                .ToArray();
        }

        public static AllMPD_ModelInstancesTable Create(string name, IEnumerable<MPD_ModelInstanceTable> modelTables)
            => Create(() => new AllMPD_ModelInstancesTable(name, modelTables));

        public override bool Load() {
            _rows = ModelInstances.OrderBy(x => x.Collection.Collection).ThenBy(x => x.ID).ToArray();
            return true;
        }

        public Structs.MPD.Model.MPD_ModelInstance[] ModelInstances { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
