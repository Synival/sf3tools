using System;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_SurfaceTile : IMPD_SurfaceTile {
        public MPD_SurfaceTile(
            IMPD_Surface surface,
            int x,
            int y,
            byte textureId = 0xFF,
            byte textureFlags = 0x00,
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

            TerrainType  = terrainType;
            TerrainFlags = terrainFlags;
            EventID      = eventId;

            if (heights != null && heights.Length == 4)
                _vertexHeights = heights;

            UpdateCenterHeight();
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

            UpdateCenterHeight();
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

        public byte CenterHeight { get; private set; }

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
            _vertexHeights[cornerInt] = value;

            UpdateCenterHeight();

            var vx = BlockHelpers.TileToVertexX(X, corner);
            var vy = BlockHelpers.TileToVertexY(Y, corner);
            Surface.GetVertex(vx, vy).UpdateNormalsInvolvingVertex();
        }

        public void SetVertexHeights(byte[] values) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(values) + ": Should have size of 4");
            _vertexHeights = values;

            UpdateCenterHeight();

            var x = X;
            var y = Y;
            Surface.UpdateVertexNormalsInvolvingVertices(x, y, x + 1, y + 1);
        }

        private void UpdateCenterHeight() {
            CenterHeight = (byte) ((_vertexHeights[0] + _vertexHeights[1] + _vertexHeights[2] + _vertexHeights[3]) / 4);
        }

        private byte[] _vertexHeights = new byte[4];

        public event EventHandler Modified;
    }
}
