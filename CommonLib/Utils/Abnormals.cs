using System;

namespace CommonLib.Utils {
    /// <summary>
    /// SF3 has these "normals" in the MPD surface mesh that seem to work perfectly fine with the lighting engine
    /// but make almost no sense when plotted. They're not normalized, they don't corallate how they should, and they
    /// don't correspond to any function I know of. So, I'm dubbing them "abnormals", and writing some functions to
    /// calculate them based on polynomial regression using data points I could find.
    /// </summary>
    public static class Abnormals {
        // Data used to derive all this nonsense.
        // AbnormalY/Z's were located across MPD files for ramps with corresponding Slopes / Y difference values.
        // (Angle and NormalY/Z were calculated using existing slope/X/Y/Z data)
        //
        // Slope   Angle              |  X     Y     Z  |  NormalY              NormalZ            |  AbnormalY    AbnormalZ
        // ---------------------------|-----------------|------------------------------------------|---------------------------
        // -300    -71.5650511791235  |  0    -3     1  |  0.316227766016837    0.948683298050514  |  0.8906555    1.66409302
        // -225    -66.0375110273093  |  0    -2.25  1  |  0.406138466053448    0.913811548620257  |  0.6712951    1.4947815
        // -200    -63.4349488247351  |  0    -2     1  |  0.447213595499958    0.894427190999916  |  0.585846     1.41454
        // -150    -56.3099324756297  |  0    -1.5   1  |  0.554700196225229    0.832050294337844  |  0.4000549    1.1999817
        // -100    -45.0000000012862  |  0    -1     1  |  0.707106781186547    0.707106781186548  |  0.2111511    0.8943786
        //  -75    -36.8698976468978  |  0    -0.75  1  |  0.8                  0.6                |  0.1273499    0.70224
        //  -50    -26.5650511778373  |  0    -0.5   1  |  0.894427190999916    0.447213595499958  |  0.0597229    0.4850158
        //  -25    -14.0362434683277  |  0    -0.25  1  |  0.970142500145332    0.242535625036334  |  0.0154724    0.2480469
        //    0      0                |  0     0     1  |  1                    0                  |  0.0000305    0
        //   25     14.0362434683277  |  0     0.25  1  |  0.970142500145332   -0.242535625036334  |  0.0154724    0.2480469
        //   50     26.5650511778373  |  0     0.5   1  |  0.894427190999916   -0.447213595499958  |  0.0597229    0.4850158
        //   75     36.8698976468978  |  0     0.75  1  |  0.8                 -0.6                |  0.1273499    0.70224
        //  100     45.0000000012862  |  0     1     1  |  0.707106781186547   -0.707106781186548  |  0.2111511    0.8943786
        //  150     56.3099324756297  |  0     1.5   1  |  0.554700196225229   -0.832050294337844  |  0.4000549    1.1999817
        //  200     63.4349488247351  |  0     2     1  |  0.447213595499958   -0.894427190999916  |  0.585846     1.41454
        //  225     66.0375110273093  |  0     2.25  1  |  0.406138466053448   -0.913811548620257  |  0.6712951    1.4947815
        //  300     71.5650511791235  |  0     3     1  |  0.316227766016837   -0.948683298050514  |  0.8906555    1.66409302

        /// <summary>
        /// Converts a normal Y to an abnormal Y.
        /// </summary>
        /// <param name="y">A normal Y component to convert.</param>
        /// <returns>An abnormal Y component.</returns>
        public static double NormalYToAbnormalY(double y) {
            const double c_zeroY = (float) (1.0f / 32768.0f);
            return y == 0.00
                ? (double) c_zeroY
                : 0.6631735 * Math.Pow(y, 4.0)
                - 3.0851886 * Math.Pow(y, 3.0)
                + 5.8478364 * Math.Pow(y, 2.0)
                - 5.5924815 * y
                + 2.1666886;
        }

        /// <summary>
        /// Converts a normal horizontal component to an abnormal horizontal component.
        /// </summary>
        /// <param name="xz">The length of the (X, Z) components of an input number.</param>
        /// <returns>The length of abnormal (X, Z) components.</returns>
        public static double NormalXzToAbnormalXz(double xz) {
            return xz == 0.00
                ? 0.00
                : 0.7417647 * Math.Pow(xz, 9.0)
                - 0.6465898 * Math.Pow(xz, 7.0)
                + 0.5487028 * Math.Pow(xz, 5.0)
                + 0.3190756 * Math.Pow(xz, 3.0)
                + 1.0024375 * xz;
        }

        /// <summary>
        /// Converts an abnormal horizontal component to an abnormal Y component.
        /// </summary>
        /// <param name="xz">The length of the (X, Z) components of an input number.</param>
        /// <returns>An abnormal Y component.</returns>
        public static double AbnormalXzToAbnormalY(double xz) {
            const double c_zeroY = (float) (1.0f / 32768.0f);
            return xz == 0.00
                ? (double) c_zeroY
                : 0.0016786542 * Math.Pow(xz, 8.0)
                - 0.0025180065 * Math.Pow(xz, 6.0)
                + 0.0205664590 * Math.Pow(xz, 4.0)
                + 0.2482587282 * Math.Pow(xz, 2.0)
                + 0.0001195671;
        }

        /// <summary>
        /// When calculating a vertex normal, all normals of neighboring polys are averaged.
        /// For some reason, these individual poly normals need to be weighted to get the correct "abnormal" values.
        /// This function calculates that weight.
        /// </summary>
        /// <param name="y">Input Y component of the normal to be weighted.</param>
        /// <returns>A weight value to be applied to the normal.</returns>
        public static double NormalWeightForAbnormalBlending(double y) {
            return (y == 1)
                ? 1
                : 0.5351235390 * Math.Pow(y, 2)
                - 1.8247789381 * y
                + 2.2891704106;
        }
    }
}
