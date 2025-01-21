using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD.TextureChunk;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllTexturesTable : Table<TextureModel> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllTexturesTable(string name, IEnumerable<TextureTable> textureTables) : base(null, name, 0) {
            Textures = textureTables.SelectMany(x => x).OrderBy(x => x.ID).ToArray();
        }

        public static AllTexturesTable Create(string name, IEnumerable<TextureTable> textureTables) {
            var newTable = new AllTexturesTable(name, textureTables);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            _rows = Textures;
            return true;
        }

        public TextureModel[] Textures { get; }
    }
}
