using System;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public VECTOR GetVertexNormal(CornerType corner) {
            if (MPD_File.SurfaceModelChunk?.VertexNormalBlockTable == null)
                return new VECTOR(0f, 1f, 0f);

            var bl = _blockVertexLocations[corner];
            return MPD_File.SurfaceModelChunk.VertexNormalBlockTable[bl.Num][bl.X, bl.Y];
        }

        public VECTOR[] GetVertexNormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexNormal(c)).ToArray();
        }
    }
}
