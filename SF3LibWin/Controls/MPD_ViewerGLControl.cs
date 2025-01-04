﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl : GLControl {
        private const MouseButtons c_MouseMiddleRight = MouseButtons.Middle | MouseButtons.Right;

        private static Vector3? s_lastPosition  = null;
        private static float? s_lastPitch       = null;
        private static float? s_lastYaw         = null;

        public MPD_ViewerGLControl() {
            InitializeComponent();

            Disposed += (s, a) => {
                _world?.Dispose();
                _surfaceModel?.Dispose();
                _surfaceEditor?.Dispose();

                _world        = null;
                _surfaceModel = null;
                _surfaceEditor       = null;

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

            _world         = new WorldResources();
            _surfaceModel  = new SurfaceModelResources();
            _surfaceEditor = new SurfaceEditorResources();

            _world.Init();
            _surfaceModel.Init();
            _surfaceEditor.Init();

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

            foreach (var shader in _world.Shaders)
                UpdateShaderModelMatrix(shader, Matrix4.Identity);
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

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
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

        public void UpdateSurfaceModels() {
            MakeCurrent();
            _surfaceModel.Update(Model);
            Invalidate();
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
            if (_surfaceModel?.Model?.UpdateAnimatedTextures() == true)
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

            var tileVertices = SurfaceModelResources.GetTileVertices(Model, _tilePos.Value);
            var target = new Vector3(
                _tilePos.Value.X + WorldResources.ModelOffsetX + 0.5f,
                tileVertices.Select(x => x.Y).Average(),
                _tilePos.Value.Y + WorldResources.ModelOffsetZ + 0.5f);

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
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X > 63 || pos.Value.Y > 63))
                pos = null;

            // Early exit if no change is necessary.
            if (_tilePos == pos)
                return;

            _tilePos = pos;
            _surfaceEditor.UpdateTileModel(Model, _world, _tilePos);

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
                    (int) Math.Round(pixel[0] / (255.0f / SurfaceModelResources.WidthInTiles)),
                    (int) Math.Round(pixel[1] / (255.0f / SurfaceModelResources.HeightInTiles))
                ));
            }
        }

        // TODO: temporary click function!! remove this when there's an actual 'edit' panel
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            if (_tilePos == null)
                return;

            System.Diagnostics.Debug.WriteLine("Tile: " + _tilePos.ToString());
            System.Diagnostics.Debug.Write(_surfaceModel.TileDebugText[_tilePos.Value.X, _tilePos.Value.Y]);
        }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private Framebuffer _selectFramebuffer;

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

        private WorldResources _world = null;
        private SurfaceModelResources _surfaceModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
    }
}
