using System.Linq;
using CommonLib.Imaging;
using OpenTK.Mathematics;

namespace SF3.Win.Views.X8PC {
    public static class PCModelViewConstants {
        public static float Pitch = -10.0f;

        public static Vector3 OutdoorLightDirection
            = new Vector3(-0x8000, 0xDD2B, 0x0000).Normalized();

        public static readonly Palette DaytimePalette
            = new(buildLightingPalette(0x11, 0x09, 0x1f, 0x0c).Select(PixelConversion.ABGR1555toChannels).ToArray());

        private static ushort[] buildLightingPalette(byte low, byte mid, byte high, byte midIdx) {   
            byte aboveBase; 
            byte baseVal;

            var colors = new ushort[0x20];
            for (int i = 0; i < 0x20; ++i) {
            if (i < midIdx) {
                baseVal = low;
                aboveBase = (byte) (i * (mid - low) / midIdx);
            }
            else {
                baseVal = mid;
                aboveBase = (byte) ((i - midIdx) * (high - mid) / (0x1f - midIdx));
            }
    
            int v = baseVal + aboveBase;
            colors[i] = (ushort) (0x8000 | v * 0x400 | v * 0x20 | v);
            }

            return colors;
        }   
    }
}
