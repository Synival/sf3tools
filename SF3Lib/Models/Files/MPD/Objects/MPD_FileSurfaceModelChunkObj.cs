using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD.Objects {
    public class MPD_FileSurfaceModelChunkObj : TableFile {
        protected MPD_FileSurfaceModelChunkObj(IByteData data, INameGetterContext nameContext, int address, string name)
        : base(data, nameContext) {
            Address = address;
            Name    = name;
        }

        public static MPD_FileSurfaceModelChunkObj Create(IByteData data, INameGetterContext nameContext, int address, string name) {
            var newFile = new MPD_FileSurfaceModelChunkObj(data, nameContext, address, name);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (CharacterRowTable      = TileSurfaceCharacterRowTable.Create     (Data, 0x0000)),
                (VertexNormalMeshBlocks = TileSurfaceVertexNormalMeshBlocks.Create(Data, 0x2000)),
                (VertexHeightMeshBlocks = TileSurfaceVertexHeightMeshBlocks.Create(Data, 0xB600)),
            };
        }

        /// <summary>
        /// Updates the vertex abnormal for a specific tile corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile to update.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        public void UpdateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, TileSurfaceHeightmapRowTable heightmap, bool useMoreAccurateCalculations)
            => UpdateSurfaceVertexAbnormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), heightmap, useMoreAccurateCalculations);

        /// <summary>
        /// Updates the vertex abnormal for a specific vertex in the vertex mesh.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        public void UpdateSurfaceVertexAbnormal(int vertexX, int vertexY, TileSurfaceHeightmapRowTable heightmap, bool useMoreAccurateCalculations) {
            if (heightmap == null)
                return;

            var abnormal = heightmap.CalculateSurfaceVertexAbnormal(vertexX, vertexY, useMoreAccurateCalculations);
            var locations = GetBlockLocations(vertexX, vertexY);
            UpdateSurfaceVertexAbnormals(locations, abnormal);
        }

        /// <summary>
        /// Updates the vertex abnormals in several blocks at once.
        /// </summary>
        /// <param name="locations">The locations of blocks to update.</param>
        /// <param name="abnormal">The abnormal to be set into each block location.</param>
        public void UpdateSurfaceVertexAbnormals(BlockVertexLocation[] locations, VECTOR abnormal) {
            var blocks = VertexNormalMeshBlocks.Rows;
            for (var i = 0; i < locations.Length; i++)
                blocks[locations[i].Num][locations[i].X, locations[i].Y] = abnormal;
        }

        /// <summary>
        /// Recalculates vertex "abnormals" for a specific tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        public void UpdateSurfaceVertexAbnormals(int tileX, int tileY, TileSurfaceHeightmapRowTable heightmap, bool useMoreAccurateCalculations) {
            if (heightmap == null)
                return;

            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.TopLeft,     heightmap, useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.TopRight,    heightmap, useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.BottomRight, heightmap, useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.BottomLeft,  heightmap, useMoreAccurateCalculations);
        }

        /// <summary>
        /// Recalculates all vertex "abnormals" for all tiles.
        /// </summary>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        public void UpdateSurfaceVertexAbnormals(TileSurfaceHeightmapRowTable heightmap, bool useMoreAccurateCalculations) {
            if (heightmap == null)
                return;
            for (var y = 0; y < 65; y++)
                for (var x = 0; x < 65; x++)
                    UpdateSurfaceVertexAbnormal(x, y, CornerType.TopLeft, heightmap, useMoreAccurateCalculations);
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }

        [BulkCopyRecurse]
        public TileSurfaceCharacterRowTable CharacterRowTable { get; private set; }

        [BulkCopyRecurse]
        public TileSurfaceVertexNormalMeshBlocks VertexNormalMeshBlocks { get; private set; }

        [BulkCopyRecurse]
        public TileSurfaceVertexHeightMeshBlocks VertexHeightMeshBlocks { get; private set; }
    }
}
