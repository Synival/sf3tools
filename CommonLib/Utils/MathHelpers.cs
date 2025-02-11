using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Utils {
    public static class MathHelpers {
        /// <summary>
        /// Why do I have to keep re-writing this code???
        /// </summary>
        public static int ActualMod(int num, int divisor) {
            var remainder = num % divisor;
            return remainder < 0 ? remainder + divisor : remainder;
        }

        /// <summary>
        /// Why do I have to keep re-writing this code???
        /// </summary>
        public static float ActualMod(float num, float divisor) {
            var remainder = num % divisor;
            return remainder < 0 ? remainder + divisor : remainder;
        }

        public static byte   Clamp(byte   value, byte   min, byte   max) => Math.Min(max, Math.Max(min, value));
        public static sbyte  Clamp(sbyte  value, sbyte  min, sbyte  max) => Math.Min(max, Math.Max(min, value));
        public static ushort Clamp(ushort value, ushort min, ushort max) => Math.Min(max, Math.Max(min, value));
        public static short  Clamp(short  value, short  min, short  max) => Math.Min(max, Math.Max(min, value));
        public static uint   Clamp(uint   value, uint   min, uint   max) => Math.Min(max, Math.Max(min, value));
        public static int    Clamp(int    value, int    min, int    max) => Math.Min(max, Math.Max(min, value));
        public static float  Clamp(float  value, float  min, float  max) => Math.Min(max, Math.Max(min, value));
        public static double Clamp(double value, double min, double max) => Math.Min(max, Math.Max(min, value));
    }
}
