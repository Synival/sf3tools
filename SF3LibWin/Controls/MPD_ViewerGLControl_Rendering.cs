using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitRendering() {
            Load      += (s, e) => OnLoadRendering();
            Disposed  += (s, e) => OnDisposeRendering();
            Resize    += (s, e) => OnResizeRendering();
            Paint     += (s, e) => OnPaintRendering();
            FrameTick += (s, e) => OnFrameTickRendering();
            TileModified += (s, e) => OnTileModifiedRendering(s);
        }

        public void UpdateLighting() {
            MakeCurrent();
            UpdateShaderLighting();
            using (var textureBitmap = CreateLightPaletteBitmap())
                _surfaceModel.SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, clampToEdge: false) : null);
        }

        private void UpdateShaderLighting() {
            MakeCurrent();
            var lightPos = GetLightPosition();
            var useNewLighting = (MPD_File == null) ? false : MPD_File.MPDHeader[0].UseNewLighting;
            foreach (var shader in _world.Shaders) {
                using (shader.Use())
                    UpdateShaderLighting(shader, lightPos, useNewLighting);
            }
        }

        private Bitmap CreateLightPaletteBitmap() {
            var lightPal = MPD_File?.LightPalette;
            if (lightPal == null)
                return null;

            var numColors = lightPal.Length;

            var colorData = new byte[numColors * 4];
            int pos = 0;
            foreach (var color in lightPal) {
                var channels = PixelConversion.ABGR1555toChannels(color.ColorABGR1555);
                colorData[pos++] = channels.b;
                colorData[pos++] = channels.g;
                colorData[pos++] = channels.r;
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

            _world         = new WorldResources();
            _surfaceModel  = new SurfaceModelResources();
            _surfaceEditor = new SurfaceEditorResources();

            _world.Init();
            _surfaceModel.Init();
            _surfaceEditor.Init();

            SetInitialCameraPosition();
            UpdateSelectFramebuffer();
            UpdateLighting();

            foreach (var shader in _world.Shaders)
                UpdateShaderModelMatrix(shader, Matrix4.Identity);
        }

        private void OnDisposeRendering() {
            _world?.Dispose();
            _surfaceModel?.Dispose();
            _surfaceEditor?.Dispose();
            _selectFramebuffer?.Dispose();

            _world             = null;
            _surfaceModel      = null;
            _surfaceEditor     = null;
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
            DrawScene();

            SwapBuffers();
        }

        private void OnFrameTickRendering() {
            UpdateAnimatedTextures();
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (Tile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[tile.BlockLocation.Num].Invalidate();
                _tileSelectedNeedsUpdate = true;
                Invalidate();
            }
        }

        public void UpdateSurfaceModels() {
            if (_surfaceModel == null)
                return;

            MakeCurrent();
            _surfaceModel.Update(MPD_File);
            Invalidate();
        }

        private void UpdateSelectFramebuffer() {
            _selectFramebuffer?.Dispose();
            _selectFramebuffer = new Framebuffer(Width, Height);
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                0.1f, 300.0f);
        }

        private void UpdateProjectionMatrices() {
            if ((_world?.Shaders?.Count ?? 0) == 0)
                return;

            UpdateProjectionMatrix();
            var projectionMatrix = _projectionMatrix;

            foreach (var shader in _world.Shaders) {
                var handle = GL.GetUniformLocation(shader.Handle, "projection");
                if (handle >= 0)
                    using (shader.Use())
                        GL.UniformMatrix4(handle, false, ref projectionMatrix);
            }
        }

        private void UpdateViewMatrix() {
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
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

        private void UpdateShaderLighting(Shader shader, Vector3 lightPos, bool useNewLighting) {
            var handle1 = GL.GetUniformLocation(shader.Handle, "lightPosition");
            var handle2 = GL.GetUniformLocation(shader.Handle, "useNewLighting");

            if (handle1 >= 0 || handle2 >= 0) {
                using (shader.Use()) {
                    if (handle1 >= 0)
                        GL.Uniform3(handle1, lightPos);
                    if (handle2 >= 0)
                        GL.Uniform1(handle2, useNewLighting ? 1 : 0);
                }
            }
        }

        private Vector3 GetLightPosition() {
            if (MPD_File == null)
                return new Vector3(0, -1, 0);

            var lightDir = MPD_File.LightDirectionTable[0];
            var pitch = lightDir.Pitch;

            var pitchInRadians = pitch / 32768f * Math.PI;
            var pitchSin = Math.Sin(pitchInRadians);
            var pitchCos = Math.Cos(pitchInRadians);

            var yawInRadians = lightDir.Yaw / 32768f * Math.PI;
            var x = -Math.Sin(yawInRadians) * pitchCos;
            var y = pitchSin;
            var z = -Math.Cos(yawInRadians) * pitchCos;

            return new Vector3((float) x, (float) y, (float) z);
        }

        private void DrawScene() {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_drawNormals) {
                using (_world.NormalsShader.Use()) {
                    foreach (var block in _surfaceModel.Blocks) {
                        if (block.Model != null || block.UntexturedModel != null) {
                            block.Model?.Draw(_world.NormalsShader, null);
                            block.UntexturedModel?.Draw(_world.NormalsShader, null);
                        }
                    }
                }
            }
            else {
                var terrainTypesTexture = DrawTerrainTypes ? _surfaceModel.TerrainTypesTexture : _world.TransparentBlackTexture;
                var eventIdsTexture     = DrawEventIDs     ? _surfaceModel.EventIDsTexture     : _world.TransparentBlackTexture;
                var lightingTexture     = _surfaceModel.LightingTexture ?? _world.WhiteTexture;

                using (terrainTypesTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
                using (eventIdsTexture.Use(MPD_TextureUnit.TextureEventIDs))
                using (lightingTexture.Use(MPD_TextureUnit.TextureLighting))
                using (_world.ObjectShader.Use()) {
                    foreach (var block in _surfaceModel.Blocks) {
                        block.Model?.Draw(_world.ObjectShader);
                        using (_world.TransparentBlackTexture.Use(MPD_TextureUnit.TextureAtlas))
                            block.UntexturedModel?.Draw(_world.ObjectShader, null);
                    }
                }
            }

            if (DrawWireframe) {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.DepthFunc(DepthFunction.Lequal);

                using (_world.WireframeShader.Use())
                using (_world.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                    UpdateShaderModelMatrix(_world.WireframeShader, Matrix4.CreateTranslation(0f, 0.02f, 0f));
                    foreach (var block in _surfaceModel.Blocks) {
                        block.UntexturedModel?.Draw(_world.WireframeShader, null);
                        block.Model?.Draw(_world.WireframeShader, null);
                    }
                    UpdateShaderModelMatrix(_world.WireframeShader, Matrix4.Identity);
                }

                GL.DepthFunc(DepthFunction.Less);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            if (DrawBoundaries) {
                if (_surfaceModel.CameraBoundaryModel != null || _surfaceModel.BattleBoundaryModel != null) {
                    using (_world.SolidShader.Use()) {
                        GL.Disable(EnableCap.DepthTest);
                        _surfaceModel.BattleBoundaryModel?.Draw(_world.SolidShader, null);
                        _surfaceModel.CameraBoundaryModel?.Draw(_world.SolidShader, null);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            using (_world.TextureShader.Use()) {
                if (_surfaceEditor.TileSelectedModel != null) {
                    GL.Disable(EnableCap.DepthTest);
                    using (_surfaceEditor.TileSelectedTexture.Use())
                        _surfaceEditor.TileSelectedModel.Draw(_world.TextureShader);
                    GL.Enable(EnableCap.DepthTest);
                }

                if (_surfaceEditor.TileHoverModel != null) {
                    GL.Disable(EnableCap.DepthTest);
                    using (_surfaceEditor.TileHoverTexture.Use())
                        _surfaceEditor.TileHoverModel.Draw(_world.TextureShader);
                    GL.Enable(EnableCap.DepthTest);
                }

                if (DrawHelp && _surfaceEditor.HelpModel != null) {
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
            }

            // TODO: Code from SurfaceMap2DControl to indicate untagged tiles. Use this later somehow!!
#if false
            // Indicate unidentified textures.
            var expectedTag = new TagKey(textureFlags);
            if (!texture.Tags.ContainsKey(expectedTag)) {
                // NOTE: Graphics.FromImage() throws an OutOfMemoryException due to a bad GDI+ implementation,
                // so we have to do it this way.
                using var questionMark = new Bitmap(image.Width / 2, image.Height / 2);
                using (var g = Graphics.FromImage(questionMark)) {
                    g.Clear(Color.Black);
                    g.DrawString("?", new Font(new FontFamily("Consolas"), (int) (questionMark.Width * 0.75)), Brushes.White, 0, 0);
                    g.Flush();
                }

                var posX = image.Width  - questionMark.Width;
                var posY = image.Height - questionMark.Height;
                image.SafeDrawImage(questionMark, posX, posY);
            }
#endif
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

        private void UpdateAnimatedTextures() {
            // The clock is at 60fps, but the animations change at 30fps, so skip every other frame.
            if (_frame % 2 == 0)
                return;

            if (_surfaceModel?.Blocks == null)
                return;
            foreach (var block in _surfaceModel.Blocks)
                if (block.Model?.UpdateAnimatedTextures() == true)
                    Invalidate();
        }

        private static bool _drawWireframe = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawWireframe {
            get => _drawWireframe;
            set {
                if (_drawWireframe != value) {
                    _drawWireframe = value;
                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawWireframe = value;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private static bool _drawBoundaries = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawBoundaries {
            get => _drawBoundaries;
            set {
                if (_drawBoundaries != value) {
                    _drawBoundaries = value;
                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawBoundaries = value;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private static bool _drawHelp = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawHelp {
            get => _drawHelp;
            set {
                if (_drawHelp != value) {
                    _drawHelp = value;
                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawHelp = value;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private static bool _drawNormals = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawNormals {
            get => _drawNormals;
            set {
                if (_drawNormals != value) {
                    _drawNormals = value;
                    if (_drawNormals == true) {
                        _drawTerrainTypes = false;
                        _drawEventIds = false;
                    }

                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawNormals      = value;
                    state.ViewerDrawTerrainTypes = _drawTerrainTypes;
                    state.ViewerDrawEventIDs     = _drawEventIds;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private static bool _drawTerrainTypes = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawTerrainTypes {
            get => _drawTerrainTypes;
            set {
                if (_drawTerrainTypes != value) {
                    _drawTerrainTypes = value;
                    if (_drawTerrainTypes == true)
                        _drawNormals = false;

                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawNormals      = _drawNormals;
                    state.ViewerDrawTerrainTypes = _drawTerrainTypes;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private static bool _drawEventIds = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawEventIDs {
            get => _drawEventIds;
            set {
                if (_drawEventIds != value) {
                    _drawEventIds = value;
                    if (_drawEventIds == true)
                        _drawNormals = false;

                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawNormals  = _drawNormals;
                    state.ViewerDrawEventIDs = _drawTerrainTypes;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private bool _tileSelectedNeedsUpdate = false;

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private WorldResources _world = null;
        private SurfaceModelResources _surfaceModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
        private Framebuffer _selectFramebuffer;
    }
}
