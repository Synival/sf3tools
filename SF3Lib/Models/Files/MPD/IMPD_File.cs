using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
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
        /// Scans the model collections for trees and associates them with specific tiles.
        /// </summary>
        void AssociateTilesWithTrees();

        /// <summary>
        /// Removes all associations between tree models and tiles.
        /// </summary>
        void ResetTileTrees();

        /// <summary>
        /// Creates a palette using PaletteTable[index]. If a palette does not exist, a 256 grayscale palette is returned.
        /// </summary>
        /// <param name="index">Corresponding zero-indexed palette number, with 0 as Palette1.</param>
        /// <returns>A 256-color palette for the requested index.</returns>
        Palette CreatePalette(int index);

        /// <summary>
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }
        IChunkData[] ModelsChunkData { get; }
        IChunkData SurfaceChunkData { get; }

        MPDHeaderModel MPDHeader { get; }
        ChunkHeaderTable ChunkHeader { get; }
        ColorTable LightPalette { get; }
        LightPosition LightPosition { get; }
        UnknownUInt16Table Unknown1Table { get; }
        LightAdjustmentModel LightAdjustment { get; }
        ModelSwitchGroupsTable ModelSwitchGroupsTable { get; }
        Dictionary<int, ModelIDTable> ModelsEnabledGroupsByAddr { get; }
        Dictionary<int, ModelIDTable> ModelsDisabledGroupsByAddr { get; }
        UnknownUInt8Table GroundAnimationTable { get; }
        TextureIDTable TextureAnimationsAlt { get; }
        ColorTable[] PaletteTables { get; }
        TextureIDTable IndexedTextureTable { get; }
        TextureAnimationTable TextureAnimations { get; }
        UnknownUInt16Table Unknown2Table { get; }
        GradientTable GradientTable { get; }
        BoundaryTable BoundariesTable { get; }

        /// <summary>
        /// The compressed data for each texture in Chunk3, paired with an offset.
        /// </summary>
        List<Chunk3Frame> Chunk3Frames { get; }

        int? SurfaceModelChunkIndex { get; }
        SurfaceModel SurfaceModel { get; }

        int[] ModelsChunkIndices { get; }
        ModelCollection[] ModelCollections { get; }

        Surface Surface { get; }

        int PrimaryTextureChunksFirstIndex { get; }
        int PrimaryTextureChunksLastIndex { get; }
        int MeshTextureChunksFirstIndex { get; }
        int MeshTextureChunksLastIndex { get; }
        int? ExtraModelTextureChunkIndex { get; }

        TextureCollection[] TextureCollections { get; }

        Tile[,] Tiles { get; }

        int RepeatingGroundChunk1Index { get; }
        int RepeatingGroundChunk2Index { get; }

        IChunkData[] RepeatingGroundChunks { get; }
        ITexture RepeatingGroundImage { get; }

        int TiledGroundTileChunk1Index { get; }
        int TiledGroundTileChunk2Index { get; }
        int TiledGroundMapChunks1Index { get; }
        int TiledGroundMapChunks2Index { get; }

        IChunkData[] TiledGroundTileChunks { get; }
        IChunkData[] TiledGroundMapChunks { get; }
        ITexture TiledGroundTileImage { get; }
        ITexture TiledGroundImage { get; }

        int SkyBoxChunk1Index { get; }
        int SkyBoxChunk2Index { get; }

        IChunkData[] SkyBoxChunks { get; }
        ITexture SkyBoxImage { get; }

        int BackgroundChunk1Index { get; }
        int BackgroundChunk2Index { get; }

        IChunkData[] BackgroundChunks { get; }
        ITexture BackgroundImage { get; }

        int ForegroundTileChunk1Index { get; }
        int ForegroundTileChunk2Index { get; }

        IChunkData[] ForegroundTileChunks { get; }
        IChunkData ForegroundMapChunk { get; }
        ITexture ForegroundTileImage { get; }
        ITexture ForegroundImage { get; }

        /// <summary>
        /// Triggered when models have been updated and something needs to be informed, like a viewer.
        /// </summary>
        EventHandler ModelsUpdated { get; set; }
    }
}
