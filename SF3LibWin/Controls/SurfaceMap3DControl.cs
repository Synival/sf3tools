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

            _timer = new Timer() { Interval = 1000 / 30 };
            _timer.Tick += (s, a) => UpdateCamera();
            _timer.Start();

            UpdateCamera();
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
            var matrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_frame * 0.2f));
            GL.UniformMatrix4(handle, false, ref matrix);

            handle = GL.GetUniformLocation(_shader.Handle, "view");
            matrix *= Matrix4.LookAt(new Vector3(0.0f, (float) Math.Sin(MathHelper.DegreesToRadians(_frame * 0.1f)) * 40.0f + 50.0f, 100.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
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

        private int _frame = 0;

        private void UpdateCamera() {
            if (_shader == null || !Visible)
                return;

            _frame = (_frame + 1) % 3600;
            Invalidate();
        }

        private Shader _shader = null;
        private QuadModel _model = null;
        private Timer _timer = null;
    }
}
