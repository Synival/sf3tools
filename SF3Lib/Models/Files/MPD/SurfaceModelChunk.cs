using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Surface;
using SF3.Models.Tables.MPD.SurfaceModel;
using SF3.Types;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public class SurfaceModelChunk : TableFile {
        protected SurfaceModelChunk(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex, ScenarioType scenario)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            ChunkIndex = chunkIndex;
            Scenario   = scenario;
        }

        public static SurfaceModelChunk Create(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex, ScenarioType scenario) {
            var newFile = new SurfaceModelChunk(data, nameContext, address, name, chunkIndex, scenario);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (TileTextureRowTable    = TileTextureRowTable.Create   (Data, "TileTextureRows", 0x0000, Scenario >= ScenarioType.Scenario3)),
                (VertexNormalBlockTable = VertexNormalBlockTable.Create(Data, "VertexNormalBlocks", 0x2000)),
                (VertexHeightBlockTable = VertexHeightBlockTable.Create(Data, "VertexHeightBlocks", 0xB600)),
            };
        }

        /// <summary>
        /// Updates the vertex normal for a specific tile corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile to update.</param>
        /// <param name="settings">Settings for normal calculations.</param>
        public void UpdateVertexNormal(int tileX, int tileY, CornerType corner, HeightmapRowTable heightmap, NormalCalculationSettings settings)
            => UpdateVertexNormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), heightmap, settings);

        /// <summary>
        /// Updates the vertex normal for a specific vertex in the vertex mesh.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="settings">Settings for normal calculations.</param>
        public void UpdateVertexNormal(int vertexX, int vertexY, HeightmapRowTable heightmap, NormalCalculationSettings settings) {
            if (heightmap == null || vertexX < 0 || vertexY < 0 || vertexX > 64 || vertexY > 64)
                return;

            var normal = heightmap.CalculateVertexNormal(vertexX, vertexY, settings);
            var locations = GetVertexBlockLocations(vertexX, vertexY);
            UpdateVertexNormals(locations, normal);
        }

        /// <summary>
        /// Updates the vertex normals in several blocks at once.
        /// </summary>
        /// <param name="locations">The locations of blocks to update.</param>
        /// <param name="normal">The normal to be set into each block location.</param>
        public void UpdateVertexNormals(BlockVertexLocation[] locations, VECTOR normal) {
            var blocks = VertexNormalBlockTable;
            for (var i = 0; i < locations.Length; i++)
                blocks[locations[i].Num][locations[i].X, locations[i].Y] = normal;
        }

        /// <summary>
        /// Recalculates vertex "normals" for a specific tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="settings">Settings for normal calculations.</param>
        public void UpdateVertexNormals(int tileX, int tileY, HeightmapRowTable heightmap, NormalCalculationSettings settings) {
            if (heightmap == null)
                return;
            foreach (var c in (CornerType[]) Enum.GetValues(typeof(CornerType)))
                UpdateVertexNormal(tileX, tileY, c, heightmap, settings);
        }

        /// <summary>
        /// Recalculates all vertex "normals" for all tiles.
        /// </summary>
        /// <param name="heightmap">The heightmap table to recalculate.</param>
        /// <param name="settings">Settings for normal calculations.</param>
        public void UpdateVertexNormals(HeightmapRowTable heightmap, NormalCalculationSettings settings) {
            if (heightmap == null)
                return;
            for (var y = 0; y < 65; y++)
                for (var x = 0; x < 65; x++)
                    UpdateVertexNormal(x, y, heightmap, settings);
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public int? ChunkIndex { get; }
        public ScenarioType Scenario { get; }

        [BulkCopyRecurse]
        public TileTextureRowTable TileTextureRowTable { get; private set; }

        [BulkCopyRecurse]
        public VertexNormalBlockTable VertexNormalBlockTable { get; private set; }

        [BulkCopyRecurse]
        public VertexHeightBlockTable VertexHeightBlockTable { get; private set; }
    }
}
