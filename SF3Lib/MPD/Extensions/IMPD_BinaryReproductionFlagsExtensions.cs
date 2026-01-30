using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_BinaryReproductionFlagsExtensions {
        public static string ToJSON_String(this IMPD_BinaryReproductionFlags flags)
            => flags.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_BinaryReproductionFlags flags) => flags.ToJObject();
        public static JObject ToJObject(this IMPD_BinaryReproductionFlags flags) {
            JToken JTokenIfExists(object obj) => obj != null ? JToken.FromObject(obj) : null;
            return new JObject(
                new JProperty("ShortEmptyAnimationTable",                     flags.ShortEmptyAnimationTable),
                new JProperty("PaletteAdjustmentIsTruncated",                 flags.PaletteAdjustmentIsTruncated),
                new JProperty("SkyPaletteSharesGroundPalette",                flags.SkyPaletteSharesGroundPalette),
                new JProperty("TexturePaletteSharesSkyPalette",               flags.TexturePaletteSharesSkyPalette),
                new JProperty("EmptyUnterminatedIgnoredTexturesTable",        flags.EmptyUnterminatedIgnoredTexturesTable),
                new JProperty("NonStandardTextureChunkDecompressedSizeLimit", flags.NonStandardTextureChunkDecompressedSizeLimit),
                new JProperty("MisplacedModelsChunkIndex",                    flags.MisplacedModelsChunkIndex),
                new JProperty("MisplacedSurfaceModelChunkIndex",              flags.MisplacedSurfaceModelChunkIndex),
                new JProperty("UnreferencedDataAfterPaletteAdjustmentTable",  JTokenIfExists(flags.UnreferencedDataAfterPaletteAdjustmentTable?.AsArray()))
            );
        }
    }
}
