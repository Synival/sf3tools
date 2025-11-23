using System;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        /// <summary>
        /// Updates all normals for the tile, also correcting neighboring tiles.
        /// </summary>
        /// <param name="settings">Settings for normal calculations.</param>
        public void UpdateNormals(NormalCalculationSettings settings) {
            MPD_File.SurfaceModel?.UpdateVertexNormals(X, Y, MPD_File.Surface.HeightmapRowTable, settings);
            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                    TriggerNeighborTileModified(x, y);
        }

        public void UpdateSharedVertexNormals(CornerType corner, NormalCalculationSettings settings) {
            var surfaceModel = MPD_File.SurfaceModel;
            if (surfaceModel == null)
                return;

            var vxCenter = TileToVertexX(X, corner);
            var vyCenter = TileToVertexY(Y, corner);

            // Normals need to be updated in a 3x3 grid.
            var heightmapRowTable = MPD_File.Surface.HeightmapRowTable;
            for (var x = -1; x <= 1; x++) {
                for (var y = -1; y <= 1; y++) {
                    var vx = x + vxCenter;
                    var vy = y + vyCenter;
                    if (vx >= 0 && vy >= 0 && vx < 65 && vy < 65)
                        surfaceModel.UpdateVertexNormal(vx, vy, heightmapRowTable, settings);
                }
            }

            // Updating vertex normals in a 3x3 grid means tiles need to be re-rendered in a 4x4 grid.
            for (var x = -2; x <= 2; x++) {
                for (var y = -2; y <= 2; y++) {
                    var tx = x + vxCenter;
                    var ty = y + vyCenter;
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) {
                        var tile = MPD_File.Tiles[tx, ty];
                        tile.Modified?.Invoke(tile, EventArgs.Empty);
                    }
                }
            }
        }

        public VECTOR GetVertexNormal(CornerType corner) {
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return new VECTOR(0f, 1 / 32768f, 0f);

            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable[bl.Num][bl.X, bl.Y];
        }

        public void SetVertexNormal(CornerType corner, VECTOR value) {
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

        public VECTOR[] GetVertexNormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexNormal(c)).ToArray();
        }
    }
}
