using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.Win.OpenGL;

namespace SF3.Win.Controls {
    public partial class SurfaceMap3DControl : GLControl {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        public SurfaceMap3DControl() {
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
                if (_timer != null) {
                    _timer.Dispose();
                    _timer = null;
                }
            };
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            TexturedShader = new Shader("Shaders/Textured.vert", "Shaders/Textured.frag");
            SolidShader    = new Shader("Shaders/Solid.vert",    "Shaders/Solid.frag");

            _timer = new Timer() { Interval = 1000 / 60 };
            _timer.Tick += (s, a) => IncrementFrame();
            _timer.Start();

            Position = new Vector3(0, 50, 100);
            LookAtTarget(new Vector3(0, 10, 0));

            UpdateFramebuffer();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

            UpdateFramebuffer();
            UpdateProjectionMatrices();
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

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            DrawScene(RenderModel);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferHandle);
            GL.ClearColor(1, 1, 1, 1);
            DrawScene(SelectModel);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            SwapBuffers();
        }

        private void DrawScene(QuadModel model) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (model == null)
                return;

            model.Shader.Use();

            var handle = GL.GetUniformLocation(model.Shader.Handle, "model");
            var matrix = Matrix4.Identity;
            GL.UniformMatrix4(handle, false, ref matrix);

            handle = GL.GetUniformLocation(model.Shader.Handle, "view");
            matrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
            GL.UniformMatrix4(handle, false, ref matrix);

            model.Draw();
        }

        public void UpdateModel(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks, TextureAnimationTable textureAnimations, TileSurfaceHeightmapRow[] heightmap) {
            MakeCurrent();

            if (RenderModel != null) {
                RenderModel.Dispose();
                RenderModel= null;
                Invalidate();
            }
            if (SelectModel != null) {
                SelectModel.Dispose();
                SelectModel = null;
            }

            const float offX = -32.0f;
            const float offY = -32.0f;

            var texturesById = (textureChunks != null) ? textureChunks
                .SelectMany(x => x.TextureTable.Rows)
                .ToDictionary(x => x.ID, x => x.Texture)
                : [];

            var animationsById = (textureAnimations != null) ? textureAnimations.Rows
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

                    var heights = heightmap[y][x];
                    var vertices = new Vector3[] {
                        (x + 0 + offX, ((heights >>  8) & 0xFF) / 16f, y + 0 + offY),
                        (x + 1 + offX, ((heights >>  0) & 0xFF) / 16f, y + 0 + offY),
                        (x + 1 + offX, ((heights >> 24) & 0xFF) / 16f, y + 1 + offY),
                        (x + 0 + offX, ((heights >> 16) & 0xFF) / 16f, y + 1 + offY)
                    };

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

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Focus();
        }

        private Dictionary<Keys, bool> _keysPressed = [];

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = true;
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            _keysPressed[(Keys) ((int) e.KeyCode & 0xFFFF)] = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            _keysPressed[(Keys) ((int) keyData & 0xFFFF)] = true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            _keysPressed.Clear();
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
            => Visible && _keysPressed.ContainsKey(keyCode) && _keysPressed[keyCode];

        private void CheckForKeys() {
            var keysDown = _keysPressed.Where(x => x.Value == true).Select(x => x.Key).ToHashSet();

            var moveUpKey    = keysDown.Contains(Keys.Subtract) || keysDown.Contains(Keys.Space);
            var moveDownKey  = keysDown.Contains(Keys.Add) || keysDown.Contains(Keys.ControlKey);
            var moveLeftKey  = keysDown.Contains(Keys.End) || keysDown.Contains(Keys.NumPad1) || keysDown.Contains(Keys.A);
            var moveRightKey = keysDown.Contains(Keys.PageDown) || keysDown.Contains(Keys.NumPad3) || keysDown.Contains(Keys.D);

            int moveX = (moveLeftKey ? -1 : 0) + (moveRightKey ? 1 : 0);
            int moveY = (moveUpKey ? 1 : 0) + (moveDownKey ? -1 : 0);
            int moveZ = (keysDown.Contains(Keys.W) ? -1 : 0) + (keysDown.Contains(Keys.S) ? 1 : 0);

            var shiftFactor = keysDown.Contains(Keys.ShiftKey) ? 3 : 1;

            if (moveX != 0 || moveY != 0 || moveZ != 0) {
                var move = new Vector3(moveX, moveY * 0.5f, moveZ)
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

        private int? _tileX = null;
        private int? _tileY = null;

        private void UpdateTilePosition(int? x, int? y) {
            // All invalid tile values should be -1, -1.
            if (x < 0 || y < 0 || x >= WidthInTiles || y >= HeightInTiles) {
                x = null;
                y = null;
            }

            // Early exit if no change is necessary.
            if (_tileX == x && _tileY == y)
                return;

            _tileX = x;
            _tileY = y;

            System.Diagnostics.Debug.WriteLine(_tileX.ToString() + ", " + _tileY.ToString());
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            UpdateTilePosition(null, null);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (e.X < 0 || e.Y < 0 || e.X >= Width || e.Y >= Height)
                return;

            var pixel = new byte[3];
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _framebufferHandle);
            GL.ReadPixels(e.X, Height - e.Y - 1, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

            if (pixel[2] == 255)
                UpdateTilePosition(null, null);
            else
                UpdateTilePosition((int) Math.Round(pixel[0] / (255.0f / WidthInTiles)), (int) Math.Round(pixel[1] / (255.0f / WidthInTiles)));
        }

        public Shader TexturedShader { get; private set; }
        public Shader SolidShader { get; private set; }

        public QuadModel RenderModel { get; private set; }
        public QuadModel SelectModel { get; private set; }

        private Timer _timer = null;

        private int _framebufferHandle = 0;
        private Texture _framebufferColorTexture = null;
        private Texture _framebufferDepthStencilTexture = null;
    }
}
