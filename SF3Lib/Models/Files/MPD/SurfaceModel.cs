using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Surface;
using SF3.Models.Tables.MPD.SurfaceModel;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD.Objects {
    public class SurfaceModel : TableFile {
        protected SurfaceModel(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            ChunkIndex = chunkIndex;
        }

        public static SurfaceModel Create(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex) {
            var newFile = new SurfaceModel(data, nameContext, address, name, chunkIndex);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (TileTextureRowTable    = TileTextureRowTable.Create   (Data, 0x0000)),
                (VertexNormalBlockTable = VertexNormalBlockTable.Create(Data, 0x2000)),
                (VertexHeightBlockTable = VertexHeightBlockTable.Create(Data, 0xB600)),
            };
        }

        /// <summary>
        /// Updates the vertex abnormal for a specific tile corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile to update.</param>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        public void UpdateVertexAbnormal(int tileX, int tileY, CornerType corner, HeightmapRowTable heightmap, POLYGON_NormalCalculationMethod calculationMethod)
            => UpdateVertexAbnormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), heightmap, calculationMethod);

        /// <summary>
        /// Updates the vertex abnormal for a specific vertex in the vertex mesh.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        public void UpdateVertexAbnormal(int vertexX, int vertexY, HeightmapRowTable heightmap, POLYGON_NormalCalculationMethod calculationMethod) {
            if (heightmap == null)
                return;

            var abnormal = heightmap.CalculateVertexAbnormal(vertexX, vertexY, calculationMethod);
            var locations = GetVertexBlockLocations(vertexX, vertexY);
            UpdateVertexAbnormals(locations, abnormal);
        }

        /// <summary>
        /// Updates the vertex abnormals in several blocks at once.
        /// </summary>
        /// <param name="locations">The locations of blocks to update.</param>
        /// <param name="abnormal">The abnormal to be set into each block location.</param>
        public void UpdateVertexAbnormals(BlockVertexLocation[] locations, VECTOR abnormal) {
            var blocks = VertexNormalBlockTable.Rows;
            for (var i = 0; i < locations.Length; i++)
                blocks[locations[i].Num][locations[i].X, locations[i].Y] = abnormal;
        }

        /// <summary>
        /// Recalculates vertex "abnormals" for a specific tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        public void UpdateVertexAbnormals(int tileX, int tileY, HeightmapRowTable heightmap, POLYGON_NormalCalculationMethod calculationMethod) {
            if (heightmap == null)
                return;

            UpdateVertexAbnormal(tileX, tileY, CornerType.TopLeft,     heightmap, calculationMethod);
            UpdateVertexAbnormal(tileX, tileY, CornerType.TopRight,    heightmap, calculationMethod);
            UpdateVertexAbnormal(tileX, tileY, CornerType.BottomRight, heightmap, calculationMethod);
            UpdateVertexAbnormal(tileX, tileY, CornerType.BottomLeft,  heightmap, calculationMethod);
        }

        /// <summary>
        /// Recalculates all vertex "abnormals" for all tiles.
        /// </summary>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        public void UpdateVertexAbnormals(HeightmapRowTable heightmap, POLYGON_NormalCalculationMethod calculationMethod) {
            if (heightmap == null)
                return;
            for (var y = 0; y < 65; y++)
                for (var x = 0; x < 65; x++)
                    UpdateVertexAbnormal(x, y, CornerType.TopLeft, heightmap, calculationMethod);
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public int? ChunkIndex { get; }

        [BulkCopyRecurse]
        public TileTextureRowTable TileTextureRowTable { get; private set; }

        [BulkCopyRecurse]
        public VertexNormalBlockTable VertexNormalBlockTable { get; private set; }

        [BulkCopyRecurse]
        public VertexHeightBlockTable VertexHeightBlockTable { get; private set; }
    }
}
