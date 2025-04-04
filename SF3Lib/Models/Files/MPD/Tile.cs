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

        /// <summary>
        /// Updates all normals for the tile, also correcting neighboring tiles.
        /// </summary>
        /// <param name="halfHeight">When on (default, SF3 behavior), quad heights are halved for the purpose of normal calculations.</param>
        public void UpdateNormals(bool halfHeight = true) {
            MPD_File.SurfaceModel?.UpdateVertexNormals(
                X, Y,
                MPD_File.Surface.HeightmapRowTable,
                POLYGON_NormalCalculationMethod.WeightedVerticalTriangles,
                halfHeight
            );

            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                    TriggerNeighborTileModified(x, y);
        }

        public float[] GetSurfaceModelVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.Surface?.HeightmapRowTable != null && MPD_File.SurfaceModel?.TileTextureRowTable != null && ModelIsFlat) {
                var brHeight = MPD_File.Surface.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                return new float[] { brHeight, brHeight, brHeight, brHeight };
            }

            if (MPD_File.Surface == null)
                return new float[] { 0, 0, 0, 0 };

            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return MPD_File.Surface.HeightmapRowTable[Y].GetQuadHeights(X);

            return BlockVertexLocations.Values
                .Select(x => MPD_File.SurfaceModel.VertexHeightBlockTable[x.Num][x.X, x.Y] / 16.0f)
                .ToArray();
        }

        public VECTOR GetVertexNormal(CornerType corner) {
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return new VECTOR(0f, 1 / 32768f, 0f);

            var loc = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable[loc.Num][loc.X, loc.Y];
        }

        public VECTOR[] GetVertexNormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexNormal(c)).ToArray();
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }
        public BlockTileLocation BlockLocation { get; }
        public Dictionary<CornerType, BlockVertexLocation> BlockVertexLocations { get; }
        public Dictionary<CornerType, BlockVertexLocation[]> SharedBlockVertexLocations { get; }

        public TerrainType MoveTerrainType {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags MoveTerrainFlags {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float MoveHeight {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetHeight(X) : 0.0f;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetHeight(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetMoveHeightmap(CornerType corner)
            => (MPD_File.Surface != null) ? MPD_File.Surface.HeightmapRowTable[Y].GetHeight(X, corner) : 0.0f;
        public void SetMoveHeightmap(CornerType corner, float value) {
            MPD_File.Surface.HeightmapRowTable[Y].SetHeight(X, corner, value);
            Modified?.Invoke(this, EventArgs.Empty);
        }

        public byte EventID {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.EventIDRowTable[Y][X] : (byte) 0;
            set {
                MPD_File.Surface.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureID {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureID(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureFlags {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureFlags(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType ModelTextureFlip {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetFlip(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType ModelTextureRotate {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetRotate(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ModelIsFlat {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetIsFlatFlag(X) : false;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetIsFlatFlag(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public int? TreeModelChunkIndex { get; set; } = null;
        public int? TreeModelID { get; set; } = null;

        public float GetModelVertexHeightmap(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexHeightBlockTable[bl.Num][bl.X, bl.Y] / 16f;
        }

        public void SetModelVertexHeightmap(CornerType corner, float value) {
            var bls = SharedBlockVertexLocations[corner];
            foreach (var bl in bls)
                MPD_File.SurfaceModel.VertexHeightBlockTable[bl.Num][bl.X, bl.Y] = (byte) (value * 16f);

            Modified?.Invoke(this, EventArgs.Empty);

            var otherTilesOffsetX = corner.GetVertexOffsetX() * 2 - 1;
            var otherTilesOffsetY = corner.GetVertexOffsetY() * 2 - 1;
            TriggerNeighborTileModified(otherTilesOffsetX, 0);
            TriggerNeighborTileModified(0, otherTilesOffsetY);
            TriggerNeighborTileModified(otherTilesOffsetX, otherTilesOffsetY);
        }

        public VECTOR GetModelVertexNormal(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable[bl.Num][bl.X, bl.Y];
        }

        public void SetModelVertexNormal(CornerType corner, VECTOR value) {
            var bls = SharedBlockVertexLocations[corner];
            foreach (var bl in bls)
                MPD_File.SurfaceModel.VertexNormalBlockTable[bl.Num][bl.X, bl.Y] = value;

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
