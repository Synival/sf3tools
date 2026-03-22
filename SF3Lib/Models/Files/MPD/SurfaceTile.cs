using System;
using CommonLib.Types;
using SF3.MPD;
using SF3.MPD.Interfaces;
using static CommonLib.Utils.BlockHelpers;
using static SF3.Utils.SurfaceUtils;

namespace SF3.Models.Files.MPD {
    public partial class SurfaceTile : IMPD_SurfaceTile {
        public SurfaceTile(IMPD_File file, int x, int y) {
            MPD_File = file;
            X = x;
            Y = y;

            BlockLocation = GetTileBlockLocation(x, y);

            for (int c = 0; c < 4; ++c) {
                _sharedTileLocations[c] = GetSharedTilesAtCorner(X, Y, (CornerType) c);
                _blockVertexLocations[c] = GetVertexBlockLocations(X, Y, (CornerType) c, onlyInBlock: true)[0];
                _sharedBlockVertexLocations[c] = GetVertexBlockLocations(X, Y, (CornerType) c, onlyInBlock: false);
            }

            RandomSeed = MPD_TileSeeds.GetTileSeed(x, y);
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; private set; }
        public BlockTileLocation BlockLocation { get; }

        public event EventHandler Modified;

        private readonly TileAndCorner[][] _sharedTileLocations = new TileAndCorner[4][];
        private readonly BlockVertexLocation[] _blockVertexLocations = new BlockVertexLocation[4];
        private readonly BlockVertexLocation[][] _sharedBlockVertexLocations = new BlockVertexLocation[4][];
    }
}
