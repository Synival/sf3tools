using System.Collections.Generic;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.ByteData;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Files.MPD {
    public class Chunk3Frame {
        public Chunk3Frame(int offset, CompressedData data) {
            Offset = offset;
            Data = data;
        }

        public int Offset { get; }
        public CompressedData Data { get; }
    };

    public interface IMPD_File : IScenarioTableFile {
        /// <summary>
        /// Recompresses compressed chunks, updating the ChunkHeader however necessary.
        /// All of the file's data will be updated to contain the new recompressed data.
        /// This could result in a different data size.
        /// </summary>
        /// <param name="onlyModified">Only perform updates for modified compressed chunks.</param>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        bool Recompress(bool onlyModified);

        /// <summary>
        /// Calculates the "abnormal" for a tile at a given corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile whose vertex abnormal should be calculated.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        /// <returns>A freshly-calculated abnormal for the vertex requested.</returns>
        VECTOR CalculateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, bool useMoreAccurateCalculations);

        /// <summary>
        /// Updates the vertex abnormal for a specific vertex of a tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile whose vertex abnormal should be updated.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        void UpdateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, bool useMoreAccurateCalculations);

        /// <summary>
        /// Updates the vertex abnormal for a specific vertex in the vertex mesh.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        void UpdateSurfaceVertexAbnormal(int vertexX, int vertexY, bool useMoreAccurateCalculations);

        /// <summary>
        /// Updates the vertex abnormals in several blocks at once.
        /// </summary>
        /// <param name="locations">The locations of blocks to update.</param>
        /// <param name="abnormal">The abnormal to be set into each block location.</param>
        void UpdateSurfaceVertexAbnormals(BlockVertexLocation[] locations, VECTOR abnormal);

        /// <summary>
        /// Recalculates vertex "abnormals" for a specific tile.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        void UpdateSurfaceVertexAbnormals(int tileX, int tileY, bool useMoreAccurateCalculations);

        /// <summary>
        /// Recalculates all vertex "abnormals" for all tiles.
        /// </summary>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        void UpdateSurfaceVertexAbnormals(bool useMoreAccurateCalculations);

        /// <summary>
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }
        IChunkData SurfaceChunkData { get; }

        MPDHeaderTable MPDHeader { get; }
        ColorTable[] TexturePalettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        ColorTable LightPalette { get; }
        LightDirectionTable LightDirectionTable { get; }
        UnknownUInt16Table Offset3Table { get; }
        Offset4Table Offset4Table { get; }

        TextureAnimationTable TextureAnimations { get; }

        /// <summary>
        /// The compressed data for each texture in Chunk3, paired with an offset.
        /// </summary>
        List<Chunk3Frame> Chunk3Frames { get; }

        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileSurfaceVertexNormalMeshBlocks TileSurfaceVertexNormalMeshBlocks { get; }
        TileSurfaceVertexHeightMeshBlocks TileSurfaceVertexHeightMeshBlocks { get; }
        TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; }
        TileHeightTerrainRowTable TileHeightTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }

        MPD_FileTextureChunk[] TextureChunks { get; }
    }
}
