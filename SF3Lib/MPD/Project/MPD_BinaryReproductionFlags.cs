using CommonLib;
using CommonLib.Extensions;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_BinaryReproductionFlags : IMPD_BinaryReproductionFlags {
        public MPD_BinaryReproductionFlags() { }

        public MPD_BinaryReproductionFlags(IMPD_BinaryReproductionFlags original) {
            ShortEmptyAnimationTable                     = original.ShortEmptyAnimationTable;
            PaletteAdjustmentIsTruncated                 = original.PaletteAdjustmentIsTruncated;
            SkyPaletteSharesGroundPalette                = original.SkyPaletteSharesGroundPalette;
            TexturePaletteSharesSkyPalette               = original.TexturePaletteSharesSkyPalette;
            EmptyUnterminatedIgnoredTexturesTable        = original.EmptyUnterminatedIgnoredTexturesTable;
            NonStandardTextureChunkDecompressedSizeLimit = original.NonStandardTextureChunkDecompressedSizeLimit;
            MisplacedModelsChunkIndex                    = original.MisplacedModelsChunkIndex;
            MisplacedSurfaceModelChunkIndex              = original.MisplacedSurfaceModelChunkIndex;

            if (original.UnreferencedDataAfterPaletteAdjustmentTable != null)
                UnreferencedDataAfterPaletteAdjustmentTable = ((byte[]) (UnreferencedDataAfterPaletteAdjustmentTable.AsArray().Clone())).ToEnumerableWithLength();
        }

        public bool ShortEmptyAnimationTable { get; set; }
        public bool PaletteAdjustmentIsTruncated { get; set; }
        public bool SkyPaletteSharesGroundPalette { get; set; }
        public bool TexturePaletteSharesSkyPalette { get; set; }
        public bool EmptyUnterminatedIgnoredTexturesTable { get; set; }
        public int? NonStandardTextureChunkDecompressedSizeLimit { get; set; }
        public int? MisplacedModelsChunkIndex { get; set; }
        public int? MisplacedSurfaceModelChunkIndex { get; set; }
        public IIndexedEnumerableWithLength<byte> UnreferencedDataAfterPaletteAdjustmentTable { get; set; }
    }
}
