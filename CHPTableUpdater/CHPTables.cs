using System;
using System.Collections.Generic;
using SF3.Types;

namespace CHPTableUpdater {
    public static class CHPTables {
        public struct ScenarioFileKey {
            public ScenarioFileKey(ScenarioType scenario, string file) {
                Scenario = scenario;
                File = file;
            }

            public override bool Equals(object obj) => obj is ScenarioFileKey key && Scenario == key.Scenario && File == key.File;
            public override int GetHashCode() => HashCode.Combine(Scenario, File);
            public override string ToString() => $"{Scenario} {File}";

            public readonly ScenarioType Scenario;
            public readonly string File;
        }

        public struct FileOffset {
            public FileOffset(string filename, uint offset) {
                Filename = filename;
                Offset   = offset;
            }

            public override bool Equals(object obj) => obj is FileOffset offset && Filename == offset.Filename && Offset == offset.Offset;
            public override int GetHashCode() => HashCode.Combine(Filename, Offset);
            public override string ToString() => $"{Filename} +0x{Offset:X4}";

            public readonly string Filename;
            public readonly uint Offset;
        }

        public struct DataWithFileOffsets {
            public DataWithFileOffsets(ushort[] data, FileOffset[] locations) {
                Data      = data;
                Locations = locations;
            }

            public readonly ushort[] Data;
            public readonly FileOffset[] Locations;
        }

        public static Dictionary<ScenarioType, Dictionary<string, DataWithFileOffsets>> CHPTableLocationsByScenarioAndFile = new Dictionary<ScenarioType, Dictionary<string, DataWithFileOffsets>>() {
            { ScenarioType.Scenario1, new Dictionary<string, DataWithFileOffsets>() {
                { "CBE00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0xa39c, 0x0000, 0xb000, 0x0000, 0xb420, 0x0001, 0x6800, 0x0000, 0x001c, 0x0001, 0x7000, 0x0000, 0x73fc, 0x0001, 0xe800, 0x0000, 0x703c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7b8c) }
                )},

                { "CBE01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x8ad4, 0x0000, 0x9800, 0x0000, 0x8aac, 0x0001, 0x2800, 0x0000, 0x738c, 0x0001, 0xa000, 0x0000, 0x845c, 0x0002, 0x2800, 0x0000, 0x92b0 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7bc0) }
                )},

                { "CBE02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x95a0, 0x0000, 0x9800, 0x0000, 0x9aec, 0x0001, 0x3800, 0x0000, 0x9928, 0x0001, 0xd800, 0x0000, 0x77d8, 0x0002, 0x5000, 0x0000, 0xae78, 0x0003, 0x0000, 0x0000, 0x63ec },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7bf4) }
                )},

                { "CBE03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x8544, 0x0000, 0x8800, 0x0000, 0xa9b4, 0x0001, 0x3800, 0x0000, 0xe6bc, 0x0002, 0x2000, 0x0000, 0x8ef8, 0x0002, 0xb000, 0x0000, 0x87b4, 0x0003, 0x3800, 0x0000, 0x81fc },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7c28) }
                )},

                { "CBE04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x9ae4, 0x0000, 0xa000, 0x0000, 0x001c, 0x0000, 0xa800, 0x0000, 0x6b14, 0x0001, 0x1800, 0x0000, 0xab08, 0x0001, 0xc800, 0x0000, 0xc47c, 0x0002, 0x9000, 0x0000, 0xe808 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7c5c) }
                )},

                { "CBE05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0xb658, 0x0000, 0xb800, 0x0000, 0xf0c4, 0x0001, 0xb000, 0x0000, 0x001c, 0x0001, 0xb800, 0x0000, 0x9bbc, 0x0002, 0x5800, 0x0000, 0x8ea0, 0x0002, 0xe800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7c90) }
                )},

                { "CBE06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x265c, 0x0000, 0x3000, 0x0000, 0x5088, 0x0000, 0x8800, 0x0000, 0x63a4, 0x0000, 0xf000, 0x0000, 0x7054, 0x0001, 0x6800, 0x0000, 0x53f0 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7cc4) }
                )},

                { "CBE07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x001c, 0x0000, 0x1000, 0x0000, 0x6170, 0x0000, 0x7800, 0x0000, 0x4d2c, 0x0000, 0xc800, 0x0000, 0x984c, 0x0001, 0x6800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7cf8) }
                )},

                { "CBE08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x684c, 0x0000, 0x7000, 0x0000, 0xa1ec, 0x0001, 0x1800, 0x0000, 0x9bf4, 0x0001, 0xb800, 0x0000, 0x001c, 0x0001, 0xc000, 0x0000, 0xa714, 0x0002, 0x6800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7d2c) }
                )},

                { "CBE09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x7eec, 0x0000, 0x8800, 0x0000, 0x8360, 0x0001, 0x1000, 0x0000, 0x7550, 0x0001, 0x8800, 0x0000, 0xa8e4, 0x0002, 0x3800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7d60) }
                )},

                { "CBE10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x8328, 0x0000, 0x8800, 0x0000, 0x76c0, 0x0001, 0x0000, 0x0000, 0x7b18, 0x0001, 0x8000, 0x0000, 0x001c, 0x0001, 0x8800, 0x0000, 0x9e54, 0x0002, 0x2800, 0x0000, 0xac14 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7d94) }
                )},

                { "CBE11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x9cf0, 0x0000, 0xa000, 0x0000, 0xabbc, 0x0001, 0x5000, 0x0000, 0x8bb4, 0x0001, 0xe000, 0x0000, 0x8f74, 0x0002, 0x7000, 0x0000, 0x935c, 0x0003, 0x0800, 0x0000, 0x89d0 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7dc8) }
                )},

                { "CBE12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x2da8, 0x0000, 0x3000, 0x0000, 0x8870, 0x0000, 0xc000, 0x0000, 0x001c, 0x0000, 0xc800, 0x0000, 0x7ec4, 0x0001, 0x4800, 0x0000, 0x001c, 0x0001, 0x5000, 0x0000, 0x8258 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7dfc) }
                )},

                { "CBE13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x7f94, 0x0000, 0x8000, 0x0000, 0x7864, 0x0001, 0x0000, 0x0000, 0x9fec, 0x0001, 0xa000, 0x0000, 0xaa68, 0x0002, 0x5000, 0x0000, 0x9fb8, 0x0002, 0xf000, 0x0000, 0xa8f8 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7e30) }
                )},

                { "CBE14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0xadb4, 0x0000, 0xb000, 0x0000, 0x5ec4, 0x0001, 0x1000, 0x0000, 0x0098, 0x0001, 0x1800, 0x0000, 0x001c, 0x0001, 0x2000, 0x0000, 0x001c, 0x0001, 0x2800, 0x0000, 0x8444 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7e64) }
                )},

                { "CBE15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x9900, 0x0000, 0xa000, 0x0000, 0x001c, 0x0000, 0xa800, 0x0000, 0xd690, 0x0001, 0x8000, 0x0000, 0x001c, 0x0001, 0x8800, 0x0000, 0x001c, 0x0001, 0x9000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7e98) }
                )},

                { "CBE16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x001c, 0x0000, 0x1000, 0x0000, 0x001c, 0x0000, 0x1800, 0x0000, 0x9278, 0x0000, 0xb000, 0x0000, 0x747c, 0x0001, 0x2800, 0x0000, 0x7c54 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7ecc) }
                )},

                { "CBE17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0xa29c, 0x0000, 0xa800, 0x0000, 0x723c, 0x0001, 0x2000, 0x0000, 0x63f4, 0x0001, 0x8800, 0x0000, 0x6d38, 0x0001, 0xf800, 0x0000, 0x001c, 0x0002, 0x0000, 0x0000, 0x75dc },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7f00) }
                )},

                { "CBE18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x9398, 0x0000, 0x9800, 0x0000, 0x9398, 0x0001, 0x3000, 0x0000, 0x8430, 0x0001, 0xb800, 0x0000, 0x001c, 0x0001, 0xc000, 0x0000, 0x50bc, 0x0002, 0x1800, 0x0000, 0x56c4 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7f34) }
                )},

                { "CBE19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x8f44, 0x0000, 0x9000, 0x0000, 0x001c, 0x0000, 0x9800, 0x0000, 0x001c, 0x0000, 0xa000, 0x0000, 0xa8b4, 0x0001, 0x5000, 0x0000, 0x001c, 0x0001, 0x5800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7f68) }
                )},

                { "CBE20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x001c, 0x0000, 0x1000, 0x0000, 0x8fdc, 0x0000, 0xa000, 0x0000, 0x001c, 0x0000, 0xa800, 0x0000, 0x001c, 0x0000, 0xb000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7f9c) }
                )},

                { "CBE21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x001c, 0x0000, 0x0800, 0x0000, 0x001c, 0x0000, 0x1000, 0x0000, 0xae68, 0x0000, 0xc000, 0x0000, 0x001c, 0x0000, 0xc800, 0x0000, 0x001c, 0x0000, 0xd000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7fd0) }
                )},

                { "CBE22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x80c4, 0x0000, 0x8800, 0x0000, 0x9014, 0x0001, 0x2000, 0x0000, 0x001c, 0x0001, 0x2800, 0x0000, 0x001c, 0x0001, 0x3000, 0x0000, 0x001c, 0x0001, 0x3800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8004) }
                )},

                { "CBE23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x6dc0, 0x0000, 0x7000, 0x0000, 0x001c, 0x0000, 0x7800, 0x0000, 0x001c, 0x0000, 0x8000, 0x0000, 0x001c, 0x0000, 0x8800, 0x0000, 0x001c, 0x0000, 0x9000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8038) }
                )},

                { "CBF00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c54, 0x0000, 0x4000, 0x0000, 0xdc20, 0x0001, 0x2000, 0x0000, 0x4148, 0x0001, 0x6800, 0x0000, 0xef04, 0x0002, 0x5800, 0x0000, 0x001c, 0x0002, 0x6000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5174),
                        new FileOffset("X008.BIN",     0x6254),
                    }
                )},

                { "CBF01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x40a8, 0x0000, 0x4800, 0x0000, 0xffc0, 0x0001, 0x4800, 0x0000, 0x47b8, 0x0001, 0x9000, 0x0000, 0xd890, 0x0002, 0x7000, 0x0000, 0x001c, 0x0002, 0x7800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x51a8),
                        new FileOffset("X008.BIN",     0x6698),
                        new FileOffset("X008.BIN",     0x6288),
                    }
                )},

                { "CBF02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x403c, 0x0000, 0x4800, 0x0000, 0xf94c, 0x0001, 0x4800, 0x0000, 0x4304, 0x0001, 0x9000, 0x0000, 0xf1e0, 0x0002, 0x8800, 0x0000, 0x001c, 0x0002, 0x9000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x51dc),
                        new FileOffset("X008.BIN",     0x66cc),
                        new FileOffset("X008.BIN",     0x62bc),
                    }
                )},

                { "CBF03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3f8c, 0x0000, 0x4000, 0x0001, 0x0370, 0x0001, 0x4800, 0x0000, 0x3c20, 0x0001, 0x8800, 0x0000, 0xf7c4, 0x0002, 0x8000, 0x0000, 0x001c, 0x0002, 0x8800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5210),
                        new FileOffset("X008.BIN",     0x6700),
                        new FileOffset("X008.BIN",     0x62f0),
                    }
                )},

                { "CBF04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3854, 0x0000, 0x4000, 0x0000, 0xcf00, 0x0001, 0x1000, 0x0000, 0x3d3c, 0x0001, 0x5000, 0x0000, 0xf568, 0x0002, 0x4800, 0x0000, 0x001c, 0x0002, 0x5000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5244),
                        new FileOffset("X008.BIN",     0x6734),
                        new FileOffset("X008.BIN",     0x6324),
                    }
                )},

                { "CBF05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x47a8, 0x0000, 0x4800, 0x0000, 0xf6b0, 0x0001, 0x4000, 0x0000, 0x4978, 0x0001, 0x9000, 0x0000, 0xfe30, 0x0002, 0x9000, 0x0000, 0x001c, 0x0002, 0x9800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5278),
                        new FileOffset("X008.BIN",     0x6768),
                        new FileOffset("X008.BIN",     0x6358),
                    }
                )},

                { "CBF06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x35cc, 0x0000, 0x3800, 0x0000, 0xd100, 0x0001, 0x1000, 0x0000, 0x3980, 0x0001, 0x5000, 0x0000, 0xe430, 0x0002, 0x3800, 0x0000, 0x001c, 0x0002, 0x4000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x52ac),
                        new FileOffset("X008.BIN",     0x679c),
                        new FileOffset("X008.BIN",     0x638c),
                    }
                )},

                { "CBF07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x39d0, 0x0000, 0x4000, 0x0000, 0xd2c4, 0x0001, 0x1800, 0x0000, 0x3aa4, 0x0001, 0x5800, 0x0000, 0xd4f0, 0x0002, 0x3000, 0x0000, 0x001c, 0x0002, 0x3800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x52e0),
                        new FileOffset("X008.BIN",     0x63c0),
                    }
                )},

                { "CBF08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4a0c, 0x0000, 0x5000, 0x0001, 0x0e5c, 0x0001, 0x6000, 0x0000, 0x4bc0, 0x0001, 0xb000, 0x0001, 0x1438, 0x0002, 0xc800, 0x0000, 0x001c, 0x0002, 0xd000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5314),
                        new FileOffset("X008.BIN",     0x6804),
                        new FileOffset("X008.BIN",     0x63f4),
                    }
                )},

                { "CBF09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4318, 0x0000, 0x4800, 0x0000, 0xc5b8, 0x0001, 0x1000, 0x0000, 0x43f8, 0x0001, 0x5800, 0x0000, 0xc9a8, 0x0002, 0x2800, 0x0000, 0x001c, 0x0002, 0x3000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5348),
                        new FileOffset("X008.BIN",     0x6838),
                        new FileOffset("X008.BIN",     0x6428),
                    }
                )},

                { "CBF10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4c30, 0x0000, 0x5000, 0x0001, 0x1634, 0x0001, 0x6800, 0x0000, 0x3c1c, 0x0001, 0xa800, 0x0000, 0xf43c, 0x0002, 0xa000, 0x0000, 0x001c, 0x0002, 0xa800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x537c),
                        new FileOffset("X008.BIN",     0x686c),
                        new FileOffset("X008.BIN",     0x645c),
                    }
                )},

                { "CBF11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c60, 0x0000, 0x4000, 0x0000, 0xef5c, 0x0001, 0x3000, 0x0000, 0x3c60, 0x0001, 0x7000, 0x0000, 0xef5c, 0x0002, 0x6000, 0x0000, 0x001c, 0x0002, 0x6800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x53b0),
                        new FileOffset("X008.BIN",     0x68a0),
                        new FileOffset("X008.BIN",     0x6490),
                    }
                )},

                { "CBF12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x42d8, 0x0000, 0x4800, 0x0000, 0xd9d8, 0x0001, 0x2800, 0x0000, 0x42d8, 0x0001, 0x7000, 0x0000, 0xd9d8, 0x0002, 0x5000, 0x0000, 0x001c, 0x0002, 0x5800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x53e4),
                        new FileOffset("X008.BIN",     0x68d4),
                        new FileOffset("X008.BIN",     0x64c4),
                    }
                )},

                { "CBF13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4530, 0x0000, 0x4800, 0x0000, 0xeb58, 0x0001, 0x3800, 0x0000, 0x4530, 0x0001, 0x8000, 0x0000, 0xeb58, 0x0002, 0x7000, 0x0000, 0x001c, 0x0002, 0x7800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5418),
                        new FileOffset("X008.BIN",     0x6908),
                        new FileOffset("X008.BIN",     0x64f8),
                    }
                )},

                { "CBF14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x2d9c, 0x0000, 0x3000, 0x0000, 0x0374, 0x0000, 0x3800, 0x0000, 0x2c14, 0x0000, 0x6800, 0x0000, 0xaa84, 0x0001, 0x1800, 0x0000, 0x001c, 0x0001, 0x2000, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x544c),
                        new FileOffset("X008.BIN",     0x693c),
                        new FileOffset("X008.BIN",     0x652c),
                    }
                )},

                { "CBF15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x524c, 0x0000, 0x5800, 0x0001, 0x0e08, 0x0001, 0x6800, 0x0000, 0x524c, 0x0001, 0xc000, 0x0001, 0x0e08, 0x0002, 0xd000, 0x0000, 0x001c, 0x0002, 0xd800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5480),
                        new FileOffset("X008.BIN",     0x6970),
                        new FileOffset("X008.BIN",     0x6560),
                    }
                )},

                { "CBF16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x44ac, 0x0000, 0x4800, 0x0000, 0xb5a8, 0x0001, 0x0000, 0x0000, 0x4548, 0x0001, 0x4800, 0x0000, 0xb5a8, 0x0002, 0x0000, 0x0000, 0x001c, 0x0002, 0x0800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x54b4),
                        new FileOffset("X008.BIN",     0x69a4),
                        new FileOffset("X008.BIN",     0x6594),
                    }
                )},

                { "CBF17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x34d4, 0x0000, 0x3800, 0x0000, 0xc1e8, 0x0001, 0x0000, 0x0000, 0x34d4, 0x0001, 0x3800, 0x0000, 0xc1e8, 0x0002, 0x0000, 0x0000, 0x001c, 0x0002, 0x0800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x54e8),
                        new FileOffset("X008.BIN",     0x69d8),
                        new FileOffset("X008.BIN",     0x65c8),
                    }
                )},

                { "CBF18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c38, 0x0000, 0x4000, 0x0000, 0xdbd8, 0x0001, 0x2000, 0x0000, 0x3c38, 0x0001, 0x6000, 0x0000, 0xdbd8, 0x0002, 0x4000, 0x0000, 0x001c, 0x0002, 0x4800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x551c),
                        new FileOffset("X008.BIN",     0x6a0c),
                        new FileOffset("X008.BIN",     0x65fc),
                    }
                )},

                { "CBF19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4144, 0x0000, 0x4800, 0x0000, 0xc074, 0x0001, 0x1000, 0x0000, 0x4144, 0x0001, 0x5800, 0x0000, 0xc074, 0x0002, 0x2000, 0x0000, 0x001c, 0x0002, 0x2800, 0x0000, 0x001c },
                    new FileOffset[] {
                        new FileOffset("X022.BIN",     0x5550),
                        new FileOffset("X008.BIN",     0x6a40),
                        new FileOffset("X008.BIN",     0x6630),
                    }
                )},

                { "CBP00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3da4, 0x0000, 0x4000, 0x0000, 0xf4b4, 0x0001, 0x3800, 0x0000, 0x424c, 0x0001, 0x8000, 0x0001, 0x0b88, 0x0002, 0x9000, 0x0000, 0x001c, 0x0002, 0x9800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x777c) }
                )},

                { "CBP01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x40a8, 0x0000, 0x4800, 0x0000, 0xf7c8, 0x0001, 0x4000, 0x0000, 0x47b8, 0x0001, 0x8800, 0x0001, 0x1788, 0x0002, 0xa000, 0x0000, 0x001c, 0x0002, 0xa800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x77b0) }
                )},

                { "CBP02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x403c, 0x0000, 0x4800, 0x0000, 0xf6c8, 0x0001, 0x4000, 0x0000, 0x4304, 0x0001, 0x8800, 0x0001, 0x0450, 0x0002, 0x9000, 0x0000, 0x001c, 0x0002, 0x9800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x77e4) }
                )},

                { "CBP03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3f8c, 0x0000, 0x4000, 0x0001, 0x09a0, 0x0001, 0x5000, 0x0000, 0x3c20, 0x0001, 0x9000, 0x0000, 0xfc40, 0x0002, 0x9000, 0x0000, 0x001c, 0x0002, 0x9800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7818) }
                )},

                { "CBP04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3854, 0x0000, 0x4000, 0x0001, 0x585c, 0x0001, 0xa000, 0x0000, 0x3d3c, 0x0001, 0xe000, 0x0000, 0xfa40, 0x0002, 0xe000, 0x0000, 0x001c, 0x0002, 0xe800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x784c) }
                )},

                { "CBP05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x47a8, 0x0000, 0x4800, 0x0001, 0x2ba0, 0x0001, 0x7800, 0x0000, 0x4978, 0x0001, 0xc800, 0x0001, 0x376c, 0x0003, 0x0000, 0x0000, 0x001c, 0x0003, 0x0800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7880) }
                )},

                { "CBP06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x35cc, 0x0000, 0x3800, 0x0000, 0xd7ec, 0x0001, 0x1000, 0x0000, 0x3980, 0x0001, 0x5000, 0x0000, 0xea44, 0x0002, 0x4000, 0x0000, 0x001c, 0x0002, 0x4800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x78b4) }
                )},

                { "CBP07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c08, 0x0000, 0x4000, 0x0000, 0xef94, 0x0001, 0x3000, 0x0000, 0x3bd4, 0x0001, 0x7000, 0x0000, 0xf058, 0x0002, 0x6800, 0x0000, 0x001c, 0x0002, 0x7000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x78e8) }
                )},

                { "CBP08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4a0c, 0x0000, 0x5000, 0x0001, 0x2088, 0x0001, 0x7800, 0x0000, 0x4bc0, 0x0001, 0xc800, 0x0001, 0x262c, 0x0002, 0xf000, 0x0000, 0x001c, 0x0002, 0xf800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x791c) }
                )},

                { "CBP09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4318, 0x0000, 0x4800, 0x0000, 0xa76c, 0x0000, 0xf000, 0x0000, 0x43f8, 0x0001, 0x3800, 0x0000, 0xaad0, 0x0001, 0xe800, 0x0000, 0x001c, 0x0001, 0xf000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7950) }
                )},

                { "CBP10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x45d8, 0x0000, 0x4800, 0x0001, 0x15f8, 0x0001, 0x6000, 0x0000, 0x3c1c, 0x0001, 0xa000, 0x0000, 0xf28c, 0x0002, 0x9800, 0x0000, 0x001c, 0x0002, 0xa000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7984) }
                )},

                { "CBP11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c60, 0x0000, 0x4000, 0x0000, 0xebd8, 0x0001, 0x3000, 0x0000, 0x3c60, 0x0001, 0x7000, 0x0000, 0xebd8, 0x0002, 0x6000, 0x0000, 0x001c, 0x0002, 0x6800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x79b8) }
                )},

                { "CBP12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x42d8, 0x0000, 0x4800, 0x0001, 0x0254, 0x0001, 0x5000, 0x0000, 0x42d8, 0x0001, 0x9800, 0x0001, 0x0254, 0x0002, 0xa000, 0x0000, 0x001c, 0x0002, 0xa800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x79ec) }
                )},

                { "CBP13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4530, 0x0000, 0x4800, 0x0001, 0x1f20, 0x0001, 0x6800, 0x0000, 0x4530, 0x0001, 0xb000, 0x0001, 0x1f20, 0x0002, 0xd000, 0x0000, 0x001c, 0x0002, 0xd800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7a20) }
                )},

                { "CBP14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x2d9c, 0x0000, 0x3000, 0x0000, 0xb154, 0x0000, 0xe800, 0x0000, 0x2c14, 0x0001, 0x1800, 0x0000, 0xaa00, 0x0001, 0xc800, 0x0000, 0x001c, 0x0001, 0xd000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7a54) }
                )},

                { "CBP15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x524c, 0x0000, 0x5800, 0x0001, 0x4418, 0x0001, 0xa000, 0x0000, 0x524c, 0x0001, 0xf800, 0x0001, 0x4418, 0x0003, 0x4000, 0x0000, 0x001c, 0x0003, 0x4800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7a88) }
                )},

                { "CBP16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4548, 0x0000, 0x4800, 0x0000, 0xaa54, 0x0000, 0xf800, 0x0000, 0x4548, 0x0001, 0x4000, 0x0000, 0xaa54, 0x0001, 0xf000, 0x0000, 0x001c, 0x0001, 0xf800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7abc) }
                )},

                { "CBP17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3500, 0x0000, 0x3800, 0x0000, 0xd784, 0x0001, 0x1000, 0x0000, 0x3500, 0x0001, 0x4800, 0x0000, 0xd784, 0x0002, 0x2000, 0x0000, 0x001c, 0x0002, 0x2800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7af0) }
                )},

                { "CBP18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x396c, 0x0000, 0x4000, 0x0000, 0xe920, 0x0001, 0x3000, 0x0000, 0x396c, 0x0001, 0x7000, 0x0000, 0xe920, 0x0002, 0x6000, 0x0000, 0x001c, 0x0002, 0x6800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7b24) }
                )},

                { "CBP19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x4144, 0x0000, 0x4800, 0x0000, 0xa20c, 0x0000, 0xf000, 0x0000, 0x4144, 0x0001, 0x3800, 0x0000, 0xa20c, 0x0001, 0xe000, 0x0000, 0x001c, 0x0001, 0xe800, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x7b58) }
                )},

                { "CBW00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x3c54, 0x0000, 0x4000, 0x0001, 0x5158, 0x0001, 0x9800, 0x0000, 0x4148, 0x0001, 0xe000, 0x0001, 0x70e4, 0x0003, 0x5800, 0x0000, 0x001c, 0x0003, 0x6000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6664) }
                )},

                { "CBW07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0000, 0x0000, 0x39d0, 0x0000, 0x4000, 0x0001, 0xa0c0, 0x0001, 0xe800, 0x0000, 0x3aa4, 0x0002, 0x2800, 0x0001, 0x4828, 0x0003, 0x7800, 0x0000, 0x001c, 0x0003, 0x8000, 0x0000, 0x001c },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x67d0) }
                )},
            }},

            { ScenarioType.Scenario2, new Dictionary<string, DataWithFileOffsets>() {
                { "CBF20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x0e94, 0x0027, 0x0403, 0x0030, 0x0f70, 0x004f, 0x0002, 0x0050, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x29c0),
                        new FileOffset("X1S_WJI.BIN",  0x1be8),
                        new FileOffset("X1S_EJI.BIN",  0x16d0),
                        new FileOffset("X1SWA_JI.BIN", 0x2068),
                        new FileOffset("X1STJI.BIN",   0x4814),
                        new FileOffset("X1STAJI.BIN",  0x2188),
                        new FileOffset("X1SHOP_T.BIN", 0x6988),
                        new FileOffset("X1LINE_H.BIN", 0x2230),
                        new FileOffset("X1FNJIN.BIN",  0x30e8),
                        new FileOffset("X1ELBEJI.BIN", 0x26ac),
                        new FileOffset("X1DUSJI.BIN",  0x1b08),
                        new FileOffset("X1BLANJI.BIN", 0x1b3c),
                        new FileOffset("X1ANAJI.BIN",  0x1ad4),
                        new FileOffset("X1AIROJI.BIN", 0x2048),
                        new FileOffset("X12BALJI.BIN", 0x1a88),
                        new FileOffset("X022.BIN",     0x54e8),
                    }
                )},

                { "CBF21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0542, 0x000b, 0x0fa4, 0x002b, 0x0426, 0x0034, 0x0f1d, 0x0053, 0x0002, 0x0054, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x29dc),
                        new FileOffset("X1S_WJI.BIN",  0x1c04),
                        new FileOffset("X1S_EJI.BIN",  0x16ec),
                        new FileOffset("X1SWA_JI.BIN", 0x2084),
                        new FileOffset("X1STJI.BIN",   0x4830),
                        new FileOffset("X1STAJI.BIN",  0x21a4),
                        new FileOffset("X1SHOP_T.BIN", 0x69a4),
                        new FileOffset("X1LINE_H.BIN", 0x224c),
                        new FileOffset("X1FNJIN.BIN",  0x3104),
                        new FileOffset("X1ELBEJI.BIN", 0x26c8),
                        new FileOffset("X1DUSJI.BIN",  0x1b24),
                        new FileOffset("X1BLANJI.BIN", 0x1b58),
                        new FileOffset("X1ANAJI.BIN",  0x1af0),
                        new FileOffset("X1AIROJI.BIN", 0x2064),
                        new FileOffset("X12BALJI.BIN", 0x1aa4),
                        new FileOffset("X022.BIN",     0x5504),
                    }
                )},

                { "CBF22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0e7b, 0x0025, 0x0595, 0x0031, 0x0ee7, 0x004f, 0x0002, 0x0050, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x29f8),
                        new FileOffset("X1S_WJI.BIN",  0x1c20),
                        new FileOffset("X1S_EJI.BIN",  0x1708),
                        new FileOffset("X1SWA_JI.BIN", 0x20a0),
                        new FileOffset("X1STJI.BIN",   0x484c),
                        new FileOffset("X1STAJI.BIN",  0x21c0),
                        new FileOffset("X1SHOP_T.BIN", 0x69c0),
                        new FileOffset("X1LINE_H.BIN", 0x2268),
                        new FileOffset("X1FNJIN.BIN",  0x3120),
                        new FileOffset("X1ELBEJI.BIN", 0x26e4),
                        new FileOffset("X1DUSJI.BIN",  0x1b40),
                        new FileOffset("X1BLANJI.BIN", 0x1b74),
                        new FileOffset("X1ANAJI.BIN",  0x1b0c),
                        new FileOffset("X1AIROJI.BIN", 0x2080),
                        new FileOffset("X12BALJI.BIN", 0x1ac0),
                        new FileOffset("X022.BIN",     0x5520),
                    }
                )},

                { "CBF23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0472, 0x0009, 0x1043, 0x002a, 0x04a8, 0x0034, 0x10fc, 0x0056, 0x0002, 0x0057, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2a14),
                        new FileOffset("X1S_WJI.BIN",  0x1c3c),
                        new FileOffset("X1S_EJI.BIN",  0x1724),
                        new FileOffset("X1SWA_JI.BIN", 0x20bc),
                        new FileOffset("X1STJI.BIN",   0x4868),
                        new FileOffset("X1STAJI.BIN",  0x21dc),
                        new FileOffset("X1SHOP_T.BIN", 0x69dc),
                        new FileOffset("X1LINE_H.BIN", 0x2284),
                        new FileOffset("X1FNJIN.BIN",  0x313c),
                        new FileOffset("X1ELBEJI.BIN", 0x2700),
                        new FileOffset("X1DUSJI.BIN",  0x1b5c),
                        new FileOffset("X1BLANJI.BIN", 0x1b90),
                        new FileOffset("X1ANAJI.BIN",  0x1b28),
                        new FileOffset("X1AIROJI.BIN", 0x209c),
                        new FileOffset("X12BALJI.BIN", 0x1adc),
                        new FileOffset("X022.BIN",     0x553c),
                    }
                )},

                { "CBF24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0431, 0x0009, 0x0e68, 0x0026, 0x0431, 0x002f, 0x0f39, 0x004e, 0x0002, 0x004f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2a30),
                        new FileOffset("X1S_WJI.BIN",  0x1c58),
                        new FileOffset("X1S_EJI.BIN",  0x1740),
                        new FileOffset("X1SWA_JI.BIN", 0x20d8),
                        new FileOffset("X1STJI.BIN",   0x4884),
                        new FileOffset("X1STAJI.BIN",  0x21f8),
                        new FileOffset("X1SHOP_T.BIN", 0x69f8),
                        new FileOffset("X1LINE_H.BIN", 0x22a0),
                        new FileOffset("X1FNJIN.BIN",  0x3158),
                        new FileOffset("X1ELBEJI.BIN", 0x271c),
                        new FileOffset("X1DUSJI.BIN",  0x1b78),
                        new FileOffset("X1BLANJI.BIN", 0x1bac),
                        new FileOffset("X1ANAJI.BIN",  0x1b44),
                        new FileOffset("X1AIROJI.BIN", 0x20b8),
                        new FileOffset("X12BALJI.BIN", 0x1af8),
                        new FileOffset("X022.BIN",     0x5558),
                    }
                )},

                { "CBF25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04e1, 0x000a, 0x11c1, 0x002e, 0x04de, 0x0038, 0x11c1, 0x005c, 0x0002, 0x005d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2a4c),
                        new FileOffset("X1S_WJI.BIN",  0x1c74),
                        new FileOffset("X1S_EJI.BIN",  0x175c),
                        new FileOffset("X1SWA_JI.BIN", 0x20f4),
                        new FileOffset("X1STJI.BIN",   0x48a0),
                        new FileOffset("X1STAJI.BIN",  0x2214),
                        new FileOffset("X1SHOP_T.BIN", 0x6a14),
                        new FileOffset("X1LINE_H.BIN", 0x22bc),
                        new FileOffset("X1FNJIN.BIN",  0x3174),
                        new FileOffset("X1ELBEJI.BIN", 0x2738),
                        new FileOffset("X1DUSJI.BIN",  0x1b94),
                        new FileOffset("X1BLANJI.BIN", 0x1bc8),
                        new FileOffset("X1ANAJI.BIN",  0x1b60),
                        new FileOffset("X1AIROJI.BIN", 0x20d4),
                        new FileOffset("X12BALJI.BIN", 0x1b14),
                        new FileOffset("X022.BIN",     0x5574),
                    }
                )},

                { "CBF26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ac, 0x0008, 0x0ea2, 0x0026, 0x03fb, 0x002e, 0x0ea2, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2a68),
                        new FileOffset("X1S_WJI.BIN",  0x1c90),
                        new FileOffset("X1S_EJI.BIN",  0x1778),
                        new FileOffset("X1SWA_JI.BIN", 0x2110),
                        new FileOffset("X1STJI.BIN",   0x48bc),
                        new FileOffset("X1STAJI.BIN",  0x2230),
                        new FileOffset("X1SHOP_T.BIN", 0x6a30),
                        new FileOffset("X1LINE_H.BIN", 0x22d8),
                        new FileOffset("X1FNJIN.BIN",  0x3190),
                        new FileOffset("X1ELBEJI.BIN", 0x2754),
                        new FileOffset("X1DUSJI.BIN",  0x1bb0),
                        new FileOffset("X1BLANJI.BIN", 0x1be4),
                        new FileOffset("X1ANAJI.BIN",  0x1b7c),
                        new FileOffset("X1AIROJI.BIN", 0x20f0),
                        new FileOffset("X12BALJI.BIN", 0x1b30),
                        new FileOffset("X022.BIN",     0x5590),
                    }
                )},

                { "CBF27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0781, 0x0010, 0x0d5b, 0x002b, 0x0793, 0x003b, 0x0d82, 0x0057, 0x0002, 0x0058, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2a84),
                        new FileOffset("X1S_WJI.BIN",  0x1cac),
                        new FileOffset("X1S_EJI.BIN",  0x1794),
                        new FileOffset("X1SWA_JI.BIN", 0x212c),
                        new FileOffset("X1STJI.BIN",   0x48d8),
                        new FileOffset("X1STAJI.BIN",  0x224c),
                        new FileOffset("X1SHOP_T.BIN", 0x6a4c),
                        new FileOffset("X1LINE_H.BIN", 0x22f4),
                        new FileOffset("X1FNJIN.BIN",  0x31ac),
                        new FileOffset("X1ELBEJI.BIN", 0x2770),
                        new FileOffset("X1DUSJI.BIN",  0x1bcc),
                        new FileOffset("X1BLANJI.BIN", 0x1c00),
                        new FileOffset("X1ANAJI.BIN",  0x1b98),
                        new FileOffset("X1AIROJI.BIN", 0x210c),
                        new FileOffset("X12BALJI.BIN", 0x1b4c),
                        new FileOffset("X022.BIN",     0x55ac),
                    }
                )},

                { "CBF28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03de, 0x0008, 0x0cdd, 0x0022, 0x039d, 0x002a, 0x0d25, 0x0045, 0x0002, 0x0046, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2aa0),
                        new FileOffset("X1S_WJI.BIN",  0x1cc8),
                        new FileOffset("X1S_EJI.BIN",  0x17b0),
                        new FileOffset("X1SWA_JI.BIN", 0x2148),
                        new FileOffset("X1STJI.BIN",   0x48f4),
                        new FileOffset("X1STAJI.BIN",  0x2268),
                        new FileOffset("X1SHOP_T.BIN", 0x6a68),
                        new FileOffset("X1LINE_H.BIN", 0x2310),
                        new FileOffset("X1FNJIN.BIN",  0x31c8),
                        new FileOffset("X1ELBEJI.BIN", 0x278c),
                        new FileOffset("X1DUSJI.BIN",  0x1be8),
                        new FileOffset("X1BLANJI.BIN", 0x1c1c),
                        new FileOffset("X1ANAJI.BIN",  0x1bb4),
                        new FileOffset("X1AIROJI.BIN", 0x2128),
                        new FileOffset("X12BALJI.BIN", 0x1b68),
                        new FileOffset("X022.BIN",     0x55c8),
                    }
                )},

                { "CBF29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0353, 0x0007, 0x0e44, 0x0024, 0x03f4, 0x002c, 0x0e5c, 0x0049, 0x0002, 0x004a, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2abc),
                        new FileOffset("X1S_WJI.BIN",  0x1ce4),
                        new FileOffset("X1S_EJI.BIN",  0x17cc),
                        new FileOffset("X1SWA_JI.BIN", 0x2164),
                        new FileOffset("X1STJI.BIN",   0x4910),
                        new FileOffset("X1STAJI.BIN",  0x2284),
                        new FileOffset("X1SHOP_T.BIN", 0x6a84),
                        new FileOffset("X1LINE_H.BIN", 0x232c),
                        new FileOffset("X1FNJIN.BIN",  0x31e4),
                        new FileOffset("X1ELBEJI.BIN", 0x27a8),
                        new FileOffset("X1DUSJI.BIN",  0x1c04),
                        new FileOffset("X1BLANJI.BIN", 0x1c38),
                        new FileOffset("X1ANAJI.BIN",  0x1bd0),
                        new FileOffset("X1AIROJI.BIN", 0x2144),
                        new FileOffset("X12BALJI.BIN", 0x1b84),
                        new FileOffset("X022.BIN",     0x55e4),
                    }
                )},

                { "CBF30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x0fa7, 0x0028, 0x039c, 0x0030, 0x0e87, 0x004e, 0x0002, 0x004f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2ad8),
                        new FileOffset("X1S_WJI.BIN",  0x1d00),
                        new FileOffset("X1S_EJI.BIN",  0x17e8),
                        new FileOffset("X1SWA_JI.BIN", 0x2180),
                        new FileOffset("X1STJI.BIN",   0x492c),
                        new FileOffset("X1STAJI.BIN",  0x22a0),
                        new FileOffset("X1SHOP_T.BIN", 0x6aa0),
                        new FileOffset("X1LINE_H.BIN", 0x2348),
                        new FileOffset("X1FNJIN.BIN",  0x3200),
                        new FileOffset("X1ELBEJI.BIN", 0x27c4),
                        new FileOffset("X1DUSJI.BIN",  0x1c20),
                        new FileOffset("X1BLANJI.BIN", 0x1c54),
                        new FileOffset("X1ANAJI.BIN",  0x1bec),
                        new FileOffset("X1AIROJI.BIN", 0x2160),
                        new FileOffset("X12BALJI.BIN", 0x1ba0),
                        new FileOffset("X022.BIN",     0x5600),
                    }
                )},

                { "CBF31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038c, 0x0008, 0x0ce4, 0x0022, 0x03ab, 0x002a, 0x0d4f, 0x0045, 0x0002, 0x0046, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2af4),
                        new FileOffset("X1S_WJI.BIN",  0x1d1c),
                        new FileOffset("X1S_EJI.BIN",  0x1804),
                        new FileOffset("X1SWA_JI.BIN", 0x219c),
                        new FileOffset("X1STJI.BIN",   0x4948),
                        new FileOffset("X1STAJI.BIN",  0x22bc),
                        new FileOffset("X1SHOP_T.BIN", 0x6abc),
                        new FileOffset("X1LINE_H.BIN", 0x2364),
                        new FileOffset("X1FNJIN.BIN",  0x321c),
                        new FileOffset("X1ELBEJI.BIN", 0x27e0),
                        new FileOffset("X1DUSJI.BIN",  0x1c3c),
                        new FileOffset("X1BLANJI.BIN", 0x1c70),
                        new FileOffset("X1ANAJI.BIN",  0x1c08),
                        new FileOffset("X1AIROJI.BIN", 0x217c),
                        new FileOffset("X12BALJI.BIN", 0x1bbc),
                        new FileOffset("X022.BIN",     0x561c),
                    }
                )},

                { "CBF32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e4, 0x0008, 0x0e2c, 0x0025, 0x03e4, 0x002d, 0x0e2c, 0x004a, 0x0002, 0x004b, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b10),
                        new FileOffset("X1S_WJI.BIN",  0x1d38),
                        new FileOffset("X1S_EJI.BIN",  0x1820),
                        new FileOffset("X1SWA_JI.BIN", 0x21b8),
                        new FileOffset("X1STJI.BIN",   0x4964),
                        new FileOffset("X1STAJI.BIN",  0x22d8),
                        new FileOffset("X1SHOP_T.BIN", 0x6ad8),
                        new FileOffset("X1LINE_H.BIN", 0x2380),
                        new FileOffset("X1FNJIN.BIN",  0x3238),
                        new FileOffset("X1ELBEJI.BIN", 0x27fc),
                        new FileOffset("X1DUSJI.BIN",  0x1c58),
                        new FileOffset("X1BLANJI.BIN", 0x1c8c),
                        new FileOffset("X1ANAJI.BIN",  0x1c24),
                        new FileOffset("X1AIROJI.BIN", 0x2198),
                        new FileOffset("X12BALJI.BIN", 0x1bd8),
                        new FileOffset("X022.BIN",     0x5638),
                    }
                )},

                { "CBF33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0865, 0x0011, 0x0d2f, 0x002c, 0x0865, 0x003d, 0x0d2f, 0x0058, 0x0002, 0x0059, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b2c),
                        new FileOffset("X1S_WJI.BIN",  0x1d54),
                        new FileOffset("X1S_EJI.BIN",  0x183c),
                        new FileOffset("X1SWA_JI.BIN", 0x21d4),
                        new FileOffset("X1STJI.BIN",   0x4980),
                        new FileOffset("X1STAJI.BIN",  0x22f4),
                        new FileOffset("X1SHOP_T.BIN", 0x6af4),
                        new FileOffset("X1LINE_H.BIN", 0x239c),
                        new FileOffset("X1FNJIN.BIN",  0x3254),
                        new FileOffset("X1ELBEJI.BIN", 0x2818),
                        new FileOffset("X1DUSJI.BIN",  0x1c74),
                        new FileOffset("X1BLANJI.BIN", 0x1ca8),
                        new FileOffset("X1ANAJI.BIN",  0x1c40),
                        new FileOffset("X1AIROJI.BIN", 0x21b4),
                        new FileOffset("X12BALJI.BIN", 0x1bf4),
                        new FileOffset("X022.BIN",     0x5654),
                    }
                )},

                { "CBF34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0396, 0x0008, 0x0e7f, 0x0025, 0x0396, 0x002d, 0x0e7f, 0x004a, 0x0002, 0x004b, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b48),
                        new FileOffset("X1S_WJI.BIN",  0x1d70),
                        new FileOffset("X1S_EJI.BIN",  0x1858),
                        new FileOffset("X1SWA_JI.BIN", 0x21f0),
                        new FileOffset("X1STJI.BIN",   0x499c),
                        new FileOffset("X1STAJI.BIN",  0x2310),
                        new FileOffset("X1SHOP_T.BIN", 0x6b10),
                        new FileOffset("X1LINE_H.BIN", 0x23b8),
                        new FileOffset("X1FNJIN.BIN",  0x3270),
                        new FileOffset("X1ELBEJI.BIN", 0x2834),
                        new FileOffset("X1DUSJI.BIN",  0x1c90),
                        new FileOffset("X1BLANJI.BIN", 0x1cc4),
                        new FileOffset("X1ANAJI.BIN",  0x1c5c),
                        new FileOffset("X1AIROJI.BIN", 0x21d0),
                        new FileOffset("X12BALJI.BIN", 0x1c10),
                        new FileOffset("X022.BIN",     0x5670),
                    }
                )},

                { "CBF35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b1, 0x0008, 0x0d6e, 0x0023, 0x03b1, 0x002b, 0x0d6e, 0x0046, 0x0002, 0x0047, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b64),
                        new FileOffset("X1S_WJI.BIN",  0x1d8c),
                        new FileOffset("X1S_EJI.BIN",  0x1874),
                        new FileOffset("X1SWA_JI.BIN", 0x220c),
                        new FileOffset("X1STJI.BIN",   0x49b8),
                        new FileOffset("X1STAJI.BIN",  0x232c),
                        new FileOffset("X1SHOP_T.BIN", 0x6b2c),
                        new FileOffset("X1LINE_H.BIN", 0x23d4),
                        new FileOffset("X1FNJIN.BIN",  0x328c),
                        new FileOffset("X1ELBEJI.BIN", 0x2850),
                        new FileOffset("X1DUSJI.BIN",  0x1cac),
                        new FileOffset("X1BLANJI.BIN", 0x1ce0),
                        new FileOffset("X1ANAJI.BIN",  0x1c78),
                        new FileOffset("X1AIROJI.BIN", 0x21ec),
                        new FileOffset("X12BALJI.BIN", 0x1c2c),
                        new FileOffset("X022.BIN",     0x568c),
                    }
                )},

                { "CBF36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b0, 0x0008, 0x0cb4, 0x0022, 0x03b0, 0x002a, 0x0cb4, 0x0044, 0x0002, 0x0045, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b80),
                        new FileOffset("X1S_WJI.BIN",  0x1da8),
                        new FileOffset("X1S_EJI.BIN",  0x1890),
                        new FileOffset("X1SWA_JI.BIN", 0x2228),
                        new FileOffset("X1STJI.BIN",   0x49d4),
                        new FileOffset("X1STAJI.BIN",  0x2348),
                        new FileOffset("X1SHOP_T.BIN", 0x6b48),
                        new FileOffset("X1LINE_H.BIN", 0x23f0),
                        new FileOffset("X1FNJIN.BIN",  0x32a8),
                        new FileOffset("X1ELBEJI.BIN", 0x286c),
                        new FileOffset("X1DUSJI.BIN",  0x1cc8),
                        new FileOffset("X1BLANJI.BIN", 0x1cfc),
                        new FileOffset("X1ANAJI.BIN",  0x1c94),
                        new FileOffset("X1AIROJI.BIN", 0x2208),
                        new FileOffset("X12BALJI.BIN", 0x1c48),
                        new FileOffset("X022.BIN",     0x56a8),
                    }
                )},

                { "CBF37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0353, 0x0007, 0x1321, 0x002e, 0x0353, 0x0035, 0x1321, 0x005c, 0x0002, 0x005d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2b9c),
                        new FileOffset("X1S_WJI.BIN",  0x1dc4),
                        new FileOffset("X1S_EJI.BIN",  0x18ac),
                        new FileOffset("X1SWA_JI.BIN", 0x2244),
                        new FileOffset("X1STJI.BIN",   0x49f0),
                        new FileOffset("X1STAJI.BIN",  0x2364),
                        new FileOffset("X1SHOP_T.BIN", 0x6b64),
                        new FileOffset("X1LINE_H.BIN", 0x240c),
                        new FileOffset("X1FNJIN.BIN",  0x32c4),
                        new FileOffset("X1ELBEJI.BIN", 0x2888),
                        new FileOffset("X1DUSJI.BIN",  0x1ce4),
                        new FileOffset("X1BLANJI.BIN", 0x1d18),
                        new FileOffset("X1ANAJI.BIN",  0x1cb0),
                        new FileOffset("X1AIROJI.BIN", 0x2224),
                        new FileOffset("X12BALJI.BIN", 0x1c64),
                        new FileOffset("X022.BIN",     0x56c4),
                    }
                )},

                { "CBF38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0b93, 0x0020, 0x03bf, 0x0028, 0x0b22, 0x003f, 0x0002, 0x0040, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2bb8),
                        new FileOffset("X1S_WJI.BIN",  0x1de0),
                        new FileOffset("X1S_EJI.BIN",  0x18c8),
                        new FileOffset("X1SWA_JI.BIN", 0x2260),
                        new FileOffset("X1STJI.BIN",   0x4a0c),
                        new FileOffset("X1STAJI.BIN",  0x2380),
                        new FileOffset("X1SHOP_T.BIN", 0x6b80),
                        new FileOffset("X1LINE_H.BIN", 0x2428),
                        new FileOffset("X1FNJIN.BIN",  0x32e0),
                        new FileOffset("X1ELBEJI.BIN", 0x28a4),
                        new FileOffset("X1DUSJI.BIN",  0x1d00),
                        new FileOffset("X1BLANJI.BIN", 0x1d34),
                        new FileOffset("X1ANAJI.BIN",  0x1ccc),
                        new FileOffset("X1AIROJI.BIN", 0x2240),
                        new FileOffset("X12BALJI.BIN", 0x1c80),
                        new FileOffset("X022.BIN",     0x56e0),
                    }
                )},

                { "CBF39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03da, 0x0008, 0x0e19, 0x0025, 0x03da, 0x002d, 0x0e19, 0x004a, 0x0002, 0x004b, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2bd4),
                        new FileOffset("X1S_WJI.BIN",  0x1dfc),
                        new FileOffset("X1S_EJI.BIN",  0x18e4),
                        new FileOffset("X1SWA_JI.BIN", 0x227c),
                        new FileOffset("X1STJI.BIN",   0x4a28),
                        new FileOffset("X1STAJI.BIN",  0x239c),
                        new FileOffset("X1SHOP_T.BIN", 0x6b9c),
                        new FileOffset("X1LINE_H.BIN", 0x2444),
                        new FileOffset("X1FNJIN.BIN",  0x32fc),
                        new FileOffset("X1ELBEJI.BIN", 0x28c0),
                        new FileOffset("X1DUSJI.BIN",  0x1d1c),
                        new FileOffset("X1BLANJI.BIN", 0x1d50),
                        new FileOffset("X1ANAJI.BIN",  0x1ce8),
                        new FileOffset("X1AIROJI.BIN", 0x225c),
                        new FileOffset("X12BALJI.BIN", 0x1c9c),
                        new FileOffset("X022.BIN",     0x56fc),
                    }
                )},

                { "CBP01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040b, 0x0009, 0x0f7d, 0x0028, 0x047c, 0x0031, 0x1179, 0x0054, 0x0002, 0x0055, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x33f0),
                        new FileOffset("X007.BIN",     0x7eb0),
                    }
                )},

                { "CBP02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0404, 0x0009, 0x0f6d, 0x0028, 0x0431, 0x0031, 0x1045, 0x0052, 0x0002, 0x0053, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x340c),
                        new FileOffset("X007.BIN",     0x7ecc),
                    }
                )},

                { "CBP03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x109a, 0x002a, 0x03c2, 0x0032, 0x0fc4, 0x0052, 0x0002, 0x0053, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3428),
                        new FileOffset("X007.BIN",     0x7ee8),
                    }
                )},

                { "CBP04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0386, 0x0008, 0x1586, 0x0034, 0x03d4, 0x003c, 0x0fa4, 0x005c, 0x0002, 0x005d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3444),
                        new FileOffset("X007.BIN",     0x7f04),
                    }
                )},

                { "CBP05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x047b, 0x0009, 0x12ba, 0x002f, 0x0498, 0x0039, 0x1377, 0x0060, 0x0002, 0x0061, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3460),
                        new FileOffset("X007.BIN",     0x7f20),
                    }
                )},

                { "CBP06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035d, 0x0007, 0x0d7f, 0x0022, 0x0398, 0x002a, 0x0ea5, 0x0048, 0x0002, 0x0049, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x347c),
                        new FileOffset("X007.BIN",     0x7f3c),
                    }
                )},

                { "CBP08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04a1, 0x000a, 0x1209, 0x002f, 0x04bc, 0x0039, 0x1263, 0x005e, 0x0002, 0x005f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3498),
                        new FileOffset("X007.BIN",     0x7f58),
                    }
                )},

                { "CBP09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0432, 0x0009, 0x0a77, 0x001e, 0x0440, 0x0027, 0x0aad, 0x003d, 0x0002, 0x003e, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x34b4),
                        new FileOffset("X007.BIN",     0x7f74),
                    }
                )},

                { "CBP10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x045e, 0x0009, 0x1160, 0x002c, 0x03c2, 0x0034, 0x0f29, 0x0053, 0x0002, 0x0054, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x34d0),
                        new FileOffset("X007.BIN",     0x7f90),
                    }
                )},

                { "CBP11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x0ebe, 0x0026, 0x03c6, 0x002e, 0x0ebe, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x34ec),
                        new FileOffset("X007.BIN",     0x7fac),
                    }
                )},

                { "CBP12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x042e, 0x0009, 0x1026, 0x002a, 0x042e, 0x0033, 0x1026, 0x0054, 0x0002, 0x0055, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3508),
                        new FileOffset("X007.BIN",     0x7fc8),
                    }
                )},

                { "CBP13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0453, 0x0009, 0x11f2, 0x002d, 0x0453, 0x0036, 0x11f2, 0x005a, 0x0002, 0x005b, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3524),
                        new FileOffset("X007.BIN",     0x7fe4),
                    }
                )},

                { "CBP14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x0b16, 0x001d, 0x02c2, 0x0023, 0x0aa0, 0x0039, 0x0002, 0x003a, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3540),
                        new FileOffset("X007.BIN",     0x8000),
                    }
                )},

                { "CBP15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0525, 0x000b, 0x1442, 0x0034, 0x0525, 0x003f, 0x1442, 0x0068, 0x0002, 0x0069, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x355c),
                        new FileOffset("X007.BIN",     0x801c),
                    }
                )},

                { "CBP16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0455, 0x0009, 0x0aa6, 0x001f, 0x0455, 0x0028, 0x0aa6, 0x003e, 0x0002, 0x003f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3578),
                        new FileOffset("X007.BIN",     0x8038),
                    }
                )},

                { "CBP17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0350, 0x0007, 0x0d79, 0x0022, 0x0350, 0x0029, 0x0d79, 0x0044, 0x0002, 0x0045, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3594),
                        new FileOffset("X007.BIN",     0x8054),
                    }
                )},

                { "CBP18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0397, 0x0008, 0x0e92, 0x0026, 0x0397, 0x002e, 0x0e92, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x35b0),
                        new FileOffset("X007.BIN",     0x8070),
                    }
                )},

                { "CBP19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0415, 0x0009, 0x0a21, 0x001e, 0x0415, 0x0027, 0x0a21, 0x003c, 0x0002, 0x003d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x35cc),
                        new FileOffset("X007.BIN",     0x808c),
                    }
                )},

                { "CBP20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x1015, 0x0029, 0x0434, 0x0032, 0x1127, 0x0055, 0x0002, 0x0056, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x35e8),
                        new FileOffset("X007.BIN",     0x80a8),
                    }
                )},

                { "CBP21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0423, 0x0009, 0x0e41, 0x0026, 0x0454, 0x002f, 0x0ef8, 0x004d, 0x0002, 0x004e, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3604),
                        new FileOffset("X007.BIN",     0x80c4),
                    }
                )},

                { "CBP22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0ef6, 0x0026, 0x0595, 0x0032, 0x0f5e, 0x0051, 0x0002, 0x0052, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3620),
                        new FileOffset("X007.BIN",     0x80e0),
                    }
                )},

                { "CBP23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x044c, 0x0009, 0x11a9, 0x002d, 0x06c6, 0x003b, 0x128e, 0x0061, 0x0002, 0x0062, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x363c),
                        new FileOffset("X007.BIN",     0x80fc),
                    }
                )},

                { "CBP24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x1089, 0x002b, 0x041f, 0x0034, 0x1081, 0x0056, 0x0002, 0x0057, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3658),
                        new FileOffset("X007.BIN",     0x8118),
                    }
                )},

                { "CBP25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x049d, 0x000a, 0x1372, 0x0031, 0x04ae, 0x003b, 0x1370, 0x0062, 0x0002, 0x0063, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3674),
                        new FileOffset("X007.BIN",     0x8134),
                    }
                )},

                { "CBP26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03bd, 0x0008, 0x0efb, 0x0026, 0x0414, 0x002f, 0x103c, 0x0050, 0x0002, 0x0051, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3690),
                        new FileOffset("X007.BIN",     0x8150),
                    }
                )},

                { "CBP27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0781, 0x0010, 0x0b54, 0x0027, 0x0793, 0x0037, 0x0b57, 0x004e, 0x0002, 0x004f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x36ac),
                        new FileOffset("X007.BIN",     0x816c),
                    }
                )},

                { "CBP28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0377, 0x0007, 0x0e2f, 0x0024, 0x054b, 0x002f, 0x0e72, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x36c8),
                        new FileOffset("X007.BIN",     0x8188),
                    }
                )},

                { "CBP29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03da, 0x0008, 0x0f94, 0x0028, 0x03d9, 0x0030, 0x0fb7, 0x0050, 0x0002, 0x0051, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x36e4),
                        new FileOffset("X007.BIN",     0x81a4),
                    }
                )},

                { "CBP30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x0ffb, 0x0028, 0x039c, 0x0030, 0x0ed3, 0x004e, 0x0002, 0x004f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3700),
                        new FileOffset("X007.BIN",     0x81c0),
                    }
                )},

                { "CBP31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x037d, 0x0007, 0x0e5b, 0x0024, 0x03be, 0x002c, 0x0f06, 0x004b, 0x0002, 0x004c, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x371c),
                        new FileOffset("X007.BIN",     0x81dc),
                    }
                )},

                { "CBP32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fa, 0x0008, 0x0f16, 0x0027, 0x03fa, 0x002f, 0x0f16, 0x004e, 0x0002, 0x004f, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3738),
                        new FileOffset("X007.BIN",     0x81f8),
                    }
                )},

                { "CBP33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0376, 0x0007, 0x0e61, 0x0024, 0x0376, 0x002b, 0x0e61, 0x0048, 0x0002, 0x0049, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3754),
                        new FileOffset("X007.BIN",     0x8214),
                    }
                )},

                { "CBP34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0396, 0x0008, 0x0ec8, 0x0026, 0x0396, 0x002e, 0x0ec8, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x3770),
                        new FileOffset("X007.BIN",     0x8230),
                    }
                )},

                { "CBP35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b6, 0x0008, 0x0e4c, 0x0025, 0x03b6, 0x002d, 0x0e4c, 0x004a, 0x0002, 0x004b, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x378c),
                        new FileOffset("X007.BIN",     0x824c),
                    }
                )},

                { "CBP36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0391, 0x0008, 0x0ed0, 0x0026, 0x0391, 0x002e, 0x0ed0, 0x004c, 0x0002, 0x004d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x37a8),
                        new FileOffset("X007.BIN",     0x8268),
                    }
                )},

                { "CBP37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0538, 0x000b, 0x1170, 0x002e, 0x0538, 0x0039, 0x1170, 0x005c, 0x0002, 0x005d, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x37c4),
                        new FileOffset("X007.BIN",     0x8284),
                    }
                )},

                { "CBP38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0bae, 0x0020, 0x03bf, 0x0028, 0x0b2b, 0x003f, 0x0002, 0x0040, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x37e0),
                        new FileOffset("X007.BIN",     0x82a0),
                    }
                )},

                { "CBP39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040f, 0x0009, 0x0fe0, 0x0029, 0x040f, 0x0032, 0x0fe0, 0x0052, 0x0002, 0x0053, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1ATBTL2.BIN", 0x37fc),
                        new FileOffset("X007.BIN",     0x82bc),
                    }
                )},

                { "CBW20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x161d, 0x0036, 0x0403, 0x003f, 0x1795, 0x006f, 0x0002, 0x0070, 0x0002 },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6b88) }
                )},

                { "ENEMY.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x0a3a, 0x0016, 0x0b42, 0x002d, 0x0002, 0x002e, 0x0740, 0x003d, 0x0704, 0x004c, 0x0d03, 0x0067, 0x08ae, 0x0079, 0x08ab, 0x008b, 0x0739, 0x009a, 0x0846, 0x00ab, 0x092b, 0x00be, 0x095a, 0x00d1, 0x09af, 0x00e5, 0x0993, 0x00f9, 0x077e, 0x0108, 0x0ae8, 0x011e, 0x063f, 0x012b, 0x0855, 0x013c, 0x0a9c, 0x0152, 0x0e6c, 0x016f, 0x08f0, 0x0181, 0x087c, 0x0192, 0x0820, 0x01a3, 0x09af, 0x01b7, 0x0806, 0x01c8, 0x06b2, 0x01d6, 0x0ab1, 0x01ec, 0x0c48, 0x0205, 0x0e81, 0x0223, 0x0b66, 0x023a, 0x0f0d, 0x0259, 0x0002, 0x025a, 0x09bc, 0x026e, 0x08ea, 0x0280, 0x0002, 0x0281, 0x0002, 0x0282, 0x0266, 0x0287, 0x0509, 0x0292, 0x063b, 0x029f, 0x0706, 0x02ae, 0x053f, 0x02b9, 0x0002, 0x02ba, 0x0002, 0x02bb, 0x0617, 0x02c8, 0x04d3, 0x02d2, 0x0985, 0x02e6, 0x0002, 0x02e7, 0x0685, 0x02f5, 0x0a1f, 0x030a, 0x09b2, 0x031e, 0x0a80, 0x0333, 0x0a72, 0x0348, 0x0002, 0x0349, 0x0002, 0x034a, 0x07ef, 0x035a, 0x0836, 0x036b, 0x0755, 0x037a, 0x0a8f, 0x0390, 0x0002, 0x0391, 0x0833, 0x03a2, 0x076c, 0x03b1, 0x07b2, 0x03c1, 0x0002, 0x03c2, 0x09e6, 0x03d6, 0x0ac2, 0x03ec, 0x09cf, 0x0400, 0x0abc, 0x0416, 0x08bc, 0x0428, 0x08f8, 0x043a, 0x0936, 0x044d, 0x089d, 0x045f, 0x02db, 0x0465, 0x0887, 0x0477, 0x0736, 0x0486, 0x0866, 0x0497, 0x07ed, 0x04a7, 0x0826, 0x04b8, 0x07fa, 0x04c8, 0x0787, 0x04d8, 0x09ff, 0x04ec, 0x09fc, 0x0500, 0x0aa7, 0x0516, 0x0a90, 0x052c, 0x0adc, 0x0542, 0x05ed, 0x054e, 0x000a, 0x054f, 0x0002, 0x0550, 0x0002, 0x0551, 0x0b77, 0x0568, 0x0990, 0x057c, 0x0002, 0x057d, 0x0d69, 0x0598, 0x0002, 0x0599, 0x0991, 0x05ad, 0x0002, 0x05ae, 0x0960, 0x05c1, 0x0002, 0x05c2, 0x0002, 0x05c3, 0x0928, 0x05d6, 0x0748, 0x05e5, 0x07c6, 0x05f5, 0x0a2a, 0x060a, 0x0724, 0x0619, 0x0640, 0x0626, 0x06d4, 0x0634, 0x0002, 0x0635, 0x075e, 0x0644, 0x093a, 0x0657, 0x093a, 0x066a, 0x0843, 0x067b, 0x0002, 0x067c, 0x050c, 0x0687, 0x056d, 0x0692, 0x08f5, 0x06a4, 0x0002, 0x06a5, 0x0002, 0x06a6, 0x0a8c, 0x06bc, 0x0002, 0x06bd, 0x0002, 0x06be, 0x0002, 0x06bf, 0x0002, 0x06c0, 0x08fe, 0x06d2, 0x0002, 0x06d3, 0x0002, 0x06d4, 0x0002, 0x06d5, 0x0002, 0x06d6, 0x0002, 0x06d7, 0x0ae7, 0x06ed, 0x0002, 0x06ee, 0x0002, 0x06ef, 0x0002, 0x06f0, 0x080d, 0x0701, 0x0902, 0x0714, 0x1170, 0x0737, 0x0672, 0x0744, 0x04f1, 0x074e, 0x0002, 0x074f, 0x0725, 0x075e, 0x0002, 0x075f, 0x0002, 0x0760, 0x0002, 0x0761, 0x0002, 0x0762, 0x0002, 0x0763, 0x0002, 0x0764, 0x0002, 0x0765, 0x0002, 0x0766, 0x0002, 0x0767, 0x09db, 0x077b, 0x0a9f, 0x0791, 0x05b1, 0x079d, 0x0770, 0x07ac, 0x09bf, 0x07c0, 0x09a5, 0x07d4, 0x0913, 0x07e7, 0x08da, 0x07f9, 0x0002, 0x07fa, 0x0521, 0x0805, 0x0002, 0x0806, 0x0a32, 0x081b, 0x0002, 0x081c, 0x0002, 0x081d, 0x0002, 0x081e, 0x0a84, 0x0834, 0x0002, 0x0835, 0x0672, 0x0842, 0x04f1, 0x084c, 0x0a6a, 0x0861, 0x0796, 0x0871, 0x0886, 0x0883, 0x0b6d, 0x089a, 0x0b50, 0x08b1, 0x03f8, 0x08b9, 0x03c9, 0x08c1, 0x03f1, 0x08c9, 0x040a, 0x08d2, 0x03de, 0x08da, 0x035e, 0x08e1, 0x0e27, 0x08fe, 0x08c3, 0x0910, 0x0994, 0x0924, 0x0002, 0x0925, 0x1076, 0x0946, 0x0002, 0x0947, 0x094a, 0x095a, 0x0002, 0x095b, 0x0731, 0x096a, 0x0002, 0x096b, 0x0002, 0x096c, 0x0002, 0x096d, 0x0002, 0x096e, 0x0002 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x82d4) }
                )},

                { "ENEMYS.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x03b9, 0x0009, 0x043e, 0x0012, 0x0002, 0x0013, 0x0324, 0x001a, 0x0308, 0x0021, 0x0d03, 0x003c, 0x04fc, 0x0046, 0x03c7, 0x004e, 0x02b7, 0x0054, 0x030c, 0x005b, 0x0369, 0x0062, 0x039a, 0x006a, 0x03a0, 0x0072, 0x0393, 0x007a, 0x0342, 0x0081, 0x0425, 0x008a, 0x025d, 0x008f, 0x0330, 0x0096, 0x0405, 0x009f, 0x03b5, 0x00a7, 0x034f, 0x00ae, 0x033c, 0x00b5, 0x02f2, 0x00bb, 0x03bb, 0x00c3, 0x035d, 0x00ca, 0x02d9, 0x00d0, 0x0410, 0x00d9, 0x073c, 0x00e8, 0x03a0, 0x00f0, 0x044d, 0x00f9, 0x0366, 0x0100, 0x0002, 0x0101, 0x03be, 0x0109, 0x0360, 0x0110, 0x0002, 0x0111, 0x0002, 0x0112, 0x0264, 0x0117, 0x0507, 0x0122, 0x0638, 0x012f, 0x0703, 0x013e, 0x053d, 0x0149, 0x0002, 0x014a, 0x0002, 0x014b, 0x03a2, 0x0153, 0x04d1, 0x015d, 0x035e, 0x0164, 0x0002, 0x0165, 0x03ec, 0x016d, 0x03cd, 0x0175, 0x039d, 0x017d, 0x03ed, 0x0185, 0x047e, 0x018e, 0x0002, 0x018f, 0x0002, 0x0190, 0x02fd, 0x0196, 0x0381, 0x019e, 0x031f, 0x01a5, 0x03fa, 0x01ad, 0x0002, 0x01ae, 0x04b4, 0x01b8, 0x0332, 0x01bf, 0x034b, 0x01c6, 0x0002, 0x01c7, 0x0443, 0x01d0, 0x03fa, 0x01d8, 0x039f, 0x01e0, 0x03fe, 0x01e8, 0x03c6, 0x01f0, 0x03de, 0x01f8, 0x02d1, 0x01fe, 0x034f, 0x0205, 0x02d9, 0x020b, 0x03f1, 0x0213, 0x0310, 0x021a, 0x033e, 0x0221, 0x03eb, 0x0229, 0x0314, 0x0230, 0x0302, 0x0237, 0x0336, 0x023e, 0x03d1, 0x0246, 0x03ce, 0x024e, 0x0418, 0x0257, 0x040c, 0x0260, 0x045b, 0x0269, 0x05ea, 0x0275, 0x000a, 0x0276, 0x0002, 0x0277, 0x0002, 0x0278, 0x0817, 0x0289, 0x04be, 0x0293, 0x0002, 0x0294, 0x03ce, 0x029c, 0x0002, 0x029d, 0x0425, 0x02a6, 0x0002, 0x02a7, 0x0395, 0x02af, 0x0002, 0x02b0, 0x0002, 0x02b1, 0x0379, 0x02b8, 0x0326, 0x02bf, 0x02f4, 0x02c5, 0x0403, 0x02ce, 0x0304, 0x02d5, 0x02ac, 0x02db, 0x0295, 0x02e1, 0x0002, 0x02e2, 0x02cf, 0x02e8, 0x03f8, 0x02f0, 0x03f8, 0x02f8, 0x031c, 0x02ff, 0x0002, 0x0300, 0x050a, 0x030b, 0x056a, 0x0316, 0x036a, 0x031d, 0x0002, 0x031e, 0x0002, 0x031f, 0x03ee, 0x0327, 0x0002, 0x0328, 0x0002, 0x0329, 0x0002, 0x032a, 0x0002, 0x032b, 0x03c6, 0x0333, 0x0002, 0x0334, 0x0002, 0x0335, 0x0002, 0x0336, 0x0002, 0x0337, 0x0002, 0x0338, 0x0002, 0x0339, 0x0002, 0x033a, 0x0002, 0x033b, 0x0002, 0x033c, 0x0374, 0x0343, 0x03e6, 0x034b, 0x0538, 0x0356, 0x0670, 0x0363, 0x04ee, 0x036d, 0x0002, 0x036e, 0x013d, 0x0371, 0x0002, 0x0372, 0x0002, 0x0373, 0x0002, 0x0374, 0x0002, 0x0375, 0x0002, 0x0376, 0x0002, 0x0377, 0x0002, 0x0378, 0x0002, 0x0379, 0x0002, 0x037a, 0x03c8, 0x0382, 0x040e, 0x038b, 0x05af, 0x0397, 0x02ea, 0x039d, 0x0382, 0x03a5, 0x0423, 0x03ae, 0x03ed, 0x03b6, 0x0356, 0x03bd, 0x0002, 0x03be, 0x03c3, 0x03c6, 0x0002, 0x03c7, 0x03e9, 0x03cf, 0x0002, 0x03d0, 0x0002, 0x03d1, 0x0002, 0x03d2, 0x03e3, 0x03da, 0x0002, 0x03db, 0x0670, 0x03e8, 0x04ee, 0x03f2, 0x03cd, 0x03fa, 0x0794, 0x040a, 0x032c, 0x0411, 0x044e, 0x041a, 0x04f3, 0x0424, 0x03f7, 0x042c, 0x03c9, 0x0434, 0x03f1, 0x043c, 0x0409, 0x0445, 0x03de, 0x044d, 0x035e, 0x0454, 0x0348, 0x045b, 0x033b, 0x0462, 0x0380, 0x0469, 0x0002, 0x046a, 0x0648, 0x0477, 0x0002, 0x0478, 0x06a0, 0x0486, 0x0002, 0x0487, 0x031a, 0x048e, 0x0002, 0x048f, 0x0002, 0x0490, 0x0002, 0x0491, 0x0002, 0x0492, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1S_WJI5.BIN", 0x2bec),
                        new FileOffset("X1S_WJI.BIN",  0x1e14),
                        new FileOffset("X1S_EJI.BIN",  0x18fc),
                        new FileOffset("X1SWA_JI.BIN", 0x2294),
                        new FileOffset("X1STJI.BIN",   0x4a40),
                        new FileOffset("X1STAJI.BIN",  0x23b4),
                        new FileOffset("X1SHOP_T.BIN", 0x6bb4),
                        new FileOffset("X1LINE_H.BIN", 0x245c),
                        new FileOffset("X1FNJIN.BIN",  0x3314),
                        new FileOffset("X1ELBEJI.BIN", 0x28d8),
                        new FileOffset("X1DUSJI.BIN",  0x1d34),
                        new FileOffset("X1BLANJI.BIN", 0x1d68),
                        new FileOffset("X1ANAJI.BIN",  0x1d00),
                        new FileOffset("X1AIROJI.BIN", 0x2274),
                        new FileOffset("X12BALJI.BIN", 0x1cb4),
                        new FileOffset("X007.BIN",     0x85d4),
                    }
                )},
            }},

            { ScenarioType.Scenario3, new Dictionary<string, DataWithFileOffsets>() {
                { "CBF00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x0dc2, 0x0024, 0x0415, 0x002d, 0x0ef1, 0x004b, 0x039a, 0x0053, 0x1433 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x42ec),
                        new FileOffset("X1OHRA_J.BIN", 0x421c),
                        new FileOffset("X1MAYA_J.BIN", 0x4234),
                        new FileOffset("X1LEMO3J.BIN", 0x4790),
                        new FileOffset("X1LEMO2J.BIN", 0x4500),
                        new FileOffset("X1LEMO1J.BIN", 0x4eb4),
                        new FileOffset("X1LECHA.BIN",  0x69c4),
                        new FileOffset("X1KANSHO.BIN", 0x39cc),
                        new FileOffset("X1HOW_J.BIN",  0x4880),
                        new FileOffset("X1GL_J.BIN",   0x4e98),
                        new FileOffset("X1FUTT_J.BIN", 0x3f00),
                        new FileOffset("X1FNKA6.BIN",  0x6474),
                        new FileOffset("X1FNKA01.BIN", 0x6d98),
                        new FileOffset("X1FNK6_J.BIN", 0x3f1c),
                        new FileOffset("X1FNK1_J.BIN", 0x3e5c),
                        new FileOffset("X1DST_J.BIN",  0x4cdc),
                        new FileOffset("X1DLMA_J.BIN", 0x4b70),
                        new FileOffset("X1DLMA2J.BIN", 0x3e70),
                        new FileOffset("X1BEERJI.BIN", 0x416c),
                        new FileOffset("X1ASPI_H.BIN", 0x4560),
                        new FileOffset("X022.BIN",     0x5ee4),
                    }
                )},

                { "CBF01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040b, 0x0009, 0x0ea0, 0x0027, 0x047c, 0x0030, 0x0d78, 0x004b, 0x0403, 0x0054, 0x0e92 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4308),
                        new FileOffset("X1OHRA_J.BIN", 0x4238),
                        new FileOffset("X1MAYA_J.BIN", 0x4250),
                        new FileOffset("X1LEMO3J.BIN", 0x47ac),
                        new FileOffset("X1LEMO2J.BIN", 0x451c),
                        new FileOffset("X1LEMO1J.BIN", 0x4ed0),
                        new FileOffset("X1LECHA.BIN",  0x69e0),
                        new FileOffset("X1HOW_J.BIN",  0x489c),
                        new FileOffset("X1GL_J.BIN",   0x4eb4),
                        new FileOffset("X1FUTT_J.BIN", 0x3f1c),
                        new FileOffset("X1FNKA6.BIN",  0x6490),
                        new FileOffset("X1FNKA01.BIN", 0x6db4),
                        new FileOffset("X1FNK6_J.BIN", 0x3f38),
                        new FileOffset("X1FNK1_J.BIN", 0x3e78),
                        new FileOffset("X1DST_J.BIN",  0x4cf8),
                        new FileOffset("X1DLMA_J.BIN", 0x4b8c),
                        new FileOffset("X1DLMA2J.BIN", 0x3e8c),
                        new FileOffset("X1BEERJI.BIN", 0x4188),
                        new FileOffset("X1ASPI_H.BIN", 0x457c),
                        new FileOffset("X022.BIN",     0x5f00),
                    }
                )},

                { "CBF02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0404, 0x0009, 0x0fb7, 0x0029, 0x0431, 0x0032, 0x0f35, 0x0051, 0x0431, 0x005a, 0x1091 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4324),
                        new FileOffset("X1OHRA_J.BIN", 0x4254),
                        new FileOffset("X1MAYA_J.BIN", 0x426c),
                        new FileOffset("X1LEMO3J.BIN", 0x47c8),
                        new FileOffset("X1LEMO2J.BIN", 0x4538),
                        new FileOffset("X1LEMO1J.BIN", 0x4eec),
                        new FileOffset("X1LECHA.BIN",  0x69fc),
                        new FileOffset("X1KANSHO.BIN", 0x3a04),
                        new FileOffset("X1HOW_J.BIN",  0x48b8),
                        new FileOffset("X1GL_J.BIN",   0x4ed0),
                        new FileOffset("X1FUTT_J.BIN", 0x3f38),
                        new FileOffset("X1FNKA6.BIN",  0x64ac),
                        new FileOffset("X1FNKA01.BIN", 0x6dd0),
                        new FileOffset("X1FNK6_J.BIN", 0x3f54),
                        new FileOffset("X1FNK1_J.BIN", 0x3e94),
                        new FileOffset("X1DST_J.BIN",  0x4d14),
                        new FileOffset("X1DLMA_J.BIN", 0x4ba8),
                        new FileOffset("X1DLMA2J.BIN", 0x3ea8),
                        new FileOffset("X1BEERJI.BIN", 0x41a4),
                        new FileOffset("X1ASPI_H.BIN", 0x4598),
                        new FileOffset("X022.BIN",     0x5f1c),
                    }
                )},

                { "CBF03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x1037, 0x0029, 0x03c2, 0x0031, 0x0f7d, 0x0050, 0x03d4, 0x0058, 0x0fb4 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4340),
                        new FileOffset("X1OHRA_J.BIN", 0x4270),
                        new FileOffset("X1MAYA_J.BIN", 0x4288),
                        new FileOffset("X1LEMO3J.BIN", 0x47e4),
                        new FileOffset("X1LEMO2J.BIN", 0x4554),
                        new FileOffset("X1LEMO1J.BIN", 0x4f08),
                        new FileOffset("X1LECHA.BIN",  0x6a18),
                        new FileOffset("X1HOW_J.BIN",  0x48d4),
                        new FileOffset("X1GL_J.BIN",   0x4eec),
                        new FileOffset("X1FUTT_J.BIN", 0x3f54),
                        new FileOffset("X1FNKA6.BIN",  0x64c8),
                        new FileOffset("X1FNKA01.BIN", 0x6dec),
                        new FileOffset("X1FNK6_J.BIN", 0x3f70),
                        new FileOffset("X1FNK1_J.BIN", 0x3eb0),
                        new FileOffset("X1DST_J.BIN",  0x4d30),
                        new FileOffset("X1DLMA_J.BIN", 0x4bc4),
                        new FileOffset("X1DLMA2J.BIN", 0x3ec4),
                        new FileOffset("X1BEERJI.BIN", 0x41c0),
                        new FileOffset("X1ASPI_H.BIN", 0x45b4),
                        new FileOffset("X022.BIN",     0x5f38),
                    }
                )},

                { "CBF04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0386, 0x0008, 0x0e16, 0x0025, 0x03d4, 0x002d, 0x0f49, 0x004c, 0x03f8, 0x0054, 0x0fe0 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x435c),
                        new FileOffset("X1OHRA_J.BIN", 0x428c),
                        new FileOffset("X1MAYA_J.BIN", 0x42a4),
                        new FileOffset("X1LEMO3J.BIN", 0x4800),
                        new FileOffset("X1LEMO2J.BIN", 0x4570),
                        new FileOffset("X1LEMO1J.BIN", 0x4f24),
                        new FileOffset("X1LECHA.BIN",  0x6a34),
                        new FileOffset("X1KANSHO.BIN", 0x3a3c),
                        new FileOffset("X1HOW_J.BIN",  0x48f0),
                        new FileOffset("X1GL_J.BIN",   0x4f08),
                        new FileOffset("X1FUTT_J.BIN", 0x3f70),
                        new FileOffset("X1FNKA6.BIN",  0x64e4),
                        new FileOffset("X1FNKA01.BIN", 0x6e08),
                        new FileOffset("X1FNK6_J.BIN", 0x3f8c),
                        new FileOffset("X1FNK1_J.BIN", 0x3ecc),
                        new FileOffset("X1DST_J.BIN",  0x4d4c),
                        new FileOffset("X1DLMA_J.BIN", 0x4be0),
                        new FileOffset("X1DLMA2J.BIN", 0x3ee0),
                        new FileOffset("X1BEERJI.BIN", 0x41dc),
                        new FileOffset("X1ASPI_H.BIN", 0x45d0),
                        new FileOffset("X022.BIN",     0x5f54),
                    }
                )},

                { "CBF05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x047b, 0x0009, 0x1242, 0x002e, 0x0498, 0x0038, 0x12d4, 0x005e, 0x0453, 0x0067, 0x11c7 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4378),
                        new FileOffset("X1OHRA_J.BIN", 0x42a8),
                        new FileOffset("X1MAYA_J.BIN", 0x42c0),
                        new FileOffset("X1LEMO3J.BIN", 0x481c),
                        new FileOffset("X1LEMO2J.BIN", 0x458c),
                        new FileOffset("X1LEMO1J.BIN", 0x4f40),
                        new FileOffset("X1LECHA.BIN",  0x6a50),
                        new FileOffset("X1KANSHO.BIN", 0x3a58),
                        new FileOffset("X1HOW_J.BIN",  0x490c),
                        new FileOffset("X1GL_J.BIN",   0x4f24),
                        new FileOffset("X1FUTT_J.BIN", 0x3f8c),
                        new FileOffset("X1FNKA6.BIN",  0x6500),
                        new FileOffset("X1FNKA01.BIN", 0x6e24),
                        new FileOffset("X1FNK6_J.BIN", 0x3fa8),
                        new FileOffset("X1FNK1_J.BIN", 0x3ee8),
                        new FileOffset("X1DST_J.BIN",  0x4d68),
                        new FileOffset("X1DLMA_J.BIN", 0x4bfc),
                        new FileOffset("X1DLMA2J.BIN", 0x3efc),
                        new FileOffset("X1BEERJI.BIN", 0x41f8),
                        new FileOffset("X1ASPI_H.BIN", 0x45ec),
                        new FileOffset("X022.BIN",     0x5f70),
                    }
                )},

                { "CBF06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035d, 0x0007, 0x0d46, 0x0022, 0x0398, 0x002a, 0x0e57, 0x0047, 0x033e, 0x004e, 0x0cda },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4394),
                        new FileOffset("X1OHRA_J.BIN", 0x42c4),
                        new FileOffset("X1MAYA_J.BIN", 0x42dc),
                        new FileOffset("X1LEMO3J.BIN", 0x4838),
                        new FileOffset("X1LEMO2J.BIN", 0x45a8),
                        new FileOffset("X1LEMO1J.BIN", 0x4f5c),
                        new FileOffset("X1LECHA.BIN",  0x6a6c),
                        new FileOffset("X1HOW_J.BIN",  0x4928),
                        new FileOffset("X1GL_J.BIN",   0x4f40),
                        new FileOffset("X1FUTT_J.BIN", 0x3fa8),
                        new FileOffset("X1FNKA6.BIN",  0x651c),
                        new FileOffset("X1FNKA01.BIN", 0x6e40),
                        new FileOffset("X1FNK6_J.BIN", 0x3fc4),
                        new FileOffset("X1FNK1_J.BIN", 0x3f04),
                        new FileOffset("X1DST_J.BIN",  0x4d84),
                        new FileOffset("X1DLMA_J.BIN", 0x4c18),
                        new FileOffset("X1DLMA2J.BIN", 0x3f18),
                        new FileOffset("X1BEERJI.BIN", 0x4214),
                        new FileOffset("X1ASPI_H.BIN", 0x4608),
                        new FileOffset("X022.BIN",     0x5f8c),
                    }
                )},

                { "CBF07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038c, 0x0008, 0x0ce4, 0x0022, 0x03ab, 0x002a, 0x0d4f, 0x0045, 0x05d3, 0x0051, 0x153b },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x43b0),
                        new FileOffset("X1OHRA_J.BIN", 0x42e0),
                        new FileOffset("X1MAYA_J.BIN", 0x42f8),
                        new FileOffset("X1LEMO3J.BIN", 0x4854),
                        new FileOffset("X1LEMO2J.BIN", 0x45c4),
                        new FileOffset("X1LEMO1J.BIN", 0x4f78),
                        new FileOffset("X1LECHA.BIN",  0x6a88),
                        new FileOffset("X1KANSHO.BIN", 0x3a90),
                        new FileOffset("X1HOW_J.BIN",  0x4944),
                        new FileOffset("X1GL_J.BIN",   0x4f5c),
                        new FileOffset("X1FUTT_J.BIN", 0x3fc4),
                        new FileOffset("X1FNKA6.BIN",  0x6538),
                        new FileOffset("X1FNKA01.BIN", 0x6e5c),
                        new FileOffset("X1FNK6_J.BIN", 0x3fe0),
                        new FileOffset("X1FNK1_J.BIN", 0x3f20),
                        new FileOffset("X1DST_J.BIN",  0x4da0),
                        new FileOffset("X1DLMA_J.BIN", 0x4c34),
                        new FileOffset("X1DLMA2J.BIN", 0x3f34),
                        new FileOffset("X1BEERJI.BIN", 0x4230),
                        new FileOffset("X1ASPI_H.BIN", 0x4624),
                        new FileOffset("X022.BIN",     0x5fa8),
                    }
                )},

                { "CBF08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04a1, 0x000a, 0x10e6, 0x002c, 0x04bc, 0x0036, 0x1144, 0x0059, 0x04b9, 0x0063, 0x12da },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x43cc),
                        new FileOffset("X1OHRA_J.BIN", 0x42fc),
                        new FileOffset("X1MAYA_J.BIN", 0x4314),
                        new FileOffset("X1LEMO3J.BIN", 0x4870),
                        new FileOffset("X1LEMO2J.BIN", 0x45e0),
                        new FileOffset("X1LEMO1J.BIN", 0x4f94),
                        new FileOffset("X1LECHA.BIN",  0x6aa4),
                        new FileOffset("X1HOW_J.BIN",  0x4960),
                        new FileOffset("X1GL_J.BIN",   0x4f78),
                        new FileOffset("X1FUTT_J.BIN", 0x3fe0),
                        new FileOffset("X1FNKA6.BIN",  0x6554),
                        new FileOffset("X1FNKA01.BIN", 0x6e78),
                        new FileOffset("X1FNK6_J.BIN", 0x3ffc),
                        new FileOffset("X1FNK1_J.BIN", 0x3f3c),
                        new FileOffset("X1DST_J.BIN",  0x4dbc),
                        new FileOffset("X1DLMA_J.BIN", 0x4c50),
                        new FileOffset("X1DLMA2J.BIN", 0x3f50),
                        new FileOffset("X1BEERJI.BIN", 0x424c),
                        new FileOffset("X1ASPI_H.BIN", 0x4640),
                        new FileOffset("X022.BIN",     0x5fc4),
                    }
                )},

                { "CBF09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0432, 0x0009, 0x0c5c, 0x0022, 0x0440, 0x002b, 0x0c9b, 0x0045, 0x041d, 0x004e, 0x0c2a },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x43e8),
                        new FileOffset("X1OHRA_J.BIN", 0x4318),
                        new FileOffset("X1MAYA_J.BIN", 0x4330),
                        new FileOffset("X1LEMO3J.BIN", 0x488c),
                        new FileOffset("X1LEMO2J.BIN", 0x45fc),
                        new FileOffset("X1LEMO1J.BIN", 0x4fb0),
                        new FileOffset("X1LECHA.BIN",  0x6ac0),
                        new FileOffset("X1HOW_J.BIN",  0x497c),
                        new FileOffset("X1GL_J.BIN",   0x4f94),
                        new FileOffset("X1FUTT_J.BIN", 0x3ffc),
                        new FileOffset("X1FNKA6.BIN",  0x6570),
                        new FileOffset("X1FNKA01.BIN", 0x6e94),
                        new FileOffset("X1FNK6_J.BIN", 0x4018),
                        new FileOffset("X1FNK1_J.BIN", 0x3f58),
                        new FileOffset("X1DST_J.BIN",  0x4dd8),
                        new FileOffset("X1DLMA_J.BIN", 0x4c6c),
                        new FileOffset("X1DLMA2J.BIN", 0x3f6c),
                        new FileOffset("X1BEERJI.BIN", 0x4268),
                        new FileOffset("X1ASPI_H.BIN", 0x465c),
                        new FileOffset("X022.BIN",     0x5fe0),
                    }
                )},

                { "CBF10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04c3, 0x000a, 0x1164, 0x002d, 0x03c2, 0x0035, 0x0f44, 0x0054, 0x037f, 0x005b, 0x0cdb },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4404),
                        new FileOffset("X1OHRA_J.BIN", 0x4334),
                        new FileOffset("X1MAYA_J.BIN", 0x434c),
                        new FileOffset("X1LEMO3J.BIN", 0x48a8),
                        new FileOffset("X1LEMO2J.BIN", 0x4618),
                        new FileOffset("X1LEMO1J.BIN", 0x4fcc),
                        new FileOffset("X1LECHA.BIN",  0x6adc),
                        new FileOffset("X1HOW_J.BIN",  0x4998),
                        new FileOffset("X1GL_J.BIN",   0x4fb0),
                        new FileOffset("X1FUTT_J.BIN", 0x4018),
                        new FileOffset("X1FNKA6.BIN",  0x658c),
                        new FileOffset("X1FNKA01.BIN", 0x6eb0),
                        new FileOffset("X1FNK6_J.BIN", 0x4034),
                        new FileOffset("X1FNK1_J.BIN", 0x3f74),
                        new FileOffset("X1DST_J.BIN",  0x4df4),
                        new FileOffset("X1DLMA_J.BIN", 0x4c88),
                        new FileOffset("X1DLMA2J.BIN", 0x3f88),
                        new FileOffset("X1BEERJI.BIN", 0x4284),
                        new FileOffset("X1ASPI_H.BIN", 0x4678),
                        new FileOffset("X022.BIN",     0x5ffc),
                    }
                )},

                { "CBF11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x0ef8, 0x0026, 0x03c6, 0x002e, 0x0ef8, 0x004c, 0x03d4, 0x0054, 0x0f41 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4420),
                        new FileOffset("X1OHRA_J.BIN", 0x4350),
                        new FileOffset("X1MAYA_J.BIN", 0x4368),
                        new FileOffset("X1LEMO3J.BIN", 0x48c4),
                        new FileOffset("X1LEMO2J.BIN", 0x4634),
                        new FileOffset("X1LEMO1J.BIN", 0x4fe8),
                        new FileOffset("X1LECHA.BIN",  0x6af8),
                        new FileOffset("X1HOW_J.BIN",  0x49b4),
                        new FileOffset("X1GL_J.BIN",   0x4fcc),
                        new FileOffset("X1FUTT_J.BIN", 0x4034),
                        new FileOffset("X1FNKA6.BIN",  0x65a8),
                        new FileOffset("X1FNKA01.BIN", 0x6ecc),
                        new FileOffset("X1FNK6_J.BIN", 0x4050),
                        new FileOffset("X1FNK1_J.BIN", 0x3f90),
                        new FileOffset("X1DST_J.BIN",  0x4e10),
                        new FileOffset("X1DLMA_J.BIN", 0x4ca4),
                        new FileOffset("X1DLMA2J.BIN", 0x3fa4),
                        new FileOffset("X1BEERJI.BIN", 0x42a0),
                        new FileOffset("X1ASPI_H.BIN", 0x4694),
                        new FileOffset("X022.BIN",     0x6018),
                    }
                )},

                { "CBF12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x042e, 0x0009, 0x107f, 0x002a, 0x042e, 0x0033, 0x107f, 0x0054, 0x0419, 0x005d, 0x105e },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x443c),
                        new FileOffset("X1OHRA_J.BIN", 0x436c),
                        new FileOffset("X1MAYA_J.BIN", 0x4384),
                        new FileOffset("X1LEMO3J.BIN", 0x48e0),
                        new FileOffset("X1LEMO2J.BIN", 0x4650),
                        new FileOffset("X1LEMO1J.BIN", 0x5004),
                        new FileOffset("X1LECHA.BIN",  0x6b14),
                        new FileOffset("X1HOW_J.BIN",  0x49d0),
                        new FileOffset("X1GL_J.BIN",   0x4fe8),
                        new FileOffset("X1FUTT_J.BIN", 0x4050),
                        new FileOffset("X1FNKA6.BIN",  0x65c4),
                        new FileOffset("X1FNKA01.BIN", 0x6ee8),
                        new FileOffset("X1FNK6_J.BIN", 0x406c),
                        new FileOffset("X1FNK1_J.BIN", 0x3fac),
                        new FileOffset("X1DST_J.BIN",  0x4e2c),
                        new FileOffset("X1DLMA_J.BIN", 0x4cc0),
                        new FileOffset("X1DLMA2J.BIN", 0x3fc0),
                        new FileOffset("X1BEERJI.BIN", 0x42bc),
                        new FileOffset("X1ASPI_H.BIN", 0x46b0),
                        new FileOffset("X022.BIN",     0x6034),
                    }
                )},

                { "CBF13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0453, 0x0009, 0x0eb6, 0x0027, 0x0453, 0x0030, 0x0eb6, 0x004e, 0x044a, 0x0057, 0x115b },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4458),
                        new FileOffset("X1OHRA_J.BIN", 0x4388),
                        new FileOffset("X1MAYA_J.BIN", 0x43a0),
                        new FileOffset("X1LEMO3J.BIN", 0x48fc),
                        new FileOffset("X1LEMO2J.BIN", 0x466c),
                        new FileOffset("X1LEMO1J.BIN", 0x5020),
                        new FileOffset("X1LECHA.BIN",  0x6b30),
                        new FileOffset("X1HOW_J.BIN",  0x49ec),
                        new FileOffset("X1GL_J.BIN",   0x5004),
                        new FileOffset("X1FUTT_J.BIN", 0x406c),
                        new FileOffset("X1FNKA6.BIN",  0x65e0),
                        new FileOffset("X1FNKA01.BIN", 0x6f04),
                        new FileOffset("X1FNK6_J.BIN", 0x4088),
                        new FileOffset("X1FNK1_J.BIN", 0x3fc8),
                        new FileOffset("X1DST_J.BIN",  0x4e48),
                        new FileOffset("X1DLMA_J.BIN", 0x4cdc),
                        new FileOffset("X1DLMA2J.BIN", 0x3fdc),
                        new FileOffset("X1BEERJI.BIN", 0x42d8),
                        new FileOffset("X1ASPI_H.BIN", 0x46cc),
                        new FileOffset("X022.BIN",     0x6050),
                    }
                )},

                { "CBF14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x0b2a, 0x001d, 0x02c2, 0x0023, 0x0aad, 0x0039, 0x03a4, 0x0041, 0x0e38 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4474),
                        new FileOffset("X1OHRA_J.BIN", 0x43a4),
                        new FileOffset("X1MAYA_J.BIN", 0x43bc),
                        new FileOffset("X1LEMO3J.BIN", 0x4918),
                        new FileOffset("X1LEMO2J.BIN", 0x4688),
                        new FileOffset("X1LEMO1J.BIN", 0x503c),
                        new FileOffset("X1LECHA.BIN",  0x6b4c),
                        new FileOffset("X1HOW_J.BIN",  0x4a08),
                        new FileOffset("X1GL_J.BIN",   0x5020),
                        new FileOffset("X1FUTT_J.BIN", 0x4088),
                        new FileOffset("X1FNKA6.BIN",  0x65fc),
                        new FileOffset("X1FNKA01.BIN", 0x6f20),
                        new FileOffset("X1FNK6_J.BIN", 0x40a4),
                        new FileOffset("X1FNK1_J.BIN", 0x3fe4),
                        new FileOffset("X1DST_J.BIN",  0x4e64),
                        new FileOffset("X1DLMA_J.BIN", 0x4cf8),
                        new FileOffset("X1DLMA2J.BIN", 0x3ff8),
                        new FileOffset("X1BEERJI.BIN", 0x42f4),
                        new FileOffset("X1ASPI_H.BIN", 0x46e8),
                        new FileOffset("X022.BIN",     0x606c),
                    }
                )},

                { "CBF15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0525, 0x000b, 0x1442, 0x0034, 0x0525, 0x003f, 0x1442, 0x0068, 0x04f8, 0x0072, 0x13a0 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4490),
                        new FileOffset("X1OHRA_J.BIN", 0x43c0),
                        new FileOffset("X1MAYA_J.BIN", 0x43d8),
                        new FileOffset("X1LEMO3J.BIN", 0x4934),
                        new FileOffset("X1LEMO2J.BIN", 0x46a4),
                        new FileOffset("X1LEMO1J.BIN", 0x5058),
                        new FileOffset("X1LECHA.BIN",  0x6b68),
                        new FileOffset("X1HOW_J.BIN",  0x4a24),
                        new FileOffset("X1GL_J.BIN",   0x503c),
                        new FileOffset("X1FUTT_J.BIN", 0x40a4),
                        new FileOffset("X1FNKA6.BIN",  0x6618),
                        new FileOffset("X1FNKA01.BIN", 0x6f3c),
                        new FileOffset("X1FNK6_J.BIN", 0x40c0),
                        new FileOffset("X1FNK1_J.BIN", 0x4000),
                        new FileOffset("X1DST_J.BIN",  0x4e80),
                        new FileOffset("X1DLMA_J.BIN", 0x4d14),
                        new FileOffset("X1DLMA2J.BIN", 0x4014),
                        new FileOffset("X1BEERJI.BIN", 0x4310),
                        new FileOffset("X1ASPI_H.BIN", 0x4704),
                        new FileOffset("X022.BIN",     0x6088),
                    }
                )},

                { "CBF16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x044b, 0x0009, 0x0b5b, 0x0020, 0x0455, 0x0029, 0x0b5b, 0x0040, 0x044a, 0x0049, 0x1110 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x44ac),
                        new FileOffset("X1OHRA_J.BIN", 0x43dc),
                        new FileOffset("X1MAYA_J.BIN", 0x43f4),
                        new FileOffset("X1LEMO3J.BIN", 0x4950),
                        new FileOffset("X1LEMO2J.BIN", 0x46c0),
                        new FileOffset("X1LEMO1J.BIN", 0x5074),
                        new FileOffset("X1LECHA.BIN",  0x6b84),
                        new FileOffset("X1KANSHO.BIN", 0x3b8c),
                        new FileOffset("X1HOW_J.BIN",  0x4a40),
                        new FileOffset("X1GL_J.BIN",   0x5058),
                        new FileOffset("X1FUTT_J.BIN", 0x40c0),
                        new FileOffset("X1FNKA6.BIN",  0x6634),
                        new FileOffset("X1FNKA01.BIN", 0x6f58),
                        new FileOffset("X1FNK6_J.BIN", 0x40dc),
                        new FileOffset("X1FNK1_J.BIN", 0x401c),
                        new FileOffset("X1DST_J.BIN",  0x4e9c),
                        new FileOffset("X1DLMA_J.BIN", 0x4d30),
                        new FileOffset("X1DLMA2J.BIN", 0x4030),
                        new FileOffset("X1BEERJI.BIN", 0x432c),
                        new FileOffset("X1ASPI_H.BIN", 0x4720),
                        new FileOffset("X022.BIN",     0x60a4),
                    }
                )},

                { "CBF17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x034e, 0x0007, 0x0c1f, 0x0020, 0x034e, 0x0027, 0x0c1f, 0x0040, 0x0338, 0x0047, 0x0bc9 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x44c8),
                        new FileOffset("X1OHRA_J.BIN", 0x43f8),
                        new FileOffset("X1MAYA_J.BIN", 0x4410),
                        new FileOffset("X1LEMO3J.BIN", 0x496c),
                        new FileOffset("X1LEMO2J.BIN", 0x46dc),
                        new FileOffset("X1LEMO1J.BIN", 0x5090),
                        new FileOffset("X1LECHA.BIN",  0x6ba0),
                        new FileOffset("X1HOW_J.BIN",  0x4a5c),
                        new FileOffset("X1GL_J.BIN",   0x5074),
                        new FileOffset("X1FUTT_J.BIN", 0x40dc),
                        new FileOffset("X1FNKA6.BIN",  0x6650),
                        new FileOffset("X1FNKA01.BIN", 0x6f74),
                        new FileOffset("X1FNK6_J.BIN", 0x40f8),
                        new FileOffset("X1FNK1_J.BIN", 0x4038),
                        new FileOffset("X1DST_J.BIN",  0x4eb8),
                        new FileOffset("X1DLMA_J.BIN", 0x4d4c),
                        new FileOffset("X1DLMA2J.BIN", 0x404c),
                        new FileOffset("X1BEERJI.BIN", 0x4348),
                        new FileOffset("X1ASPI_H.BIN", 0x473c),
                        new FileOffset("X022.BIN",     0x60c0),
                    }
                )},

                { "CBF18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c4, 0x0008, 0x0dbe, 0x0024, 0x03c4, 0x002c, 0x0dbe, 0x0048, 0x03d7, 0x0050, 0x0e08 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x44e4),
                        new FileOffset("X1OHRA_J.BIN", 0x4414),
                        new FileOffset("X1MAYA_J.BIN", 0x442c),
                        new FileOffset("X1LEMO3J.BIN", 0x4988),
                        new FileOffset("X1LEMO2J.BIN", 0x46f8),
                        new FileOffset("X1LEMO1J.BIN", 0x50ac),
                        new FileOffset("X1LECHA.BIN",  0x6bbc),
                        new FileOffset("X1HOW_J.BIN",  0x4a78),
                        new FileOffset("X1GL_J.BIN",   0x5090),
                        new FileOffset("X1FUTT_J.BIN", 0x40f8),
                        new FileOffset("X1FNKA6.BIN",  0x666c),
                        new FileOffset("X1FNKA01.BIN", 0x6f90),
                        new FileOffset("X1FNK6_J.BIN", 0x4114),
                        new FileOffset("X1FNK1_J.BIN", 0x4054),
                        new FileOffset("X1DST_J.BIN",  0x4ed4),
                        new FileOffset("X1DLMA_J.BIN", 0x4d68),
                        new FileOffset("X1DLMA2J.BIN", 0x4068),
                        new FileOffset("X1BEERJI.BIN", 0x4364),
                        new FileOffset("X1ASPI_H.BIN", 0x4758),
                        new FileOffset("X022.BIN",     0x60dc),
                    }
                )},

                { "CBF19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0415, 0x0009, 0x0c08, 0x0022, 0x0415, 0x002b, 0x0c08, 0x0044, 0x040c, 0x004d, 0x0bf5 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4500),
                        new FileOffset("X1OHRA_J.BIN", 0x4430),
                        new FileOffset("X1MAYA_J.BIN", 0x4448),
                        new FileOffset("X1LEMO3J.BIN", 0x49a4),
                        new FileOffset("X1LEMO2J.BIN", 0x4714),
                        new FileOffset("X1LEMO1J.BIN", 0x50c8),
                        new FileOffset("X1LECHA.BIN",  0x6bd8),
                        new FileOffset("X1HOW_J.BIN",  0x4a94),
                        new FileOffset("X1GL_J.BIN",   0x50ac),
                        new FileOffset("X1FUTT_J.BIN", 0x4114),
                        new FileOffset("X1FNKA6.BIN",  0x6688),
                        new FileOffset("X1FNKA01.BIN", 0x6fac),
                        new FileOffset("X1FNK6_J.BIN", 0x4130),
                        new FileOffset("X1FNK1_J.BIN", 0x4070),
                        new FileOffset("X1DST_J.BIN",  0x4ef0),
                        new FileOffset("X1DLMA_J.BIN", 0x4d84),
                        new FileOffset("X1DLMA2J.BIN", 0x4084),
                        new FileOffset("X1BEERJI.BIN", 0x4380),
                        new FileOffset("X1ASPI_H.BIN", 0x4774),
                        new FileOffset("X022.BIN",     0x60f8),
                    }
                )},

                { "CBF20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x0e94, 0x0027, 0x0434, 0x0030, 0x0f70, 0x004f, 0x06a8, 0x005d, 0x1819 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x451c),
                        new FileOffset("X1OHRA_J.BIN", 0x444c),
                        new FileOffset("X1MAYA_J.BIN", 0x4464),
                        new FileOffset("X1LEMO3J.BIN", 0x49c0),
                        new FileOffset("X1LEMO2J.BIN", 0x4730),
                        new FileOffset("X1LEMO1J.BIN", 0x50e4),
                        new FileOffset("X1LECHA.BIN",  0x6bf4),
                        new FileOffset("X1HOW_J.BIN",  0x4ab0),
                        new FileOffset("X1GL_J.BIN",   0x50c8),
                        new FileOffset("X1FUTT_J.BIN", 0x4130),
                        new FileOffset("X1FNKA6.BIN",  0x66a4),
                        new FileOffset("X1FNKA01.BIN", 0x6fc8),
                        new FileOffset("X1FNK6_J.BIN", 0x414c),
                        new FileOffset("X1FNK1_J.BIN", 0x408c),
                        new FileOffset("X1DST_J.BIN",  0x4f0c),
                        new FileOffset("X1DLMA_J.BIN", 0x4da0),
                        new FileOffset("X1DLMA2J.BIN", 0x40a0),
                        new FileOffset("X1BEERJI.BIN", 0x439c),
                        new FileOffset("X1ASPI_H.BIN", 0x4790),
                        new FileOffset("X022.BIN",     0x6114),
                    }
                )},

                { "CBF21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0542, 0x000b, 0x0fa4, 0x002b, 0x0426, 0x0034, 0x0f1d, 0x0053, 0x0447, 0x005c, 0x0ef1 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4538),
                        new FileOffset("X1OHRA_J.BIN", 0x4468),
                        new FileOffset("X1MAYA_J.BIN", 0x4480),
                        new FileOffset("X1LEMO3J.BIN", 0x49dc),
                        new FileOffset("X1LEMO2J.BIN", 0x474c),
                        new FileOffset("X1LEMO1J.BIN", 0x5100),
                        new FileOffset("X1LECHA.BIN",  0x6c10),
                        new FileOffset("X1HOW_J.BIN",  0x4acc),
                        new FileOffset("X1GL_J.BIN",   0x50e4),
                        new FileOffset("X1FUTT_J.BIN", 0x414c),
                        new FileOffset("X1FNKA6.BIN",  0x66c0),
                        new FileOffset("X1FNKA01.BIN", 0x6fe4),
                        new FileOffset("X1FNK6_J.BIN", 0x4168),
                        new FileOffset("X1FNK1_J.BIN", 0x40a8),
                        new FileOffset("X1DST_J.BIN",  0x4f28),
                        new FileOffset("X1DLMA_J.BIN", 0x4dbc),
                        new FileOffset("X1DLMA2J.BIN", 0x40bc),
                        new FileOffset("X1BEERJI.BIN", 0x43b8),
                        new FileOffset("X1ASPI_H.BIN", 0x47ac),
                        new FileOffset("X022.BIN",     0x6130),
                    }
                )},

                { "CBF22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0e7b, 0x0025, 0x03a6, 0x002d, 0x0ee7, 0x004b, 0x03b3, 0x0053, 0x0f1d },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4554),
                        new FileOffset("X1OHRA_J.BIN", 0x4484),
                        new FileOffset("X1MAYA_J.BIN", 0x449c),
                        new FileOffset("X1LEMO3J.BIN", 0x49f8),
                        new FileOffset("X1LEMO2J.BIN", 0x4768),
                        new FileOffset("X1LEMO1J.BIN", 0x511c),
                        new FileOffset("X1LECHA.BIN",  0x6c2c),
                        new FileOffset("X1HOW_J.BIN",  0x4ae8),
                        new FileOffset("X1GL_J.BIN",   0x5100),
                        new FileOffset("X1FUTT_J.BIN", 0x4168),
                        new FileOffset("X1FNKA6.BIN",  0x66dc),
                        new FileOffset("X1FNKA01.BIN", 0x7000),
                        new FileOffset("X1FNK6_J.BIN", 0x4184),
                        new FileOffset("X1FNK1_J.BIN", 0x40c4),
                        new FileOffset("X1DST_J.BIN",  0x4f44),
                        new FileOffset("X1DLMA_J.BIN", 0x4dd8),
                        new FileOffset("X1DLMA2J.BIN", 0x40d8),
                        new FileOffset("X1BEERJI.BIN", 0x43d4),
                        new FileOffset("X1ASPI_H.BIN", 0x47c8),
                        new FileOffset("X022.BIN",     0x614c),
                    }
                )},

                { "CBF23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0472, 0x0009, 0x1043, 0x002a, 0x04a8, 0x0034, 0x10fc, 0x0056, 0x0476, 0x005f, 0x11ee },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4570),
                        new FileOffset("X1OHRA_J.BIN", 0x44a0),
                        new FileOffset("X1MAYA_J.BIN", 0x44b8),
                        new FileOffset("X1LEMO3J.BIN", 0x4a14),
                        new FileOffset("X1LEMO2J.BIN", 0x4784),
                        new FileOffset("X1LEMO1J.BIN", 0x5138),
                        new FileOffset("X1LECHA.BIN",  0x6c48),
                        new FileOffset("X1HOW_J.BIN",  0x4b04),
                        new FileOffset("X1GL_J.BIN",   0x511c),
                        new FileOffset("X1FUTT_J.BIN", 0x4184),
                        new FileOffset("X1FNKA6.BIN",  0x66f8),
                        new FileOffset("X1FNKA01.BIN", 0x701c),
                        new FileOffset("X1FNK6_J.BIN", 0x41a0),
                        new FileOffset("X1FNK1_J.BIN", 0x40e0),
                        new FileOffset("X1DST_J.BIN",  0x4f60),
                        new FileOffset("X1DLMA_J.BIN", 0x4df4),
                        new FileOffset("X1DLMA2J.BIN", 0x40f4),
                        new FileOffset("X1BEERJI.BIN", 0x43f0),
                        new FileOffset("X1ASPI_H.BIN", 0x47e4),
                        new FileOffset("X022.BIN",     0x6168),
                    }
                )},

                { "CBF24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0431, 0x0009, 0x0e68, 0x0026, 0x0431, 0x002f, 0x0f39, 0x004e, 0x0433, 0x0057, 0x10e1 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x458c),
                        new FileOffset("X1OHRA_J.BIN", 0x44bc),
                        new FileOffset("X1MAYA_J.BIN", 0x44d4),
                        new FileOffset("X1LEMO3J.BIN", 0x4a30),
                        new FileOffset("X1LEMO2J.BIN", 0x47a0),
                        new FileOffset("X1LEMO1J.BIN", 0x5154),
                        new FileOffset("X1LECHA.BIN",  0x6c64),
                        new FileOffset("X1HOW_J.BIN",  0x4b20),
                        new FileOffset("X1GL_J.BIN",   0x5138),
                        new FileOffset("X1FUTT_J.BIN", 0x41a0),
                        new FileOffset("X1FNKA6.BIN",  0x6714),
                        new FileOffset("X1FNKA01.BIN", 0x7038),
                        new FileOffset("X1FNK6_J.BIN", 0x41bc),
                        new FileOffset("X1FNK1_J.BIN", 0x40fc),
                        new FileOffset("X1DST_J.BIN",  0x4f7c),
                        new FileOffset("X1DLMA_J.BIN", 0x4e10),
                        new FileOffset("X1DLMA2J.BIN", 0x4110),
                        new FileOffset("X1BEERJI.BIN", 0x440c),
                        new FileOffset("X1ASPI_H.BIN", 0x4800),
                        new FileOffset("X022.BIN",     0x6184),
                    }
                )},

                { "CBF25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04e1, 0x000a, 0x11c1, 0x002e, 0x04de, 0x0038, 0x11c1, 0x005c, 0x04f8, 0x0066, 0x1223 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x45a8),
                        new FileOffset("X1OHRA_J.BIN", 0x44d8),
                        new FileOffset("X1MAYA_J.BIN", 0x44f0),
                        new FileOffset("X1LEMO3J.BIN", 0x4a4c),
                        new FileOffset("X1LEMO2J.BIN", 0x47bc),
                        new FileOffset("X1LEMO1J.BIN", 0x5170),
                        new FileOffset("X1LECHA.BIN",  0x6c80),
                        new FileOffset("X1HOW_J.BIN",  0x4b3c),
                        new FileOffset("X1GL_J.BIN",   0x5154),
                        new FileOffset("X1FUTT_J.BIN", 0x41bc),
                        new FileOffset("X1FNKA6.BIN",  0x6730),
                        new FileOffset("X1FNKA01.BIN", 0x7054),
                        new FileOffset("X1FNK6_J.BIN", 0x41d8),
                        new FileOffset("X1FNK1_J.BIN", 0x4118),
                        new FileOffset("X1DST_J.BIN",  0x4f98),
                        new FileOffset("X1DLMA_J.BIN", 0x4e2c),
                        new FileOffset("X1DLMA2J.BIN", 0x412c),
                        new FileOffset("X1BEERJI.BIN", 0x4428),
                        new FileOffset("X1ASPI_H.BIN", 0x481c),
                        new FileOffset("X022.BIN",     0x61a0),
                    }
                )},

                { "CBF26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ac, 0x0008, 0x0ea2, 0x0026, 0x03fb, 0x002e, 0x0ea2, 0x004c, 0x03a8, 0x0054, 0x0d83 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x45c4),
                        new FileOffset("X1OHRA_J.BIN", 0x44f4),
                        new FileOffset("X1MAYA_J.BIN", 0x450c),
                        new FileOffset("X1LEMO3J.BIN", 0x4a68),
                        new FileOffset("X1LEMO2J.BIN", 0x47d8),
                        new FileOffset("X1LEMO1J.BIN", 0x518c),
                        new FileOffset("X1LECHA.BIN",  0x6c9c),
                        new FileOffset("X1HOW_J.BIN",  0x4b58),
                        new FileOffset("X1GL_J.BIN",   0x5170),
                        new FileOffset("X1FUTT_J.BIN", 0x41d8),
                        new FileOffset("X1FNKA6.BIN",  0x674c),
                        new FileOffset("X1FNKA01.BIN", 0x7070),
                        new FileOffset("X1FNK6_J.BIN", 0x41f4),
                        new FileOffset("X1FNK1_J.BIN", 0x4134),
                        new FileOffset("X1DST_J.BIN",  0x4fb4),
                        new FileOffset("X1DLMA_J.BIN", 0x4e48),
                        new FileOffset("X1DLMA2J.BIN", 0x4148),
                        new FileOffset("X1BEERJI.BIN", 0x4444),
                        new FileOffset("X1ASPI_H.BIN", 0x4838),
                        new FileOffset("X022.BIN",     0x61bc),
                    }
                )},

                { "CBF27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048c, 0x000a, 0x0d5f, 0x0025, 0x0497, 0x002f, 0x0d82, 0x004b, 0x07d2, 0x005b, 0x0dff },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x45e0),
                        new FileOffset("X1OHRA_J.BIN", 0x4510),
                        new FileOffset("X1MAYA_J.BIN", 0x4528),
                        new FileOffset("X1LEMO3J.BIN", 0x4a84),
                        new FileOffset("X1LEMO2J.BIN", 0x47f4),
                        new FileOffset("X1LEMO1J.BIN", 0x51a8),
                        new FileOffset("X1LECHA.BIN",  0x6cb8),
                        new FileOffset("X1HOW_J.BIN",  0x4b74),
                        new FileOffset("X1GL_J.BIN",   0x518c),
                        new FileOffset("X1FUTT_J.BIN", 0x41f4),
                        new FileOffset("X1FNKA6.BIN",  0x6768),
                        new FileOffset("X1FNKA01.BIN", 0x708c),
                        new FileOffset("X1FNK6_J.BIN", 0x4210),
                        new FileOffset("X1FNK1_J.BIN", 0x4150),
                        new FileOffset("X1DST_J.BIN",  0x4fd0),
                        new FileOffset("X1DLMA_J.BIN", 0x4e64),
                        new FileOffset("X1DLMA2J.BIN", 0x4164),
                        new FileOffset("X1BEERJI.BIN", 0x4460),
                        new FileOffset("X1ASPI_H.BIN", 0x4854),
                        new FileOffset("X022.BIN",     0x61d8),
                    }
                )},

                { "CBF28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03de, 0x0008, 0x0cdd, 0x0022, 0x039d, 0x002a, 0x0d25, 0x0045, 0x0393, 0x004d, 0x0cfd },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x45fc),
                        new FileOffset("X1OHRA_J.BIN", 0x452c),
                        new FileOffset("X1MAYA_J.BIN", 0x4544),
                        new FileOffset("X1LEMO3J.BIN", 0x4aa0),
                        new FileOffset("X1LEMO2J.BIN", 0x4810),
                        new FileOffset("X1LEMO1J.BIN", 0x51c4),
                        new FileOffset("X1LECHA.BIN",  0x6cd4),
                        new FileOffset("X1HOW_J.BIN",  0x4b90),
                        new FileOffset("X1GL_J.BIN",   0x51a8),
                        new FileOffset("X1FUTT_J.BIN", 0x4210),
                        new FileOffset("X1FNKA6.BIN",  0x6784),
                        new FileOffset("X1FNKA01.BIN", 0x70a8),
                        new FileOffset("X1FNK6_J.BIN", 0x422c),
                        new FileOffset("X1FNK1_J.BIN", 0x416c),
                        new FileOffset("X1DST_J.BIN",  0x4fec),
                        new FileOffset("X1DLMA_J.BIN", 0x4e80),
                        new FileOffset("X1DLMA2J.BIN", 0x4180),
                        new FileOffset("X1BEERJI.BIN", 0x447c),
                        new FileOffset("X1ASPI_H.BIN", 0x4870),
                        new FileOffset("X022.BIN",     0x61f4),
                    }
                )},

                { "CBF29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0353, 0x0007, 0x0e44, 0x0024, 0x03f4, 0x002c, 0x0e5c, 0x0049, 0x036f, 0x0050, 0x0dd9 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4618),
                        new FileOffset("X1OHRA_J.BIN", 0x4548),
                        new FileOffset("X1MAYA_J.BIN", 0x4560),
                        new FileOffset("X1LEMO3J.BIN", 0x4abc),
                        new FileOffset("X1LEMO2J.BIN", 0x482c),
                        new FileOffset("X1LEMO1J.BIN", 0x51e0),
                        new FileOffset("X1LECHA.BIN",  0x6cf0),
                        new FileOffset("X1HOW_J.BIN",  0x4bac),
                        new FileOffset("X1GL_J.BIN",   0x51c4),
                        new FileOffset("X1FUTT_J.BIN", 0x422c),
                        new FileOffset("X1FNKA6.BIN",  0x67a0),
                        new FileOffset("X1FNKA01.BIN", 0x70c4),
                        new FileOffset("X1FNK6_J.BIN", 0x4248),
                        new FileOffset("X1FNK1_J.BIN", 0x4188),
                        new FileOffset("X1DST_J.BIN",  0x5008),
                        new FileOffset("X1DLMA_J.BIN", 0x4e9c),
                        new FileOffset("X1DLMA2J.BIN", 0x419c),
                        new FileOffset("X1BEERJI.BIN", 0x4498),
                        new FileOffset("X1ASPI_H.BIN", 0x488c),
                        new FileOffset("X022.BIN",     0x6210),
                    }
                )},

                { "CBF30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x0fa7, 0x0028, 0x039c, 0x0030, 0x0e87, 0x004e, 0x0454, 0x0057, 0x0e95 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4634),
                        new FileOffset("X1OHRA_J.BIN", 0x4564),
                        new FileOffset("X1MAYA_J.BIN", 0x457c),
                        new FileOffset("X1LEMO3J.BIN", 0x4ad8),
                        new FileOffset("X1LEMO2J.BIN", 0x4848),
                        new FileOffset("X1LEMO1J.BIN", 0x51fc),
                        new FileOffset("X1LECHA.BIN",  0x6d0c),
                        new FileOffset("X1HOW_J.BIN",  0x4bc8),
                        new FileOffset("X1GL_J.BIN",   0x51e0),
                        new FileOffset("X1FUTT_J.BIN", 0x4248),
                        new FileOffset("X1FNKA6.BIN",  0x67bc),
                        new FileOffset("X1FNKA01.BIN", 0x70e0),
                        new FileOffset("X1FNK6_J.BIN", 0x4264),
                        new FileOffset("X1FNK1_J.BIN", 0x41a4),
                        new FileOffset("X1DST_J.BIN",  0x5024),
                        new FileOffset("X1DLMA_J.BIN", 0x4eb8),
                        new FileOffset("X1DLMA2J.BIN", 0x41b8),
                        new FileOffset("X1BEERJI.BIN", 0x44b4),
                        new FileOffset("X1ASPI_H.BIN", 0x48a8),
                        new FileOffset("X022.BIN",     0x622c),
                    }
                )},

                { "CBF31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0389, 0x0008, 0x0ce5, 0x0022, 0x0389, 0x002a, 0x0ce5, 0x0044, 0x03db, 0x004c, 0x0dff },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4650),
                        new FileOffset("X1OHRA_J.BIN", 0x4580),
                        new FileOffset("X1MAYA_J.BIN", 0x4598),
                        new FileOffset("X1LEMO3J.BIN", 0x4af4),
                        new FileOffset("X1LEMO2J.BIN", 0x4864),
                        new FileOffset("X1LEMO1J.BIN", 0x5218),
                        new FileOffset("X1LECHA.BIN",  0x6d28),
                        new FileOffset("X1HOW_J.BIN",  0x4be4),
                        new FileOffset("X1GL_J.BIN",   0x51fc),
                        new FileOffset("X1FUTT_J.BIN", 0x4264),
                        new FileOffset("X1FNKA6.BIN",  0x67d8),
                        new FileOffset("X1FNKA01.BIN", 0x70fc),
                        new FileOffset("X1FNK6_J.BIN", 0x4280),
                        new FileOffset("X1FNK1_J.BIN", 0x41c0),
                        new FileOffset("X1DST_J.BIN",  0x5040),
                        new FileOffset("X1DLMA_J.BIN", 0x4ed4),
                        new FileOffset("X1DLMA2J.BIN", 0x41d4),
                        new FileOffset("X1BEERJI.BIN", 0x44d0),
                        new FileOffset("X1ASPI_H.BIN", 0x48c4),
                        new FileOffset("X022.BIN",     0x6248),
                    }
                )},

                { "CBF32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e4, 0x0008, 0x0e2c, 0x0025, 0x03e4, 0x002d, 0x0e2c, 0x004a, 0x03ed, 0x0052, 0x0e22 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x466c),
                        new FileOffset("X1OHRA_J.BIN", 0x459c),
                        new FileOffset("X1MAYA_J.BIN", 0x45b4),
                        new FileOffset("X1LEMO3J.BIN", 0x4b10),
                        new FileOffset("X1LEMO2J.BIN", 0x4880),
                        new FileOffset("X1LEMO1J.BIN", 0x5234),
                        new FileOffset("X1LECHA.BIN",  0x6d44),
                        new FileOffset("X1HOW_J.BIN",  0x4c00),
                        new FileOffset("X1GL_J.BIN",   0x5218),
                        new FileOffset("X1FUTT_J.BIN", 0x4280),
                        new FileOffset("X1FNKA6.BIN",  0x67f4),
                        new FileOffset("X1FNKA01.BIN", 0x7118),
                        new FileOffset("X1FNK6_J.BIN", 0x429c),
                        new FileOffset("X1FNK1_J.BIN", 0x41dc),
                        new FileOffset("X1DST_J.BIN",  0x505c),
                        new FileOffset("X1DLMA_J.BIN", 0x4ef0),
                        new FileOffset("X1DLMA2J.BIN", 0x41f0),
                        new FileOffset("X1BEERJI.BIN", 0x44ec),
                        new FileOffset("X1ASPI_H.BIN", 0x48e0),
                        new FileOffset("X022.BIN",     0x6264),
                    }
                )},

                { "CBF33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0865, 0x0011, 0x0d2f, 0x002c, 0x0865, 0x003d, 0x0d2f, 0x0058, 0x0344, 0x005f, 0x0bec },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4688),
                        new FileOffset("X1OHRA_J.BIN", 0x45b8),
                        new FileOffset("X1MAYA_J.BIN", 0x45d0),
                        new FileOffset("X1LEMO3J.BIN", 0x4b2c),
                        new FileOffset("X1LEMO2J.BIN", 0x489c),
                        new FileOffset("X1LEMO1J.BIN", 0x5250),
                        new FileOffset("X1LECHA.BIN",  0x6d60),
                        new FileOffset("X1HOW_J.BIN",  0x4c1c),
                        new FileOffset("X1GL_J.BIN",   0x5234),
                        new FileOffset("X1FUTT_J.BIN", 0x429c),
                        new FileOffset("X1FNKA6.BIN",  0x6810),
                        new FileOffset("X1FNKA01.BIN", 0x7134),
                        new FileOffset("X1FNK6_J.BIN", 0x42b8),
                        new FileOffset("X1FNK1_J.BIN", 0x41f8),
                        new FileOffset("X1DST_J.BIN",  0x5078),
                        new FileOffset("X1DLMA_J.BIN", 0x4f0c),
                        new FileOffset("X1DLMA2J.BIN", 0x420c),
                        new FileOffset("X1BEERJI.BIN", 0x4508),
                        new FileOffset("X1ASPI_H.BIN", 0x48fc),
                        new FileOffset("X022.BIN",     0x6280),
                    }
                )},

                { "CBF34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0396, 0x0008, 0x0e7f, 0x0025, 0x0396, 0x002d, 0x0e7f, 0x004a, 0x0402, 0x0053, 0x100c },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x46a4),
                        new FileOffset("X1OHRA_J.BIN", 0x45d4),
                        new FileOffset("X1MAYA_J.BIN", 0x45ec),
                        new FileOffset("X1LEMO3J.BIN", 0x4b48),
                        new FileOffset("X1LEMO2J.BIN", 0x48b8),
                        new FileOffset("X1LEMO1J.BIN", 0x526c),
                        new FileOffset("X1LECHA.BIN",  0x6d7c),
                        new FileOffset("X1KANSHO.BIN", 0x3d84),
                        new FileOffset("X1HOW_J.BIN",  0x4c38),
                        new FileOffset("X1GL_J.BIN",   0x5250),
                        new FileOffset("X1FUTT_J.BIN", 0x42b8),
                        new FileOffset("X1FNKA6.BIN",  0x682c),
                        new FileOffset("X1FNKA01.BIN", 0x7150),
                        new FileOffset("X1FNK6_J.BIN", 0x42d4),
                        new FileOffset("X1FNK1_J.BIN", 0x4214),
                        new FileOffset("X1DST_J.BIN",  0x5094),
                        new FileOffset("X1DLMA_J.BIN", 0x4f28),
                        new FileOffset("X1DLMA2J.BIN", 0x4228),
                        new FileOffset("X1BEERJI.BIN", 0x4524),
                        new FileOffset("X1ASPI_H.BIN", 0x4918),
                        new FileOffset("X022.BIN",     0x629c),
                    }
                )},

                { "CBF35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b1, 0x0008, 0x0d6e, 0x0023, 0x03b1, 0x002b, 0x0d6e, 0x0046, 0x0414, 0x004f, 0x0ecd },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x46c0),
                        new FileOffset("X1OHRA_J.BIN", 0x45f0),
                        new FileOffset("X1MAYA_J.BIN", 0x4608),
                        new FileOffset("X1LEMO3J.BIN", 0x4b64),
                        new FileOffset("X1LEMO2J.BIN", 0x48d4),
                        new FileOffset("X1LEMO1J.BIN", 0x5288),
                        new FileOffset("X1LECHA.BIN",  0x6d98),
                        new FileOffset("X1HOW_J.BIN",  0x4c54),
                        new FileOffset("X1GL_J.BIN",   0x526c),
                        new FileOffset("X1FUTT_J.BIN", 0x42d4),
                        new FileOffset("X1FNKA6.BIN",  0x6848),
                        new FileOffset("X1FNKA01.BIN", 0x716c),
                        new FileOffset("X1FNK6_J.BIN", 0x42f0),
                        new FileOffset("X1FNK1_J.BIN", 0x4230),
                        new FileOffset("X1DST_J.BIN",  0x50b0),
                        new FileOffset("X1DLMA_J.BIN", 0x4f44),
                        new FileOffset("X1DLMA2J.BIN", 0x4244),
                        new FileOffset("X1BEERJI.BIN", 0x4540),
                        new FileOffset("X1ASPI_H.BIN", 0x4934),
                        new FileOffset("X022.BIN",     0x62b8),
                    }
                )},

                { "CBF36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b0, 0x0008, 0x0cb4, 0x0022, 0x03b0, 0x002a, 0x0cb4, 0x0044, 0x03e4, 0x004c, 0x0e2a },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x46dc),
                        new FileOffset("X1OHRA_J.BIN", 0x460c),
                        new FileOffset("X1MAYA_J.BIN", 0x4624),
                        new FileOffset("X1LEMO3J.BIN", 0x4b80),
                        new FileOffset("X1LEMO2J.BIN", 0x48f0),
                        new FileOffset("X1LEMO1J.BIN", 0x52a4),
                        new FileOffset("X1LECHA.BIN",  0x6db4),
                        new FileOffset("X1KANSHO.BIN", 0x3dbc),
                        new FileOffset("X1HOW_J.BIN",  0x4c70),
                        new FileOffset("X1GL_J.BIN",   0x5288),
                        new FileOffset("X1FUTT_J.BIN", 0x42f0),
                        new FileOffset("X1FNKA6.BIN",  0x6864),
                        new FileOffset("X1FNKA01.BIN", 0x7188),
                        new FileOffset("X1FNK6_J.BIN", 0x430c),
                        new FileOffset("X1FNK1_J.BIN", 0x424c),
                        new FileOffset("X1DST_J.BIN",  0x50cc),
                        new FileOffset("X1DLMA_J.BIN", 0x4f60),
                        new FileOffset("X1DLMA2J.BIN", 0x4260),
                        new FileOffset("X1BEERJI.BIN", 0x455c),
                        new FileOffset("X1ASPI_H.BIN", 0x4950),
                        new FileOffset("X022.BIN",     0x62d4),
                    }
                )},

                { "CBF37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0353, 0x0007, 0x1321, 0x002e, 0x0353, 0x0035, 0x1321, 0x005c, 0x035f, 0x0063, 0x12c8 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x46f8),
                        new FileOffset("X1OHRA_J.BIN", 0x4628),
                        new FileOffset("X1MAYA_J.BIN", 0x4640),
                        new FileOffset("X1LEMO3J.BIN", 0x4b9c),
                        new FileOffset("X1LEMO2J.BIN", 0x490c),
                        new FileOffset("X1LEMO1J.BIN", 0x52c0),
                        new FileOffset("X1LECHA.BIN",  0x6dd0),
                        new FileOffset("X1HOW_J.BIN",  0x4c8c),
                        new FileOffset("X1GL_J.BIN",   0x52a4),
                        new FileOffset("X1FUTT_J.BIN", 0x430c),
                        new FileOffset("X1FNKA6.BIN",  0x6880),
                        new FileOffset("X1FNKA01.BIN", 0x71a4),
                        new FileOffset("X1FNK6_J.BIN", 0x4328),
                        new FileOffset("X1FNK1_J.BIN", 0x4268),
                        new FileOffset("X1DST_J.BIN",  0x50e8),
                        new FileOffset("X1DLMA_J.BIN", 0x4f7c),
                        new FileOffset("X1DLMA2J.BIN", 0x427c),
                        new FileOffset("X1BEERJI.BIN", 0x4578),
                        new FileOffset("X1ASPI_H.BIN", 0x496c),
                        new FileOffset("X022.BIN",     0x62f0),
                    }
                )},

                { "CBF38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0b93, 0x0020, 0x03bf, 0x0028, 0x0b22, 0x003f, 0x04b8, 0x0049, 0x0dff },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4714),
                        new FileOffset("X1OHRA_J.BIN", 0x4644),
                        new FileOffset("X1MAYA_J.BIN", 0x465c),
                        new FileOffset("X1LEMO3J.BIN", 0x4bb8),
                        new FileOffset("X1LEMO2J.BIN", 0x4928),
                        new FileOffset("X1LEMO1J.BIN", 0x52dc),
                        new FileOffset("X1LECHA.BIN",  0x6dec),
                        new FileOffset("X1HOW_J.BIN",  0x4ca8),
                        new FileOffset("X1GL_J.BIN",   0x52c0),
                        new FileOffset("X1FUTT_J.BIN", 0x4328),
                        new FileOffset("X1FNKA6.BIN",  0x689c),
                        new FileOffset("X1FNKA01.BIN", 0x71c0),
                        new FileOffset("X1FNK6_J.BIN", 0x4344),
                        new FileOffset("X1FNK1_J.BIN", 0x4284),
                        new FileOffset("X1DST_J.BIN",  0x5104),
                        new FileOffset("X1DLMA_J.BIN", 0x4f98),
                        new FileOffset("X1DLMA2J.BIN", 0x4298),
                        new FileOffset("X1BEERJI.BIN", 0x4594),
                        new FileOffset("X1ASPI_H.BIN", 0x4988),
                        new FileOffset("X022.BIN",     0x630c),
                    }
                )},

                { "CBF39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03da, 0x0008, 0x0e19, 0x0025, 0x03da, 0x002d, 0x0e19, 0x004a, 0x03f4, 0x0052, 0x0e76 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4730),
                        new FileOffset("X1OHRA_J.BIN", 0x4660),
                        new FileOffset("X1MAYA_J.BIN", 0x4678),
                        new FileOffset("X1LEMO3J.BIN", 0x4bd4),
                        new FileOffset("X1LEMO2J.BIN", 0x4944),
                        new FileOffset("X1LEMO1J.BIN", 0x52f8),
                        new FileOffset("X1LECHA.BIN",  0x6e08),
                        new FileOffset("X1HOW_J.BIN",  0x4cc4),
                        new FileOffset("X1GL_J.BIN",   0x52dc),
                        new FileOffset("X1FUTT_J.BIN", 0x4344),
                        new FileOffset("X1FNKA6.BIN",  0x68b8),
                        new FileOffset("X1FNKA01.BIN", 0x71dc),
                        new FileOffset("X1FNK6_J.BIN", 0x4360),
                        new FileOffset("X1FNK1_J.BIN", 0x42a0),
                        new FileOffset("X1DST_J.BIN",  0x5120),
                        new FileOffset("X1DLMA_J.BIN", 0x4fb4),
                        new FileOffset("X1DLMA2J.BIN", 0x42b4),
                        new FileOffset("X1BEERJI.BIN", 0x45b0),
                        new FileOffset("X1ASPI_H.BIN", 0x49a4),
                        new FileOffset("X022.BIN",     0x6328),
                    }
                )},

                { "CBF40.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d4, 0x0008, 0x0e06, 0x0025, 0x03d4, 0x002d, 0x0e06, 0x004a, 0x03cf, 0x0052, 0x0df9 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x474c),
                        new FileOffset("X1OHRA_J.BIN", 0x467c),
                        new FileOffset("X1MAYA_J.BIN", 0x4694),
                        new FileOffset("X1LEMO3J.BIN", 0x4bf0),
                        new FileOffset("X1LEMO2J.BIN", 0x4960),
                        new FileOffset("X1LEMO1J.BIN", 0x5314),
                        new FileOffset("X1LECHA.BIN",  0x6e24),
                        new FileOffset("X1HOW_J.BIN",  0x4ce0),
                        new FileOffset("X1GL_J.BIN",   0x52f8),
                        new FileOffset("X1FUTT_J.BIN", 0x4360),
                        new FileOffset("X1FNKA6.BIN",  0x68d4),
                        new FileOffset("X1FNKA01.BIN", 0x71f8),
                        new FileOffset("X1FNK6_J.BIN", 0x437c),
                        new FileOffset("X1FNK1_J.BIN", 0x42bc),
                        new FileOffset("X1DST_J.BIN",  0x513c),
                        new FileOffset("X1DLMA_J.BIN", 0x4fd0),
                        new FileOffset("X1DLMA2J.BIN", 0x42d0),
                        new FileOffset("X1BEERJI.BIN", 0x45cc),
                        new FileOffset("X1ASPI_H.BIN", 0x49c0),
                        new FileOffset("X022.BIN",     0x6344),
                    }
                )},

                { "CBF41.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0381, 0x0008, 0x0d13, 0x0023, 0x0381, 0x002b, 0x0d13, 0x0046, 0x0381, 0x004e, 0x0d13 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4768),
                        new FileOffset("X1OHRA_J.BIN", 0x4698),
                        new FileOffset("X1MAYA_J.BIN", 0x46b0),
                        new FileOffset("X1LEMO3J.BIN", 0x4c0c),
                        new FileOffset("X1LEMO2J.BIN", 0x497c),
                        new FileOffset("X1LEMO1J.BIN", 0x5330),
                        new FileOffset("X1LECHA.BIN",  0x6e40),
                        new FileOffset("X1HOW_J.BIN",  0x4cfc),
                        new FileOffset("X1GL_J.BIN",   0x5314),
                        new FileOffset("X1FUTT_J.BIN", 0x437c),
                        new FileOffset("X1FNKA6.BIN",  0x68f0),
                        new FileOffset("X1FNKA01.BIN", 0x7214),
                        new FileOffset("X1FNK6_J.BIN", 0x4398),
                        new FileOffset("X1FNK1_J.BIN", 0x42d8),
                        new FileOffset("X1DST_J.BIN",  0x5158),
                        new FileOffset("X1DLMA_J.BIN", 0x4fec),
                        new FileOffset("X1DLMA2J.BIN", 0x42ec),
                        new FileOffset("X1BEERJI.BIN", 0x45e8),
                        new FileOffset("X1ASPI_H.BIN", 0x49dc),
                        new FileOffset("X022.BIN",     0x6360),
                    }
                )},

                { "CBF42.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x0e55, 0x0026, 0x0403, 0x002f, 0x0e55, 0x004c, 0x03df, 0x0054, 0x0d58 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4784),
                        new FileOffset("X1OHRA_J.BIN", 0x46b4),
                        new FileOffset("X1MAYA_J.BIN", 0x46cc),
                        new FileOffset("X1LEMO3J.BIN", 0x4c28),
                        new FileOffset("X1LEMO2J.BIN", 0x4998),
                        new FileOffset("X1LEMO1J.BIN", 0x534c),
                        new FileOffset("X1LECHA.BIN",  0x6e5c),
                        new FileOffset("X1HOW_J.BIN",  0x4d18),
                        new FileOffset("X1GL_J.BIN",   0x5330),
                        new FileOffset("X1FUTT_J.BIN", 0x4398),
                        new FileOffset("X1FNKA6.BIN",  0x690c),
                        new FileOffset("X1FNKA01.BIN", 0x7230),
                        new FileOffset("X1FNK6_J.BIN", 0x43b4),
                        new FileOffset("X1FNK1_J.BIN", 0x42f4),
                        new FileOffset("X1DST_J.BIN",  0x5174),
                        new FileOffset("X1DLMA_J.BIN", 0x5008),
                        new FileOffset("X1DLMA2J.BIN", 0x4308),
                        new FileOffset("X1BEERJI.BIN", 0x4604),
                        new FileOffset("X1ASPI_H.BIN", 0x49f8),
                        new FileOffset("X022.BIN",     0x637c),
                    }
                )},

                { "CBF43.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0439, 0x0009, 0x1112, 0x002c, 0x0439, 0x0035, 0x1112, 0x0058, 0x042a, 0x0061, 0x10d3 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x47a0),
                        new FileOffset("X1OHRA_J.BIN", 0x46d0),
                        new FileOffset("X1MAYA_J.BIN", 0x46e8),
                        new FileOffset("X1LEMO3J.BIN", 0x4c44),
                        new FileOffset("X1LEMO2J.BIN", 0x49b4),
                        new FileOffset("X1LEMO1J.BIN", 0x5368),
                        new FileOffset("X1LECHA.BIN",  0x6e78),
                        new FileOffset("X1HOW_J.BIN",  0x4d34),
                        new FileOffset("X1GL_J.BIN",   0x534c),
                        new FileOffset("X1FUTT_J.BIN", 0x43b4),
                        new FileOffset("X1FNKA6.BIN",  0x6928),
                        new FileOffset("X1FNKA01.BIN", 0x724c),
                        new FileOffset("X1FNK6_J.BIN", 0x43d0),
                        new FileOffset("X1FNK1_J.BIN", 0x4310),
                        new FileOffset("X1DST_J.BIN",  0x5190),
                        new FileOffset("X1DLMA_J.BIN", 0x5024),
                        new FileOffset("X1DLMA2J.BIN", 0x4324),
                        new FileOffset("X1BEERJI.BIN", 0x4620),
                        new FileOffset("X1ASPI_H.BIN", 0x4a14),
                        new FileOffset("X022.BIN",     0x6398),
                    }
                )},

                { "CBF44.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e2, 0x0008, 0x0f74, 0x0027, 0x03e2, 0x002f, 0x0f74, 0x004e, 0x03eb, 0x0056, 0x0f88 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x47bc),
                        new FileOffset("X1OHRA_J.BIN", 0x46ec),
                        new FileOffset("X1MAYA_J.BIN", 0x4704),
                        new FileOffset("X1LEMO3J.BIN", 0x4c60),
                        new FileOffset("X1LEMO2J.BIN", 0x49d0),
                        new FileOffset("X1LEMO1J.BIN", 0x5384),
                        new FileOffset("X1LECHA.BIN",  0x6e94),
                        new FileOffset("X1HOW_J.BIN",  0x4d50),
                        new FileOffset("X1GL_J.BIN",   0x5368),
                        new FileOffset("X1FUTT_J.BIN", 0x43d0),
                        new FileOffset("X1FNKA6.BIN",  0x6944),
                        new FileOffset("X1FNKA01.BIN", 0x7268),
                        new FileOffset("X1FNK6_J.BIN", 0x43ec),
                        new FileOffset("X1FNK1_J.BIN", 0x432c),
                        new FileOffset("X1DST_J.BIN",  0x51ac),
                        new FileOffset("X1DLMA_J.BIN", 0x5040),
                        new FileOffset("X1DLMA2J.BIN", 0x4340),
                        new FileOffset("X1BEERJI.BIN", 0x463c),
                        new FileOffset("X1ASPI_H.BIN", 0x4a30),
                        new FileOffset("X022.BIN",     0x63b4),
                    }
                )},

                { "CBF45.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0448, 0x0009, 0x0fa1, 0x0029, 0x0448, 0x0032, 0x0fa1, 0x0052, 0x0450, 0x005b, 0x0fd3 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x47d8),
                        new FileOffset("X1OHRA_J.BIN", 0x4708),
                        new FileOffset("X1MAYA_J.BIN", 0x4720),
                        new FileOffset("X1LEMO3J.BIN", 0x4c7c),
                        new FileOffset("X1LEMO2J.BIN", 0x49ec),
                        new FileOffset("X1LEMO1J.BIN", 0x53a0),
                        new FileOffset("X1LECHA.BIN",  0x6eb0),
                        new FileOffset("X1KANSHO.BIN", 0x3eb8),
                        new FileOffset("X1HOW_J.BIN",  0x4d6c),
                        new FileOffset("X1GL_J.BIN",   0x5384),
                        new FileOffset("X1FUTT_J.BIN", 0x43ec),
                        new FileOffset("X1FNKA6.BIN",  0x6960),
                        new FileOffset("X1FNKA01.BIN", 0x7284),
                        new FileOffset("X1FNK6_J.BIN", 0x4408),
                        new FileOffset("X1FNK1_J.BIN", 0x4348),
                        new FileOffset("X1DST_J.BIN",  0x51c8),
                        new FileOffset("X1DLMA_J.BIN", 0x505c),
                        new FileOffset("X1DLMA2J.BIN", 0x435c),
                        new FileOffset("X1BEERJI.BIN", 0x4658),
                        new FileOffset("X1ASPI_H.BIN", 0x4a4c),
                        new FileOffset("X022.BIN",     0x63d0),
                    }
                )},

                { "CBF46.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c9, 0x0008, 0x0dc2, 0x0024, 0x03c9, 0x002c, 0x0dc2, 0x0048, 0x0384, 0x0050, 0x0cc4 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x47f4),
                        new FileOffset("X1OHRA_J.BIN", 0x4724),
                        new FileOffset("X1MAYA_J.BIN", 0x473c),
                        new FileOffset("X1LEMO3J.BIN", 0x4c98),
                        new FileOffset("X1LEMO2J.BIN", 0x4a08),
                        new FileOffset("X1LEMO1J.BIN", 0x53bc),
                        new FileOffset("X1LECHA.BIN",  0x6ecc),
                        new FileOffset("X1HOW_J.BIN",  0x4d88),
                        new FileOffset("X1GL_J.BIN",   0x53a0),
                        new FileOffset("X1FUTT_J.BIN", 0x4408),
                        new FileOffset("X1FNKA6.BIN",  0x697c),
                        new FileOffset("X1FNKA01.BIN", 0x72a0),
                        new FileOffset("X1FNK6_J.BIN", 0x4424),
                        new FileOffset("X1FNK1_J.BIN", 0x4364),
                        new FileOffset("X1DST_J.BIN",  0x51e4),
                        new FileOffset("X1DLMA_J.BIN", 0x5078),
                        new FileOffset("X1DLMA2J.BIN", 0x4378),
                        new FileOffset("X1BEERJI.BIN", 0x4674),
                        new FileOffset("X1ASPI_H.BIN", 0x4a68),
                        new FileOffset("X022.BIN",     0x63ec),
                    }
                )},

                { "CBF47.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fb, 0x0008, 0x0bcf, 0x0020, 0x03fb, 0x0028, 0x0bcf, 0x0040, 0x0453, 0x0049, 0x0cd3 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4810),
                        new FileOffset("X1OHRA_J.BIN", 0x4740),
                        new FileOffset("X1MAYA_J.BIN", 0x4758),
                        new FileOffset("X1LEMO3J.BIN", 0x4cb4),
                        new FileOffset("X1LEMO2J.BIN", 0x4a24),
                        new FileOffset("X1LEMO1J.BIN", 0x53d8),
                        new FileOffset("X1LECHA.BIN",  0x6ee8),
                        new FileOffset("X1HOW_J.BIN",  0x4da4),
                        new FileOffset("X1GL_J.BIN",   0x53bc),
                        new FileOffset("X1FUTT_J.BIN", 0x4424),
                        new FileOffset("X1FNKA6.BIN",  0x6998),
                        new FileOffset("X1FNKA01.BIN", 0x72bc),
                        new FileOffset("X1FNK6_J.BIN", 0x4440),
                        new FileOffset("X1FNK1_J.BIN", 0x4380),
                        new FileOffset("X1DST_J.BIN",  0x5200),
                        new FileOffset("X1DLMA_J.BIN", 0x5094),
                        new FileOffset("X1DLMA2J.BIN", 0x4394),
                        new FileOffset("X1BEERJI.BIN", 0x4690),
                        new FileOffset("X1ASPI_H.BIN", 0x4a84),
                        new FileOffset("X022.BIN",     0x6408),
                    }
                )},

                { "CBF48.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f6, 0x0008, 0x1697, 0x0036, 0x03f6, 0x003e, 0x1697, 0x006c, 0x0406, 0x0075, 0x0edf },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x482c),
                        new FileOffset("X1OHRA_J.BIN", 0x475c),
                        new FileOffset("X1MAYA_J.BIN", 0x4774),
                        new FileOffset("X1LEMO3J.BIN", 0x4cd0),
                        new FileOffset("X1LEMO2J.BIN", 0x4a40),
                        new FileOffset("X1LEMO1J.BIN", 0x53f4),
                        new FileOffset("X1LECHA.BIN",  0x6f04),
                        new FileOffset("X1HOW_J.BIN",  0x4dc0),
                        new FileOffset("X1GL_J.BIN",   0x53d8),
                        new FileOffset("X1FUTT_J.BIN", 0x4440),
                        new FileOffset("X1FNKA6.BIN",  0x69b4),
                        new FileOffset("X1FNKA01.BIN", 0x72d8),
                        new FileOffset("X1FNK6_J.BIN", 0x445c),
                        new FileOffset("X1FNK1_J.BIN", 0x439c),
                        new FileOffset("X1DST_J.BIN",  0x521c),
                        new FileOffset("X1DLMA_J.BIN", 0x50b0),
                        new FileOffset("X1DLMA2J.BIN", 0x43b0),
                        new FileOffset("X1BEERJI.BIN", 0x46ac),
                        new FileOffset("X1ASPI_H.BIN", 0x4aa0),
                        new FileOffset("X022.BIN",     0x6424),
                    }
                )},

                { "CBF49.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048a, 0x000a, 0x0d78, 0x0025, 0x048a, 0x002f, 0x0d78, 0x004a, 0x0483, 0x0054, 0x0d69 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4848),
                        new FileOffset("X1OHRA_J.BIN", 0x4778),
                        new FileOffset("X1MAYA_J.BIN", 0x4790),
                        new FileOffset("X1LEMO3J.BIN", 0x4cec),
                        new FileOffset("X1LEMO2J.BIN", 0x4a5c),
                        new FileOffset("X1LEMO1J.BIN", 0x5410),
                        new FileOffset("X1LECHA.BIN",  0x6f20),
                        new FileOffset("X1HOW_J.BIN",  0x4ddc),
                        new FileOffset("X1GL_J.BIN",   0x53f4),
                        new FileOffset("X1FUTT_J.BIN", 0x445c),
                        new FileOffset("X1FNKA6.BIN",  0x69d0),
                        new FileOffset("X1FNKA01.BIN", 0x72f4),
                        new FileOffset("X1FNK6_J.BIN", 0x4478),
                        new FileOffset("X1FNK1_J.BIN", 0x43b8),
                        new FileOffset("X1DST_J.BIN",  0x5238),
                        new FileOffset("X1DLMA_J.BIN", 0x50cc),
                        new FileOffset("X1DLMA2J.BIN", 0x43cc),
                        new FileOffset("X1BEERJI.BIN", 0x46c8),
                        new FileOffset("X1ASPI_H.BIN", 0x4abc),
                        new FileOffset("X022.BIN",     0x6440),
                    }
                )},

                { "CBF50.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03cd, 0x0008, 0x0e94, 0x0026, 0x03cd, 0x002e, 0x0e94, 0x004c, 0x03b3, 0x0054, 0x0e37 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4864),
                        new FileOffset("X1OHRA_J.BIN", 0x4794),
                        new FileOffset("X1MAYA_J.BIN", 0x47ac),
                        new FileOffset("X1LEMO3J.BIN", 0x4d08),
                        new FileOffset("X1LEMO2J.BIN", 0x4a78),
                        new FileOffset("X1LEMO1J.BIN", 0x542c),
                        new FileOffset("X1LECHA.BIN",  0x6f3c),
                        new FileOffset("X1HOW_J.BIN",  0x4df8),
                        new FileOffset("X1GL_J.BIN",   0x5410),
                        new FileOffset("X1FUTT_J.BIN", 0x4478),
                        new FileOffset("X1FNKA6.BIN",  0x69ec),
                        new FileOffset("X1FNKA01.BIN", 0x7310),
                        new FileOffset("X1FNK6_J.BIN", 0x4494),
                        new FileOffset("X1FNK1_J.BIN", 0x43d4),
                        new FileOffset("X1DST_J.BIN",  0x5254),
                        new FileOffset("X1DLMA_J.BIN", 0x50e8),
                        new FileOffset("X1DLMA2J.BIN", 0x43e8),
                        new FileOffset("X1BEERJI.BIN", 0x46e4),
                        new FileOffset("X1ASPI_H.BIN", 0x4ad8),
                        new FileOffset("X022.BIN",     0x645c),
                    }
                )},

                { "CBF51.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0395, 0x0008, 0x0e4d, 0x0025, 0x0395, 0x002d, 0x0e4d, 0x004a, 0x03c2, 0x0052, 0x0f0e },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4880),
                        new FileOffset("X1OHRA_J.BIN", 0x47b0),
                        new FileOffset("X1MAYA_J.BIN", 0x47c8),
                        new FileOffset("X1LEMO3J.BIN", 0x4d24),
                        new FileOffset("X1LEMO2J.BIN", 0x4a94),
                        new FileOffset("X1LEMO1J.BIN", 0x5448),
                        new FileOffset("X1LECHA.BIN",  0x6f58),
                        new FileOffset("X1HOW_J.BIN",  0x4e14),
                        new FileOffset("X1GL_J.BIN",   0x542c),
                        new FileOffset("X1FUTT_J.BIN", 0x4494),
                        new FileOffset("X1FNKA6.BIN",  0x6a08),
                        new FileOffset("X1FNKA01.BIN", 0x732c),
                        new FileOffset("X1FNK6_J.BIN", 0x44b0),
                        new FileOffset("X1FNK1_J.BIN", 0x43f0),
                        new FileOffset("X1DST_J.BIN",  0x5270),
                        new FileOffset("X1DLMA_J.BIN", 0x5104),
                        new FileOffset("X1DLMA2J.BIN", 0x4404),
                        new FileOffset("X1BEERJI.BIN", 0x4700),
                        new FileOffset("X1ASPI_H.BIN", 0x4af4),
                        new FileOffset("X022.BIN",     0x6478),
                    }
                )},

                { "CBF52.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x1057, 0x002a, 0x0420, 0x0033, 0x1057, 0x0054, 0x0403, 0x005d, 0x1057 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x489c),
                        new FileOffset("X1OHRA_J.BIN", 0x47cc),
                        new FileOffset("X1MAYA_J.BIN", 0x47e4),
                        new FileOffset("X1LEMO3J.BIN", 0x4d40),
                        new FileOffset("X1LEMO2J.BIN", 0x4ab0),
                        new FileOffset("X1LEMO1J.BIN", 0x5464),
                        new FileOffset("X1LECHA.BIN",  0x6f74),
                        new FileOffset("X1HOW_J.BIN",  0x4e30),
                        new FileOffset("X1GL_J.BIN",   0x5448),
                        new FileOffset("X1FUTT_J.BIN", 0x44b0),
                        new FileOffset("X1FNKA6.BIN",  0x6a24),
                        new FileOffset("X1FNKA01.BIN", 0x7348),
                        new FileOffset("X1FNK6_J.BIN", 0x44cc),
                        new FileOffset("X1FNK1_J.BIN", 0x440c),
                        new FileOffset("X1DST_J.BIN",  0x528c),
                        new FileOffset("X1DLMA_J.BIN", 0x5120),
                        new FileOffset("X1DLMA2J.BIN", 0x4420),
                        new FileOffset("X1BEERJI.BIN", 0x471c),
                        new FileOffset("X1ASPI_H.BIN", 0x4b10),
                        new FileOffset("X022.BIN",     0x6494),
                    }
                )},

                { "CBF53.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0454, 0x0009, 0x0e70, 0x0026, 0x0454, 0x002f, 0x0e70, 0x004c, 0x0417, 0x0055, 0x0db6 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x48b8),
                        new FileOffset("X1OHRA_J.BIN", 0x47e8),
                        new FileOffset("X1MAYA_J.BIN", 0x4800),
                        new FileOffset("X1LEMO3J.BIN", 0x4d5c),
                        new FileOffset("X1LEMO2J.BIN", 0x4acc),
                        new FileOffset("X1LEMO1J.BIN", 0x5480),
                        new FileOffset("X1LECHA.BIN",  0x6f90),
                        new FileOffset("X1HOW_J.BIN",  0x4e4c),
                        new FileOffset("X1GL_J.BIN",   0x5464),
                        new FileOffset("X1FUTT_J.BIN", 0x44cc),
                        new FileOffset("X1FNKA6.BIN",  0x6a40),
                        new FileOffset("X1FNKA01.BIN", 0x7364),
                        new FileOffset("X1FNK6_J.BIN", 0x44e8),
                        new FileOffset("X1FNK1_J.BIN", 0x4428),
                        new FileOffset("X1DST_J.BIN",  0x52a8),
                        new FileOffset("X1DLMA_J.BIN", 0x513c),
                        new FileOffset("X1DLMA2J.BIN", 0x443c),
                        new FileOffset("X1BEERJI.BIN", 0x4738),
                        new FileOffset("X1ASPI_H.BIN", 0x4b2c),
                        new FileOffset("X022.BIN",     0x64b0),
                    }
                )},

                { "CBF54.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f7, 0x0008, 0x0e75, 0x0025, 0x03f7, 0x002d, 0x0e75, 0x004a, 0x03da, 0x0052, 0x0e0d },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x48d4),
                        new FileOffset("X1OHRA_J.BIN", 0x4804),
                        new FileOffset("X1MAYA_J.BIN", 0x481c),
                        new FileOffset("X1LEMO3J.BIN", 0x4d78),
                        new FileOffset("X1LEMO2J.BIN", 0x4ae8),
                        new FileOffset("X1LEMO1J.BIN", 0x549c),
                        new FileOffset("X1LECHA.BIN",  0x6fac),
                        new FileOffset("X1HOW_J.BIN",  0x4e68),
                        new FileOffset("X1GL_J.BIN",   0x5480),
                        new FileOffset("X1FUTT_J.BIN", 0x44e8),
                        new FileOffset("X1FNKA6.BIN",  0x6a5c),
                        new FileOffset("X1FNKA01.BIN", 0x7380),
                        new FileOffset("X1FNK6_J.BIN", 0x4504),
                        new FileOffset("X1FNK1_J.BIN", 0x4444),
                        new FileOffset("X1DST_J.BIN",  0x52c4),
                        new FileOffset("X1DLMA_J.BIN", 0x5158),
                        new FileOffset("X1DLMA2J.BIN", 0x4458),
                        new FileOffset("X1BEERJI.BIN", 0x4754),
                        new FileOffset("X1ASPI_H.BIN", 0x4b48),
                        new FileOffset("X022.BIN",     0x64cc),
                    }
                )},

                { "CBF55.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x06f4, 0x000e, 0x1b13, 0x0045, 0x06f4, 0x0053, 0x1b13, 0x008a, 0x06e6, 0x0098, 0x1ada },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x48f0),
                        new FileOffset("X1OHRA_J.BIN", 0x4820),
                        new FileOffset("X1MAYA_J.BIN", 0x4838),
                        new FileOffset("X1LEMO3J.BIN", 0x4d94),
                        new FileOffset("X1LEMO2J.BIN", 0x4b04),
                        new FileOffset("X1LEMO1J.BIN", 0x54b8),
                        new FileOffset("X1LECHA.BIN",  0x6fc8),
                        new FileOffset("X1HOW_J.BIN",  0x4e84),
                        new FileOffset("X1GL_J.BIN",   0x549c),
                        new FileOffset("X1FUTT_J.BIN", 0x4504),
                        new FileOffset("X1FNKA6.BIN",  0x6a78),
                        new FileOffset("X1FNKA01.BIN", 0x739c),
                        new FileOffset("X1FNK6_J.BIN", 0x4520),
                        new FileOffset("X1FNK1_J.BIN", 0x4460),
                        new FileOffset("X1DST_J.BIN",  0x52e0),
                        new FileOffset("X1DLMA_J.BIN", 0x5174),
                        new FileOffset("X1DLMA2J.BIN", 0x4474),
                        new FileOffset("X1BEERJI.BIN", 0x4770),
                        new FileOffset("X1ASPI_H.BIN", 0x4b64),
                        new FileOffset("X022.BIN",     0x64e8),
                    }
                )},

                { "CBF56.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04be, 0x000a, 0x0d5f, 0x0025, 0x04be, 0x002f, 0x0d5f, 0x004a, 0x04c3, 0x0054, 0x0d81 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x490c),
                        new FileOffset("X1OHRA_J.BIN", 0x483c),
                        new FileOffset("X1MAYA_J.BIN", 0x4854),
                        new FileOffset("X1LEMO3J.BIN", 0x4db0),
                        new FileOffset("X1LEMO2J.BIN", 0x4b20),
                        new FileOffset("X1LEMO1J.BIN", 0x54d4),
                        new FileOffset("X1LECHA.BIN",  0x6fe4),
                        new FileOffset("X1HOW_J.BIN",  0x4ea0),
                        new FileOffset("X1GL_J.BIN",   0x54b8),
                        new FileOffset("X1FUTT_J.BIN", 0x4520),
                        new FileOffset("X1FNKA6.BIN",  0x6a94),
                        new FileOffset("X1FNKA01.BIN", 0x73b8),
                        new FileOffset("X1FNK6_J.BIN", 0x453c),
                        new FileOffset("X1FNK1_J.BIN", 0x447c),
                        new FileOffset("X1DST_J.BIN",  0x52fc),
                        new FileOffset("X1DLMA_J.BIN", 0x5190),
                        new FileOffset("X1DLMA2J.BIN", 0x4490),
                        new FileOffset("X1BEERJI.BIN", 0x478c),
                        new FileOffset("X1ASPI_H.BIN", 0x4b80),
                        new FileOffset("X022.BIN",     0x6504),
                    }
                )},

                { "CBF57.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0361, 0x0007, 0x0c5b, 0x0020, 0x0361, 0x0027, 0x0c5b, 0x0040, 0x02e9, 0x0046, 0x0a98 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4928),
                        new FileOffset("X1OHRA_J.BIN", 0x4858),
                        new FileOffset("X1MAYA_J.BIN", 0x4870),
                        new FileOffset("X1LEMO3J.BIN", 0x4dcc),
                        new FileOffset("X1LEMO2J.BIN", 0x4b3c),
                        new FileOffset("X1LEMO1J.BIN", 0x54f0),
                        new FileOffset("X1LECHA.BIN",  0x7000),
                        new FileOffset("X1HOW_J.BIN",  0x4ebc),
                        new FileOffset("X1GL_J.BIN",   0x54d4),
                        new FileOffset("X1FUTT_J.BIN", 0x453c),
                        new FileOffset("X1FNKA6.BIN",  0x6ab0),
                        new FileOffset("X1FNKA01.BIN", 0x73d4),
                        new FileOffset("X1FNK6_J.BIN", 0x4558),
                        new FileOffset("X1FNK1_J.BIN", 0x4498),
                        new FileOffset("X1DST_J.BIN",  0x5318),
                        new FileOffset("X1DLMA_J.BIN", 0x51ac),
                        new FileOffset("X1DLMA2J.BIN", 0x44ac),
                        new FileOffset("X1BEERJI.BIN", 0x47a8),
                        new FileOffset("X1ASPI_H.BIN", 0x4b9c),
                        new FileOffset("X022.BIN",     0x6520),
                    }
                )},

                { "CBF58.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x05f9, 0x000c, 0x0cf4, 0x0026, 0x05f9, 0x0032, 0x0cf4, 0x004c, 0x0623, 0x0059, 0x0d6a },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4944),
                        new FileOffset("X1OHRA_J.BIN", 0x4874),
                        new FileOffset("X1MAYA_J.BIN", 0x488c),
                        new FileOffset("X1LEMO3J.BIN", 0x4de8),
                        new FileOffset("X1LEMO2J.BIN", 0x4b58),
                        new FileOffset("X1LEMO1J.BIN", 0x550c),
                        new FileOffset("X1LECHA.BIN",  0x701c),
                        new FileOffset("X1HOW_J.BIN",  0x4ed8),
                        new FileOffset("X1GL_J.BIN",   0x54f0),
                        new FileOffset("X1FUTT_J.BIN", 0x4558),
                        new FileOffset("X1FNKA6.BIN",  0x6acc),
                        new FileOffset("X1FNKA01.BIN", 0x73f0),
                        new FileOffset("X1FNK6_J.BIN", 0x4574),
                        new FileOffset("X1FNK1_J.BIN", 0x44b4),
                        new FileOffset("X1DST_J.BIN",  0x5334),
                        new FileOffset("X1DLMA_J.BIN", 0x51c8),
                        new FileOffset("X1DLMA2J.BIN", 0x44c8),
                        new FileOffset("X1BEERJI.BIN", 0x47c4),
                        new FileOffset("X1ASPI_H.BIN", 0x4bb8),
                        new FileOffset("X022.BIN",     0x653c),
                    }
                )},

                { "CBF59.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ee, 0x0008, 0x0e92, 0x0026, 0x03ee, 0x002e, 0x0e92, 0x004c, 0x03ee, 0x0054, 0x0e92 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4960),
                        new FileOffset("X1OHRA_J.BIN", 0x4890),
                        new FileOffset("X1MAYA_J.BIN", 0x48a8),
                        new FileOffset("X1LEMO3J.BIN", 0x4e04),
                        new FileOffset("X1LEMO2J.BIN", 0x4b74),
                        new FileOffset("X1LEMO1J.BIN", 0x5528),
                        new FileOffset("X1LECHA.BIN",  0x7038),
                        new FileOffset("X1HOW_J.BIN",  0x4ef4),
                        new FileOffset("X1GL_J.BIN",   0x550c),
                        new FileOffset("X1FUTT_J.BIN", 0x4574),
                        new FileOffset("X1FNKA6.BIN",  0x6ae8),
                        new FileOffset("X1FNKA01.BIN", 0x740c),
                        new FileOffset("X1FNK6_J.BIN", 0x4590),
                        new FileOffset("X1FNK1_J.BIN", 0x44d0),
                        new FileOffset("X1DST_J.BIN",  0x5350),
                        new FileOffset("X1DLMA_J.BIN", 0x51e4),
                        new FileOffset("X1DLMA2J.BIN", 0x44e4),
                        new FileOffset("X1BEERJI.BIN", 0x47e0),
                        new FileOffset("X1ASPI_H.BIN", 0x4bd4),
                        new FileOffset("X022.BIN",     0x6558),
                    }
                )},

                { "CBP00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03db, 0x0008, 0x0f4c, 0x0027, 0x0425, 0x0030, 0x10b9, 0x0052, 0x03b6, 0x005a, 0x0f0b },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8154) }
                )},

                { "CBP01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040b, 0x0009, 0x0f7d, 0x0028, 0x047c, 0x0031, 0x1179, 0x0054, 0x0408, 0x005d, 0x0f3d },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8170) }
                )},

                { "CBP02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0404, 0x0009, 0x0f6d, 0x0028, 0x0431, 0x0031, 0x1045, 0x0052, 0x0431, 0x005b, 0x1058 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x818c) }
                )},

                { "CBP03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x109a, 0x002a, 0x03c2, 0x0032, 0x0fc4, 0x0052, 0x03d4, 0x005a, 0x0fd1 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x81a8) }
                )},

                { "CBP04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0386, 0x0008, 0x1586, 0x0034, 0x03d4, 0x003c, 0x0fa4, 0x005c, 0x03f8, 0x0064, 0x102c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x81c4) }
                )},

                { "CBP05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x047b, 0x0009, 0x12ba, 0x002f, 0x0498, 0x0039, 0x1377, 0x0060, 0x0453, 0x0069, 0x1268 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x81e0) }
                )},

                { "CBP06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035d, 0x0007, 0x0d7f, 0x0022, 0x0398, 0x002a, 0x0ea5, 0x0048, 0x033e, 0x004f, 0x0d10 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x81fc) }
                )},

                { "CBP07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x037d, 0x0007, 0x0e5b, 0x0024, 0x03be, 0x002c, 0x0f06, 0x004b, 0x03e3, 0x0053, 0x0f89 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8218) }
                )},

                { "CBP08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04a1, 0x000a, 0x1209, 0x002f, 0x04bc, 0x0039, 0x1263, 0x005e, 0x04b9, 0x0068, 0x1270 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8234) }
                )},

                { "CBP09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0432, 0x0009, 0x0a77, 0x001e, 0x0440, 0x0027, 0x0aad, 0x003d, 0x041d, 0x0046, 0x0a54 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8250) }
                )},

                { "CBP10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x045e, 0x0009, 0x1160, 0x002c, 0x03c2, 0x0034, 0x0f29, 0x0053, 0x037f, 0x005a, 0x0dfd },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x826c) }
                )},

                { "CBP11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x0ebe, 0x0026, 0x03c6, 0x002e, 0x0ebe, 0x004c, 0x03d4, 0x0054, 0x0f02 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8288) }
                )},

                { "CBP12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x042e, 0x0009, 0x1026, 0x002a, 0x042e, 0x0033, 0x1026, 0x0054, 0x0419, 0x005d, 0x101a },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x82a4) }
                )},

                { "CBP13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0453, 0x0009, 0x11f2, 0x002d, 0x0453, 0x0036, 0x11f2, 0x005a, 0x044a, 0x0063, 0x11d5 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x82c0) }
                )},

                { "CBP14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x0b16, 0x001d, 0x02c2, 0x0023, 0x0aa0, 0x0039, 0x03a4, 0x0041, 0x0e0f },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x82dc) }
                )},

                { "CBP15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0525, 0x000b, 0x141d, 0x0034, 0x0525, 0x003f, 0x141d, 0x0068, 0x04f8, 0x0072, 0x138e },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x82f8) }
                )},

                { "CBP16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0455, 0x0009, 0x0aa6, 0x001f, 0x0455, 0x0028, 0x0aa6, 0x003e, 0x0448, 0x0047, 0x10fe },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8314) }
                )},

                { "CBP17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0350, 0x0007, 0x0d79, 0x0022, 0x0350, 0x0029, 0x0d79, 0x0044, 0x0339, 0x004b, 0x0d18 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8330) }
                )},

                { "CBP18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0397, 0x0008, 0x0e92, 0x0026, 0x0397, 0x002e, 0x0e92, 0x004c, 0x03a8, 0x0054, 0x0ec5 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x834c) }
                )},

                { "CBP19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0415, 0x0009, 0x0a21, 0x001e, 0x0415, 0x0027, 0x0a21, 0x003c, 0x040c, 0x0045, 0x0a0f },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8368) }
                )},

                { "CBP20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x1015, 0x0029, 0x0434, 0x0032, 0x1127, 0x0055, 0x0431, 0x005e, 0x1151 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8384) }
                )},

                { "CBP21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0423, 0x0009, 0x0e41, 0x0026, 0x0454, 0x002f, 0x0ef8, 0x004d, 0x0447, 0x0056, 0x0ea3 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x83a0) }
                )},

                { "CBP22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0ef6, 0x0026, 0x03a6, 0x002e, 0x0f5e, 0x004d, 0x03b3, 0x0055, 0x0f8a },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x83bc) }
                )},

                { "CBP23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x044c, 0x0009, 0x11a9, 0x002d, 0x0488, 0x0037, 0x128e, 0x005d, 0x0476, 0x0066, 0x1242 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x83d8) }
                )},

                { "CBP24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x1089, 0x002b, 0x041f, 0x0034, 0x1081, 0x0056, 0x0433, 0x005f, 0x10e9 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x83f4) }
                )},

                { "CBP25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x049d, 0x000a, 0x1372, 0x0031, 0x04ae, 0x003b, 0x1370, 0x0062, 0x04c0, 0x006c, 0x13f5 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8410) }
                )},

                { "CBP26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03bd, 0x0008, 0x0efb, 0x0026, 0x0414, 0x002f, 0x103c, 0x0050, 0x03cd, 0x0058, 0x0f01 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x842c) }
                )},

                { "CBP27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048c, 0x000a, 0x0b54, 0x0021, 0x0497, 0x002b, 0x0b57, 0x0042, 0x07d2, 0x0052, 0x0bb9 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8448) }
                )},

                { "CBP28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0377, 0x0007, 0x0e2f, 0x0024, 0x0381, 0x002c, 0x0e72, 0x0049, 0x037e, 0x0050, 0x0e47 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8464) }
                )},

                { "CBP29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d9, 0x0008, 0x0f94, 0x0028, 0x03d9, 0x0030, 0x0fb7, 0x0050, 0x03be, 0x0058, 0x0f32 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8480) }
                )},

                { "CBP30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x0ffb, 0x0028, 0x039c, 0x0030, 0x0ed3, 0x004e, 0x03a4, 0x0056, 0x0edd },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x849c) }
                )},

                { "CBP31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03db, 0x0008, 0x0fc9, 0x0028, 0x03db, 0x0030, 0x0fc9, 0x0050, 0x0435, 0x0059, 0x1165 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x84b8) }
                )},

                { "CBP32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fa, 0x0008, 0x0f16, 0x0027, 0x03fa, 0x002f, 0x0f16, 0x004e, 0x03ed, 0x0056, 0x0f08 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x84d4) }
                )},

                { "CBP33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0376, 0x0007, 0x0e61, 0x0024, 0x0376, 0x002b, 0x0e61, 0x0048, 0x0326, 0x004f, 0x0cf7 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x84f0) }
                )},

                { "CBP34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0396, 0x0008, 0x0ec8, 0x0026, 0x0396, 0x002e, 0x0ec8, 0x004c, 0x0402, 0x0055, 0x1028 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x850c) }
                )},

                { "CBP35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b6, 0x0008, 0x0e4c, 0x0025, 0x03b6, 0x002d, 0x0e4c, 0x004a, 0x0421, 0x0053, 0x0fe4 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8528) }
                )},

                { "CBP36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0391, 0x0008, 0x0ed0, 0x0026, 0x0391, 0x002e, 0x0ed0, 0x004c, 0x03c6, 0x0054, 0x0f75 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8544) }
                )},

                { "CBP37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0538, 0x000b, 0x1170, 0x002e, 0x0538, 0x0039, 0x1170, 0x005c, 0x0552, 0x0067, 0x11ce },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8560) }
                )},

                { "CBP38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0bae, 0x0020, 0x03bf, 0x0028, 0x0b2b, 0x003f, 0x04b8, 0x0049, 0x0df1 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x857c) }
                )},

                { "CBP39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040f, 0x0009, 0x0fe0, 0x0029, 0x040f, 0x0032, 0x0fe0, 0x0052, 0x0430, 0x005b, 0x1055 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8598) }
                )},

                { "CBP40.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d4, 0x0008, 0x0f0a, 0x0027, 0x03d4, 0x002f, 0x0f0a, 0x004e, 0x03cf, 0x0056, 0x0f0b },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x85b4) }
                )},

                { "CBP41.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0381, 0x0008, 0x090b, 0x001b, 0x0381, 0x0023, 0x090b, 0x0036, 0x0381, 0x003e, 0x090b },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x85d0) }
                )},

                { "CBP42.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x0f51, 0x0028, 0x0403, 0x0031, 0x0f51, 0x0050, 0x03df, 0x0058, 0x0f4c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x85ec) }
                )},

                { "CBP43.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0439, 0x0009, 0x115a, 0x002c, 0x0439, 0x0035, 0x115a, 0x0058, 0x042a, 0x0061, 0x111c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8608) }
                )},

                { "CBP44.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e2, 0x0008, 0x0f48, 0x0027, 0x03e2, 0x002f, 0x0f48, 0x004e, 0x03eb, 0x0056, 0x09b7 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8624) }
                )},

                { "CBP45.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0448, 0x0009, 0x116a, 0x002c, 0x0448, 0x0035, 0x116a, 0x0058, 0x045d, 0x0061, 0x11a9 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8640) }
                )},

                { "CBP46.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c9, 0x0008, 0x0c43, 0x0021, 0x03c9, 0x0029, 0x0c43, 0x0042, 0x0384, 0x004a, 0x0b7c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x865c) }
                )},

                { "CBP47.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fb, 0x0008, 0x09d1, 0x001c, 0x03fb, 0x0024, 0x09d1, 0x0038, 0x0453, 0x0041, 0x0ac2 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8678) }
                )},

                { "CBP48.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f6, 0x0008, 0x1008, 0x0029, 0x03f6, 0x0031, 0x1008, 0x0052, 0x0406, 0x005b, 0x1032 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8694) }
                )},

                { "CBP49.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048a, 0x000a, 0x0afb, 0x0020, 0x048a, 0x002a, 0x0afb, 0x0040, 0x0483, 0x004a, 0x0aea },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x86b0) }
                )},

                { "CBP50.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03cd, 0x0008, 0x0fca, 0x0028, 0x03cd, 0x0030, 0x0fca, 0x0050, 0x03b3, 0x0058, 0x0f5d },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x86cc) }
                )},

                { "CBP51.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0605, 0x000d, 0x1728, 0x003c, 0x0605, 0x0049, 0x1728, 0x0078, 0x05c3, 0x0084, 0x163c },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x86e8) }
                )},

                { "CBP52.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x0c42, 0x0022, 0x0420, 0x002b, 0x0c42, 0x0044, 0x0403, 0x004d, 0x0bef },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8704) }
                )},

                { "CBP53.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0454, 0x0009, 0x1094, 0x002b, 0x0454, 0x0034, 0x1094, 0x0056, 0x0417, 0x005f, 0x0fac },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8720) }
                )},

                { "CBP54.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f0, 0x0008, 0x0f47, 0x0027, 0x03f0, 0x002f, 0x0f47, 0x004e, 0x03dd, 0x0056, 0x0ed4 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x873c) }
                )},

                { "CBP55.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x06f4, 0x000e, 0x10d0, 0x0030, 0x06f4, 0x003e, 0x10d0, 0x0060, 0x06e6, 0x006e, 0x10b2 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8758) }
                )},

                { "CBP56.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04be, 0x000a, 0x0fea, 0x002a, 0x04be, 0x0034, 0x0fea, 0x0054, 0x04c3, 0x005e, 0x1014 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8774) }
                )},

                { "CBP57.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0598, 0x000c, 0x13db, 0x0034, 0x0598, 0x0040, 0x13db, 0x0068, 0x05c3, 0x0074, 0x1443 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8790) }
                )},

                { "CBP58.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x033f, 0x0007, 0x0ded, 0x0023, 0x033f, 0x002a, 0x0ded, 0x0046, 0x035f, 0x004d, 0x0e66 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x87ac) }
                )},

                { "CBP59.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ee, 0x0008, 0x06a8, 0x0016, 0x03ee, 0x001e, 0x06a8, 0x002c, 0x03ee, 0x0034, 0x06a8 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x87c8) }
                )},

                { "CBW00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x1516, 0x0033, 0x0415, 0x003c, 0x170f, 0x006b, 0x039a, 0x0073, 0x1433 },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6bf8) }
                )},

                { "CBW07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038c, 0x0008, 0x13ef, 0x0030, 0x03ab, 0x0038, 0x1483, 0x0062, 0x05d3, 0x006e, 0x153b },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6c30) }
                )},

                { "CBW20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x161d, 0x0036, 0x0403, 0x003f, 0x1795, 0x006f, 0x06a8, 0x007d, 0x15e6 },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6c14) }
                )},

                { "ENEMY.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x0a3a, 0x0016, 0x0b42, 0x002d, 0x0b81, 0x0045, 0x0740, 0x0054, 0x0704, 0x0063, 0x0d03, 0x007e, 0x08ae, 0x0090, 0x066e, 0x009d, 0x0739, 0x00ac, 0x0846, 0x00bd, 0x092b, 0x00d0, 0x095a, 0x00e3, 0x09af, 0x00f7, 0x0993, 0x010b, 0x077e, 0x011a, 0x0ae8, 0x0130, 0x063f, 0x013d, 0x0855, 0x014e, 0x0a9c, 0x0164, 0x0e6c, 0x0181, 0x08f0, 0x0193, 0x087c, 0x01a4, 0x0820, 0x01b5, 0x09af, 0x01c9, 0x0806, 0x01da, 0x06b2, 0x01e8, 0x0ab1, 0x01fe, 0x0c48, 0x0217, 0x0e81, 0x0235, 0x0b66, 0x024c, 0x0c43, 0x0265, 0x0bb3, 0x027d, 0x09bc, 0x0291, 0x08ea, 0x02a3, 0x0765, 0x02b2, 0x06ed, 0x02c0, 0x0266, 0x02c5, 0x0509, 0x02d0, 0x063b, 0x02dd, 0x0706, 0x02ec, 0x06eb, 0x02fa, 0x0a7e, 0x030f, 0x0974, 0x0322, 0x0617, 0x032f, 0x04d3, 0x0339, 0x0985, 0x034d, 0x05c6, 0x0359, 0x0685, 0x0367, 0x0a1f, 0x037c, 0x09b2, 0x0390, 0x0a80, 0x03a5, 0x0a72, 0x03ba, 0x0af9, 0x03d0, 0x09f3, 0x03e4, 0x07ef, 0x03f4, 0x0836, 0x0405, 0x0755, 0x0414, 0x0a8f, 0x042a, 0x09a6, 0x043e, 0x0833, 0x044f, 0x076c, 0x045e, 0x07b2, 0x046e, 0x058f, 0x047a, 0x09e6, 0x048e, 0x0ac2, 0x04a4, 0x09cf, 0x04b8, 0x0abc, 0x04ce, 0x08bc, 0x04e0, 0x08f8, 0x04f2, 0x0936, 0x0505, 0x089d, 0x0517, 0x02db, 0x051d, 0x0887, 0x052f, 0x0736, 0x053e, 0x0866, 0x054f, 0x07ed, 0x055f, 0x0826, 0x0570, 0x07fa, 0x0580, 0x0787, 0x0590, 0x09ff, 0x05a4, 0x09fc, 0x05b8, 0x0aa7, 0x05ce, 0x0a90, 0x05e4, 0x0adc, 0x05fa, 0x05ed, 0x0606, 0x000a, 0x0607, 0x0002, 0x0608, 0x0712, 0x0617, 0x0b77, 0x062e, 0x0a49, 0x0643, 0x0002, 0x0644, 0x0971, 0x0657, 0x0891, 0x0669, 0x0991, 0x067d, 0x0002, 0x067e, 0x0960, 0x0691, 0x0002, 0x0692, 0x0c7c, 0x06ab, 0x0928, 0x06be, 0x0748, 0x06cd, 0x07c6, 0x06dd, 0x0a94, 0x06f3, 0x0724, 0x0702, 0x0640, 0x070f, 0x06d4, 0x071d, 0x0002, 0x071e, 0x075e, 0x072d, 0x093a, 0x0740, 0x114b, 0x0763, 0x0843, 0x0774, 0x0c00, 0x078c, 0x06a0, 0x079a, 0x061c, 0x07a7, 0x08f5, 0x07b9, 0x096a, 0x07cc, 0x0a99, 0x07e2, 0x0a8c, 0x07f8, 0x0a5c, 0x080d, 0x0891, 0x081f, 0x0873, 0x0830, 0x086c, 0x0841, 0x0002, 0x0842, 0x0002, 0x0843, 0x0002, 0x0844, 0x0002, 0x0845, 0x0002, 0x0846, 0x0002, 0x0847, 0x089c, 0x0859, 0x083e, 0x086a, 0x05c4, 0x0876, 0x0abc, 0x088c, 0x080d, 0x089d, 0x0902, 0x08b0, 0x0696, 0x08be, 0x0672, 0x08cb, 0x04f1, 0x08d5, 0x0002, 0x08d6, 0x0725, 0x08e5, 0x0002, 0x08e6, 0x0749, 0x08f5, 0x0827, 0x0906, 0x0866, 0x0917, 0x0808, 0x0928, 0x0878, 0x0939, 0x0002, 0x093a, 0x0002, 0x093b, 0x0a44, 0x0950, 0x09db, 0x0964, 0x0a9f, 0x097a, 0x05b1, 0x0986, 0x0770, 0x0995, 0x09bf, 0x09a9, 0x09a5, 0x09bd, 0x0913, 0x09d0, 0x08da, 0x09e2, 0x0a62, 0x09f7, 0x0521, 0x0a02, 0x0002, 0x0a03, 0x0a32, 0x0a18, 0x093e, 0x0a2b, 0x0a55, 0x0a40, 0x03f8, 0x0a48, 0x09af, 0x0a5c, 0x0002, 0x0a5d, 0x0002, 0x0a5e, 0x08c8, 0x0a70, 0x0a6a, 0x0a85, 0x0796, 0x0a95, 0x0886, 0x0aa7, 0x0b6d, 0x0abe, 0x0b50, 0x0ad5, 0x0993, 0x0ae9, 0x0922, 0x0afc, 0x0927, 0x0b0f, 0x0a1c, 0x0b24, 0x09e0, 0x0b38, 0x0877, 0x0b49, 0x0e27, 0x0b66, 0x08c3, 0x0b78, 0x0994, 0x0b8c, 0x0002, 0x0b8d, 0x1076, 0x0bae, 0x0002, 0x0baf, 0x094a, 0x0bc2, 0x0002, 0x0bc3, 0x0731, 0x0bd2, 0x0072, 0x0bd3, 0x0002, 0x0bd4, 0x07eb, 0x0be4, 0x0002, 0x0be5, 0x0002 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x87e0) }
                )},

                { "ENEMYS.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x03b9, 0x0009, 0x043e, 0x0012, 0x0459, 0x001b, 0x0324, 0x0022, 0x0308, 0x0029, 0x0d03, 0x0044, 0x04fc, 0x004e, 0x066b, 0x005b, 0x02b7, 0x0061, 0x030c, 0x0068, 0x0369, 0x006f, 0x039a, 0x0077, 0x03a0, 0x007f, 0x0393, 0x0087, 0x0342, 0x008e, 0x0425, 0x0097, 0x025d, 0x009c, 0x0330, 0x00a3, 0x0405, 0x00ac, 0x03b5, 0x00b4, 0x034f, 0x00bb, 0x033c, 0x00c2, 0x02f2, 0x00c8, 0x03bb, 0x00d0, 0x035d, 0x00d7, 0x02d9, 0x00dd, 0x0410, 0x00e6, 0x073c, 0x00f5, 0x03a0, 0x00fd, 0x044d, 0x0106, 0x049f, 0x0110, 0x04f8, 0x011a, 0x03be, 0x0122, 0x0360, 0x0129, 0x0347, 0x0130, 0x06eb, 0x013e, 0x0264, 0x0143, 0x0507, 0x014e, 0x0638, 0x015b, 0x0703, 0x016a, 0x0425, 0x0173, 0x03f4, 0x017b, 0x0381, 0x0183, 0x03a2, 0x018b, 0x04d1, 0x0195, 0x035e, 0x019c, 0x060a, 0x01a9, 0x03ec, 0x01b1, 0x03cd, 0x01b9, 0x039d, 0x01c1, 0x03ed, 0x01c9, 0x047e, 0x01d2, 0x0af8, 0x01e8, 0x03d0, 0x01f0, 0x02fd, 0x01f6, 0x0381, 0x01fe, 0x031f, 0x0205, 0x03fa, 0x020d, 0x03d0, 0x0215, 0x04b4, 0x021f, 0x0332, 0x0226, 0x034b, 0x022d, 0x058e, 0x0239, 0x0443, 0x0242, 0x03fa, 0x024a, 0x039f, 0x0252, 0x03fe, 0x025a, 0x03c6, 0x0262, 0x03de, 0x026a, 0x02d1, 0x0270, 0x034f, 0x0277, 0x02d9, 0x027d, 0x03f1, 0x0285, 0x0310, 0x028c, 0x033e, 0x0293, 0x03eb, 0x029b, 0x0314, 0x02a2, 0x0302, 0x02a9, 0x0336, 0x02b0, 0x03d1, 0x02b8, 0x03ce, 0x02c0, 0x0418, 0x02c9, 0x040c, 0x02d2, 0x045b, 0x02db, 0x05ea, 0x02e7, 0x000a, 0x02e8, 0x0002, 0x02e9, 0x0317, 0x02f0, 0x0817, 0x0301, 0x03ea, 0x0309, 0x0002, 0x030a, 0x0971, 0x031d, 0x03b5, 0x0325, 0x0425, 0x032e, 0x0002, 0x032f, 0x0395, 0x0337, 0x0002, 0x0338, 0x04ae, 0x0342, 0x0379, 0x0349, 0x0326, 0x0350, 0x02f4, 0x0356, 0x048e, 0x0360, 0x0304, 0x0367, 0x02ac, 0x036d, 0x0295, 0x0373, 0x0002, 0x0374, 0x02cf, 0x037a, 0x03f8, 0x0382, 0x0a6b, 0x0397, 0x031c, 0x039e, 0x0464, 0x03a7, 0x069d, 0x03b5, 0x03ab, 0x03bd, 0x036a, 0x03c4, 0x0368, 0x03cb, 0x03db, 0x03d3, 0x03ee, 0x03db, 0x03db, 0x03e3, 0x03a5, 0x03eb, 0x033c, 0x03f2, 0x0331, 0x03f9, 0x0002, 0x03fa, 0x0002, 0x03fb, 0x0002, 0x03fc, 0x0002, 0x03fd, 0x0002, 0x03fe, 0x0002, 0x03ff, 0x03ae, 0x0407, 0x0325, 0x040e, 0x0376, 0x0415, 0x04a9, 0x041f, 0x0374, 0x0426, 0x03e6, 0x042e, 0x03f6, 0x0436, 0x0670, 0x0443, 0x04ee, 0x044d, 0x0002, 0x044e, 0x013d, 0x0451, 0x0002, 0x0452, 0x0316, 0x0459, 0x04e2, 0x0463, 0x0508, 0x046e, 0x04d0, 0x0478, 0x031c, 0x047f, 0x0002, 0x0480, 0x0002, 0x0481, 0x0523, 0x048c, 0x03c8, 0x0494, 0x040e, 0x049d, 0x05af, 0x04a9, 0x02ea, 0x04af, 0x0382, 0x04b7, 0x0423, 0x04c0, 0x03ed, 0x04c8, 0x0356, 0x04cf, 0x04be, 0x04d9, 0x03c3, 0x04e1, 0x0002, 0x04e2, 0x03e9, 0x04ea, 0x038d, 0x04f2, 0x03f4, 0x04fa, 0x03f6, 0x0502, 0x03a8, 0x050a, 0x0002, 0x050b, 0x0670, 0x0518, 0x04ee, 0x0522, 0x0a6a, 0x0537, 0x0794, 0x0547, 0x032c, 0x054e, 0x044e, 0x0557, 0x04f3, 0x0561, 0x038c, 0x0569, 0x037f, 0x0570, 0x035e, 0x0577, 0x03c3, 0x057f, 0x03b4, 0x0587, 0x032b, 0x058e, 0x0348, 0x0595, 0x033b, 0x059c, 0x0380, 0x05a3, 0x0002, 0x05a4, 0x0648, 0x05b1, 0x0002, 0x05b2, 0x06a0, 0x05c0, 0x0002, 0x05c1, 0x031a, 0x05c8, 0x0072, 0x05c9, 0x0002, 0x05ca, 0x030b, 0x05d1, 0x0002, 0x05d2, 0x0002 },
                    new FileOffset[] {
                        new FileOffset("X1SHOP_T.BIN", 0x4978),
                        new FileOffset("X1OHRA_J.BIN", 0x48a8),
                        new FileOffset("X1MAYA_J.BIN", 0x48c0),
                        new FileOffset("X1LEMO3J.BIN", 0x4e1c),
                        new FileOffset("X1LEMO2J.BIN", 0x4b8c),
                        new FileOffset("X1LEMO1J.BIN", 0x5540),
                        new FileOffset("X1LECHA.BIN",  0x7050),
                        new FileOffset("X1HOW_J.BIN",  0x4f0c),
                        new FileOffset("X1GL_J.BIN",   0x5524),
                        new FileOffset("X1FUTT_J.BIN", 0x458c),
                        new FileOffset("X1FNKA6.BIN",  0x6b00),
                        new FileOffset("X1FNKA01.BIN", 0x7424),
                        new FileOffset("X1FNK6_J.BIN", 0x45a8),
                        new FileOffset("X1FNK1_J.BIN", 0x44e8),
                        new FileOffset("X1DST_J.BIN",  0x5368),
                        new FileOffset("X1DLMA_J.BIN", 0x51fc),
                        new FileOffset("X1DLMA2J.BIN", 0x44fc),
                        new FileOffset("X1BEERJI.BIN", 0x47f8),
                        new FileOffset("X1ASPI_H.BIN", 0x4bec),
                        new FileOffset("X007.BIN",     0x8ae0),
                    }
                )},
            }},

            { ScenarioType.PremiumDisk, new Dictionary<string, DataWithFileOffsets>() {
                { "CBF00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03db, 0x0008, 0x030c, 0x000f, 0x0425, 0x0018, 0x039f, 0x0020, 0x03b6, 0x0028, 0x0f0b },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x33c8),
                        new FileOffset("X1ENDING.BIN", 0x2d38),
                        new FileOffset("X022.BIN",     0x5ee4),
                    }
                )},

                { "CBF01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040b, 0x0009, 0x057c, 0x0014, 0x0479, 0x001d, 0x03fe, 0x0025, 0x0408, 0x002e, 0x0f3d },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x33e4),
                        new FileOffset("X1ENDING.BIN", 0x2d54),
                        new FileOffset("X022.BIN",     0x5f00),
                    }
                )},

                { "CBF02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0404, 0x0009, 0x04f3, 0x0013, 0x0431, 0x001c, 0x03f1, 0x0024, 0x0431, 0x002d, 0x1058 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3400),
                        new FileOffset("X1ENDING.BIN", 0x2d70),
                        new FileOffset("X022.BIN",     0x5f1c),
                    }
                )},

                { "CBF03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x03c6, 0x0010, 0x03c2, 0x0018, 0x0314, 0x001f, 0x03d4, 0x0027, 0x0fd1 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x341c),
                        new FileOffset("X1ENDING.BIN", 0x2d8c),
                        new FileOffset("X022.BIN",     0x5f38),
                    }
                )},

                { "CBF04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0386, 0x0008, 0x02bd, 0x000e, 0x03d4, 0x0016, 0x03d1, 0x001e, 0x03f8, 0x0026, 0x102c },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3438),
                        new FileOffset("X1ENDING.BIN", 0x2da8),
                        new FileOffset("X022.BIN",     0x5f54),
                    }
                )},

                { "CBF05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x047b, 0x0009, 0x03f8, 0x0011, 0x0498, 0x001b, 0x0418, 0x0024, 0x0453, 0x002d, 0x1268 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3454),
                        new FileOffset("X1ENDING.BIN", 0x2dc4),
                        new FileOffset("X022.BIN",     0x5f70),
                    }
                )},

                { "CBF06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035d, 0x0007, 0x03ee, 0x000f, 0x0398, 0x0017, 0x03ce, 0x001f, 0x033e, 0x0026, 0x0d10 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3470),
                        new FileOffset("X1ENDING.BIN", 0x2de0),
                        new FileOffset("X022.BIN",     0x5f8c),
                    }
                )},

                { "CBF07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x037d, 0x0007, 0x0423, 0x0010, 0x03be, 0x0018, 0x040c, 0x0021, 0x03e3, 0x0029, 0x0f89 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x348c),
                        new FileOffset("X1ENDING.BIN", 0x2dfc),
                        new FileOffset("X022.BIN",     0x5fa8),
                    }
                )},

                { "CBF08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04a1, 0x000a, 0x0385, 0x0012, 0x04bc, 0x001c, 0x03e9, 0x0024, 0x04b9, 0x002e, 0x1270 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x34a8),
                        new FileOffset("X1ENDING.BIN", 0x2e18),
                        new FileOffset("X022.BIN",     0x5fc4),
                    }
                )},

                { "CBF09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0432, 0x0009, 0x058e, 0x0015, 0x0445, 0x001e, 0x0395, 0x0026, 0x041d, 0x002f, 0x0a54 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x34c4),
                        new FileOffset("X1ENDING.BIN", 0x2e34),
                        new FileOffset("X022.BIN",     0x5fe0),
                    }
                )},

                { "CBF10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x045e, 0x0009, 0x0037, 0x000a, 0x03c2, 0x0012, 0x05ea, 0x001e, 0x037f, 0x0025, 0x0dfd },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x34e0),
                        new FileOffset("X1ENDING.BIN", 0x2e50),
                        new FileOffset("X022.BIN",     0x5ffc),
                    }
                )},

                { "CBF11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02b7, 0x0006, 0x03e6, 0x000e, 0x03c6, 0x0016, 0x0817, 0x0027, 0x03d4, 0x002f, 0x0f02 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x34fc),
                        new FileOffset("X1ENDING.BIN", 0x2e6c),
                        new FileOffset("X022.BIN",     0x6018),
                    }
                )},

                { "CBF12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0638, 0x000d, 0x03bf, 0x0015, 0x042e, 0x001e, 0x0405, 0x0027, 0x0419, 0x0030, 0x101a },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3518),
                        new FileOffset("X1ENDING.BIN", 0x2e88),
                        new FileOffset("X022.BIN",     0x6034),
                    }
                )},

                { "CBF13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03bb, 0x0008, 0x044d, 0x0011, 0x0453, 0x001a, 0x0381, 0x0022, 0x044a, 0x002b, 0x11d5 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3534),
                        new FileOffset("X1ENDING.BIN", 0x2ea4),
                        new FileOffset("X022.BIN",     0x6050),
                    }
                )},

                { "CBF14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x0370, 0x000d, 0x02c2, 0x0013, 0x039d, 0x001b, 0x03a4, 0x0023, 0x0e0f },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3550),
                        new FileOffset("X1ENDING.BIN", 0x2ec0),
                        new FileOffset("X022.BIN",     0x606c),
                    }
                )},

                { "CBF15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x032e, 0x000f, 0x0525, 0x001a, 0x02d9, 0x0020, 0x04f8, 0x002a, 0x138e },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x356c),
                        new FileOffset("X1ENDING.BIN", 0x2edc),
                        new FileOffset("X022.BIN",     0x6088),
                    }
                )},

                { "CBF16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03de, 0x0008, 0x0389, 0x0010, 0x0455, 0x0019, 0x0456, 0x0022, 0x0448, 0x002b, 0x10fe },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3588),
                        new FileOffset("X1ENDING.BIN", 0x2ef8),
                        new FileOffset("X022.BIN",     0x60a4),
                    }
                )},

                { "CBF17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0425, 0x0009, 0x0374, 0x0010, 0x0350, 0x0017, 0x039e, 0x001f, 0x0339, 0x0026, 0x0d18 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x35a4),
                        new FileOffset("X1ENDING.BIN", 0x2f14),
                        new FileOffset("X022.BIN",     0x60c0),
                    }
                )},

                { "CBF18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04ae, 0x000a, 0x04d0, 0x0014, 0x0397, 0x001c, 0x03ba, 0x0024, 0x03a8, 0x002c, 0x0ec5 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x35c0),
                        new FileOffset("X1ENDING.BIN", 0x2f30),
                        new FileOffset("X022.BIN",     0x60dc),
                    }
                )},

                { "CBF19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0443, 0x0009, 0x01b9, 0x000d, 0x0415, 0x0016, 0x0464, 0x001f, 0x040c, 0x0028, 0x0a0f },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x35dc),
                        new FileOffset("X1ENDING.BIN", 0x2f4c),
                        new FileOffset("X022.BIN",     0x60f8),
                    }
                )},

                { "CBF20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x0361, 0x000f, 0x0434, 0x0018, 0x03c6, 0x0020, 0x0431, 0x0029, 0x1151 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x35f8),
                        new FileOffset("X1ENDING.BIN", 0x2f68),
                        new FileOffset("X022.BIN",     0x6114),
                    }
                )},

                { "CBF21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0423, 0x0009, 0x0357, 0x0010, 0x0454, 0x0019, 0x03db, 0x0021, 0x0447, 0x002a, 0x0ea3 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3614),
                        new FileOffset("X1ENDING.BIN", 0x2f84),
                        new FileOffset("X022.BIN",     0x6130),
                    }
                )},

                { "CBF22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0370, 0x000f, 0x03a6, 0x0017, 0x03a5, 0x001f, 0x03b3, 0x0027, 0x0f8a },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3630),
                        new FileOffset("X1ENDING.BIN", 0x2fa0),
                        new FileOffset("X022.BIN",     0x614c),
                    }
                )},

                { "CBF23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x044c, 0x0009, 0x035e, 0x0010, 0x0488, 0x001a, 0x0331, 0x0021, 0x0476, 0x002a, 0x1242 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x364c),
                        new FileOffset("X1ENDING.BIN", 0x2fbc),
                        new FileOffset("X022.BIN",     0x6168),
                    }
                )},

                { "CBF24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x037c, 0x0010, 0x041f, 0x0019, 0x0325, 0x0020, 0x0433, 0x0029, 0x10e9 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3668),
                        new FileOffset("X1ENDING.BIN", 0x2fd8),
                        new FileOffset("X022.BIN",     0x6184),
                    }
                )},

                { "CBF25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x049d, 0x000a, 0x0389, 0x0012, 0x04ae, 0x001c, 0x033c, 0x0023, 0x04c0, 0x002d, 0x13f5 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3684),
                        new FileOffset("X1ENDING.BIN", 0x2ff4),
                        new FileOffset("X022.BIN",     0x61a0),
                    }
                )},

                { "CBF26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03bd, 0x0008, 0x0348, 0x000f, 0x0414, 0x0018, 0x04e2, 0x0022, 0x03cd, 0x002a, 0x0f01 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x36a0),
                        new FileOffset("X1ENDING.BIN", 0x3010),
                        new FileOffset("X022.BIN",     0x61bc),
                    }
                )},

                { "CBF27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048c, 0x000a, 0x034e, 0x0011, 0x0497, 0x001b, 0x0508, 0x0026, 0x07d2, 0x0036, 0x0bb9 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x36bc),
                        new FileOffset("X1ENDING.BIN", 0x302c),
                        new FileOffset("X022.BIN",     0x61d8),
                    }
                )},

                { "CBF28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0377, 0x0007, 0x02f3, 0x000d, 0x0381, 0x0015, 0x04d0, 0x001f, 0x037e, 0x0026, 0x0e47 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x36d8),
                        new FileOffset("X1ENDING.BIN", 0x3048),
                        new FileOffset("X022.BIN",     0x61f4),
                    }
                )},

                { "CBF29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d9, 0x0008, 0x0429, 0x0011, 0x03d9, 0x0019, 0x08c8, 0x002b, 0x03be, 0x0033, 0x0f32 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x36f4),
                        new FileOffset("X1ENDING.BIN", 0x3064),
                        new FileOffset("X022.BIN",     0x6210),
                    }
                )},

                { "CBF30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x031a, 0x000f, 0x039c, 0x0017, 0x0a03, 0x002c, 0x03a4, 0x0034, 0x0edd },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3710),
                        new FileOffset("X1ENDING.BIN", 0x3080),
                        new FileOffset("X022.BIN",     0x622c),
                    }
                )},

                { "CBF31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038d, 0x0008, 0x02ea, 0x000e, 0x03db, 0x0016, 0x03f6, 0x001e, 0x0435, 0x0027, 0x1165 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x372c),
                        new FileOffset("X1ENDING.BIN", 0x309c),
                        new FileOffset("X022.BIN",     0x6248),
                    }
                )},

                { "CBF32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f4, 0x0008, 0x0313, 0x000f, 0x03fa, 0x0017, 0x03a8, 0x001f, 0x03ed, 0x0027, 0x0f08 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3748),
                        new FileOffset("X1ENDING.BIN", 0x30b8),
                        new FileOffset("X022.BIN",     0x6264),
                    }
                )},

                { "CBF33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0794, 0x0010, 0x03f7, 0x0018, 0x0376, 0x001f, 0x0356, 0x0026, 0x0326, 0x002d, 0x0cf7 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3764),
                        new FileOffset("X1ENDING.BIN", 0x30d4),
                        new FileOffset("X022.BIN",     0x6280),
                    }
                )},

                { "CBF34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x032c, 0x0007, 0x0484, 0x0011, 0x0396, 0x0019, 0x0381, 0x0021, 0x0402, 0x002a, 0x1028 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3780),
                        new FileOffset("X1ENDING.BIN", 0x30f0),
                        new FileOffset("X022.BIN",     0x629c),
                    }
                )},

                { "CBF35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038c, 0x0008, 0x04c6, 0x0012, 0x03b6, 0x001a, 0x0380, 0x0021, 0x0421, 0x002a, 0x0fe4 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x379c),
                        new FileOffset("X1ENDING.BIN", 0x310c),
                        new FileOffset("X022.BIN",     0x62b8),
                    }
                )},

                { "CBF36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x037f, 0x0007, 0x0537, 0x0012, 0x0391, 0x001a, 0x033b, 0x0021, 0x03c6, 0x0029, 0x0f75 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x37b8),
                        new FileOffset("X1ENDING.BIN", 0x3128),
                        new FileOffset("X022.BIN",     0x62d4),
                    }
                )},

                { "CBF37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035e, 0x0007, 0x035d, 0x000e, 0x0538, 0x0019, 0x03f7, 0x0021, 0x0552, 0x002c, 0x11ce },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x37d4),
                        new FileOffset("X1ENDING.BIN", 0x3144),
                        new FileOffset("X022.BIN",     0x62f0),
                    }
                )},

                { "CBF38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0530, 0x0013, 0x03bf, 0x001b, 0x03c7, 0x0023, 0x04b8, 0x002d, 0x0df1 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x37f0),
                        new FileOffset("X1ENDING.BIN", 0x3160),
                        new FileOffset("X022.BIN",     0x630c),
                    }
                )},

                { "CBF39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c3, 0x0008, 0x02b8, 0x000e, 0x040f, 0x0017, 0x03f0, 0x001f, 0x0430, 0x0028, 0x1055 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x380c),
                        new FileOffset("X1ENDING.BIN", 0x317c),
                        new FileOffset("X022.BIN",     0x6328),
                    }
                )},

                { "CBF40.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b4, 0x0008, 0x041f, 0x0011, 0x03d4, 0x0019, 0x0409, 0x0022, 0x03cf, 0x002a, 0x0f0b },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3828),
                        new FileOffset("X1ENDING.BIN", 0x3198),
                        new FileOffset("X022.BIN",     0x6344),
                    }
                )},

                { "CBF41.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03eb, 0x0008, 0x090b, 0x001b, 0x0381, 0x0023, 0x090b, 0x0036, 0x0381, 0x003e, 0x090b },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3844),
                        new FileOffset("X1ENDING.BIN", 0x31b4),
                        new FileOffset("X022.BIN",     0x6360),
                    }
                )},

                { "CBF42.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x032b, 0x0007, 0x03dd, 0x000f, 0x0403, 0x0018, 0x03dd, 0x0020, 0x03df, 0x0028, 0x0f4c },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3860),
                        new FileOffset("X1ENDING.BIN", 0x31d0),
                        new FileOffset("X022.BIN",     0x637c),
                    }
                )},

                { "CBF43.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x064b, 0x000d, 0x0423, 0x0016, 0x0439, 0x001f, 0x035c, 0x0026, 0x042a, 0x002f, 0x111c },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x387c),
                        new FileOffset("X1ENDING.BIN", 0x31ec),
                        new FileOffset("X022.BIN",     0x6398),
                    }
                )},

                { "CBF44.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x034f, 0x0007, 0x033f, 0x000e, 0x03e2, 0x0016, 0x0f48, 0x0035, 0x03eb, 0x003d, 0x09b7 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3898),
                        new FileOffset("X1ENDING.BIN", 0x3208),
                        new FileOffset("X022.BIN",     0x63b4),
                    }
                )},

                { "CBF45.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x06d9, 0x000e, 0x02dd, 0x0014, 0x0448, 0x001d, 0x116a, 0x0040, 0x045d, 0x0049, 0x11a9 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x38b4),
                        new FileOffset("X1ENDING.BIN", 0x3224),
                        new FileOffset("X022.BIN",     0x63d0),
                    }
                )},

                { "CBF46.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0c43, 0x0019, 0x040d, 0x0022, 0x03c9, 0x002a, 0x0c43, 0x0043, 0x0384, 0x004b, 0x0b7c },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x38d0),
                        new FileOffset("X1ENDING.BIN", 0x3240),
                        new FileOffset("X022.BIN",     0x63ec),
                    }
                )},

                { "CBF47.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0543, 0x000b, 0x03b9, 0x0013, 0x03fb, 0x001b, 0x09d1, 0x002f, 0x0453, 0x0038, 0x0ac2 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x38ec),
                        new FileOffset("X1ENDING.BIN", 0x325c),
                        new FileOffset("X022.BIN",     0x6408),
                    }
                )},

                { "CBF48.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x025d, 0x000b, 0x03f6, 0x0013, 0x1008, 0x0034, 0x0406, 0x003d, 0x1032 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3908),
                        new FileOffset("X1ENDING.BIN", 0x3278),
                        new FileOffset("X022.BIN",     0x6424),
                    }
                )},

                { "CBF49.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0636, 0x000d, 0x02f2, 0x0013, 0x048a, 0x001d, 0x0afb, 0x0033, 0x0483, 0x003d, 0x0aea },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3924),
                        new FileOffset("X1ENDING.BIN", 0x3294),
                        new FileOffset("X022.BIN",     0x6440),
                    }
                )},

                { "CBF50.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0359, 0x0007, 0x04f8, 0x0011, 0x03cd, 0x0019, 0x0fca, 0x0039, 0x03b3, 0x0041, 0x0f5d },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3940),
                        new FileOffset("X1ENDING.BIN", 0x32b0),
                        new FileOffset("X022.BIN",     0x645c),
                    }
                )},

                { "CBF51.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04dc, 0x000a, 0x03a0, 0x0012, 0x0605, 0x001f, 0x1728, 0x004e, 0x05c3, 0x005a, 0x163c },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x395c),
                        new FileOffset("X1ENDING.BIN", 0x32cc),
                        new FileOffset("X022.BIN",     0x6478),
                    }
                )},

                { "CBF52.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02f9, 0x0006, 0x0360, 0x000d, 0x0420, 0x0016, 0x0c42, 0x002f, 0x0403, 0x0038, 0x0bef },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3978),
                        new FileOffset("X1ENDING.BIN", 0x32e8),
                        new FileOffset("X022.BIN",     0x6494),
                    }
                )},

                { "CBF53.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0398, 0x0008, 0x0507, 0x0013, 0x0454, 0x001c, 0x1094, 0x003e, 0x0417, 0x0047, 0x0fac },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3994),
                        new FileOffset("X1ENDING.BIN", 0x3304),
                        new FileOffset("X022.BIN",     0x64b0),
                    }
                )},

                { "CBF54.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x046b, 0x0009, 0x04d1, 0x0013, 0x03f0, 0x001b, 0x0f47, 0x003a, 0x03dd, 0x0042, 0x0ed4 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x39b0),
                        new FileOffset("X1ENDING.BIN", 0x3320),
                        new FileOffset("X022.BIN",     0x64cc),
                    }
                )},

                { "CBF55.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0437, 0x0009, 0x03ec, 0x0011, 0x06f4, 0x001f, 0x10d0, 0x0041, 0x06e6, 0x004f, 0x10b2 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x39cc),
                        new FileOffset("X1ENDING.BIN", 0x333c),
                        new FileOffset("X022.BIN",     0x64e8),
                    }
                )},

                { "CBF56.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ab, 0x0008, 0x039d, 0x0010, 0x04be, 0x001a, 0x0fea, 0x003a, 0x04c3, 0x0044, 0x1014 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x39e8),
                        new FileOffset("X1ENDING.BIN", 0x3358),
                        new FileOffset("X022.BIN",     0x6504),
                    }
                )},

                { "CBF57.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02f0, 0x0006, 0x047e, 0x000f, 0x0598, 0x001b, 0x13db, 0x0043, 0x05c3, 0x004f, 0x1443 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3a04),
                        new FileOffset("X1ENDING.BIN", 0x3374),
                        new FileOffset("X022.BIN",     0x6520),
                    }
                )},

                { "CBF58.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x034e, 0x0007, 0x04b4, 0x0011, 0x033f, 0x0018, 0x0ded, 0x0034, 0x035f, 0x003b, 0x0e66 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3a20),
                        new FileOffset("X1ENDING.BIN", 0x3390),
                        new FileOffset("X022.BIN",     0x653c),
                    }
                )},

                { "CBF59.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04d0, 0x000a, 0x02ca, 0x0010, 0x034f, 0x0017, 0x0878, 0x0028, 0x0434, 0x0031, 0x0878 },
                    new FileOffset[] {
                        new FileOffset("X1ENDING.BIN", 0x3a3c),
                        new FileOffset("X1ENDING.BIN", 0x33ac),
                        new FileOffset("X022.BIN",     0x6558),
                    }
                )},

                { "CBP00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03db, 0x0008, 0x0f4c, 0x0027, 0x0425, 0x0030, 0x10b9, 0x0052, 0x03b6, 0x005a, 0x0f0b },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x363c),
                        new FileOffset("X1BTLP09.BIN", 0x34d8),
                        new FileOffset("X1BTLP08.BIN", 0x3778),
                        new FileOffset("X1BTLP07.BIN", 0x38c4),
                        new FileOffset("X1BTLP06.BIN", 0x3524),
                        new FileOffset("X1BTLP04.BIN", 0x3038),
                        new FileOffset("X1BTLP03.BIN", 0x3a74),
                        new FileOffset("X1BTLP02.BIN", 0x3430),
                        new FileOffset("X1BTLP01.BIN", 0x3758),
                        new FileOffset("X007.BIN",     0x8114),
                    }
                )},

                { "CBP01.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040b, 0x0009, 0x0f7d, 0x0028, 0x0479, 0x0031, 0x1179, 0x0054, 0x0408, 0x005d, 0x0f3d },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3658),
                        new FileOffset("X1BTLP09.BIN", 0x34f4),
                        new FileOffset("X1BTLP08.BIN", 0x3794),
                        new FileOffset("X1BTLP07.BIN", 0x38e0),
                        new FileOffset("X1BTLP06.BIN", 0x3540),
                        new FileOffset("X1BTLP04.BIN", 0x3054),
                        new FileOffset("X1BTLP03.BIN", 0x3a90),
                        new FileOffset("X1BTLP02.BIN", 0x344c),
                        new FileOffset("X1BTLP01.BIN", 0x3774),
                        new FileOffset("X007.BIN",     0x8130),
                    }
                )},

                { "CBP02.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0404, 0x0009, 0x0f6d, 0x0028, 0x0431, 0x0031, 0x1045, 0x0052, 0x0431, 0x005b, 0x1058 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3674),
                        new FileOffset("X1BTLP09.BIN", 0x3510),
                        new FileOffset("X1BTLP08.BIN", 0x37b0),
                        new FileOffset("X1BTLP07.BIN", 0x38fc),
                        new FileOffset("X1BTLP06.BIN", 0x355c),
                        new FileOffset("X1BTLP04.BIN", 0x3070),
                        new FileOffset("X1BTLP03.BIN", 0x3aac),
                        new FileOffset("X1BTLP02.BIN", 0x3468),
                        new FileOffset("X1BTLP01.BIN", 0x3790),
                        new FileOffset("X007.BIN",     0x814c),
                    }
                )},

                { "CBP03.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x109a, 0x002a, 0x03c2, 0x0032, 0x0fc4, 0x0052, 0x03d4, 0x005a, 0x0fd1 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3690),
                        new FileOffset("X1BTLP09.BIN", 0x352c),
                        new FileOffset("X1BTLP08.BIN", 0x37cc),
                        new FileOffset("X1BTLP07.BIN", 0x3918),
                        new FileOffset("X1BTLP06.BIN", 0x3578),
                        new FileOffset("X1BTLP04.BIN", 0x308c),
                        new FileOffset("X1BTLP03.BIN", 0x3ac8),
                        new FileOffset("X1BTLP02.BIN", 0x3484),
                        new FileOffset("X1BTLP01.BIN", 0x37ac),
                        new FileOffset("X007.BIN",     0x8168),
                    }
                )},

                { "CBP04.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0386, 0x0008, 0x1586, 0x0034, 0x03d4, 0x003c, 0x0fa4, 0x005c, 0x03f8, 0x0064, 0x102c },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x36ac),
                        new FileOffset("X1BTLP09.BIN", 0x3548),
                        new FileOffset("X1BTLP08.BIN", 0x37e8),
                        new FileOffset("X1BTLP07.BIN", 0x3934),
                        new FileOffset("X1BTLP06.BIN", 0x3594),
                        new FileOffset("X1BTLP04.BIN", 0x30a8),
                        new FileOffset("X1BTLP03.BIN", 0x3ae4),
                        new FileOffset("X1BTLP02.BIN", 0x34a0),
                        new FileOffset("X1BTLP01.BIN", 0x37c8),
                        new FileOffset("X007.BIN",     0x8184),
                    }
                )},

                { "CBP05.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x047b, 0x0009, 0x12ba, 0x002f, 0x0498, 0x0039, 0x1377, 0x0060, 0x0453, 0x0069, 0x1268 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x36c8),
                        new FileOffset("X1BTLP09.BIN", 0x3564),
                        new FileOffset("X1BTLP08.BIN", 0x3804),
                        new FileOffset("X1BTLP07.BIN", 0x3950),
                        new FileOffset("X1BTLP06.BIN", 0x35b0),
                        new FileOffset("X1BTLP04.BIN", 0x30c4),
                        new FileOffset("X1BTLP03.BIN", 0x3b00),
                        new FileOffset("X1BTLP02.BIN", 0x34bc),
                        new FileOffset("X1BTLP01.BIN", 0x37e4),
                        new FileOffset("X007.BIN",     0x81a0),
                    }
                )},

                { "CBP06.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x035d, 0x0007, 0x0d7f, 0x0022, 0x0398, 0x002a, 0x0ea5, 0x0048, 0x033e, 0x004f, 0x0d10 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x36e4),
                        new FileOffset("X1BTLP09.BIN", 0x3580),
                        new FileOffset("X1BTLP08.BIN", 0x3820),
                        new FileOffset("X1BTLP07.BIN", 0x396c),
                        new FileOffset("X1BTLP06.BIN", 0x35cc),
                        new FileOffset("X1BTLP04.BIN", 0x30e0),
                        new FileOffset("X1BTLP03.BIN", 0x3b1c),
                        new FileOffset("X1BTLP02.BIN", 0x34d8),
                        new FileOffset("X1BTLP01.BIN", 0x3800),
                        new FileOffset("X007.BIN",     0x81bc),
                    }
                )},

                { "CBP07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x037d, 0x0007, 0x0e5b, 0x0024, 0x03be, 0x002c, 0x0f06, 0x004b, 0x03e3, 0x0053, 0x0f89 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3700),
                        new FileOffset("X1BTLP09.BIN", 0x359c),
                        new FileOffset("X1BTLP08.BIN", 0x383c),
                        new FileOffset("X1BTLP07.BIN", 0x3988),
                        new FileOffset("X1BTLP06.BIN", 0x35e8),
                        new FileOffset("X1BTLP04.BIN", 0x30fc),
                        new FileOffset("X1BTLP03.BIN", 0x3b38),
                        new FileOffset("X1BTLP02.BIN", 0x34f4),
                        new FileOffset("X1BTLP01.BIN", 0x381c),
                        new FileOffset("X007.BIN",     0x81d8),
                    }
                )},

                { "CBP08.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04a1, 0x000a, 0x1209, 0x002f, 0x04bc, 0x0039, 0x1263, 0x005e, 0x04b9, 0x0068, 0x1270 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x371c),
                        new FileOffset("X1BTLP09.BIN", 0x35b8),
                        new FileOffset("X1BTLP08.BIN", 0x3858),
                        new FileOffset("X1BTLP07.BIN", 0x39a4),
                        new FileOffset("X1BTLP06.BIN", 0x3604),
                        new FileOffset("X1BTLP04.BIN", 0x3118),
                        new FileOffset("X1BTLP03.BIN", 0x3b54),
                        new FileOffset("X1BTLP02.BIN", 0x3510),
                        new FileOffset("X1BTLP01.BIN", 0x3838),
                        new FileOffset("X007.BIN",     0x81f4),
                    }
                )},

                { "CBP09.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0432, 0x0009, 0x0a77, 0x001e, 0x0445, 0x0027, 0x0aad, 0x003d, 0x041d, 0x0046, 0x0a54 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3738),
                        new FileOffset("X1BTLP09.BIN", 0x35d4),
                        new FileOffset("X1BTLP08.BIN", 0x3874),
                        new FileOffset("X1BTLP07.BIN", 0x39c0),
                        new FileOffset("X1BTLP06.BIN", 0x3620),
                        new FileOffset("X1BTLP04.BIN", 0x3134),
                        new FileOffset("X1BTLP03.BIN", 0x3b70),
                        new FileOffset("X1BTLP02.BIN", 0x352c),
                        new FileOffset("X1BTLP01.BIN", 0x3854),
                        new FileOffset("X007.BIN",     0x8210),
                    }
                )},

                { "CBP10.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x045e, 0x0009, 0x1160, 0x002c, 0x03c2, 0x0034, 0x0f29, 0x0053, 0x037f, 0x005a, 0x0dfd },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3754),
                        new FileOffset("X1BTLP09.BIN", 0x35f0),
                        new FileOffset("X1BTLP08.BIN", 0x3890),
                        new FileOffset("X1BTLP07.BIN", 0x39dc),
                        new FileOffset("X1BTLP06.BIN", 0x363c),
                        new FileOffset("X1BTLP04.BIN", 0x3150),
                        new FileOffset("X1BTLP03.BIN", 0x3b8c),
                        new FileOffset("X1BTLP02.BIN", 0x3548),
                        new FileOffset("X1BTLP01.BIN", 0x3870),
                        new FileOffset("X007.BIN",     0x822c),
                    }
                )},

                { "CBP11.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x0ebe, 0x0026, 0x03c6, 0x002e, 0x0ebe, 0x004c, 0x03d4, 0x0054, 0x0f02 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3770),
                        new FileOffset("X1BTLP09.BIN", 0x360c),
                        new FileOffset("X1BTLP08.BIN", 0x38ac),
                        new FileOffset("X1BTLP07.BIN", 0x39f8),
                        new FileOffset("X1BTLP06.BIN", 0x3658),
                        new FileOffset("X1BTLP04.BIN", 0x316c),
                        new FileOffset("X1BTLP03.BIN", 0x3ba8),
                        new FileOffset("X1BTLP02.BIN", 0x3564),
                        new FileOffset("X1BTLP01.BIN", 0x388c),
                        new FileOffset("X007.BIN",     0x8248),
                    }
                )},

                { "CBP12.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x042e, 0x0009, 0x1026, 0x002a, 0x042e, 0x0033, 0x1026, 0x0054, 0x0419, 0x005d, 0x101a },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x378c),
                        new FileOffset("X1BTLP09.BIN", 0x3628),
                        new FileOffset("X1BTLP08.BIN", 0x38c8),
                        new FileOffset("X1BTLP07.BIN", 0x3a14),
                        new FileOffset("X1BTLP06.BIN", 0x3674),
                        new FileOffset("X1BTLP04.BIN", 0x3188),
                        new FileOffset("X1BTLP03.BIN", 0x3bc4),
                        new FileOffset("X1BTLP02.BIN", 0x3580),
                        new FileOffset("X1BTLP01.BIN", 0x38a8),
                        new FileOffset("X007.BIN",     0x8264),
                    }
                )},

                { "CBP13.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0453, 0x0009, 0x11f2, 0x002d, 0x0453, 0x0036, 0x11f2, 0x005a, 0x044a, 0x0063, 0x11d5 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x37a8),
                        new FileOffset("X1BTLP09.BIN", 0x3644),
                        new FileOffset("X1BTLP08.BIN", 0x38e4),
                        new FileOffset("X1BTLP07.BIN", 0x3a30),
                        new FileOffset("X1BTLP06.BIN", 0x3690),
                        new FileOffset("X1BTLP04.BIN", 0x31a4),
                        new FileOffset("X1BTLP03.BIN", 0x3be0),
                        new FileOffset("X1BTLP02.BIN", 0x359c),
                        new FileOffset("X1BTLP01.BIN", 0x38c4),
                        new FileOffset("X007.BIN",     0x8280),
                    }
                )},

                { "CBP14.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x02da, 0x0006, 0x0b16, 0x001d, 0x02c2, 0x0023, 0x0aa0, 0x0039, 0x03a4, 0x0041, 0x0e0f },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x37c4),
                        new FileOffset("X1BTLP09.BIN", 0x3660),
                        new FileOffset("X1BTLP08.BIN", 0x3900),
                        new FileOffset("X1BTLP07.BIN", 0x3a4c),
                        new FileOffset("X1BTLP06.BIN", 0x36ac),
                        new FileOffset("X1BTLP04.BIN", 0x31c0),
                        new FileOffset("X1BTLP03.BIN", 0x3bfc),
                        new FileOffset("X1BTLP02.BIN", 0x35b8),
                        new FileOffset("X1BTLP01.BIN", 0x38e0),
                        new FileOffset("X007.BIN",     0x829c),
                    }
                )},

                { "CBP15.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0525, 0x000b, 0x141d, 0x0034, 0x0525, 0x003f, 0x141d, 0x0068, 0x04f8, 0x0072, 0x138e },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x37e0),
                        new FileOffset("X1BTLP09.BIN", 0x367c),
                        new FileOffset("X1BTLP08.BIN", 0x391c),
                        new FileOffset("X1BTLP07.BIN", 0x3a68),
                        new FileOffset("X1BTLP06.BIN", 0x36c8),
                        new FileOffset("X1BTLP04.BIN", 0x31dc),
                        new FileOffset("X1BTLP03.BIN", 0x3c18),
                        new FileOffset("X1BTLP02.BIN", 0x35d4),
                        new FileOffset("X1BTLP01.BIN", 0x38fc),
                        new FileOffset("X007.BIN",     0x82b8),
                    }
                )},

                { "CBP16.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0455, 0x0009, 0x0aa6, 0x001f, 0x0455, 0x0028, 0x0aa6, 0x003e, 0x0448, 0x0047, 0x10fe },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x37fc),
                        new FileOffset("X1BTLP09.BIN", 0x3698),
                        new FileOffset("X1BTLP08.BIN", 0x3938),
                        new FileOffset("X1BTLP07.BIN", 0x3a84),
                        new FileOffset("X1BTLP06.BIN", 0x36e4),
                        new FileOffset("X1BTLP04.BIN", 0x31f8),
                        new FileOffset("X1BTLP03.BIN", 0x3c34),
                        new FileOffset("X1BTLP02.BIN", 0x35f0),
                        new FileOffset("X1BTLP01.BIN", 0x3918),
                        new FileOffset("X007.BIN",     0x82d4),
                    }
                )},

                { "CBP17.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0350, 0x0007, 0x0d79, 0x0022, 0x0350, 0x0029, 0x0d79, 0x0044, 0x0339, 0x004b, 0x0d18 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3818),
                        new FileOffset("X1BTLP09.BIN", 0x36b4),
                        new FileOffset("X1BTLP08.BIN", 0x3954),
                        new FileOffset("X1BTLP07.BIN", 0x3aa0),
                        new FileOffset("X1BTLP06.BIN", 0x3700),
                        new FileOffset("X1BTLP04.BIN", 0x3214),
                        new FileOffset("X1BTLP03.BIN", 0x3c50),
                        new FileOffset("X1BTLP02.BIN", 0x360c),
                        new FileOffset("X1BTLP01.BIN", 0x3934),
                        new FileOffset("X007.BIN",     0x82f0),
                    }
                )},

                { "CBP18.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0397, 0x0008, 0x0e92, 0x0026, 0x0397, 0x002e, 0x0e92, 0x004c, 0x03a8, 0x0054, 0x0ec5 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3834),
                        new FileOffset("X1BTLP09.BIN", 0x36d0),
                        new FileOffset("X1BTLP08.BIN", 0x3970),
                        new FileOffset("X1BTLP07.BIN", 0x3abc),
                        new FileOffset("X1BTLP06.BIN", 0x371c),
                        new FileOffset("X1BTLP04.BIN", 0x3230),
                        new FileOffset("X1BTLP03.BIN", 0x3c6c),
                        new FileOffset("X1BTLP02.BIN", 0x3628),
                        new FileOffset("X1BTLP01.BIN", 0x3950),
                        new FileOffset("X007.BIN",     0x830c),
                    }
                )},

                { "CBP19.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0415, 0x0009, 0x0a21, 0x001e, 0x0415, 0x0027, 0x0a21, 0x003c, 0x040c, 0x0045, 0x0a0f },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3850),
                        new FileOffset("X1BTLP09.BIN", 0x36ec),
                        new FileOffset("X1BTLP08.BIN", 0x398c),
                        new FileOffset("X1BTLP07.BIN", 0x3ad8),
                        new FileOffset("X1BTLP06.BIN", 0x3738),
                        new FileOffset("X1BTLP04.BIN", 0x324c),
                        new FileOffset("X1BTLP03.BIN", 0x3c88),
                        new FileOffset("X1BTLP02.BIN", 0x3644),
                        new FileOffset("X1BTLP01.BIN", 0x396c),
                        new FileOffset("X007.BIN",     0x8328),
                    }
                )},

                { "CBP20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f9, 0x0008, 0x1015, 0x0029, 0x0434, 0x0032, 0x1127, 0x0055, 0x0431, 0x005e, 0x1151 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x386c),
                        new FileOffset("X1BTLP09.BIN", 0x3708),
                        new FileOffset("X1BTLP08.BIN", 0x39a8),
                        new FileOffset("X1BTLP07.BIN", 0x3af4),
                        new FileOffset("X1BTLP06.BIN", 0x3754),
                        new FileOffset("X1BTLP04.BIN", 0x3268),
                        new FileOffset("X1BTLP03.BIN", 0x3ca4),
                        new FileOffset("X1BTLP02.BIN", 0x3660),
                        new FileOffset("X1BTLP01.BIN", 0x3988),
                        new FileOffset("X007.BIN",     0x8344),
                    }
                )},

                { "CBP21.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0423, 0x0009, 0x0e41, 0x0026, 0x0454, 0x002f, 0x0ef8, 0x004d, 0x0447, 0x0056, 0x0ea3 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3888),
                        new FileOffset("X1BTLP09.BIN", 0x3724),
                        new FileOffset("X1BTLP08.BIN", 0x39c4),
                        new FileOffset("X1BTLP07.BIN", 0x3b10),
                        new FileOffset("X1BTLP06.BIN", 0x3770),
                        new FileOffset("X1BTLP04.BIN", 0x3284),
                        new FileOffset("X1BTLP03.BIN", 0x3cc0),
                        new FileOffset("X1BTLP02.BIN", 0x367c),
                        new FileOffset("X1BTLP01.BIN", 0x39a4),
                        new FileOffset("X007.BIN",     0x8360),
                    }
                )},

                { "CBP22.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0390, 0x0008, 0x0ef6, 0x0026, 0x03a6, 0x002e, 0x0f5e, 0x004d, 0x03b3, 0x0055, 0x0f8a },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x38a4),
                        new FileOffset("X1BTLP09.BIN", 0x3740),
                        new FileOffset("X1BTLP08.BIN", 0x39e0),
                        new FileOffset("X1BTLP07.BIN", 0x3b2c),
                        new FileOffset("X1BTLP06.BIN", 0x378c),
                        new FileOffset("X1BTLP04.BIN", 0x32a0),
                        new FileOffset("X1BTLP03.BIN", 0x3cdc),
                        new FileOffset("X1BTLP02.BIN", 0x3698),
                        new FileOffset("X1BTLP01.BIN", 0x39c0),
                        new FileOffset("X007.BIN",     0x837c),
                    }
                )},

                { "CBP23.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x044c, 0x0009, 0x11a9, 0x002d, 0x0488, 0x0037, 0x128e, 0x005d, 0x0476, 0x0066, 0x1242 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x38c0),
                        new FileOffset("X1BTLP09.BIN", 0x375c),
                        new FileOffset("X1BTLP08.BIN", 0x39fc),
                        new FileOffset("X1BTLP07.BIN", 0x3b48),
                        new FileOffset("X1BTLP06.BIN", 0x37a8),
                        new FileOffset("X1BTLP04.BIN", 0x32bc),
                        new FileOffset("X1BTLP03.BIN", 0x3cf8),
                        new FileOffset("X1BTLP02.BIN", 0x36b4),
                        new FileOffset("X1BTLP01.BIN", 0x39dc),
                        new FileOffset("X007.BIN",     0x8398),
                    }
                )},

                { "CBP24.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x1089, 0x002b, 0x041f, 0x0034, 0x1081, 0x0056, 0x0433, 0x005f, 0x10e9 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x38dc),
                        new FileOffset("X1BTLP09.BIN", 0x3778),
                        new FileOffset("X1BTLP08.BIN", 0x3a18),
                        new FileOffset("X1BTLP07.BIN", 0x3b64),
                        new FileOffset("X1BTLP06.BIN", 0x37c4),
                        new FileOffset("X1BTLP04.BIN", 0x32d8),
                        new FileOffset("X1BTLP03.BIN", 0x3d14),
                        new FileOffset("X1BTLP02.BIN", 0x36d0),
                        new FileOffset("X1BTLP01.BIN", 0x39f8),
                        new FileOffset("X007.BIN",     0x83b4),
                    }
                )},

                { "CBP25.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x049d, 0x000a, 0x1372, 0x0031, 0x04ae, 0x003b, 0x1370, 0x0062, 0x04c0, 0x006c, 0x13f5 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x38f8),
                        new FileOffset("X1BTLP09.BIN", 0x3794),
                        new FileOffset("X1BTLP08.BIN", 0x3a34),
                        new FileOffset("X1BTLP07.BIN", 0x3b80),
                        new FileOffset("X1BTLP06.BIN", 0x37e0),
                        new FileOffset("X1BTLP04.BIN", 0x32f4),
                        new FileOffset("X1BTLP03.BIN", 0x3d30),
                        new FileOffset("X1BTLP02.BIN", 0x36ec),
                        new FileOffset("X1BTLP01.BIN", 0x3a14),
                        new FileOffset("X007.BIN",     0x83d0),
                    }
                )},

                { "CBP26.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03bd, 0x0008, 0x0efb, 0x0026, 0x0414, 0x002f, 0x103c, 0x0050, 0x03cd, 0x0058, 0x0f01 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3914),
                        new FileOffset("X1BTLP09.BIN", 0x37b0),
                        new FileOffset("X1BTLP08.BIN", 0x3a50),
                        new FileOffset("X1BTLP07.BIN", 0x3b9c),
                        new FileOffset("X1BTLP06.BIN", 0x37fc),
                        new FileOffset("X1BTLP04.BIN", 0x3310),
                        new FileOffset("X1BTLP03.BIN", 0x3d4c),
                        new FileOffset("X1BTLP02.BIN", 0x3708),
                        new FileOffset("X1BTLP01.BIN", 0x3a30),
                        new FileOffset("X007.BIN",     0x83ec),
                    }
                )},

                { "CBP27.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048c, 0x000a, 0x0b54, 0x0021, 0x0497, 0x002b, 0x0b57, 0x0042, 0x07d2, 0x0052, 0x0bb9 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3930),
                        new FileOffset("X1BTLP09.BIN", 0x37cc),
                        new FileOffset("X1BTLP08.BIN", 0x3a6c),
                        new FileOffset("X1BTLP07.BIN", 0x3bb8),
                        new FileOffset("X1BTLP06.BIN", 0x3818),
                        new FileOffset("X1BTLP04.BIN", 0x332c),
                        new FileOffset("X1BTLP03.BIN", 0x3d68),
                        new FileOffset("X1BTLP02.BIN", 0x3724),
                        new FileOffset("X1BTLP01.BIN", 0x3a4c),
                        new FileOffset("X007.BIN",     0x8408),
                    }
                )},

                { "CBP28.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0377, 0x0007, 0x0e2f, 0x0024, 0x0381, 0x002c, 0x0e72, 0x0049, 0x037e, 0x0050, 0x0e47 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x394c),
                        new FileOffset("X1BTLP09.BIN", 0x37e8),
                        new FileOffset("X1BTLP08.BIN", 0x3a88),
                        new FileOffset("X1BTLP07.BIN", 0x3bd4),
                        new FileOffset("X1BTLP06.BIN", 0x3834),
                        new FileOffset("X1BTLP04.BIN", 0x3348),
                        new FileOffset("X1BTLP03.BIN", 0x3d84),
                        new FileOffset("X1BTLP02.BIN", 0x3740),
                        new FileOffset("X1BTLP01.BIN", 0x3a68),
                        new FileOffset("X007.BIN",     0x8424),
                    }
                )},

                { "CBP29.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d9, 0x0008, 0x0f94, 0x0028, 0x03d9, 0x0030, 0x0fb7, 0x0050, 0x03be, 0x0058, 0x0f32 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3968),
                        new FileOffset("X1BTLP09.BIN", 0x3804),
                        new FileOffset("X1BTLP08.BIN", 0x3aa4),
                        new FileOffset("X1BTLP07.BIN", 0x3bf0),
                        new FileOffset("X1BTLP06.BIN", 0x3850),
                        new FileOffset("X1BTLP04.BIN", 0x3364),
                        new FileOffset("X1BTLP03.BIN", 0x3da0),
                        new FileOffset("X1BTLP02.BIN", 0x375c),
                        new FileOffset("X1BTLP01.BIN", 0x3a84),
                        new FileOffset("X007.BIN",     0x8440),
                    }
                )},

                { "CBP30.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e3, 0x0008, 0x0ffb, 0x0028, 0x039c, 0x0030, 0x0ed3, 0x004e, 0x03a4, 0x0056, 0x0edd },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3984),
                        new FileOffset("X1BTLP09.BIN", 0x3820),
                        new FileOffset("X1BTLP08.BIN", 0x3ac0),
                        new FileOffset("X1BTLP07.BIN", 0x3c0c),
                        new FileOffset("X1BTLP06.BIN", 0x386c),
                        new FileOffset("X1BTLP04.BIN", 0x3380),
                        new FileOffset("X1BTLP03.BIN", 0x3dbc),
                        new FileOffset("X1BTLP02.BIN", 0x3778),
                        new FileOffset("X1BTLP01.BIN", 0x3aa0),
                        new FileOffset("X007.BIN",     0x845c),
                    }
                )},

                { "CBP31.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03db, 0x0008, 0x0fc9, 0x0028, 0x03db, 0x0030, 0x0fc9, 0x0050, 0x0435, 0x0059, 0x1165 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x39a0),
                        new FileOffset("X1BTLP09.BIN", 0x383c),
                        new FileOffset("X1BTLP08.BIN", 0x3adc),
                        new FileOffset("X1BTLP07.BIN", 0x3c28),
                        new FileOffset("X1BTLP06.BIN", 0x3888),
                        new FileOffset("X1BTLP04.BIN", 0x339c),
                        new FileOffset("X1BTLP03.BIN", 0x3dd8),
                        new FileOffset("X1BTLP02.BIN", 0x3794),
                        new FileOffset("X1BTLP01.BIN", 0x3abc),
                        new FileOffset("X007.BIN",     0x8478),
                    }
                )},

                { "CBP32.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fa, 0x0008, 0x0f16, 0x0027, 0x03fa, 0x002f, 0x0f16, 0x004e, 0x03ed, 0x0056, 0x0f08 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x39bc),
                        new FileOffset("X1BTLP09.BIN", 0x3858),
                        new FileOffset("X1BTLP08.BIN", 0x3af8),
                        new FileOffset("X1BTLP07.BIN", 0x3c44),
                        new FileOffset("X1BTLP06.BIN", 0x38a4),
                        new FileOffset("X1BTLP04.BIN", 0x33b8),
                        new FileOffset("X1BTLP03.BIN", 0x3df4),
                        new FileOffset("X1BTLP02.BIN", 0x37b0),
                        new FileOffset("X1BTLP01.BIN", 0x3ad8),
                        new FileOffset("X007.BIN",     0x8494),
                    }
                )},

                { "CBP33.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0376, 0x0007, 0x0e61, 0x0024, 0x0376, 0x002b, 0x0e61, 0x0048, 0x0326, 0x004f, 0x0cf7 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x39d8),
                        new FileOffset("X1BTLP09.BIN", 0x3874),
                        new FileOffset("X1BTLP08.BIN", 0x3b14),
                        new FileOffset("X1BTLP07.BIN", 0x3c60),
                        new FileOffset("X1BTLP06.BIN", 0x38c0),
                        new FileOffset("X1BTLP04.BIN", 0x33d4),
                        new FileOffset("X1BTLP03.BIN", 0x3e10),
                        new FileOffset("X1BTLP02.BIN", 0x37cc),
                        new FileOffset("X1BTLP01.BIN", 0x3af4),
                        new FileOffset("X007.BIN",     0x84b0),
                    }
                )},

                { "CBP34.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0396, 0x0008, 0x0ec8, 0x0026, 0x0396, 0x002e, 0x0ec8, 0x004c, 0x0402, 0x0055, 0x1028 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x39f4),
                        new FileOffset("X1BTLP09.BIN", 0x3890),
                        new FileOffset("X1BTLP08.BIN", 0x3b30),
                        new FileOffset("X1BTLP07.BIN", 0x3c7c),
                        new FileOffset("X1BTLP06.BIN", 0x38dc),
                        new FileOffset("X1BTLP04.BIN", 0x33f0),
                        new FileOffset("X1BTLP03.BIN", 0x3e2c),
                        new FileOffset("X1BTLP02.BIN", 0x37e8),
                        new FileOffset("X1BTLP01.BIN", 0x3b10),
                        new FileOffset("X007.BIN",     0x84cc),
                    }
                )},

                { "CBP35.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03b6, 0x0008, 0x0e4c, 0x0025, 0x03b6, 0x002d, 0x0e4c, 0x004a, 0x0421, 0x0053, 0x0fe4 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a10),
                        new FileOffset("X1BTLP09.BIN", 0x38ac),
                        new FileOffset("X1BTLP08.BIN", 0x3b4c),
                        new FileOffset("X1BTLP07.BIN", 0x3c98),
                        new FileOffset("X1BTLP06.BIN", 0x38f8),
                        new FileOffset("X1BTLP04.BIN", 0x340c),
                        new FileOffset("X1BTLP03.BIN", 0x3e48),
                        new FileOffset("X1BTLP02.BIN", 0x3804),
                        new FileOffset("X1BTLP01.BIN", 0x3b2c),
                        new FileOffset("X007.BIN",     0x84e8),
                    }
                )},

                { "CBP36.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0391, 0x0008, 0x0ed0, 0x0026, 0x0391, 0x002e, 0x0ed0, 0x004c, 0x03c6, 0x0054, 0x0f75 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a2c),
                        new FileOffset("X1BTLP09.BIN", 0x38c8),
                        new FileOffset("X1BTLP08.BIN", 0x3b68),
                        new FileOffset("X1BTLP07.BIN", 0x3cb4),
                        new FileOffset("X1BTLP06.BIN", 0x3914),
                        new FileOffset("X1BTLP04.BIN", 0x3428),
                        new FileOffset("X1BTLP03.BIN", 0x3e64),
                        new FileOffset("X1BTLP02.BIN", 0x3820),
                        new FileOffset("X1BTLP01.BIN", 0x3b48),
                        new FileOffset("X007.BIN",     0x8504),
                    }
                )},

                { "CBP37.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0538, 0x000b, 0x1170, 0x002e, 0x0538, 0x0039, 0x1170, 0x005c, 0x0552, 0x0067, 0x11ce },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a48),
                        new FileOffset("X1BTLP09.BIN", 0x38e4),
                        new FileOffset("X1BTLP08.BIN", 0x3b84),
                        new FileOffset("X1BTLP07.BIN", 0x3cd0),
                        new FileOffset("X1BTLP06.BIN", 0x3930),
                        new FileOffset("X1BTLP04.BIN", 0x3444),
                        new FileOffset("X1BTLP03.BIN", 0x3e80),
                        new FileOffset("X1BTLP02.BIN", 0x383c),
                        new FileOffset("X1BTLP01.BIN", 0x3b64),
                        new FileOffset("X007.BIN",     0x8520),
                    }
                )},

                { "CBP38.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03ed, 0x0008, 0x0bae, 0x0020, 0x03bf, 0x0028, 0x0b2b, 0x003f, 0x04b8, 0x0049, 0x0df1 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a64),
                        new FileOffset("X1BTLP09.BIN", 0x3900),
                        new FileOffset("X1BTLP08.BIN", 0x3ba0),
                        new FileOffset("X1BTLP07.BIN", 0x3cec),
                        new FileOffset("X1BTLP06.BIN", 0x394c),
                        new FileOffset("X1BTLP04.BIN", 0x3460),
                        new FileOffset("X1BTLP03.BIN", 0x3e9c),
                        new FileOffset("X1BTLP02.BIN", 0x3858),
                        new FileOffset("X1BTLP01.BIN", 0x3b80),
                        new FileOffset("X007.BIN",     0x853c),
                    }
                )},

                { "CBP39.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x040f, 0x0009, 0x0fe0, 0x0029, 0x040f, 0x0032, 0x0fe0, 0x0052, 0x0430, 0x005b, 0x1055 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a80),
                        new FileOffset("X1BTLP09.BIN", 0x391c),
                        new FileOffset("X1BTLP08.BIN", 0x3bbc),
                        new FileOffset("X1BTLP07.BIN", 0x3d08),
                        new FileOffset("X1BTLP06.BIN", 0x3968),
                        new FileOffset("X1BTLP04.BIN", 0x347c),
                        new FileOffset("X1BTLP03.BIN", 0x3eb8),
                        new FileOffset("X1BTLP02.BIN", 0x3874),
                        new FileOffset("X1BTLP01.BIN", 0x3b9c),
                        new FileOffset("X007.BIN",     0x8558),
                    }
                )},

                { "CBP40.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03d4, 0x0008, 0x0f0a, 0x0027, 0x03d4, 0x002f, 0x0f0a, 0x004e, 0x03cf, 0x0056, 0x0f0b },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3a9c),
                        new FileOffset("X1BTLP09.BIN", 0x3938),
                        new FileOffset("X1BTLP08.BIN", 0x3bd8),
                        new FileOffset("X1BTLP07.BIN", 0x3d24),
                        new FileOffset("X1BTLP06.BIN", 0x3984),
                        new FileOffset("X1BTLP04.BIN", 0x3498),
                        new FileOffset("X1BTLP03.BIN", 0x3ed4),
                        new FileOffset("X1BTLP02.BIN", 0x3890),
                        new FileOffset("X1BTLP01.BIN", 0x3bb8),
                        new FileOffset("X007.BIN",     0x8574),
                    }
                )},

                { "CBP41.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0381, 0x0008, 0x090b, 0x001b, 0x0381, 0x0023, 0x090b, 0x0036, 0x0381, 0x003e, 0x090b },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3ab8),
                        new FileOffset("X1BTLP09.BIN", 0x3954),
                        new FileOffset("X1BTLP08.BIN", 0x3bf4),
                        new FileOffset("X1BTLP07.BIN", 0x3d40),
                        new FileOffset("X1BTLP06.BIN", 0x39a0),
                        new FileOffset("X1BTLP04.BIN", 0x34b4),
                        new FileOffset("X1BTLP03.BIN", 0x3ef0),
                        new FileOffset("X1BTLP02.BIN", 0x38ac),
                        new FileOffset("X1BTLP01.BIN", 0x3bd4),
                        new FileOffset("X007.BIN",     0x8590),
                    }
                )},

                { "CBP42.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x0f51, 0x0028, 0x0403, 0x0031, 0x0f51, 0x0050, 0x03df, 0x0058, 0x0f4c },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3ad4),
                        new FileOffset("X1BTLP09.BIN", 0x3970),
                        new FileOffset("X1BTLP08.BIN", 0x3c10),
                        new FileOffset("X1BTLP07.BIN", 0x3d5c),
                        new FileOffset("X1BTLP06.BIN", 0x39bc),
                        new FileOffset("X1BTLP04.BIN", 0x34d0),
                        new FileOffset("X1BTLP03.BIN", 0x3f0c),
                        new FileOffset("X1BTLP02.BIN", 0x38c8),
                        new FileOffset("X1BTLP01.BIN", 0x3bf0),
                        new FileOffset("X007.BIN",     0x85ac),
                    }
                )},

                { "CBP43.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0439, 0x0009, 0x115a, 0x002c, 0x0439, 0x0035, 0x115a, 0x0058, 0x042a, 0x0061, 0x111c },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3af0),
                        new FileOffset("X1BTLP09.BIN", 0x398c),
                        new FileOffset("X1BTLP08.BIN", 0x3c2c),
                        new FileOffset("X1BTLP07.BIN", 0x3d78),
                        new FileOffset("X1BTLP06.BIN", 0x39d8),
                        new FileOffset("X1BTLP04.BIN", 0x34ec),
                        new FileOffset("X1BTLP03.BIN", 0x3f28),
                        new FileOffset("X1BTLP02.BIN", 0x38e4),
                        new FileOffset("X1BTLP01.BIN", 0x3c0c),
                        new FileOffset("X007.BIN",     0x85c8),
                    }
                )},

                { "CBP44.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03e2, 0x0008, 0x0f48, 0x0027, 0x03e2, 0x002f, 0x0f48, 0x004e, 0x03eb, 0x0056, 0x09b7 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b0c),
                        new FileOffset("X1BTLP09.BIN", 0x39a8),
                        new FileOffset("X1BTLP08.BIN", 0x3c48),
                        new FileOffset("X1BTLP07.BIN", 0x3d94),
                        new FileOffset("X1BTLP06.BIN", 0x39f4),
                        new FileOffset("X1BTLP04.BIN", 0x3508),
                        new FileOffset("X1BTLP03.BIN", 0x3f44),
                        new FileOffset("X1BTLP02.BIN", 0x3900),
                        new FileOffset("X1BTLP01.BIN", 0x3c28),
                        new FileOffset("X007.BIN",     0x85e4),
                    }
                )},

                { "CBP45.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0448, 0x0009, 0x116a, 0x002c, 0x0448, 0x0035, 0x116a, 0x0058, 0x045d, 0x0061, 0x11a9 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b28),
                        new FileOffset("X1BTLP09.BIN", 0x39c4),
                        new FileOffset("X1BTLP08.BIN", 0x3c64),
                        new FileOffset("X1BTLP07.BIN", 0x3db0),
                        new FileOffset("X1BTLP06.BIN", 0x3a10),
                        new FileOffset("X1BTLP04.BIN", 0x3524),
                        new FileOffset("X1BTLP03.BIN", 0x3f60),
                        new FileOffset("X1BTLP02.BIN", 0x391c),
                        new FileOffset("X1BTLP01.BIN", 0x3c44),
                        new FileOffset("X007.BIN",     0x8600),
                    }
                )},

                { "CBP46.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c9, 0x0008, 0x0c43, 0x0021, 0x03c9, 0x0029, 0x0c43, 0x0042, 0x0384, 0x004a, 0x0b7c },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b44),
                        new FileOffset("X1BTLP09.BIN", 0x39e0),
                        new FileOffset("X1BTLP08.BIN", 0x3c80),
                        new FileOffset("X1BTLP07.BIN", 0x3dcc),
                        new FileOffset("X1BTLP06.BIN", 0x3a2c),
                        new FileOffset("X1BTLP04.BIN", 0x3540),
                        new FileOffset("X1BTLP03.BIN", 0x3f7c),
                        new FileOffset("X1BTLP02.BIN", 0x3938),
                        new FileOffset("X1BTLP01.BIN", 0x3c60),
                        new FileOffset("X007.BIN",     0x861c),
                    }
                )},

                { "CBP47.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03fb, 0x0008, 0x09d1, 0x001c, 0x03fb, 0x0024, 0x09d1, 0x0038, 0x0453, 0x0041, 0x0ac2 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b60),
                        new FileOffset("X1BTLP09.BIN", 0x39fc),
                        new FileOffset("X1BTLP08.BIN", 0x3c9c),
                        new FileOffset("X1BTLP07.BIN", 0x3de8),
                        new FileOffset("X1BTLP06.BIN", 0x3a48),
                        new FileOffset("X1BTLP04.BIN", 0x355c),
                        new FileOffset("X1BTLP03.BIN", 0x3f98),
                        new FileOffset("X1BTLP02.BIN", 0x3954),
                        new FileOffset("X1BTLP01.BIN", 0x3c7c),
                        new FileOffset("X007.BIN",     0x8638),
                    }
                )},

                { "CBP48.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f6, 0x0008, 0x1008, 0x0029, 0x03f6, 0x0031, 0x1008, 0x0052, 0x0406, 0x005b, 0x1032 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b7c),
                        new FileOffset("X1BTLP09.BIN", 0x3a18),
                        new FileOffset("X1BTLP08.BIN", 0x3cb8),
                        new FileOffset("X1BTLP07.BIN", 0x3e04),
                        new FileOffset("X1BTLP06.BIN", 0x3a64),
                        new FileOffset("X1BTLP04.BIN", 0x3578),
                        new FileOffset("X1BTLP03.BIN", 0x3fb4),
                        new FileOffset("X1BTLP02.BIN", 0x3970),
                        new FileOffset("X1BTLP01.BIN", 0x3c98),
                        new FileOffset("X007.BIN",     0x8654),
                    }
                )},

                { "CBP49.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x048a, 0x000a, 0x0afb, 0x0020, 0x048a, 0x002a, 0x0afb, 0x0040, 0x0483, 0x004a, 0x0aea },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3b98),
                        new FileOffset("X1BTLP09.BIN", 0x3a34),
                        new FileOffset("X1BTLP08.BIN", 0x3cd4),
                        new FileOffset("X1BTLP07.BIN", 0x3e20),
                        new FileOffset("X1BTLP06.BIN", 0x3a80),
                        new FileOffset("X1BTLP04.BIN", 0x3594),
                        new FileOffset("X1BTLP03.BIN", 0x3fd0),
                        new FileOffset("X1BTLP02.BIN", 0x398c),
                        new FileOffset("X1BTLP01.BIN", 0x3cb4),
                        new FileOffset("X007.BIN",     0x8670),
                    }
                )},

                { "CBP50.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03cd, 0x0008, 0x0fca, 0x0028, 0x03cd, 0x0030, 0x0fca, 0x0050, 0x03b3, 0x0058, 0x0f5d },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3bb4),
                        new FileOffset("X1BTLP09.BIN", 0x3a50),
                        new FileOffset("X1BTLP08.BIN", 0x3cf0),
                        new FileOffset("X1BTLP07.BIN", 0x3e3c),
                        new FileOffset("X1BTLP06.BIN", 0x3a9c),
                        new FileOffset("X1BTLP04.BIN", 0x35b0),
                        new FileOffset("X1BTLP03.BIN", 0x3fec),
                        new FileOffset("X1BTLP02.BIN", 0x39a8),
                        new FileOffset("X1BTLP01.BIN", 0x3cd0),
                        new FileOffset("X007.BIN",     0x868c),
                    }
                )},

                { "CBP51.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0605, 0x000d, 0x1728, 0x003c, 0x0605, 0x0049, 0x1728, 0x0078, 0x05c3, 0x0084, 0x163c },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3bd0),
                        new FileOffset("X1BTLP09.BIN", 0x3a6c),
                        new FileOffset("X1BTLP08.BIN", 0x3d0c),
                        new FileOffset("X1BTLP07.BIN", 0x3e58),
                        new FileOffset("X1BTLP06.BIN", 0x3ab8),
                        new FileOffset("X1BTLP04.BIN", 0x35cc),
                        new FileOffset("X1BTLP03.BIN", 0x4008),
                        new FileOffset("X1BTLP02.BIN", 0x39c4),
                        new FileOffset("X1BTLP01.BIN", 0x3cec),
                        new FileOffset("X007.BIN",     0x86a8),
                    }
                )},

                { "CBP52.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0420, 0x0009, 0x0c42, 0x0022, 0x0420, 0x002b, 0x0c42, 0x0044, 0x0403, 0x004d, 0x0bef },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3bec),
                        new FileOffset("X1BTLP09.BIN", 0x3a88),
                        new FileOffset("X1BTLP08.BIN", 0x3d28),
                        new FileOffset("X1BTLP07.BIN", 0x3e74),
                        new FileOffset("X1BTLP06.BIN", 0x3ad4),
                        new FileOffset("X1BTLP04.BIN", 0x35e8),
                        new FileOffset("X1BTLP03.BIN", 0x4024),
                        new FileOffset("X1BTLP02.BIN", 0x39e0),
                        new FileOffset("X1BTLP01.BIN", 0x3d08),
                        new FileOffset("X007.BIN",     0x86c4),
                    }
                )},

                { "CBP53.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0454, 0x0009, 0x1094, 0x002b, 0x0454, 0x0034, 0x1094, 0x0056, 0x0417, 0x005f, 0x0fac },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c08),
                        new FileOffset("X1BTLP09.BIN", 0x3aa4),
                        new FileOffset("X1BTLP08.BIN", 0x3d44),
                        new FileOffset("X1BTLP07.BIN", 0x3e90),
                        new FileOffset("X1BTLP06.BIN", 0x3af0),
                        new FileOffset("X1BTLP04.BIN", 0x3604),
                        new FileOffset("X1BTLP03.BIN", 0x4040),
                        new FileOffset("X1BTLP02.BIN", 0x39fc),
                        new FileOffset("X1BTLP01.BIN", 0x3d24),
                        new FileOffset("X007.BIN",     0x86e0),
                    }
                )},

                { "CBP54.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03f0, 0x0008, 0x0f47, 0x0027, 0x03f0, 0x002f, 0x0f47, 0x004e, 0x03dd, 0x0056, 0x0ed4 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c24),
                        new FileOffset("X1BTLP09.BIN", 0x3ac0),
                        new FileOffset("X1BTLP08.BIN", 0x3d60),
                        new FileOffset("X1BTLP07.BIN", 0x3eac),
                        new FileOffset("X1BTLP06.BIN", 0x3b0c),
                        new FileOffset("X1BTLP04.BIN", 0x3620),
                        new FileOffset("X1BTLP03.BIN", 0x405c),
                        new FileOffset("X1BTLP02.BIN", 0x3a18),
                        new FileOffset("X1BTLP01.BIN", 0x3d40),
                        new FileOffset("X007.BIN",     0x86fc),
                    }
                )},

                { "CBP55.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x06f4, 0x000e, 0x10d0, 0x0030, 0x06f4, 0x003e, 0x10d0, 0x0060, 0x06e6, 0x006e, 0x10b2 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c40),
                        new FileOffset("X1BTLP09.BIN", 0x3adc),
                        new FileOffset("X1BTLP08.BIN", 0x3d7c),
                        new FileOffset("X1BTLP07.BIN", 0x3ec8),
                        new FileOffset("X1BTLP06.BIN", 0x3b28),
                        new FileOffset("X1BTLP04.BIN", 0x363c),
                        new FileOffset("X1BTLP03.BIN", 0x4078),
                        new FileOffset("X1BTLP02.BIN", 0x3a34),
                        new FileOffset("X1BTLP01.BIN", 0x3d5c),
                        new FileOffset("X007.BIN",     0x8718),
                    }
                )},

                { "CBP56.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x04be, 0x000a, 0x0fea, 0x002a, 0x04be, 0x0034, 0x0fea, 0x0054, 0x04c3, 0x005e, 0x1014 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c5c),
                        new FileOffset("X1BTLP09.BIN", 0x3af8),
                        new FileOffset("X1BTLP08.BIN", 0x3d98),
                        new FileOffset("X1BTLP07.BIN", 0x3ee4),
                        new FileOffset("X1BTLP06.BIN", 0x3b44),
                        new FileOffset("X1BTLP04.BIN", 0x3658),
                        new FileOffset("X1BTLP03.BIN", 0x4094),
                        new FileOffset("X1BTLP02.BIN", 0x3a50),
                        new FileOffset("X1BTLP01.BIN", 0x3d78),
                        new FileOffset("X007.BIN",     0x8734),
                    }
                )},

                { "CBP57.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0598, 0x000c, 0x13db, 0x0034, 0x0598, 0x0040, 0x13db, 0x0068, 0x05c3, 0x0074, 0x1443 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c78),
                        new FileOffset("X1BTLP09.BIN", 0x3b14),
                        new FileOffset("X1BTLP08.BIN", 0x3db4),
                        new FileOffset("X1BTLP07.BIN", 0x3f00),
                        new FileOffset("X1BTLP06.BIN", 0x3b60),
                        new FileOffset("X1BTLP04.BIN", 0x3674),
                        new FileOffset("X1BTLP03.BIN", 0x40b0),
                        new FileOffset("X1BTLP02.BIN", 0x3a6c),
                        new FileOffset("X1BTLP01.BIN", 0x3d94),
                        new FileOffset("X007.BIN",     0x8750),
                    }
                )},

                { "CBP58.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x033f, 0x0007, 0x0ded, 0x0023, 0x033f, 0x002a, 0x0ded, 0x0046, 0x035f, 0x004d, 0x0e66 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3c94),
                        new FileOffset("X1BTLP09.BIN", 0x3b30),
                        new FileOffset("X1BTLP08.BIN", 0x3dd0),
                        new FileOffset("X1BTLP07.BIN", 0x3f1c),
                        new FileOffset("X1BTLP06.BIN", 0x3b7c),
                        new FileOffset("X1BTLP04.BIN", 0x3690),
                        new FileOffset("X1BTLP03.BIN", 0x40cc),
                        new FileOffset("X1BTLP02.BIN", 0x3a88),
                        new FileOffset("X1BTLP01.BIN", 0x3db0),
                        new FileOffset("X007.BIN",     0x876c),
                    }
                )},

                { "CBP59.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0434, 0x0009, 0x0878, 0x001a, 0x0434, 0x0023, 0x0878, 0x0034, 0x0434, 0x003d, 0x0878 },
                    new FileOffset[] {
                        new FileOffset("X1BTLP10.BIN", 0x3cb0),
                        new FileOffset("X1BTLP09.BIN", 0x3b4c),
                        new FileOffset("X1BTLP08.BIN", 0x3dec),
                        new FileOffset("X1BTLP07.BIN", 0x3f38),
                        new FileOffset("X1BTLP06.BIN", 0x3b98),
                        new FileOffset("X1BTLP04.BIN", 0x36ac),
                        new FileOffset("X1BTLP03.BIN", 0x40e8),
                        new FileOffset("X1BTLP02.BIN", 0x3aa4),
                        new FileOffset("X1BTLP01.BIN", 0x3dcc),
                        new FileOffset("X007.BIN",     0x8788),
                    }
                )},

                { "CBW00.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x03c6, 0x0008, 0x1516, 0x0033, 0x0415, 0x003c, 0x170f, 0x006b, 0x039a, 0x0073, 0x1433 },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6c00) }
                )},

                { "CBW07.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x038c, 0x0008, 0x13ef, 0x0030, 0x03ab, 0x0038, 0x1483, 0x0062, 0x05d3, 0x006e, 0x153b },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6c38) }
                )},

                { "CBW20.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0403, 0x0009, 0x161d, 0x0036, 0x0403, 0x003f, 0x1795, 0x006f, 0x06a8, 0x007d, 0x15e6 },
                    new FileOffset[] { new FileOffset("X008.BIN", 0x6c1c) }
                )},

                { "ENEMY.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x0a3a, 0x0016, 0x0b42, 0x002d, 0x0b81, 0x0045, 0x0740, 0x0054, 0x0704, 0x0063, 0x0d03, 0x007e, 0x08ae, 0x0090, 0x066e, 0x009d, 0x0739, 0x00ac, 0x0846, 0x00bd, 0x092b, 0x00d0, 0x095a, 0x00e3, 0x09af, 0x00f7, 0x0993, 0x010b, 0x077e, 0x011a, 0x0ae8, 0x0130, 0x063f, 0x013d, 0x0855, 0x014e, 0x0a9c, 0x0164, 0x0e6c, 0x0181, 0x08f0, 0x0193, 0x087c, 0x01a4, 0x0820, 0x01b5, 0x09af, 0x01c9, 0x0806, 0x01da, 0x06b2, 0x01e8, 0x0ab1, 0x01fe, 0x0c48, 0x0217, 0x0e81, 0x0235, 0x0b66, 0x024c, 0x0c43, 0x0265, 0x0bb3, 0x027d, 0x09bc, 0x0291, 0x08ea, 0x02a3, 0x0765, 0x02b2, 0x06ed, 0x02c0, 0x0266, 0x02c5, 0x0509, 0x02d0, 0x063b, 0x02dd, 0x0706, 0x02ec, 0x06eb, 0x02fa, 0x0a7e, 0x030f, 0x0974, 0x0322, 0x0617, 0x032f, 0x04d3, 0x0339, 0x0985, 0x034d, 0x05c6, 0x0359, 0x0685, 0x0367, 0x0a1f, 0x037c, 0x09b2, 0x0390, 0x0a80, 0x03a5, 0x0a72, 0x03ba, 0x0af9, 0x03d0, 0x09f3, 0x03e4, 0x07ef, 0x03f4, 0x0836, 0x0405, 0x0755, 0x0414, 0x0a8f, 0x042a, 0x09a6, 0x043e, 0x0833, 0x044f, 0x076c, 0x045e, 0x07b2, 0x046e, 0x058f, 0x047a, 0x09e6, 0x048e, 0x0ac2, 0x04a4, 0x09cf, 0x04b8, 0x0abc, 0x04ce, 0x08bc, 0x04e0, 0x08f8, 0x04f2, 0x0936, 0x0505, 0x089d, 0x0517, 0x02db, 0x051d, 0x0672, 0x052a, 0x04f1, 0x0534, 0x06c3, 0x0542, 0x07ed, 0x0552, 0x0826, 0x0563, 0x07fa, 0x0573, 0x0787, 0x0583, 0x09ff, 0x0597, 0x09fc, 0x05ab, 0x0aa7, 0x05c1, 0x0a90, 0x05d7, 0x0adc, 0x05ed, 0x05ed, 0x05f9, 0x000a, 0x05fa, 0x0002, 0x05fb, 0x0712, 0x060a, 0x0b77, 0x0621, 0x0a49, 0x0636, 0x0002, 0x0637, 0x0971, 0x064a, 0x0891, 0x065c, 0x0991, 0x0670, 0x0002, 0x0671, 0x0960, 0x0684, 0x0002, 0x0685, 0x0c7c, 0x069e, 0x0928, 0x06b1, 0x0748, 0x06c0, 0x07c6, 0x06d0, 0x0a94, 0x06e6, 0x0724, 0x06f5, 0x0640, 0x0702, 0x06d4, 0x0710, 0x0002, 0x0711, 0x075e, 0x0720, 0x093a, 0x0733, 0x114b, 0x0756, 0x0843, 0x0767, 0x0c00, 0x077f, 0x06a0, 0x078d, 0x061c, 0x079a, 0x08f5, 0x07ac, 0x096a, 0x07bf, 0x0a99, 0x07d5, 0x0a8c, 0x07eb, 0x0a5c, 0x0800, 0x0891, 0x0812, 0x0873, 0x0823, 0x086c, 0x0834, 0x0002, 0x0835, 0x088a, 0x0847, 0x0347, 0x084e, 0x0002, 0x084f, 0x1023, 0x0870, 0x0e81, 0x088e, 0x089c, 0x08a0, 0x083e, 0x08b1, 0x05c4, 0x08bd, 0x0abc, 0x08d3, 0x080d, 0x08e4, 0x0902, 0x08f7, 0x0696, 0x0905, 0x0672, 0x0912, 0x04f1, 0x091c, 0x0002, 0x091d, 0x0725, 0x092c, 0x0002, 0x092d, 0x0749, 0x093c, 0x0827, 0x094d, 0x0866, 0x095e, 0x0808, 0x096f, 0x0878, 0x0980, 0x0002, 0x0981, 0x0002, 0x0982, 0x0a44, 0x0997, 0x09db, 0x09ab, 0x0a9f, 0x09c1, 0x05b1, 0x09cd, 0x0770, 0x09dc, 0x09bf, 0x09f0, 0x09a5, 0x0a04, 0x0913, 0x0a17, 0x08da, 0x0a29, 0x0a62, 0x0a3e, 0x094a, 0x0a51, 0x0002, 0x0a52, 0x0a32, 0x0a67, 0x093e, 0x0a7a, 0x0a55, 0x0a8f, 0x03f8, 0x0a97, 0x09af, 0x0aab, 0x0002, 0x0aac, 0x0002, 0x0aad, 0x08c8, 0x0abf, 0x0a6a, 0x0ad4, 0x0796, 0x0ae4, 0x0886, 0x0af6, 0x0b6d, 0x0b0d, 0x0b50, 0x0b24, 0x0993, 0x0b38, 0x0922, 0x0b4b, 0x0927, 0x0b5e, 0x0a1c, 0x0b73, 0x09e0, 0x0b87, 0x0877, 0x0b98, 0x0e27, 0x0bb5, 0x08c3, 0x0bc7, 0x0994, 0x0bdb, 0x0002, 0x0bdc, 0x1076, 0x0bfd, 0x0002, 0x0bfe, 0x094a, 0x0c11, 0x094a, 0x0c24, 0x0731, 0x0c33, 0x0072, 0x0c34, 0x0002, 0x0c35, 0x07eb, 0x0c45, 0x0002, 0x0c46, 0x0002 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x87a0) }
                )},

                { "ENEMYS.CHP", new DataWithFileOffsets(
                    new ushort[] { 0x0000, 0x0002, 0x0001, 0x03b9, 0x0009, 0x043e, 0x0012, 0x0459, 0x001b, 0x0324, 0x0022, 0x0308, 0x0029, 0x0d03, 0x0044, 0x04fc, 0x004e, 0x066b, 0x005b, 0x02b7, 0x0061, 0x030c, 0x0068, 0x0369, 0x006f, 0x039a, 0x0077, 0x03a0, 0x007f, 0x0393, 0x0087, 0x0342, 0x008e, 0x0425, 0x0097, 0x025d, 0x009c, 0x0330, 0x00a3, 0x0405, 0x00ac, 0x03b5, 0x00b4, 0x034f, 0x00bb, 0x033c, 0x00c2, 0x02f2, 0x00c8, 0x03bb, 0x00d0, 0x035d, 0x00d7, 0x02d9, 0x00dd, 0x0410, 0x00e6, 0x073c, 0x00f5, 0x03a0, 0x00fd, 0x044d, 0x0106, 0x049f, 0x0110, 0x04f8, 0x011a, 0x03be, 0x0122, 0x0360, 0x0129, 0x0347, 0x0130, 0x06eb, 0x013e, 0x0264, 0x0143, 0x0507, 0x014e, 0x0638, 0x015b, 0x0703, 0x016a, 0x0425, 0x0173, 0x03f4, 0x017b, 0x0381, 0x0183, 0x03a2, 0x018b, 0x04d1, 0x0195, 0x035e, 0x019c, 0x060a, 0x01a9, 0x03ec, 0x01b1, 0x03cd, 0x01b9, 0x039d, 0x01c1, 0x03ed, 0x01c9, 0x047e, 0x01d2, 0x0af8, 0x01e8, 0x03d0, 0x01f0, 0x02fd, 0x01f6, 0x0381, 0x01fe, 0x031f, 0x0205, 0x03fa, 0x020d, 0x03d0, 0x0215, 0x04b4, 0x021f, 0x0332, 0x0226, 0x034b, 0x022d, 0x058e, 0x0239, 0x0443, 0x0242, 0x03fa, 0x024a, 0x039f, 0x0252, 0x03fe, 0x025a, 0x03c6, 0x0262, 0x03de, 0x026a, 0x02d1, 0x0270, 0x034f, 0x0277, 0x02d9, 0x027d, 0x0670, 0x028a, 0x04ee, 0x0294, 0x0b49, 0x02ab, 0x03eb, 0x02b3, 0x0314, 0x02ba, 0x0302, 0x02c1, 0x0336, 0x02c8, 0x03d1, 0x02d0, 0x03ce, 0x02d8, 0x0418, 0x02e1, 0x040c, 0x02ea, 0x045b, 0x02f3, 0x05ea, 0x02ff, 0x000a, 0x0300, 0x0002, 0x0301, 0x0317, 0x0308, 0x0817, 0x0319, 0x03ea, 0x0321, 0x0002, 0x0322, 0x0971, 0x0335, 0x03b5, 0x033d, 0x0425, 0x0346, 0x0002, 0x0347, 0x0395, 0x034f, 0x0002, 0x0350, 0x04ae, 0x035a, 0x0379, 0x0361, 0x0326, 0x0368, 0x02f4, 0x036e, 0x048e, 0x0378, 0x0304, 0x037f, 0x02ac, 0x0385, 0x0295, 0x038b, 0x0002, 0x038c, 0x02cf, 0x0392, 0x03f8, 0x039a, 0x0a6b, 0x03af, 0x031c, 0x03b6, 0x0464, 0x03bf, 0x069d, 0x03cd, 0x03ab, 0x03d5, 0x036a, 0x03dc, 0x0368, 0x03e3, 0x03db, 0x03eb, 0x03ee, 0x03f3, 0x03db, 0x03fb, 0x03a5, 0x0403, 0x033c, 0x040a, 0x0331, 0x0411, 0x0002, 0x0412, 0x0002, 0x0413, 0x0345, 0x041a, 0x0002, 0x041b, 0x0002, 0x041c, 0x0e7a, 0x0439, 0x03ae, 0x0441, 0x0325, 0x0448, 0x0376, 0x044f, 0x04a9, 0x0459, 0x0374, 0x0460, 0x03e6, 0x0468, 0x03f6, 0x0470, 0x0670, 0x047d, 0x04ee, 0x0487, 0x0002, 0x0488, 0x013d, 0x048b, 0x0002, 0x048c, 0x0316, 0x0493, 0x04e2, 0x049d, 0x0508, 0x04a8, 0x04d0, 0x04b2, 0x031c, 0x04b9, 0x0002, 0x04ba, 0x0002, 0x04bb, 0x0523, 0x04c6, 0x03c8, 0x04ce, 0x040e, 0x04d7, 0x05af, 0x04e3, 0x02ea, 0x04e9, 0x0382, 0x04f1, 0x0423, 0x04fa, 0x03ed, 0x0502, 0x0356, 0x0509, 0x04be, 0x0513, 0x0370, 0x051a, 0x0002, 0x051b, 0x03e9, 0x0523, 0x038d, 0x052b, 0x03f4, 0x0533, 0x03f6, 0x053b, 0x03a8, 0x0543, 0x0002, 0x0544, 0x0670, 0x0551, 0x04ee, 0x055b, 0x0a6a, 0x0570, 0x0794, 0x0580, 0x032c, 0x0587, 0x044e, 0x0590, 0x04f3, 0x059a, 0x038c, 0x05a2, 0x037f, 0x05a9, 0x035e, 0x05b0, 0x03c3, 0x05b8, 0x03b4, 0x05c0, 0x032b, 0x05c7, 0x0348, 0x05ce, 0x033b, 0x05d5, 0x0380, 0x05dc, 0x0002, 0x05dd, 0x0648, 0x05ea, 0x0002, 0x05eb, 0x0370, 0x05f2, 0x0370, 0x05f9, 0x031a, 0x0600, 0x0072, 0x0601, 0x0002, 0x0602, 0x030b, 0x0609, 0x0002, 0x060a, 0x0002 },
                    new FileOffset[] { new FileOffset("X007.BIN", 0x8aa0) }
                )},
            }}
        };
    }
}
