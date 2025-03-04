﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.MPD_File {
    public class Renderer {
        public class RendererOptions {
            public bool DrawModels;
            public bool DrawSurfaceModel;
            public bool DrawGround;
            public bool DrawSkyBox;
            public bool DrawGradients;

            public bool DrawNormals;
            public bool DrawWireframe;

            public bool DrawTerrainTypes;
            public bool DrawEventIDs;
            public bool DrawBoundaries;

            public bool DrawHelp;

            public bool ApplyLighting;
            public bool UseOutsideLighting;
            public bool RotateSpritesUp;

            public bool WillDrawSurfaceModel
                => DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs;
            public bool WillDrawAnyObjects
                => DrawModels || WillDrawSurfaceModel;
            public bool WillDrawSurfaceModelWireframe
                => DrawWireframe && !DrawNormals && (DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs);
        }

        public void DrawScene(
            WorldResources world,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            GroundModelResources groundModel,
            SkyBoxModelResources skyBoxModel,
            GradientResources gradients,
            LightAdjustmentModel lightAdj,
            LightingResources lighting,
            BoundaryModelResources boundaryModels,
            SurfaceEditorResources surfaceEditor,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            int screenWidth,
            int screenHeight,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            GL.StencilMask(0xFF);

            // Enable 'CullFace' to draw single-sided, and 'StencilTest' for gradients on various render passes.
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            if (options.DrawSkyBox)
                DrawSceneSkyBox(world, skyBoxModel, gradients, options, cameraYaw, cameraPitch, ref projectionMatrix, ref viewMatrix);
            if (options.DrawGround)
                DrawSceneGround(world, groundModel, gradients, lightAdj, options, ref projectionMatrix, ref viewMatrix);

            if (options.DrawNormals)
                DrawSceneObjectNormals(world, models, surfaceModel, options, cameraYaw, cameraPitch);
            else if (options.WillDrawAnyObjects)
                DrawSceneObjects(world, models, surfaceModel, gradients, lighting, options, cameraYaw, cameraPitch, ref projectionMatrix, ref viewMatrix);

            // Done rendering gradients; disable the stencil test.
            GL.Disable(EnableCap.StencilTest);

            if (options.DrawWireframe)
                DrawSceneWireframes(world, models, surfaceModel, options, cameraYaw, cameraPitch);

            // Done rendering the real scene; disable one-sided rendering.
            GL.Disable(EnableCap.CullFace);

            if (options.DrawBoundaries)
                DrawSceneBoundaries(world, boundaryModels);

            DrawEditorElements(world, surfaceEditor, options, screenWidth, screenHeight, ref projectionMatrix, ref viewMatrix);
        }

        public void DrawSceneObjectNormals(
            WorldResources world,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            if (options.DrawModels)
                DrawSceneModelsNormals(world, models, options, cameraYaw, cameraPitch);

            if (options.DrawSurfaceModel)
                DrawSceneSurfaceModelNormals(world, surfaceModel);
        }

        public void DrawSceneObjects(
            WorldResources world,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            GradientResources gradients,
            LightingResources lighting,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            GL.StencilFunc(StencilFunction.Always, 4, 0x04);
            GL.StencilMask(0x04);

            if (options.DrawModels)
                DrawSceneModels(world, models, lighting, options, cameraYaw, cameraPitch);

            if (options.WillDrawSurfaceModel)
                DrawSceneSurfaceModel(world, surfaceModel, lighting, options);

            if (options.DrawGradients)
                DrawSceneGradient(world, gradients?.ModelsGradientModel, 0x04, true, ref projectionMatrix, ref viewMatrix);
        }

        public void DrawSceneWireframes(
            WorldResources world,
            ModelResources models,
            SurfaceModelResources surfaceModel,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-2.0f, -2.0f);

            using (world.WireframeShader.Use())
            using (world.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                if (options.DrawModels)
                    DrawSceneModelsWireframe(world, models, options, cameraYaw, cameraPitch);

                if (options.WillDrawSurfaceModelWireframe)
                    DrawSceneSurfaceModelWireframe(world, surfaceModel);
            }

            world.WireframeShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public void DrawSceneBoundaries(WorldResources world, BoundaryModelResources boundaryModels) {
            if (boundaryModels.CameraBoundaryModel == null && boundaryModels.BattleBoundaryModel == null)
                return;

            using (world.SolidShader.Use()) {
                GL.Disable(EnableCap.DepthTest);
                boundaryModels.BattleBoundaryModel?.Draw(world.SolidShader, null);
                boundaryModels.CameraBoundaryModel?.Draw(world.SolidShader, null);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        public void DrawEditorElements(
            WorldResources world,
            SurfaceEditorResources surfaceEditor,
            RendererOptions options,
            int screenWidth,
            int screenHeight,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            using (world.TextureShader.Use()) {
                DrawSurfaceEditorTileSelected(world, surfaceEditor);
                DrawSurfaceEditorTileHover(world, surfaceEditor);

                if (options.DrawHelp)
                    DrawEditorHelp(world, surfaceEditor, screenWidth, screenHeight, ref projectionMatrix, ref viewMatrix);
            }
        }

        public void DrawSceneModelsNormals(
            WorldResources world,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            if (models?.ModelsByMemoryAddress == null)
                return;

            var modelsWithGroups = models.Models
                .Select(x => new { Model = x, ModelGroup = models.ModelsByMemoryAddress.TryGetValue(x.PData1, out ModelGroup pd) ? pd : null })
                .Where(x => x.ModelGroup != null)
                .ToArray();

            foreach (var mwg in modelsWithGroups) {
                SetModelAndNormalMatricesForModel(models, mwg.Model, world.NormalsShader, options, cameraYaw, cameraPitch);
                mwg.ModelGroup.SolidTexturedModel?.Draw(world.NormalsShader, null);
                mwg.ModelGroup.SolidUntexturedModel?.Draw(world.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentTexturedModel?.Draw(world.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentUntexturedModel?.Draw(world.NormalsShader, null);
            }

            // Reset model matrices to their identity.
            world.NormalsShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            world.NormalsShader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
        }

        public void DrawSceneSurfaceModelNormals(WorldResources world, SurfaceModelResources surfaceModel) {
            if (!(surfaceModel?.Blocks?.Length > 0))
                return;

            using (world.NormalsShader.Use()) {
                foreach (var block in surfaceModel.Blocks) {
                    if (block.Model != null || block.UntexturedModel != null) {
                        block.Model?.Draw(world.NormalsShader, null);
                        block.UntexturedModel?.Draw(world.NormalsShader, null);
                    }
                }
            }
        }

        public void DrawSceneSkyBox(
            WorldResources world,
            SkyBoxModelResources skyBoxModel,
            GradientResources gradients,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (!(skyBoxModel.Model != null))
                return;

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            world.TextureShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, Matrix4.Identity);

            const float c_repeatCount = 8f;
            var xOffset = (MathHelpers.ActualMod((cameraYaw / 360f) * c_repeatCount, 1.0f) - 0.5f) * 2.0f;
            var yOffset = (float) Math.Sin(-cameraPitch / 360.0f) * 32f + 0.975f;

            world.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix, Matrix4.Identity * Matrix4.CreateTranslation(xOffset, yOffset, 0));

            GL.StencilFunc(StencilFunction.Always, 2, 0x02);
            GL.StencilMask(0x02);

            using (skyBoxModel.Texture.Use(TextureUnit.Texture0))
                skyBoxModel.Model.Draw(world.TextureShader, null);

            if (options.DrawGradients)
                DrawSceneGradient(world, gradients?.SkyBoxGradientModel, 0x02, false, ref projectionMatrix, ref viewMatrix);

            world.TextureShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref projectionMatrix);
            world.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawSceneGround(
            WorldResources world,
            GroundModelResources groundModel,
            GradientResources gradients,
            LightAdjustmentModel lightAdj,
            RendererOptions options,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (groundModel.Model == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            Vector3 glow = Vector3.Zero;
            if (options.ApplyLighting && lightAdj != null) {
                glow = new Vector3(
                    lightAdj.GroundRAdjustment / (float) 0x1F, 
                    lightAdj.GroundGAdjustment / (float) 0x1F, 
                    lightAdj.GroundBAdjustment / (float) 0x1F
                );
            }
            world.TextureShader.UpdateUniform(ShaderUniformType.GlobalGlow, ref glow);

            GL.StencilFunc(StencilFunction.Always, 1, 0x01);
            GL.StencilMask(0x01);

            using (groundModel.Texture.Use(TextureUnit.Texture0))
                groundModel.Model.Draw(world.TextureShader, null);

            if (options.DrawGradients)
                DrawSceneGradient(world, gradients?.GroundGradientModel, 0x01, false, ref projectionMatrix, ref viewMatrix);

            world.TextureShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawSceneModels(
            WorldResources world,
            ModelResources models,
            LightingResources lighting,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            if (models?.Models == null)
                return;

            world.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, options.ApplyLighting ? 1 : 0);
            var lightingTexture = lighting.LightingTexture ?? world.WhiteTexture;

            using (world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (world.ObjectShader.Use()) {
                var modelsWithGroups = models.Models
                    .Select(x => new { Model = x, ModelGroup = models.ModelsByMemoryAddress.TryGetValue(x.PData1, out ModelGroup pd) ? pd : null })
                    .Where(x => x.ModelGroup != null)
                    .ToArray();

                // Pass 1: Textured models
                foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidTexturedModel != null).ToArray()) {
                    SetModelAndNormalMatricesForModel(models, mwg.Model, world.ObjectShader, options, cameraYaw, cameraPitch);
                    mwg.ModelGroup.SolidTexturedModel.Draw(world.ObjectShader);
                }

                // Pass 2: Untextured models
                using (world.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas)) {
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidUntexturedModel != null).ToArray()) {
                        SetModelAndNormalMatricesForModel(models, mwg.Model, world.ObjectShader, options, cameraYaw, cameraPitch);
                        mwg.ModelGroup.SolidUntexturedModel.Draw(world.ObjectShader, null);
                    }
                }

                // Draw semi-transparent shaders now. Don't write to the depth buffer.
                GL.DepthMask(false);

                // Pass 3: Semi-transparent textured models
                foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentTexturedModel != null).ToArray()) {
                    SetModelAndNormalMatricesForModel(models, mwg.Model, world.ObjectShader, options, cameraYaw, cameraPitch);
                    mwg.ModelGroup.SemiTransparentTexturedModel.Draw(world.ObjectShader);
                }

                // Pass 4: Semi-transparent untextured models
                using (world.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas)) {
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentUntexturedModel != null).ToArray()) {
                        SetModelAndNormalMatricesForModel(models, mwg.Model, world.ObjectShader, options, cameraYaw, cameraPitch);
                        mwg.ModelGroup.SemiTransparentUntexturedModel.Draw(world.ObjectShader, null);
                    }
                }

                // Done drawing semi-transparent shaders now. Write to the depth buffer again.
                GL.DepthMask(true);
            }

            // Reset model matrices to their identity.
            world.ObjectShader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
            world.ObjectShader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
        }

        public void DrawSceneSurfaceModel(
            WorldResources world,
            SurfaceModelResources surfaceModel,
            LightingResources lighting,
            RendererOptions options
        ) {
            if (!(surfaceModel?.Blocks?.Length > 0))
                return;

            var terrainTypesTexture = options.DrawTerrainTypes ? surfaceModel.TerrainTypesTexture : world.TransparentBlackTexture;
            var eventIdsTexture     = options.DrawEventIDs     ? surfaceModel.EventIDsTexture     : world.TransparentBlackTexture;
            var lightingTexture     = lighting.LightingTexture ?? world.WhiteTexture;

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-1.0f, -1.0f);

            if (options.UseOutsideLighting)
                world.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, options.ApplyLighting ? 2 : 0);

            using (terrainTypesTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (eventIdsTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (options.DrawSurfaceModel ? null : world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
            using (world.ObjectShader.Use()) {
                foreach (var block in surfaceModel.Blocks) {
                    if (options.DrawSurfaceModel)
                        block.Model?.Draw(world.ObjectShader);
                    else
                        block.Model?.Draw(world.ObjectShader, null);

                    using (world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
                        block.UntexturedModel?.Draw(world.ObjectShader, null);
                }
            }

            if (options.UseOutsideLighting)
                world.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, options.ApplyLighting ? 1 : 0);

            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSceneGradient(
            WorldResources world,
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

            world.SolidShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, Matrix4.Identity);
            world.SolidShader.UpdateUniform(ShaderUniformType.ViewMatrix, Matrix4.Identity);

            GL.StencilFunc(StencilFunction.Equal, stencilBit, stencilBit);
            gradientModel.Draw(world.SolidShader, null);
    
            world.SolidShader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref projectionMatrix);
            world.SolidShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);

            if (depthCurrentlyEnabled) {
                GL.DepthMask(true);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        public void DrawSceneModelsWireframe(
            WorldResources world,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            if (models.Models == null)
                return;

            foreach (var model in models.Models) {
                var modelGroup = models.ModelsByMemoryAddress.TryGetValue(model.PData1, out ModelGroup pd) ? pd : null;
                if (modelGroup != null) {
                    SetModelAndNormalMatricesForModel(models, model, world.WireframeShader, options, cameraYaw, cameraPitch);
                    modelGroup.SolidTexturedModel?.Draw(world.WireframeShader);
                    modelGroup.SolidUntexturedModel?.Draw(world.WireframeShader);
                    modelGroup.SemiTransparentTexturedModel?.Draw(world.WireframeShader);
                    modelGroup.SemiTransparentUntexturedModel?.Draw(world.WireframeShader);
                }
            }
        }

        public void DrawSceneSurfaceModelWireframe(WorldResources world, SurfaceModelResources surfaceModel) {
            foreach (var block in surfaceModel.Blocks) {
                block.UntexturedModel?.Draw(world.WireframeShader, null);
                block.Model?.Draw(world.WireframeShader, null);
            }
        }

        public void DrawSurfaceEditorTileSelected(WorldResources world, SurfaceEditorResources surfaceEditor) {
            if (surfaceEditor.TileSelectedModel == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            using (surfaceEditor.TileSelectedTexture.Use())
                surfaceEditor.TileSelectedModel.Draw(world.TextureShader);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawSurfaceEditorTileHover(WorldResources world, SurfaceEditorResources surfaceEditor) {
            if (surfaceEditor.TileHoverModel == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            using (surfaceEditor.TileHoverTexture.Use())
                surfaceEditor.TileHoverModel.Draw(world.TextureShader);
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawEditorHelp(
            WorldResources world,
            SurfaceEditorResources surfaceEditor,
            int screenWidth,
            int screenHeight,
            ref Matrix4 projectionMatrix,
            ref Matrix4 viewMatrix
        ) {
            if (surfaceEditor.HelpModel == null)
                return;

            const float c_viewSize = 0.40f;

            var viewportRatio = (float) screenWidth / screenHeight;
            var textureRatio = (float) surfaceEditor.HelpTexture.Width / surfaceEditor.HelpTexture.Height;

            world.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix,
                Matrix4.CreateScale(2f / viewportRatio * textureRatio * c_viewSize, 2f * c_viewSize, 2f * c_viewSize) *
                Matrix4.CreateTranslation(1, -1, 0) *
                projectionMatrix.Inverted());

            GL.Disable(EnableCap.DepthTest);
            using (surfaceEditor.HelpTexture.Use())
                surfaceEditor.HelpModel.Draw(world.TextureShader);
            GL.Enable(EnableCap.DepthTest);

            world.TextureShader.UpdateUniform(ShaderUniformType.ViewMatrix, ref viewMatrix);
        }


        private Dictionary<Model, Matrix4?> _modelMatricesByModel = [];
        private Dictionary<Model, Matrix3?> _normalMatricesByModel = [];

        public void InvalidateModelMatrices() {
            _normalMatricesByModel.Clear();
            _modelMatricesByModel.Clear();
        }

        public void InvalidateSpriteMatrices(ModelResources models) {
            if (models?.Models == null)
                return;
            var spriteModels = models.Models.Where(x => x.AlwaysFacesCamera).ToList();
            foreach (var sm in spriteModels) {
                _modelMatricesByModel.Remove(sm);
                _normalMatricesByModel.Remove(sm);
            }
        }

        private void SetModelAndNormalMatricesForModel(
            ModelResources models,
            Model model,
            Shader shader,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch
        ) {
            var modelMatrix = _modelMatricesByModel.TryGetValue(model, out var modelMatrixValue) ? modelMatrixValue : null;
            if (!modelMatrix.HasValue) {
                var angleXAdjust = 0.00f;
                var angleYAdjust = model.AlwaysFacesCamera ? (float) ((cameraYaw + 180.0f) / 180.0f * Math.PI) : 0.00f;
                var scaleAdjust  = 1.00f;

                var yAdjust = 0.00f;
                var prePostAdjustY = 0.00f;

                if (model.AlwaysFacesCamera && options.RotateSpritesUp) {
                    // Not all sprites rotate around the X axis the same way, so get the center X to help with offsets.
                    var pdata    = models.PDatasByAddress.TryGetValue(model.PData1, out var pdataValue) ? pdataValue : null;
                    var vertices = (pdata == null) ? null : models.VerticesByAddress.TryGetValue(pdata.VerticesOffset, out var verticesValue) ? verticesValue : null;

                    var topY     = vertices?.Min(x => Math.Min(x.Vector.Y.Float, x.Vector.Z.Float)) / 32.0f ?? 0.00f;
                    var bottomY  = vertices?.Max(x => Math.Max(x.Vector.Y.Float, x.Vector.Z.Float)) / 32.0f ?? 0.00f;
                    var centerY  = (topY + bottomY) * 0.5f;

                    angleXAdjust = (float) (cameraPitch / 180.0f * Math.PI) * -1.00f;

                    scaleAdjust = 1.00f - (float) Math.Sin(Math.Abs(angleXAdjust)) * 0.25f;
                    prePostAdjustY = centerY * scaleAdjust;
                }

                var newModelMatrix =
                    Matrix4.CreateScale(model.ScaleX * scaleAdjust, model.ScaleY * scaleAdjust, model.ScaleZ * scaleAdjust) *
                    Matrix4.CreateRotationX(model.AngleX * (float) Math.PI * -2.00f) *
                    Matrix4.CreateTranslation(0, prePostAdjustY, 0) *
                    Matrix4.CreateRotationX(angleXAdjust) *
                    Matrix4.CreateRotationY(model.AngleY * (float) Math.PI * -2.00f + angleYAdjust) *
                    Matrix4.CreateRotationZ(model.AngleZ * (float) Math.PI * 2.00f) *
                    Matrix4.CreateTranslation(model.PositionX / -32.0f - 32.0f, model.PositionY / -32.0f - prePostAdjustY + yAdjust, model.PositionZ / 32.0f + 32.0f);

                _modelMatricesByModel[model] = newModelMatrix;
                modelMatrix = newModelMatrix;
            }

            var normalMatrix = _normalMatricesByModel.TryGetValue(model, out var normalMatrixValue) ? normalMatrixValue : null;
            if (!normalMatrix.HasValue) {
                var newNormalMatrix = new Matrix3(modelMatrix.Value).Inverted();
                newNormalMatrix.Transpose();

                _normalMatricesByModel[model] = newNormalMatrix;
                normalMatrix = newNormalMatrix;
            }

            shader.UpdateUniform(ShaderUniformType.ModelMatrix, modelMatrix.Value);
            shader.UpdateUniform(ShaderUniformType.NormalMatrix, normalMatrix.Value);
        }
    }
}
