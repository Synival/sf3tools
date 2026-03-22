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

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            SuspendLayout();
            InitializeComponent();
            selectionPanel.Hide();
            ResumeLayout();

            Disposed += (s, e) => GLControl.Dispose();

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

            var appSettings = AppSettings.Get();
            _appSettingsRenderEventHandler = new DisposableEventHandlerCollection<EventHandler>(appSettings);

            Disposed += (s, e) => {
                if (_appSettingsRenderEventHandler != null) {
                    _appSettingsRenderEventHandler.Dispose();
                    _appSettingsRenderEventHandler = null;
                }
            };

            var eHandler = _appSettingsRenderEventHandler;

            eHandler.Subscribe(nameof(appSettings.ViewerDrawSurfaceModelChanged),   (s, e) => { tsbDrawSurfaceModel.Checked  = appSettings.ViewerDrawSurfaceModel; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawModelsChanged),         (s, e) => { tsbDrawModels.Checked        = appSettings.ViewerDrawModels; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawExtraModelsChanged),    (s, e) => { tsbDrawExtraModels.Checked   = appSettings.ViewerDrawExtraModels; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawGroundChanged),         (s, e) => { tsbDrawGround.Checked        = appSettings.ViewerDrawGround; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawSkyChanged),            (s, e) => { tsbDrawSky.Checked           = appSettings.ViewerDrawSky; });
            eHandler.Subscribe(nameof(appSettings.ViewerRunAnimationsChanged),      (s, e) => { tsbRunAnimations.Checked     = appSettings.ViewerRunAnimations; });
            eHandler.Subscribe(nameof(appSettings.ViewerApplyLightingChanged),      (s, e) => { tsbApplyLighting.Checked     = appSettings.ViewerApplyLighting; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawGradientsChanged),      (s, e) => { tsbDrawGradients.Checked     = appSettings.ViewerDrawGradients; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawActorsChanged),         (s, e) => { tsbDrawActors.Checked        = appSettings.ViewerDrawActors; });

            eHandler.Subscribe(nameof(appSettings.ViewerDrawWireframeChanged),      (s, e) => { tsbToggleWireframe.Checked   = appSettings.ViewerDrawWireframe; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawBoundariesChanged),     (s, e) => { tsbToggleBoundaries.Checked  = appSettings.ViewerDrawBoundaries; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawBattleZonesChanged),    (s, e) => { tsbToggleBattleZones.Checked = appSettings.ViewerDrawBattleZones; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawTerrainTypesChanged),   (s, e) => { tsbToggleTerrainType.Checked = appSettings.ViewerDrawTerrainTypes; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawEventIDsChanged),       (s, e) => { tsbToggleEventID.Checked     = appSettings.ViewerDrawEventIDs; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawCollisionLinesChanged), (s, e) => { tsbToggleCollisions.Checked  = appSettings.ViewerDrawCollisionLines; });
            eHandler.Subscribe(nameof(appSettings.HideModelsNotFacingCameraChanged), (s, e) => { tsbHideModelsNotFacingCamera.Checked = appSettings.HideModelsNotFacingCamera; });
            eHandler.Subscribe(nameof(appSettings.ViewerApplyShadowTagsChanged),    (s, e) => { tsbApplyShadowTags.Checked   = appSettings.ViewerApplyShadowTags; });
            eHandler.Subscribe(nameof(appSettings.ViewerApplyHideTagsChanged),      (s, e) => { tsbApplyHideTags.Checked     = appSettings.ViewerApplyHideTags; });

            eHandler.Subscribe(nameof(appSettings.RenderOnBlackBackgroundChanged),  (s, e) => { tsbRenderOnBlackBackground.Checked = appSettings.RenderOnBlackBackground; });
            eHandler.Subscribe(nameof(appSettings.ViewerDrawNormalsChanged),        (s, e) => { tsbToggleNormals.Checked     = appSettings.ViewerDrawNormals; });
            eHandler.Subscribe(nameof(appSettings.ViewerRotateSpritesUpChanged),    (s, e) => { tsbRotateSpritesUp.Checked   = appSettings.ViewerRotateSpritesUp; });

            // Experimental controls that only apply to a modified FIELD.MPD on the PD (BlankField_V2.MPD).
            void ShowHideExperimentalBrushes(bool value) {
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

            eHandler.Subscribe(nameof(appSettings.EnableExperimentalBlankFieldV2BrushesChanged), (s, e) => {
                var isEnabled = appSettings.EnableExperimentalBlankFieldV2Brushes;
                ShowHideExperimentalBrushes(isEnabled);
                if (!isEnabled && GLControl.CursorMode.IsDrawingMode())
                    GLControl.CursorMode = ViewerCursorMode.Select;
            });
            ShowHideExperimentalBrushes(appSettings.EnableExperimentalBlankFieldV2Brushes);

            // Activate tile editor when an editor is clicked.
            GLControl.ObjectsSelectedChanged += (s, objs) => {
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
            };
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

        private DisposableEventHandlerCollection<EventHandler> _appSettingsRenderEventHandler;
    }
}
