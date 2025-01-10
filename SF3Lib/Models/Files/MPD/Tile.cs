using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public class Tile {
        public Tile(IMPD_File mpdFile, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
            BlockLocation = GetTileBlockLocation(x, y);
            BlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, true)[0]);
            SharedBlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, false));
        }

        private void TriggerNeighborTileModified(int offsetX, int offsetY) {
            var x = X + offsetX;
            var y = Y + offsetY;

            if (x >= 0 && y >= 0 && x < 64 && y < 64) {
                var tile = MPD_File.Tiles[x, y];
                tile.Modified?.Invoke(tile, EventArgs.Empty);
            }
        }

        public void UpdateAbnormals() {
            MPD_File.SurfaceModel?.UpdateVertexAbnormals(
                X, Y,
                MPD_File.Surface.HeightmapRowTable,
                POLYGON_NormalCalculationMethod.MostExtremeVerticalTriangle
            );

            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                    TriggerNeighborTileModified(x, y);
        }

        public float[] GetSurfaceModelVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the walking heightmap is used.
            if (MPD_File.Surface?.HeightmapRowTable != null && MPD_File.SurfaceModel?.TileTextureRowTable != null && ModelUseMoveHeightmap)
                return MPD_File.Surface?.HeightmapRowTable.Rows[Y].GetQuadHeights(X);

            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return new float[] { 0, 0, 0, 0 };

            return BlockVertexLocations.Values
                .Select(x => MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[x.Num][x.X, x.Y] / 16.0f)
                .ToArray();
        }

        public VECTOR GetVertexAbnormal(CornerType corner) {
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable?.Rows == null)
                return new VECTOR(0f, 1 / 32768f, 0f);

            var loc = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable.Rows[loc.Num][loc.X, loc.Y];
        }

        public VECTOR[] GetVertexAbnormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexAbnormal(c)).ToArray();
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }
        public BlockTileLocation BlockLocation { get; }
        public Dictionary<CornerType, BlockVertexLocation> BlockVertexLocations { get; }
        public Dictionary<CornerType, BlockVertexLocation[]> SharedBlockVertexLocations { get; }

        public TerrainType MoveTerrain {
            get => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].GetTerrainType(X);
            set {
                MPD_File.Surface.HeightTerrainRowTable.Rows[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float MoveHeight {
            get => MPD_File.Surface.HeightTerrainRowTable.Rows[Y].GetHeight(X);
            set {
                MPD_File.Surface.HeightTerrainRowTable.Rows[Y].SetHeight(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetMoveHeightmap(CornerType corner)
            => MPD_File.Surface.HeightmapRowTable.Rows[Y].GetHeight(X, corner);
        public void SetMoveHeightmap(CornerType corner, float value) {
            MPD_File.Surface.HeightmapRowTable.Rows[Y].SetHeight(X, corner, value);
            Modified?.Invoke(this, EventArgs.Empty);
        }

        public byte EventID {
            get => MPD_File.Surface.EventIDRowTable.Rows[Y][X];
            set {
                MPD_File.Surface.EventIDRowTable.Rows[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureID {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetTextureID(X);
            set {
                MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureFlags {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetTextureFlags(X);
            set {
                MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType ModelTextureFlip {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetFlip(X);
            set {
                MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType ModelTextureRotate {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetRotate(X);
            set {
                MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ModelUseMoveHeightmap {
            get => MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].GetUseMoveHeightmapFlag(X);
            set {
                MPD_File.SurfaceModel.TileTextureRowTable.Rows[Y].SetUseMoveHeightmapFlag(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetModelVertexHeightmap(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[bl.Num][bl.X, bl.Y] / 16f;
        }

        public void SetModelVertexHeightmap(CornerType corner, float value) {
            var bls = SharedBlockVertexLocations[corner];
            foreach (var bl in bls)
                MPD_File.SurfaceModel.VertexHeightBlockTable.Rows[bl.Num][bl.X, bl.Y] = (byte) (value * 16f);

            Modified?.Invoke(this, EventArgs.Empty);

            var otherTilesOffsetX = corner.GetVertexOffsetX() * 2 - 1;
            var otherTilesOffsetY = corner.GetVertexOffsetY() * 2 - 1;
            TriggerNeighborTileModified(otherTilesOffsetX, 0);
            TriggerNeighborTileModified(0, otherTilesOffsetY);
            TriggerNeighborTileModified(otherTilesOffsetX, otherTilesOffsetY);
        }

        public float GetAverageHeight()
            => (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType))).Average(x => GetMoveHeightmap(x) * 16f)) / 16f;

        public EventHandler Modified;
    }
}
