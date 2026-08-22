using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.Shared {
    public class GradientRenderer {
        public void Draw(
            GeneralResources general,
            QuadModel gradientModel,
            int stencilBit,
            bool depthCurrentlyEnabled,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (gradientModel == null)
                return;

            // TODO: 'depthCurrentlyEnabled' is pretty stilly. We should track the state somehow, and just push it.
            if (depthCurrentlyEnabled) {
                GL.Disable(EnableCap.DepthTest);
                GL.DepthMask(false);
            }

            general.SolidShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, Matrix4.Identity);
            general.SolidShader.UpdateUniform(ShaderUniformType.ViewMatrix, Matrix4.Identity);

            GL.StencilFunc(StencilFunction.Equal, stencilBit, stencilBit);
            gradientModel.Draw(general.SolidShader, null);
    
            general.SolidShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref projectionMatrix);
            general.SolidShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);

            if (depthCurrentlyEnabled) {
                GL.DepthMask(true);
                GL.Enable(EnableCap.DepthTest);
            }
        }
    }
}
