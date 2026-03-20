using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonLib;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Win.App;
using SF3.Win.OpenGL.MPD;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitRendering() {
            Load      += (s, e) => OnLoadRendering();
            Resize    += (s, e) => OnResizeRendering();
            Paint     += (s, e) => OnPaintRendering();
            FrameTick += (s, deltaInMs) => OnFrameTickRendering(deltaInMs);
            TileModified += (s, e) => OnTileModifiedRendering(s);
            GotFocus  += (s, e) => InvalidateFrame();
            LostFocus += (s, e) => InvalidateFrame();

            _appSettings.ViewerDrawSurfaceModelChanged   += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawModelsChanged         += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawExtraModelsChanged    += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawGroundChanged         += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawSkyChanged            += (s, e) => InvalidateFrame();
            _appSettings.ViewerRunAnimationsChanged      += (s, e) => InvalidateFrame();
            _appSettings.ViewerApplyLightingChanged      += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawGradientsChanged      += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawActorsChanged         += (s, e) => InvalidateFrame();

            _appSettings.ViewerDrawWireframeChanged      += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawBoundariesChanged     += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawBattleZonesChanged    += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawTerrainTypesChanged   += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawEventIDsChanged       += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawCollisionLinesChanged += (s, e) => InvalidateFrame();
            _appSettings.HideModelsNotFacingCameraChanged += (s, e) => InvalidateFrame();

            _appSettings.ViewerApplyShadowTagsChanged += (s, e) => {
                if (_models != null) {
                    _models.ApplyShadowTags = _appSettings.ViewerApplyShadowTags;
                    InvalidateModels();
                }
            };
            _appSettings.ViewerApplyHideTagsChanged += (s, e) => {
                if (_models != null) {
                    _models.ApplyHideTags = _appSettings.ViewerApplyHideTags;
                    InvalidateModels();
                }
            };

            _appSettings.RenderOnBlackBackgroundChanged  += (s, e) => InvalidateFrame();
            _appSettings.ViewerDrawNormalsChanged        += (s, e) => InvalidateFrame();
            _appSettings.ViewerRotateSpritesUpChanged    += (s, e) => { _renderer.InvalidateSpriteMatrices(_models); InvalidateFrame(); };

            var scene = AppResources.Get();
            scene.ActiveSceneChanged += (s, e) => { InvalidateActors(); };
            scene.ActiveCHRChanged   += (s, e) => { InvalidateActors(); };
        }

        /// <summary>
        /// We have to dispose of resources here because by the time OnDisposeRendering() would
        /// be called via the Disposed event, the context is already gone.
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e) {
            OnDisposeRendering();
            base.OnHandleDestroyed(e);
        }

        private void OnLoadRendering() {
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.Max);

            _general         = new GeneralResources();
            _models          = new ModelResources(_appSettings.ViewerApplyShadowTags, _appSettings.ViewerApplyHideTags);
            _surfaceModel    = new SurfaceModelResources();
            _groundModel     = new GroundModelResources();
            _skyModel        = new SkyModelResources();
            _collisionModels = new CollisionResources();
            _editor          = new EditorResources();
            _gradients       = new GradientResources();
            _lighting        = new LightingResources();
            _boundaryModels  = new BoundaryModelResources();
            _sceneResources  = new SceneResources();
            _screenResources = new ScreenResources();

            _renderer = new Renderer();

            _general.Init();
            _models.Init();
            _surfaceModel.Init();
            _groundModel.Init();
            _skyModel.Init();
            _collisionModels.Init();
            _editor.Init();
            _gradients.Init();
            _lighting.Init();
            _boundaryModels.Init();
            _sceneResources.Init();
            _screenResources.Init();

            SetInitialCameraPosition();
            _screenResources.Update(ClientSize.Width, ClientSize.Height);
            UpdateProjectionMatrices(ClientSize.Width, ClientSize.Height);

            using (_general.ObjectShader.Use()) {
                _general.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, 0);
                _general.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);
            }

            foreach (var shader in _general.Shaders) {
                using (shader.Use()) {
                    shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                    shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
                }
            }

            UpdateInvalidatedResources();
        }

        private void OnDisposeRendering() {
            MakeCurrent();

            _general?.Dispose();
            _models?.Dispose();
            _surfaceModel?.Dispose();
            _groundModel?.Dispose();
            _skyModel?.Dispose();
            _collisionModels?.Dispose();
            _editor?.Dispose();
            _gradients?.Dispose();
            _lighting?.Dispose();
            _boundaryModels?.Dispose();
            _sceneResources?.Dispose();
            _screenResources?.Dispose();

            _general           = null;
            _models            = null;
            _surfaceModel      = null;
            _groundModel       = null;
            _skyModel          = null;
            _collisionModels   = null;
            _editor            = null;
            _gradients         = null;
            _lighting          = null;
            _boundaryModels    = null;
            _sceneResources    = null;
            _screenResources   = null;
        }

        private void OnResizeRendering() => UpdateViewport(ClientSize.Width, ClientSize.Height);

        private int _lastViewportWidth  = 0;
        private int _lastViewportHeight = 0;

        public void UpdateViewport(int width, int height) {
            // Prevent some bogus dimensions.
            width  = Math.Max(100, width);
            height = Math.Max(100, height);

            if (_lastViewportWidth == width && _lastViewportHeight == height)
                return;

            _lastViewportWidth  = width;
            _lastViewportHeight = height;

            MakeCurrent();

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, width, height);

            _screenResources?.Update(width, height);
            UpdateProjectionMatrices(width, height);

            InvalidateFrame();
        }

        private void OnPaintRendering() {
            using (new ScopeGuard(() => _inPaintCounter++, () => _inPaintCounter--)) {
                // Too many paint events can cause the frame ticks to get dropped.
                // Let's force them to happen if we exceed 1/60th of a second.
                var frameTime = GetNow() - _lastTimeInMs;
                if (frameTime > 16)
                    IncrementFrame();

                RenderFrame();
            }
        }

        public void RenderFrame() {
            MakeCurrent();

            // Update models, textures, model switch groups, etc. that have been modified since the last frame.
            UpdateInvalidatedResources();

            var resources = new Renderer.RendererResources() {
                General         = _general,
                Models          = _models,
                SurfaceModel    = _surfaceModel,
                GroundModel     = _groundModel,
                SkyModel        = _skyModel,
                Gradients       = _gradients,
                Lighting        = _lighting,
                BoundaryModels  = _boundaryModels,
                CollisionModels = _collisionModels,
                Scene           = _sceneResources,
                Editor          = _editor,
                Screen          = _screenResources,
            };

            // TODO: these options should be cached!!!
            var truncatedPaletteAdjustments = MPD_File?.BinaryReproductionFlags?.PaletteAdjustmentIsTruncated == true;
            var options = new Renderer.RendererOptions() {
                DrawModels         = DrawModels,
                DrawExtraModels    = DrawExtraModels,
                DrawSurfaceModel   = DrawSurfaceModel,
                DrawGround         = DrawGround,
                DrawSky            = MPD_File?.Flags?.Bit_0x0800_HasCutsceneSky == true && DrawSky,
                DrawGradients      = DrawGradients,
                DrawActors         = DrawActors,
                ApplyLighting      = ApplyLighting,

                HideModelsNotFacingCamera = HideModelsNotFacingCamera,
                ModelsYRotation    = MPD_File?.Settings?.ModelsYRotation ?? 180.0f,
                ModelsViewAngleMin = MPD_File?.Settings?.ModelsViewAngleMin ?? 0,
                ModelsViewAngleMax = MPD_File?.Settings?.ModelsViewAngleMax ?? 0,

                DrawNormals        = DrawNormals,
                DrawWireframe      = DrawWireframe,
                RotateSpritesUp    = RotateSpritesUp,
                DrawOutlines       = true,

                DrawTerrainTypes   = DrawTerrainTypes,
                DrawEventIDs       = DrawEventIDs,
                DrawBoundaries     = DrawBoundaries,
                DrawBattleZones    = DrawBattleZones,
                DrawCollisionLines = DrawCollisionLines,

                BackgroundX        = MPD_File?.Planes?.BackgroundX ?? 0,
                BackgroundY        = MPD_File?.Planes?.BackgroundY ?? 0,

                GroundAdj          = truncatedPaletteAdjustments ? null : MPD_File?.Settings?.GroundPaletteAdjustment,

                UseOutsideLighting = MPD_File?.Flags?.Bit_0x2000_NarrowAngleBasedLightmap == true,

                ModelsToHide       = _modelInstancesToHide,
            };

            // Make sure every shader has the latest view matrix.
            // TODO: Perhaps an update isn't necessary if nothing changed?
            UpdateViewMatrix();
            foreach (var shader in _general.Shaders)
                shader.UpdateUniform(ShaderUniformType.ViewMatrix, ref _viewMatrix);

            var state = new Renderer.RendererState() {
                CameraYaw        = Yaw,
                CameraPitch      = Pitch,
                ScreenWidth      = ClientSize.Width,
                ScreenHeight     = ClientSize.Height,
                ProjectionMatrix = _projectionMatrix,
                ViewMatrix       = _viewMatrix
            };

            // Render the invisible scene used for mouse selection
            using (_screenResources.SelectFramebuffer.UseDraw())
                _renderer.DrawSelectionScene(resources, options, state);

            // Determine what's under the mouse. This will be fed into the final scene render.
            // This may have invalidated some resources, so update them.
            UpdateMouseoverObject();
            UpdateEditorResources();

            // Render the final scene.
            PerformClear();
            _renderer.DrawScene(resources, options, state);

            // If we have input focus, draw a box to indicate it.
            if (Focused)
                _renderer.DrawControlFocusedBox(resources, options, state);

            SwapBuffers();
        }

        public void Clear() {
            MakeCurrent();
            PerformClear();
            SwapBuffers();
        }

        private void PerformClear() {
            if (RenderOnBlackBackground)
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            else
                GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.StencilMask(0xFF);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }

        private void UpdateInvalidatedResources() {
            UpdateEditorResources();

            if (_actorsNeedUpdate) {
                _sceneResources.Update(MPD_File);
                _actorsNeedUpdate = false;
            }

            if (_lightingTextureNeedsUpdate) {
                _lighting.Update(MPD_File);
                _lightingTextureNeedsUpdate = false;
            }

            if (_lightPositionNeedsUpdate) {
                var lightPos = GetLightPosition();
                foreach (var shader in _general.Shaders) {
                    using (shader.Use())
                        shader.UpdateUniform(ShaderUniformType.LightPosition, ref lightPos);
                }
                _lightPositionNeedsUpdate = false;
            }

            if (_modelsNeedUpdate) {
                _renderer.InvalidateModelMatrices();
                _models?.Update(MPD_File);
                _gradients?.Update(MPD_File);
                _boundaryModels?.Update(MPD_File);
                _collisionModels?.Update(MPD_File.Collisions, MPD_File.Surface, MPD_File.Planes.GroundY);

                _modelsNeedUpdate = false;
            }

            if (_surfaceModelNeedsUpdate) {
                _surfaceModel?.Update(MPD_File);
                _surfaceModelNeedsUpdate = false;
            }

            foreach (var block in _surfaceModel.Blocks)
                if (block.NeedsUpdate)
                    block.Update(MPD_File);

            if (_planesNeedUpdate) {
                _groundModel?.Update(MPD_File);
                _skyModel?.Update(MPD_File);
                _planesNeedUpdate = false;
            }

            if (_modelInstancesToHideNeedsUpdate) {
                UpdateModelInstancesToHide();
                _modelInstancesToHideNeedsUpdate = false;
            }
        }

        private void UpdateEditorResources() {
            if (_editorNeedsUpdate) {
                _editor.UpdateMouseoverObject(MPD_File, _general, _mouseoverObject);
                _editor.UpdateSelectedObjects(MPD_File, _general, _selectedObjects);
                _editorNeedsUpdate = false;
            }
        }

        private void OnFrameTickRendering(float deltaInMs) {
            if (RunAnimations)
                UpdateAnimatedTextures(deltaInMs);
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (IMPD_SurfaceTile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[BlockHelpers.GetTileBlockLocation(tile.X, tile.Y).Num].Invalidate();
                _editorNeedsUpdate = true;
                InvalidateFrame();
            }
        }

        private void UpdateProjectionMatrix(int width, int height) {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) width / height,
                0.05f, 65536.0f) * Matrix4.CreateTranslation((float) (ProjectionXAdjustment * 2) / (float) ClientSize.Width, 0, 0);
        }

        public void UpdateProjectionMatrices(int width, int height) {
            UpdateProjectionMatrix(width, height);
            if (_general?.Shaders?.Count > 0)
                foreach (var shader in _general.Shaders)
                    shader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref _projectionMatrix);
        }

        private void UpdateViewMatrix() {
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        private void UpdateModelInstancesToHide() {
            // Determine which models to hide based on flags.
            // TODO: This isn't how it actually works; it's very non-deterministic.
            //   Models appear to be hidden by default if they're in any "VisibleModelsWhenFlagOn" table.
            //   Their visibility is toggled on/off when the flag is toggled.
            //   If a model is present in multiple switches (as is the case in IWAOKA.MPD) then the visibility
            //      of the model depends on the order in which the flags were toggled.
            var modelsToHide = new HashSet<int>();
            if (MPD_File?.ModelSwitchGroups != null) {
                // Assume everything is hidden by default.
                foreach (var switchGroup in MPD_File.ModelSwitchGroups) {
                    if (switchGroup.ModelInstancesVisibleWhenOn != null)
                        foreach (var modelInstId in switchGroup.ModelInstancesVisibleWhenOn)
                            modelsToHide.Add(modelInstId);

                    if (switchGroup.ModelInstancesVisibleWhenOff != null)
                        foreach (var modelInstId in switchGroup.ModelInstancesVisibleWhenOff)
                            modelsToHide.Add(modelInstId);
                }

                // Enable models selectively based on flags.
                foreach (var switchGroup in MPD_File.ModelSwitchGroups) {
                    var turnOnList = switchGroup.StateInEditor ? switchGroup.ModelInstancesVisibleWhenOn : switchGroup.ModelInstancesVisibleWhenOff;
                    if (turnOnList != null)
                        foreach (var modelInstId in turnOnList)
                            modelsToHide.Remove(modelInstId);
                }
            }

            _modelInstancesToHide = modelsToHide;
        }

        private Vector3 GetLightPosition() {
            if (MPD_File == null)
                return new Vector3(0, -1, 0);

            var lighting = MPD_File.Lighting;

            var pitchInRadians = lighting.Pitch / 180.0f * Math.PI;
            var pitchSin = -Math.Sin(pitchInRadians);
            var pitchCos = Math.Cos(pitchInRadians);

            var yawInRadians = lighting.Yaw / 180.0f * Math.PI;
            var x = -Math.Sin(yawInRadians) * pitchCos;
            var y = pitchSin;
            var z = Math.Cos(yawInRadians) * pitchCos;

            return new Vector3((float) x, (float) y, (float) z);
        }

        private float _frameDeltaTimeInMs = 0;

        private void UpdateAnimatedTextures(float deltaInMs) {
            const float c_frameDurationInMs = (1000.0f / 30.0f);

            _frameDeltaTimeInMs += deltaInMs;
            if (_frameDeltaTimeInMs >= c_frameDurationInMs) {
                while (_frameDeltaTimeInMs >= c_frameDurationInMs)
                    _frameDeltaTimeInMs -= c_frameDurationInMs;

                if (_surfaceModel?.Blocks != null)
                    foreach (var block in _surfaceModel.Blocks)
                        if (block.Model?.UpdateAnimatedTextures() == true)
                            InvalidateFrame();

                if (_models?.ModelsByIDByCollection != null)
                    foreach (var mc in _models.ModelsByIDByCollection.Values)
                        foreach (var modelGroup in mc.Values)
                            foreach (var model in modelGroup.Models)
                                if (model.UpdateAnimatedTextures() == true)
                                    InvalidateFrame();
            }
        }

        private bool UpdateAppSetting(string propertyName, bool value) {
            var property = AppSettings.GetType().GetProperty(propertyName);
            if (property == null || (bool) property.GetValue(AppSettings, null) == value)
                return false;

            property.SetValue(AppSettings, value);
            return true;
        }

        private void InvalidateResource(ref bool flag, bool invalidatePainter = true) {
            flag = true;
            if (invalidatePainter)
                InvalidateFrame();
        }

        public void InvalidateFrame() {
            if (_inPaintCounter == 0)
                Invalidate();
        }

        public void InvalidateEditor         (bool invalidatePainter = true) => InvalidateResource(ref _editorNeedsUpdate,          invalidatePainter);
        public void InvalidateActors         (bool invalidatePainter = true) => InvalidateResource(ref _actorsNeedUpdate,           invalidatePainter);
        public void InvalidateLightingTexture(bool invalidatePainter = true) => InvalidateResource(ref _lightingTextureNeedsUpdate, invalidatePainter);
        public void InvalidateLightPosition  (bool invalidatePainter = true) => InvalidateResource(ref _lightPositionNeedsUpdate,   invalidatePainter);
        public void InvalidateModels         (bool invalidatePainter = true) => InvalidateResource(ref _modelsNeedUpdate,           invalidatePainter);
        public void InvalidateSurfaceModel   (bool invalidatePainter = true) => InvalidateResource(ref _surfaceModelNeedsUpdate,    invalidatePainter);
        public void InvalidatePlanes         (bool invalidatePainter = true) => InvalidateResource(ref _planesNeedUpdate,           invalidatePainter);
        public void InvalidateModelInstancesToHide(bool invalidatePainter = true) => InvalidateResource(ref _modelInstancesToHideNeedsUpdate, invalidatePainter);

        public void InvalidateAllResources(bool invalidatePainter = true) {
            InvalidateEditor(false);
            InvalidateActors(false);
            InvalidateLightingTexture(false);
            InvalidateLightPosition(false);
            InvalidateModels(false);
            InvalidateSurfaceModel(false);
            InvalidatePlanes(false);
            InvalidateModelInstancesToHide(false);

            if (invalidatePainter)
                InvalidateFrame();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawSurfaceModel {
            get => AppSettings.ViewerDrawSurfaceModel;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawSurfaceModel), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawModels {
            get => AppSettings.ViewerDrawModels;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawModels), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawExtraModels {
            get => AppSettings.ViewerDrawExtraModels;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawExtraModels), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGround {
            get => AppSettings.ViewerDrawGround;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawGround), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawSky {
            get => AppSettings.ViewerDrawSky;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawSky), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunAnimations {
            get => AppSettings.ViewerRunAnimations;
            set => UpdateAppSetting(nameof(AppSettings.ViewerRunAnimations), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyLighting {
            get => AppSettings.ViewerApplyLighting;
            set => UpdateAppSetting(nameof(AppSettings.ViewerApplyLighting), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGradients {
            get => AppSettings.ViewerDrawGradients;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawGradients), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawActors {
            get => AppSettings.ViewerDrawActors;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawActors), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawWireframe {
            get => AppSettings.ViewerDrawWireframe;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawWireframe), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawBoundaries {
            get => AppSettings.ViewerDrawBoundaries;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawBoundaries), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawBattleZones {
            get => AppSettings.ViewerDrawBattleZones;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawBattleZones), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawCollisionLines {
            get => AppSettings.ViewerDrawCollisionLines;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawCollisionLines), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawTerrainTypes {
            get => AppSettings.ViewerDrawTerrainTypes;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawTerrainTypes), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawEventIDs {
            get => AppSettings.ViewerDrawEventIDs;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawEventIDs), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HideModelsNotFacingCamera {
            get => AppSettings.HideModelsNotFacingCamera;
            set => UpdateAppSetting(nameof(AppSettings.HideModelsNotFacingCamera), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyShadowTags {
            get => AppSettings.ViewerApplyShadowTags;
            set => UpdateAppSetting(nameof(AppSettings.ViewerApplyShadowTags), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyHideTags {
            get => AppSettings.ViewerApplyHideTags;
            set => UpdateAppSetting(nameof(AppSettings.ViewerApplyHideTags), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderOnBlackBackground {
            get => AppSettings.RenderOnBlackBackground;
            set => UpdateAppSetting(nameof(AppSettings.RenderOnBlackBackground), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawNormals {
            get => AppSettings.ViewerDrawNormals;
            set => UpdateAppSetting(nameof(AppSettings.ViewerDrawNormals), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RotateSpritesUp {
            get => AppSettings.ViewerRotateSpritesUp;
            set => UpdateAppSetting(nameof(AppSettings.ViewerRotateSpritesUp), value);
        }

        public int ProjectionXAdjustment { get; set; }

        private bool _editorNeedsUpdate          = false;
        private bool _actorsNeedUpdate           = true;
        private bool _lightingTextureNeedsUpdate = true;
        private bool _lightPositionNeedsUpdate   = true;
        private bool _modelsNeedUpdate           = true;
        private bool _surfaceModelNeedsUpdate    = true;
        private bool _planesNeedUpdate           = true;
        private bool _modelInstancesToHideNeedsUpdate = true;

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private GeneralResources       _general         = null;
        private ModelResources         _models          = null;
        private SurfaceModelResources  _surfaceModel    = null;
        private GroundModelResources   _groundModel     = null;
        private SkyModelResources      _skyModel        = null;
        private CollisionResources     _collisionModels = null;
        private EditorResources        _editor          = null;
        private GradientResources      _gradients       = null;
        private LightingResources      _lighting        = null;
        private BoundaryModelResources _boundaryModels  = null;
        private SceneResources         _sceneResources  = null;
        private ScreenResources        _screenResources = null;
        private HashSet<int>           _modelInstancesToHide = null;

        private Renderer _renderer = null;
        private int _inPaintCounter = 0;
    }
}
