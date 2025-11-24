using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using SF3.Extensions;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {

        /// <summary>
        /// If a tree is assigned, it's placed very far off the camera screen and
        /// disassociated with the tile.
        /// </summary>
        /// <returns>'true' if a tree was associated and now unassigned, otherwise 'false'.</returns>
        public bool OrphanTree() {
            // Get the tree model.
            if (!TreeModelID.HasValue || !TreeModelChunkIndex.HasValue || TreeModelID < 0)
                return false;

            var modelCollection = MPD_File.ModelCollections.TryGetValue(CollectionType.Primary, out var mcOut) ? (ModelChunk) mcOut : null;
            if (modelCollection == null)
                return false;

            if (TreeModelID >= modelCollection.ModelInstanceTable.Length)
                return false;

            var model = modelCollection.ModelInstanceTable[TreeModelID.Value];

            // Simply place it really far off the map.
            model.PositionX -= 4096;

            // Do whatever we need to do to detach the tree from the tile, and return success.
            TreeModelID = null;
            TreeModelChunkIndex = null;
            return true;
        }

        /// <summary>
        /// Looks for a tree that hasn't been assigned a tile and, if available,
        /// positions it to the tile and associates it. Does nothing if a tree is
        /// already associated.
        /// </summary>
        /// <returns>'true' if a new tree was associated and moved, otherwise 'false'.</returns>
        public bool AdoptTree() {
            // Do nothing if this tile already has a tree.
            if (TreeModelID.HasValue)
                return false;

            // Do nothing unless the IMPD_File's tiles are file-based tiles.
            // (We have better methods otherwise)
            if (MPD_File.Surface == null || !(MPD_File.Surface.GetTile(0, 0) is Tile))
                return false;

            // Get a list of all currently associated trees.
            var chunkIndex = MPD_File.Flags.Chunk20IsModels ? 20 : 1;
            var modelCollection = MPD_File.ModelCollections.TryGetValue(CollectionType.Primary, out var mcOut) ? (ModelChunk) mcOut : null;
            if (modelCollection == null || modelCollection.PDataTable.Length == 0)
                return false;

            var associatedModelsList = MPD_File.Surface.GetAllTiles()
                .Cast<Tile>()
                .Where(x => x.TreeModelID.HasValue)
                .Select(x => x.TreeModelID.Value)
                .ToList();

            var associatedModels = new HashSet<int>(associatedModelsList);

            // Get the PDATA that represents a tree.
            var pdata = MPD_File.GetTreePData0();
            if (pdata == null)
                return false;

            // Look for any tree that doesn't have an assigned tile.
            var model = modelCollection.ModelInstanceTable
                .FirstOrDefault(x => x.PData0 == pdata.RamAddress && x.AlwaysFacesCamera && !associatedModels.Contains(x.ID));
            if (model == null)
                return false;

            // We found a tree, so let's position it, associate it, and return success.
            model.PositionX = (short) ((X + 0.5f) * -32.0f);
            model.PositionY = (short) (this.GetAverageVertexHeight() * -32.0f);
            model.PositionZ = (short) ((Y + 0.5f) * -32.0f);

            TreeModelID = model.ID;
            TreeModelChunkIndex = chunkIndex;
            return true;
        }

        public int? TreeModelChunkIndex { get; set; } = null;
        public int? TreeModelID { get; set; } = null;
    }
}
