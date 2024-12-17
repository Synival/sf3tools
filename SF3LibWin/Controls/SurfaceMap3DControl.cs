using System;
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

            var quad = new Quad([
                new Vector3(-1.0f,  0.0f,  1.0f),
                new Vector3(-1.0f,  0.0f, -1.0f),
                new Vector3(1.0f,  0.0f, -1.0f),
                new Vector3(1.0f,  0.0f,  1.0f)
            ]);
            _model = new QuadModel([quad]);

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
                    MathHelper.DegreesToRadians(45.0f), (float) ClientSize.Width / ClientSize.Height,
                    0.1f, 100.0f);
                GL.UniformMatrix4(handle, false, ref matrix);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            var handle = GL.GetUniformLocation(_shader.Handle, "model");
            var matrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_frame * 5));
            GL.UniformMatrix4(handle, false, ref matrix);

            handle = GL.GetUniformLocation(_shader.Handle, "view");
            matrix *= Matrix4.LookAt(new Vector3(0.0f, (float) Math.Sin(MathHelper.DegreesToRadians(_frame)) * 4.0f + 5.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.UniformMatrix4(handle, false, ref matrix);

            _model.Draw();

            SwapBuffers(); // Display the result.
        }

        public void UpdateModel(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks, TileSurfaceHeightmapRow[] heightmap) {
            // TODO: actual work!
            Invalidate();
        }

        private int _frame = 0;

        private void UpdateCamera() {
            if (_shader == null || !Visible)
                return;

            _frame = (_frame + 1) % 360;
            Invalidate();
        }

        private Shader _shader = null;
        private QuadModel _model = null;
        private Timer _timer = null;
    }
}
