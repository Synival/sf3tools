using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class BoundaryRenderer {
        public void Draw(GeneralResources general, BoundaryModelResources boundaryModels) {
            if (boundaryModels?.CameraBoundaryModel == null && boundaryModels?.BattleBoundaryModel == null)
                return;

            using (general.SolidShader.Use()) {
                GL.Disable(EnableCap.DepthTest);
                boundaryModels.BattleBoundaryModel?.Draw(general.SolidShader, null);
                boundaryModels.CameraBoundaryModel?.Draw(general.SolidShader, null);
                GL.Enable(EnableCap.DepthTest);
            }
        }
    }
}