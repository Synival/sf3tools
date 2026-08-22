using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.Renderers.Shared {
    public class FocusedBoxRenderer {
        public void Draw(GeneralResources general, ScreenResources screen) {
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            using (general.ColorToScreenShader.Use())
                screen.FocusedBox.Draw(general.ColorToScreenShader);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
    }
}
