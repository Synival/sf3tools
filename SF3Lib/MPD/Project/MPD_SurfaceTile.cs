using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;
using static SF3.Utils.SurfaceUtils;

namespace SF3.MPD.Project {
    public class MPD_SurfaceTile : IMPD_SurfaceTile {
        public MPD_SurfaceTile(
            IMPD_Surface surface,
            int x,
            int y,
            byte textureId = 0xFF,
            byte textureFlags = 0x00,
            byte unknownTextureFlags = 0x00,
            byte eventId = 0,
            TerrainType terrainType = TerrainType.NoEntry,
            TerrainFlags terrainFlags = 0,
            byte[] heights = null
        ) {
            Surface      = surface;
            X            = x;
            Y            = y;
            RandomSeed   = MPD_TileSeeds.GetTileSeed(x, y);

            TextureID    = textureId;
            TextureFlags = textureFlags;
            UnknownTextureFlags = unknownTextureFlags;

            TerrainType  = terrainType;
            TerrainFlags = terrainFlags;
            EventID      = eventId;

            if (heights != null && heights.Length == 4)
                _vertexHeights = (byte[]) (heights.Clone());

            var allCorners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            _sharedTileLocations = allCorners
                .ToDictionary(c => c, c => GetSharedTilesAtCorner(X, Y, c));
        }

        public MPD_SurfaceTile(IMPD_Surface surface, IMPD_SurfaceTile original, int x, int y) {
            Surface     = surface;
            X           = x;
            Y           = y;
            RandomSeed  = MPD_TileSeeds.GetTileSeed(x, y);

            TextureID     = original.TextureID;
            TextureFlip   = original.TextureFlip;
            TextureRotate = original.TextureRotate;
            UnknownTextureFlags = original.UnknownTextureFlags;
            IsFlat        = original.IsFlat;

            TerrainType   = original.TerrainType;
            TerrainFlags  = original.TerrainFlags;
            EventID       = original.EventID;

            _vertexHeights = (byte[]) (original.GetVertexHeights().Clone());
        }

        public IMPD_Surface Surface { get; }
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; }

        public byte TextureFlags {
            get => (byte) ((byte) TextureFlip | (byte) TextureRotate | (IsFlat ? 0x80 : 0));
            set {
                TextureFlip   = (TextureFlipType)   (value & 0x30);
                TextureRotate = (TextureRotateType) (value & 0x03);
                IsFlat        = (value & 0x80) == 0x80;
            }
        }

        public byte UnknownTextureFlags { get; set; }

        public byte TextureID { get; set; }
        public TextureFlipType TextureFlip { get; set; }
        public TextureRotateType TextureRotate { get; set; }
        public bool IsFlat { get; set; }

        private byte? _centerHeight = null;
        public byte CenterHeight {
            get {
                if (!_centerHeight.HasValue)
                    _centerHeight = (byte) ((_vertexHeights[0] + _vertexHeights[1] + _vertexHeights[2] + _vertexHeights[3]) / 4);
                return _centerHeight.Value;
            }
        }

        public TerrainType TerrainType { get; set; }
        public TerrainFlags TerrainFlags { get; set; }

        public byte EventID { get; set; }

        public byte GetVertexHeight(CornerType corner) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            return _vertexHeights[cornerInt];
        }

        public byte[] GetVertexHeights() => (byte[]) (_vertexHeights.Clone());

        public void SetVertexHeight(CornerType corner, byte value) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));

            if (_vertexHeights[cornerInt] == value)
                return;

            var sharedTiles = GetSharedVerticesAtCorner(corner);
            foreach (var st in sharedTiles) {
                var tile = (MPD_SurfaceTile) Surface.GetTile(st.X, st.Y);
                tile._vertexHeights[(int) st.Corner] = value;
                tile.InvalidateCenterHeight();
            }

            var vx = BlockHelpers.TileToVertexX(X, corner);
            var vy = BlockHelpers.TileToVertexY(Y, corner);
            Surface.UpdateVertexNormals(Surface.GetNormalVertexRangeAffectedByHeightOf(vx, vy));
        }

        public void SetVertexHeights(byte[] values) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(values) + ": Should have size of 4");

            int updatedCount = 0;
            for (int i = 0; i < 4; ++i) {
                var corner = (CornerType) i;
                if (_vertexHeights[i] == values[i])
                    continue;

                var sharedTiles = GetSharedVerticesAtCorner(corner);
                foreach (var st in sharedTiles) {
                    var tile = (MPD_SurfaceTile) Surface.GetTile(st.X, st.Y);
                    tile._vertexHeights[(int) st.Corner] = values[i];
                    tile.InvalidateCenterHeight();
                    updatedCount++;
                }
            }
            if (updatedCount == 0)
                return;

            var x = X;
            var y = Y;
            Surface.UpdateVertexNormals(Surface.GetNormalVertexRangeAffectedByHeightsOf(x, y, x + 1, y + 1));
        }

        private void InvalidateCenterHeight() => _centerHeight = null;

        public TileAndCorner[] GetSharedVerticesAtCorner(CornerType corner) {
            // Flat tiles have nothing linked.
            if (IsFlat)
                return new TileAndCorner[] { _sharedTileLocations[corner][0] };

            // Otherwise, this vertex is shared with all other adjacent non-flat tiles.
            var tiles = new List<TileAndCorner>();
            foreach (var tile in _sharedTileLocations[corner]) {
                var tileObj = Surface.GetTile(tile.X, tile.Y);
                if (tileObj != null && tileObj == this || !tileObj.IsFlat)
                    tiles.Add(tile);
            }

            return tiles.ToArray();
        }

        private byte[] _vertexHeights = new byte[4];
        private Dictionary<CornerType, TileAndCorner[]> _sharedTileLocations { get; }

        public event EventHandler Modified;
    }
}
