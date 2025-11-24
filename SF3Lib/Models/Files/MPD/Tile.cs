using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD;

namespace SF3.Models.Files.MPD {
    public partial class Tile : IMPD_Tile {
        private static int[,] s_tileSeeds = null;

        public Tile(IMPD_File file, int x, int y) {
            MPD_File = file;
            X = x;
            Y = y;

            var allCorners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            BlockLocation = BlockHelpers.GetTileBlockLocation(x, y);

            _sharedTileLocations = allCorners
                .ToDictionary(c => c, GetSharedTilesAtCorner);
            _blockVertexLocations = allCorners
                .ToDictionary(c => c, c => BlockHelpers.GetVertexBlockLocations(X, Y, c, onlyInBlock: true)[0]);
            _sharedBlockVertexLocations = allCorners
                .ToDictionary(c => c, c => BlockHelpers.GetVertexBlockLocations(X, Y, c, onlyInBlock: false));

            if (s_tileSeeds == null)
                GenerateTileSeeds();

            RandomSeed = s_tileSeeds[x, y];
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; private set; }
        public BlockHelpers.BlockTileLocation BlockLocation { get; }

        public bool HasModel => MPD_File.SurfaceModel != null;
        public bool HasRotatableTexture => HasModel && MPD_File.SurfaceModel.TileTextureRowTable.HasRotation && MPD_File.Flags.HasSurfaceTextureRotation;


        public event EventHandler Modified;

        private struct TileAndCorner {
            public int X;
            public int Y;
            public CornerType Corner;
        }

        private Dictionary<CornerType, TileAndCorner[]> _sharedTileLocations { get; }
        private Dictionary<CornerType, BlockHelpers.BlockVertexLocation> _blockVertexLocations { get; }
        private Dictionary<CornerType, BlockHelpers.BlockVertexLocation[]> _sharedBlockVertexLocations { get; }
    }
}
