using System;

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

        /// <summary>
        /// Performs linear interpolation of values 'a' and 'b' with amount 't'.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="t">Amount between values 'a' and 'b', from 0.00 (a) to 1.00 (b).</param>
        /// <returns>A linearly-interpolated value.</returns>
        public static byte Lerp(byte a, byte b, float t)
            => (byte) ((a * (1.00f - t)) + b * t);

        /// <summary>
        /// Guesses the dimensions and bytes-per-pixel of an image based on the size provided.
        /// The size is guessed to be as close to a square as possible. The guessed rectangular size is always 'Width > Height'.
        /// 16-bit estimations are preferred over 8-bit, as this is more common typically.
        /// </summary>
        /// <param name="size">The size of the data to guess the dimensions and bytes-per-pixel for.</param>
        /// <param name="canBe8Bit">Assume this can be an 8-bit image (1 byte-per-pixel).</param>
        /// <param name="canBe16Bit">Assume this can be a 16-bit image (2 bytes-per-pixel).</param>
        /// <returns>A tuple with a 'Width', 'Height', and 'BytesPerPixel'. The product of these three is always equal to the input 'size'.</returns>
        /// <exception cref="ArgumentException">Thrown when neither 'canBe8Bit' or 'canBe16Bit' is set.</exception>
        public static (int Width, int Height, int BytesPerPixel) GuessImageDimensions(int size, bool canBe8Bit, bool canBe16Bit) {
            if (!canBe8Bit && !canBe16Bit)
                throw new ArgumentException($"Either {nameof(canBe8Bit)} or {nameof(canBe16Bit)} must be set");

            if (canBe16Bit && size % 2 == 0) {
                var size2 = size / 2;
                var divisor = (int) Math.Sqrt(size2);
                while (divisor > 1) {
                    var quotient = size2 / (double) divisor;
                    if (quotient == (int) quotient)
                        return (divisor, (int) quotient, 2);
                    divisor--;
                }
            }
            else if (canBe8Bit) {
                var divisor = (int) Math.Sqrt(size);
                while (divisor > 1) {
                    var quotient = size / (double) divisor;
                    if (quotient == (int) quotient)
                        return (divisor, (int) quotient, 1);
                    divisor--;
                }
            }

            if (canBe16Bit && (size % 2 == 0 || !canBe8Bit))
                return (1, size / 2, 2);
            else
                return (1, size, 1);
        }
    }
}
