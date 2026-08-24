using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class MPD_SurfaceModelRenderer {
        public void Draw(
            GeneralResources general,
            MPD_SurfaceModelResources surfaceModel,
            LightingResources lighting,
            MPD_RendererOptions options
        ) {
            if (!(surfaceModel?.Blocks?.Length > 0))
                return;

            var terrainTypesTexture = options.DrawTerrainTypes ? surfaceModel.TerrainTypesTexture : general.TransparentBlackTexture;
            var eventIdsTexture     = options.DrawEventIDs     ? surfaceModel.EventIDsTexture     : general.TransparentBlackTexture;
            var lightingTexture     = lighting.LightingTexture ?? general.WhiteTexture;

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-1.0f, -1.0f);

            general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, options.ApplyLighting ? options.UseOutsideLighting ? 2 : 1 : 0);
            general.ObjectShader.UpdateUniform(ShaderUniformType.SmoothLighting, options.SmoothLighting);

            using (terrainTypesTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureTerrainTypes))
            using (eventIdsTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureEventIDs))
            using (lightingTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureLighting))
            using (options.DrawSurfaceModel ? null : general.TransparentBlackTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureAtlas))
            using (general.ObjectShader.Use()) {
                foreach (var block in surfaceModel.Blocks) {
                    if (options.DrawSurfaceModel)
                        block.Model?.Draw(general.ObjectShader);
                    else
                        block.Model?.Draw(general.ObjectShader, null);

                    if (block.MissingTexturesModel != null)
                        using (general.WhiteTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureAtlas))
                            block.MissingTexturesModel?.Draw(general.ObjectShader, null);

                    if (block.UntexturedModel != null)
                        using (general.TransparentBlackTexture.Use((ObjectShaderTextureUnit) MPD_ObjectShaderTextureUnit.TextureAtlas))
                            block.UntexturedModel?.Draw(general.ObjectShader, null);
                }
            }

            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawWireframe(GeneralResources general, MPD_SurfaceModelResources surfaceModel) {
            if (surfaceModel?.Blocks == null)
                return;

            foreach (var block in surfaceModel.Blocks) {
                block.UntexturedModel?.Draw(general.WireframeShader, null);
                block.MissingTexturesModel?.Draw(general.WireframeShader, null);
                block.Model?.Draw(general.WireframeShader, null);
            }
        }

        public void DrawNormals(GeneralResources general, MPD_SurfaceModelResources surfaceModel) {
            if (!(surfaceModel?.Blocks?.Length > 0))
                return;

            using (general.NormalsShader.Use()) {
                foreach (var block in surfaceModel.Blocks) {
                    if (block.Model != null || block.UntexturedModel != null) {
                        block.Model?.Draw(general.NormalsShader, null);
                        block.UntexturedModel?.Draw(general.NormalsShader, null);
                    }
                }
            }
        }

        public void DrawSelection(
            GeneralResources general,
            MPD_SurfaceModelResources surfaceModel
        ) {
            using (general.SolidShader.Use()) {
                foreach (var block in surfaceModel.Blocks)
                    if (block.SelectionModel != null)
                        block.SelectionModel.Draw(general.SolidShader);
            }
        }
    }
}
