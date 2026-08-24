using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Win.Extensions;
using SF3.Win.OpenGL.GLResources.Shared;
using SF3.Win.OpenGL.Renderers.SGL_Model;
using SF3.Win.OpenGL.Renderers.Shared;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class SGL_ModelViewerControl : GLControl {
        public SGL_ModelViewerControl() {
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
            _screen   = new ScreenResources();
            _models   = new ModelResources(false, false);
            _lighting = new LightingResources();

            _general.Init();
            _screen.Init();
            _models.Init();
            _lighting.Init();

            _renderer = new SGL_ModelRenderer();

            if (TextureContainer != null && _sglModel != null)
                Update(TextureContainer, _sglModel);

            var lighting = new Palette(Enumerable.Range(0, 32)
                .Select(i => {
                    var level = i / 31f;
                    return new PixelChannels() {
                        A = 255,
                        R = (byte) ((level * 0.75f  + 0.125f) * 255),
                        G = (byte) ((level * 0.50f +  0.25f)  * 255),
                        B = (byte) ((level * 0.25f  + 0.375f) * 255)
                    };
                })
                .ToArray()
            );

            _lighting.Update(lighting, null);

            UpdateLightPos();

            _general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, 0);
            _general.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            foreach (var shader in _general.Shaders) {
                shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }

            if (_globalTimer == null) {
                // TODO: use good timer code from ViewerGLControl
                // TODO: get this rendering at 60fps
                _globalTimer = new Timer() { Interval = 1000 / 45 };
                _globalTimer.Tick += (s, a) => Yaw = (Yaw + 1.0f) % 360f;
                _globalTimer.Start();
            }

            // TODO: use good timer code from ViewerGLControl
            // TODO: get this rendering at 60fps
            _timer = new Timer() { Interval = 1000 / 45 };
            _timer.Tick += (s, a) => IncrementFrame();
            _timer.Start();

            Disposed += (s, e) => {
                _general?.Dispose();
                _screen?.Dispose();
                _models?.Dispose();
                _lighting?.Dispose();
                _timer?.Dispose();

                _general  = null;
                _screen   = null;
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
            var yawRadians = MathHelper.DegreesToRadians(Yaw);

            Position = new Vector3(0.66f * (float) Math.Sin(yawRadians), 0.45f, 0.66f * (float) Math.Cos(yawRadians)).Normalized() * _dist;
            Pitch = -MathHelper.RadiansToDegrees((float) Math.Atan2(Position.Y, double.Hypot(Position.X, Position.Z)));

            Position += _center;
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
                new SGL_ModelRendererResources() {
                    General  = _general,
                    Screen   = _screen,
                    Models   = _models,
                    Lighting = _lighting,
                },
                new RendererOptions() {
                    DrawModels      = true,
                    DrawExtraModels = true,
                    ApplyLighting   = true,
                    DrawWireframe   = true,
                    SmoothLighting  = true,
                },
                new RendererState() {
                    CameraYaw        = Yaw,
                    CameraPitch      = Pitch,
                    ScreenWidth      = ClientSize.Width,
                    ScreenHeight     = ClientSize.Height,
                    ProjectionMatrix = _projectionMatrix,
                    ViewMatrix       = _viewMatrix,
                }
            );

            SwapBuffers();
        }

        public ITextureContainer TextureContainer { get; private set; } = null;
        private ISGL_Model _sglModel = null;

        public void Update(
            ITextureContainer texContainer, ISGL_Model sglModel,
            float rotX = 0f, float rotY = 0f, float rotZ = 0f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f
        ) {
            if (_sglModel == sglModel)
                return;

            TextureContainer = texContainer;
            _sglModel        = sglModel;
            _vertices        = null;
            _size            = 1.0f;
            _center          = new Vector3();
            _dist            = 1.0f;

            if (_models != null) {
                _models.Reset();
                if (TextureContainer != null && sglModel != null) {
                    var texturesById = texContainer.GetAnimatableTexturesByModelCollectionID(sglModel?.ModelCollectionID ?? -1);
                    _models.Update(
                        sglModel, texturesById, () => {
                            return new SGL_ModelInstance((mi, lod) => sglModel) {
                                ModelCollectionID = sglModel.ModelCollectionID,
                                ModelID = sglModel.ModelID,
                                PositionX = 32 * 32,
                                PositionZ = 32 * 32,
                                AngleX = rotX,
                                AngleY = rotY,
                                AngleZ = rotZ,
                                ScaleX = scaleX,
                                ScaleY = scaleY,
                                ScaleZ = scaleZ,
                            };
                        },
                        forceSemiTransparentValue: null, isHideMesh: false
                    );

                    var verticesMatrix =
                        Matrix3.CreateScale(scaleX, scaleY, scaleZ) *
                        Matrix3.CreateRotationX(rotX * (float) Math.PI / 180.0f) *
                        Matrix3.CreateRotationY(rotY * (float) Math.PI / 180.0f) *
                        Matrix3.CreateRotationZ(rotZ * (float) Math.PI / 180.0f);

                    _vertices = sglModel.Vertices.Select(x => x.ToVector3() * verticesMatrix).ToArray();

                    _minX = _vertices.Min(x => x.X) / 32.0f;
                    _minY = _vertices.Min(x => x.Y) / 32.0f;
                    _minZ = _vertices.Min(x => x.Z) / 32.0f;

                    _maxX = _vertices.Max(x => x.X) / 32.0f;
                    _maxY = _vertices.Max(x => x.Y) / 32.0f;
                    _maxZ = _vertices.Max(x => x.Z) / 32.0f;

                    _width  = _maxX - _minX;
                    _height = _maxY - _minY;
                    _depth  = _maxZ - _minZ;

                    _size   = Math.Max(0.1f, Math.Max(_width, Math.Max(_height, _depth)));
                    _center = new Vector3((_minX + _maxX) / 2, (_minY + _maxY) / -2, (_minZ + _maxZ) / -2);

                    _dist = (float) Math.Pow(_size, 0.875f) * 4f;
                }
                else {
                    // TODO: throw?? what to do here???
                }
            }

            Invalidate();
        }

        private void IncrementFrame() {
            if (!Visible)
                return;

            // TODO: this doesn't update at 30fps, please fix!
            if (_sglModel != null) {
                var collectionId = _sglModel.ModelCollectionID;
                foreach (var modelGroup in _models.ModelGroupsByIDByCollection[collectionId].Values)
                    foreach (var model in modelGroup.Models)
                        _ = model.UpdateAnimatedTextures();
            }

            Invalidate();
        }

        public Vector3 Position { get; private set; }
        public static float Yaw { get; private set; }
        public float Pitch { get; private set; }

        private float _minX = 0f;
        private float _minY = 0f;
        private float _minZ = 0f;

        private float _maxX = 0f;
        private float _maxY = 0f;
        private float _maxZ = 0f;

        private float _width  = 0f;
        private float _height = 0f;
        private float _depth  = 0f;

        private float _size = 0f;
        private Vector3 _center;
        private float _dist = 0f;

        private Vector3[] _vertices = null;
        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private GeneralResources   _general  = null;
        private ScreenResources    _screen   = null;
        private ModelResources     _models   = null;
        private LightingResources  _lighting = null;

        private SGL_ModelRenderer _renderer = null;
        private Timer _timer = null;

        private static Timer _globalTimer = null;
    }
}
