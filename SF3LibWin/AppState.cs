using System;
using System.IO;
using System.Linq;
using CommonLib.Types;
using Newtonsoft.Json;
using SF3.Types;

namespace SF3.Win {
    public class AppState {
        private static AppState _globalAppState = null;

        public AppState() { }

        public AppState(string appName) {
            AppName = appName;
            FileFullPath = GetFileFullPath(appName);
        }

        public static bool Initialized => _globalAppState != null;

        public static AppState RetrieveAppState() {
            if (_globalAppState == null)
                _globalAppState = new AppState();
            return _globalAppState;
        }

        public static AppState RetrieveAppState(string appName) {
            if (_globalAppState == null) {
                _globalAppState = Deserialize(appName);
                if (_globalAppState == null) {
                    _globalAppState = new AppState(appName);
                    _globalAppState.Serialize();
                }
            }
            return _globalAppState;
        }

        public string GetFileFullPath()
            => GetFileFullPath(AppName);

        public static string GetFileFullPath(string appName) {
            if (appName == null)
                return null;
            else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SF3Tools", appName + " State.json");
        }

        public static AppState Deserialize(string appName) {
            if (appName == null)
                return null;

            var fullPath = GetFileFullPath(appName);
            if (fullPath == null || !File.Exists(fullPath))
                return null;

            try {
                var text = File.ReadAllText(fullPath);
                var newState = JsonConvert.DeserializeObject<AppState>(text);
                newState.AppName = appName;
                newState.FileFullPath = fullPath;
                newState.ViewerCursorMode = 0; /* 'Select' cursor */
                return newState;
            }
            catch {
                return null;
            }
        }

        public void Serialize() {
            if (FileFullPath == null)
                return;
            if (!File.Exists(FileFullPath))
                Directory.CreateDirectory(Path.GetDirectoryName(FileFullPath));
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FileFullPath, text);
        }

        /// <summary>
        /// Prepends a new file to the list of most recent files, truncating to 'max' if necessary.
        /// </summary>
        /// <param name="file">The new file to add at index 0.</param>
        /// <param name="max">The maximum number of files to keep in the 'RecentFiles' array.</param>
        public void PushRecentFile(string filename, ScenarioType scenario, SF3FileType fileType, int max = 10) {
            RecentFiles = new RecentFile[] { new RecentFile { Filename = filename, Scenario = scenario, FileType = fileType } }
                .Concat(RecentFiles
                    .Where(x => x.Filename != filename)
                    .Take(max - 1)
                )
                .ToArray();
        }

        /// <summary>
        /// Creates a NormalCalculationSettings based on settings in the AppState.
        /// </summary>
        /// <returns>A new NormalCalculationSettings instance.</returns>
        public NormalCalculationSettings MakeNormalCalculationSettings() {
            return new NormalCalculationSettings(
                UseImprovedNormalCalculations ? POLYGON_NormalCalculationMethod.WeightedVerticalTriangles : POLYGON_NormalCalculationMethod.TopRightTriangle,
                UseVanillaHalfHeightForSurfaceNormalCalculations,
                FixSurfaceMapTileNormalOverflowUnderflowErrors
            );
        }

        private void SetValue(ref bool valueVar, bool newValue, EventHandler changedEvent) {
            if (valueVar != newValue) {
                valueVar = newValue;
                changedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetValue(ref int valueVar, int newValue, EventHandler changedEvent) {
            if (valueVar != newValue) {
                valueVar = newValue;
                changedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Name of the application. Should be set when initializing the AppState.
        /// </summary>
        [JsonIgnore]
        public string AppName { get; private set; } = null;

        /// <summary>
        /// Filename of the AppState file with its absolute path. Should be set when initializing the AppState.
        /// </summary>
        [JsonIgnore]
        public string FileFullPath { get; private set; } = null;

        /// <summary>
        /// When enabled, the MPD Viewer will draw the surface model.
        /// </summary>
        public bool ViewerDrawSurfaceModel {
            get => _viewerDrawSurfaceModel;
            set => SetValue(ref _viewerDrawSurfaceModel, value, ViewerDrawSurfaceModelChanged);
        }
        private bool _viewerDrawSurfaceModel = true;
        public event EventHandler ViewerDrawSurfaceModelChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will draw models.
        /// </summary>
        public bool ViewerDrawModels {
            get => _viewerDrawModels;
            set => SetValue(ref _viewerDrawModels, value, ViewerDrawModelsChanged);
        }
        private bool _viewerDrawModels = true;
        public event EventHandler ViewerDrawModelsChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will draw the ground / water layer.
        /// </summary>
        public bool ViewerDrawGround {
            get => _viewerDrawGround;
            set => SetValue(ref _viewerDrawGround, value, ViewerDrawGroundChanged);
        }
        private bool _viewerDrawGround = true;
        public event EventHandler ViewerDrawGroundChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will draw the skyboxes present in Scenario 2 MPD files onward.
        /// </summary>
        public bool ViewerDrawSkyBox {
            get => _viewerDrawSkyBox;
            set => SetValue(ref _viewerDrawSkyBox, value, ViewerDrawSkyBoxChanged);
        }
        private bool _viewerDrawSkyBox = true;
        public event EventHandler ViewerDrawSkyBoxChanged;

        /// <summary>
        /// When enabled, texture animations will run in the MPD Viewer.
        /// </summary>
        public bool ViewerRunAnimations {
            get => _viewerRunAnimations;
            set => SetValue(ref _viewerRunAnimations, value, ViewerRunAnimationsChanged);
        }
        private bool _viewerRunAnimations = true;
        public event EventHandler ViewerRunAnimationsChanged;

        /// <summary>
        /// When enabled, lighting will be applied to models and the surface model in the MPD Viewer.
        /// </summary>
        public bool ViewerApplyLighting {
            get => _viewerApplyLighting;
            set => SetValue(ref _viewerApplyLighting, value, ViewerApplyLightingChanged);
        }
        private bool _viewerApplyLighting = true;
        public event EventHandler ViewerApplyLightingChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will apply gradients if they're available.
        /// </summary>
        public bool ViewerDrawGradients {
            get => _viewerDrawGradients;
            set => SetValue(ref _viewerDrawGradients, value, ViewerDrawGradientsChanged);
        }
        private bool _viewerDrawGradients = true;
        public event EventHandler ViewerDrawGradientsChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will draw wireframes on models and the surface model.
        /// </summary>
        public bool ViewerDrawWireframe {
            get => _viewerDrawWireframe;
            set => SetValue(ref _viewerDrawWireframe, value, ViewerDrawWireframeChanged);
        }
        private bool _viewerDrawWireframe = false;
        public event EventHandler ViewerDrawWireframeChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will overlay boxes for the camera boundaries and battle cursor boundaries.
        /// </summary>
        public bool ViewerDrawBoundaries {
            get => _viewerDrawBoundaries;
            set => SetValue(ref _viewerDrawBoundaries, value, ViewerDrawBoundariesChanged);
        }
        private bool _viewerDrawBoundaries = false;
        public event EventHandler ViewerDrawBoundariesChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will draw 2D collision lines that affect movement when freely walking.
        /// </summary>
        public bool ViewerDrawCollisionLines {
            get => _viewerDrawCollisionLines;
            set => SetValue(ref _viewerDrawCollisionLines, value, ViewerDrawCollisionLinesChanged);
        }
        private bool _viewerDrawCollisionLines = false;
        public event EventHandler ViewerDrawCollisionLinesChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will overlay an icon for terrain types over each tile, or darken the tile for 'NoEntry' terrain.
        /// </summary>
        public bool ViewerDrawTerrainTypes {
            get => _viewerDrawTerrainTypes;
            set => SetValue(ref _viewerDrawTerrainTypes, value, ViewerDrawTerrainTypesChanged);
        }
        private bool _viewerDrawTerrainTypes = false;
        public event EventHandler ViewerDrawTerrainTypesChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will overlay an icon for an event ID when present.
        /// </summary>
        public bool ViewerDrawEventIDs {
            get => _viewerDrawEventIDs;
            set => SetValue(ref _viewerDrawEventIDs, value, ViewerDrawEventIDsChanged);
        }
        private bool _viewerDrawEventIDs = false;
        public event EventHandler ViewerDrawEventIDsChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will render an RGB value representing the normal instead of a lit texture.
        /// </summary>
        public bool ViewerDrawNormals {
            get => _viewerDrawNormals;
            set => SetValue(ref _viewerDrawNormals, value, ViewerDrawNormalsChanged);
        }
        private bool _viewerDrawNormals = false;
        public event EventHandler ViewerDrawNormalsChanged;

        /// <summary>
        /// When enabled, trees will be rotated upwards to face the camera. This isn't done during gameplay,
        /// but useful for editing when viewing the terrain from above.
        /// </summary>
        public bool ViewerRotateSpritesUp {
            get => _viewerRotateSpritesUp;
            set => SetValue(ref _viewerRotateSpritesUp, value, ViewerRotateSpritesUpChanged);
        }
        private bool _viewerRotateSpritesUp = false;
        public event EventHandler ViewerRotateSpritesUpChanged;

        /// <summary>
        /// When enabled, MPDs are rendered on a black background instead of the usual cyan for visibility.
        /// </summary>
        public bool RenderOnBlackBackground {
            get => _renderOnBlackBackground;
            set => SetValue(ref _renderOnBlackBackground, value, RenderOnBlackBackgroundChanged);
        }
        private bool _renderOnBlackBackground = false;
        public event EventHandler RenderOnBlackBackgroundChanged;

        /// <summary>
        /// When enabled, the MPD Viewer will display a small 'Help' cheat sheet in the lower-right corner.
        /// </summary>
        public bool ViewerDrawHelp {
            get => _viewerDrawHelp;
            set => SetValue(ref _viewerDrawHelp, value, ViewerDrawHelpChanged);
        }
        private bool _viewerDrawHelp = true;
        public event EventHandler ViewerDrawHelpChanged;

        /// <summary>
        /// When set, models that are only visible from certain camera directions are not rendered.
        /// </summary>
        public bool HideModelsNotFacingCamera {
            get => _hideModelsNotFacingCamera;
            set => SetValue(ref _hideModelsNotFacingCamera, value, HideModelsNotFacingCameraChanged);
        }
        private bool _hideModelsNotFacingCamera = false;
        public event EventHandler HideModelsNotFacingCameraChanged;

        /// <summary>
        /// The current the cursor tool selected in the MPD Viewer.
        /// </summary>
        public int ViewerCursorMode {
            get => _viewerCursorMode;
            set => SetValue(ref _viewerCursorMode, value, ViewerCursorModeChanged);
        }
        private int _viewerCursorMode = 0;
        public event EventHandler ViewerCursorModeChanged;

        /// <summary>
        /// The current ScenarioType to open, serialized as an int for simplicity.
        /// </summary>
        public int OpenScenario {
            get => _openScenario;
            set => SetValue(ref _openScenario, value, OpenScenarioChanged);
        }
        private int _openScenario = -1;
        public event EventHandler OpenScenarioChanged;

        /// <summary>
        /// When 'true', clicking a value with a named value (e.g, an enemy ID that has an associated name like 'Masked Monk')
        /// will raise a dropdown with possible named values. When 'false', a numeric value editor will be raised instead.
        /// </summary>
        public bool UseDropdownsForNamedValues {
            get => _useDropdownsForNamedValues;
            set => SetValue(ref _useDropdownsForNamedValues, value, UseDropdownsForNamedValuesChanged);
        }
        private bool _useDropdownsForNamedValues = true;
        public event EventHandler UseDropdownsForNamedValuesChanged;

        /// <summary>
        /// When enabled, normals will be calculated using a method that considers all four corners of each quad, not just
        /// the triangle in the upper-right corner.
        /// </summary>
        public bool UseImprovedNormalCalculations {
            get => _useImprovedNormalCalculations;
            set => SetValue(ref _useImprovedNormalCalculations, value, UseImprovedNormalCalculationsChanged);
        }
        private bool _useImprovedNormalCalculations = true;
        public event EventHandler UseImprovedNormalCalculationsChanged;

        /// <summary>
        /// When disabled, the vanilla behavior is used, when halves the Y coordinates of every vertex when calculating the normal vector.
        /// When enabled, the Y coordinates are not halved when calculating the normal vector, which results in much more contrast between
        /// slopes and flat tiles, but is not accurate for SF3 maps.
        /// </summary>
        public bool UseVanillaHalfHeightForSurfaceNormalCalculations {
            get => _useVanillaHalfHeightForSurfaceNormalCalculations;
            set => SetValue(ref _useVanillaHalfHeightForSurfaceNormalCalculations, value, UseVanillaHalfHeightForSurfaceNormalCalculationsChanged);
        }
        private bool _useVanillaHalfHeightForSurfaceNormalCalculations = true;
        public event EventHandler UseVanillaHalfHeightForSurfaceNormalCalculationsChanged;

        /// <summary>
        /// Enables experimental map editing features that only work on a modified version of FIELD.MPD for the Premium Disk.
        /// You'll never want this unless you're working with the BlankField_V2.MPD template.
        /// </summary>
        public bool EnableExperimentalBlankFieldV2Brushes {
            get => _enableExperimentalBlankFieldV2Brushes;
            set => SetValue(ref _enableExperimentalBlankFieldV2Brushes, value, EnableExperimentalBlankFieldV2BrushesChanged);
        }
        private bool _enableExperimentalBlankFieldV2Brushes = false;
        public event EventHandler EnableExperimentalBlankFieldV2BrushesChanged;

        /// <summary>
        /// When enabled, several debugging options appear that aren't usually relevant.
        /// </summary>
        public bool EnableDebugSettings {
            get => _enableDebugSettings;
            set => SetValue(ref _enableDebugSettings, value, EnableDebugSettingsChanged);
        }
        private bool _enableDebugSettings = false;
        public event EventHandler EnableDebugSettingsChanged;

        /// <summary>
        /// When enabled, each component of surface map tile normal vectors are clamped to (roughly) within the -.5 to .5 range to
        /// prevent weird errors that occur when the "0.5 bit" is interpreted as the signed most-significant bit.
        /// (See implementation of ByteData.SetWeirdCompressedFIXED and GetWeirdCompressedFIXED to see why this could happen)
        /// </summary>
        public bool FixSurfaceMapTileNormalOverflowUnderflowErrors {
            get => _fixSurfaceMapTileNormalOverflowUnderflowErrors;
            set => SetValue(ref _fixSurfaceMapTileNormalOverflowUnderflowErrors, value, FixSurfaceMapTileNormalOverflowUnderflowErrorsChanged);
        }
        private bool _fixSurfaceMapTileNormalOverflowUnderflowErrors = true;
        public event EventHandler FixSurfaceMapTileNormalOverflowUnderflowErrorsChanged;

        public struct RecentFile {
            public string Filename { get; set; }
            public ScenarioType Scenario { get; set; }
            public SF3FileType FileType { get; set; }
        }

        /// <summary>
        /// List of recent files, with the first index being the most recent.
        /// </summary>
        public RecentFile[] RecentFiles {
            get => _recentFiles ?? [];
            set {
                var newValue = value ?? (_recentFiles?.Length == 0 ? _recentFiles : []);
                if (_recentFiles != newValue) {
                    _recentFiles = newValue;
                    RecentFilesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private RecentFile[] _recentFiles = [];
        public event EventHandler RecentFilesChanged;
    }
}
