using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonLib;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.App;
using SF3.Win.OpenGL;
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

            _appState.ViewerDrawSurfaceModelChanged   += (s, e) => InvalidateFrame();
            _appState.ViewerDrawModelsChanged         += (s, e) => InvalidateFrame();
            _appState.ViewerDrawGroundChanged         += (s, e) => InvalidateFrame();
            _appState.ViewerDrawSkyChanged            += (s, e) => InvalidateFrame();
            _appState.ViewerRunAnimationsChanged      += (s, e) => InvalidateFrame();
            _appState.ViewerApplyLightingChanged      += (s, e) => InvalidateFrame();
            _appState.ViewerDrawGradientsChanged      += (s, e) => InvalidateFrame();
            _appState.ViewerDrawActorsChanged         += (s, e) => InvalidateFrame();

            _appState.ViewerDrawWireframeChanged      += (s, e) => InvalidateFrame();
            _appState.ViewerDrawBoundariesChanged     += (s, e) => InvalidateFrame();
            _appState.ViewerDrawTerrainTypesChanged   += (s, e) => InvalidateFrame();
            _appState.ViewerDrawEventIDsChanged       += (s, e) => InvalidateFrame();
            _appState.ViewerDrawCollisionLinesChanged += (s, e) => InvalidateFrame();
            _appState.HideModelsNotFacingCameraChanged += (s, e) => InvalidateFrame();

            _appState.ViewerApplyShadowTagsChanged += (s, e) => {
                if (_models != null) {
                    _models.ApplyShadowTags = _appState.ViewerApplyShadowTags;
                    InvalidateModels();
                }
            };
            _appState.ViewerApplyHideTagsChanged += (s, e) => {
                if (_models != null) {
                    _models.ApplyHideTags = _appState.ViewerApplyHideTags;
                    InvalidateModels();
                }
            };

            _appState.RenderOnBlackBackgroundChanged  += (s, e) => InvalidateFrame();
            _appState.ViewerDrawNormalsChanged        += (s, e) => InvalidateFrame();
            _appState.ViewerRotateSpritesUpChanged    += (s, e) => { _renderer.InvalidateSpriteMatrices(_models); InvalidateFrame(); };

            var scene = AppScene.Get();
            scene.ActiveActorCollectionChanged += (s, e) => { InvalidateActors(); };
            scene.ActiveCHRChanged             += (s, e) => { InvalidateActors(); };
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
            _models          = new ModelResources(_appState.ViewerApplyShadowTags, _appState.ViewerApplyHideTags);
            _surfaceModel    = new SurfaceModelResources();
            _groundModel     = new GroundModelResources();
            _skyModel        = new SkyModelResources();
            _collisionModels = new CollisionResources();
            _editor          = new EditorResources();
            _gradients       = new GradientResources();
            _lighting        = new LightingResources();
            _boundaryModels  = new BoundaryModelResources();
            _actorResources  = new ActorResources();

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
            _actorResources.Init();

            SetInitialCameraPosition();
            UpdateFramebuffers(ClientSize.Width, ClientSize.Height);
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
            _actorResources?.Dispose();

            _selectFramebuffer?.Dispose();
            _outlineFramebuffer1?.Dispose();
            _outlineFramebuffer2?.Dispose();

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
            _actorResources    = null;

            _selectFramebuffer   = null;
            _outlineFramebuffer1 = null;
            _outlineFramebuffer2 = null;
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

            UpdateFramebuffers(width, height);
            UpdateProjectionMatrices(width, height);

            InvalidateFrame();
        }

        private void OnPaintRendering() {
            using (new ScopeGuard(() => _inPaintCounter++, () => _inPaintCounter--))
                RenderFrame();
        }

        public void RenderFrame() {
            if (_inPaintCounter != 1)
                ;

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
                Actors          = _actorResources,
                Editor          = _editor,

                OutlineFramebuffer1 = _outlineFramebuffer1,
                OutlineFramebuffer2 = _outlineFramebuffer2,

            };

            // TODO: these options should be cached!!!
            var truncatedPaletteAdjustments = MPD_File?.BinaryReproductionFlags?.PaletteAdjustmentIsTruncated == true;
            var options = new Renderer.RendererOptions() {
                DrawModels         = DrawModels,
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
                DrawCollisionLines = DrawCollisionLines,

                BackgroundX        = MPD_File?.Planes?.BackgroundX ?? 0,
                BackgroundY        = MPD_File?.Planes?.BackgroundY ?? 0,

                GroundAdj          = truncatedPaletteAdjustments ? null : MPD_File?.Settings?.GroundPaletteAdjustment,

                UseOutsideLighting = MPD_File?.Flags?.Bit_0x2000_NarrowAngleBasedLightmap == true,

                ModelsToHide       = _modelInstancesToHide,
            };

            var state = new Renderer.RendererState() {
                CameraYaw        = Yaw,
                CameraPitch      = Pitch,
                ScreenWidth      = ClientSize.Width,
                ScreenHeight     = ClientSize.Height,
                ProjectionMatrix = _projectionMatrix,
                ViewMatrix       = _viewMatrix
            };

            // Make sure every shader has the latest view matrix.
            // TODO: Perhaps an update isn't necessary if nothing changed?
            UpdateViewMatrix();
            foreach (var shader in _general.Shaders)
                shader.UpdateUniform(ShaderUniformType.ViewMatrix, ref _viewMatrix);

            // Render the invisible scene used for mouse selection
            using (_selectFramebuffer.UseDraw())
                _renderer.DrawSelectionScene(resources, options, state);

            // Determine what's under the mouse. This will be fed into the final scene render.
            // This may have invalidated some resources, so update them.
            UpdateTilePosition();
            UpdateEditorResources();

            // Render the final scene.
            PerformClear();
            _renderer.DrawScene(resources, options, state);

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
                _actorResources.Update(MPD_File);
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
                _editor.UpdateTileHoverModel(MPD_File, _general, _tileHoverPos);
                _editor.UpdateTileSelectedModel(MPD_File, _general, _tileSelectedPos);
                _editorNeedsUpdate = false;
            }
        }

        private void OnFrameTickRendering(float deltaInMs) {
            if (RunAnimations)
                UpdateAnimatedTextures(deltaInMs);
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (SurfaceTile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[tile.BlockLocation.Num].Invalidate();
                _editorNeedsUpdate = true;
                InvalidateFrame();
            }
        }

        private void UpdateFramebuffers(int width, int height) {
            const int c_outlinePixelCount = 400 * 400;

            _selectFramebuffer?.Dispose();
            _selectFramebuffer = new Framebuffer(width, height, 3, RenderbufferStorage.DepthComponent);

            // Create two framebuffers for outlines to account for the 2 blur passes.
            // Make the size of the framebuffer no larger than 160,000 pixels (400x400), with the same width:height ratio as the viewport.
            // This is so make the outlines appear bigger without the need for more complicated gaussian blur passes.
            int outlineWidth, outlineHeight;
            if (width * height > c_outlinePixelCount) {
                var ratio = (float) width / height;
                outlineWidth  = (int) Math.Sqrt(c_outlinePixelCount * ratio);
                outlineHeight = c_outlinePixelCount / outlineWidth;
            }
            else {
                outlineWidth = width;
                outlineHeight = height;
            }

            _outlineFramebuffer1?.Dispose();
            _outlineFramebuffer2?.Dispose();
            _outlineFramebuffer1 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
            _outlineFramebuffer2 = new Framebuffer(outlineWidth, outlineHeight, 4, null, false, false);
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

        private bool UpdateAppState(string propertyName, bool value) {
            var property = AppState.GetType().GetProperty(propertyName);
            if (property == null || (bool) property.GetValue(AppState, null) == value)
                return false;

            property.SetValue(AppState, value);
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
            get => AppState.ViewerDrawSurfaceModel;
            set => UpdateAppState(nameof(AppState.ViewerDrawSurfaceModel), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawModels {
            get => AppState.ViewerDrawModels;
            set => UpdateAppState(nameof(AppState.ViewerDrawModels), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGround {
            get => AppState.ViewerDrawGround;
            set => UpdateAppState(nameof(AppState.ViewerDrawGround), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawSky {
            get => AppState.ViewerDrawSky;
            set => UpdateAppState(nameof(AppState.ViewerDrawSky), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RunAnimations {
            get => AppState.ViewerRunAnimations;
            set => UpdateAppState(nameof(AppState.ViewerRunAnimations), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyLighting {
            get => AppState.ViewerApplyLighting;
            set => UpdateAppState(nameof(AppState.ViewerApplyLighting), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawGradients {
            get => AppState.ViewerDrawGradients;
            set => UpdateAppState(nameof(AppState.ViewerDrawGradients), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawActors {
            get => AppState.ViewerDrawActors;
            set => UpdateAppState(nameof(AppState.ViewerDrawActors), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawWireframe {
            get => AppState.ViewerDrawWireframe;
            set => UpdateAppState(nameof(AppState.ViewerDrawWireframe), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawBoundaries {
            get => AppState.ViewerDrawBoundaries;
            set => UpdateAppState(nameof(AppState.ViewerDrawBoundaries), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawCollisionLines {
            get => AppState.ViewerDrawCollisionLines;
            set => UpdateAppState(nameof(AppState.ViewerDrawCollisionLines), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawTerrainTypes {
            get => AppState.ViewerDrawTerrainTypes;
            set => UpdateAppState(nameof(AppState.ViewerDrawTerrainTypes), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawEventIDs {
            get => AppState.ViewerDrawEventIDs;
            set => UpdateAppState(nameof(AppState.ViewerDrawEventIDs), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HideModelsNotFacingCamera {
            get => AppState.HideModelsNotFacingCamera;
            set => UpdateAppState(nameof(AppState.HideModelsNotFacingCamera), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyShadowTags {
            get => AppState.ViewerApplyShadowTags;
            set => UpdateAppState(nameof(AppState.ViewerApplyShadowTags), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyHideTags {
            get => AppState.ViewerApplyHideTags;
            set => UpdateAppState(nameof(AppState.ViewerApplyHideTags), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderOnBlackBackground {
            get => AppState.RenderOnBlackBackground;
            set => UpdateAppState(nameof(AppState.RenderOnBlackBackground), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawNormals {
            get => AppState.ViewerDrawNormals;
            set => UpdateAppState(nameof(AppState.ViewerDrawNormals), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RotateSpritesUp {
            get => AppState.ViewerRotateSpritesUp;
            set => UpdateAppState(nameof(AppState.ViewerRotateSpritesUp), value);
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
        private ActorResources         _actorResources  = null;
        private HashSet<int>           _modelInstancesToHide = null;

        private Renderer _renderer = null;
        private int _inPaintCounter = 0;

        private Framebuffer _selectFramebuffer;
        private Framebuffer _outlineFramebuffer1;
        private Framebuffer _outlineFramebuffer2;
    }
}
