using CommonLib;
using SF3.RawEditors;
using SF3.TableModels;
using SF3.TableModels.MPD;
using SF3.TableModels.MPD.TextureAnimation;

namespace SF3.FileModels.MPD {
    public interface IMPD_Editor : IScenarioTableEditor {
        /// <summary>
        /// Recompresses compressed chunks, updating the ChunkHeader however necessary.
        /// All of the editor's data will be updated to contain the new recompressed data.
        /// This could result in a different data size.
        /// </summary>
        /// <param name="onlyModified">Only perform updates for modified compressed chunks.</param>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        bool Recompress(bool onlyModified);

        /// <summary>
        /// Byte editor for (de)compressed data for chunks
        /// </summary>
        IChunkEditor[] ChunkEditors { get; }
        IChunkEditor SurfaceChunkEditor { get; }

        Tables.MPD.HeaderTable Header { get; }
        ColorTable[] Palettes { get; }
        ChunkHeaderTable ChunkHeader { get; }
        UnknownUInt16Table Offset1Table { get; }
        UnknownUInt32Table Offset2Table { get; }
        UnknownUInt16Table Offset3Table { get; }
        Offset4Table Offset4Table { get; }

        TextureAnimationTable TextureAnimations { get; }
        FrameTable TextureAnimFrames { get; }
        CompressedEditor[] TextureAnimFrameEditors { get; }

        Chunk[] Chunks { get; }

        Chunk SurfaceChunk { get; }
        TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; }
        TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; }
        TileHeightTerrainRowTable TileHeightTerrainRows { get; }
        TileItemRowTable TileItemRows { get; }

        TextureChunkEditor[] TextureChunks { get; }
    }
}
