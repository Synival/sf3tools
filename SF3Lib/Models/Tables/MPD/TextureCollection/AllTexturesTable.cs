using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD.TextureChunk;

namespace SF3.Models.Tables.MPD.TextureCollection {
    public class AllTexturesTable : Table<TextureModel> {
        // TODO: We need a kind of "non-addressed" table here that doesn't have its own data or address!
        //       Using base(null, 0) is a horrible hack :( :( :(
        protected AllTexturesTable(IEnumerable<TextureTable> textureTables) : base(null, 0) {
            Textures = textureTables.SelectMany(x => x.Rows).OrderBy(x => x.ID).ToArray();
        }

        public static AllTexturesTable Create(IEnumerable<TextureTable> textureTables) {
            var newTable = new AllTexturesTable(textureTables);
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
