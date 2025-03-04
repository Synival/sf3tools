using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.Imaging;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables.MPD;
using SF3.Win.OpenGL.MPD_File;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class PDataViewerControl : GLControl {
        public PDataViewerControl() {
            InitializeComponent();
            MaximumSize = MinimumSize = new System.Drawing.Size(320, 320);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.Max);

            _general  = new GeneralResources();
            _models   = new ModelResources();
            _lighting = new LightingResources();

            _general.Init();
            _models.Init();
            _lighting.Init();

            _renderer = new Renderer();

            if (MPD_File != null && Models != null && _pdata != null)
                _models.Update(MPD_File, Models, _pdata);

            var lighting = ColorTable.Create(new ByteData.ByteData(new ByteArray(256)), "Colors", 0, 32);
            for (int i = 0; i < 32; i++) {
                byte level = (byte) (i * 255 / 32);
                var channels = new PixelChannels() { a = 255, r = level, g = level, b = level };
                lighting[i].ColorABGR1555 = channels.ToABGR1555();
            }
            _lighting.Update(lighting, null);

            Disposed += (s, e) => {
                _general?.Dispose();
                _models?.Dispose();
                _lighting?.Dispose();

                _general  = null;
                _models   = null;
                _lighting = null;
            };

            var lightPos = new Vector3(-0.632f, 0.632f, 0.447f);
            foreach (var shader in _general.Shaders) {
                using (shader.Use())
                    shader.UpdateUniform(ShaderUniformType.LightPosition, ref lightPos);
            }

            _general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, 0);
            _general.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            foreach (var shader in _general.Shaders) {
                shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            MakeCurrent();

            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Width);
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix =
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(22.50f), 1f, 0.05f, 65536.0f);
        }

        private void UpdateProjectionMatrices() {
            UpdateProjectionMatrix();
            if (_general?.Shaders?.Count > 0)
                foreach (var shader in _general.Shaders)
                    shader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref _projectionMatrix);
        }

        private void UpdateCameraPosition() {
            float size = 1.0f;
            Vector3 center = Vector3.Zero;

            if (_models != null && _pdata != null) {
                if (_models.VerticesByAddress.TryGetValue(_pdata.VerticesOffset, out var vertices)) {
                    var minX = vertices.Min(x => x.X) / 32.0f;
                    var maxX = vertices.Max(x => x.X) / 32.0f;
                    var minY = vertices.Min(x => x.Y) / 32.0f;
                    var maxY = vertices.Max(x => x.Y) / 32.0f;
                    var minZ = vertices.Min(x => x.Z) / 32.0f;
                    var maxZ = vertices.Max(x => x.Z) / 32.0f;

                    var width  = maxX - minX;
                    var height = maxY - minY;
                    var depth  = maxZ - minZ;
                    size = Math.Max(0.1f, Math.Max(width, Math.Max(height, depth)));

                    center = new Vector3((minX + maxX) / -2, (minY + maxY) / -2, (minZ + maxZ) / 2);
                }
            }

            Position = new Vector3(0.33f, 0.45f, 0.66f).Normalized() * (float) Math.Pow(size, 0.875f) * 5f;
            Pitch    = -MathHelper.RadiansToDegrees((float) Math.Atan2(Position.Y, double.Hypot(Position.X, Position.Z)));
            Yaw      =  MathHelper.RadiansToDegrees((float) Math.Atan2(Position.X, Position.Z));

            Position += center;
        }

        private void UpdateViewMatrix() {
            UpdateCameraPosition();
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        protected override void OnPaint(PaintEventArgs e) {
            MakeCurrent();

            UpdateViewMatrix();
            foreach (var shader in _general.Shaders)
                shader.UpdateUniform(ShaderUniformType.ViewMatrix, ref _viewMatrix);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _renderer.DrawScene(
                _general, _models, null, null, null, null, null, _lighting, null, null,
                new Renderer.RendererOptions() {
                    DrawModels            = true,
                    ApplyLighting         = true,
                    DrawWireframe         = true,
                    ForceTwoSidedTextures = true,
                },
                Yaw, Pitch, ClientSize.Width, ClientSize.Height,
                ref _projectionMatrix, ref _viewMatrix
            );

            SwapBuffers();
        }

        public IMPD_File MPD_File { get; private set; } = null;
        public ModelCollection Models { get; private set; } = null;

        private PDataModel _pdata = null;

        public void Update(IMPD_File mpdFile, ModelCollection models, PDataModel pdata) {
            if (Models == models && _pdata == pdata)
                return;

            MPD_File = mpdFile;
            Models = models;
            _pdata = pdata;

            if (_models != null) {
                _models.Reset();
                if (MPD_File != null && Models != null && _pdata != null)
                    _models.Update(MPD_File, Models, _pdata);
            }

            Invalidate();
        }

        public Vector3 Position { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private GeneralResources  _general  = null;
        private ModelResources    _models   = null;
        private LightingResources _lighting = null;

        private Renderer _renderer = null;
    }
}
