using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;
using SF3.Extensions;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        private struct TreeModelInfo {
            public ModelChunk ModelCollection;
            public MPD_ModelInstance ModelInstance;
            public VECTOR TilePosition;
            public SurfaceTile Tile;
            public float Distance;
        }

        public void AssociateTilesWithTrees() {
            ResetTileTrees();

            // Gather a list of models that appear to be trees, associated with their tile.
            var treeModels = new List<TreeModelInfo>();
            foreach (var imc in ModelCollections.Values) {
                var mc = imc as ModelChunk;
                if (mc == null || !mc.ChunkIndex.HasValue)
                    continue;

                foreach (var model in mc.ModelInstanceTable) {
                    try {
                        // Trees always face the camera.
                        if (!model.AlwaysFacesCamera)
                            continue;

                        // Look into the model...
                        var pdataAddr = model.PData0;
                        if (!mc.PDatasByMemoryAddress.ContainsKey(pdataAddr))
                            continue;
                        var pdata = mc.PDatasByMemoryAddress[pdataAddr];

                        // Trees always have one polygon.
                        var firstAttrAddr = pdata.AttributesOffset;
                        if (firstAttrAddr == 0 || !mc.AttrTablesByMemoryAddress.ContainsKey(firstAttrAddr))
                            continue;
                        var attr = mc.AttrTablesByMemoryAddress[firstAttrAddr];
                        if (attr.Count != 1)
                            continue;

                        // Get the tile at its location. Skip it if it's out of bounds.
                        var tilePosition = new VECTOR(model.PositionX / -32.0f - 0.5f, model.PositionY / -32.0f, model.PositionZ / -32.0f - 0.5f);
                        int tileX = (int) Math.Round(tilePosition.X.Float);
                        int tileZ = (int) Math.Round(tilePosition.Z.Float);
                        if (tileX < 0 || tileX >= 64 || tileZ < 0 || tileZ >= 64)
                            continue;

                        var tile = Surface.GetTile(tileX, tileZ) as SurfaceTile;
                        if (tile == null)
                            continue;
                        var tileY = tile.GetAverageVertexHeight() / 16.0f;

                        // Trees should be very close to the center of the tile vertically.
                        var distance = (new VECTOR(tileX, tileY, tileZ) - tilePosition).GetLength();
                        if (Math.Abs(tileY - tilePosition.Y.Float) > 0.25f)
                            continue;

                        // Looks like a tree -- add it to the list.
                        treeModels.Add(new TreeModelInfo() {
                            ModelCollection = mc,
                            ModelInstance = model,
                            TilePosition = tilePosition,
                            Tile = tile,
                            Distance = distance
                        });
                    }
                    catch {
                        // TODO: what to do in this case??
                    }
                }
            }

            // Sort trees by their distance to their tile, from closest to farthest.
            // This can be considered their accuracy.
            var mostAccurateTreesForTile = treeModels
                .OrderBy(x => x.Distance)
                .ToArray();

            foreach (var tree in mostAccurateTreesForTile) {
                // Skip tiles already accounted for by more accurate trees.
                if (tree.Tile.TreeModelID.HasValue)
                    continue;
                tree.Tile.TreeModelID = tree.ModelInstance.ID;
            }
        }

        public void ResetTileTrees() {
            foreach (var tile in Surface.GetAllTiles())
                if (tile is SurfaceTile fileTile)
                    fileTile.TreeModelID = null;
        }

        public MPD_SGL_Model_PDataStruct GetTreePData0() {
            var mc = ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcOut) ? mcOut as ModelChunk : null;
            if (mc == null)
                return null;

            // Look for the first PDATA with one polygon that uses the tree texture (usually 0, but not always).
            return mc.PDataTable.FirstOrDefault(x => {
                if (x.FaceCount != 1)
                    return false;

                var attr = mc.AttrTablesByMemoryAddress[x.AttributesOffset][0];
                return attr.UseTexture;
            });
        }
    }
}
