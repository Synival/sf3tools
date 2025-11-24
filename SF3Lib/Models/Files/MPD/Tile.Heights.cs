using System;
using System.Collections.Generic;
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
                var bvl = _blockVertexLocations[corner];
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
                return _blockVertexLocations.Values
                    .Select(bvl => MPD_File.SurfaceModel.VertexHeightBlockTable[bvl.Num][bvl.X, bvl.Y] / 16.0f)
                    .ToArray();
            }

            // If that doesn't exist, fall back to the surface heightmap.
            if (MPD_File.SurfaceData?.HeightmapRowTable != null)
                return MPD_File.SurfaceData.HeightmapRowTable[Y].GetQuadHeights(X);

            // If *that* doesn't exist, there isn't a surface; return nothing.
            return new float[] { 0, 0, 0, 0 };
        }

        public void SetVisualVertexHeight(CornerType corner, float value) {
            SetVisualVertexHeight(corner, value, out var tilesModified);
            foreach (var t in tilesModified)
                t.Modified?.Invoke(t, EventArgs.Empty);
        }

        private void SetVisualVertexHeight(CornerType corner, float value, out HashSet<Tile> tilesModified) {
            // Height can only be set in increments of 1/16.
            value = (float) Math.Round(value * 16.0f) / 16.0f;

            // Track tiles updated so they can be informed of updates afterwards, without redundancy.
            tilesModified = new HashSet<Tile>();

            // Update positions in the SurfaceData tables.
            if (MPD_File.SurfaceData != null) {
                var tilesToUpdate = IsFlat
                    ? new TileAndCorner[] { _sharedTileLocations[corner][0] }
                    : _sharedTileLocations[corner];

                foreach (var stl in tilesToUpdate) {
                    var tile = (Tile) Surface.GetTile(stl.X, stl.Y);
                    if (tile != this && tile.IsFlat)
                        continue;

                    var rowCorners = MPD_File.SurfaceData.HeightmapRowTable[tile.Y];
                    var rowCenter = MPD_File.SurfaceData.HeightTerrainRowTable[tile.Y];

                    rowCorners.SetHeight(tile.X, stl.Corner, value);
                    rowCenter.SetHeight(tile.X, ((CornerType[]) Enum.GetValues(typeof(CornerType))).Select(x => rowCorners.GetHeight(tile.X, x)).Average());

                    tilesModified.Add(tile);
                }
            }

            if (MPD_File.SurfaceModel != null) {
                if (!IsFlat)
                    foreach (var bvl in _sharedBlockVertexLocations[corner])
                        MPD_File.SurfaceModel.VertexHeightBlockTable[bvl.Num].SetHeight(bvl.X, bvl.Y, value);

                UpdateVertexNormals(corner, out var tilesModifiedHere);
                foreach (var t in tilesModifiedHere)
                    tilesModified.Add(t);
            }
        }

        public void SetVisualVertexHeights(float[] values) {
            var tilesModified = new HashSet<Tile>();

            foreach (var corner in (CornerType[]) Enum.GetValues(typeof(CornerType))) {
                SetVisualVertexHeight(corner, values[(int) corner], out var tilesModifiedHere);
                foreach (var t in tilesModifiedHere)
                    tilesModified.Add(t);
            }

            foreach (var t in tilesModified) {
                System.Diagnostics.Debug.WriteLine($"({t.X}, {t.Y})");
                t.Modified?.Invoke(t, EventArgs.Empty);
            }
        }

        public bool IsFlat {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetIsFlatFlag(X) : false;
            set {
                if (MPD_File.SurfaceModel == null)
                    return;
                if (MPD_File.SurfaceModel.TileTextureRowTable[Y].GetIsFlatFlag(X) != value) {
                    // If flattening the tile, set heights to the lowest value.
                    if (value) {
                        var minHeight = GetVisualVertexHeights().Min();
                        MPD_File.SurfaceModel.TileTextureRowTable[Y].SetIsFlatFlag(X, true);
                        SetVisualVertexHeights(new float[] { minHeight, minHeight, minHeight, minHeight });
                    }
                    // If unflattening the tile, update its heights to its neighbors.
                    else {
                        // The bottom-right corner of the heightmap table determines the height.
                        // We're going to use that as a fallback if there's no non-flat tile to fetch here.
                        var brHeight = MPD_File.SurfaceData.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                        MPD_File.SurfaceModel.TileTextureRowTable[Y].SetIsFlatFlag(X, false);

                        foreach (var corner in (CornerType[]) Enum.GetValues(typeof(CornerType))) {
                            var newHeight = brHeight;
                            foreach (var stl in _sharedTileLocations[corner]) {
                                if (stl.X == X && stl.Y == Y)
                                    continue;
                                var tile = Surface.GetTile(stl.X, stl.Y);
                                if (tile.IsFlat)
                                    continue;
                                newHeight = tile.GetVisualVertexHeight(stl.Corner);
                                break;
                            }

                            SetVisualVertexHeight(corner, newHeight);
                        }
                    }

                    Modified?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public float CenterHeight
            => (MPD_File.SurfaceData != null) ? MPD_File.SurfaceData.HeightTerrainRowTable[Y].GetHeight(X) : 0.0f;
    }
}
