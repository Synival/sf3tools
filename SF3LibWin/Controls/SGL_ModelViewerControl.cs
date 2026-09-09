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
using SF3.Win.Utils;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public partial class SGL_ModelViewerControl : GLControl {
        public SGL_ModelViewerControl() {
            InitializeComponent();
            MaximumSize = MinimumSize = new System.Drawing.Size(320, 320);
            ForceLighting = null;

            RenderOptions = new RendererOptions() {
                DrawModels      = true,
                DrawExtraModels = true,
                ApplyLighting   = true,
                DrawWireframe   = true,
                SmoothLighting  = true,
            };
        }

        protected override void OnLoad(EventArgs e) {
            MakeCurrent();
            base.OnLoad(e);

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

            if (TextureContainer != null && _sglModels.Length != 0)
                Update(TextureContainer, _sglModels);

            _updateLightPalette = true;
            _lightPalette = new Palette(Enumerable.Range(0, 32)
                .Select(i => {
                    var level = Math.Pow(i / 31f, 4.0f);
                    return new PixelChannels() {
                        A = 255,
                        R = (byte) ((level * 1.00f  + 0.000f) * 255),
                        G = (byte) ((level * 0.75f +  0.125f) * 255),
                        B = (byte) ((level * 0.33f  + 0.333f) * 255)
                    };
                })
                .ToArray()
            );

            UpdateLighting();

            _general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, 0);
            _general.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            foreach (var shader in _general.Shaders) {
                shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }

            if (_globalTimer == null) {
                _globalTimer = new BetterTimer(60);
                _globalTimer.FrameTick += (s, delta) => GlobalYaw = (GlobalYaw + delta * 0.0225f) % 360f;
                _globalTimer.Start();
            }

            _timer = new BetterTimer(60);
            _timer.FrameTick += IncrementFrame;
            _timer.Start();

            Disposed += (s, e) => {
                _timer.FrameTick -= IncrementFrame;
                _timer?.Dispose();
                _timer = null;
            };
        }

        /// <summary>
        /// We have to dispose of resources here because by the time OnDisposeRendering() would
        /// be called via the Disposed event, the context is already gone.
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e) {
            OnDisposeRendering();
            base.OnHandleDestroyed(e);
        }

        void OnDisposeRendering() {
            if (Context != null) {
                MakeCurrent();

                _general?.Dispose();
                _screen?.Dispose();
                _models?.Dispose();
                _lighting?.Dispose();
            }

            _general  = null;
            _screen   = null;
            _models   = null;
            _lighting = null;
        }

        protected override void OnResize(EventArgs e) {
            MakeCurrent();
            base.OnResize(e);

            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Width);
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix =
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(22.50f), 1f, 0.05f, 65536.0f) *
                Matrix4.CreateTranslation(0, (float) (Height - Width) / Width, 0);
        }

        private void UpdateProjectionMatrices() {
            MakeCurrent();
            UpdateProjectionMatrix();
            if (_general?.Shaders?.Count > 0)
                foreach (var shader in _general.Shaders)
                    shader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref _projectionMatrix);
        }

        private void UpdateLighting() {
            MakeCurrent();

            if (_updateLightPalette)
                _lighting.Update(LightPalette, null);

            var lightPos = LightDirection.Normalized()
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw ?? GlobalYaw));

            foreach (var shader in _general.Shaders) {
                using (shader.Use())
                    shader.UpdateUniform(ShaderUniformType.LightPosition, ref lightPos);
            }
        }

        private void UpdateModelOffsets() {
            // Build transformation matrices for each model instance.
            var vertexMatrices = _sglModels.Select(x =>
                Matrix4.CreateScale(x.ScaleX, -x.ScaleY, -x.ScaleZ) *
                Matrix4.CreateRotationX( x.AngleX * (float) Math.PI / 180.0f) *
                Matrix4.CreateRotationY(-x.AngleY * (float) Math.PI / 180.0f) *
                Matrix4.CreateRotationZ(-x.AngleZ * (float) Math.PI / 180.0f) *
                Matrix4.CreateTranslation(x.PositionX, -x.PositionY, -x.PositionZ) *
                (x.Matrix?.ToOpenTKMarix() ?? Matrix4.Identity)
            ).ToArray();

            // Build updated vertices for all models.
            var transformedVertices = _sglModels.SelectMany((x, i) => x.GetModel(0).Vertices.Select(y => (y.ToVector4() * vertexMatrices[i]).Xyz)).ToArray();

            // Recalculate bounds.
            if (transformedVertices.Length > 0) {
                _minX = transformedVertices.Min(x => x.X);
                _minY = transformedVertices.Min(x => x.Y);
                _minZ = transformedVertices.Min(x => x.Z);

                _maxX = transformedVertices.Max(x => x.X);
                _maxY = transformedVertices.Max(x => x.Y);
                _maxZ = transformedVertices.Max(x => x.Z);
            }
            else {
                _minX = _minY = _minZ = -1.0f;
                _maxX = _maxY = _maxZ = 1.0f;
            }

            _width  = _maxX - _minX;
            _height = _maxY - _minY;
            _depth  = _maxZ - _minZ;

            _size   = Math.Max(0.1f, Math.Max(_width, Math.Max(_height, _depth)));
            _center = new Vector3((_minX + _maxX) / 2, (_minY + _maxY) / 2, (_minZ + _maxZ) / 2);

            _dist = (float) Math.Pow(_size, 0.875f) * 6.5f / Zoom;
        }

        private void UpdateCameraPosition() {
            var yawRadians = MathHelper.DegreesToRadians(Yaw ??GlobalYaw);

            Position = new Vector3(0.0f, 0.0f, 1.0f)
                * Matrix3.CreateRotationX(Pitch * (float) Math.PI / 180.0f)
                * Matrix3.CreateRotationY((Yaw ?? GlobalYaw) * (float) Math.PI / 180.0f)
                * _dist;

            Position += _center;
        }

        private void UpdateViewMatrix() {
            MakeCurrent();
            UpdateModelOffsets();
            UpdateCameraPosition();
            UpdateLighting();
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-(Yaw ?? GlobalYaw)))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        protected override void OnPaint(PaintEventArgs e) {
            MakeCurrent();

            UpdateViewMatrix();
            foreach (var shader in _general.Shaders)
                shader.UpdateUniform(ShaderUniformType.ViewMatrix, ref _viewMatrix);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _renderer.InvalidateModelMatrices();
            _renderer.DrawScene(
                new SGL_ModelRendererResources() {
                    General  = _general,
                    Screen   = _screen,
                    Models   = _models,
                    Lighting = _lighting,
                },
                RenderOptions,
                new RendererState() {
                    CameraYaw        = Yaw ?? GlobalYaw,
                    CameraPitch      = Pitch,
                    ScreenWidth      = ClientSize.Width,
                    ScreenHeight     = ClientSize.Height,
                    ProjectionMatrix = _projectionMatrix,
                    ViewMatrix       = _viewMatrix,
                }
            );

            SwapBuffers();
        }

        public ITextureMetaCollection TextureContainer { get; private set; } = null;
        private ISGL_ModelInstance[] _sglModels = [];

        public void Update(ITextureMetaCollection texContainer, ISGL_Model sglModel)
            => Update(texContainer, (sglModel == null) ? [] : [sglModel]);

        public void Update(ITextureMetaCollection texContainer, ISGL_Model[] sglModels) {
            Update(texContainer, sglModels
                .Select((x, i) => new SGL_ModelInstance((_, _) => x) {
                    ModelID = x.ModelID, ModelCollectionID = x.ModelCollectionID, ModelInstanceID = i
                })
                .ToArray()
            );
        }

        public void Update(ITextureMetaCollection texContainer, ISGL_ModelInstance sglModel)
            => Update(texContainer, (sglModel == null) ? [] : [sglModel]);

        public void Update(ITextureMetaCollection texContainer, ISGL_ModelInstance[] sglModels) {
            sglModels ??= [];

            if (Enumerable.SequenceEqual(_sglModels, sglModels))
                return;

            MakeCurrent();

            // Always use the first collection ID.
            var collectionId = sglModels.Length == 0 ? -1 : sglModels[0].ModelCollectionID;
            sglModels = sglModels.Where(x => x.ModelCollectionID == collectionId).ToArray();

            TextureContainer = texContainer;
            _sglModels       = sglModels;
            _size            = 1.0f;
            _center          = new Vector3();
            _dist            = 1.0f;

            if (_models != null) {
                _models.Reset();
                if (TextureContainer != null) {
                    var texturesById = texContainer.GetAnimatableTexturesByModelCollectionID(collectionId);

                    // TODO: this should also use instances instead of models!!!
                    var models = sglModels
                        .Select(
                            x => { return x.GetModel(0); }
                        ).ToArray();
                    _models.Update(
                        models, texturesById, (idx) => sglModels[idx],
                        forceSemiTransparentValues: sglModels.Select(x => x.ForceTransparency).ToArray(),
                        isHideMesh: false, forceLighting: ForceLighting, forceBlackIfTransparentNonIndexed: false
                    );
                }
                else {
                    // TODO: throw?? what to do here???
                }
            }

            Invalidate();
        }

        private float _updateTexMs = 0;

        private void IncrementFrame(object sender, float delta) {
            if (!Visible || IsDisposed)
                return;
            MakeCurrent();

            _updateTexMs += delta;
            if (_updateTexMs > 500)
                _updateTexMs = 500;

            while (_updateTexMs >= 33.33f) {
                _updateTexMs -= 33.33f;
                var collectionIds = _sglModels.Select(x => x.ModelCollectionID).Distinct().ToArray();
                foreach (var collectionId in collectionIds)
                    foreach (var modelGroup in _models.ModelGroupsByIDByCollection[collectionId].Values)
                        foreach (var model in modelGroup.Models)
                            _ = model.UpdateAnimatedTextures();
            }

            Invalidate();

            // Run any custom timers attached.
            FrameTick?.Invoke(this, delta);
        }

        public Vector3 Position { get; private set; }
        public static float GlobalYaw { get; set; }
        public RendererOptions RenderOptions { get; }

        public float? Yaw { get; set; } = null;
        public float Pitch { get; set; } = -30.0f;
        public bool? ForceLighting { get; set; }
        public float Zoom { get; set; } = 1.0f;
        public float PosHeight { get; set; } = 0.45f;
        public Vector3 LightDirection { get; set; } = new Vector3(-0.50f, 0.25f, 0.75f);

        private bool _updateLightPalette = false;
        private Palette _lightPalette;
        public Palette LightPalette {
            get => _lightPalette;
            set {
                if (_lightPalette != value) {
                    if (value == null)
                        throw new ArgumentNullException(nameof(LightPalette));
                    if (value.ColorCount != 0x20)
                        throw new ArgumentOutOfRangeException(nameof(LightPalette));
                    _lightPalette = value;
                    _updateLightPalette = true;
                }
            }
        }

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

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private GeneralResources   _general  = null;
        private ScreenResources    _screen   = null;
        private ModelResources     _models   = null;
        private LightingResources  _lighting = null;

        private SGL_ModelRenderer _renderer = null;
        private BetterTimer _timer = null;

        private static BetterTimer _globalTimer = null;

        public event FrameTickEventHandler FrameTick;
    }
}
