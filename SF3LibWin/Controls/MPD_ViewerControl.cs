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
using SF3.Actors;
using SF3.Models.Structs.X1.Battle;
using SF3.Models.Structs.X1.Town;

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
            tsbDrawGround.Checked        = GLControl.DrawGround;
            tsbDrawSky.Checked           = GLControl.DrawSky;
            tsbRunAnimations.Checked     = GLControl.RunAnimations;
            tsbApplyLighting.Checked     = GLControl.ApplyLighting;
            tsbDrawGradients.Checked     = GLControl.DrawGradients;
            tsbDrawActors.Checked        = GLControl.DrawActors;

            tsbToggleWireframe.Checked   = GLControl.DrawWireframe;
            tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
            tsbToggleCollisions.Checked  = GLControl.DrawCollisionLines;
            tsbHideModelsNotFacingCamera.Checked = GLControl.HideModelsNotFacingCamera;
            tsbApplyShadowTags.Checked   = GLControl.ApplyShadowTags;
            tsbApplyHideTags.Checked     = GLControl.ApplyHideTags;

            tsbRenderOnBlackBackground.Checked = GLControl.RenderOnBlackBackground;
            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbRotateSpritesUp.Checked   = GLControl.RotateSpritesUp;

            var appState = AppState.RetrieveAppState();

            appState.ViewerDrawSurfaceModelChanged   += (s, e) => { tsbDrawSurfaceModel.Checked  = appState.ViewerDrawSurfaceModel; };
            appState.ViewerDrawModelsChanged         += (s, e) => { tsbDrawModels.Checked        = appState.ViewerDrawModels; };
            appState.ViewerDrawGroundChanged         += (s, e) => { tsbDrawGround.Checked        = appState.ViewerDrawGround; };
            appState.ViewerDrawSkyChanged            += (s, e) => { tsbDrawSky.Checked           = appState.ViewerDrawSky; };
            appState.ViewerRunAnimationsChanged      += (s, e) => { tsbRunAnimations.Checked     = appState.ViewerRunAnimations; };
            appState.ViewerApplyLightingChanged      += (s, e) => { tsbApplyLighting.Checked     = appState.ViewerApplyLighting; };
            appState.ViewerDrawGradientsChanged      += (s, e) => { tsbDrawGradients.Checked     = appState.ViewerDrawGradients; };
            appState.ViewerDrawActorsChanged         += (s, e) => { tsbDrawActors.Checked        = appState.ViewerDrawActors; };

            appState.ViewerDrawWireframeChanged      += (s, e) => { tsbToggleWireframe.Checked   = appState.ViewerDrawWireframe; };
            appState.ViewerDrawBoundariesChanged     += (s, e) => { tsbToggleBoundaries.Checked  = appState.ViewerDrawBoundaries; };
            appState.ViewerDrawTerrainTypesChanged   += (s, e) => { tsbToggleTerrainType.Checked = appState.ViewerDrawTerrainTypes; };
            appState.ViewerDrawEventIDsChanged       += (s, e) => { tsbToggleEventID.Checked     = appState.ViewerDrawEventIDs; };
            appState.ViewerDrawCollisionLinesChanged += (s, e) => { tsbToggleCollisions.Checked  = appState.ViewerDrawCollisionLines; };
            appState.HideModelsNotFacingCameraChanged += (s, e) => { tsbHideModelsNotFacingCamera.Checked = appState.HideModelsNotFacingCamera; };
            appState.ViewerApplyShadowTagsChanged    += (s, e) => { tsbApplyShadowTags.Checked   = appState.ViewerApplyShadowTags; };
            appState.ViewerApplyHideTagsChanged      += (s, e) => { tsbApplyHideTags.Checked     = appState.ViewerApplyHideTags; };

            appState.RenderOnBlackBackgroundChanged  += (s, e) => { tsbRenderOnBlackBackground.Checked = appState.RenderOnBlackBackground; };
            appState.ViewerDrawNormalsChanged        += (s, e) => { tsbToggleNormals.Checked     = appState.ViewerDrawNormals; };
            appState.ViewerRotateSpritesUpChanged    += (s, e) => { tsbRotateSpritesUp.Checked   = appState.ViewerRotateSpritesUp; };

            // Experimental controls that only apply to a modified FIELD.MPD on the PD (BlankField_V2.MPD).
            toolStrip2.Visible = appState.EnableExperimentalBlankFieldV2Brushes;
            appState.EnableExperimentalBlankFieldV2BrushesChanged += (s, e) => {
                var isEnabled = appState.EnableExperimentalBlankFieldV2Brushes;
                toolStrip2.Visible = isEnabled;
                if (!isEnabled && GLControl.CursorMode.IsDrawingMode())
                    GLControl.CursorMode = ViewerCursorMode.Select;
            };

            // Activate tile editor when an editor is clicked.
            GLControl.ObjectSelected += (s, obj) => {
                if (obj is IMPD_SurfaceTile tile)
                    SwitchToTileEditor(tile);
                else if (obj is IMPD_ModelInstance modelInstance)
                    SwitchToModelInstanceEditor(modelInstance);
                else if (obj is IActor actor) {
                    if (actor is Slot slot)
                        SwitchToActorBattleInstanceEditor(slot);
                    else if (actor is Npc npc)
                        SwitchToActorNPCInstanceEditor(npc);
                    else
                        SetSideEditorControl(null);
                }
                else
                    SetSideEditorControl(null);
            };
        }

        private SurfaceTilePropertiesControl SwitchToTileEditor(IMPD_SurfaceTile tile) {
            return SetSideEditorControl(
                tile, ref _surfaceTilePropertiesControl, 
                c => c.Tile = tile
            );
        }

        private ModelInstancePropertiesControl SwitchToModelInstanceEditor(IMPD_ModelInstance modelInstance) {
            return SetSideEditorControl(
                modelInstance, ref _modelInstancePropertiesControl, 
                c => { c.ModelInstance = modelInstance; }
            );
        }

        private ActorBattlePropertiesControl SwitchToActorBattleInstanceEditor(Slot actor) {
            return SetSideEditorControl(
                actor, ref _actorBattlePropertiesControl, 
                c => { c.Actor = actor; }
            );
        }

        private ActorNPCPropertiesControl SwitchToActorNPCInstanceEditor(Npc actor) {
            return SetSideEditorControl(
                actor, ref _actorNPCPropertiesControl, 
                c => { c.Actor = actor; }
            );
        }

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

        private TControl SetSideEditorControl<TObj, TControl>(TObj obj, ref TControl control, Action<TControl> controlActivateFunc)
        where TControl : PropertiesControlBase, new() {
            if (_currentSideEditorControl is TControl existingControl) {
                if (existingControl != null)
                    controlActivateFunc(existingControl);
                return existingControl;
            }

            if (control != null) {
                SetSideEditorControl(control, controlActivateFunc);
                return control;
            }

            control = new TControl();
            control.CmdKey += SideEditorCmdKeyHandler;
            SetSideEditorControl(control, controlActivateFunc);

            return control;
        }

        private void SetSideEditorControl(PropertiesControlBase control)
            => SetSideEditorControl(control, null);

        private void SetSideEditorControl<T>(T control, Action<T> init) where T : PropertiesControlBase {
            // Do nothing if we're already using that control.
            if (_currentSideEditorControl == control)
                return;

            SuspendLayout();

            // Replace the control with a new one. Keep it hidden for now until the layout is completed.
            if (_currentSideEditorControl != null)
                selectionPanel.Controls.Remove(_currentSideEditorControl);
            if (control != null) {
                control.Hide();
                init?.Invoke(control);
                selectionPanel.Controls.Add(control);
            }

            // Show or hide the panel depending on if a control exists.
            selectionPanel.Visible = (control != null);

            // NOTE: Uncomment to shift 3D viewer contents when the panel is active!
/*
            // Shift the projection matrix over if the panel is visible. This is a flicker-free alternative to
            // resizing the thing.
            var visibilityChanged = (control == null) ^ (_currentSideEditorControl == null);
            if (visibilityChanged) {
                GLControl.ProjectionXAdjustment = (control == null) ? 0 : -selectionPanel.Width / 2;
                GLControl.UpdateProjectionMatrices(GLControl.ClientSize.Width, GLControl.ClientSize.Height);
                GLControl.RenderFrame();
            }
*/

            ResumeLayout(true);
            control?.Show();

            _currentSideEditorControl = control;
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

        private void tsbDrawSurfaceModel_Click(object sender, EventArgs e)  => tsbDrawSurfaceModel.Checked  = GLControl.DrawSurfaceModel = !GLControl.DrawSurfaceModel;
        private void tsbDrawModels_Click(object sender, EventArgs e)        => tsbDrawModels.Checked        = GLControl.DrawModels       = !GLControl.DrawModels;
        private void tsbDrawGround_Click(object sender, EventArgs e)        => tsbDrawGround.Checked        = GLControl.DrawGround       = !GLControl.DrawGround;
        private void tsbDrawSky_Click(object sender, EventArgs e)           => tsbDrawSky.Checked           = GLControl.DrawSky          = !GLControl.DrawSky;
        private void tsbRunAnimations_Click(object sender, EventArgs e)     => tsbRunAnimations.Checked     = GLControl.RunAnimations    = !GLControl.RunAnimations;
        private void tsbApplyLighting_Click(object sender, EventArgs e)     => tsbApplyLighting.Checked     = GLControl.ApplyLighting    = !GLControl.ApplyLighting;
        private void tsbDrawGradients_Click(object sender, EventArgs e)     => tsbDrawGradients.Checked     = GLControl.DrawGradients    = !GLControl.DrawGradients;
        private void tsbDrawActors_Click(object sender, EventArgs e)        => tsbDrawActors.Checked        = GLControl.DrawActors       = !GLControl.DrawActors;

        private void tsbToggleWireframe_Click(object sender, EventArgs e)   => tsbToggleWireframe.Checked   = GLControl.DrawWireframe    = !GLControl.DrawWireframe;
        private void tsbToggleBoundaries_Click(object sender, EventArgs e)  => tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries   = !GLControl.DrawBoundaries;
        private void tsbToggleTerrainType_Click(object sender, EventArgs e) => tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes = !GLControl.DrawTerrainTypes;
        private void tsbToggleEventID_Click(object sender, EventArgs e)     => tsbToggleEventID.Checked     = GLControl.DrawEventIDs     = !GLControl.DrawEventIDs;
        private void tsbToggleCollisions_Click(object sender, EventArgs e)  => tsbToggleCollisions.Checked  = GLControl.DrawCollisionLines = !GLControl.DrawCollisionLines;
        private void tsbHideModelsNotFacingCamera_Click(object sender, EventArgs e) => tsbHideModelsNotFacingCamera.Checked = GLControl.HideModelsNotFacingCamera = !GLControl.HideModelsNotFacingCamera;
        private void tsbApplyShadowTags_Click(object sender, EventArgs e)   => tsbApplyShadowTags.Checked   = GLControl.ApplyShadowTags  = !GLControl.ApplyShadowTags;
        private void tsbApplyHideTags_Click(object sender, EventArgs e)     => tsbApplyHideTags.Checked     = GLControl.ApplyHideTags    = !GLControl.ApplyHideTags;

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
    }
}
