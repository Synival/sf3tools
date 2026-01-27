using CommonLib.Imaging;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Lighting : IMPD_Lighting {
        public MPD_Lighting() {
            Palette = new Palette(0x20);
        }

        public MPD_Lighting(IMPD_Lighting original) {
            if (original.Palette != null)
                Palette = new Palette(original.Palette);
            Pitch = original.Pitch;
            Yaw   = original.Yaw;
        }

        public Palette Palette { get; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
    }
}
