using System;

namespace CommonLib.Utils {
#pragma warning disable IDE1006 // Naming Styles
    public static class MemoryUtils {
        public static byte[] MemCpy(byte[] dst, byte[] src, int count) {
            src.AsSpan(0, count).CopyTo(dst.AsSpan(0, count));
            return dst;
        }

        public static unsafe IntPtr MemCpyUnsafe(IntPtr dst, IntPtr src, int count) {
            Buffer.MemoryCopy(src.ToPointer(), dst.ToPointer(), count, count);
            return dst;
        }

        public static int MemCmp(byte[] lhs, byte[] rhs, int count)
            => lhs.AsSpan(0, count).SequenceCompareTo(rhs.AsSpan(0, count));

        public static unsafe int memcmp(IntPtr lhs, IntPtr rhs, long count) {
            var sLhs = new ReadOnlySpan<byte>(lhs.ToPointer(), (int) count);
            var sRhs = new ReadOnlySpan<byte>(rhs.ToPointer(), (int) count);
            return sLhs.SequenceCompareTo(sRhs);
        }

        public static bool MemEqual(byte[] lhs, byte[] rhs)
            => lhs.AsSpan().SequenceEqual(rhs);
    }
#pragma warning restore IDE1006 // Naming Styles
}
