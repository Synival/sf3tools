using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class MPD_SkyRenderer {
        public MPD_SkyRenderer(GradientRenderer gradientRenderer) {
            GradientRenderer = gradientRenderer;
        }

        public void Draw(
            GeneralResources general,
            MPD_SkyModelResources skyModel,
            GradientResources gradients,
            MPD_RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (skyModel?.Model == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            general.TextureShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, Matrix4.Identity);

            const float c_horizRepeatCount = 2f;
            const float c_vertRepeatCount = 15f;
            const float c_width = 1.6f;
            const float c_height = 1.0666f;
            var xOffset = (MathHelpers.ActualMod(cameraYaw / 360f * c_horizRepeatCount - options.BackgroundX / 512.0f, 1.0f) * c_width - 0.5f) * 2.0f;
            var yOffset = (MathHelpers.ActualMod(cameraPitch / -360f * c_vertRepeatCount + options.BackgroundY / 256.0f, 1.0f) * c_height - 0.5f) * 2.0f - 0.1f;

            general.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix,
                Matrix4.Identity *
                Matrix4.CreateScale(c_width, c_height, 1.0f) *
                Matrix4.CreateTranslation(xOffset, yOffset, 0)
            );

            GL.StencilFunc(StencilFunction.Always, 0x02, 0x02);
            GL.StencilMask(0x02);

            using (skyModel.Texture.Use(TextureUnit.Texture0))
                skyModel.Model.Draw(general.TextureShader, null);

            if (options.DrawGradients)
                GradientRenderer.Draw(general, gradients?.SkyGradientModel, 0x02, false, ref projectionMatrix, ref viewMatrix);

            general.TextureShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref projectionMatrix);
            general.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public GradientRenderer GradientRenderer { get; }
    }
}
