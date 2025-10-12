using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using SF3.Types;

namespace CHPTableUpdater {
    public class Program {
        // ,--- Enter the paths for all your X1 files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
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
            // Confirm everything is what we think it is!!
            foreach (var (scenario, dataWithLocationsByChpFile) in CHPTables.CHPTableLocationsByScenarioAndFile) {
                Console.WriteLine($"{scenario}:");

                var chpTablesByBinFile = dataWithLocationsByChpFile
                    .SelectMany(x => x.Value.Locations
                        .Select(y => new { CHPFile = x.Key, x.Value.Data, BINFile = y }))
                    .GroupBy(y => y.BINFile.Filename)
                    .ToDictionary(y => y.Key, y => y.Select(z => new CHPTableDataAtLocation(z.CHPFile, z.Data, z.BINFile.Offset)));

                foreach (var (binFile, chpTables) in chpTablesByBinFile) {
                    ConfirmDataForBINFile(scenario, binFile, chpTables.ToArray());
                }
            }
        }

        private static void ConfirmDataForBINFile(ScenarioType scenario, string binFile, CHPTableDataAtLocation[] chpTables) {
            var binFullPath = $"{c_pathsIn[scenario]}{binFile}";
            Console.WriteLine($"  {binFullPath}:");

            var binData = File.ReadAllBytes(binFullPath);
            foreach (var chpTable in chpTables) {
                Console.WriteLine($"    0x{chpTable.Offset:X4}: {chpTable.CHPFile} ({chpTable.Data.Length})");

                int pos1 = (int) chpTable.Offset;
                int pos2 = pos1 + chpTable.Data.Length * 2;

                var actualData = binData[pos1..pos2].ToUShorts();
                if (!Enumerable.SequenceEqual(chpTable.Data, actualData)) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("      Not a match!");
                    Console.ForegroundColor = default;
                }
            }
        }
    }
}
