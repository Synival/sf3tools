using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class ZoneRenderer {
        public void Draw(
            GeneralResources general,
            SceneResources scene
        ) {
            if (scene == null || scene.ZoneModels == null)
                return;

            using (general.SolidShader.Use()) {
                GL.Disable(EnableCap.DepthTest);
                foreach (var zone in scene.ZoneModels)
                    zone.Draw(general.SolidShader, null);
                GL.Enable(EnableCap.DepthTest);
            }
        }
    }
}