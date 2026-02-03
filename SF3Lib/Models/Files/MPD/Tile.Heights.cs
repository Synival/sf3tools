using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public byte GetVertexHeight(CornerType corner) {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.SurfaceDataChunk?.HeightmapRowTable != null && MPD_File.SurfaceModelChunk?.TileTextureRowTable != null && IsFlat)
                return MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);

            // The model to show should come from the surface model.
            if (MPD_File.SurfaceModelChunk?.VertexHeightBlockTable != null) {
                var bvl = _blockVertexLocations[corner];
                return MPD_File.SurfaceModelChunk.VertexHeightBlockTable[bvl.Num][bvl.X, bvl.Y];
            }

            // If that doesn't exist, fall back to the surface heightmap.
            if (MPD_File.SurfaceDataChunk?.HeightmapRowTable != null)
                return MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeight(X, corner);

            // If *that* doesn't exist, there isn't a surface; return nothing.
            return 0;
        }

        public byte[] GetVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.SurfaceDataChunk?.HeightmapRowTable != null && MPD_File.SurfaceModelChunk?.TileTextureRowTable != null && IsFlat) {
                var brHeight = MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                return new byte[] { brHeight, brHeight, brHeight, brHeight };
            }

            // The model to show should come from the surface model.
            if (MPD_File.SurfaceModelChunk?.VertexHeightBlockTable != null) {
                return _blockVertexLocations.Values
                    .Select(bvl => MPD_File.SurfaceModelChunk.VertexHeightBlockTable[bvl.Num][bvl.X, bvl.Y])
                    .ToArray();
            }

            // If that doesn't exist, fall back to the surface heightmap.
            if (MPD_File.SurfaceDataChunk?.HeightmapRowTable != null)
                return MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeights(X);

            // If *that* doesn't exist, there isn't a surface; return nothing.
            return new byte[] { 0, 0, 0, 0 };
        }

        public void SetVertexHeight(CornerType corner, byte value) {
            SetVertexHeight(corner, value, out var tilesModified);
            foreach (var t in tilesModified)
                t.Modified?.Invoke(t, EventArgs.Empty);
        }

        private void SetVertexHeight(CornerType corner, byte value, out HashSet<Tile> tilesModified) {
            // Track tiles updated so they can be informed of updates afterwards, without redundancy.
            tilesModified = new HashSet<Tile>();

            // Update positions in the SurfaceData tables.
            if (MPD_File.SurfaceDataChunk != null) {
                var tilesToUpdate = IsFlat
                    ? new TileAndCorner[] { _sharedTileLocations[corner][0] }
                    : _sharedTileLocations[corner];

                foreach (var stl in tilesToUpdate) {
                    var tile = (Tile) Surface.GetTile(stl.X, stl.Y);
                    if (tile != this && tile.IsFlat)
                        continue;

                    var rowCorners = MPD_File.SurfaceDataChunk.HeightmapRowTable[tile.Y];
                    var rowCenter = MPD_File.SurfaceDataChunk.HeightTerrainRowTable[tile.Y];

                    rowCorners.SetHeight(tile.X, stl.Corner, value);

                    var avg = ((CornerType[]) Enum.GetValues(typeof(CornerType))).Select(x => (int) rowCorners.GetHeight(tile.X, x)).Average();
                    rowCenter.SetHeight(tile.X, (byte) avg);

                    tilesModified.Add(tile);
                }
            }

            if (MPD_File.SurfaceModelChunk != null) {
                if (!IsFlat)
                    foreach (var bvl in _sharedBlockVertexLocations[corner])
                        MPD_File.SurfaceModelChunk.VertexHeightBlockTable[bvl.Num].SetHeight(bvl.X, bvl.Y, value);

                UpdateVertexNormals(corner, out var tilesModifiedHere);
                foreach (var t in tilesModifiedHere)
                    tilesModified.Add(t);
            }
        }

        public void SetVertexHeights(byte[] values) {
            var tilesModified = new HashSet<Tile>();

            foreach (var corner in (CornerType[]) Enum.GetValues(typeof(CornerType))) {
                SetVertexHeight(corner, values[(int) corner], out var tilesModifiedHere);
                foreach (var t in tilesModifiedHere)
                    tilesModified.Add(t);
            }

            foreach (var t in tilesModified)
                t.Modified?.Invoke(t, EventArgs.Empty);
        }

        public bool IsFlat {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetIsFlatFlag(X) : false;
            set {
                if (MPD_File.SurfaceModelChunk == null)
                    return;
                if (MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetIsFlatFlag(X) != value) {
                    // If flattening the tile, set heights to the lowest value.
                    if (value) {
                        var minHeight = GetVertexHeights().Min();
                        MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetIsFlatFlag(X, true);
                        SetVertexHeights(new byte[] { minHeight, minHeight, minHeight, minHeight });
                    }
                    // If unflattening the tile, update its heights to its neighbors.
                    else {
                        // The bottom-right corner of the heightmap table determines the height.
                        // We're going to use that as a fallback if there's no non-flat tile to fetch here.
                        var brHeight = MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                        MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetIsFlatFlag(X, false);

                        foreach (var corner in (CornerType[]) Enum.GetValues(typeof(CornerType))) {
                            var newHeight = brHeight;
                            foreach (var stl in _sharedTileLocations[corner]) {
                                if (stl.X == X && stl.Y == Y)
                                    continue;
                                var tile = Surface.GetTile(stl.X, stl.Y);
                                if (tile.IsFlat)
                                    continue;
                                newHeight = tile.GetVertexHeight(stl.Corner);
                                break;
                            }

                            SetVertexHeight(corner, newHeight);
                        }
                    }

                    Modified?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public byte CenterHeight
            => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetHeight(X) : (byte) 0;
    }
}
