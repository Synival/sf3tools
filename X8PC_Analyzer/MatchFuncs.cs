using SF3.Models.Files.X8PC;

namespace X8PC_Analyzer {
    public static class MatchFuncs {
        public static string[]? HasUnexpectedValuesInTexHeader(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable) {
                var texHeader = pc.TexDefChunkHeader;
                if (texHeader.TexDefsOffset != 0x14 || texHeader.Unknown0x0C != 0x00 || texHeader.Unknown0x10 != 0x00)
                    strings.Add($"0x{texHeader.TexDefsOffset:X2}, 0x{texHeader.Unknown0x0C:X2}, 0x{texHeader.Unknown0x10:X2}");
            }

            return strings.ToArray();
        }
    }
}
