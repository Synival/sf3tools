using System;
using System.Linq;
using CommonLib.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public float GetVisualVertexHeight(CornerType corner) {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.SurfaceData?.HeightmapRowTable != null && MPD_File.SurfaceModel?.TileTextureRowTable != null && IsFlat)
                return MPD_File.SurfaceData.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);

            // The model to show should come from the surface model.
            if (MPD_File.SurfaceModel?.VertexHeightBlockTable != null) {
                var bvl = BlockVertexLocations[corner];
                return MPD_File.SurfaceModel.VertexHeightBlockTable[bvl.Num][bvl.X, bvl.Y] / 16.0f;
            }

            // If that doesn't exist, fall back to the surface heightmap.
            if (MPD_File.SurfaceData?.HeightmapRowTable != null)
                return MPD_File.SurfaceData.HeightmapRowTable[Y].GetHeight(X, corner);

            // If *that* doesn't exist, there isn't a surface; return nothing.
            return 0;
        }

        public float[] GetVisualVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.SurfaceData?.HeightmapRowTable != null && MPD_File.SurfaceModel?.TileTextureRowTable != null && IsFlat) {
                var brHeight = MPD_File.SurfaceData.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                return new float[] { brHeight, brHeight, brHeight, brHeight };
            }

            // The model to show should come from the surface model.
            if (MPD_File.SurfaceModel?.VertexHeightBlockTable != null) {
                return BlockVertexLocations.Values
                    .Select(bvl => MPD_File.SurfaceModel.VertexHeightBlockTable[bvl.Num][bvl.X, bvl.Y] / 16.0f)
                    .ToArray();
            }

            // If that doesn't exist, fall back to the surface heightmap.
            if (MPD_File.SurfaceData?.HeightmapRowTable != null)
                return MPD_File.SurfaceData.HeightmapRowTable[Y].GetQuadHeights(X);

            // If *that* doesn't exist, there isn't a surface; return nothing.
            return new float[] { 0, 0, 0, 0 };
        }

        public float GetSurfaceDataVertexHeight(CornerType corner)
            => MPD_File.SurfaceData?.HeightmapRowTable?[Y]?.GetHeight(X, corner) ?? 0.0f;
        public void SetSurfaceDataVertexHeight(CornerType corner, float value) {
            if (MPD_File.SurfaceData?.HeightTerrainRowTable == null)
                return;
            MPD_File.SurfaceData.HeightmapRowTable[Y].SetHeight(X, corner, value);
            Modified?.Invoke(this, EventArgs.Empty);
        }

        public float GetSurfaceModelVertexHeight(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return ((MPD_File.SurfaceModel?.VertexHeightBlockTable?[bl.Num]?[bl.X, bl.Y]) ?? 0) / 16f;
        }

        public void SetSurfaceModelVertexHeight(CornerType corner, float value) {
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

        public float GetAverageSurfaceDataVertexHeight()
            => (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType))).Average(x => GetSurfaceDataVertexHeight(x) * 16f)) / 16f;

        public void CopySurfaceDataVertexHeightToNonFlatNeighbors(CornerType corner) {
            var height = GetSurfaceDataVertexHeight(corner);
            var adjacentCorners = GetAdjacentTilesAtCorner(corner);
            foreach (var ac in adjacentCorners) {
                var acTile = MPD_File.Tiles[ac.X, ac.Y];
                if (!acTile.IsFlat) {
                    acTile.SetSurfaceDataVertexHeight(ac.Corner, height);
                    acTile.CenterHeight = acTile.GetAverageSurfaceDataVertexHeight();
                }
            }
        }

        public bool IsFlat {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetIsFlatFlag(X) : false;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetIsFlatFlag(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float CenterHeight {
            get => (MPD_File.SurfaceData != null) ? MPD_File.SurfaceData.HeightTerrainRowTable[Y].GetHeight(X) : 0.0f;
            set {
                MPD_File.SurfaceData.HeightTerrainRowTable[Y].SetHeight(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
