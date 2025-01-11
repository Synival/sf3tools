using System;
using System.ComponentModel;
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
            if (_surfaceModelUpdateFrames > 0) {
                _surfaceModelUpdateFrames--;
                if (_surfaceModelUpdateFrames == 0)
                    UpdateSurfaceModels();
            }
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (Tile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[tile.BlockLocation.Num].Invalidate();
                _tileSelectedNeedsUpdate = true;
                Invalidate();
            }
        }

        public void UpdateSurfaceModelsIn(int frames) {
            if (frames <= 0)
                UpdateSurfaceModels();
            else
                _surfaceModelUpdateFrames = frames;
        }

        public void UpdateSurfaceModels() {
            _surfaceModelUpdateFrames = 0;
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
                using (shader.Use()) {
                    var handle = GL.GetUniformLocation(shader.Handle, "projection");
                    GL.UniformMatrix4(handle, false, ref projectionMatrix);
                }
            }
        }

        private void UpdateViewMatrix() {
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        private void UpdateShaderViewMatrix(Shader shader, Matrix4 matrix) {
            using (shader.Use()) {
                var handle = GL.GetUniformLocation(shader.Handle, "view");
                GL.UniformMatrix4(handle, false, ref matrix);
            }
        }

        private void UpdateShaderModelMatrix(Shader shader, Matrix4 matrix) {
            using (shader.Use()) {
                var handle = GL.GetUniformLocation(shader.Handle, "model");
                GL.UniformMatrix4(handle, false, ref matrix);
            }
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

                using (terrainTypesTexture.Use(MPD_TextureUnit.TextureTerrainTypes))
                using (eventIdsTexture.Use(MPD_TextureUnit.TextureEventIDs))
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
                    UpdateShaderViewMatrix(_world.TextureShader,
                        Matrix4.CreateScale(2f * c_viewSize, 2f * c_viewSize, 2f * c_viewSize) *
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

        private int _surfaceModelUpdateFrames = 0;
        private bool _tileSelectedNeedsUpdate = false;

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private WorldResources _world = null;
        private SurfaceModelResources _surfaceModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
        private Framebuffer _selectFramebuffer;
    }
}
