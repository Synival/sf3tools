using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using SF3.Win.OpenGL;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl : GLControl {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        private const float c_offX = WidthInTiles / -2f;
        private const float c_offY = HeightInTiles / -2f;
        private const MouseButtons c_MouseMiddleRight = MouseButtons.Middle | MouseButtons.Right;

        private static Vector3? s_lastPosition  = null;
        private static float? s_lastPitch       = null;
        private static float? s_lastYaw         = null;

        public MPD_ViewerGLControl() {
            InitializeComponent();

            Disposed += (s, a) => {
                if (_surfaceModels != null)
                    foreach (var model in _surfaceModels)
                        model.Dispose();

                if (_shaders != null)
                    foreach (var shader in _shaders)
                        shader.Dispose();

                if (_textures != null)
                    foreach (var texture in _textures)
                        texture.Dispose();

                _tileModel?.Dispose();
                _helpModel?.Dispose();

                if (_selectFramebuffer != null)
                    _selectFramebuffer.Dispose();

                if (_timer != null)
                    _timer.Dispose();
            };

            var state      = AppState.RetrieveAppState();
            _drawWireframe = state.ViewerDrawWireframe;
            _drawHelp      = state.ViewerDrawHelp;
            _drawNormals   = state.ViewerDrawNormals;
        }

        protected override void WndProc(ref Message m) {
            const int WM_RBUTTONDBLCLK   = 0x0206;
            const int WM_NCMBUTTONDBLCLK = 0x0209;

            switch (m.Msg) {
                case WM_RBUTTONDBLCLK:
                    OnRightDoubleClick(EventArgs.Empty);
                    break;

                case WM_NCMBUTTONDBLCLK:
                    OnMiddleDoubleClick(EventArgs.Empty);
                    break;
            }

            base.WndProc(ref m);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _textureShader    = new Shader("Shaders/Texture.vert",    "Shaders/Texture.frag");
            _twoTextureShader = new Shader("Shaders/TwoTexture.vert", "Shaders/TwoTexture.frag");
            _solidShader      = new Shader("Shaders/Solid.vert",      "Shaders/Solid.frag");
            _normalsShader    = new Shader("Shaders/Normals.vert",    "Shaders/Normals.frag");
            _wireframeShader  = new Shader("Shaders/Wireframe.vert",  "Shaders/Wireframe.frag");

            _whiteTexture         = new Texture((Bitmap) Image.FromFile("Images/White.bmp"));
            _transparentTexture   = new Texture((Bitmap) Image.FromFile("Images/Transparent.bmp"));
            _tileWireframeTexture = new Texture((Bitmap) Image.FromFile("Images/TileWireframe.bmp"));
            _tileHoverTexture     = new Texture((Bitmap) Image.FromFile("Images/TileHover.bmp"));
            _helpTexture          = new Texture((Bitmap) Image.FromFile("Images/ViewerHelp.bmp"));

            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();
            _timer.Start();

            if (!s_lastPosition.HasValue)
                s_lastPosition = Position = new Vector3(0, 60, 120);
            else
                Position = s_lastPosition.Value;

            if (!s_lastPitch.HasValue || !s_lastYaw.HasValue) {
                LookAtTarget(new Vector3(0, 5, 0));
                s_lastPitch = Pitch;
                s_lastYaw   = Yaw;
            }
            else {
                Pitch = s_lastPitch.Value;
                Yaw   = s_lastYaw.Value;
            }

            UpdateFramebuffer();

            _textures = [_whiteTexture, _transparentTexture, _tileWireframeTexture, _tileHoverTexture, _helpTexture];
            _shaders  = [_textureShader, _twoTextureShader, _solidShader, _normalsShader, _wireframeShader];

            foreach (var shader in _shaders)
                UpdateShaderModelMatrix(shader, Matrix4.Identity);

            var helpWidth = _helpTexture.Width / _helpTexture.Height;
            _helpModel = new QuadModel([
                new Quad([
                    new Vector3(-helpWidth,  1, 0),
                    new Vector3(         0,  1, 0),
                    new Vector3(         0,  0, 0),
                    new Vector3(-helpWidth,  0, 0)
                ])
            ]);
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            UpdateFramebuffer();
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void UpdateFramebuffer() {
            if (_selectFramebuffer != null)
                _selectFramebuffer.Dispose();
            _selectFramebuffer = new Framebuffer(Width, Height);
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                0.1f, 300.0f);
        }

        private void UpdateProjectionMatrices() {
            if (_shaders == null || _shaders.Length == 0)
                return;

            UpdateProjectionMatrix();
            var projectionMatrix = _projectionMatrix;

            foreach (var shader in _shaders) {
                using (shader.Use()) {
                    var handle = GL.GetUniformLocation(shader.Handle, "projection");
                    GL.UniformMatrix4(handle, false, ref projectionMatrix);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            MakeCurrent();

            UpdateViewMatrix();
            foreach (var shader in _shaders)
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

            using (_textureShader.Use()) {
                if (_surfaceModel != null || _untexturedSurfaceModel != null) {
                    if (_drawNormals) {
                        _surfaceModel?.Draw(_normalsShader, false);
                        _untexturedSurfaceModel?.Draw(_normalsShader, false);
                    }
                    else
                        _surfaceModel?.Draw(_textureShader);

                    if (DrawWireframe) {
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.DepthFunc(DepthFunction.Lequal);

                        using (_wireframeShader.Use())
                        using (_tileWireframeTexture.Use(TextureUnit.Texture1)) {
                            UpdateShaderModelMatrix(_wireframeShader, Matrix4.CreateTranslation(0f, 0.02f, 0f));
                            _untexturedSurfaceModel?.Draw(_wireframeShader, false);
                            _surfaceModel?.Draw(_wireframeShader, false);
                            UpdateShaderModelMatrix(_wireframeShader, Matrix4.Identity);
                        }

                        GL.DepthFunc(DepthFunction.Less);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    }
                }

                if (_tileModel != null) {
                    GL.Disable(EnableCap.DepthTest);
                    using (_tileHoverTexture.Use())
                        _tileModel.Draw(_textureShader);
                    GL.Enable(EnableCap.DepthTest);
                }

                if (DrawHelp && _helpModel != null) {
                    const float c_viewSize = 0.20f;
                    UpdateShaderViewMatrix(_textureShader,
                        Matrix4.CreateScale((float) Height / Width * 2f * c_viewSize, 2f * c_viewSize, 2f * c_viewSize) *
                        Matrix4.CreateTranslation(1, -1, 0) *
                        _projectionMatrix.Inverted());

                    GL.Disable(EnableCap.DepthTest);
                    using (_helpTexture.Use())
                        _helpModel.Draw(_textureShader);
                    GL.Enable(EnableCap.DepthTest);

                    UpdateShaderViewMatrix(_textureShader, _viewMatrix);
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
            if (_surfaceSelectionModel != null)
                _surfaceSelectionModel.Draw(_solidShader);
        }

        private Vector3[] GetTileVertices(Point pos) {
            float[] heights;

            // For any tile whose character/texture ID has flag 0x80, the walking heightmap is used.
            if (Model.TileSurfaceHeightmapRows != null && (Model.TileSurfaceCharacterRows?.Rows[pos.Y]?.GetTextureFlags(pos.X) & 0x80) == 0x80)
                heights = Model.TileSurfaceHeightmapRows.Rows[pos.Y].GetHeights(pos.X);
            // Otherwise, gather heights from the 5x5 block with the surface mesh's heightmap.
            else if (Model.TileSurfaceVertexHeightMeshBlocks != null) {
                var blockLocations = new BlockVertexLocation[] {
                    GetBlockLocations(pos.X, pos.Y, CornerType.TopLeft,     true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.TopRight,    true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.BottomRight, true)[0],
                    GetBlockLocations(pos.X, pos.Y, CornerType.BottomLeft,  true)[0],
                };

                heights = blockLocations
                    .Select(x => Model.TileSurfaceVertexHeightMeshBlocks.Rows[x.Num][x.X, x.Y] / 16.0f)
                    .ToArray();
            }
            else
                heights = [0, 0, 0, 0];

            return [
                (pos.X + 0 + c_offX, heights[0], pos.Y + 0 + c_offY),
                (pos.X + 1 + c_offX, heights[1], pos.Y + 0 + c_offY),
                (pos.X + 1 + c_offX, heights[2], pos.Y + 1 + c_offY),
                (pos.X + 0 + c_offX, heights[3], pos.Y + 1 + c_offY)
            ];
        }

        private string[,] _debugText = new string[64, 64];

        public void UpdateSurfaceModels() {
            MakeCurrent();

            if (_surfaceModels != null) {
                foreach (var model in _surfaceModels)
                    model.Dispose();
                Invalidate();
                _surfaceModels = null;
            }

            var texturesById = (Model.TextureChunks != null) ? Model.TextureChunks
                .SelectMany(x => x.TextureTable.Rows)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = (Model.TextureAnimations != null) ? Model.TextureAnimations.Rows
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => x.Frames.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray())
                : [];

            var surfaceQuads           = new List<Quad>();
            var untexturedSurfaceQuads = new List<Quad>();
            var surfaceSelectionQuads  = new List<Quad>();

            Vector3 GetVertexAbnormal(int tileX, int tileY, CornerType corner) {
                var locations = GetBlockLocations(tileX, tileY, corner);
                if (locations.Length == 0 || Model.TileSurfaceVertexNormalMeshBlocks?.Rows == null)
                    return new Vector3(0f, 1 / 32768f, 0f);

                // The vertex abnormals SHOULD be the same, so just use the first one.
                var loc = locations[0];
                return Model.TileSurfaceVertexNormalMeshBlocks.Rows[loc.Num][loc.X, loc.Y].ToVector3();
            };

            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            for (var y = 0; y < WidthInTiles; y++) {
                for (var x = 0; x < HeightInTiles; x++) {
                    TextureAnimation anim = null;
                    byte textureFlags = 0;

                    if (textureData != null) {
                        var key = textureData[x, y];
                        var textureId = textureData[x, y] & 0xFF;
                        textureFlags = (byte) ((textureData[x, y] >> 8) & 0xFF);

                        // Get texture. Fetch animated textures if possible.
                        if (textureId != 0xFF && texturesById.ContainsKey(textureId)) {
                            if (animationsById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, animationsById[textureId]);
                            else if (texturesById.ContainsKey(textureId))
                                anim = new TextureAnimation(textureId, [texturesById[textureId]]);
                        }
                    }

                    var vertexAbnormals = new Vector3[] {
                        GetVertexAbnormal(x, y, CornerType.TopLeft),
                        GetVertexAbnormal(x, y, CornerType.TopRight),
                        GetVertexAbnormal(x, y, CornerType.BottomRight),
                        GetVertexAbnormal(x, y, CornerType.BottomLeft),
                    };
                    var abnormalVboData = vertexAbnormals.SelectMany(x => x.ToFloatArray()).ToArray().To2DArray(4, 3);

                    var vertices = GetTileVertices(new Point(x, y));
                    if (anim != null) {
                        var newQuad = new Quad(vertices, anim, textureFlags);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        surfaceQuads.Add(newQuad);
                    }
                    else {
                        var newQuad = new Quad(vertices);
                        newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec3, "normal", 4, abnormalVboData));
                        untexturedSurfaceQuads.Add(newQuad);
                    }

                    surfaceSelectionQuads.Add(new Quad(vertices, new Vector3(x / (float) WidthInTiles, y / (float) HeightInTiles, 0)));
                    _debugText[x, y] =
                        "  [" + (x - 0.5) + ", " + (y - 0.5) + "] Pos: " + vertices[0] + ", Normal: " + vertexAbnormals[0] + "\n" +
                        "  [" + (x + 0.5) + ", " + (y - 0.5) + "] Pos: " + vertices[1] + ", Normal: " + vertexAbnormals[1] + "\n" +
                        "  [" + (x + 0.5) + ", " + (y + 0.5) + "] Pos: " + vertices[2] + ", Normal: " + vertexAbnormals[2] + "\n" +
                        "  [" + (x - 0.5) + ", " + (y + 0.5) + "] Pos: " + vertices[3] + ", Normal: " + vertexAbnormals[3] + "\n";
                }
            }

            var models = new List<QuadModel>();

            if (surfaceQuads.Count > 0) {
                _surfaceModel = new QuadModel(surfaceQuads.ToArray());
                models.Add(_surfaceModel);
            }
            if (untexturedSurfaceQuads.Count > 0) {
                _untexturedSurfaceModel = new QuadModel(untexturedSurfaceQuads.ToArray());
                models.Add(_untexturedSurfaceModel);
            }
            if (surfaceSelectionQuads.Count > 0) {
                _surfaceSelectionModel = new QuadModel(surfaceSelectionQuads.ToArray());
                models.Add(_surfaceSelectionModel);
            }

            if (models.Count > 0) {
                _surfaceModels = models.ToArray();
                Invalidate();
            }
        }

        public void LookAtTarget(Vector3 target) {
            // Calculate the pitch/yaw to point the camera to that target.
            var posMinusTarget = Position - target;
            Pitch = -MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.Y, double.Hypot(posMinusTarget.X, posMinusTarget.Z)));
            Yaw   =  MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.X, posMinusTarget.Z));
        }

        public IMPD_File Model { get; set; }

        private Vector3 _position = new Vector3(0, 0, 0);
        private float _pitch = 0;
        private float _yaw = 0;

        /// <summary>
        /// Position of the camera
        /// </summary>
        public Vector3 Position {
            get => _position;
            set {
                if (value != _position) {
                    _position = value;
                    s_lastPosition = value;
                }
            }
        }

        /// <summary>
        /// Pitch, in degrees
        /// </summary>
        public float Pitch {
            get => _pitch;
            set {
                _pitch = Math.Clamp(value, -90, 90);
                s_lastPitch = _pitch;
            }
        }

        /// <summary>
        /// Yaw, in degrees
        /// </summary>
        public float Yaw {
            get => _yaw;
            set {
                _yaw = value - 360.0f * ((int) value / 360);
                s_lastYaw = _yaw;
            }
        }

        private MouseButtons _mouseButtons = MouseButtons.None;
        private Point? _mousePos = null;
        private Point? _tilePos = null;
        private Point? _lastMousePos = null;

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Focus();
            _mouseButtons |= e.Button;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            _mouseButtons &= ~e.Button;
        }

        private Dictionary<Keys, bool> _keysPressed = [];

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = true;
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (Enabled && Focused && Visible)
                _keysPressed[(Keys) ((int) keyData & 0xFFFF)] = true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            _keysPressed.Clear();
            _mouseButtons = MouseButtons.None;
            _lastMousePos = null;
        }

        private int _frame = 0;

        private void IncrementFrame() {
            if (!Visible)
                return;

            _frame = (_frame + 1) % 3600;
            CheckForKeys();
            UpdateAnimatedTextures();
        }

        private bool KeyIsDown(Keys keyCode)
            => Enabled && Focused && Visible && _keysPressed.ContainsKey(keyCode) && _keysPressed[keyCode];

        private float GetShiftFactor()
            => _keysPressed.TryGetValue(Keys.ShiftKey, out bool on) ? (on ? 3 : 1) : 1;

        private void CheckForKeys() {
            var keysDown = _keysPressed.Where(x => x.Value == true).Select(x => x.Key).ToHashSet();

            var moveUpKey    = keysDown.Contains(Keys.Subtract) || keysDown.Contains(Keys.R);
            var moveDownKey  = keysDown.Contains(Keys.Add) || keysDown.Contains(Keys.F);
            var moveLeftKey  = keysDown.Contains(Keys.End) || keysDown.Contains(Keys.NumPad1) || keysDown.Contains(Keys.A);
            var moveRightKey = keysDown.Contains(Keys.PageDown) || keysDown.Contains(Keys.NumPad3) || keysDown.Contains(Keys.D);

            int moveX = (moveLeftKey ? -1 : 0) + (moveRightKey ? 1 : 0);
            int moveY = (moveUpKey ? 1 : 0) + (moveDownKey ? -1 : 0);
            int moveZ = (keysDown.Contains(Keys.W) ? -1 : 0) + (keysDown.Contains(Keys.S) ? 1 : 0);

            var shiftFactor = GetShiftFactor();

            if (moveX != 0 || moveY != 0 || moveZ != 0) {
                var move = new Vector3(moveX, moveY * 0.5f, moveZ * 2.5f)
                    * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                    * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                Position += move * 0.25f * shiftFactor;
                Invalidate();
            }

            var rotateLeftKey  = keysDown.Contains(Keys.Left)  || keysDown.Contains(Keys.NumPad4);
            var rotateRightKey = keysDown.Contains(Keys.Right) || keysDown.Contains(Keys.NumPad6);

            int rotateYaw = (rotateLeftKey ? 1 : 0) + (rotateRightKey ? -1 : 0);
            if (rotateYaw != 0) {
                Yaw += rotateYaw * 1.0f * shiftFactor;
                Invalidate();
            }

            var rotateUpKey   = keysDown.Contains(Keys.Up)   || keysDown.Contains(Keys.NumPad8);
            var rotateDownKey = keysDown.Contains(Keys.Down) || keysDown.Contains(Keys.NumPad2);

            int rotatePitch = (rotateUpKey ? -1 : 0) + (rotateDownKey ? 1 : 0);
            if (rotatePitch != 0) {
                var oldPitch = Pitch;
                Pitch += rotatePitch * 0.5f * shiftFactor;
                if (oldPitch != Pitch)
                    Invalidate();
            }
        }

        private void UpdateAnimatedTextures() {
            if (_frame % 2 == 0)
                return;
            if (_surfaceModel?.UpdateAnimatedTextures() == true)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            _lastMousePos = null;
            _mouseButtons = MouseButtons.None;

            if (_mousePos != null)
                UpdateMousePosition(null);
            else
                UpdateTilePosition(null);
        }

        private (Vector3, float) GetCurrentTileTargetAndDistance() {
            if (!_tilePos.HasValue)
                throw new NullReferenceException(nameof(_tilePos));

            var tileVertices = GetTileVertices(_tilePos.Value);
            var target = new Vector3(
                _tilePos.Value.X + c_offX + 0.5f,
                tileVertices.Select(x => x.Y).Average(),
                _tilePos.Value.Y + c_offY + 0.5f);

            var dist = (float) Math.Sqrt(
                Math.Pow(target.X - Position.X, 2) +
                Math.Pow(target.Y - Position.Y, 2) +
                Math.Pow(target.Z - Position.Z, 2)
            );

            return (target, dist);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (_lastMousePos.HasValue && (_lastMousePos != e.Location)) {
                var shiftFactor = GetShiftFactor();
                var deltaX = (e.X - _lastMousePos.Value.X) * shiftFactor;
                var deltaY = (e.Y - _lastMousePos.Value.Y) * shiftFactor;

                // For middle+right drag, rotate around, keeping the tile over the mouse in place.
                if ((_mouseButtons & c_MouseMiddleRight) == c_MouseMiddleRight && _tilePos.HasValue) {
                    // TODO: SIMPLYIFY ALL THIS, AND IMPROVE IT!!!

                    var (target, dist) = GetCurrentTileTargetAndDistance();
                    var distVec = new Vector3(0, 0, dist);

                    var distForward = -distVec
                        * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                        * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                    var targetBackward = target - distForward;
                    var targetBackwardOffset = Position - targetBackward;

                    var deltaYaw   = deltaX / -10.0f;
                    var deltaPitch = deltaY / -10.0f;
                    Yaw   += deltaYaw;
                    Pitch += deltaPitch;

                    Position = target +
                        distVec
                            * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                            * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw))
                        + new Vector3(targetBackwardOffset.X, 0, targetBackwardOffset.Z)
                            // TODO: for some reason, applying pitch is really bogus here, so for now we don't have it :(
                            * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(deltaYaw))
                        + new Vector3(0, targetBackwardOffset.Y, 0);

                    Invalidate();
                }
                // For middle drag, pan around.
                else if ((_mouseButtons & MouseButtons.Middle) != 0) {
                    Position += new Vector3(deltaX / -40.0f, deltaY / 40.0f, 0)
                        * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                        * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
                    Invalidate();
                }
                // For middle drag, look around.
                else if ((_mouseButtons & MouseButtons.Right) != 0) {
                    Yaw   += deltaX / -10.0f;
                    Pitch += deltaY / -10.0f;
                    Invalidate();
                }
            }

            if (e.X < 0 || e.Y < 0 || e.X >= Width || e.Y >= Height)
                UpdateMousePosition(null);
            else
                UpdateMousePosition(e.Location);

            _lastMousePos = e.Location;
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            Position += new Vector3(0, 0, e.Delta / -50 * GetShiftFactor())
                * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
            Invalidate();
        }

        private void LookAtCurrentTileTarget() {
            if (!_tilePos.HasValue)
                return;

            var (target, dist) = GetCurrentTileTargetAndDistance();
            LookAtTarget(target);
            Invalidate();
        }

        private void PanToCurrentTileTarget() {
            if (!_tilePos.HasValue)
                return;

            var (target, dist) = GetCurrentTileTargetAndDistance();
            var distVec = new Vector3(0, 0, dist);
            var distForward = -distVec
                * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));

            Position = target - distForward;
            Invalidate();
        }

        protected void OnRightDoubleClick(EventArgs e)
            => LookAtCurrentTileTarget();

        protected void OnMiddleDoubleClick(EventArgs e)
            => PanToCurrentTileTarget();

        private void UpdateTilePosition(Point? pos) {
            // All invalid tile values should be 'null'.
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X >= WidthInTiles || pos.Value.Y >= HeightInTiles))
                pos = null;

            // Early exit if no change is necessary.
            if (_tilePos == pos)
                return;

            _tilePos = pos;

            _tileModel?.Dispose();
            _tileModel = null;

            if (_tilePos != null) {
                var quad = new Quad(GetTileVertices(_tilePos.Value));
                _tileModel = new QuadModel([quad]);
            }

            Invalidate();
        }

        private void UpdateMousePosition(Point? pos) {
            if (_mousePos == pos)
                return;

            _mousePos = pos;
            UpdateTilePosition();
        }

        private void UpdateTilePosition() {
            // Changing the tile is locked while rotating around a tile.
            if ((_mouseButtons & c_MouseMiddleRight) == c_MouseMiddleRight)
                return;

            if (_mousePos == null) {
                UpdateTilePosition(null);
                return;
            }

            var pixel = new byte[3];
            using (_selectFramebuffer.Use(FramebufferTarget.ReadFramebuffer))
                GL.ReadPixels(_mousePos.Value.X, Height - _mousePos.Value.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);

            if (pixel[2] == 255)
                UpdateTilePosition(null);
            else {
                UpdateTilePosition(new Point(
                    (int) Math.Round(pixel[0] / (255.0f / WidthInTiles)),
                    (int) Math.Round(pixel[1] / (255.0f / WidthInTiles))
                ));
            }
        }

        // TODO: temporary click function!! remove this when there's an actual 'edit' panel
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            if (_tilePos == null)
                return;

            System.Diagnostics.Debug.WriteLine("Tile: " + _tilePos.ToString());
            System.Diagnostics.Debug.Write(_debugText[_tilePos.Value.X, _tilePos.Value.Y]);
        }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private Shader _textureShader;
        private Shader _twoTextureShader;
        private Shader _solidShader;
        private Shader _normalsShader;
        private Shader _wireframeShader;

        private Framebuffer _selectFramebuffer;

        private QuadModel _surfaceModel;
        private QuadModel _untexturedSurfaceModel;
        private QuadModel _surfaceSelectionModel;
        private QuadModel _tileModel;
        private QuadModel _helpModel;

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

        private Timer _timer = null;

        private Texture _tileWireframeTexture = null;
        private Texture _whiteTexture         = null;
        private Texture _transparentTexture   = null;
        private Texture _tileHoverTexture     = null;
        private Texture _helpTexture          = null;

        private QuadModel[] _surfaceModels = null;
        private Shader[]  _shaders         = null;
        private Texture[] _textures        = null;
    }
}
