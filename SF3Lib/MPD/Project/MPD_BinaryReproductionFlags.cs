using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
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
                UnreferencedDataAfterPaletteAdjustmentTable = ((byte[]) (original.UnreferencedDataAfterPaletteAdjustmentTable.AsArray().Clone())).ToEnumerableWithLength();
        }

        public static MPD_BinaryReproductionFlags FromJToken(JToken token) => new MPD_BinaryReproductionFlags(token);
        private MPD_BinaryReproductionFlags(JToken token) {
            var jObject = (JObject) token;

            ShortEmptyAnimationTable                     = (bool) jObject["ShortEmptyAnimationTable"];
            PaletteAdjustmentIsTruncated                 = (bool) jObject["PaletteAdjustmentIsTruncated"];
            SkyPaletteSharesGroundPalette                = (bool) jObject["SkyPaletteSharesGroundPalette"];
            TexturePaletteSharesSkyPalette               = (bool) jObject["TexturePaletteSharesSkyPalette"];
            EmptyUnterminatedIgnoredTexturesTable        = (bool) jObject["EmptyUnterminatedIgnoredTexturesTable"];
            NonStandardTextureChunkDecompressedSizeLimit = (int?) jObject["NonStandardTextureChunkDecompressedSizeLimit"];
            MisplacedModelsChunkIndex                    = (int?) jObject["MisplacedModelsChunkIndex"];
            MisplacedSurfaceModelChunkIndex              = (int?) jObject["MisplacedSurfaceModelChunkIndex"];
            UnreferencedDataAfterPaletteAdjustmentTable  = jObject.GetValueIfExists("UnreferencedDataAfterPaletteAdjustmentTable",
                t => ((JArray) t).Select(x => (byte) x).ToArray().ToEnumerableWithLength());
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
