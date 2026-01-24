using System.Linq;
using CommonLib;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPD_BinaryReproductionFlags : IMPD_BinaryReproductionFlags {
        public MPD_BinaryReproductionFlags(IMPD_File file) {
            MPD_File = file;
        }

        public IMPD_File MPD_File { get; }

        public bool ShortEmptyAnimationTable {
            get {
                if (MPD_File.Scenario < ScenarioType.Scenario2)
                    return MPD_File.MPDHeader.OffsetUnknown2 - MPD_File.MPDHeader.OffsetAnimations == 0x02;
                else
                    return MPD_File.MPDHeader.OffsetGradient - MPD_File.MPDHeader.OffsetAnimations == 0x02;
            }
            set {}
        }

        public bool PaletteAdjustmentIsTruncated {
            get => MPD_File.PaletteAdjustment?.IsTruncated ?? false;
            set {}
        }

        public bool SkyPaletteSharesGroundPalette {
            get => MPD_File.MPDHeader.OffsetSkyPalette == MPD_File.MPDHeader.OffsetGroundPalette;
            set {}
        }

        public bool TexturePaletteSharesSkyPalette {
            get => MPD_File.MPDHeader.OffsetTexturePalette == MPD_File.MPDHeader.OffsetSkyPalette;
            set {}
        }

        public bool EmptyUnterminatedIgnoredTexturesTable {
            get => MPD_File.MPDHeader.OffsetIgnoredTextures == MPD_File.MPDHeader.OffsetGroundPalette;
            set {}
        }

        public int? NonStandardTextureChunkDecompressedSizeLimit {
            get {
                var max = MPD_File.TextureChunks.Max(x => x.Data.Length);
                return max > 0xFFFF ? max : (int?) null;
            }
            set {}
        }

        public int? MisplacedModelsChunkIndex {
            get {
                var index = ((ModelChunk) MPD_File.ModelCollections.Values.FirstOrDefault(x => (x as ModelChunk)?.Collection == MPD_CollectionType.Primary))?.ChunkIndex;
                return (index != MPD_File.Flags.ModelsChunkIndex) ? index : null;
            }
            set {}
        }

        public int? MisplacedSurfaceModelChunkIndex {
            get {
                var index = MPD_File.SurfaceModelChunkData?.Index;
                return (index != MPD_File.Flags.SurfaceModelChunkIndex) ? index : null;
            }
            set {}
        }

        public IIndexedEnumerableWithLength<byte> UnreferencedDataAfterPaletteAdjustmentTable {
            get => MPD_File.UnreferencedDataAfterPaletteAdjustmentTable;
            set {}
        }
    }
}
