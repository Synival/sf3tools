using System;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Types;
using System.ComponentModel;
using CommonLib.Geometry;
using SF3.Win.OpenGL.MPD;
using SF3.MPD.Interfaces;
using SF3.Win.App;
using SF3.Models.Structs.X1.Battle;
using SF3.Models.Structs.X1.Town;
using CommonLib;
using static SF3.Win.Controls.MPD_ViewerGLControl;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            SuspendLayout();
            InitializeComponent();
            selectionPanel.Hide();
            ResumeLayout();

            Disposed += (s, e) => {
                GLControl.Dispose();
                _surfaceTilePropertiesControl?.Dispose();
                _modelInstancePropertiesControl?.Dispose();
                _actorBattlePropertiesControl?.Dispose();
                _actorNPCPropertiesControl?.Dispose();
            };

            var cursorMode = GLControl.CursorMode;
            GLControl.CursorModeChanged += (s, e) => UpdatedSelectedCursorModeButton();
            UpdatedSelectedCursorModeButton();

            tsbDrawSurfaceModel.Checked  = GLControl.DrawSurfaceModel;
            tsbDrawModels.Checked        = GLControl.DrawModels;
            tsbDrawExtraModels.Checked   = GLControl.DrawExtraModels;
            tsbDrawGround.Checked        = GLControl.DrawGround;
            tsbDrawSky.Checked           = GLControl.DrawSky;
            tsbRunAnimations.Checked     = GLControl.RunAnimations;
            tsbApplyLighting.Checked     = GLControl.ApplyLighting;
            tsbDrawGradients.Checked     = GLControl.DrawGradients;
            tsbDrawActors.Checked        = GLControl.DrawActors;

            tsbToggleWireframe.Checked   = GLControl.DrawWireframe;
            tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries;
            tsbToggleBattleZones.Checked = GLControl.DrawBattleZones;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
            tsbToggleCollisions.Checked  = GLControl.DrawCollisionLines;
            tsbHideModelsNotFacingCamera.Checked = GLControl.HideModelsNotFacingCamera;
            tsbApplyShadowTags.Checked   = GLControl.ApplyShadowTags;
            tsbApplyHideTags.Checked     = GLControl.ApplyHideTags;

            tsbRenderOnBlackBackground.Checked = GLControl.RenderOnBlackBackground;
            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbRotateSpritesUp.Checked   = GLControl.RotateSpritesUp;

            _appSettings = AppSettings.Get();

            _appSettingsRenderEventHandler = new DisposableEventHandlerCollection<EventHandler>(_appSettings);
            Disposed += (s, e) => _appSettingsRenderEventHandler?.Dispose();
            var eHandler = _appSettingsRenderEventHandler;

            eHandler.Subscribe(nameof(_appSettings.ViewerDrawSurfaceModelChanged),   ViewerDrawSurfaceModelChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawModelsChanged),         ViewerDrawModelsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawExtraModelsChanged),    ViewerDrawExtraModelsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawGroundChanged),         ViewerDrawGroundChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawSkyChanged),            ViewerDrawSkyChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerRunAnimationsChanged),      ViewerRunAnimationsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerApplyLightingChanged),      ViewerApplyLightingChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawGradientsChanged),      ViewerDrawGradientsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawActorsChanged),         ViewerDrawActorsChangedHandler);

            eHandler.Subscribe(nameof(_appSettings.ViewerDrawWireframeChanged),      ViewerDrawWireframeChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawBoundariesChanged),     ViewerDrawBoundariesChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawBattleZonesChanged),    ViewerDrawBattleZonesChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawTerrainTypesChanged),   ViewerDrawTerrainTypesChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawEventIDsChanged),       ViewerDrawEventIDsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawCollisionLinesChanged), ViewerDrawCollisionLinesChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.HideModelsNotFacingCameraChanged), HideModelsNotFacingCameraChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerApplyShadowTagsChanged),    ViewerApplyShadowTagsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerApplyHideTagsChanged),      ViewerApplyHideTagsChangedHandler);

            eHandler.Subscribe(nameof(_appSettings.RenderOnBlackBackgroundChanged),  RenderOnBlackBackgroundChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerDrawNormalsChanged),        ViewerDrawNormalsChangedHandler);
            eHandler.Subscribe(nameof(_appSettings.ViewerRotateSpritesUpChanged),    ViewerRotateSpritesUpChangedHandler);

            eHandler.Subscribe(nameof(_appSettings.EnableExperimentalBlankFieldV2BrushesChanged), EnableExperimentalBlankFieldV2BrushesChangedHandler);
            ShowHideExperimentalBrushes(_appSettings.EnableExperimentalBlankFieldV2Brushes);

            // Activate tile editor when an editor is clicked.
            _glControlEventHandler = new DisposableEventHandlerCollection<ObjectsSelectedChangedEventHandler>(GLControl);
            Disposed += (s, e) => _glControlEventHandler?.Dispose();

            _glControlEventHandler.Subscribe(nameof(GLControl.ObjectsSelectedChanged), (s, objs) => {
                if (objs.Length == 0)
                    UnsetSideEditorControl();
                else {
                    var firstObj = objs[0];
                    if (firstObj is IMPD_SurfaceTile) {
                        // TODO: multiple selection!
                        SwitchToTileEditor((IMPD_SurfaceTile) objs[0]);
                    }
                    else if (firstObj is IMPD_ModelInstance) {
                        // TODO: multiple selection!
                        SwitchToModelInstanceEditor((IMPD_ModelInstance) objs[0]);
                    }
                    else if (firstObj is Slot) {
                        // TODO: multiple selection!
                        SwitchToActorBattleInstanceEditor((Slot) objs[0]);
                    }
                    else if (firstObj is Npc) {
                        // TODO: multiple selection!
                        SwitchToActorNPCInstanceEditor((Npc) objs[0]);
                    }
                    else
                        UnsetSideEditorControl();
                }
            });
        }

        void ViewerDrawSurfaceModelChangedHandler   (object sender, EventArgs args) => tsbDrawSurfaceModel.Checked  = _appSettings.ViewerDrawSurfaceModel;
        void ViewerDrawModelsChangedHandler         (object sender, EventArgs args) => tsbDrawModels.Checked        = _appSettings.ViewerDrawModels;
        void ViewerDrawExtraModelsChangedHandler    (object sender, EventArgs args) => tsbDrawExtraModels.Checked   = _appSettings.ViewerDrawExtraModels;
        void ViewerDrawGroundChangedHandler         (object sender, EventArgs args) => tsbDrawGround.Checked        = _appSettings.ViewerDrawGround;
        void ViewerDrawSkyChangedHandler            (object sender, EventArgs args) => tsbDrawSky.Checked           = _appSettings.ViewerDrawSky;
        void ViewerRunAnimationsChangedHandler      (object sender, EventArgs args) => tsbRunAnimations.Checked     = _appSettings.ViewerRunAnimations;
        void ViewerApplyLightingChangedHandler      (object sender, EventArgs args) => tsbApplyLighting.Checked     = _appSettings.ViewerApplyLighting;
        void ViewerDrawGradientsChangedHandler      (object sender, EventArgs args) => tsbDrawGradients.Checked     = _appSettings.ViewerDrawGradients;
        void ViewerDrawActorsChangedHandler         (object sender, EventArgs args) => tsbDrawActors.Checked        = _appSettings.ViewerDrawActors;
        void ViewerDrawWireframeChangedHandler      (object sender, EventArgs args) => tsbToggleWireframe.Checked   = _appSettings.ViewerDrawWireframe;
        void ViewerDrawBoundariesChangedHandler     (object sender, EventArgs args) => tsbToggleBoundaries.Checked  = _appSettings.ViewerDrawBoundaries;
        void ViewerDrawBattleZonesChangedHandler    (object sender, EventArgs args) => tsbToggleBattleZones.Checked = _appSettings.ViewerDrawBattleZones;
        void ViewerDrawTerrainTypesChangedHandler   (object sender, EventArgs args) => tsbToggleTerrainType.Checked = _appSettings.ViewerDrawTerrainTypes;
        void ViewerDrawEventIDsChangedHandler       (object sender, EventArgs args) => tsbToggleEventID.Checked     = _appSettings.ViewerDrawEventIDs;
        void ViewerDrawCollisionLinesChangedHandler (object sender, EventArgs args) => tsbToggleCollisions.Checked  = _appSettings.ViewerDrawCollisionLines;
        void HideModelsNotFacingCameraChangedHandler(object sender, EventArgs args) => tsbHideModelsNotFacingCamera.Checked = _appSettings.HideModelsNotFacingCamera;
        void ViewerApplyShadowTagsChangedHandler    (object sender, EventArgs args) => tsbApplyShadowTags.Checked   = _appSettings.ViewerApplyShadowTags;
        void ViewerApplyHideTagsChangedHandler      (object sender, EventArgs args) => tsbApplyHideTags.Checked     = _appSettings.ViewerApplyHideTags;
        void RenderOnBlackBackgroundChangedHandler  (object sender, EventArgs args) => tsbRenderOnBlackBackground.Checked = _appSettings.RenderOnBlackBackground;
        void ViewerDrawNormalsChangedHandler        (object sender, EventArgs args) => tsbToggleNormals.Checked     = _appSettings.ViewerDrawNormals;
        void ViewerRotateSpritesUpChangedHandler    (object sender, EventArgs args) => tsbRotateSpritesUp.Checked   = _appSettings.ViewerRotateSpritesUp;

        void EnableExperimentalBlankFieldV2BrushesChangedHandler(object sender, EventArgs args) {
            var isEnabled = _appSettings.EnableExperimentalBlankFieldV2Brushes;
            ShowHideExperimentalBrushes(isEnabled);
            if (!isEnabled && GLControl.CursorMode.IsDrawingMode())
                GLControl.CursorMode = ViewerCursorMode.Select;
        }

        // Experimental controls that only apply to a modified FIELD.MPD on the PD (BlankField_V2.MPD).
        private void ShowHideExperimentalBrushes(bool value) {
            tsbSeparator4.Visible        = value;
            tsbDrawBridge.Visible        = value;
            tsbDrawBrownMountain.Visible = value;
            tsbDrawDarkGrass.Visible     = value;
            tsbDrawDesert.Visible        = value;
            tsbDrawDirt.Visible          = value;
            tsbDrawForest.Visible        = value;
            tsbDrawGrassland.Visible     = value;
            tsbDrawGreyMountain.Visible  = value;
            tsbDrawMountainPeak.Visible  = value;
            tsbDrawNoEntry.Visible       = value;
            tsbDrawRiver.Visible         = value;
            tsbDrawWater.Visible         = value;
            tsbSeparator5.Visible        = value;
            tsbFixTiles.Visible          = value;
        }

        private SurfaceTilePropertiesControl SwitchToTileEditor(IMPD_SurfaceTile tile)
            => SetSideEditorControl(ref _surfaceTilePropertiesControl, tile);
        private ModelInstancePropertiesControl SwitchToModelInstanceEditor(IMPD_ModelInstance modelInstance)
            => SetSideEditorControl(ref _modelInstancePropertiesControl, modelInstance);
        private ActorBattlePropertiesControl SwitchToActorBattleInstanceEditor(Slot actor)
            => SetSideEditorControl(ref _actorBattlePropertiesControl, actor);
        private ActorNPCPropertiesControl SwitchToActorNPCInstanceEditor(Npc actor)
            => SetSideEditorControl(ref _actorNPCPropertiesControl, actor);

        private void SideEditorCmdKeyHandler(object sender, ref Message msg, Keys keyData, ref bool wasProcessed) {
            if (wasProcessed)
                return;

            bool sendToGLControl = false;

            var keyPressed = (Keys) ((int) keyData & 0xFFFF);
            switch (keyPressed) {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    if (keyData.HasFlag(Keys.Control))
                        sendToGLControl = true;
                    break;
            }

            if (sendToGLControl)
                GLControl.RunCmdKeyEvent(sender, ref msg, keyData, ref wasProcessed);
        }

        private TControl SetSideEditorControl<TControl, TObj>(ref TControl control, TObj obj)
        where TControl : PropertiesControlBase<TObj>, new()
        where TObj : class {
            // Create the control if it doesn't exist.
            if (control == null) {
                control = new TControl();
                control.CmdKey += SideEditorCmdKeyHandler;
                control.Viewer = this;
            }
            // Do nothing if we're already using that control.
            else if (_currentSideEditorControl == control) {
                control.EditingObjects = (obj == null) ? [] : [obj];
                return control;
            }

            SuspendLayout();

            if (_currentSideEditorControl != null)
                selectionPanel.Controls.Remove(_currentSideEditorControl);

            // Hiding this control prevents some ugly rendering...
            control.Hide();

            control.EditingObjects = (obj == null) ? [] : [obj];
            selectionPanel.Controls.Add(control);

            ShowEditorPanel(true);

            ResumeLayout(true);

            // Show the control now. For some reason, it renders nicely this way.
            control?.Show();

            _currentSideEditorControl = control;
            return control;
        }

        private void UnsetSideEditorControl() {
            // Do nothing if the editor is already unset.
            if (_currentSideEditorControl == null)
                return;

            SuspendLayout();

            selectionPanel.Controls.Remove(_currentSideEditorControl);
            ShowEditorPanel(false);

            ResumeLayout(true);

            _currentSideEditorControl = null;
        }

        private void ShowEditorPanel(bool show) {
            selectionPanel.Visible = show;

            // NOTE: Uncomment to shift 3D viewer contents when the panel is active!
            /*
                        // Shift the projection matrix over if the panel is visible. This is a flicker-free alternative to
                        // resizing the thing.
                        var visibilityChanged = !show ^ (_currentSideEditorControl == null);
                        if (visibilityChanged) {
                            GLControl.ProjectionXAdjustment = show ? -selectionPanel.Width / 2 : 0;
                            GLControl.UpdateProjectionMatrices(GLControl.ClientSize.Width, GLControl.ClientSize.Height);
                            GLControl.RenderFrame();
                        }
            */
        }

        private void UpdatedSelectedCursorModeButton() {
            var cursorMode = GLControl.CursorMode;
            tsbCursorSelect.Checked      = cursorMode == ViewerCursorMode.Select;
            tsbCursorNavigate.Checked    = cursorMode == ViewerCursorMode.Navigate;
            tsbDrawGrassland.Checked     = cursorMode == ViewerCursorMode.DrawGrassland;
            tsbDrawDirt.Checked          = cursorMode == ViewerCursorMode.DrawDirt;
            tsbDrawDarkGrass.Checked     = cursorMode == ViewerCursorMode.DrawDarkGrass;
            tsbDrawForest.Checked        = cursorMode == ViewerCursorMode.DrawForest;
            tsbDrawBrownMountain.Checked = cursorMode == ViewerCursorMode.DrawBrownMountain;
            tsbDrawGreyMountain.Checked  = cursorMode == ViewerCursorMode.DrawGreyMountain;
            tsbDrawMountainPeak.Checked  = cursorMode == ViewerCursorMode.DrawMountainPeak;
            tsbDrawDesert.Checked        = cursorMode == ViewerCursorMode.DrawDesert;
            tsbDrawRiver.Checked         = cursorMode == ViewerCursorMode.DrawRiver;
            tsbDrawBridge.Checked        = cursorMode == ViewerCursorMode.DrawBridge;
            tsbDrawWater.Checked         = cursorMode == ViewerCursorMode.DrawWater;
            tsbDrawNoEntry.Checked       = cursorMode == ViewerCursorMode.DrawNoEntry;
        }

        private IMPD _mpdFile = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD MPD_File {
            get => _mpdFile;
            set {
                if (value != _mpdFile) {
                    _mpdFile = value;
                    GLControl.MPD_File = value;
                }
            }
        }

        public MPD_ViewerGLControl GLControl => mpdViewerGLControl1;

        private void tsbDrawSurfaceModel_Click(object sender, EventArgs e) => tsbDrawSurfaceModel.Checked  = GLControl.DrawSurfaceModel = !GLControl.DrawSurfaceModel;
        private void tsbDrawModels_Click(object sender, EventArgs e) => tsbDrawModels.Checked        = GLControl.DrawModels       = !GLControl.DrawModels;
        private void tsbDrawExtraModels_Click(object sender, EventArgs e) => tsbDrawExtraModels.Checked   = GLControl.DrawExtraModels  = !GLControl.DrawExtraModels;
        private void tsbDrawGround_Click(object sender, EventArgs e) => tsbDrawGround.Checked        = GLControl.DrawGround       = !GLControl.DrawGround;
        private void tsbDrawSky_Click(object sender, EventArgs e) => tsbDrawSky.Checked           = GLControl.DrawSky          = !GLControl.DrawSky;
        private void tsbRunAnimations_Click(object sender, EventArgs e) => tsbRunAnimations.Checked     = GLControl.RunAnimations    = !GLControl.RunAnimations;
        private void tsbApplyLighting_Click(object sender, EventArgs e) => tsbApplyLighting.Checked     = GLControl.ApplyLighting    = !GLControl.ApplyLighting;
        private void tsbDrawGradients_Click(object sender, EventArgs e) => tsbDrawGradients.Checked     = GLControl.DrawGradients    = !GLControl.DrawGradients;
        private void tsbDrawActors_Click(object sender, EventArgs e) => tsbDrawActors.Checked        = GLControl.DrawActors       = !GLControl.DrawActors;

        private void tsbToggleWireframe_Click(object sender, EventArgs e) => tsbToggleWireframe.Checked   = GLControl.DrawWireframe    = !GLControl.DrawWireframe;
        private void tsbToggleBoundaries_Click(object sender, EventArgs e) => tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries   = !GLControl.DrawBoundaries;
        private void tsbToggleBattleZones_Click(object sender, EventArgs e) => tsbToggleBattleZones.Checked = GLControl.DrawBattleZones  = !GLControl.DrawBattleZones;
        private void tsbToggleTerrainType_Click(object sender, EventArgs e) => tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes = !GLControl.DrawTerrainTypes;
        private void tsbToggleEventID_Click(object sender, EventArgs e) => tsbToggleEventID.Checked     = GLControl.DrawEventIDs     = !GLControl.DrawEventIDs;
        private void tsbToggleCollisions_Click(object sender, EventArgs e) => tsbToggleCollisions.Checked  = GLControl.DrawCollisionLines = !GLControl.DrawCollisionLines;
        private void tsbHideModelsNotFacingCamera_Click(object sender, EventArgs e) => tsbHideModelsNotFacingCamera.Checked = GLControl.HideModelsNotFacingCamera = !GLControl.HideModelsNotFacingCamera;
        private void tsbApplyShadowTags_Click(object sender, EventArgs e) => tsbApplyShadowTags.Checked   = GLControl.ApplyShadowTags  = !GLControl.ApplyShadowTags;
        private void tsbApplyHideTags_Click(object sender, EventArgs e) => tsbApplyHideTags.Checked     = GLControl.ApplyHideTags    = !GLControl.ApplyHideTags;

        private void tsbRenderOnBlackBackground_Click(object sender, EventArgs e) => tsbRenderOnBlackBackground.Checked = GLControl.RenderOnBlackBackground = !GLControl.RenderOnBlackBackground;
        private void tsbToggleNormals_Click(object sender, EventArgs e) => tsbToggleNormals.Checked     = GLControl.DrawNormals      = !GLControl.DrawNormals;
        private void tsbRotateSpritesUp_Click(object sender, EventArgs e) => tsbRotateSpritesUp.Checked   = GLControl.RotateSpritesUp  = !GLControl.RotateSpritesUp;

        public void InvalidateLighting(bool invalidatePainter = true) {
            if (MPD_File != null) {
                GLControl.InvalidateLightPosition(invalidatePainter: false);
                GLControl.InvalidateLightingTexture(invalidatePainter: false);
                if (invalidatePainter)
                    GLControl.InvalidateFrame();
            }
        }

        public void InvalidateMPDResources() {
            if (MPD_File != null) {
                if (MPD_File is IMPD_File mpdFile) {
                    mpdFile.AssociateTilesWithTrees();
                    // TODO: UpdatePlaneImages() shouldn't be necessary!!
                    mpdFile.UpdatePlaneImages();
                }
                GLControl.InvalidateAllResources();
            }
        }

        private struct CameraRefs {
            public float Width;
            public float Height;
            public Vector3 Center;
        }

        private CameraRefs CreateCameraRefs() {
            float width   = 64.0f;
            float depth   = 64.0f;
            float centerX = 0.0f;
            float groundY = 0.0f;
            float centerZ = 0.0f;

            var boundaries =
                new IRectangleShort[] {
                    MPD_File.CameraBoundaries,
                    MPD_File.BattleCursorBoundaries
                }
                .Where(x => x != null)
                .ToArray();

            if (boundaries.Length > 0) {
                var x1 = boundaries.Min(x => x.X1);
                var z1 = boundaries.Min(x => x.Y1);
                var x2 = boundaries.Max(x => x.X2);
                var z2 = boundaries.Max(x => x.Y2);

                width   = (x2 - x1) / 32.00f;
                depth   = (z2 - z1) / 32.00f;
                centerX = (x1 + x2) / 2.0f /  32.00f + GeneralResources.ModelOffsetX;
                centerZ = (z1 + z2) / 2.0f / -32.00f + GeneralResources.ModelOffsetZ + 64.00f;
                groundY = (MPD_File?.Planes?.GroundY ?? 0) / -32.0f;
            }

            return new CameraRefs {
                Width  = width,
                Height = depth,
                Center = new Vector3(centerX, groundY, centerZ)
            };
        }

        private void tsbCameraReset_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();
            var size = Math.Max(refs.Width, refs.Height);
            GLControl.ResetCamera(refs.Center + (0, size * 0.071f, 0), size * 2f);
            GLControl.InvalidateFrame();
        }

        private void tsbCameraTopView_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();

            var screenRatio = (float) GLControl.Width / GLControl.Height;
            var zoomFactorWidth = refs.Width / screenRatio;
            var zoomFactorHeight = refs.Height;

            GLControl.Position = (refs.Center.X, Math.Max(zoomFactorWidth, zoomFactorHeight) * 2.75f + refs.Center.Y, refs.Center.Z);
            GLControl.Pitch    = -90;
            GLControl.Yaw      = 0;
            GLControl.InvalidateFrame();
        }

        private void tsbCameraLookAtCenter_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();
            GLControl.LookAtTarget(refs.Center);
            GLControl.InvalidateFrame();
        }

        private void tsbCursorSelect_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.Select;
        private void tsbCursorNavigate_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.Navigate;

        private void tsbDrawGrassland_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawGrassland;
        private void tsbDrawDirt_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawDirt;
        private void tsbDrawDarkGrass_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawDarkGrass;
        private void tsbDrawForest_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawForest;
        private void tsbDrawBrownMountain_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawBrownMountain;
        private void tsbDrawGreyMountain_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawGreyMountain;
        private void tsbDrawMountainPeak_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawMountainPeak;
        private void tsbDrawDesert_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawDesert;
        private void tsbDrawRiver_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawRiver;
        private void tsbDrawBridge_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawBridge;
        private void tsbDrawWater_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawWater;
        private void tsbDrawNoEntry_Click(object sender, EventArgs e) => GLControl.CursorMode = ViewerCursorMode.DrawNoEntry;

        // TODO: big dumb hack!!!
        private void tsbFixTiles_Click(object sender, EventArgs e) {
            if (MPD_File != null) {
                FieldEditing.FieldEditing.UpdateTileTextures(MPD_File.Surface, true);
                GLControl.InvalidateModels();
            }
        }

        private PropertiesControlBase _currentSideEditorControl = null;

        private SurfaceTilePropertiesControl   _surfaceTilePropertiesControl   = null;
        private ModelInstancePropertiesControl _modelInstancePropertiesControl = null;
        private ActorBattlePropertiesControl   _actorBattlePropertiesControl   = null;
        private ActorNPCPropertiesControl      _actorNPCPropertiesControl      = null;

        private AppSettings _appSettings;
        private DisposableEventHandlerCollection<EventHandler> _appSettingsRenderEventHandler;
        private DisposableEventHandlerCollection<ObjectsSelectedChangedEventHandler> _glControlEventHandler;
    }
}
