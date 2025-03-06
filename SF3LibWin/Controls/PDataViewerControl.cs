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
                var level = i / 31f;
                var channels = new PixelChannels() {
                    a = 255,
                    r = (byte) ((level * 0.75f  + 0.125f) * 255),
                    g = (byte) ((level * 0.50f +  0.25f)  * 255),
                    b = (byte) ((level * 0.25f  + 0.375f) * 255)
                };
                lighting[i].ColorABGR1555 = channels.ToABGR1555();
            }
            _lighting.Update(lighting, null);

            UpdateLightPos();

            _general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, 0);
            _general.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            foreach (var shader in _general.Shaders) {
                shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }

            // TODO: use good timer code from ViewerGLControl
            // TODO: get this rendering at 60fps
            _timer = new Timer() { Interval = 1000 / 45 };
            _timer.Tick += (s, a) => IncrementFrame();
            _timer.Start();

            Disposed += (s, e) => {
                _general?.Dispose();
                _models?.Dispose();
                _lighting?.Dispose();
                _timer?.Dispose();

                _general  = null;
                _models   = null;
                _lighting = null;
                _timer    = null;
            };
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

        private void UpdateLightPos() {
            var lightPos = new Vector3(-1.00f, 0.50f, 0.50f).Normalized()
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));

            foreach (var shader in _general.Shaders) {
                using (shader.Use())
                    shader.UpdateUniform(ShaderUniformType.LightPosition, ref lightPos);
            }
        }

        private void UpdateCameraPosition() {
            float size = 1.0f;
            Vector3 center = Vector3.Zero;

            if (_models != null && _pdata != null && Models != null) {
                if (_models.VerticesByAddressByCollection[Models.CollectionType].TryGetValue(_pdata.VerticesOffset, out var vertices)) {
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

            var dist = (float) Math.Pow(size, 0.875f) * 4f;
            var yawRadians = MathHelper.DegreesToRadians(Yaw);

            Position = new Vector3(0.66f * (float) Math.Sin(yawRadians), 0.45f, 0.66f * (float) Math.Cos(yawRadians)).Normalized() * dist;
            Pitch = -MathHelper.RadiansToDegrees((float) Math.Atan2(Position.Y, double.Hypot(Position.X, Position.Z)));

            Position += center;
        }

        private void UpdateViewMatrix() {
            UpdateCameraPosition();
            UpdateLightPos();
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
                _general, _models, null, null, null, null, null, _lighting, null, null, null,
                new Renderer.RendererOptions() {
                    DrawModels     = true,
                    ApplyLighting  = true,
                    DrawWireframe  = true,
                    SmoothLighting = true,
                },
                Yaw, Pitch, ClientSize.Width, ClientSize.Height,
                ref _projectionMatrix, ref _viewMatrix
            );

            SwapBuffers();
        }

        public IMPD_File MPD_File { get; private set; } = null;
        public ModelCollection Models { get; private set; } = null;

        private PDataModel _pdata = null;

        public void Update(IMPD_File mpdFile, PDataModel pdata) {
            if (_pdata == pdata)
                return;

            MPD_File = mpdFile;
            _pdata = pdata;
            Models = (pdata == null) ? null : mpdFile.ModelCollections.FirstOrDefault(x => x.PDatasByMemoryAddress.ContainsKey(pdata.RamAddress));

            if (_models != null) {
                _models.Reset();
                if (MPD_File != null && Models != null && _pdata != null)
                    _models.Update(MPD_File, Models, _pdata);
            }

            Invalidate();
        }

        private void IncrementFrame() {
            if (!Visible)
                return;

            Yaw = (Yaw + 1.0f) % 360f;

            // TODO: this doesn't update at 30fps, please fix!
            if (_models != null && Models != null)
                foreach (var modelGroup in _models.ModelsByMemoryAddressByCollection[Models.CollectionType].Values)
                    foreach (var model in modelGroup.Models)
                        model.UpdateAnimatedTextures();

            Invalidate();
        }

        public Vector3 Position { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private GeneralResources  _general  = null;
        private ModelResources    _models   = null;
        private LightingResources _lighting = null;

        private Renderer _renderer = null;
        private Timer _timer = null;
    }
}
