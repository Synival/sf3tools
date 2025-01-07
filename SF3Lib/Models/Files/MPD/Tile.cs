using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public class Tile {
        public Tile(IMPD_File mpdFile, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
        }

        public void UpdateAbnormals() {
            MPD_File.SurfaceModel?.UpdateVertexAbnormals(
                X, Y,
                MPD_File.Surface.HeightmapRowTable,
                POLYGON_NormalCalculationMethod.MostExtremeVerticalTriangle
            );
        }

        public float[] GetSurfaceModelVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the walking heightmap is used.
            if (MPD_File.Surface?.HeightmapRowTable != null && ModelUseMoveHeightmap)
                return MPD_File.Surface?.HeightmapRowTable.Rows[Y].GetHeights(X);

            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return new float[] { 0, 0, 0, 0 };

            var blockLocations = new BlockVertexLocation[] {
                GetBlockLocations(X, Y, CornerType.TopLeft,     true)[0],
                GetBlockLocations(X, Y, CornerType.TopRight,    true)[0],
                GetBlockLocations(X, Y, CornerType.BottomRight, true)[0],
                GetBlockLocations(X, Y, CornerType.BottomLeft,  true)[0],
            };
            return blockLocations
                .Select(x => MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[x.Num][x.X, x.Y] / 16.0f)
                .ToArray();
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }

        public TerrainType MoveTerrain {
            get => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].GetTerrainType(X);
            set => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].SetTerrainType(X, value);
        }

        public float MoveHeight {
            get => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].GetHeight(X);
            set => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].SetHeight(X, value);
        }

        public float GetMoveHeightmap(CornerType corner)
            => MPD_File.Surface.HeightmapRowTable.Rows[Y].GetHeight(X, corner);
        public void SetMoveHeightmap(CornerType corner, float value)
            => MPD_File.Surface.HeightmapRowTable.Rows[Y].SetHeight(X, corner, value);

        public byte EventID {
            get => MPD_File.Surface.EventIDRowTable.Rows[Y][X];
            set => MPD_File.Surface.EventIDRowTable.Rows[Y][X] = value;
        }

        public byte ModelTextureID {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetTextureID(X);
            set => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetTextureID(X, value);
        }

        public TextureFlipType ModelTextureFlip {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetFlip(X);
            set => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetFlip(X, value);
        }

        public TextureRotateType ModelTextureRotate {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetRotate(X);
            set => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetRotate(X, value);
        }

        public bool ModelUseMoveHeightmap {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetUseMoveHeightmapFlag(X);
            set => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetUseMoveHeightmapFlag(X, value);
        }

        public float GetModelVertexHeightmap(CornerType corner) {
            var bl = BlockHelpers.GetBlockLocations(X, Y, corner, true)[0];
            return MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[bl.Num][bl.X, bl.Y] / 16f;
        }

        public void SetModelVertexHeightmap(CornerType corner, float value) {
            var bls = BlockHelpers.GetBlockLocations(X, Y, corner, false);
            foreach (var bl in bls)
                MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[bl.Num][bl.X, bl.Y] = (byte) (value * 16f);
        }
    }
}
