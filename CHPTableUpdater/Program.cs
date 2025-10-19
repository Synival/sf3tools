using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;

namespace CHPTableUpdater {
    public class Program {
        private static readonly Dictionary<ScenarioType, string> c_chpPaths = new() {
            { ScenarioType.Scenario1,   "output/scenario1" },
            { ScenarioType.Scenario2,   "output/scenario2" },
            { ScenarioType.Scenario3,   "output/scenario3" },
            { ScenarioType.PremiumDisk, "output/premium-disk" },
        };

        private static readonly Dictionary<ScenarioType, string> c_dfrPaths = new() {
            { ScenarioType.Scenario1,   "dfrs/scenario1" },
            { ScenarioType.Scenario2,   "dfrs/scenario2" },
            { ScenarioType.Scenario3,   "dfrs/scenario3" },
            { ScenarioType.PremiumDisk, "dfrs/premium-disk" },
        };

        private struct CHPTableDataAtLocation {
            public CHPTableDataAtLocation(string chpFile, ushort[] data, uint offset) {
                CHPFile = chpFile;
                Data    = data;
                Offset  = offset;
            }

            public readonly string CHPFile;
            public readonly ushort[] Data;
            public readonly uint Offset;
        }

        public static void Main(string[] args) {
            // TODO: don't do it this way, omg :( :( :(
            SpriteResources.SpritePath           = "resources/sprites";
            SpriteResources.SpritesheetPath      = "resources/spritesheets";
            SpriteResources.FrameHashLookupsFile = "resources/FrameHashLookups.json";

            foreach (var (scenario, dataWithLocationsByChpFile) in CHPTables.CHPTableLocationsByScenarioAndFile) {
                // Gather all the new data from compiled CHPs.
                Console.WriteLine($"Determining table data for {scenario} CHPs...");
                var dataByChpFile = new Dictionary<string, ushort[]>();
                using (Logger.IndentedSection()) {
                    try {
                        var nameGetter = new NameGetterContext(scenario);
                        foreach (var (chpFile, _) in dataWithLocationsByChpFile)
                            dataByChpFile.Add(chpFile, GetCompiledCHPTableData(scenario, chpFile, nameGetter));
                    }
                    catch (Exception ex) {
                        Logger.LogException(ex);
                    }
                }

                // Create DFRs.
                Console.WriteLine($"Creating DFRs for {scenario}:");
                using (Logger.IndentedSection()) {
                    try {
                        var chpTablesByBinFile = dataWithLocationsByChpFile
                            .SelectMany(x => x.Value.Locations
                                .Select(y => new { CHPFile = x.Key, x.Value.Data, BINFile = y }))
                            .GroupBy(y => y.BINFile.Filename)
                            .ToDictionary(y => y.Key, y => y.Select(z => new CHPTableDataAtLocation(z.CHPFile, z.Data, z.BINFile.Offset)));

                        foreach (var (binFile, chpTables) in chpTablesByBinFile)
                            CreateDFR(scenario, binFile, chpTables.ToArray(), dataByChpFile);
                    }
                    catch (Exception ex) {
                        Logger.LogException(ex);
                    }
                }
            }
        }

        private static ushort[] GetCompiledCHPTableData(ScenarioType scenario, string chpFile, INameGetterContext nameGetter) {
            var chpFullPath = $"{c_chpPaths[scenario]}/{chpFile}";
            var chp = CHP_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(chpFullPath))), nameGetter, scenario);

            var memberSize = (scenario == ScenarioType.Scenario1) ? 4 : 2;
            var dataSize = chp.CHR_EntriesByOffset.Count * memberSize * 2;
            var data = new byte[dataSize];

            int pos = 0;
            foreach (var (chrOffset, chr) in chp.CHR_EntriesByOffset) {
                if (memberSize == 4) {
                    data[pos++] = (byte) (chrOffset >> 24);
                    data[pos++] = (byte) (chrOffset >> 16);
                    data[pos++] = (byte) (chrOffset >> 8);
                    data[pos++] = (byte) chrOffset;
                }
                else {
                    var sector = chrOffset / 0x800;
                    data[pos++] = (byte) (sector >> 8);
                    data[pos++] = (byte) sector;
                }

                var size = chr.GetSize();
                if (memberSize == 4) {
                    data[pos++] = (byte) (size >> 24);
                    data[pos++] = (byte) (size >> 16);
                    data[pos++] = (byte) (size >> 8);
                    data[pos++] = (byte) size;
                }
                else {
                    var shiftedSize = (size + 0x0f) / 0x10;
                    data[pos++] = (byte) (shiftedSize >> 8);
                    data[pos++] = (byte) shiftedSize;
                }
            }

            var dataShorts = data.ToUShorts();
            return dataShorts;
        }

        private static bool CreateDFR(ScenarioType scenario, string binFile, CHPTableDataAtLocation[] chpTables, Dictionary<string, ushort[]> updatedDataByChpFile) {
            var dfrFullPath = $"{c_dfrPaths[scenario]}/{binFile}.DFR";
            Logger.Write($"{dfrFullPath}: ");
            string dfrOutput = "";

            using (Logger.IndentedSection()) {
                try {
                    foreach (var chpTable in chpTables) {
                        var tableName = $"+0x{chpTable.Offset:X4} ({chpTable.CHPFile})";

                        int pos1 = (int) chpTable.Offset;
                        int pos2 = pos1 + chpTable.Data.Length * 2;

                        var updatedData = updatedDataByChpFile[chpTable.CHPFile];
                        if (!Enumerable.SequenceEqual(chpTable.Data, updatedData)) {
                            Logger.WriteLine($"{tableName} updated");
                            dfrOutput += $"; {chpTable.CHPFile}\n";
                            dfrOutput += chpTable.Offset.ToString("x2") + ",";
                            dfrOutput += chpTable.Data.Select(x => x.ToString("x4")).Aggregate((a, b) => a + b) + ",";
                            dfrOutput += updatedData.Select(x => x.ToString("x4")).Aggregate((a, b) => a + b) + "\n";
                        }
                    }

                    if (dfrOutput == "") {
                        Logger.Write("no updates");
                        Logger.FinishLine();
                        return false;
                    }

                    File.WriteAllText(dfrFullPath, dfrOutput);
                    return true;
                }
                catch (Exception ex) {
                    Logger.LogException(ex);
                    return false;
                }
            }
        }
    }
}
