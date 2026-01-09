namespace SF3.Tests.Utils {
    public static class DataUtils {
        public struct ByteComparisonSkipRegion {
            public int Offset;
            public int Size;
            public int ActualDataExtraBytes;
        }

        public static List<string> ByteComparisonErrors(byte[] expected, byte[] actual, out float percentageCorrect, ByteComparisonSkipRegion[]? skipRegions = null) {
            var errors = new List<string>();

            if (expected.Length != actual.Length)
                errors.Add($"Length is wrong: should be {expected.Length} (0x{expected.Length:X5}), is {actual.Length} (0x{actual.Length:X5})");
            (uint ExpectedOffset, uint ActualOffset)? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            // Sort the skip regions.
            skipRegions = (skipRegions ?? []).OrderBy(x => x.Offset).ToArray();
            int skipRegionIndex = 0;
            var skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;

            for (int i = 0, j = 0; i < expected.Length && j < actual.Length; i++, j++) {
                if (i == skipRegion?.Offset) {
                    i += skipRegion.Value.Size - 1;
                    j += skipRegion.Value.Size - 1;
                    i += skipRegion.Value.ActualDataExtraBytes;
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
                errors.Add($"First wrong byte is at {firstWrongByte!.Value.ExpectedOffset} (0x{firstWrongByte!.Value.ExpectedOffset:X4}):");
                errors.Add($"  Should be {rightByte} (0x{rightByte:X2}), is {wrongByte} (0x{wrongByte:X2})");
            }

            return errors;
        }
    }
}
