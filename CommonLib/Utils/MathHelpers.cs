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
    }
}
