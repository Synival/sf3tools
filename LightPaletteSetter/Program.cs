using CommonLib.Arrays;
using CommonLib.Utils;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace LightPaletteSetter {
    public class Program {
        public class Lighting {
            public Lighting(ushort[] palette, ushort? pitch, ushort? yaw) {
                Palette = (ushort[]) palette.Clone();
                Pitch   = pitch;
                Yaw     = yaw;
            }

            public readonly ushort[] Palette;
            public readonly ushort? Pitch;
            public readonly ushort? Yaw;
        }

        public static readonly Lighting c_quonusLighting = new Lighting(
            [
                0x20C3, 0x524F, 0x24E4, 0x24E4,
                0x2905, 0x5105, 0x524F, 0x524F,
                0x524F, 0x524F, 0x4E2E, 0x4E2E,
                0x4E2E, 0x3989, 0x3DAA, 0x3DAA,
                0x41CB, 0x41CB, 0x45EC, 0x45EC,
                0x4A0D, 0x4A0D, 0x4E2E, 0x4E2E,
                0x524F, 0x4189, 0x5670, 0x5670,
                0x5A91, 0x5A91, 0x524F, 0x3D69,
            ], null, null
        );

        public static readonly Lighting c_superDarkQuonusLighting = new Lighting(
            [
                0x0C41, 0x20C5, 0x1041, 0x1041,
                0x1462, 0x2862, 0x2D06, 0x2D07,
                0x3127, 0x3127, 0x3127, 0x3127,
                0x3528, 0x28E5, 0x2D06, 0x2D06,
                0x3127, 0x3527, 0x3948, 0x3968,
                0x4189, 0x418A, 0x49AB, 0x49AB,
                0x51EC, 0x4147, 0x5A0E, 0x5A2E,
                0x624F, 0x6250, 0x5E2E, 0x4548,
            ], 0x8200, null
        );

        private const string c_pathIn = "D:/";
                                      // ^
                                      //  `-- Enter the path for all your MPD files here!

        private const string c_pathOut = "../../../Private";
                                      // ^
                                      //  `-- Updated MPD files are dumped here!

        private static readonly Lighting c_lighting = c_superDarkQuonusLighting;

        public static void Main(string[] args) {
            // Get a list of all .MPD files from 'c_pathIn'.
            var filesIn = Directory.GetFiles(c_pathIn, "*.MPD");

            // (NameGetterContext is irrelevant for this project. It's used to get named values
            //  for stuff, like character names, classes, spells, items, etc.)
            var scenario = ScenarioType.Scenario1;
            var nameGetter = new NameGetterContext(scenario);

            // For every file, assign the lightmap.
            foreach (var fileIn in filesIn) {
                Console.WriteLine(fileIn);

                // Get a raw data editing context for the file.
                var byteData = new ByteData(new ByteArray(File.ReadAllBytes(fileIn)));

                // Create an MPD file that works with our new ByteData.
                var mpdFile = MPD_File.Create(byteData, nameGetter, scenario);

                // Update light palette.
                var lightingPalette = mpdFile.LightPalette.Rows;
                for (var i = 0; i < 32; i++)
                    lightingPalette[i].ColorABGR1555 = c_lighting.Palette[i];

                // Update light direction.
                var lightDirection = mpdFile.Offset2Table.Rows[0].Value;
                if (c_lighting.Pitch.HasValue) {
                    lightDirection &= ~0xFFFF0000;
                    lightDirection |= (uint) (c_lighting.Pitch.Value << 16);
                }
                if (c_lighting.Yaw.HasValue) {
                    lightDirection &= ~(uint) 0x0000FFFF;
                    lightDirection |= c_lighting.Yaw.Value;
                }
                mpdFile.Offset2Table.Rows[0].Value = lightDirection;

                // This will compress chunks and update the chunk table header.
                _ = mpdFile.Finish();

                // Write it back out!
                var output = mpdFile.Data.GetDataCopy();
                var fileOut = Path.Combine(c_pathOut, Path.GetFileName(fileIn));
                File.WriteAllBytes(fileOut, output);
            };
        }
    }
}
