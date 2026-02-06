using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD;
using SF3.MPD.Interfaces;
using static SF3.Utils.SurfaceUtils;

namespace SF3.Models.Files.MPD {
    public partial class SurfaceTile : IMPD_SurfaceTile {
        public SurfaceTile(IMPD_File file, int x, int y) {
            MPD_File = file;
            X = x;
            Y = y;

            var allCorners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            BlockLocation = BlockHelpers.GetTileBlockLocation(x, y);

            _sharedTileLocations = allCorners
                .ToDictionary(c => c, c => GetSharedTilesAtCorner(X, Y, c));
            _blockVertexLocations = allCorners
                .ToDictionary(c => c, c => BlockHelpers.GetVertexBlockLocations(X, Y, c, onlyInBlock: true)[0]);
            _sharedBlockVertexLocations = allCorners
                .ToDictionary(c => c, c => BlockHelpers.GetVertexBlockLocations(X, Y, c, onlyInBlock: false));

            RandomSeed = MPD_TileSeeds.GetTileSeed(x, y);
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; private set; }
        public BlockHelpers.BlockTileLocation BlockLocation { get; }

        public event EventHandler Modified;

        private Dictionary<CornerType, TileAndCorner[]> _sharedTileLocations { get; }
        private Dictionary<CornerType, BlockHelpers.BlockVertexLocation> _blockVertexLocations { get; }
        private Dictionary<CornerType, BlockHelpers.BlockVertexLocation[]> _sharedBlockVertexLocations { get; }
    }
}
