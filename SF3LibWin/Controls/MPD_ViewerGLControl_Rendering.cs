using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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

            UpdateViewMatrix();
            foreach (var shader in _world.Shaders)
                UpdateShaderViewMatrix(shader, _viewMatrix);

            using (_selectFramebuffer.Use(FramebufferTarget.Framebuffer)) {
                GL.ClearColor(1, 1, 1, 1);
                GL.Enable(EnableCap.CullFace);
                DrawSelectionScene();
                GL.Disable(EnableCap.CullFace);
            }
            UpdateTilePosition();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            DrawScene();

            SwapBuffers();
        }

        private void OnFrameTickRendering()
            => UpdateAnimatedTextures();

        public void UpdateSurfaceModels() {
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

            if (_surfaceModel.Model != null || _surfaceModel.UntexturedModel != null) {
                if (_drawNormals) {
                    using (_world.NormalsShader.Use()) {
                        _surfaceModel.Model?.Draw(_world.NormalsShader, false);
                        _surfaceModel.UntexturedModel?.Draw(_world.NormalsShader, false);
                    }
                }
                else
                    _surfaceModel.Model?.Draw(_world.ObjectShader);

                if (DrawWireframe) {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    GL.DepthFunc(DepthFunction.Lequal);

                    using (_world.WireframeShader.Use())
                    using (_world.TileWireframeTexture.Use(TextureUnit.Texture1)) {
                        UpdateShaderModelMatrix(_world.WireframeShader, Matrix4.CreateTranslation(0f, 0.02f, 0f));
                        _surfaceModel.UntexturedModel?.Draw(_world.WireframeShader, false);
                        _surfaceModel.Model?.Draw(_world.WireframeShader, false);
                        UpdateShaderModelMatrix(_world.WireframeShader, Matrix4.Identity);
                    }

                    GL.DepthFunc(DepthFunction.Less);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }

                using (_world.TextureShader.Use()) {
                    if (_surfaceEditor.TileModel != null) {
                        GL.Disable(EnableCap.DepthTest);
                        using (_surfaceEditor.TileHoverTexture.Use())
                            _surfaceEditor.TileModel.Draw(_world.TextureShader);
                        GL.Enable(EnableCap.DepthTest);
                    }

                    if (DrawHelp && _surfaceEditor.HelpModel != null) {
                        const float c_viewSize = 0.20f;
                        UpdateShaderViewMatrix(_world.TextureShader,
                            Matrix4.CreateScale((float) Height / Width * 2f * c_viewSize, 2f * c_viewSize, 2f * c_viewSize) *
                            Matrix4.CreateTranslation(1, -1, 0) *
                            _projectionMatrix.Inverted());

                        GL.Disable(EnableCap.DepthTest);
                        using (_surfaceEditor.HelpTexture.Use())
                            _surfaceEditor.HelpModel.Draw(_world.TextureShader);
                        GL.Enable(EnableCap.DepthTest);

                        UpdateShaderViewMatrix(_world.TextureShader, _viewMatrix);
                    }
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
            if (_surfaceModel.SelectionModel != null)
                _surfaceModel.SelectionModel.Draw(_world.SolidShader);
        }

        private void UpdateAnimatedTextures() {
            if (_frame % 2 == 0)
                return;
            if (_surfaceModel?.Model?.UpdateAnimatedTextures() == true)
                Invalidate();
        }

        private static bool _drawWireframe = true;

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

        public bool DrawNormals {
            get => _drawNormals;
            set {
                if (_drawNormals != value) {
                    _drawNormals = value;
                    var state = AppState.RetrieveAppState();
                    state.ViewerDrawNormals = value;
                    state.Serialize();
                    Invalidate();
                }
            }
        }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private WorldResources _world = null;
        private SurfaceModelResources _surfaceModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
        private Framebuffer _selectFramebuffer;
    }
}
