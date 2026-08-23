using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Types;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.Types;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class ModelRenderer {
        public void Draw(
            GeneralResources general,
            ModelResources models,
            LightingResources lighting,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups,
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

            Vector4 ModelSelectionColor(ISGL_ModelInstance model) {
                var r = model.ModelInstanceID % 64 / 64.0f;
                var g = model.ModelInstanceID / 64 / 64.0f;
                return new Vector4(r, g, model.ModelCollectionID == (int) MPD_CollectionType.Primary ? RendererSelectionConstants.PrimaryModelsB : RendererSelectionConstants.ExtraModelsB, 1.0f);
            }

            var lightingTexture = selectionColors ? null : lighting.LightingTexture ?? general.WhiteTexture;
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

        public void DrawWireframe(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
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

        public void DrawNormals(
            GeneralResources general,
            ModelResources models,
            RendererOptions options,
            float cameraYaw,
            float cameraPitch,
            (ISGL_ModelInstance Model, ModelGroup ModelGroup)[] modelsWithGroups
        ) {
            if (models?.ModelGroupsByIDByCollection == null)
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

        public void SetModelAndNormalMatricesForModel(
            ModelResources models,
            ISGL_ModelInstance modelInstance,
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
                    var mpdModel = models.SGL_ModelsByIDByCollection[modelInstance.ModelCollectionID].TryGetValue(modelInstance.ModelID, out var mpdModelOut) ? mpdModelOut : null;

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

        private Dictionary<ISGL_ModelInstance, Matrix4?> _modelMatricesByModel = [];
        private Dictionary<ISGL_ModelInstance, Matrix3?> _normalMatricesByModel = [];
    }
}
