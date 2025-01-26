using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;

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
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }
        IChunkData ModelsChunkData { get; }
        IChunkData SurfaceChunkData { get; }

        MPDHeaderTable MPDHeader { get; }
        TextureAnimationAltTable TextureAnimationsAlt { get; }
        ColorTable[] TexturePalettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        ColorTable LightPalette { get; }
        LightPositionTable LightPositionTable { get; }
        UnknownUInt16Table Offset3Table { get; }
        Offset4Table Offset4Table { get; }
        UnknownUInt8Table Offset7Table { get; }
        TextureAnimationTable TextureAnimations { get; }
        BoundaryTable BoundariesTable { get; }

        /// <summary>
        /// The compressed data for each texture in Chunk3, paired with an offset.
        /// </summary>
        List<Chunk3Frame> Chunk3Frames { get; }

        int? SurfaceModelChunkIndex { get; }
        SurfaceModel SurfaceModel { get; }

        int? ModelsChunkIndex { get; }
        Models Models { get; }

        Surface Surface { get; }
        TextureCollection[] TextureCollections { get; }

        Tile[,] Tiles { get; }
    }
}
