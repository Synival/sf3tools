using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_BinaryReproductionFlagsExtensions {
        public static string ToJSON_String(this IMPD_BinaryReproductionFlags flags)
            => flags.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_BinaryReproductionFlags flags) => flags.ToJObject();
        public static JObject ToJObject(this IMPD_BinaryReproductionFlags flags) {
            return new JObject {
                { "ShortEmptyAnimationTable",                     flags.ShortEmptyAnimationTable },
                { "PaletteAdjustmentIsTruncated",                 flags.PaletteAdjustmentIsTruncated },
                { "SkyPaletteSharesGroundPalette",                flags.SkyPaletteSharesGroundPalette },
                { "TexturePaletteSharesSkyPalette",               flags.TexturePaletteSharesSkyPalette },
                { "EmptyUnterminatedIgnoredTexturesTable",        flags.EmptyUnterminatedIgnoredTexturesTable },
                { "NonStandardTextureChunkDecompressedSizeLimit", flags.NonStandardTextureChunkDecompressedSizeLimit },
                { "MisplacedModelsChunkIndex",                    flags.MisplacedModelsChunkIndex },
                { "MisplacedSurfaceModelChunkIndex",              flags.MisplacedSurfaceModelChunkIndex },
                { "UnreferencedDataAfterPaletteAdjustmentTable",  flags.UnreferencedDataAfterPaletteAdjustmentTable?.AsArray()?.ToJArray() },
            };
        }
    }
}
