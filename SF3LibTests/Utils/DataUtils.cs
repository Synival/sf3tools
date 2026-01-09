namespace SF3.Tests.Utils {
    public static class DataUtils {
        public struct ByteComparisonSkipRegion {
            public int Offset;
            public int Size;
            public int ActualDataExtraBytes;
        }

        public static List<string> ByteComparisonErrors(byte[] expected, byte[] actual, int reportingOffset, string reportInfo, out float percentageCorrect, ByteComparisonSkipRegion[]? skipRegions = null) {
            var errors = new List<string>();

            var expectedSizeDiff = (skipRegions != null) ? -skipRegions.Sum(x => x.ActualDataExtraBytes) : 0;
            var expectedLength = expected.Length + expectedSizeDiff;

            if (expectedLength != actual.Length)
                errors.Add($"Length is wrong: should be {expectedLength} (0x{expectedLength:X5}), is {actual.Length} (0x{actual.Length:X5})");
            (uint ExpectedOffset, uint ActualOffset)? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            // Sort the skip regions.
            skipRegions = (skipRegions ?? []).OrderBy(x => x.Offset).ToArray();
            int skipRegionIndex = 0;
            var skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;

            for (int i = 0, j = 0; i < expected.Length && j < actual.Length; i++, j++) {
                if (i + reportingOffset == skipRegion?.Offset) {
                    i += skipRegion.Value.Size - 1;
                    j += skipRegion.Value.Size - 1;
                    i -= skipRegion.Value.ActualDataExtraBytes;
                    skipRegionIndex++;
                    skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;
                    continue;
                }

                if (expected[i] != actual[j]) {
                    if (!firstWrongByte.HasValue)
                        firstWrongByte = ((uint) i, (uint) j);
                    wrongBytes++;
                }
            }

            percentageCorrect = (float) Math.Floor((float) (bytesToCompare - wrongBytes) / bytesToCompare * 10000.0f) / 100.0f;
            if (wrongBytes > 0) {
                errors.Add($"Comparable data is wrong: {percentageCorrect:0.00}% accurate ({wrongBytes} wrong bytes)");

                var rightByte = expected[firstWrongByte!.Value.ExpectedOffset];
                var wrongByte = actual[firstWrongByte!.Value.ActualOffset];

                var fwbOffset = firstWrongByte!.Value.ExpectedOffset + reportingOffset;

                errors.Add($"First wrong byte is at {fwbOffset} (0x{fwbOffset:X4}):");
                errors.Add($"  Should be {rightByte} (0x{rightByte:X2}), is {wrongByte} (0x{wrongByte:X2})");
            }

            if (errors.Count > 0 && reportInfo != null && reportInfo != "")
                errors = [reportInfo + ":", ..errors.Select(x => "  " + x)];

            return errors;
        }

        public static void AssertByteComparison(byte[] expected, byte[] actual, ByteComparisonSkipRegion[]? skipRegions = null, float acceptablePercentage = 100.0f)
            => AssertByteComparison(expected, actual, 0, null, skipRegions, acceptablePercentage);

        public static void AssertByteComparison(byte[] expected, byte[] actual, int reportingOffset, string reportInfo, ByteComparisonSkipRegion[]? skipRegions = null, float acceptablePercentage = 100.0f) {
            var errors = ByteComparisonErrors(expected, actual, reportingOffset, reportInfo, out var percentageCorrect, skipRegions) ?? [];
            if (percentageCorrect >= acceptablePercentage) {
                foreach (var error in errors)
                    System.Diagnostics.Debug.WriteLine(error);
            }
            else if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
        }
    }
}
