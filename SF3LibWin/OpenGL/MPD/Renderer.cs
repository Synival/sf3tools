using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.OpenGL.Shared;
using SF3.Win.Types;
using static SF3.Win.Controls.MPD_ViewerGLControl;

namespace SF3.Win.OpenGL.MPD {
    public class Renderer {
        public const float c_selectionSurfaceTile     = 0;
        public const float c_selectionPrimaryModels   = 1;
        public const float c_selectionExtraModels     = 2;
        public const float c_selectionActors          = 3;
        public const float c_selectionCollisionLines  = 4;
        public const float c_selectionCollisionPoints = 5;

        public const float c_selectionSurfaceTileB     = c_selectionSurfaceTile     * (1.0f / 64.0f);
        public const float c_selectionPrimaryModelsB   = c_selectionPrimaryModels   * (1.0f / 64.0f);
        public const float c_selectionExtraModelsB     = c_selectionExtraModels     * (1.0f / 64.0f);
        public const float c_selectionActorsB          = c_selectionActors          * (1.0f / 64.0f);
        public const float c_selectionCollisionLinesB  = c_selectionCollisionLines  * (1.0f / 64.0f);
        public const float c_selectionCollisionPointsB = c_selectionCollisionPoints * (1.0f / 64.0f);

        public class RendererResources {
            public GeneralResources General;
            public ModelResources Models;
            public SurfaceModelResources SurfaceModel;
            public GroundModelResources GroundModel;
            public SkyModelResources SkyModel;
            public GradientResources Gradients;
            public LightingResources Lighting;
            public BoundaryModelResources BoundaryModels;
            public CollisionResources CollisionModels;
            public SceneResources Scene;
            public EditorResources Editor;
            public ScreenResources Screen;
        }

        public class RendererOptions {
            public bool DrawModels;
            public bool DrawExtraModels;
            public bool DrawSurfaceModel;
            public bool DrawGround;
            public bool DrawSky;
            public bool DrawGradients;
            public bool DrawActors;

            public bool DrawNormals;
            public bool DrawWireframe;
            public bool DrawOutlines;

            public bool DrawTerrainTypes;
            public bool DrawEventIDs;
            public bool DrawBoundaries;
            public bool DrawBattleZones;
            public bool DrawCollisionLines;

            public bool ApplyLighting;

            public bool HideModelsNotFacingCamera;
            public float ModelsYRotation = 0.0f;
            public float ModelsViewAngleMax = 108.0f;
            public float ModelsViewAngleMin = -108.0f;

            public bool UseOutsideLighting;
            public bool RotateSpritesUp;
            public bool ForceTwoSidedTextures;
            public bool SmoothLighting;

            public float BackgroundX = 0.00f;
            public float BackgroundY = 0.00f;

            public IColorAdjustRGB555 GroundAdj;

            public HashSet<int> ModelsToHide;

            public bool WillDrawSurfaceModel
                => DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs;
            public bool WillDrawAnyModels
                => DrawModels || DrawExtraModels;
            public bool WillDrawAnyObjects
                => WillDrawAnyModels || WillDrawSurfaceModel;
            public bool WillDrawSurfaceModelWireframe
                => DrawWireframe && !DrawNormals && (DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs);
        }

        public class RendererState {
            public float CameraYaw;
            public float CameraPitch;
            public int ScreenWidth;
            public int ScreenHeight;
            public Matrix4 ProjectionMatrix;
            public Matrix4 ViewMatrix;

            public (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] GetModelsWithGroups(ModelResources models, RendererOptions options) {
                if (_modelsWithGroups != null)
                    return _modelsWithGroups;

                var modelDirectionsFacingCamera = GetModelDirectionsFacingCamera(options);

                if (models.ModelInstances == null) {
                    _modelsWithGroups = [];
                    return _modelsWithGroups;
                }

                bool IsVisibleCollection(MPD_CollectionType collection) {
                    return
                        (collection != MPD_CollectionType.ExtraModels && options.DrawModels) ||
                        (collection == MPD_CollectionType.ExtraModels && options.DrawExtraModels);
                }

                _modelsWithGroups = models.ModelInstances
                    .Select(x => (Model: x, ModelGroup: models.ModelsByIDByCollection[x.Collection.Collection].TryGetValue(x.ModelID, out var pd) ? pd : null))
                    .Where(x => x.ModelGroup != null && IsVisibleCollection(x.Model.Collection.Collection))
                    .Where(x => {
                        var direction = x.Model.OnlyVisibleFromDirection;
                        return direction == ModelDirectionType.Unset || modelDirectionsFacingCamera[(int) direction];
                    })
                    .Where(x => options?.ModelsToHide?.Contains(x.Model.ID) != true)
                    .ToArray();

                return _modelsWithGroups;
            }

            private bool[] GetModelDirectionsFacingCamera(RendererOptions options) {
                if (_modelDirectionsFacingCamera != null)
                    return _modelDirectionsFacingCamera;

                var showModelsInAllDirections = !options.HideModelsNotFacingCamera;

                bool WithinAngleRange(ModelDirectionType dir) {
                    if (showModelsInAllDirections)
                        return true;
                    var angle = 540 - 45 * (int) dir % 360;
                    var angleDiff = MathHelpers.ActualMod(CameraYaw - angle, 360.0f);
                    if (angleDiff > 180.0f)
                        angleDiff -= 360.0f;

                    return angleDiff > options.ModelsViewAngleMin && angleDiff < options.ModelsViewAngleMax;
                }

                _modelDirectionsFacingCamera = [
                    WithinAngleRange(ModelDirectionType.North),
                    WithinAngleRange(ModelDirectionType.Northeast),
                    WithinAngleRange(ModelDirectionType.East),
                    WithinAngleRange(ModelDirectionType.Southeast),
                    WithinAngleRange(ModelDirectionType.South),
                    WithinAngleRange(ModelDirectionType.Southwest),
                    WithinAngleRange(ModelDirectionType.West),
                    WithinAngleRange(ModelDirectionType.Northwest),
                ];

                return _modelDirectionsFacingCamera;
            }

            private (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] _modelsWithGroups;
            private bool[] _modelDirectionsFacingCamera;
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
                DrawSceneSky(resources.General, resources.SkyModel, resources.Gradients, options, state.CameraYaw, state.CameraPitch, ref state.ProjectionMatrix, ref state.ViewMatrix);
            if (options.DrawGround)
                DrawSceneGround(resources.General, resources.GroundModel, resources.Gradients, options.GroundAdj, options, ref state.ProjectionMatrix, ref state.ViewMatrix);

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
                DrawSceneCollisionLines(resources.General, resources.CollisionModels, state.CameraYaw, selectionColors: false);

            if (options.DrawWireframe)
                DrawSceneWireframes(resources.General, resources.Models, resources.SurfaceModel, options, state.CameraYaw, state.CameraPitch, modelsWithGroups);

            if (options.DrawBoundaries)
                DrawSceneBoundaries(resources.General, resources.BoundaryModels);

            if (options.DrawBattleZones)
                DrawZones(resources.General, resources.Scene);

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
                DrawSceneModels(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: false, selectionColors: true);

            if (options.DrawSurfaceModel && resources.SurfaceModel?.Blocks != null) {
                using (resources.General.SolidShader.Use()) {
                    foreach (var block in resources.SurfaceModel.Blocks)
                        if (block.SelectionModel != null)
                            block.SelectionModel.Draw(resources.General.SolidShader);
                }
            }

            if (options.WillDrawAnyModels)
                DrawSceneModels(resources.General, resources.Models, null, options, state.CameraYaw, state.CameraPitch, modelsWithGroups, transparentPass: true, selectionColors:  true);

            if (options.DrawActors)
                DrawActors(resources.General, resources.Scene, state.CameraYaw, state.CameraPitch, selectionColors: true);

            if (options.DrawCollisionLines)
                DrawSceneCollisionLines(resources.General, resources.CollisionModels, state.CameraYaw, selectionColors: true);

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
                DrawSceneModelsNormals(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);

            if (options.DrawSurfaceModel)
                DrawSceneSurfaceModelNormals(general, surfaceModel);
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
                DrawSceneModels(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: false, selectionColors: false);

            if (options.WillDrawSurfaceModel)
                DrawSceneSurfaceModel(general, surfaceModel, lighting, options);

            if (options.DrawActors)
                DrawActors(general, scenes, cameraYaw, cameraPitch, selectionColors: false);

            if (options.WillDrawAnyModels)
                DrawSceneModels(general, models, lighting, options, cameraYaw, cameraPitch, modelsWithGroups, transparentPass: true, selectionColors: false);

            if (options.DrawGradients)
                DrawSceneGradient(general, gradients?.ModelsGradientModel, 0x04, true, ref projectionMatrix, ref viewMatrix);
        }

        public void DrawSceneCollisionLines(GeneralResources general, CollisionResources collisionModels, float cameraYaw, bool selectionColors) {
            if (collisionModels == null)
                return;

            var shader = selectionColors ? general.ColorizeShader : general.SolidShader;
            if (selectionColors)
                shader.UpdateUniform("alwaysShow", true);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-4.0f, -4.0f);

            var yawSinCos = Math.SinCos(MathHelper.DegreesToRadians(cameraYaw));
            var sortedModels = collisionModels.IndividualModels
                .OrderBy(x => x.Quads[0].Center.X * yawSinCos.Sin + x.Quads[0].Center.Z * yawSinCos.Cos)
                .ToArray();

            Vector4 ModelSelectionColor(CollisionResources.CollisionQuadModel model) {
                var id = model.ID;
                var r = (id % 64) / 64.0f;
                var g = (id / 64) / 64.0f;
                return new Vector4(r, g, model.IsPoint ? c_selectionCollisionPointsB : c_selectionCollisionLinesB, 1.0f);
            }

            using (shader.Use()) {
                if (selectionColors) {
                    GL.Disable(EnableCap.CullFace);
                    foreach (var model in sortedModels) {
                        shader.UpdateUniform("color", ModelSelectionColor(model));
                        model.Draw(shader, null);
                    }
                    GL.Enable(EnableCap.CullFace);
                }
                else {
                    foreach (var model in sortedModels)
                        model.Draw(shader, null);

                    GL.Disable(EnableCap.DepthTest);
                    GL.DepthMask(false);

                    collisionModels.FullModel.Draw(shader, null);

                    GL.DepthMask(true);
                    GL.Enable(EnableCap.DepthTest);
                }
            }

            GL.Disable(EnableCap.PolygonOffsetFill);
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
                    DrawSceneModelsWireframe(general, models, options, cameraYaw, cameraPitch, modelsWithGroups);

                if (options.WillDrawSurfaceModelWireframe)
                    DrawSceneSurfaceModelWireframe(general, surfaceModel);
            }

            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public void DrawSceneBoundaries(GeneralResources general, BoundaryModelResources boundaryModels) {
            if (boundaryModels?.CameraBoundaryModel == null && boundaryModels?.BattleBoundaryModel == null)
                return;

            using (general.SolidShader.Use()) {
                GL.Disable(EnableCap.DepthTest);
                boundaryModels.BattleBoundaryModel?.Draw(general.SolidShader, null);
                boundaryModels.CameraBoundaryModel?.Draw(general.SolidShader, null);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        public void DrawSceneModelsNormals(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            if (models?.ModelsByIDByCollection == null)
                return;

            foreach (var mwg in modelsWithGroups) {
                SetModelAndNormalMatricesForModel(models, mwg.Model, general.NormalsShader, options, cameraYaw, cameraPitch);
                mwg.ModelGroup.SolidTexturedModel?.Draw(general.NormalsShader, null);
                mwg.ModelGroup.SolidUntexturedModel?.Draw(general.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentTexturedModel?.Draw(general.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentUntexturedModel?.Draw(general.NormalsShader, null);
            }

            // Reset model matrices to their identity.
            general.NormalsShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            general.NormalsShader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
        }

        public void DrawSceneSurfaceModelNormals(GeneralResources general, SurfaceModelResources surfaceModel) {
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

        public void DrawSceneSky(
            GeneralResources general,
            SkyModelResources skyModel,
            GradientResources gradients,
            RendererOptions options,
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
                DrawSceneGradient(general, gradients?.SkyGradientModel, 0x02, false, ref projectionMatrix, ref viewMatrix);

            general.TextureShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref projectionMatrix);
            general.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawSceneGround(
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
                DrawSceneGradient(general, gradients?.GroundGradientModel, 0x01, false, ref projectionMatrix, ref viewMatrix);

            general.TextureShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawSceneModels(
            GeneralResources general,
            ModelResources models,
            LightingResources lighting,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups,
            bool transparentPass,
            bool selectionColors
        ) {
            if (models?.ModelInstances == null)
                return;

            var shader = selectionColors ? general.ColorizeShader : general.ObjectShader;
            if (selectionColors)
                shader.UpdateUniform("alwaysShow", false);
            else {
                shader.UpdateUniform(ShaderUniformType.LightingMode, options.ApplyLighting ? 1 : 0);
                shader.UpdateUniform(ShaderUniformType.SmoothLighting, options.SmoothLighting);
            }

            Vector4 ModelSelectionColor(IMPD_ModelInstance model) {
                var r = (model.ID % 64) / 64.0f;
                var g = (model.ID / 64) / 64.0f;
                return new Vector4(r, g, model.Collection.Collection == MPD_CollectionType.Primary ? c_selectionPrimaryModelsB : c_selectionExtraModelsB, 1.0f);
            }

            var lightingTexture = selectionColors ? null : (lighting.LightingTexture ?? general.WhiteTexture);
            var usedSolidShader = false;

            using (selectionColors ? null : general.TransparentBlackTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (selectionColors ? null : general.TransparentBlackTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (selectionColors ? null : lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (selectionColors ? general.TransparentBlackTexture.Use() : null)
            using (shader.Use()) {
                if (!transparentPass) {
                    if (!selectionColors) {
                        // Pass 1: Stencil
                        var modelsWithStencils = modelsWithGroups.Where(x => x.ModelGroup.HideModel != null).ToArray();
                        if (modelsWithStencils.Length > 0) {
                            usedSolidShader = true;
                            GL.ColorMask(false, false, false, false);
                            using (general.SolidShader.Use()) {
                                foreach (var mwg in modelsWithStencils) {
                                    SetModelAndNormalMatricesForModel(models, mwg.Model, general.SolidShader, options, cameraYaw, cameraPitch);
                                    mwg.ModelGroup.HideModel.Draw(general.SolidShader, null);
                                }
                            }
                            GL.ColorMask(true, true, true, true);
                        }
                    }

                    // Pass 2: Textured models
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidTexturedModel != null).ToArray()) {
                        if (selectionColors)
                            shader.UpdateUniform("color", ModelSelectionColor(mwg.Model));
                        SetModelAndNormalMatricesForModel(models, mwg.Model, shader, options, cameraYaw, cameraPitch);
                        mwg.ModelGroup.SolidTexturedModel.Draw(shader);
                    }

                    // Pass 3: Untextured models
                    using (selectionColors ? general.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas) : null) {
                        foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidUntexturedModel != null).ToArray()) {
                            if (selectionColors)
                                shader.UpdateUniform("color", ModelSelectionColor(mwg.Model));
                            SetModelAndNormalMatricesForModel(models, mwg.Model, shader, options, cameraYaw, cameraPitch);
                            mwg.ModelGroup.SolidUntexturedModel.Draw(shader);
                        }
                    }
                }
                else {
                    // Draw semi-transparent shaders now. Don't write to the depth buffer.
                    GL.DepthMask(false);

                    // Pass 3: Semi-transparent textured models
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentTexturedModel != null).ToArray()) {
                        if (selectionColors)
                            shader.UpdateUniform("color", ModelSelectionColor(mwg.Model));
                        SetModelAndNormalMatricesForModel(models, mwg.Model, shader, options, cameraYaw, cameraPitch);
                        mwg.ModelGroup.SemiTransparentTexturedModel.Draw(shader);
                    }

                    // Pass 4: Semi-transparent untextured models
                    using (general.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas)) {
                        foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentUntexturedModel != null).ToArray()) {
                            if (selectionColors)
                                shader.UpdateUniform("color", ModelSelectionColor(mwg.Model));
                            SetModelAndNormalMatricesForModel(models, mwg.Model, shader, options, cameraYaw, cameraPitch);
                            mwg.ModelGroup.SemiTransparentUntexturedModel.Draw(shader, null);
                        }
                    }

                    // Done drawing semi-transparent shaders now. Write to the depth buffer again.
                    GL.DepthMask(true);
                }
            }

            // Reset model matrices to their identity.
            shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);

            if (usedSolidShader) {
                general.SolidShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                general.SolidShader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }
        }

        public void DrawSceneSurfaceModel(
            GeneralResources general,
            SurfaceModelResources surfaceModel,
            LightingResources lighting,
            RendererOptions options
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

            using (terrainTypesTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (eventIdsTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (options.DrawSurfaceModel ? null : general.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
            using (general.ObjectShader.Use()) {
                foreach (var block in surfaceModel.Blocks) {
                    if (options.DrawSurfaceModel)
                        block.Model?.Draw(general.ObjectShader);
                    else
                        block.Model?.Draw(general.ObjectShader, null);

                    if (block.MissingTexturesModel != null)
                        using (general.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas))
                            block.MissingTexturesModel?.Draw(general.ObjectShader, null);

                    if (block.UntexturedModel != null)
                        using (general.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
                            block.UntexturedModel?.Draw(general.ObjectShader, null);
                }
            }

            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawActors(
            GeneralResources general,
            SceneResources scene,
            float cameraYaw,
            float cameraPitch,
            bool selectionColors
        ) {
            if (scene == null || scene.ModelsBySpriteID == null || scene.ModelsBySpriteID.Count == 0)
                return;

            Vector4 ModelSelectionColor(SceneResources.ActorModelInstance actor) {
                var r = (actor.ID % 64) / 64.0f;
                var g = (actor.ID / 64) / 64.0f;
                return new Vector4(r, g, c_selectionActorsB, 1.0f);
            }

            var (baseMatrix, baseRotationMatrix) = GetSpriteDrawMatrices(cameraYaw, cameraPitch);

            var shader = general.SpriteShader;
            using (shader.Use())
            using (scene.ActorTextureAtlas.Use()) {
                _ = shader.UpdateUniform("colorize", selectionColors);

                if (!selectionColors) {
                    // Render shadows first.
                    _ = shader.UpdateUniform("direction", 0.0f);
                    _ = shader.UpdateUniform("cameraDistAdjust", 0.25f);
                    foreach (var actorGroup in scene.ActorsBySpriteID) {
                        var spriteId = actorGroup.Key;
                        var shadow = scene.ShadowsBySpriteID[spriteId];

                        foreach (var actor in actorGroup.Value) {
                            var modelMatrix = baseMatrix * Matrix4.CreateTranslation(new Vector3(actor.X, actor.Y, actor.Z));
                            _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix);
                            shadow.Draw(shader);
                        }
                    }
                }

                // Now render sprites.
                _ = shader.UpdateUniform("cameraDistAdjust", 0.5f);
                foreach (var actorGroup in scene.ActorsBySpriteID) {
                    var spriteId = actorGroup.Key;
                    var model    = scene.ModelsBySpriteID[spriteId];

                    foreach (var actor in actorGroup.Value) {
                        SetupSpriteShaderUniforms(shader, baseRotationMatrix, actor, cameraYaw, selectionColors ? ModelSelectionColor(actor) : null);
                        model.Draw(shader);
                    }
                }
                _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            }
        }

        public void DrawZones(
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

        private void SetupSpriteShaderUniforms(Shader shader, Matrix4 rotationMatrix, SceneResources.ActorModelInstance actor, float cameraYaw, Vector4? color) {
            var modelMatrix = rotationMatrix * Matrix4.CreateTranslation(new Vector3(actor.X, actor.Y + actor.VerticalOffset, actor.Z));
            _ = shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix);

            // Convert facing direction (0=north, 90=east, ...) to shader direction (0=south, 0.25=east, ...)
            _ = shader.UpdateUniform("direction", MathHelpers.ActualMod((180.0f - actor.Direction - cameraYaw) / 360.0f, 1.0f));

            if (color.HasValue)
                _ = shader.UpdateUniform("color", color.Value);
        }

        private (Matrix4 BaseMatrix, Matrix4 BaseRotationMatrix) GetSpriteDrawMatrices(float cameraYaw, float cameraPitch) {
            var baseMatrix = Matrix4.CreateScale(0.75f);

            var baseRotationMatrix = baseMatrix *
                Matrix4.CreateRotationX(cameraPitch / 180.0f * (float) Math.PI) *
                Matrix4.CreateRotationY(cameraYaw   / 180.0f * (float) Math.PI);

            return (baseMatrix, baseRotationMatrix);
        }

        public void DrawSceneGradient(
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

        public void DrawSceneModelsWireframe(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (IMPD_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            if (models?.ModelInstances == null)
                return;

            foreach (var mwg in modelsWithGroups) {
                SetModelAndNormalMatricesForModel(models, mwg.Model, general.WireframeShader, options, cameraYaw, cameraPitch);
                mwg.ModelGroup.SolidTexturedModel?.Draw(general.WireframeShader);
                mwg.ModelGroup.SolidUntexturedModel?.Draw(general.WireframeShader);
                mwg.ModelGroup.SemiTransparentTexturedModel?.Draw(general.WireframeShader);
                mwg.ModelGroup.SemiTransparentUntexturedModel?.Draw(general.WireframeShader);
            }

            general.WireframeShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
        }

        public void DrawSceneSurfaceModelWireframe(GeneralResources general, SurfaceModelResources surfaceModel) {
            if (surfaceModel?.Blocks == null)
                return;

            foreach (var block in surfaceModel.Blocks) {
                block.UntexturedModel?.Draw(general.WireframeShader, null);
                block.MissingTexturesModel?.Draw(general.WireframeShader, null);
                block.Model?.Draw(general.WireframeShader, null);
            }
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

                    SetModelAndNormalMatricesForModel(models, mwg.Model, general.ColorizeShader, options, cameraYaw, cameraPitch);
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
                    var (baseMatrix, baseRotationMatrix) = GetSpriteDrawMatrices(cameraYaw, cameraPitch);

                    var shader = general.SpriteShader;
                    using (scene.ActorTextureAtlas.Use()) {
                        _ = shader.UpdateUniform("colorize", true);
                        _ = shader.UpdateUniform("cameraDistAdjust", 0.5f);

                        SetupSpriteShaderUniforms(shader, baseRotationMatrix, actor, cameraYaw, color);
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
                RenderActor(mouseoverActor, new Vector4(0.25f, 0.25f + (0.25f / 4), 0.5f, 0.5f));
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

        private Dictionary<IMPD_ModelInstance, Matrix4?> _modelMatricesByModel = [];
        private Dictionary<IMPD_ModelInstance, Matrix3?> _normalMatricesByModel = [];

        public void InvalidateModelMatrices() {
            _normalMatricesByModel.Clear();
            _modelMatricesByModel.Clear();
        }

        public void InvalidateSpriteMatrices(ModelResources models) {
            if (models?.ModelInstances == null)
                return;
            var spriteModels = models.ModelInstances.Where(x => x.AlwaysFacesCamera).ToList();
            foreach (var sm in spriteModels) {
                _modelMatricesByModel.Remove(sm);
                _normalMatricesByModel.Remove(sm);
            }
        }

        private void SetModelAndNormalMatricesForModel(
            ModelResources models,
            IMPD_ModelInstance modelInstance,
            Shader shader,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            var modelMatrix = _modelMatricesByModel.TryGetValue(modelInstance, out var modelMatrixValue) ? modelMatrixValue : null;
            if (!modelMatrix.HasValue) {
                var angleXAdjust = 0.00f;
                var angleYAdjust = modelInstance.AlwaysFacesCamera ? (float) ((cameraYaw + 180.0f) / 180.0f * Math.PI) : 0.00f;
                var scaleAdjust  = 1.00f;

                var yAdjust = 0.00f;
                var prePostAdjustY = 0.00f;

                if (modelInstance.AlwaysFacesCamera && options.RotateSpritesUp) {
                    // Not all sprites rotate around the X axis the same way, so get the center X to help with offsets.
                    var mpdModel = models.MPD_ModelsByIDByCollection[modelInstance.Collection.Collection].TryGetValue(modelInstance.ModelID, out var mpdModelOut) ? mpdModelOut : null;

                    var topY     = mpdModel.Vertices?.Min(x => Math.Min(x.Y.Float, x.Z.Float)) / 32.0f ?? 0.00f;
                    var bottomY  = mpdModel.Vertices?.Max(x => Math.Max(x.Y.Float, x.Z.Float)) / 32.0f ?? 0.00f;
                    var centerY  = (topY + bottomY) * 0.5f;

                    angleXAdjust = (float) (cameraPitch / 180.0f * Math.PI) * -1.00f;

                    scaleAdjust = 1.00f - (float) Math.Sin(Math.Abs(angleXAdjust)) * 0.25f;
                    prePostAdjustY = centerY * scaleAdjust;
                }

                var newModelMatrix =
                    Matrix4.CreateScale(modelInstance.ScaleX * scaleAdjust, modelInstance.ScaleY * scaleAdjust, modelInstance.ScaleZ * scaleAdjust) *
                    Matrix4.CreateRotationX(modelInstance.AngleX * (float) Math.PI / -180.00f) *
                    Matrix4.CreateTranslation(0, prePostAdjustY, 0) *
                    Matrix4.CreateRotationX(angleXAdjust) *
                    Matrix4.CreateRotationY(modelInstance.AngleY * (float) Math.PI / -180.00f + angleYAdjust) *
                    Matrix4.CreateRotationZ(modelInstance.AngleZ * (float) Math.PI / 180.00f) *
                    Matrix4.CreateRotationY((float) Math.PI) *
                    Matrix4.CreateTranslation(modelInstance.PositionX / 32.0f, modelInstance.PositionY / -32.0f - prePostAdjustY + yAdjust, modelInstance.PositionZ / -32.0f) *
                    Matrix4.CreateRotationY(options.ModelsYRotation * (float) Math.PI / -180.00f) *
                    Matrix4.CreateTranslation(-32.0f, 0, 32.0f);

                _modelMatricesByModel[modelInstance] = newModelMatrix;
                modelMatrix = newModelMatrix;
            }

            var normalMatrix = _normalMatricesByModel.TryGetValue(modelInstance, out var normalMatrixValue) ? normalMatrixValue : null;
            if (!normalMatrix.HasValue) {
                var newNormalMatrix = new Matrix3(modelMatrix.Value).Inverted();
                newNormalMatrix.Transpose();

                _normalMatricesByModel[modelInstance] = newNormalMatrix;
                normalMatrix = newNormalMatrix;
            }

            shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix.Value);
            shader.UpdateUniform(ShaderUniformType.NormalMatrix, normalMatrix.Value);
        }

        public void DrawControlFocusedBox(RendererResources resources, RendererOptions options, RendererState state) {
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            using (resources.General.ColorToScreenShader.Use())
                resources.Screen.FocusedBox.Draw(resources.General.ColorToScreenShader);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
    }
}
