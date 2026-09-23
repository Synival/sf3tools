using CommonLib.SGL;
using ModelConverter;
using SF3.Models.Files.X8PC;
using SF3.Models.Structs.X8PC;
using SF3.X8PC;

namespace X8PC_Analyzer {
    public static class MatchFuncs {
        public static string[]? HasUnexpectedValuesInTexHeader(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable) {
                var texHeader = pc.TexDefChunkHeader;
                if (texHeader.TexDefsOffset != 0x14 || texHeader.Unknown0x0C != 0x00 || texHeader.Unknown0x10 != 0x00)
                    strings.Add($"TexDefHeader: 0x{texHeader.TexDefsOffset:X2}, 0x{texHeader.Unknown0x0C:X2}, 0x{texHeader.Unknown0x10:X2}");

                var modelHeader = pc.ModelChunkHeader;
                if (modelHeader.ModelsOffset != 0x08)
                    strings.Add($"ModelHeader: 0x{modelHeader.ModelsOffset:X2}");
            }

            return strings.ToArray();
        }

        public static string[]? PrintRigs(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable) {
                strings.Add($"{pc.Name}:\n" + pc.Rig.RootBone.ToOutline());
            }

            return strings.Count > 0 ? strings.ToArray() : null;
        }

        public static string[]? HasUnassociatedXPData(IX8PC_File x8pcFile) {
            var strings = new List<string>();

            foreach (var pc in x8pcFile.PolyCharTable)
                foreach (var xpdata in pc.XPDataTables.SelectMany(x => x))
                    if (xpdata.BonePath == "(none)")
                        strings.Add($"{pc.Name}:\n" + xpdata.Name);

            return strings.Count > 0 ? strings.ToArray() : null;
        }

        public static string[]? WriteGLBs(IX8PC_File x8pcFile, string filename, bool forceLit) {
            var flags = new ModelConversionFlags() { ForceLit = forceLit };
            var converter = new ModelConverter.ModelConverter(flags);

            foreach (PolyChar pc in x8pcFile.PolyCharTable) {
                var directory = $"./PolyCharXPDatas/{x8pcFile.Scenario}/{filename}/";
                Directory.CreateDirectory(directory);
                var xpdatas = pc.XPDataTables.SelectMany(x => x).ToArray();
                var data = converter.ModelToGLB_Data(xpdatas, pc);
                File.WriteAllBytes(directory + $"{pc.ID}.glb", data);
            }
            return [];
        }

        public static string[]? WriteGLBsTwoPasses(IX8PC_File x8pcFile, string filename) {
            var converter = new ModelConverter.ModelConverter();

            foreach (PolyChar pc in x8pcFile.PolyCharTable) {
                var directory = $"./PolyCharXPDatas/{x8pcFile.Scenario}/{filename}/";
                Directory.CreateDirectory(directory);
                var input1  = pc.XPDataTables.SelectMany(x => x).ToArray();
                var output1 = converter.ModelToGLTF_ModelRoot(input1, pc);
                var input2  = converter.GLTF_ModelRootToModels(output1, null, null, null);
                var output2 = converter.ModelToGLB_Data(input2, pc);
                File.WriteAllBytes(directory + $"{pc.ID}.glb", output2);
            }
            return [];
        }

        public static string[]? GLTFsWithTwoPrimitives(IX8PC_File x8pcFile, string filename) {
            var converter = new ModelConverter.ModelConverter();

            var report = new List<string>();
            foreach (PolyChar pc in x8pcFile.PolyCharTable) {
                var xpdatas = pc.XPDataTables.SelectMany(x => x).ToArray();
                var modelRoot = converter.ModelToGLTF_ModelRoot(xpdatas, pc);
                foreach (var mesh in modelRoot.LogicalMeshes.Select((x, i) => (Mesh: x, Index: i)).Where(x => x.Mesh.Primitives.Count == 2))
                    report.Add($"{pc.ID}.{mesh.Index}");
            }
            return report.ToArray();
        }

        public static string[]? PolyCharsWithFlippingInATTRs(IX8PC_File x8pcFile, string filename) {
            var converter = new ModelConverter.ModelConverter();

            var report = new List<string>();
            foreach (PolyChar pc in x8pcFile.PolyCharTable) {
                var models = pc.XPDataTables.SelectMany(x => x).Cast<ISGL_Model>().ToArray();
                foreach (var model in models.Select((x, i) => (Model: x, Index: i)).Where(x => x.Model.Faces.Any(y => y.Attributes.HFlip || y.Attributes.VFlip)))
                    report.Add($"{pc.ID}.{model.Index}");
            }
            return report.ToArray();
        }

        public static string[]? PolyCharsWithUseLightAttrs(IX8PC_File x8pcFile, string filename) {
            var converter = new ModelConverter.ModelConverter();

            var report = new List<string>();
            foreach (PolyChar pc in x8pcFile.PolyCharTable) {
                var models = pc.XPDataTables.SelectMany(x => x).Cast<ISGL_Model>().ToArray();
                foreach (var model in models.Select((x, i) => (Model: x, Index: i)).Where(x => x.Model.Faces.Any(y => y.Attributes.UseLight)))
                    report.Add($"{pc.ID}.{model.Index}");
            }
            return report.ToArray();
        }
    }
}
