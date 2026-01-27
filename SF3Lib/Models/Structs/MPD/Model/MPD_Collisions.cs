using System.Collections.Generic;
using System.Linq;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;

namespace SF3.Models.Structs.MPD.Model {
    public class MPD_Collisions : IMPD_Collisions {
        public MPD_Collisions(IMPD_File mpdFile) {
            MPD_File = mpdFile;

            var modelsChunk = mpdFile.ModelCollections != null
                ? (ModelChunk) mpdFile.ModelCollections.Values.FirstOrDefault(x => x.Collection == Types.MPD_CollectionType.Primary)
                : null;

            Points = (IEnumerable<IMPD_CollisionPoint>) (modelsChunk?.CollisionPointTable) ?? new IMPD_CollisionPoint[0];
            Lines  = (IEnumerable<IMPD_CollisionLine>)  (modelsChunk?.CollisionLineTable)  ?? new IMPD_CollisionLine[0];
        }

        public IEnumerable<IMPD_CollisionPoint> Points { get; }
        public IEnumerable<IMPD_CollisionLine> Lines { get; }

        public IMPD_File MPD_File { get; }
    }
}
