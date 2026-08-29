using SF3.Models.Files.X8PC;
using SF3.X8PC;

namespace X8PC_Analyzer {
    public static class MatchFuncs {
        public static string[]? HasUnexpectedValuesInTexHeader(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable) {
                {
                    var texHeader = pc.TexDefChunkHeader;
                    if (texHeader.TexDefsOffset != 0x14 || texHeader.Unknown0x0C != 0x00 || texHeader.Unknown0x10 != 0x00)
                        strings.Add($"TexDefHeader: 0x{texHeader.TexDefsOffset:X2}, 0x{texHeader.Unknown0x0C:X2}, 0x{texHeader.Unknown0x10:X2}");
                }

                {
                    var modelHeader = pc.ModelChunkHeader;
                    if (modelHeader.ModelsOffset != 0x08)
                        strings.Add($"ModelHeader: 0x{modelHeader.ModelsOffset:X2}");
                }
            }

            return strings.ToArray();
        }

        public static string[]? PrintSkeletons(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable) {
                var parser = new SkeletonParser(pc.ModelChunk.DecompressedData);
                var skeleton = parser.Parse((int) pc.ModelChunkHeader.SkeletonOffset);
                strings.Add($"{pc.Name}:\n" + skeleton.ToOutline());
            }

            return strings.Count > 0 ? strings.ToArray() : null;
        }
    }
}
