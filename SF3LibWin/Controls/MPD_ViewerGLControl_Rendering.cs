using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;
using SF3.Win.OpenGL;
using SF3.Win.OpenGL.MPD_File;
using SF3.Win.Types;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private void InitRendering() {
            Load      += (s, e) => OnLoadRendering();
            Disposed  += (s, e) => OnDisposeRendering();
            Resize    += (s, e) => OnResizeRendering();
            Paint     += (s, e) => OnPaintRendering();
            FrameTick += (s, deltaInMs) => OnFrameTickRendering(deltaInMs);
            TileModified += (s, e) => OnTileModifiedRendering(s);
        }

        public void UpdateLightingTexture() {
            MakeCurrent();
            using (var textureBitmap = CreateLightPaletteBitmap())
                _surfaceModel.SetLightingTexture(textureBitmap != null ? new Texture(textureBitmap, clampToEdge: false) : null);
        }

        public void UpdateLightPosition() {
            MakeCurrent();
            var lightPos = GetLightPosition();
            foreach (var shader in _world.Shaders) {
                using (shader.Use())
                    shader.UpdateUniform(ShaderUniformType.LightPosition, ref lightPos);
            }
        }

        private Bitmap CreateLightPaletteBitmap() {
            var lightPal = MPD_File?.LightPalette;
            if (lightPal == null)
                return null;

            var lightAdjustment = MPD_File.LightAdjustment;
            var adjR = lightAdjustment?.RAdjustment ?? 0;
            var adjG = lightAdjustment?.GAdjustment ?? 0;
            var adjB = lightAdjustment?.BAdjustment ?? 0;

            var numColors = lightPal.Length;

            var colorData = new byte[numColors * 4];
            int pos = 0;
            foreach (var color in lightPal) {
                var colorValue = color.ColorABGR1555;
                var colorR = MathHelpers.Clamp((short) ((colorValue >>  0) & 0x1F) + adjR, 0x00, 0x1F);
                var colorG = MathHelpers.Clamp((short) ((colorValue >>  5) & 0x1F) + adjG, 0x00, 0x1F);
                var colorB = MathHelpers.Clamp((short) ((colorValue >> 10) & 0x1F) + adjB, 0x00, 0x1F);

                colorData[pos++] = (byte) (colorB * 255 / 31);
                colorData[pos++] = (byte) (colorG * 255 / 31);
                colorData[pos++] = (byte) (colorR * 255 / 31);
                colorData[pos++] = 255;
            }

            var textureBitmap = new Bitmap(1, numColors, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bitmapData = textureBitmap.LockBits(new Rectangle(0, 0, 1, numColors), ImageLockMode.WriteOnly, textureBitmap.PixelFormat);
            Marshal.Copy(colorData, 0, bitmapData.Scan0, colorData.Length);
            textureBitmap.UnlockBits(bitmapData);

            return textureBitmap;
        }

        private void OnLoadRendering() {
            MakeCurrent();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.Max);

            _world         = new WorldResources();
            _models        = new ModelResources();
            _surfaceModel  = new SurfaceModelResources();
            _groundModel   = new GroundModelResources();
            _skyBoxModel   = new SkyBoxModelResources();
            _surfaceEditor = new SurfaceEditorResources();
            _gradients     = new GradientResources();

            _renderer = new Renderer();

            _world.Init();
            _models.Init();
            _surfaceModel.Init();
            _groundModel.Init();
            _skyBoxModel.Init();
            _surfaceEditor.Init();
            _gradients.Init();

            SetInitialCameraPosition();
            UpdateSelectFramebuffer();

            UpdateLightingTexture();
            UpdateLightPosition();
            _world.ObjectShader.UpdateUniform(ShaderUniformType.LightingMode, false);
            _world.ObjectShader.UpdateUniform(ShaderUniformType.GlobalGlow, Vector3.Zero);

            foreach (var shader in _world.Shaders) {
                shader.UpdateUniform(ShaderUniformType.ModelMatrix, Matrix4.Identity);
                shader.UpdateUniform(ShaderUniformType.NormalMatrix, Matrix3.Identity);
            }
        }

        private void OnDisposeRendering() {
            _world?.Dispose();
            _models?.Dispose();
            _surfaceModel?.Dispose();
            _groundModel?.Dispose();
            _skyBoxModel?.Dispose();
            _surfaceEditor?.Dispose();
            _gradients?.Dispose();

            _selectFramebuffer?.Dispose();

            _world             = null;
            _models            = null;
            _surfaceModel      = null;
            _groundModel       = null;
            _skyBoxModel       = null;
            _surfaceEditor     = null;
            _gradients         = null;

            _selectFramebuffer = null;
        }

        private void OnResizeRendering() {
            MakeCurrent();

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

            UpdateSelectFramebuffer();
            UpdateProjectionMatrices();

            Invalidate();
        }

        private void OnPaintRendering() {
            MakeCurrent();

            foreach (var block in _surfaceModel.Blocks)
                if (block.NeedsUpdate)
                    block.Update(MPD_File);

            if (_tileSelectedNeedsUpdate) {
                _surfaceEditor.UpdateTileSelectedModel(MPD_File, _world, _tileSelectedPos);
                _tileSelectedNeedsUpdate = false;
            }

            UpdateViewMatrix();
            foreach (var shader in _world.Shaders)
                shader.UpdateUniform(ShaderUniformType.ViewMatrix, ref _viewMatrix);

            using (_selectFramebuffer.Use()) {
                GL.ClearColor(1, 1, 1, 1);
                DrawSelectionScene();
            }
            UpdateTilePosition();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _renderer.DrawScene(
                _world, _models, _surfaceModel, _groundModel, _skyBoxModel,
                _gradients, MPD_File?.LightAdjustment, _surfaceEditor,
                new Renderer.RendererOptions() {
                    DrawModels = DrawModels,
                    DrawSurfaceModel = DrawSurfaceModel,
                    DrawGround = DrawGround,
                    DrawSkyBox = MPD_File?.Scenario >= ScenarioType.Scenario2 && DrawSkyBox,
                    DrawGradients = DrawGradients,
                    ApplyLighting = ApplyLighting,

                    DrawNormals = DrawNormals,
                    DrawWireframe = DrawWireframe,
                    RotateSpritesUp = RotateSpritesUp,

                    DrawTerrainTypes = DrawTerrainTypes,
                    DrawEventIDs = DrawEventIDs,
                    DrawBoundaries = DrawBoundaries,

                    DrawHelp = DrawHelp,

                    UseOutsideLighting = MPD_File?.MPDHeader?.OutdoorLighting == true
                },
                Yaw, Pitch, Width, Height,
                ref _projectionMatrix, ref _viewMatrix
            );

            SwapBuffers();
        }

        private void OnFrameTickRendering(float deltaInMs) {
            if (RunAnimations)
                UpdateAnimatedTextures(deltaInMs);
        }

        private void OnTileModifiedRendering(object sender) {
            var tile = (Tile) sender;
            if (_surfaceModel != null) {
                _surfaceModel.Blocks[tile.BlockLocation.Num].Invalidate();
                _tileSelectedNeedsUpdate = true;
                Invalidate();
            }
        }

        public void UpdateModels() {
            _renderer.InvalidateModelMatrices();

            MakeCurrent();
            _models?.Update(MPD_File);
            _surfaceModel?.Update(MPD_File);
            _groundModel?.Update(MPD_File);
            _skyBoxModel?.Update(MPD_File);
            _gradients?.Update(MPD_File);
            Invalidate();
        }

        private void UpdateSelectFramebuffer() {
            _selectFramebuffer?.Dispose();
            _selectFramebuffer = new Framebuffer(Width, Height);
        }

        private void UpdateProjectionMatrix() {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(22.50f), (float) ClientSize.Width / ClientSize.Height,
                0.05f, 65536.0f);
        }

        private void UpdateProjectionMatrices() {
            if ((_world?.Shaders?.Count ?? 0) == 0)
                return;

            UpdateProjectionMatrix();
            foreach (var shader in _world.Shaders)
                shader.UpdateUniform(ShaderUniformType.ProjectionMatrix, ref _projectionMatrix);
        }

        private void UpdateViewMatrix() {
            _viewMatrix = Matrix4.CreateTranslation(-Position)
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch));
        }

        private Vector3 GetLightPosition() {
            if (MPD_File == null)
                return new Vector3(0, -1, 0);

            var lightPos = MPD_File.LightPosition;
            var pitch = lightPos.Pitch;

            var pitchInRadians = pitch / 32768f * Math.PI;
            var pitchSin = -Math.Sin(pitchInRadians);
            var pitchCos = Math.Cos(pitchInRadians);

            var yawInRadians = lightPos.Yaw / 32768f * Math.PI;
            var x = -Math.Sin(yawInRadians) * pitchCos;
            var y = pitchSin;
            var z = Math.Cos(yawInRadians) * pitchCos;

            return new Vector3((float) x, (float) y, (float) z);
        }

        private void DrawSelectionScene() {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (_surfaceModel?.Blocks == null)
                return;
            using (_world.SolidShader.Use())
                foreach (var block in _surfaceModel.Blocks)
                    if (block.SelectionModel != null)
                        block.SelectionModel.Draw(_world.SolidShader);
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
                            Invalidate();

                if (_models?.ModelsByMemoryAddress != null)
                    foreach (var modelGroup in _models.ModelsByMemoryAddress.Values)
                        foreach (var model in modelGroup.Models)
                            if (model.UpdateAnimatedTextures() == true)
                                Invalidate();
            }
        }

        private AppState _appState = null;
        private AppState AppState {
            get {
                if (_appState == null)
                    _appState = AppState.RetrieveAppState();
                return _appState;
            }
        }

        private bool UpdateAppState(string propertyName, bool value) {
            var property = AppState.GetType().GetProperty(propertyName);
            if (property == null || (bool) property.GetValue(AppState, null) == value)
                return false;

            property.SetValue(AppState, value);
            AppState.Serialize();
            Invalidate();

            return true;
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
        public bool DrawSkyBox {
            get => AppState.ViewerDrawSkyBox;
            set => UpdateAppState(nameof(AppState.ViewerDrawSkyBox), value);
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
        public bool DrawNormals {
            get => AppState.ViewerDrawNormals;
            set => UpdateAppState(nameof(AppState.ViewerDrawNormals), value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RotateSpritesUp {
            get => AppState.ViewerRotateSpritesUp;
            set {
                if (UpdateAppState(nameof(AppState.ViewerRotateSpritesUp), value))
                    _renderer.InvalidateSpriteMatrices(_models);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawHelp {
            get => AppState.ViewerDrawHelp;
            set => UpdateAppState(nameof(AppState.ViewerDrawHelp), value);
        }

        private bool _tileSelectedNeedsUpdate = false;

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private WorldResources _world = null;
        private ModelResources _models = null;
        private SurfaceModelResources _surfaceModel = null;
        private GroundModelResources _groundModel = null;
        private SkyBoxModelResources _skyBoxModel = null;
        private SurfaceEditorResources _surfaceEditor = null;
        private GradientResources _gradients = null;

        private Renderer _renderer = null;

        private Framebuffer _selectFramebuffer;
    }
}
