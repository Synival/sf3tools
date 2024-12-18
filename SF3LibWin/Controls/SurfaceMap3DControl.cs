using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Win.OpenGL;

namespace SF3.Win.Controls {
    public partial class SurfaceMap3DControl : GLControl {
        public SurfaceMap3DControl() {
            InitializeComponent();

            Disposed += (s, a) => {
                if (_model != null) {
                    _model.Dispose();
                    _model = null;
                }
                if (_shader != null) {
                    _shader.Dispose();
                    _shader = null;
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

            _shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");

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

            if (_shader != null) {
                _shader.Use();
                var handle = GL.GetUniformLocation(_shader.Handle, "projection");
                var matrix = Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                    0.1f, 300.0f);
                GL.UniformMatrix4(handle, false, ref matrix);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (_model == null)
                return;

            _shader.Use();
            var handle = GL.GetUniformLocation(_shader.Handle, "model");
            var matrix = Matrix4.Identity;
            GL.UniformMatrix4(handle, false, ref matrix);

            handle = GL.GetUniformLocation(_shader.Handle, "view");

            // Determine the view matrix for our position and orientation.
            matrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));

            GL.UniformMatrix4(handle, false, ref matrix);

            _model.Draw();

            SwapBuffers(); // Display the result.
        }

        public void UpdateModel(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks, TileSurfaceHeightmapRow[] heightmap) {
            MakeCurrent();

            if (_model != null) {
                _model.Dispose();
                _model = null;
                Invalidate();
            }

            if (textureData == null || heightmap == null)
                return;

            var quads = new List<Quad>();
            var offX = (float) textureData.GetLength(0) / -2;
            var offY = (float) textureData.GetLength(1) / -2;

            var texturesById = textureChunks
                .SelectMany(x => x.TextureTable.Rows)
                .ToDictionary(x => x.ID, x => x.Texture);

            for (var y = 0; y < textureData.GetLength(1); y++) {
                for (var x = 0; x < textureData.GetLength(1); x++) {
                    var key = textureData[x, y];
                    var textureId = textureData[x, y] & 0xFF;
                    var textureFlags = (byte) ((textureData[x, y] >> 8) & 0xFF);

                    if (textureId == 0xFF || !texturesById.ContainsKey(textureId))
                        continue;
                    var texture = texturesById[textureId];

                    var heights = heightmap[y][x];
                    quads.Add(new Quad([
                        new Vector3(x + 0 + offX, ((heights >>  8) & 0xFF) / 16f, y + 0 + offY),
                        new Vector3(x + 1 + offX, ((heights >>  0) & 0xFF) / 16f, y + 0 + offY),
                        new Vector3(x + 1 + offX, ((heights >> 24) & 0xFF) / 16f, y + 1 + offY),
                        new Vector3(x + 0 + offX, ((heights >> 16) & 0xFF) / 16f, y + 1 + offY)
                    ], texture, textureFlags));
                }
            }

            _model = new QuadModel(quads.ToArray());
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

        /// <summary>
        /// Pitch, in degrees
        /// </summary>
        public float Pitch { get; set; } = 0;

        /// <summary>
        /// Yaw, in degrees
        /// </summary>
        public float Yaw { get; set; } = 0;

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
                while (Yaw < 0)
                    Yaw += 360;
                while (Yaw >= 360)
                    Yaw -= 360;
                Invalidate();
            }

            var rotateUpKey   = keysDown.Contains(Keys.Up)   || keysDown.Contains(Keys.NumPad8);
            var rotateDownKey = keysDown.Contains(Keys.Down) || keysDown.Contains(Keys.NumPad2);

            int rotatePitch = (rotateUpKey ? -1 : 0) + (rotateDownKey ? 1 : 0);
            if (rotatePitch != 0) {
                var oldPitch = Pitch;
                Pitch = Math.Clamp(Pitch + rotatePitch * 0.5f * shiftFactor, -180, 180);
                if (oldPitch != Pitch)
                    Invalidate();
            }
        }

        private Shader _shader = null;
        private QuadModel _model = null;
        private Timer _timer = null;
    }
}
