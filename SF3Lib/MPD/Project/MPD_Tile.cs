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

            TextureFlags  = 0; // TODO: these should be derived!
            TextureID     = original.TextureID;
            TextureFlip   = original.TextureFlip;
            TextureRotate = original.TextureRotate;
            IsFlat        = original.IsFlat;

            TerrainType   = original.TerrainType;
            TerrainFlags  = 0; // TODO: these should be derived!

            EventID       = original.EventID;
        }

        public IMPD_Surface Surface { get; }
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; }

        public byte TextureFlags { get; set; }
        public byte TextureID { get; set; }
        public TextureFlipType TextureFlip { get; set; }
        public TextureRotateType TextureRotate { get; set; }
        public bool IsFlat { get; set; }

        public float CenterHeight => 0;

        public TerrainType TerrainType { get; set; }
        public TerrainFlags TerrainFlags { get; set; }
        public byte EventID { get; set; }

        public float GetVertexHeight(CornerType corner) => 0;
        public float[] GetVertexHeights() => new float[] { 0, 0, 0, 0 };
        public VECTOR GetVertexNormal(CornerType corner) => new VECTOR(0, 1, 0);
        public VECTOR[] GetVertexNormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexNormal(c)).ToArray();
        }

        public void SetVertexHeight(CornerType corner, float value) {}
        public void SetVertexHeights(float[] values) {}


        public event EventHandler Modified;
    }
}
