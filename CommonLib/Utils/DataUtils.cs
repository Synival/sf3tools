namespace CommonLib.Utils {
    public static class DataUtils {
        public static unsafe int IndexOfSubset(this byte[] src, byte[] subset, int startPos = 0) {
            // Get some easy failure cases out of the way.
            if (src == null || subset == null || src.Length == 0 || subset.Length == 0 || subset.Length - startPos > src.Length || startPos >= src.Length || startPos < 0)
                return -1;

            var searchLen = src.Length - subset.Length + 1;

            fixed (byte* srcPtrStart = &src[0])
            fixed (byte* subsetPtrStart = &subset[0]) {
                byte* srcPtr = srcPtrStart + startPos;

                for (int i = startPos; i < searchLen; i++) {
                    if (*(srcPtr++) != *(subsetPtrStart))
                        continue;

                    byte* srcSearchPtr = srcPtr;
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
    }
}