using System.Collections.Generic;

namespace CommonLib.Utils {
    public static class DataUtils {
        public static int[] IndicesOfSubset(this byte[] src, byte[] subset, int startPos = 0, int alignment = 1) {
            var indices = new List<int>();
            while (true) {
                var index = IndexOfSubset(src, subset, startPos, alignment);
                if (index < 0)
                    break;
                startPos = index + alignment;
                indices.Add(index);
            }
            return indices.ToArray();
        }

        public static unsafe int IndexOfSubset(this byte[] src, byte[] subset, int startPos = 0, int alignment = 1) {
            // Check for invalid values.
            if (src == null || subset == null || startPos < 0 || alignment < 1)
                return -1;

            // Make sure 'startPos' is in increments of 'alignment'.
            if (startPos % alignment != 0)
                startPos = ((startPos / alignment) + 1) * alignment;

            // Make sure we're not trying to search beyond the length of 'src'.
            if (startPos >= src.Length)
                return -1;

            // Searching for 0 bytes is easy, it's right there!
            if (subset.Length == 0)
                return startPos;

            // Bail early if what we're searching for is too large for the search area.
            var searchLen = src.Length - subset.Length + 1;
            if (startPos >= searchLen)
                return -1;

            fixed (byte* srcPtrStart = &src[0])
            fixed (byte* subsetPtrStart = &subset[0]) {
                byte* srcPtr = srcPtrStart + startPos;

                for (int i = startPos; i < searchLen; i += alignment) {
                    var equal = *srcPtr == *subsetPtrStart;
                    srcPtr += alignment;
                    if (!equal)
                        continue;

                    byte* srcSearchPtr = srcPtr - alignment + 1;
                    byte* subsetSearchPtr = subsetPtrStart + 1;

                    int j = 1;
                    while (j < subset.Length) {
                        if (*(srcSearchPtr++) != *(subsetSearchPtr++))
                            break;
                        j++;
                    }

                    if (j == subset.Length)
                        return i;
                }
            }
            return -1;
        }

        public static unsafe byte[] ToByteArray(this ushort src)
            => ToByteArray(new ushort[] { src });

        public static unsafe byte[] ToByteArray(this int src)
            => ToByteArray(new uint[] { (uint) src });

        public static unsafe byte[] ToByteArray(this uint src)
            => ToByteArray(new uint[] { src });

        public static unsafe byte[] ToByteArray(this ushort[] src) {
            var output = new byte[src.Length * 2];
            fixed (ushort* srcPtrStart = &src[0])
            fixed (byte* outputPtrStart = &output[0]) {
                ushort* srcPtr = srcPtrStart;
                byte* outputPtr = outputPtrStart;

                for (int i = 0; i < src.Length; i++) {
                    ushort srcValue = *(srcPtr++);
                    *(outputPtr++) = (byte) (srcValue >> 8);
                    *(outputPtr++) = (byte) (srcValue);
                }
            }

            return output;
        }

        public static unsafe byte[] ToByteArray(this uint[] src) {
            var output = new byte[src.Length * 4];
            fixed (uint* srcPtrStart = &src[0])
            fixed (byte* outputPtrStart = &output[0]) {
                uint* srcPtr = srcPtrStart;
                byte* outputPtr = outputPtrStart;

                for (int i = 0; i < src.Length; i++) {
                    uint srcValue = *(srcPtr++);
                    *(outputPtr++) = (byte) (srcValue >> 24);
                    *(outputPtr++) = (byte) (srcValue >> 16);
                    *(outputPtr++) = (byte) (srcValue >> 8);
                    *(outputPtr++) = (byte) (srcValue);
                }
            }

            return output;
        }

        public static ushort GetUInt16(this byte[] data, int offset) {
            return (ushort) ((data[offset + 0] <<  8) |
                             (data[offset + 1] <<  0));
        }

        public static uint GetUInt32(this byte[] data, int offset) {
            return (uint) ((data[offset + 0] << 24) |
                           (data[offset + 1] << 16) |
                           (data[offset + 2] <<  8) |
                           (data[offset + 3] <<  0));
        }
    }
}