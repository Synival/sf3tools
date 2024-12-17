using System;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Win.Shaders;

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

            Disposed += (s, a) => {
                _shader.Dispose();
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_vertexBufferObject);
            };
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vertexArrayObject);
            _shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers(); // Display the result.
        }

        public void UpdateModel(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks, TileSurfaceHeightmapRow[] heightmap) {
            // TODO: actual work!
            Invalidate();
        }

        private readonly float[] _vertices = {
            -0.25f,  0.5f, 0.0f, // Top-left vertex
            -0.5f,  -0.5f, 0.0f, // Bottom-left vertex
             0.5f,  -0.5f, 0.0f, // Bottom-right vertex
             0.25f,  0.5f, 0.0f, // Top-right vertex
        };

        private readonly uint[] _indices = {
            0, 1, 3,
            1, 2, 3
        };

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;

        private Shader _shader;
    }
}
