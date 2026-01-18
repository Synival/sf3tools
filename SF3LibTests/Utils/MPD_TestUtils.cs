using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Utils {
    public static class MPD_TestUtils {
        public static MPD_File MakeMPD_File(ScenarioType scenario, string filename) {
            var filePath = TestDataPaths.ResourcePath(scenario, Path.GetFileName(filename))!;
            var fileData = File.ReadAllBytes(filePath);
            return MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(fileData)), new NameGetterContext(scenario), scenario);
        }

        public static IMPD_File RecreateMPD_File(IMPD_File mpd) {
            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(mpd);
                outputData = memoryStream.ToArray();
            }

            return MPD_File.Create(
                new SF3.ByteData.ByteData(new ByteArray(outputData)),
                mpd.NameGetterContext,
                mpd.Scenario
            );
        }

        public static void ForEachMPD_File(ScenarioType scenario, Action<IMPD_File> action) {
            var path = TestDataPaths.ScenarioDataPaths[scenario];
            var mpdFiles = Directory
                .GetFiles(path, "*.MPD")
                .Select(x => Path.GetFileName(x))
                .Where(x => x != "SHIP2.MPD")
                .ToArray();
            var testCases = mpdFiles
                .Select(x => new SF3FileTestCase(scenario, x))
                .ToArray();

            SF3FileTestCase.Run(testCases, testCase => {
                var mpd = MakeMPD_File(scenario, testCase.Filename);
                action(mpd);
            });
        }
    }
}
