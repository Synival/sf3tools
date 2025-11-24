using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        private static void GenerateTileSeeds() {
            s_tileSeeds = new int[64, 64];
            for (int x = 0; x < 64; x++) {
                for (int y = 0; y < 64; y++) {
                    var seed = 2166136261;
                    seed += (uint) x;
                    seed *= 16777619;
                    seed += (uint) y;
                    seed *= 16777619;
                    s_tileSeeds[x, y] = (int) seed;
                }
            }
        }

        private TileAndCorner[] GetSharedTilesAtCorner(CornerType corner) {
            TileAndCorner[] GetUnfiltered() {
                switch (corner) {
                    case CornerType.TopLeft:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X,     Y = Y,     Corner = corner },
                            new TileAndCorner { X = X - 1, Y = Y + 1, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = X - 1, Y = Y + 0, Corner = CornerType.TopRight },
                            new TileAndCorner { X = X - 0, Y = Y + 1, Corner = CornerType.BottomLeft },
                        };

                    case CornerType.TopRight:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X,     Y = Y,     Corner = corner },
                            new TileAndCorner { X = X + 1, Y = Y + 1, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = X + 1, Y = Y + 0, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = X + 0, Y = Y + 1, Corner = CornerType.BottomRight },
                        };

                    case CornerType.BottomRight:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X,     Y = Y,     Corner = corner },
                            new TileAndCorner { X = X + 1, Y = Y - 1, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = X + 1, Y = Y - 0, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = X + 0, Y = Y - 1, Corner = CornerType.TopRight },
                        };

                    case CornerType.BottomLeft:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X,     Y = Y,     Corner = corner },
                            new TileAndCorner { X = X - 1, Y = Y - 1, Corner = CornerType.TopRight },
                            new TileAndCorner { X = X - 1, Y = Y - 0, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = X - 0, Y = Y - 1, Corner = CornerType.TopLeft },
                        };

                    default:
                        throw new ArgumentException(nameof(corner));
                }
            }

            return GetUnfiltered()
                .Where(x => x.X >= 0 && x.Y >= 0 && x.X < 64 && x.Y < 64)
                .ToArray();
        }

        private void UpdateVertexNormals(CornerType corner, out HashSet<Tile> tilesModified) {
            var surfaceModel = MPD_File.SurfaceModelChunk;
            if (surfaceModel == null) {
                tilesModified = new HashSet<Tile>();
                return;
            }

            var vxCenter = BlockHelpers.TileToVertexX(X, corner);
            var vyCenter = BlockHelpers.TileToVertexY(Y, corner);

            // Normals need to be updated in a 3x3 grid.
            var heightmapRowTable = MPD_File.SurfaceDataChunk.HeightmapRowTable;
            for (var x = -1; x <= 1; x++) {
                for (var y = -1; y <= 1; y++) {
                    var vx = x + vxCenter;
                    var vy = y + vyCenter;
                    if (vx >= 0 && vy >= 0 && vx < 65 && vy < 65)
                        surfaceModel.UpdateVertexNormal(vx, vy, heightmapRowTable, Surface.NormalSettings);
                }
            }

            // Updating vertex normals in a 3x3 grid means tiles need to be re-rendered in a 4x4 grid.
            tilesModified = new HashSet<Tile>();
            for (var x = -2; x <= 1; x++) {
                for (var y = -2; y <= 1; y++) {
                    var tx = x + vxCenter;
                    var ty = y + vyCenter;
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) {
                        var tile = MPD_File.Surface.GetTile(tx, ty) as Tile;
                        tilesModified.Add(tile);
                    }
                }
            }
        }
    }
}
