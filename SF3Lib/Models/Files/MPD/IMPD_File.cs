using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.Animation;
using SF3.Models.Tables.MPD.Main;
using SF3.Models.Tables.Shared;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
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
        /// Gets a reference to a palette based the palette type. If a palette does not exist, 'null' is returned.
        /// </summary>
        /// <param name="paletteType">Identifier for which palette to use.</param>
        /// <returns>A 256-color palette for the requested format, or 'null' if it doesn't exist.</returns>
        Palette GetPalette(MPD_PaletteType paletteType);

        /// <summary>
        /// Fetches the PDataModel for a PDATA that looks like a tree, if one could be found.
        /// </summary>
        /// <returns>A PDataModel reference if one with matching criteria was found. Otherwise, 'null'.</returns>
        PDataStruct GetTreePData0();

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
        /// Sets images used for rendering the ground, sky, background, and foreground planes.
        /// </summary>
        void UpdatePlaneImages();

        /// <summary>
        /// Byte data for (de)compressed data for chunks
        /// </summary>
        IChunkData[] ChunkData { get; }

        IChunkData[] ModelChunkDatas { get; }

        IChunkData SurfaceChunkData { get; }

        IChunkData[] GroundImageChunkDatas { get; }
        IChunkData[] GroundTilesetChunkDatas { get; }
        IChunkData[] GroundTileAssignmentChunkDatas { get; }
        IChunkData[] BackgroundChunkDatas { get; }

        IChunkData[] SkyChunkDatas { get; }
        IChunkData[] ForegroundTileChunkDatas { get; }
        IChunkData ForegroundTileAssignmentChunkData { get; }

        MPD_Header MPDHeader { get; }
        ChunkLocationTable ChunkLocations { get; }
        ColorTable LightPaletteColorTable { get; }
        LightPosition LightPosition { get; }
        UnknownUInt16Table Unknown1Table { get; }
        LightAdjustment LightAdjustment { get; }
        ModelSwitchGroupsTable ModelSwitchGroupsTable { get; }
        Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOffByAddr { get; }
        Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOnByAddr { get; }
        UnknownUInt8Table GroundAnimationTable { get; }
        IgnoredTextureTable IgnoredTextureTable { get; }
        ColorTable GroundPaletteColorTable { get; }
        ColorTable SkyPaletteColorTable { get; }
        ColorTable TexturePaletteColorTable { get; }
        IndexedTextureTable IndexedTextureTable { get; }
        AnimationTable Animations { get; }
        UnknownUInt16Table Unknown2Table { get; }
        GradientTable GradientTable { get; }
        BoundaryTable BoundariesTable { get; }

        int? SurfaceModelChunkIndex { get; }
        SurfaceModelChunk SurfaceModelChunk { get; }

        int[] ModelChunkIndices { get; }

        AnimationFrameChunk AnimationFrameChunk { get; }
        SurfaceDataChunk SurfaceDataChunk { get; }

        int PrimaryTextureChunksFirstIndex { get; }
        int PrimaryTextureChunksLastIndex { get; }
        int MeshTextureChunksFirstIndex { get; }
        int MeshTextureChunksLastIndex { get; }

        TextureChunk[] TextureChunks { get; }
        PlaneTileAssignmentChunk[] GroundTileAssignmentChunks { get; }
        PlaneTileAssignmentChunk ForegroundTileAssignmentChunk { get; }

        int GroundImageChunk1Index { get; }
        int GroundImageChunk2Index { get; }
        int GroundTilesetChunk1Index { get; }
        int GroundTilesetChunk2Index { get; }
        int GroundTileAssignmentChunk1Index { get; }
        int GroundTileAssignmentChunk2Index { get; }
        int BackgroundChunk1Index { get; }
        int BackgroundChunk2Index { get; }

        int SkyChunk1Index { get; }
        int SkyChunk2Index { get; }
        int ForegroundTilesetChunk1Index { get; }
        int ForegroundTilesetChunk2Index { get; }

        /// <summary>
        /// Triggered when models have been updated and something needs to be informed, like a viewer.
        /// </summary>
        EventHandler ModelsUpdated { get; set; }
    }
}
