using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
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
            Surface       = surface;
            X             = x;
            Y             = y;
            RandomSeed    = MPD_TileSeeds.GetTileSeed(x, y);

            _textureId    = textureId;
            TextureFlags  = textureFlags;
            _unknownTextureFlags = unknownTextureFlags;

            _terrainType  = terrainType;
            _terrainFlags = terrainFlags;
            _eventId      = eventId;

            if (heights != null && heights.Length == 4)
                _vertexHeights = (byte[]) (heights.Clone());

            var allCorners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            _sharedTileLocations = allCorners
                .ToDictionary(c => c, c => GetSharedTilesAtCorner(X, Y, c));
        }

        public MPD_SurfaceTile(IMPD_Surface surface, IMPD_SurfaceTile original, int x, int y) {
            Surface        = surface;
            X              = x;
            Y              = y;
            RandomSeed     = MPD_TileSeeds.GetTileSeed(x, y);

            _textureId     = original.TextureID;
            _textureFlip   = original.TextureFlip;
            _textureRotate = original.TextureRotate;
            _unknownTextureFlags = original.UnknownTextureFlags;
            _isFlat        = original.IsFlat;

            _terrainType   = original.TerrainType;
            _terrainFlags  = original.TerrainFlags;
            _eventId       = original.EventID;

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

        private void SetAndUpdateModified<T>(ref T field, T newValue) where T : struct {
            if (field.Equals(newValue))
                return;
            field = newValue;
            Modified?.Invoke(this, EventArgs.Empty);
        }

        private byte _unknownTextureFlags;
        public byte UnknownTextureFlags {
            get => _unknownTextureFlags;
            set => SetAndUpdateModified(ref _unknownTextureFlags, value);
        }

        private byte _textureId;
        public byte TextureID {
            get => _textureId;
            set => SetAndUpdateModified(ref _textureId, value);
        }

        private TextureFlipType _textureFlip;
        public TextureFlipType TextureFlip {
            get => _textureFlip;
            set => SetAndUpdateModified(ref _textureFlip, value);
        }

        private TextureRotateType _textureRotate;
        public TextureRotateType TextureRotate {
            get => _textureRotate;
            set => SetAndUpdateModified(ref _textureRotate, value);
        }

        private bool _isFlat;
        public bool IsFlat {
            get => _isFlat;
            set => SetAndUpdateModified(ref _isFlat, value);
        }

        private byte? _centerHeight = null;
        public byte CenterHeight {
            get {
                if (!_centerHeight.HasValue)
                    _centerHeight = (byte) ((_vertexHeights[0] + _vertexHeights[1] + _vertexHeights[2] + _vertexHeights[3]) / 4);
                return _centerHeight.Value;
            }
        }

        private TerrainType _terrainType;
        public TerrainType TerrainType {
            get => _terrainType;
            set => SetAndUpdateModified(ref _terrainType, value);
        }

        private TerrainFlags _terrainFlags;
        public TerrainFlags TerrainFlags {
            get => _terrainFlags;
            set => SetAndUpdateModified(ref _terrainFlags, value);
        }

        private byte _eventId;
        public byte EventID {
            get => _eventId;
            set => SetAndUpdateModified(ref _eventId, value);
        }

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

            var vx = TileToVertexX(X, corner);
            var vy = TileToVertexY(Y, corner);
            var vertexRange = Surface.GetNormalVertexRangeAffectedByHeightOf(vx, vy);
            Surface.UpdateVertexNormals(vertexRange);

            var tileRange = Surface.GetTileRangeContainingVertexRange(vertexRange);
            for (int ty = tileRange.Top; ty <= tileRange.Bottom; ty++) {
                for (int tx = tileRange.Left; tx <= tileRange.Right; tx++) {
                    var tile = (MPD_SurfaceTile) Surface.GetTile(tx, ty);
                    tile.Modified?.Invoke(tile, EventArgs.Empty);
                }
            }
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
            var vertexRange = Surface.GetNormalVertexRangeAffectedByHeightsOf(x, y, x + 1, y + 1);
            Surface.UpdateVertexNormals(vertexRange);

            var tileRange = Surface.GetTileRangeContainingVertexRange(vertexRange);
            for (int ty = tileRange.Top; ty <= tileRange.Bottom; ty++) {
                for (int tx = tileRange.Left; tx <= tileRange.Right; tx++) {
                    var tile = (MPD_SurfaceTile) Surface.GetTile(tx, ty);
                    tile.Modified?.Invoke(tile, EventArgs.Empty);
                }
            }
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
