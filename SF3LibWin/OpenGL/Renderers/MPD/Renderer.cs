using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.Shared;
using SF3.Win.Types;
using static SF3.Win.Controls.MPD_ViewerGLControl;

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
        }

        public void DrawScene(
            RendererResources resources,
            RendererOptions options,
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
                DrawOutlines(
                    resources.General, resources.Models, resources.Scene, resources.Editor, resources.Screen, resources.CollisionModels,
                    options, state.CameraYaw, state.CameraPitch, state.ScreenWidth, state.ScreenHeight
                );
            }
        }

        public void DrawSelectionScene(
            RendererResources resources,
            RendererOptions options,
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

            if (options.DrawSurfaceModel && resources.SurfaceModel?.Blocks != null) {
                using (resources.General.SolidShader.Use()) {
                    foreach (var block in resources.SurfaceModel.Blocks)
                        if (block.SelectionModel != null)
                            block.SelectionModel.Draw(resources.General.SolidShader);
                }
            }

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
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
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
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
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
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
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

        public void DrawOutlines(
            GeneralResources general,
            ModelResources models,
            SceneResources scene,
            EditorResources editor,
            ScreenResources screen,
            CollisionResources collision,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            int screenWidth,
            int screenHeight
        ) {
            var outlineFramebuffer1 = screen.OutlineFramebuffer1;
            var outlineFramebuffer2 = screen.OutlineFramebuffer2;

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            void RenderOutlinesFor(Action renderAction, Shader shader = null) {
                // Draw the visible models that need outlines on the screen's stencil buffer *only*.
                using ((shader ?? general.ColorizeShader).Use()) {
                    GL.ColorMask(false, false, false, false);
                    GL.Enable(EnableCap.StencilTest);
                    GL.StencilFunc(StencilFunction.Always, 0x08, 0x08);
                    GL.StencilMask(0x08);
                    GL.Clear(ClearBufferMask.StencilBufferBit);

                    renderAction();

                    GL.Disable(EnableCap.StencilTest);
                    GL.ColorMask(true, true, true, true);
                }

                GL.Viewport(0, 0, outlineFramebuffer1.Width, outlineFramebuffer1.Height);
                using (outlineFramebuffer1.UseDraw()) {
                    GL.ClearColor(0, 0, 0, 0);
                    GL.Clear(ClearBufferMask.ColorBufferBit);

                    // Now start producing the outline by first rendering the visible models to a framebuffer.
                    using (general.ColorizeShader.Use())
                        renderAction();
                }

                GL.Disable(EnableCap.Blend);
                using (general.OutlineBlurPassShader.Use()) {
                    general.OutlineBlurPassShader.UpdateUniform("texelSize", new Vector2(1.0f / outlineFramebuffer1.Width, 1.0f / outlineFramebuffer1.Height));

                    using (outlineFramebuffer1.ColorTexture.Use())
                    using (outlineFramebuffer2.UseDraw()) {
                        general.OutlineBlurPassShader.UpdateUniform("blurDirectionVector", new Vector2(1.0f, 0.0f));
                        general.FullScreenQuad.Draw(general.OutlineBlurPassShader);
                    }

                    using (outlineFramebuffer2.ColorTexture.Use())
                    using (outlineFramebuffer1.UseDraw()) {
                        general.OutlineBlurPassShader.UpdateUniform("blurDirectionVector", new Vector2(0.0f, 1.0f));
                        general.FullScreenQuad.Draw(general.OutlineBlurPassShader);
                    }
                }
                GL.Enable(EnableCap.Blend);
                GL.Viewport(0, 0, screenWidth, screenHeight);

                using (general.OutlineToScreenShader.Use())
                using (outlineFramebuffer1.ColorTexture.Use()) {
                    GL.Enable(EnableCap.StencilTest);
                    GL.StencilFunc(StencilFunction.Notequal, 0x08, 0x08);
                    GL.StencilMask(0x00);

                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    general.FullScreenQuad.Draw(general.OutlineToScreenShader);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                    GL.Disable(EnableCap.StencilTest);
                }
            }

            void RenderTile(QuadModel model, Texture texture, Vector4 color) {
                RenderOutlinesFor(() => {
                    general.ColorizeShader.UpdateUniform("color", color);
                    general.ColorizeShader.UpdateUniform("alwaysShow", true);
                    model.Draw(general.ColorizeShader);
                });
            }

            (IMPD_ModelInstance Model, ModelGroup ModelGroup) GetModelInstance(SelectableModel selectableModel) {
                var modelGroups = models.ModelsByIDByCollection.TryGetValue(selectableModel.Collection, out var collectionObj) ? collectionObj : null;
                if (modelGroups == null)
                    return (null, null);

                var modelInstance = models.ModelInstances.FirstOrDefault(x => x.ID == selectableModel.InstanceID && x.Collection.Collection == selectableModel.Collection);
                if (modelInstance == null)
                    return (null, null);

                var modelGroup = modelGroups.TryGetValue(modelInstance.ModelID, out var modelGroupObj) ? modelGroupObj : null;
                if (modelGroup == null)
                    return (null, null);

                return (modelInstance, modelGroup);
            };

            void RenderModel(SelectableModel selectableModel, Vector4 color) {
                var mwg = GetModelInstance(selectableModel);
                if (mwg.Model == null || mwg.ModelGroup == null)
                    return;

                RenderOutlinesFor(() => {
                    general.ColorizeShader.UpdateUniform("color", color);
                    general.ColorizeShader.UpdateUniform("alwaysShow", false);

                    ModelRenderer.SetModelAndNormalMatricesForModel(models, mwg.Model, general.ColorizeShader, options, cameraYaw, cameraPitch);
                    mwg.ModelGroup.SolidTexturedModel?.Draw(general.ColorizeShader);
                    mwg.ModelGroup.SolidUntexturedModel?.Draw(general.ColorizeShader);
                    mwg.ModelGroup.SemiTransparentTexturedModel?.Draw(general.ColorizeShader);
                    mwg.ModelGroup.SemiTransparentUntexturedModel?.Draw(general.ColorizeShader);

                    general.ColorizeShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                });
            }

            void RenderActor(SelectableActor selectableActor, Vector4 color) {
                var actor = scene?.ActorsBySpriteID?.Values?.SelectMany(x => x)?.FirstOrDefault(x => x.ID == selectableActor.ID);
                if (actor == null)
                    return;

                var spriteId = actor.SpriteID;
                var model    = scene.ModelsBySpriteID.TryGetValue(spriteId, out var modelObj) ? modelObj : null;
                if (model == null)
                    return;

                RenderOutlinesFor(() => {
                    var (baseMatrix, baseRotationMatrix) = ActorRenderer.GetSpriteDrawMatrices(cameraYaw, cameraPitch);

                    var shader = general.SpriteShader;
                    using (scene.ActorTextureAtlas.Use()) {
                        _ = shader.UpdateUniform("colorize", true);
                        _ = shader.UpdateUniform("cameraDistAdjust", 0.5f);

                        ActorRenderer.SetupSpriteShaderUniforms(shader, baseRotationMatrix, actor, cameraYaw, color);
                        model.Draw(shader);

                        _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                    }
                }, general.SpriteShader);
            }

            void RenderCollisionLine(SelectableCollisionLine line, Vector4 color) {
                var model = collision.IndividualModels.FirstOrDefault(x => !x.IsPoint && x.ID == line.ID);
                if (model == null)
                    return;

                RenderOutlinesFor(() => {
                    general.ColorizeShader.UpdateUniform("color", color);
                    general.ColorizeShader.UpdateUniform("alwaysShow", true);
                    model.Draw(general.ColorizeShader);
                });
            }

            void RenderCollisionPoint(SelectableCollisionPoint point, Vector4 color) {
                var model = collision.IndividualModels.FirstOrDefault(x => x.IsPoint && x.ID == point.ID);
                if (model == null)
                    return;

                RenderOutlinesFor(() => {
                    general.ColorizeShader.UpdateUniform("color", color);
                    general.ColorizeShader.UpdateUniform("alwaysShow", true);
                    model.Draw(general.ColorizeShader);
                });
            }

            if (editor.MouseoverTileModel != null)
                RenderTile(editor.MouseoverTileModel, editor.MouseoverTileTexture, new Vector4(0.25f, 0.5f, 0.5f, 0.5f));
            else if (editor.MouseoverObject is SelectableModel mouseoverModel)
                RenderModel(mouseoverModel, mouseoverModel.Collection == MPD_CollectionType.Primary ? new Vector4(0.5f, 0.375f, 0.25f, 0.5f) : new Vector4(0.5f, 0.25f, 0.25f, 0.5f));
            else if (editor.MouseoverObject is SelectableActor mouseoverActor)
                RenderActor(mouseoverActor, new Vector4(0.25f, 0.25f + 0.25f / 4, 0.5f, 0.5f));
            else if (editor.MouseoverObject is SelectableCollisionLine mouseoverLine)
                RenderCollisionLine(mouseoverLine, new Vector4(1, 0.75f, 1, 0.5f));
            else if (editor.MouseoverObject is SelectableCollisionPoint mouseoverPoint)
                RenderCollisionPoint(mouseoverPoint, new Vector4(1, 0.75f, 1, 0.5f));

            if (editor.SelectedTileModel != null)
                RenderTile(editor.SelectedTileModel, editor.SelectedTileTexture, new Vector4(0.0f, 1.0f, 1.0f, 1.0f));
            else if (editor.SelectedObjects.Count > 0) {
                foreach (var selectedObject in editor.SelectedObjects) {
                    if (selectedObject is SelectableModel selectedModel)
                        RenderModel(selectedModel, selectedModel.Collection == MPD_CollectionType.Primary ? new Vector4(1.0f, 0.5f, 0.0f, 1.0f) : new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                    else if (selectedObject is SelectableActor selectedActor)
                        RenderActor(selectedActor, new Vector4(0.0f, 0.25f, 1.0f, 1.0f));
                    else if (selectedObject is SelectableCollisionLine selectedLine)
                        RenderCollisionLine(selectedLine, new Vector4(1, 0.5f, 1, 1.0f));
                    else if (selectedObject is SelectableCollisionPoint selectedPoint)
                        RenderCollisionPoint(selectedPoint, new Vector4(1, 0.5f, 1, 1.0f));
                }
            }

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public void InvalidateModelMatrices() => ModelRenderer.InvalidateModelMatrices();
        public void InvalidateSpriteMatrices(ModelResources models) => ModelRenderer.InvalidateSpriteMatrices(models);

        public void DrawControlFocusedBox(RendererResources resources, RendererOptions options, RendererState state) {
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            using (resources.General.ColorToScreenShader.Use())
                resources.Screen.FocusedBox.Draw(resources.General.ColorToScreenShader);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }

        public CollisionLineRenderer CollisionLineRenderer { get; }
        public SurfaceModelRenderer SurfaceModelRenderer { get; }
        public GradientRenderer GradientRenderer { get; }
        public SkyRenderer SkyRenderer { get; }
        public GroundRenderer GroundRenderer { get; }
        public ActorRenderer ActorRenderer { get; }
        public ModelRenderer ModelRenderer { get; }
        public BoundaryRenderer BoundaryRenderer { get; }
        public ZoneRenderer ZoneRenderer { get; }
    }
}
