using CommonLib.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class GroundRenderer {
        public GroundRenderer(GradientRenderer gradientRenderer) {
            GradientRenderer = gradientRenderer;
        }

        public void Draw(
            GeneralResources general,
            GroundModelResources groundModel,
            GradientResources gradients,
            IColorAdjustRGB555 groundAdj,
            RendererOptions options,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (groundModel?.Model == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            var glow = Vector3.Zero;
            if (options.ApplyLighting && groundAdj != null) {
                glow = new Vector3(
                    groundAdj.R / (float) 0x1F, 
                    groundAdj.G / (float) 0x1F, 
                    groundAdj.B / (float) 0x1F
                );
            }
            general.TextureShader.UpdateUniform(ShaderUniformType.GlobalGlow, ref glow);

            GL.StencilFunc(StencilFunction.Always, 0x01, 0x01);
            GL.StencilMask(0x01);

            using (groundModel.Texture.Use(TextureUnit.Texture0))
                groundModel.Model.Draw(general.TextureShader, null);

            if (options.DrawGradients)
                GradientRenderer.Draw(general, gradients?.GroundGradientModel, 0x01, false, ref projectionMatrix, ref viewMatrix);

            general.TextureShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public GradientRenderer GradientRenderer { get; }
    }
}
