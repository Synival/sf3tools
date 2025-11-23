using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.MPD;

namespace SF3.Models.Files.MPD {
    public class Chunk3Frame {
        public Chunk3Frame(int offset, CompressedData data) {
            Offset = offset;
            Data = data;
        }

        public int Offset { get; }
        public CompressedData Data { get; }
    };

    public struct ReplaceTexturesFromFilesResult {
        public int Replaced;
        public int Missing;
        public int Skipped;
        public int Failed;
    }

    public struct ExportTexturesToPathResult {
        public int Exported;
        public int Skipped;
        public int Failed;
    }

    public interface IMPD_File : IScenarioTableFile, IMPD {
        /// <summary>
        /// Recompresses compressed chunks.
        /// </summary>
        /// <param name="onlyModified">Only perform updates for modified compressed chunks.</param>
        void RecompressChunks(bool onlyModified);

        /// <summary>
        /// Rewrites the chunk offset and size table to reflect contents in ChunkData[].
        /// </summary>
        void RebuildChunkTable();

        /// <summary>
        /// Writes all chunk data to the main data.
        /// </summary>
        void CommitChunks();

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
        /// Fetches the PDataModel for a PDATA that looks like a tree, if one could be found.
        /// </summary>
        /// <returns>A PDataModel reference if one with matching criteria was found. Otherwise, 'null'.</returns>
        PDataModel GetTreePData0();

        /// <summary>
        /// Replaces a set of textures based on appropriately named files (e.g, Texture_0A.png).
        /// </summary>
        /// <param name="files">A list of files named by their texture in format "Texture_{HexID:X2}".</param>
        /// <param name="abgr1555ImageDataLoader">Function to convert the filename provided to image data.</param>
        /// <returns>Returns a summary report of the textures replaced.</returns>
        ReplaceTexturesFromFilesResult ReplaceTexturesFromFiles(string[] files, Func<string, ushort[,]> abgr1555ImageDataLoader);

        /// <summary>
        /// Exports all textures to a path with an appropriate filename (e.g, Texture_0A.png).
        /// </summary>
        /// <param name="path">Path to which all textures should be exported.</param>
        /// <param name="abgr1555ImageDataWriter">Function to save the ABGR1555 image data to a file.</param>
        /// <returns>Returns a summary report of the textures exported.</returns>
        ExportTexturesToPathResult ExportTexturesToPath(string path, Action<string, ushort[,]> abgr1555ImageDataWriter);

        /// <summary>
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }
        IChunkData[] ModelsChunkData { get; }
        IChunkData SurfaceChunkData { get; }

        MPDHeaderModel MPDHeader { get; }
        ChunkLocationTable ChunkLocations { get; }
        ColorTable LightPalette { get; }
        LightPosition LightPosition { get; }
        UnknownUInt16Table Unknown1Table { get; }
        LightAdjustmentModel LightAdjustment { get; }
        ModelSwitchGroupsTable ModelSwitchGroupsTable { get; }
        Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOffByAddr { get; }
        Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOnByAddr { get; }
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
        List<ITexture> Textures { get; }

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
