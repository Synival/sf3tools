using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using SF3.MPD;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public partial class Tile : IMPD_Tile {
        private static int[,] s_tileSeeds = null;

        public Tile(IMPD_File mpdFile, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
            BlockLocation = GetTileBlockLocation(x, y);
            BlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, true)[0]);
            SharedBlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, false));

            if (s_tileSeeds == null)
                GenerateTileSeeds();
            RandomSeed = s_tileSeeds[x, y];
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; private set; }

        public BlockTileLocation BlockLocation { get; }
        public Dictionary<CornerType, BlockVertexLocation> BlockVertexLocations { get; }
        public Dictionary<CornerType, BlockVertexLocation[]> SharedBlockVertexLocations { get; }

        public EventHandler Modified;
    }
}
