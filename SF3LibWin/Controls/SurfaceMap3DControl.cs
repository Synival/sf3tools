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
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MakeCurrent();

            _shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _timer = new Timer() { Interval = 1000 / 30 };
            _timer.Tick += (s, a) => UpdateCamera();
            _timer.Start();
            UpdateCamera();

            Disposed += (s, a) => {
                _shader.Dispose();
                _shader = null;
                GL.DeleteBuffer(_elementBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_vertexBufferObject);
                _timer.Dispose();
                _timer = null;
            };
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

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

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

        private float[] _vertices = {
            -1.0f,  0.0f,  1.0f,
            -1.0f,  0.0f, -1.0f,
             1.0f,  0.0f, -1.0f,
             1.0f,  0.0f,  1.0f,
        };

        private uint[] _indices = {
            0, 1, 3,
            1, 2, 3
        };

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;

        private Shader _shader;
        private Timer _timer;
    }
}
