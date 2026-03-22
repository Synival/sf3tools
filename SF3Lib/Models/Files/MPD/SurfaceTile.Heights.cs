using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using SF3.MPD.Extensions;
using static SF3.Utils.SurfaceUtils;

namespace SF3.Models.Files.MPD {
    public partial class SurfaceTile {
        public byte GetVertexHeight(CornerType corner) {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.SurfaceDataChunk?.HeightmapRowTable != null && MPD_File.SurfaceModelChunk?.TileTextureRowTable != null && IsFlat)
                return MPD_File.SurfaceDataChunk.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);

            // The model to show should come from the surface model.
            if (MPD_File.SurfaceModelChunk?.VertexHeightBlockTable != null) {
                var bvl = _blockVertexLocations[(int) corner];
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
                return _blockVertexLocations
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

        public TileAndCorner[] GetSharedVerticesAtCorner(CornerType corner) {
            // No height data if there's no surface data (which would be VERY strange!).
            if (MPD_File.SurfaceDataChunk == null)
                return new TileAndCorner[0];

            // Flat tiles have nothing linked.
            if (IsFlat)
                return new TileAndCorner[] { _sharedTileLocations[(int) corner][0] };

            // Otherwise, this vertex is shared with all other adjacent non-flat tiles.
            var tiles = new List<TileAndCorner>();
            foreach (var tile in _sharedTileLocations[(int) corner]) {
                var tileObj = Surface.GetTile(tile.X, tile.Y);
                if (tileObj != null && tileObj == this || !tileObj.IsFlat)
                    tiles.Add(tile);
            }

            return tiles.ToArray();
        }

        private void SetVertexHeight(CornerType corner, byte value, out HashSet<SurfaceTile> tilesModified) {
            // Track tiles updated so they can be informed of updates afterwards, without redundancy.
            tilesModified = new HashSet<SurfaceTile>();

            // Update positions in the SurfaceData tables.
            var tilesToUpdate = GetSharedVerticesAtCorner(corner);
            foreach (var stl in tilesToUpdate) {
                var tile = (SurfaceTile) Surface.GetTile(stl.X, stl.Y);

                var rowCorners = MPD_File.SurfaceDataChunk.HeightmapRowTable[tile.Y];
                var rowCenter = MPD_File.SurfaceDataChunk.HeightTerrainRowTable[tile.Y];

                rowCorners.SetHeight(tile.X, stl.Corner, value);

                var avg = ((CornerType[]) Enum.GetValues(typeof(CornerType))).Select(x => (int) rowCorners.GetHeight(tile.X, x)).Average();
                rowCenter.SetHeight(tile.X, (byte) avg);

                tilesModified.Add(tile);
            }

            if (MPD_File.SurfaceModelChunk != null) {
                if (!IsFlat)
                    foreach (var bvl in _sharedBlockVertexLocations[(int) corner])
                        MPD_File.SurfaceModelChunk.VertexHeightBlockTable[bvl.Num].SetHeight(bvl.X, bvl.Y, value);

                var normalVertices = Surface.GetNormalVertexRangeAffectedByHeightOf(X, Y, corner);
                Surface.UpdateVertexNormals(normalVertices);
                var normalTiles = Surface.GetTileRangeContainingVertexRange(normalVertices);

                for (var ty = normalTiles.Top; ty <= normalTiles.Bottom; ty++)
                    for (var tx = normalTiles.Left; tx <= normalTiles.Right; tx++)
                        tilesModified.Add((SurfaceTile) Surface.GetTile(tx, ty));
            }
        }

        public void SetVertexHeights(byte[] values) {
            var tilesModified = new HashSet<SurfaceTile>();

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
                    MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetIsFlatFlag(X, value);
                    Modified?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public byte CenterHeight
            => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetHeight(X) : (byte) 0;
    }
}
