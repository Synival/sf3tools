using System.Collections.Generic;
using System.Linq;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllModelInstancesTable : Table<Structs.MPD.Model.ModelInstance> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllModelInstancesTable(string name, IEnumerable<ModelInstanceTable> modelTables) : base(null, name, 0) {
            ModelInstances = modelTables
                .Where(x => x != null)
                .SelectMany(x => x.Rows)
                .OrderBy(x => x.CollectionType)
                .OrderBy(x => x.ID)
                .ToArray();
        }

        public static AllModelInstancesTable Create(string name, IEnumerable<ModelInstanceTable> modelTables)
            => Create(() => new AllModelInstancesTable(name, modelTables));

        public override bool Load() {
            _rows = ModelInstances.OrderBy(x => x.CollectionType).ThenBy(x => x.ID).ToArray();
            return true;
        }

        public Structs.MPD.Model.ModelInstance[] ModelInstances { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
