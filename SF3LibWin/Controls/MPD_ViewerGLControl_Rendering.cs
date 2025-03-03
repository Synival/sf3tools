using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;
using SF3.Win.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitRendering() {
            Load      += (s, e) => OnLoadRendering();
            Disposed  += (s, e) => OnDisposeRendering();
            Resize    += (s, e) => OnResizeRendering();
            Paint     += (s, e) => OnPaintRendering();
            FrameTick += (s, deltaInMs) => OnFrameTickRendering(deltaInMs);
            TileModified += (s, e) => OnTileModifiedRendering(s);
        }

        public void UpdateLightingTexture() {
            MakeCurrent();
            using (var textureBitmap = CreateLightPaletteBitmap())
                _surfaceModel.SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, clampToEdge: false) : null);
        }

        public void UpdateLightPosition() {
            MakeCurrent();
            var lightPos = GetLightPosition();
            foreach (var shader in _world.Shaders) {
                using (shader.Use())
                    UpdateLightPosition(shader, lightPos);
            }
        }

        private Bitmap CreateLightPaletteBitmap() {
            var lightPal = MPD_File?.LightPalette;
            if (lightPal == null)
                return null;

            var lightAdjustment = MPD_File.LightAdjustment;
            var adjR = lightAdjustment?.RAdjustment ?? 0;
            var adjG = lightAdjustment?.GAdjustment ?? 0;
            var adjB = lightAdjustment?.BAdjustment ?? 0;

            var numColors = lightPal.Length;

            var colorData = new byte[numColors * 4];
            int pos = 0;
            foreach (var color in lightPal) {
                var colorValue = color.ColorABGR1555;
                var colorR = MathHelpers.Clamp((short) ((colorValue >>  0) & 0x1F) + adjR, 0x00, 0x1F);
                var colorG = MathHelpers.Clamp((short) ((colorValue >>  5) & 0x1F) + adjG, 0x00, 0x1F);
                var colorB = MathHelpers.Clamp((short) ((colorValue >> 10) & 0x1F) + adjB, 0x00, 0x1F);

                colorData[pos++] = (byte) (colorB * 255 / 31);
                colorData[pos++] = (byte) (colorG * 255 / 31);
                colorData[pos++] = (byte) (colorR * 255 / 31);
                colorData[pos++] = 255;
            }

            var textureBitmap = new Bitmap(1, numColors, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bitmapData = textureBitmap.LockBits(new Rectangle(0, 0, 1, numColors), ImageLockMode.WriteOnly, textureBitmap.PixelFormat);
            Marshal.Copy(colorData, 0, bitmapData.Scan0, colorData.Length);
            textureBitmap.UnlockBits(bitmapData);

            return textureBitmap;
        }

        private void OnLoadRendering() {
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.Max);

            _world         = new WorldResources();
            _models        = new ModelResources();
            _surfaceModel  = new SurfaceModelResources();
            _groundModel   = new GroundModelResources();
            _skyBoxModel   = new SkyBoxModelResources();
            _surfaceEditor = new SurfaceEditorResources();
            _gradients     = new GradientResources();

            _world.Init();
            _surfaceModel.Init();
            _surfaceEditor.Init();
            _groundModel.Init();

            SetInitialCameraPosition();
            UpdateSelectFramebuffer();

            UpdateLightingTexture();
            UpdateLightPosition();
            UpdateLightingMode(_world.ObjectShader, false);
            UpdateGroundGlow(_world.TextureShader, Vector3.Zero);

            foreach (var shader in _world.Shaders) {
                UpdateShaderModelMatrix(shader, Matrix4.Identity);
                UpdateShaderNormalMatrix(shader, Matrix3.Identity);
            }
        }

        private void OnDisposeRendering() {
            _world?.Dispose();
            _models?.Dispose();
            _surfaceModel?.Dispose();
            _groundModel?.Dispose();
            _skyBoxModel?.Dispose();
            _surfaceEditor?.Dispose();
            _gradients?.Dispose();

            _selectFramebuffer?.Dispose();

            _world             = null;
            _models            = null;
            _surfaceModel      = null;
            _groundModel       = null;
            _skyBoxModel       = null;
            _surfaceEditor     = null;
            _gradients         = null;

            _selectFramebuffer = null;
        }

        private void OnResizeRendering() {
            MakeCurrent();

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            UpdateSelectFramebuffer();
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void OnPaintRendering() {
            MakeCurrent();

            foreach (var block in _surfaceModel.Blocks)
                if (block.NeedsUpdate)
                    block.Update(MPD_File);

            if (_tileSelectedNeedsUpdate) {
                _surfaceEditor.UpdateTileSelectedModel(MPD_File, _world, _tileSelectedPos);
                _tileSelectedNeedsUpdate = false;
            }

            UpdateViewMatrix();
            foreach (var shader in _world.Shaders)
                UpdateShaderViewMatrix(shader, _viewMatrix);

            using (_selectFramebuffer.Use()) {
                GL.ClearColor(1, 1, 1, 1);
                DrawSelectionScene();
            }
            UpdateTilePosition();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            DrawScene();

            SwapBuffers();
        }

        private void OnFrameTickRendering(float deltaInMs) {
            if (RunAnimations)
                UpdateAnimatedTextures(deltaInMs);
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (Tile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[tile.BlockLocation.Num].Invalidate();
                _tileSelectedNeedsUpdate = true;
                Invalidate();
            }
        }

        public void UpdateModels() {
            _normalMatricesByModel.Clear();
            _modelMatricesByModel.Clear();

            MakeCurrent();
            _models?.Update(MPD_File);
            _surfaceModel?.Update(MPD_File);
            _groundModel?.Update(MPD_File);
            _skyBoxModel?.Update(MPD_File);
            _gradients?.Update(MPD_File);
            Invalidate();
        }

        private void UpdateSelectFramebuffer() {
            _selectFramebuffer?.Dispose();
            _selectFramebuffer = new Framebuffer(Width, Height);
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                0.05f, 65536.0f);
        }

        private void UpdateProjectionMatrices() {
            if ((_world?.Shaders?.Count ?? 0) == 0)
                return;

            UpdateProjectionMatrix();
            foreach (var shader in _world.Shaders)
                UpdateShaderProjectionMatrix(shader, _projectionMatrix);
        }

        private void UpdateViewMatrix() {
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        private void UpdateShaderProjectionMatrix(Shader shader, Matrix4 matrix) {
            var handle = GL.GetUniformLocation(shader.Handle, "projection");
            if (handle >= 0)
                using (shader.Use())
                    GL.UniformMatrix4(handle, false, ref matrix);
        }

        private void UpdateShaderViewMatrix(Shader shader, Matrix4 matrix) {
            var handle = GL.GetUniformLocation(shader.Handle, "view");
            if (handle >= 0)
                using (shader.Use())
                    GL.UniformMatrix4(handle, false, ref matrix);
        }

        private void UpdateShaderModelMatrix(Shader shader, Matrix4 matrix) {
            var handle = GL.GetUniformLocation(shader.Handle, "model");
            if (handle >= 0)
                using (shader.Use())
                    GL.UniformMatrix4(handle, false, ref matrix);
        }

        private void UpdateShaderNormalMatrix(Shader shader, Matrix3 matrix) {
            var handle = GL.GetUniformLocation(shader.Handle, "normalMatrix");
            if (handle >= 0)
                using (shader.Use())
                    GL.UniformMatrix3(handle, false, ref matrix);
        }

        private void UpdateLightPosition(Shader shader, Vector3 lightPos) {
            var handle = GL.GetUniformLocation(shader.Handle, "lightPosition");
            if (handle >= 0)
                using (shader.Use())
                    GL.Uniform3(handle, lightPos);
        }

        private void UpdateLightingMode(Shader shader, bool useFancyOutdoorSurfaceLighting) {
            var handle = GL.GetUniformLocation(shader.Handle, "lightingMode");
            if (handle >= 0) {
                using (shader.Use()) {
                    if (handle >= 0) {
                        var lightingMode = !ApplyLighting ? 0 : !useFancyOutdoorSurfaceLighting ? 1 : 2;
                        GL.Uniform1(handle, lightingMode);
                    }
                }
            }
        }

        private void UpdateGroundGlow(Shader shader, Vector3 glow) {
            var handle = GL.GetUniformLocation(shader.Handle, "globalGlow");
            if (handle >= 0)
                using (shader.Use())
                    GL.Uniform3(handle, glow);
        }

        private Vector3 GetLightPosition() {
            if (MPD_File == null)
                return new Vector3(0, -1, 0);

            var lightPos = MPD_File.LightPosition;
            var pitch = lightPos.Pitch;

            var pitchInRadians = pitch / 32768f * Math.PI;
            var pitchSin = -Math.Sin(pitchInRadians);
            var pitchCos = Math.Cos(pitchInRadians);

            var yawInRadians = lightPos.Yaw / 32768f * Math.PI;
            var x = -Math.Sin(yawInRadians) * pitchCos;
            var y = pitchSin;
            var z = Math.Cos(yawInRadians) * pitchCos;

            return new Vector3((float) x, (float) y, (float) z);
        }

        private void DrawScene() {
            GL.StencilMask(0xFF);

            // Enable 'CullFace' to draw single-sided, and 'StencilTest' for gradients on various render passes.
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            if (DrawSkyBox)
                DrawSceneSkyBox();
            if (DrawGround)
                DrawSceneGround();

            if (DrawNormals)
                DrawSceneObjectNormals();
            else if (DrawModels || DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs)
                DrawSceneObjects();

            // Done rendering gradients; disable the stencil test.
            GL.Disable(EnableCap.StencilTest);

            if (DrawWireframe)
                DrawSceneWireframes();

            // Done rendering the real scene; disable one-sided rendering.
            GL.Disable(EnableCap.CullFace);

            if (DrawBoundaries)
                DrawSceneBoundaries();

            DrawEditorElements();
        }

        private void DrawSceneObjectNormals() {
            if (DrawModels)
                DrawSceneModelsNormals();

            if (DrawSurfaceModel)
                DrawSceneSurfaceModelNormals();
        }

        private void DrawSceneObjects() {
            GL.StencilFunc(StencilFunction.Always, 4, 0x04);
            GL.StencilMask(0x04);

            if (DrawModels)
                DrawSceneModels();

            if (DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs)
                DrawSceneSurfaceModel();

            if (DrawGradients)
                DrawSceneGradient(_gradients?.ModelsGradientModel, 0x04, true);
        }

        private void DrawSceneWireframes() {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-2.0f, -2.0f);

            using (_world.WireframeShader.Use())
            using (_world.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                if (DrawModels)
                    DrawSceneModelsWireframe();

                if (DrawSurfaceModel || (!DrawNormals && (DrawTerrainTypes || DrawEventIDs)))
                    DrawSceneSurfaceModelWireframe();
            }

            UpdateShaderModelMatrix(_world.WireframeShader, Matrix4.Identity);
            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private void DrawSceneBoundaries() {
            if (_surfaceModel.CameraBoundaryModel != null || _surfaceModel.BattleBoundaryModel != null) {
                using (_world.SolidShader.Use()) {
                    GL.Disable(EnableCap.DepthTest);
                    _surfaceModel.BattleBoundaryModel?.Draw(_world.SolidShader, null);
                    _surfaceModel.CameraBoundaryModel?.Draw(_world.SolidShader, null);
                    GL.Enable(EnableCap.DepthTest);
                }
            }
        }

        private void DrawEditorElements() {
            using (_world.TextureShader.Use()) {
                DrawSurfaceEditorTileSelected();
                DrawSurfaceEditorTileHover();

                if (DrawHelp)
                    DrawEditorHelp();
            }
        }

        private void DrawSceneModelsNormals() {
            if (_models?.ModelsByMemoryAddress == null)
                return;

            var modelsWithGroups = _models.Models
                .Select(x => new { Model = x, ModelGroup = _models.ModelsByMemoryAddress.TryGetValue(x.PData1, out ModelGroup pd) ? pd : null })
                .Where(x => x.ModelGroup != null)
                .ToArray();

            foreach (var mwg in modelsWithGroups) {
                SetModelAndNormalMatricesForModel(mwg.Model, _world.NormalsShader);
                mwg.ModelGroup.SolidTexturedModel?.Draw(_world.NormalsShader, null);
                mwg.ModelGroup.SolidUntexturedModel?.Draw(_world.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentTexturedModel?.Draw(_world.NormalsShader, null);
                mwg.ModelGroup.SemiTransparentUntexturedModel?.Draw(_world.NormalsShader, null);
            }

            // Reset model matrices to their identity.
            UpdateShaderModelMatrix(_world.NormalsShader, Matrix4.Identity);
            UpdateShaderNormalMatrix(_world.NormalsShader, Matrix3.Identity);
        }

        private void DrawSceneSurfaceModelNormals() {
            if (!(_surfaceModel?.Blocks?.Length > 0))
                return;

            using (_world.NormalsShader.Use()) {
                foreach (var block in _surfaceModel.Blocks) {
                    if (block.Model != null || block.UntexturedModel != null) {
                        block.Model?.Draw(_world.NormalsShader, null);
                        block.UntexturedModel?.Draw(_world.NormalsShader, null);
                    }
                }
            }
        }

        private void DrawSceneSkyBox() {
            if (MPD_File.Scenario >= ScenarioType.Scenario2 && _skyBoxModel.Model != null) {
                GL.Disable(EnableCap.DepthTest);
                GL.DepthMask(false);

                UpdateShaderProjectionMatrix(_world.TextureShader, Matrix4.Identity);

                const float c_repeatCount = 8f;
                var xOffset = (MathHelpers.ActualMod((Yaw / 360f) * c_repeatCount, 1.0f) - 0.5f) * 2.0f;
                var yOffset = (float) Math.Sin(-Pitch / 360.0f) * 32f + 0.975f;

                UpdateShaderViewMatrix(_world.TextureShader, Matrix4.Identity * Matrix4.CreateTranslation(xOffset, yOffset, 0));

                GL.StencilFunc(StencilFunction.Always, 2, 0x02);
                GL.StencilMask(0x02);

                using (_skyBoxModel.Texture.Use(TextureUnit.Texture0))
                    _skyBoxModel.Model.Draw(_world.TextureShader, null);

                if (DrawGradients)
                    DrawSceneGradient(_gradients?.SkyBoxGradientModel, 0x02, false);

                UpdateShaderProjectionMatrix(_world.TextureShader, _projectionMatrix);
                UpdateShaderViewMatrix(_world.TextureShader, _viewMatrix);

                GL.DepthMask(true);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        private void DrawSceneGround() {
            if (_groundModel.Model != null) {
                GL.Disable(EnableCap.DepthTest);
                GL.DepthMask(false);

                Vector3 glow = Vector3.Zero;
                if (ApplyLighting && MPD_File.LightAdjustment != null) {
                    var lightAdj = MPD_File.LightAdjustment;
                    glow = new Vector3(
                        lightAdj.GroundRAdjustment / (float) 0x1F, 
                        lightAdj.GroundGAdjustment / (float) 0x1F, 
                        lightAdj.GroundBAdjustment / (float) 0x1F
                    );
                }
                UpdateGroundGlow(_world.TextureShader, glow);

                GL.StencilFunc(StencilFunction.Always, 1, 0x01);
                GL.StencilMask(0x01);

                using (_groundModel.Texture.Use(TextureUnit.Texture0))
                    _groundModel.Model.Draw(_world.TextureShader, null);

                if (DrawGradients)
                    DrawSceneGradient(_gradients?.GroundGradientModel, 0x01, false);

                UpdateGroundGlow(_world.TextureShader, Vector3.Zero);

                GL.DepthMask(true);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        private void DrawSceneModels() {
            if (_models?.Models == null)
                return;

            UpdateLightingMode(_world.ObjectShader, false);
            var lightingTexture = _surfaceModel.LightingTexture ?? _world.WhiteTexture;

            using (_world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (_world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (_world.ObjectShader.Use()) {
                var modelsWithGroups = _models.Models
                    .Select(x => new { Model = x, ModelGroup = _models.ModelsByMemoryAddress.TryGetValue(x.PData1, out ModelGroup pd) ? pd : null })
                    .Where(x => x.ModelGroup != null)
                    .ToArray();

                // Pass 1: Textured models
                foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidTexturedModel != null).ToArray()) {
                    SetModelAndNormalMatricesForModel(mwg.Model, _world.ObjectShader);
                    mwg.ModelGroup.SolidTexturedModel.Draw(_world.ObjectShader);
                }

                // Pass 2: Untextured models
                using (_world.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas)) {
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SolidUntexturedModel != null).ToArray()) {
                        SetModelAndNormalMatricesForModel(mwg.Model, _world.ObjectShader);
                        mwg.ModelGroup.SolidUntexturedModel.Draw(_world.ObjectShader, null);
                    }
                }

                // Draw semi-transparent shaders now. Don't write to the depth buffer.
                GL.DepthMask(false);

                // Pass 3: Semi-transparent textured models
                foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentTexturedModel != null).ToArray()) {
                    SetModelAndNormalMatricesForModel(mwg.Model, _world.ObjectShader);
                    mwg.ModelGroup.SemiTransparentTexturedModel.Draw(_world.ObjectShader);
                }

                // Pass 4: Semi-transparent untextured models
                using (_world.WhiteTexture.Use(MPD_TextureUnit.TextureAtlas)) {
                    foreach (var mwg in modelsWithGroups.Where(x => x.ModelGroup.SemiTransparentUntexturedModel != null).ToArray()) {
                        SetModelAndNormalMatricesForModel(mwg.Model, _world.ObjectShader);
                        mwg.ModelGroup.SemiTransparentUntexturedModel.Draw(_world.ObjectShader, null);
                    }
                }

                // Done drawing semi-transparent shaders now. Write to the depth buffer again.
                GL.DepthMask(true);
            }

            // Reset model matrices to their identity.
            UpdateShaderModelMatrix(_world.ObjectShader, Matrix4.Identity);
            UpdateShaderNormalMatrix(_world.ObjectShader, Matrix3.Identity);
        }

        private void DrawSceneSurfaceModel() {
            if (!(_surfaceModel?.Blocks?.Length > 0))
                return;

            var terrainTypesTexture = DrawTerrainTypes ? _surfaceModel.TerrainTypesTexture : _world.TransparentBlackTexture;
            var eventIdsTexture     = DrawEventIDs     ? _surfaceModel.EventIDsTexture     : _world.TransparentBlackTexture;
            var lightingTexture     = _surfaceModel.LightingTexture ?? _world.WhiteTexture;

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-1.0f, -1.0f);

            var useFancyOutdoorSurfaceLighting = (MPD_File == null) ? false : MPD_File.MPDHeader.OutdoorLighting;
            if (useFancyOutdoorSurfaceLighting)
                UpdateLightingMode(_world.ObjectShader, true);

            using (terrainTypesTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
            using (eventIdsTexture.Use(MPD_TextureUnit.TextureEventIDs))
            using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
            using (DrawSurfaceModel ? null : _world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
            using (_world.ObjectShader.Use()) {
                foreach (var block in _surfaceModel.Blocks) {
                    if (DrawSurfaceModel)
                        block.Model?.Draw(_world.ObjectShader);
                    else
                        block.Model?.Draw(_world.ObjectShader, null);

                    using (_world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
                        block.UntexturedModel?.Draw(_world.ObjectShader, null);
                }
            }

            if (useFancyOutdoorSurfaceLighting)
                UpdateLightingMode(_world.ObjectShader, false);

            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        private void DrawSceneGradient(QuadModel gradientModel, int stencilBit, bool depthCurrentlyEnabled) {
            if (gradientModel == null)
                return;

            // TODO: 'depthCurrentlyEnabled' is pretty stilly. We should track the state somehow, and just push it.
            if (depthCurrentlyEnabled) {
                GL.Disable(EnableCap.DepthTest);
                GL.DepthMask(false);
            }

            UpdateShaderProjectionMatrix(_world.SolidShader, Matrix4.Identity);
            UpdateShaderViewMatrix(_world.SolidShader, Matrix4.Identity);

            GL.StencilFunc(StencilFunction.Equal, stencilBit, stencilBit);
            gradientModel.Draw(_world.SolidShader, null);
    
            UpdateShaderProjectionMatrix(_world.SolidShader, _projectionMatrix);
            UpdateShaderViewMatrix(_world.SolidShader, _viewMatrix);

            if (depthCurrentlyEnabled) {
                GL.DepthMask(true);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        private void DrawSceneModelsWireframe() {
            if (_models.Models == null)
                return;

            foreach (var model in _models.Models) {
                var modelGroup = _models.ModelsByMemoryAddress.TryGetValue(model.PData1, out ModelGroup pd) ? pd : null;
                if (modelGroup != null) {
                    SetModelAndNormalMatricesForModel(model, _world.WireframeShader);
                    modelGroup.SolidTexturedModel?.Draw(_world.WireframeShader);
                    modelGroup.SolidUntexturedModel?.Draw(_world.WireframeShader);
                    modelGroup.SemiTransparentTexturedModel?.Draw(_world.WireframeShader);
                    modelGroup.SemiTransparentUntexturedModel?.Draw(_world.WireframeShader);
                }
            }
        }

        private void DrawSceneSurfaceModelWireframe() {
            foreach (var block in _surfaceModel.Blocks) {
                block.UntexturedModel?.Draw(_world.WireframeShader, null);
                block.Model?.Draw(_world.WireframeShader, null);
            }
        }

        private void DrawSurfaceEditorTileSelected() {
            if (_surfaceEditor.TileSelectedModel == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            using (_surfaceEditor.TileSelectedTexture.Use())
                _surfaceEditor.TileSelectedModel.Draw(_world.TextureShader);
            GL.Enable(EnableCap.DepthTest);
        }

        private void DrawSurfaceEditorTileHover() {
            if (_surfaceEditor.TileHoverModel == null)
                return;

            GL.Disable(EnableCap.DepthTest);
            using (_surfaceEditor.TileHoverTexture.Use())
                _surfaceEditor.TileHoverModel.Draw(_world.TextureShader);
            GL.Enable(EnableCap.DepthTest);
        }

        private void DrawEditorHelp() {
            if (_surfaceEditor.HelpModel == null)
                return;

            const float c_viewSize = 0.40f;

            var viewportRatio = (float) Width / Height;
            var textureRatio = (float) _surfaceEditor.HelpTexture.Width / _surfaceEditor.HelpTexture.Height;

            UpdateShaderViewMatrix(_world.TextureShader,
                Matrix4.CreateScale(2f / viewportRatio * textureRatio * c_viewSize, 2f * c_viewSize, 2f * c_viewSize) *
                Matrix4.CreateTranslation(1, -1, 0) *
                _projectionMatrix.Inverted());

            GL.Disable(EnableCap.DepthTest);
            using (_surfaceEditor.HelpTexture.Use())
                _surfaceEditor.HelpModel.Draw(_world.TextureShader);
            GL.Enable(EnableCap.DepthTest);

            UpdateShaderViewMatrix(_world.TextureShader, _viewMatrix);
        }

        private Dictionary<Model, Matrix4?> _modelMatricesByModel = [];
        private Dictionary<Model, Matrix3?> _normalMatricesByModel = [];

        private void InvalidateSpriteMatrices() {
            if (_models?.Models == null)
                return;
            var spriteModels = _models.Models.Where(x => x.AlwaysFacesCamera).ToList();
            foreach (var sm in spriteModels) {
                _modelMatricesByModel.Remove(sm);
                _normalMatricesByModel.Remove(sm);
            }
        }

        private void SetModelAndNormalMatricesForModel(Model model, Shader shader) {
            var modelMatrix = _modelMatricesByModel.TryGetValue(model, out var modelMatrixValue) ? modelMatrixValue : null;
            if (!modelMatrix.HasValue) {
                var angleXAdjust = 0.00f;
                var angleYAdjust = model.AlwaysFacesCamera ? (float) ((Yaw + 180.0f) / 180.0f * Math.PI) : 0.00f;
                var scaleAdjust  = 1.00f;

                var yAdjust = 0.00f;
                var prePostAdjustY = 0.00f;

                if (model.AlwaysFacesCamera && RotateSpritesUp) {
                    // Not all sprites rotate around the X axis the same way, so get the center X to help with offsets.
                    var pdata    = MPD_File.ModelCollections[0].PDatasByMemoryAddress.TryGetValue(model.PData1, out var pdataValue) ? pdataValue : null;
                    var vertices = (pdata == null) ? null : MPD_File.ModelCollections[0].VertexTablesByMemoryAddress.TryGetValue(pdata.VerticesOffset, out var verticesValue) ? verticesValue : null;

                    var topY     = vertices?.Min(x => Math.Min(x.Vector.Y.Float, x.Vector.Z.Float)) / 32.0f ?? 0.00f;
                    var bottomY  = vertices?.Max(x => Math.Max(x.Vector.Y.Float, x.Vector.Z.Float)) / 32.0f ?? 0.00f;
                    var centerY  = (topY + bottomY) * 0.5f;

                    angleXAdjust = (float) (Pitch / 180.0f * Math.PI) * -1.00f;

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

            UpdateShaderModelMatrix(shader, modelMatrix.Value);
            UpdateShaderNormalMatrix(shader, normalMatrix.Value);
        }

        private void DrawSelectionScene() {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (_surfaceModel?.Blocks == null)
                return;
            using (_world.SolidShader.Use())
                foreach (var block in _surfaceModel.Blocks)
                    if (block.SelectionModel != null)
                        block.SelectionModel.Draw(_world.SolidShader);
        }

        private float _frameDeltaTimeInMs = 0;

        private void UpdateAnimatedTextures(float deltaInMs) {

            const float c_frameDurationInMs = (1000.0f / 30.0f);

            _frameDeltaTimeInMs += deltaInMs;
            if (_frameDeltaTimeInMs >= c_frameDurationInMs) {
                while (_frameDeltaTimeInMs >= c_frameDurationInMs)
                    _frameDeltaTimeInMs -= c_frameDurationInMs;

                if (_surfaceModel?.Blocks != null)
                    foreach (var block in _surfaceModel.Blocks)
                        if (block.Model?.UpdateAnimatedTextures() == true)
                            Invalidate();

                if (_models?.ModelsByMemoryAddress != null)
                    foreach (var modelGroup in _models.ModelsByMemoryAddress.Values)
                        foreach (var model in modelGroup.Models)
                            if (model.UpdateAnimatedTextures() == true)
                                Invalidate();
            }
        }

        private AppState _appState = null;
        private AppState AppState {
            get {
                if (_appState == null)
                    _appState = AppState.RetrieveAppState();
                return _appState;
            }
        }

        private bool UpdateAppState(string propertyName, bool value) {
            var property = AppState.GetType().GetProperty(propertyName);
            if (property == null || (bool) property.GetValue(AppState, null) == value)
                return false;

            property.SetValue(AppState, value);
            AppState.Serialize();
            Invalidate();

            return true;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawSurfaceModel {
            get => AppState.ViewerDrawSurfaceModel;
            set => UpdateAppState(nameof(AppState.ViewerDrawSurfaceModel), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawModels {
            get => AppState.ViewerDrawModels;
            set => UpdateAppState(nameof(AppState.ViewerDrawModels), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGround {
            get => AppState.ViewerDrawGround;
            set => UpdateAppState(nameof(AppState.ViewerDrawGround), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawSkyBox {
            get => AppState.ViewerDrawSkyBox;
            set => UpdateAppState(nameof(AppState.ViewerDrawSkyBox), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunAnimations {
            get => AppState.ViewerRunAnimations;
            set => UpdateAppState(nameof(AppState.ViewerRunAnimations), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyLighting {
            get => AppState.ViewerApplyLighting;
            set => UpdateAppState(nameof(AppState.ViewerApplyLighting), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGradients {
            get => AppState.ViewerDrawGradients;
            set => UpdateAppState(nameof(AppState.ViewerDrawGradients), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawWireframe {
            get => AppState.ViewerDrawWireframe;
            set => UpdateAppState(nameof(AppState.ViewerDrawWireframe), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawBoundaries {
            get => AppState.ViewerDrawBoundaries;
            set => UpdateAppState(nameof(AppState.ViewerDrawBoundaries), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawTerrainTypes {
            get => AppState.ViewerDrawTerrainTypes;
            set => UpdateAppState(nameof(AppState.ViewerDrawTerrainTypes), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawEventIDs {
            get => AppState.ViewerDrawEventIDs;
            set => UpdateAppState(nameof(AppState.ViewerDrawEventIDs), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawNormals {
            get => AppState.ViewerDrawNormals;
            set => UpdateAppState(nameof(AppState.ViewerDrawNormals), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RotateSpritesUp {
            get => AppState.ViewerRotateSpritesUp;
            set {
                if (UpdateAppState(nameof(AppState.ViewerRotateSpritesUp), value))
                    InvalidateSpriteMatrices();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawHelp {
            get => AppState.ViewerDrawHelp;
            set => UpdateAppState(nameof(AppState.ViewerDrawHelp), value);
        }

        private bool _tileSelectedNeedsUpdate = false;

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private WorldResources _world = null;
        private ModelResources _models = null;
        private SurfaceModelResources _surfaceModel = null;
        private GroundModelResources _groundModel = null;
        private SkyBoxModelResources _skyBoxModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
        private GradientResources _gradients = null;

        private Framebuffer _selectFramebuffer;
    }
}
