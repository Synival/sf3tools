using System.Collections.Generic;
using System.Linq;
using SF3.Models.Files.MPD;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public class MPD_Collisions : IMPD_Collisions {
        public MPD_Collisions(IMPD_File mpdFile) {
            MPD_File = mpdFile;

            var modelsChunk = mpdFile.ModelCollections != null
                ? (ModelChunk) mpdFile.ModelCollections.Values.FirstOrDefault(x => x.Collection == Types.CollectionType.Primary)
                : null;

            Lines = modelsChunk?.CollisionLineTable != null
                ? modelsChunk.CollisionLineTable.Select(x => new MPD_CollisionLine(x, modelsChunk.CollisionPointTable)).ToArray()
                : null;
        }

        public IEnumerable<IMPD_CollisionLine> Lines { get; }

        public IMPD_File MPD_File { get; }
    }
}
