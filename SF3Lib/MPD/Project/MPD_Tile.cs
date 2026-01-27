using System;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Tile : IMPD_Tile {
        public MPD_Tile(IMPD_Surface surface, int x, int y) {
            Surface     = surface;
            X           = x;
            Y           = y;
            RandomSeed  = MPD_TileSeeds.GetTileSeed(x, y);

            TextureID   = 0xFF;
            TerrainType = TerrainType.NoEntry;
        }

        public MPD_Tile(IMPD_Surface surface, IMPD_Tile original, int x, int y) {
            Surface     = surface;
            X           = x;
            Y           = y;
            RandomSeed  = MPD_TileSeeds.GetTileSeed(x, y);

            TextureID     = original.TextureID;
            TextureFlip   = original.TextureFlip;
            TextureRotate = original.TextureRotate;
            IsFlat        = original.IsFlat;

            TerrainType   = original.TerrainType;
            TerrainFlags  = original.TerrainFlags;
            EventID       = original.EventID;

            _vertexHeights = (float[]) (original.GetVertexHeights().Clone());
            _vertexNormals = (VECTOR[]) (original.GetVertexNormals().Clone());

            UpdateCenterHeight();
        }

        public IMPD_Surface Surface { get; }
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; }

        public byte TextureFlags {
            get => (byte) ((byte) TextureFlip | (byte) TextureRotate | (IsFlat ? 0x80 : 0));
            set {}
        }

        public byte TextureID { get; set; }
        public TextureFlipType TextureFlip { get; set; }
        public TextureRotateType TextureRotate { get; set; }
        public bool IsFlat { get; set; }

        public float CenterHeight { get; private set; }

        public TerrainType TerrainType { get; set; }
        public TerrainFlags TerrainFlags { get; set; }

        public byte EventID { get; set; }

        public float GetVertexHeight(CornerType corner) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            return _vertexHeights[cornerInt];
        }

        public VECTOR GetVertexNormal(CornerType corner) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            return _vertexNormals[cornerInt];
        }

        public float[] GetVertexHeights() => (float[]) (_vertexHeights.Clone());
        public VECTOR[] GetVertexNormals() => (VECTOR[]) (_vertexNormals.Clone());

        public void SetVertexHeight(CornerType corner, float value) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            _vertexHeights[cornerInt] = value;
            UpdateCenterHeight();
        }

        public void SetVertexHeights(float[] values) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(values) + ": Should have size of 4");
            _vertexHeights = values;
            UpdateCenterHeight();
        }

        private void UpdateCenterHeight() {
            var values = new int[] {
                (int) Math.Round(_vertexHeights[0] * 16),
                (int) Math.Round(_vertexHeights[1] * 16),
                (int) Math.Round(_vertexHeights[2] * 16),
                (int) Math.Round(_vertexHeights[3] * 16)
            };
            var sum = values[0] + values[1] + values[2] + values[3];

            CenterHeight = (float) Math.Floor(sum / 4f) / 16f;
        }

        private float[] _vertexHeights = new float[4];
        private VECTOR[] _vertexNormals = new VECTOR[4];

        public event EventHandler Modified;
    }
}
