using System;
using System.Linq;
using CommonLib.Types;

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

        private void TriggerNeighborTileModified(int offsetX, int offsetY) {
            var x = X + offsetX;
            var y = Y + offsetY;

            if (x >= 0 && y >= 0 && x < 64 && y < 64) {
                var tile = MPD_File.Tiles[x, y];
                tile.Modified?.Invoke(tile, EventArgs.Empty);
            }
        }

        private struct TileAndCorner {
            public int X;
            public int Y;
            public CornerType Corner;
        }

        private TileAndCorner[] GetAdjacentTilesAtCorner(CornerType corner) {
            TileAndCorner[] GetUnfiltered() {
                switch (corner) {
                    case CornerType.TopLeft:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X - 1, Y = Y + 1, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = X - 1, Y = Y + 0, Corner = CornerType.TopRight },
                            new TileAndCorner { X = X - 0, Y = Y + 1, Corner = CornerType.BottomLeft },
                        };

                    case CornerType.TopRight:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X + 1, Y = Y + 1, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = X + 1, Y = Y + 0, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = X + 0, Y = Y + 1, Corner = CornerType.BottomRight },
                        };

                    case CornerType.BottomRight:
                        return new TileAndCorner[] {
                            new TileAndCorner { X = X + 1, Y = Y - 1, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = X + 1, Y = Y - 0, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = X + 0, Y = Y - 1, Corner = CornerType.TopRight },
                        };

                    case CornerType.BottomLeft:
                        return new TileAndCorner[] {
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
    }
}
