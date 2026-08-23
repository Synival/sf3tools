using CommonLib.SGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class Renderer {
        public Renderer() {
            CollisionLineRenderer = new CollisionLineRenderer();
            SurfaceModelRenderer  = new SurfaceModelRenderer();
            GradientRenderer      = new GradientRenderer();
            SkyRenderer           = new SkyRenderer(GradientRenderer);
            GroundRenderer        = new GroundRenderer(GradientRenderer);
            ActorRenderer         = new ActorRenderer();
            ModelRenderer         = new ModelRenderer();
            BoundaryRenderer      = new BoundaryRenderer();
            ZoneRenderer          = new ZoneRenderer();
            OutlineRenderer       = new OutlineRenderer(ModelRenderer, ActorRenderer);
            FocusedBoxRenderer    = new FocusedBoxRenderer();
        }

        public void DrawScene(
            RendererResources resources,
            MPD_RendererOptions options,
            RendererState state
        ) {
            // Enable 'CullFace' to draw everything single-sided (as the game actually is)
            if (!options.ForceTwoSidedTextures)
                GL.Enable(EnableCap.CullFace);

            // Enable 'StencilTest' for gradients on various render passes.
            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            if (options.DrawSky)
                SkyRenderer.Draw(resources.General, resources.SkyModel, resources.Gradients, options, state.CameraYaw, state.CameraPitch, ref state.ProjectionMatrix, ref state.ViewMatrix);
            if (options.DrawGround)
                GroundRenderer.Draw(resources.General, resources.GroundModel, resources.Gradients, options.GroundAdj, options, ref state.ProjectionMatrix, ref state.ViewMatrix);

            var modelsWithGroups = state.GetModelsWithGroups(resources.Models, options);
            if (options.DrawNormals)
                DrawSceneObjectNormals(resources.General, resources.Models, resources.SurfaceModel, options, state.CameraYaw, state.CameraPitch, modelsWithGroups);
            else if (options.WillDrawAnyObjects)
                DrawSceneObjects(resources.General, resources.Models, resources.SurfaceModel, resources.Scene, resources.Gradients, resources.Lighting, options, state.CameraYaw, state.CameraPitch, ref state.ProjectionMatrix, ref state.ViewMatrix, modelsWithGroups);

            // Done rendering gradients; disable the stencil test.
            GL.Disable(EnableCap.StencilTest);

            // Done rendering the real scene; disable one-sided rendering
            if (!options.ForceTwoSidedTextures)
                GL.Disable(EnableCap.CullFace);

            if (options.DrawCollisionLines)
                CollisionLineRenderer.Draw(resources.General, resources.CollisionModels, state.CameraYaw, selectionColors: false);

            if (options.DrawWireframe)
                DrawSceneWireframes(resources.General, resources.Models, resources.SurfaceModel, options, state.CameraYaw, state.CameraPitch, modelsWithGroups);

            if (options.DrawBoundaries)
                BoundaryRenderer.Draw(resources.General, resources.BoundaryModels);

            if (options.DrawBattleZones)
                ZoneRenderer.Draw(resources.General, resources.Scene);

            if (options.DrawOutlines && resources.Screen != null) {
                OutlineRenderer.Draw(
                    resources.General, resources.Models, resources.Scene, resources.Editor, resources.Screen, resources.CollisionModels,
                    options, state.CameraYaw, state.CameraPitch, state.ScreenWidth, state.ScreenHeight
                );
            }
        }

        public void DrawSelectionScene(
            RendererResources resources,
            MPD_RendererOptions options,
            RendererState state
        ) {
            // Enable 'CullFace' to draw everything single-sided (as the game actually is)
            if (!options.ForceTwoSidedTextures)
                GL.Enable(EnableCap.CullFace);

            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var modelsWithGroups = state.GetModelsWithGroups(resources.Models, options);
            if (options.WillDrawAnyModels)
                ModelRenderer.Draw(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: false, selectionColors: true);

            if (options.DrawSurfaceModel && resources.SurfaceModel?.Blocks != null)
                SurfaceModelRenderer.DrawSelection(resources.General, resources.SurfaceModel);

            if (options.WillDrawAnyModels)
                ModelRenderer.Draw(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: true, selectionColors:  true);

            if (options.DrawActors)
                ActorRenderer.Draw(resources.General, resources.Scene, state.CameraYaw, state.CameraPitch, selectionColors: true);

            if (options.DrawCollisionLines)
                CollisionLineRenderer.Draw(resources.General, resources.CollisionModels, state.CameraYaw, selectionColors: true);

            // Disable 'CullFace' if previously enabled
            if (!options.ForceTwoSidedTextures)
                GL.Disable(EnableCap.CullFace);
        }

        public void DrawSceneObjectNormals(
            GeneralResources general,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            MPD_RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            if (options.WillDrawAnyModels)
                ModelRenderer.DrawNormals(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);

            if (options.DrawSurfaceModel)
                SurfaceModelRenderer.DrawNormals(general, surfaceModel);
        }

        public void DrawSceneObjects(
            GeneralResources general,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            SceneResources scenes,
            GradientResources gradients,
            LightingResources lighting,
            MPD_RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            GL.StencilFunc(StencilFunction.Always, 0x04, 0x04);
            GL.StencilMask(0x04);

            if (options.WillDrawAnyModels)
                ModelRenderer.Draw(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: false, selectionColors: false);

            if (options.WillDrawSurfaceModel)
                SurfaceModelRenderer.Draw(general, surfaceModel, lighting, options);

            if (options.DrawActors)
                ActorRenderer.Draw(general, scenes, cameraYaw, cameraPitch, selectionColors: false);

            if (options.WillDrawAnyModels)
                ModelRenderer.Draw(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: true, selectionColors: false);

            if (options.DrawGradients)
                GradientRenderer.Draw(general, gradients?.ModelsGradientModel, 0x04, true, ref projectionMatrix, ref viewMatrix);
        }

        public void DrawSceneWireframes(
            GeneralResources general,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            MPD_RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-2.0f, -2.0f);

            using (general.WireframeShader.Use())
            using (general.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                if (options.WillDrawAnyModels)
                    ModelRenderer.DrawWireframe(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);

                if (options.WillDrawSurfaceModelWireframe)
                    SurfaceModelRenderer.DrawWireframe(general, surfaceModel);
            }

            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public void InvalidateModelMatrices() => ModelRenderer.InvalidateModelMatrices();
        public void InvalidateSpriteMatrices(ModelResources models) => ModelRenderer.InvalidateSpriteMatrices(models);

        public void DrawControlFocusedBox(RendererResources resources) => FocusedBoxRenderer.Draw(resources.General, resources.Screen);

        public CollisionLineRenderer CollisionLineRenderer { get; }
        public SurfaceModelRenderer SurfaceModelRenderer { get; }
        public GradientRenderer GradientRenderer { get; }
        public SkyRenderer SkyRenderer { get; }
        public GroundRenderer GroundRenderer { get; }
        public ActorRenderer ActorRenderer { get; }
        public ModelRenderer ModelRenderer { get; }
        public BoundaryRenderer BoundaryRenderer { get; }
        public ZoneRenderer ZoneRenderer { get; }
        public OutlineRenderer OutlineRenderer { get; }
        public FocusedBoxRenderer FocusedBoxRenderer { get; }
    }
}
