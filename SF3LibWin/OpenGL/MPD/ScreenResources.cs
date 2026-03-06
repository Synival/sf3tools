using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL.MPD {
    public class ScreenResources : ResourcesBase {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            SelectFramebuffer?.Dispose();
            OutlineFramebuffer1?.Dispose();
            OutlineFramebuffer2?.Dispose();

            SelectFramebuffer = null;
            OutlineFramebuffer1 = null;
            OutlineFramebuffer2 = null;
        }

        public void Update(int width, int height) {
            UpdateFramebuffers(width, height);
        }

        public void UpdateFramebuffers(int width, int height) {
            const int c_outlinePixelCount = 400 * 400;

            SelectFramebuffer?.Dispose();
            SelectFramebuffer = new Framebuffer(width, height, 3, RenderbufferStorage.DepthComponent);

            // Create two framebuffers for outlines to account for the 2 blur passes.
            // Make the size of the framebuffer no larger than 160,000 pixels (400x400), with the same width:height ratio as the viewport.
            // This is so make the outlines appear bigger without the need for more complicated gaussian blur passes.
            int outlineWidth, outlineHeight;
            if (width * height > c_outlinePixelCount) {
                var ratio = (float) width / height;
                outlineWidth  = (int) Math.Sqrt(c_outlinePixelCount * ratio);
                outlineHeight = c_outlinePixelCount / outlineWidth;
            }
            else {
                outlineWidth = width;
                outlineHeight = height;
            }

            OutlineFramebuffer1?.Dispose();
            OutlineFramebuffer2?.Dispose();
            OutlineFramebuffer1 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
            OutlineFramebuffer2 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
        }

        public Framebuffer SelectFramebuffer { get; private set; }
        public Framebuffer OutlineFramebuffer1 { get; private set; }
        public Framebuffer OutlineFramebuffer2 { get; private set; }
    }
}
