using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SF3.Win.OpenGL.MPD {
    public class ScreenResources : ResourcesBase {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            SelectFramebuffer?.Dispose();
            OutlineFramebuffer1?.Dispose();
            OutlineFramebuffer2?.Dispose();
            FocusedBox?.Dispose();

            SelectFramebuffer   = null;
            OutlineFramebuffer1 = null;
            OutlineFramebuffer2 = null;
            FocusedBox          = null;

            Width  = 0;
            Height = 0;
        }

        public void Update(int width, int height) {
            Width  = width;
            Height = height;

            UpdateFramebuffers();
            UpdateFocusedBox();
        }

        public void UpdateFramebuffers() {
            const int c_outlinePixelCount = 400 * 400;

            SelectFramebuffer?.Dispose();
            SelectFramebuffer = new Framebuffer(Width, Height, 3, RenderbufferStorage.DepthComponent);

            // Create two framebuffers for outlines to account for the 2 blur passes.
            // Make the size of the framebuffer no larger than 160,000 pixels (400x400), with the same width:height ratio as the viewport.
            // This is so make the outlines appear bigger without the need for more complicated gaussian blur passes.
            int outlineWidth, outlineHeight;
            if (Width * Height > c_outlinePixelCount) {
                var ratio = (float) Width / Height;
                outlineWidth  = (int) Math.Sqrt(c_outlinePixelCount * ratio);
                outlineHeight = c_outlinePixelCount / outlineWidth;
            }
            else {
                outlineWidth  = Width;
                outlineHeight = Height;
            }

            OutlineFramebuffer1?.Dispose();
            OutlineFramebuffer2?.Dispose();
            OutlineFramebuffer1 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
            OutlineFramebuffer2 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
        }

        private static readonly Vector4 c_focusColor = new Vector4(1, 1, 1, 0.5f);
        public void UpdateFocusedBox() {
            FocusedBox?.Dispose();

            const int c_focusedBorder = 3;
            float inH = c_focusedBorder / (float) (Width / 2);
            float inV = c_focusedBorder / (float) (Height / 2);

            var TL_outer  = new Vector3(-1, -1, 0);
            var TR_outer  = new Vector3( 1, -1, 0);
            var BR_outer  = new Vector3( 1,  1, 0);
            var BL_outer  = new Vector3(-1,  1, 0);

            var TL_hInner = new Vector3(-1 + inH, -1, 0);
            var TR_hInner = new Vector3( 1 - inH, -1, 0);
            var BR_hInner = new Vector3( 1 - inH,  1, 0);
            var BL_hInner = new Vector3(-1 + inH,  1, 0);

            var TL_inner  = new Vector3(-1 + inH, -1 + inV, 0);
            var TR_inner  = new Vector3( 1 - inH, -1 + inV, 0);
            var BR_inner  = new Vector3( 1 - inH,  1 - inV, 0);
            var BL_inner  = new Vector3(-1 + inH,  1 - inV, 0);

            var quads = new Quad[] {
                new([TL_outer,  BL_outer,  BL_hInner, TL_hInner], c_focusColor),
                new([TL_hInner, TL_inner,  TR_inner,  TR_hInner], c_focusColor),
                new([TR_hInner, BR_hInner, BR_outer,  TR_outer ], c_focusColor),
                new([BL_inner,  BL_hInner, BR_hInner, BR_inner ], c_focusColor),
            };

            FocusedBox = new QuadModel(quads);
        }

        public Framebuffer SelectFramebuffer { get; private set; }
        public Framebuffer OutlineFramebuffer1 { get; private set; }
        public Framebuffer OutlineFramebuffer2 { get; private set; }

        public QuadModel FocusedBox { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
