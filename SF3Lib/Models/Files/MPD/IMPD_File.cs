using System.Collections.Generic;
using CommonLib.Imaging;
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

        MPDHeaderTable MPDHeader { get; }
        ChunkHeaderTable ChunkHeader { get; }
        ColorTable LightPalette { get; }
        LightPositionTable LightPositionTable { get; }
        UnknownUInt16Table Unknown1Table { get; }
        LightAdjustmentTable LightAdjustmentTable { get; }
        ModelSwitchGroupsTable ModelSwitchGroupsTable { get; }
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
        TextureCollection[] TextureCollections { get; }

        Tile[,] Tiles { get; }

        IChunkData[] RepeatingGroundChunkData { get; }
        TwoChunkImage RepeatingGroundImage { get; }

        IChunkData[] TiledGroundChunkData { get; }
        TwoChunkImage TiledGroundTileImage { get; }
        ITexture TiledGroundImage { get; }

        IChunkData[] SkyBoxChunkData { get; }
        TwoChunkImage SkyBoxImage { get; }

        IChunkData[] BackgroundChunkData { get; }
        TwoChunkImage BackgroundImage { get; }
    }
}
