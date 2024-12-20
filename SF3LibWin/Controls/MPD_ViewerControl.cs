using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.Win.Extensions;
using SF3.Win.OpenGL;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : GLControl {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;
        private const float c_offX = WidthInTiles / -2f;
        private const float c_offY = HeightInTiles / -2f;
        private const MouseButtons c_MouseMiddleRight = MouseButtons.Middle | MouseButtons.Right;

        public MPD_ViewerControl() {
            InitializeComponent();

            Disposed += (s, a) => {
                if (RenderModel != null) {
                    RenderModel.Dispose();
                    RenderModel = null;
                }
                if (SelectModel != null) {
                    SelectModel.Dispose();
                    SelectModel = null;
                }
                if (TileModel != null) {
                    TileModel.Dispose();
                    TileModel = null;
                }

                if (TexturedShader != null) {
                    TexturedShader.Dispose();
                    TexturedShader = null;
                }
                if (SolidShader != null) {
                    SolidShader.Dispose();
                    SolidShader = null;
                }

                if (_framebufferHandle != 0) {
                    GL.DeleteFramebuffer(_framebufferHandle);
                    _framebufferHandle = 0;
                }
                if (_framebufferColorTexture != null) {
                    _framebufferColorTexture.Dispose();
                    _framebufferColorTexture = null;
                }
                if (_framebufferDepthStencilTexture != null) {
                    _framebufferDepthStencilTexture.Dispose();
                    _framebufferDepthStencilTexture = null;
                }

                if (_timer != null) {
                    _timer.Dispose();
                    _timer = null;
                }
            };
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

            TexturedShader = new Shader("Shaders/Textured.vert", "Shaders/Textured.frag");
            SolidShader    = new Shader("Shaders/Solid.vert", "Shaders/Solid.frag");

            using (var tileHoverImage = (Bitmap) Image.FromFile("Images/TileHover.bmp")) {
                var tileHoverTexture = tileHoverImage.CreateTextureABGR1555(9999, 0, 0);
                _tileHoverTextureAnimation = new TextureAnimation(tileHoverTexture.ID, [tileHoverTexture]);
            }

            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();
            _timer.Start();

            Position = new Vector3(0, 60, 120);
            LookAtTarget(new Vector3(0, 5, 0));

            UpdateFramebuffer();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

            UpdateFramebuffer();
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void UpdateFramebuffer() {
            int width = Width;
            int height = Height;

            // Update color texture.
            if (_framebufferColorTexture != null)
                _framebufferColorTexture.Dispose();
            _framebufferColorTexture = new Texture(width, height, PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte);

            // Update depth/stencil texture.
            if (_framebufferDepthStencilTexture != null)
                _framebufferDepthStencilTexture.Dispose();
            _framebufferDepthStencilTexture = new Texture(width, height, PixelInternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);

            // (Re)create the framebuffer.
            if (_framebufferHandle != 0)
                GL.DeleteFramebuffer(_framebufferHandle);
            _framebufferHandle = GL.GenFramebuffer();

            // Attach txtures to the framebuffer.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferHandle);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _framebufferColorTexture.Handle, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2D, _framebufferDepthStencilTexture.Handle, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        private void UpdateProjectionMatrices() {
            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                0.1f, 300.0f);

            if (TexturedShader != null) {
                TexturedShader.Use();
                var handle = GL.GetUniformLocation(TexturedShader.Handle, "projection");
                GL.UniformMatrix4(handle, false, ref projectionMatrix);
            }

            if (SolidShader != null) {
                SolidShader.Use();
                var handle = GL.GetUniformLocation(SolidShader.Handle, "projection");
                GL.UniformMatrix4(handle, false, ref projectionMatrix);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            MakeCurrent();

            UpdateShaderMVP(TexturedShader);
            UpdateShaderMVP(SolidShader);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferHandle);
            GL.ClearColor(1, 1, 1, 1);
            GL.Enable(EnableCap.CullFace);
            DrawScene(SelectModel, false);
            GL.Disable(EnableCap.CullFace);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            UpdateTilePosition();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            DrawScene(RenderModel, true);

            SwapBuffers();
        }

        private void UpdateShaderMVP(Shader shader) {
            shader.Use();

            var handle = GL.GetUniformLocation(shader.Handle, "model");
            var matrix = Matrix4.Identity;
            GL.UniformMatrix4(handle, false, ref matrix);

            handle = GL.GetUniformLocation(shader.Handle, "view");
            matrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
            GL.UniformMatrix4(handle, false, ref matrix);
        }

        private void DrawScene(QuadModel model, bool isVisible) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (model != null) {
                model.Shader.Use();
                model.Draw();
            }

            if (isVisible) {
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

                if (TileModel != null) {
                    GL.Disable(EnableCap.DepthTest);
                    TileModel.Shader.Use();
                    TileModel.Draw();
                    GL.Enable(EnableCap.DepthTest);
                }
            }
        }

        private Vector3[] GetTileVertices(Point pos) {
            var heights = _heightmap[pos.X, pos.Y];
            return [
                (pos.X + 0 + c_offX, ((heights >>  8) & 0xFF) / 16f, pos.Y + 0 + c_offY),
                (pos.X + 1 + c_offX, ((heights >>  0) & 0xFF) / 16f, pos.Y + 0 + c_offY),
                (pos.X + 1 + c_offX, ((heights >> 24) & 0xFF) / 16f, pos.Y + 1 + c_offY),
                (pos.X + 0 + c_offX, ((heights >> 16) & 0xFF) / 16f, pos.Y + 1 + c_offY)
            ];
        }

        public void UpdateModel(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks, TextureAnimationTable textureAnimations, TileSurfaceHeightmapRow[] heightmap) {
            MakeCurrent();

            for (var y = 0; y < HeightInTiles; y++)
                for (var x = 0; x < WidthInTiles; x++)
                    _heightmap[x, y] = heightmap[y][x];

            if (RenderModel != null) {
                RenderModel.Dispose();
                RenderModel= null;
                Invalidate();
            }
            if (SelectModel != null) {
                SelectModel.Dispose();
                SelectModel = null;
            }

            var texturesById = (textureChunks != null) ? textureChunks
                .SelectMany(x => x.TextureTable.Rows)
                .GroupBy(x => x.ID)
                .Select(x => x.First())
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = (textureAnimations != null) ? textureAnimations.Rows
                .GroupBy(x => x.TextureID)
                .Select(x => x.First())
                .ToDictionary(x => (int) x.TextureID, x => x.Frames.OrderBy(x => x.FrameNum).Select(x => x.Texture).ToArray())
                : [];

            var renderQuads = new List<Quad>();
            var selectQuads = new List<Quad>();

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

                    var vertices = GetTileVertices(new Point(x, y));
                    if (anim != null)
                        renderQuads.Add(new Quad(vertices, anim, textureFlags));

                    selectQuads.Add(new Quad(vertices, new Vector3(x / (float) WidthInTiles, y / (float) HeightInTiles, 0)));
                }
            }

            if (renderQuads.Count > 0)
                RenderModel = new QuadModel(renderQuads.ToArray(), TexturedShader);
            if (selectQuads.Count > 0)
                SelectModel = new QuadModel(selectQuads.ToArray(), SolidShader);

            Invalidate();
        }

        public void LookAtTarget(Vector3 target) {
            // Calculate the pitch/yaw to point the camera to that target.
            var posMinusTarget = Position - target;
            Pitch = -MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.Y, double.Hypot(posMinusTarget.X, posMinusTarget.Z)));
            Yaw   =  MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.X, posMinusTarget.Z));
        }

        /// <summary>
        /// Position of the camera
        /// </summary>
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        private float _pitch = 0;
        private float _yaw = 0;

        /// <summary>
        /// Pitch, in degrees
        /// </summary>
        public float Pitch {
            get => _pitch;
            set => _pitch = Math.Clamp(value, -90, 90);
        }

        /// <summary>
        /// Yaw, in degrees
        /// </summary>
        public float Yaw {
            get => _yaw;
            set => _yaw = value - 360.0f * ((int) value / 360);
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

            var moveUpKey    = keysDown.Contains(Keys.Subtract) || keysDown.Contains(Keys.Space);
            var moveDownKey  = keysDown.Contains(Keys.Add) || keysDown.Contains(Keys.ControlKey);
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
            if (RenderModel?.UpdateAnimatedTextures() == true)
                Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);
            _mouseIsOver = true;
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            _mouseIsOver = false;
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

        private bool _mouseIsOver = false;

        private void UpdateTilePosition(Point? pos) {
            // All invalid tile values should be 'null'.
            if (pos.HasValue && (pos.Value.X < 0 || pos.Value.Y < 0 || pos.Value.X >= WidthInTiles || pos.Value.Y >= HeightInTiles))
                pos = null;

            // Early exit if no change is necessary.
            if (_tilePos == pos)
                return;

            _tilePos = pos;

            TileModel?.Dispose();
            TileModel = null;

            if (_tilePos != null) {
                var quad = new Quad(GetTileVertices(_tilePos.Value), _tileHoverTextureAnimation, 0);
                TileModel = new QuadModel([quad], TexturedShader);
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
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _framebufferHandle);
            GL.ReadPixels(_mousePos.Value.X, Height - _mousePos.Value.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

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
            if (_tilePos != null)
                System.Diagnostics.Debug.WriteLine("Tile: " + _tilePos.ToString());
        }

        public Shader TexturedShader { get; private set; }
        public Shader SolidShader { get; private set; }

        public QuadModel RenderModel { get; private set; }
        public QuadModel SelectModel { get; private set; }
        public QuadModel TileModel { get; private set; }

        private Timer _timer = null;

        private uint[,] _heightmap = new uint[WidthInTiles, HeightInTiles];
        private TextureAnimation _tileHoverTextureAnimation = null;

        private int _framebufferHandle = 0;
        private Texture _framebufferColorTexture = null;
        private Texture _framebufferDepthStencilTexture = null;
    }
}
