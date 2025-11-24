using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs;
using SF3.Models.Structs.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace MPD_DataSearcher {
    public class Program {
        // ,--- Enter the paths for all your MPD files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        private static readonly Dictionary<ScenarioType, HashSet<string>> UnusedMaps = new() {
            { ScenarioType.Scenario1, [
                "FIELD",
                "HNSN00",
                "MGMA00",
                "MUHASI",
                "NASU00",
                "SHIO00",
                "SHIP2",
                "TESTMAP",
                "TNKA00",
                "TORI00",
                "TREE00",
                "TURI00",
            ]},
            { ScenarioType.Scenario3, [
                "AS_OKU",
                "BTL42",
                "FIELD",
                "MUHASI",
                "SNIOKI",
                "YSKI00",
                "VOID3",
            ]}
        };

        private class DataRange {
            public DataRange(IByteArray data, string? name, int start, int end) {
                Data  = data;
                Name  = name;
                Start = start;
                End   = end;
            }

            public IByteArray Data;
            public string? Name;
            public int Start;
            public int End;

            public int Size => End - Start;

            public string RangeString => (End == Start + 1)
                ? $"[0x{Start:X4}]"
                : $"[0x{Start:X4} - 0x{(End - 1):X4}]";

            public override string ToString() => (Name == null)
                ? RangeString
                : $"{Name} {RangeString}";
        }

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];
                var unusedMaps = UnusedMaps.TryGetValue(scenario, out var val) ? val : [];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Skip maps that aren't used at all.
                    if (unusedMaps.Contains(filename))
                        continue;

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts, scenario)) {
                            var mpdFileData = mpdFile.Data.Data;

                            var fileStr = GetFileString(scenario, file, mpdFile);
                            Console.WriteLine(fileStr);

                            var dataRanges = new List<DataRange>();
                            void AddDataRange(string name, IByteArray data, int addr, int size) {
                                while (data is ByteArraySegment dataSeg) {
                                    addr += dataSeg.Offset;
                                    data = dataSeg.ParentArray;
                                }
                                dataRanges.Add(new DataRange(data, name, addr, addr + size));
                            }

                            foreach (var table in mpdFile.Tables) {
                                if (table.IsContiguous)
                                    AddDataRange(table.Name, table.Data.Data, table.Address, table.SizeInBytesPlusTerminator);
                                else
                                    foreach (var e in table.RowObjs)
                                        AddDataRange(e.Name, e.Data.Data, e.Address, e.Size);
                            }

                            dataRanges.AddRange(
                                new List<IStruct>() {
                                    mpdFile?.MPDHeader!,
                                    mpdFile?.LightPosition!,
                                    mpdFile?.LightAdjustment!,
                                }
                                .Where(x => x != null)
                                .Select(x => new DataRange(mpdFileData, x.Name, x.Address, x.Address + x.Size))
                            );

                            foreach (var imc in mpdFile!.ModelCollections) {
                                var mc = (ModelCollection) imc;

                                var mh = mc.ModelsHeader;
                                if (mh != null)
                                    AddDataRange(mh.Name, mh.Data.Data, mh.Address, mh.Size);

                                var clh = mc.CollisionLinesHeader;
                                if (clh != null)
                                    AddDataRange(clh.Name, clh.Data.Data, clh.Address, clh.Size);
                            }

                            // Add some extra untracked pointers.
                            dataRanges.Add(new DataRange(mpdFileData, "Header**", 0, 4));
                            var subPointer = mpdFile.Data.GetDouble(0) - 0x290000;
                            dataRanges.Add(new DataRange(mpdFileData, "Header*", subPointer, subPointer + 4));

                            // Add compressed data as tracked data streams. This will prevent the *compressed data* from being
                            // analyzed, but the *uncompressed data* will still be looked at.
                            foreach (var cd in mpdFile.ChunkLocations)
                                if (cd.Exists && (cd.CompressionType != CompressionType.Uncompressed || cd.ChunkType == ChunkType.Unknown))
                                    dataRanges.Add(new DataRange(mpdFileData, "Chunk" + cd.ID, cd.ChunkFileAddress, cd.ChunkFileAddress + cd.ChunkSize));

                            // Add textures as tracked data ranges.
                            foreach (var tc in mpdFile.TextureCollections)
                                foreach (var tex in tc.TextureTable)
                                    dataRanges.Add(new DataRange(tex.Data.Data, tex.Name + "_Data", tex.ImageDataOffset, tex.ImageDataOffset + tex.ImageDataSize));

                            // Detect overlapping data.
                            var dataRangeGroups = dataRanges
                                .OrderBy(x => x.Start)
                                .GroupBy(x => x.Data)
                                .ToDictionary(x => x.Key, x => x.ToArray());

                            foreach (var group in dataRangeGroups) {
                                var dataRangesArray = group.Value;
                                for (int i = 0; i < dataRangesArray.Length; i++) {
                                    for (int j = i + 1; j < dataRangesArray.Length; j++) {
                                        var dri = dataRangesArray[i];
                                        var drj = dataRangesArray[j];

                                        // Texture palettes are allowed to overlap.
                                        if (dri.Start == drj.Start && dri.End == drj.End && dri.Name?.StartsWith("TexturePalette") == true && drj.Name?.StartsWith("TexturePalette") == true)
                                            continue;

                                        if (drj.Start >= dri.Start && drj.Start < dri.End)
                                            Console.WriteLine($"    !!! Overlap detected: {dri}, {drj}");
                                    }
                                }
                            }

                            // Zero-out referenced data.
                            foreach (var dr in dataRanges)
                                for (int i = dr.Start; i < dr.End; i++)
                                    dr.Data[i] = 0x00;

                            // Start looking for unknown data, in every set of data, which contains literal file data
                            // and the uncompressed data.
                            foreach (var drg in dataRangeGroups) {
                                var data   = drg.Key;
                                var ranges = drg.Value;

                                var unknownDataRange = new DataRange(data, null, -1, -1);
                                void UpdateDataRange(int pos, ushort? value) {
                                    if (unknownDataRange.Start == -1) {
                                        if (value != null && value != 0x0000) {
                                            unknownDataRange.Start = pos;
                                            unknownDataRange.End = pos + 2;
                                        }
                                        return;
                                    }

                                    if (value == null || value == 0x0000) {
                                        var prevDataRange = dataRangeGroups[data].Where(x => x.End <= unknownDataRange.Start).Cast<DataRange?>().LastOrDefault();

                                        var printLine = $"   {unknownDataRange}";
                                        if (prevDataRange != null)
                                            printLine += $", Prev={prevDataRange}";
                                        printLine += ": ";

                                        Console.Write(printLine);
                                        int writePos = (printLine.Length - 8) / 2;

                                        if (writePos + unknownDataRange.End - unknownDataRange.Start - 1 >= 64) {
                                            writePos = 0;
                                            Console.WriteLine();
                                        }

                                        for (int i = unknownDataRange.Start; i < unknownDataRange.End; i++) {
                                            if (writePos != 0 && writePos % 64 == 0)
                                                Console.WriteLine();
                                            if (writePos % 64 == 0)
                                                Console.Write("        ");
                                            Console.Write(data[i].ToString("X2"));
                                            writePos++;
                                        }
                                        Console.WriteLine();
                                        unknownDataRange.Start = unknownDataRange.End = -1;
                                    }
                                    else {
                                        unknownDataRange.End = pos + 2;
                                    }
                                }

                                for (int i = 0; i < data.Length - 1; i += 2)
                                    UpdateDataRange(i, (ushort) ((data[i] << 8) | data[i + 1]));
                                UpdateDataRange(data.Length, null);
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }
        }

        private static string BitString(ushort bits) {
            var str = "";
            for (var i = 0; i < 16; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string ChunkString(ChunkLocation[] chunkHeaders) {
            var chunkString = "";
            for (var i = 0; i < chunkHeaders.Length; i++) {
                if (chunkHeaders[i].Address == 0)
                    break;
                if (i % 4 == 0 && i != 0)
                    chunkString += ",";
                chunkString += (chunkHeaders[i].Exists) ? "1" : "0";
            }
            return chunkString;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, IMPD_File mpdFile) {
            var mapFlags = mpdFile.MPDHeader.MapFlags;
            var chunkHeaders = mpdFile.ChunkLocations;

            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                //+ " | " + mapFlags.ToString("X4") + ", " + BitString(mapFlags)
                //+ " | " + ChunkString(chunkHeaders.Rows)
                ;
        }
    }
}
