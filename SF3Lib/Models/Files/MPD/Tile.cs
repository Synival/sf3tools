using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public class Tile {
        public Tile(IMPD_File mpdFile, int x, int y) {
            MPD_File = mpdFile;
            X = x;
            Y = y;
            BlockLocation = GetTileBlockLocation(x, y);
            BlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, true)[0]);
            SharedBlockVertexLocations = ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .ToDictionary(c => c, c => GetVertexBlockLocations(X, Y, c, false));
        }

        private void TriggerNeighborTileModified(int offsetX, int offsetY) {
            var x = X + offsetX;
            var y = Y + offsetY;

            if (x >= 0 && y >= 0 && x < 64 && y < 64) {
                var tile = MPD_File.Tiles[x, y];
                tile.Modified?.Invoke(tile, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Updates all normals for the tile, also correcting neighboring tiles.
        /// </summary>
        /// <param name="halfHeight">When on (default, SF3 behavior), quad heights are halved for the purpose of normal calculations.</param>
        public void UpdateNormals(bool halfHeight = true) {
            MPD_File.SurfaceModel?.UpdateVertexNormals(
                X, Y,
                MPD_File.Surface.HeightmapRowTable,
                POLYGON_NormalCalculationMethod.WeightedVerticalTriangles,
                halfHeight
            );

            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                    TriggerNeighborTileModified(x, y);
        }

        public float[] GetSurfaceModelVertexHeights() {
            // For any tile whose character/texture ID has flag 0x80, the bottom-right corner of the walking heightmap is used.
            if (MPD_File.Surface?.HeightmapRowTable != null && MPD_File.SurfaceModel?.TileTextureRowTable != null && ModelIsFlat) {
                var brHeight = MPD_File.Surface.HeightmapRowTable[Y].GetHeight(X, CornerType.BottomRight);
                return new float[] { brHeight, brHeight, brHeight, brHeight };
            }

            if (MPD_File.Surface == null)
                return new float[] { 0, 0, 0, 0 };

            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return MPD_File.Surface.HeightmapRowTable[Y].GetQuadHeights(X);

            return BlockVertexLocations.Values
                .Select(x => MPD_File.SurfaceModel.VertexHeightBlockTable[x.Num][x.X, x.Y] / 16.0f)
                .ToArray();
        }

        public VECTOR GetVertexNormal(CornerType corner) {
            if (MPD_File.SurfaceModel?.VertexNormalBlockTable == null)
                return new VECTOR(0f, 1 / 32768f, 0f);

            var loc = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable[loc.Num][loc.X, loc.Y];
        }

        public VECTOR[] GetVertexNormals() {
            return ((CornerType[]) Enum.GetValues(typeof(CornerType)))
                .Select(c => GetVertexNormal(c)).ToArray();
        }

        public IMPD_File MPD_File { get; }
        public int X { get; }
        public int Y { get; }
        public BlockTileLocation BlockLocation { get; }
        public Dictionary<CornerType, BlockVertexLocation> BlockVertexLocations { get; }
        public Dictionary<CornerType, BlockVertexLocation[]> SharedBlockVertexLocations { get; }

        public TerrainType MoveTerrainType {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainType(X) : 0;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainType(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TerrainFlags MoveTerrainFlags {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetTerrainFlags(X) : 0;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetTerrainFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float MoveHeight {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.HeightTerrainRowTable[Y].GetHeight(X) : 0.0f;
            set {
                MPD_File.Surface.HeightTerrainRowTable[Y].SetHeight(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetMoveHeightmap(CornerType corner)
            => (MPD_File.Surface != null) ? MPD_File.Surface.HeightmapRowTable[Y].GetHeight(X, corner) : 0.0f;
        public void SetMoveHeightmap(CornerType corner, float value) {
            MPD_File.Surface.HeightmapRowTable[Y].SetHeight(X, corner, value);
            Modified?.Invoke(this, EventArgs.Empty);
        }

        public byte EventID {
            get => (MPD_File.Surface != null) ? MPD_File.Surface.EventIDRowTable[Y][X] : (byte) 0;
            set {
                MPD_File.Surface.EventIDRowTable[Y][X] = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureID {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureID(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureID(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public byte ModelTextureFlags {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetTextureFlags(X) : (byte) 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetTextureFlags(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureFlipType ModelTextureFlip {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetFlip(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetFlip(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public TextureRotateType ModelTextureRotate {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetRotate(X) : 0;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetRotate(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ModelIsFlat {
            get => (MPD_File.SurfaceModel != null) ? MPD_File.SurfaceModel.TileTextureRowTable[Y].GetIsFlatFlag(X) : false;
            set {
                MPD_File.SurfaceModel.TileTextureRowTable[Y].SetIsFlatFlag(X, value);
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        public int? TreeModelChunkIndex { get; set; } = null;
        public int? TreeModelID { get; set; } = null;

        public float GetModelVertexHeightmap(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexHeightBlockTable[bl.Num][bl.X, bl.Y] / 16f;
        }

        public void SetModelVertexHeightmap(CornerType corner, float value) {
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

        public VECTOR GetModelVertexNormal(CornerType corner) {
            var bl = BlockVertexLocations[corner];
            return MPD_File.SurfaceModel.VertexNormalBlockTable[bl.Num][bl.X, bl.Y];
        }

        public void SetModelVertexNormal(CornerType corner, VECTOR value) {
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

        public float GetAverageHeight()
            => (float) Math.Round(((CornerType[]) Enum.GetValues(typeof(CornerType))).Average(x => GetMoveHeightmap(x) * 16f)) / 16f;

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

        public void CopyMoveHeightToNonFlatNeighbors(CornerType corner) {
            var height = GetMoveHeightmap(corner);
            var adjacentCorners = GetAdjacentTilesAtCorner(corner);
            foreach (var ac in adjacentCorners) {
                var acTile = MPD_File.Tiles[ac.X, ac.Y];
                if (!acTile.ModelIsFlat) {
                    acTile.SetMoveHeightmap(ac.Corner, height);
                    acTile.MoveHeight = acTile.GetAverageHeight();
                }
            }
        }

        public void UpdateNormalsForCorner(CornerType corner, bool halfHeight = true) {
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
                        surfaceModel.UpdateVertexNormal(vx, vy, heightmapRowTable, POLYGON_NormalCalculationMethod.WeightedVerticalTriangles, halfHeight);
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

        /// <summary>
        /// If a tree is assigned, it's placed very far off the camera screen and
        /// disassociated with the tile.
        /// </summary>
        /// <returns>'true' if a tree was associated and now unassigned, otherwise 'false'.</returns>
        public bool OrphanTree() {
            // Get the tree model.
            if (!TreeModelID.HasValue || !TreeModelChunkIndex.HasValue || TreeModelID < 0)
                return false;

            var modelCollection = MPD_File.ModelCollections.FirstOrDefault(x => x.ChunkIndex == TreeModelChunkIndex.Value);
            if (modelCollection == null)
                return false;

            if (TreeModelID >= modelCollection.ModelTable.Length)
                return false;

            var model = modelCollection.ModelTable[TreeModelID.Value];

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

            // Get a list of all currently associated trees.
            var chunkIndex = MPD_File.MPDHeader.ModelsChunkIndex;
            if (!chunkIndex.HasValue)
                return false;

            var modelCollection = MPD_File.ModelCollections.FirstOrDefault(x => x.ChunkIndex == chunkIndex.Value);
            if (modelCollection == null || modelCollection.PDataTable.Length == 0)
                return false;

            var associatedModelsList = MPD_File.Tiles.To1DArray()
                .Where(x => x.TreeModelID.HasValue)
                .Select(x => x.TreeModelID.Value)
                .ToList();

            var associatedModels = new HashSet<int>(associatedModelsList);

            // Get the PDATA that represents a tree.
            var pdata = GetTreePData0(modelCollection);
            if (pdata == null)
                return false;

            // Look for any tree that doesn't have an assigned tile.
            var model = modelCollection.ModelTable
                .FirstOrDefault(x => x.PData0 == pdata.RamAddress && x.AlwaysFacesCamera && !associatedModels.Contains(x.ID));
            if (model == null)
                return false;

            // We found a tree, so let's position it, associate it, and return success.
            model.PositionX = (short) ((X + 0.5f) * -32.0f);
            model.PositionY = (short) (GetAverageHeight() * -32.0f);
            model.PositionZ = (short) ((Y + 0.5f) * -32.0f);

            TreeModelID = model.ID;
            TreeModelChunkIndex = chunkIndex;
            return true;
        }

        private PDataModel GetTreePData0(ModelCollection mc) {
            // Look for the first PDATA with one polygon that uses the tree texture (seems to always be 0).
            return mc.PDataTable.FirstOrDefault(x => {
                if (x.PolygonCount != 1)
                    return false;

                var attr = mc.AttrTablesByMemoryAddress[x.AttributesOffset][0];
                return attr.HasTexture && attr.TextureNo == 0;
            });
        }

        public EventHandler Modified;
    }
}
