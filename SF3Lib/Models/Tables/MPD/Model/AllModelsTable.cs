using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllModelsTable : Table<Structs.MPD.Model.Model> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllModelsTable(string name, IEnumerable<ModelTable> modelTables) : base(null, name, 0) {
            Models = modelTables
                .Where(x => x != null)
                .SelectMany(x => x.Rows)
                .OrderBy(x => x.CollectionType)
                .OrderBy(x => x.ID)
                .ToArray();
        }

        public static AllModelsTable Create(string name, IEnumerable<ModelTable> modelTables) {
            var newTable = new AllModelsTable(name, modelTables);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            _rows = Models.OrderBy(x => x.CollectionType).ThenBy(x => x.ID).ToArray();
            return true;
        }

        public Structs.MPD.Model.Model[] Models { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
