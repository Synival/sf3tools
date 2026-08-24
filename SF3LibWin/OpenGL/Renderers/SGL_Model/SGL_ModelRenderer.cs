using CommonLib.SGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.Shared;

namespace SF3.Win.OpenGL.Renderers.SGL_Model {
    public class SGL_ModelRenderer {
        public SGL_ModelRenderer() {
            ModelRenderer      = new ModelRenderer();
            FocusedBoxRenderer = new FocusedBoxRenderer();
        }

        public void DrawScene(
            SGL_ModelRendererResources resources,
            RendererOptions options,
            RendererState state
        ) {
            // Enable 'CullFace' to draw everything single-sided (as the game actually is)
            if (!options.ForceTwoSidedTextures)
                GL.Enable(EnableCap.CullFace);

            var modelsWithGroups = state.GetModelsWithGroups(resources.Models, options);
            if (options.DrawNormals)
                DrawSceneObjectNormals(resources.General, resources.Models, options, state.CameraYaw, state.CameraPitch, modelsWithGroups);
            DrawSceneObjects(resources.General, resources.Models, resources.Lighting, options, state.CameraYaw, state.CameraPitch, ref state.ProjectionMatrix, ref state.ViewMatrix, modelsWithGroups);

            // Done rendering the real scene; disable one-sided rendering
            if (!options.ForceTwoSidedTextures)
                GL.Disable(EnableCap.CullFace);

            if (options.DrawWireframe)
                DrawSceneWireframes(resources.General, resources.Models, options, state.CameraYaw, state.CameraPitch, modelsWithGroups);
        }

        public void DrawSelectionScene(
            SGL_ModelRendererResources resources,
            RendererOptions options,
            RendererState state
        ) {
            // Enable 'CullFace' to draw everything single-sided (as the game actually is)
            if (!options.ForceTwoSidedTextures)
                GL.Enable(EnableCap.CullFace);

            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var modelsWithGroups = state.GetModelsWithGroups(resources.Models, options);
            ModelRenderer.Draw(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: false, selectionColors: true);
            ModelRenderer.Draw(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: true, selectionColors:  true);

            // Disable 'CullFace' if previously enabled
            if (!options.ForceTwoSidedTextures)
                GL.Disable(EnableCap.CullFace);
        }

        public void DrawSceneObjectNormals(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            ModelRenderer.DrawNormals(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);
        }

        public void DrawSceneObjects(
            GeneralResources general,
            ModelResources models,
            LightingResources lighting,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            ModelRenderer.Draw(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: false, selectionColors: false);
            ModelRenderer.Draw(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: true,  selectionColors: false);
        }

        public void DrawSceneWireframes(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-2.0f, -2.0f);

            using (general.WireframeShader.Use())
            using (general.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                ModelRenderer.DrawWireframe(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);
            }

            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public void InvalidateModelMatrices() => ModelRenderer.InvalidateModelMatrices();
        public void InvalidateSpriteMatrices(ModelResources models) => ModelRenderer.InvalidateSpriteMatrices(models);

        public void DrawControlFocusedBox(SGL_ModelRendererResources resources) => FocusedBoxRenderer.Draw(resources.General, resources.Screen);

        public FocusedBoxRenderer FocusedBoxRenderer { get; }
        public ModelRenderer ModelRenderer { get; }
    }
}
