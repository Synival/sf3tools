// TODO: for testing, please remove!
//#define USE_SOLID_SHADER

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
                    TexturedShader= null;
                }
                if (SolidShader != null) {
                    SolidShader.Dispose();
                    SolidShader= null;
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

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
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
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (RenderModel != null) {
                RenderModel.Shader.Use();

                var handle = GL.GetUniformLocation(RenderModel.Shader.Handle, "model");
                var matrix = Matrix4.Identity;
                GL.UniformMatrix4(handle, false, ref matrix);

                handle = GL.GetUniformLocation(RenderModel.Shader.Handle, "view");
                matrix = Matrix4.CreateTranslation(-Position)
                    * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                    * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
                GL.UniformMatrix4(handle, false, ref matrix);

                RenderModel.Draw();
            }

            SwapBuffers(); // Display the result.
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

            const int tileWidth = 64;
            const int tileHeight = 64;
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

            for (var y = 0; y < tileWidth; y++) {
                for (var x = 0; x < tileHeight; x++) {
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

                    selectQuads.Add(new Quad(vertices, new Vector3(x / 64.0f, y / 64.0f, 0)));
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

        public Shader TexturedShader { get; private set; }
        public Shader SolidShader { get; private set; }

        public QuadModel RenderModel { get; private set; }
        public QuadModel SelectModel { get; private set; }

        private Timer _timer = null;
    }
}
