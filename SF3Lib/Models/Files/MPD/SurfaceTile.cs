using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using SF3.Extensions;
using SF3.MPD;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;
using static SF3.Utils.SurfaceUtils;

namespace SF3.Models.Files.MPD {
    public class SurfaceTile : IMPD_SurfaceTile {
        public SurfaceTile(IMPD_File file, int x, int y) {
            MPD_File = file;
            X = x;
            Y = y;

            BlockLocation = GetTileBlockLocation(x, y);

            for (int c = 0; c < 4; ++c) {
                _sharedTileLocations[c] = GetSharedTilesAtCorner(X, Y, (CornerType) c);
                _blockVertexLocations[c] = GetVertexBlockLocations(X, Y, (CornerType) c, onlyInBlock: true)[0];
                _sharedBlockVertexLocations[c] = GetVertexBlockLocations(X, Y, (CornerType) c, onlyInBlock: false);
            }

            RandomSeed = MPD_TileSeeds.GetTileSeed(x, y);
        }

        public float GetSurfaceDataVertexHeight(CornerType corner)
            => MPD_File.SurfaceDataChunk?.HeightmapRowTable?[Y]?.GetHeight(X, corner) ?? 0.0f;

        public float GetSurfaceModelVertexHeight(CornerType corner) {
            var bl = _blockVertexLocations[(int) corner];
            return ((MPD_File.SurfaceModelChunk?.VertexHeightBlockTable?[bl.Num]?[bl.X, bl.Y]) ?? 0) / 16f;
        }

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

        public byte TextureID {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetTextureID(X) : (byte) 0xFF;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte TextureFlags {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetTextureFlags(X) : (byte) 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType TextureFlip {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetFlip(X) : 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType TextureRotate {
            get => (MPD_File.SurfaceModelChunk != null) ? MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].GetRotate(X) : 0;
            set {
                MPD_File.SurfaceModelChunk.TileTextureRowTable[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte UnknownTextureFlags {
            get {
                if (MPD_File.SurfaceModelChunk == null)
                    return 0;

                var row = MPD_File.SurfaceModelChunk.TileTextureRowTable[Y];
                var hasRotate = row.HasRotation;
                var flags = row.GetTextureFlags(X);

                return (byte) (flags & ~(hasRotate ? 0xB3 : 0xB0));
            }
            set {}
        }

        public TerrainType TerrainType {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                if (MPD_File.SurfaceDataChunk?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags TerrainFlags {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                if (MPD_File.SurfaceDataChunk?.HeightTerrainRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte EventID {
            get => (MPD_File.SurfaceDataChunk != null) ? MPD_File.SurfaceDataChunk.EventIDRowTable[Y][X] : (byte) 0;
            set {
                if (MPD_File.SurfaceDataChunk?.EventIDRowTable == null)
                    return;
                MPD_File.SurfaceDataChunk.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// If a tree is assigned, it's placed very far off the camera screen and
        /// disassociated with the tile.
        /// </summary>
        /// <returns>'true' if a tree was associated and now unassigned, otherwise 'false'.</returns>
        public bool OrphanTree() {
            // Get the tree model.
            if (!TreeModelID.HasValue || TreeModelID < 0)
                return false;

            var modelCollection = MPD_File.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcOut) ? mcOut as ModelChunk : null;
            if (modelCollection == null)
                return false;

            if (TreeModelID >= modelCollection.ModelInstanceTable.Count)
                return false;

            var model = modelCollection.ModelInstanceTable[TreeModelID.Value];

            // Simply place it really far off the map.
            model.PositionX -= 4096;

            // Do whatever we need to do to detach the tree from the tile, and return success.
            TreeModelID = null;
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
            if (MPD_File.Surface == null || !(MPD_File.Surface.GetTile(0, 0) is SurfaceTile))
                return false;

            // Get a list of all currently associated trees.
            var modelCollection = MPD_File.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcOut) ? mcOut as ModelChunk : null;
            if (modelCollection == null || modelCollection.PDataTable.Count == 0)
                return false;

            var associatedModelsList = MPD_File.Surface.GetAllTiles()
                .Cast<SurfaceTile>()
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
            model.PositionY = (short) (this.GetAverageVertexHeight() * -2.0f);
            model.PositionZ = (short) ((Y + 0.5f) * -32.0f);

            TreeModelID = model.ID;
            return true;
        }

        public IMPD_File MPD_File { get; }
        public IMPD_Surface Surface => MPD_File.Surface;
        public int X { get; }
        public int Y { get; }
        public int RandomSeed { get; private set; }
        public BlockTileLocation BlockLocation { get; }
        public int? TreeModelID { get; set; } = null;

        public event EventHandler Modified;
 
        private readonly TileAndCorner[][] _sharedTileLocations = new TileAndCorner[4][];
        private readonly BlockVertexLocation[] _blockVertexLocations = new BlockVertexLocation[4];
        private readonly BlockVertexLocation[][] _sharedBlockVertexLocations = new BlockVertexLocation[4][];
    }
}
