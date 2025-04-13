using System;
using System.IO;
using System.Linq;
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

        [JsonIgnore]
        public string AppName { get; private set; } = null;

        [JsonIgnore]
        public string FileFullPath { get; private set; } = null;

        public bool ViewerDrawSurfaceModel {
            get => _viewerDrawSurfaceModel;
            set {
                if (value != _viewerDrawSurfaceModel) {
                    _viewerDrawSurfaceModel = value;
                    ViewerDrawSurfaceModelChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawSurfaceModel = true;
        public event EventHandler ViewerDrawSurfaceModelChanged;

        public bool ViewerDrawModels {
            get => _viewerDrawModels;
            set {
                if (value != _viewerDrawModels) {
                    _viewerDrawModels = value;
                    ViewerDrawModelsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawModels = true;
        public event EventHandler ViewerDrawModelsChanged;

        public bool ViewerDrawGround {
            get => _viewerDrawGround;
            set {
                if (value != _viewerDrawGround) {
                    _viewerDrawGround = value;
                    ViewerDrawGroundChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawGround = true;
        public event EventHandler ViewerDrawGroundChanged;

        public bool ViewerDrawSkyBox {
            get => _viewerDrawSkyBox;
            set {
                if (value != _viewerDrawSkyBox) {
                    _viewerDrawSkyBox = value;
                    ViewerDrawSkyBoxChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawSkyBox = true;
        public event EventHandler ViewerDrawSkyBoxChanged;

        public bool ViewerRunAnimations {
            get => _viewerRunAnimations;
            set {
                if (value != _viewerRunAnimations) {
                    _viewerRunAnimations = value;
                    ViewerRunAnimationsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerRunAnimations = true;
        public event EventHandler ViewerRunAnimationsChanged;

        public bool ViewerApplyLighting {
            get => _viewerApplyLighting;
            set {
                if (value != _viewerApplyLighting) {
                    _viewerApplyLighting = value;
                    ViewerApplyLightingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerApplyLighting = true;
        public event EventHandler ViewerApplyLightingChanged;

        public bool ViewerDrawGradients {
            get => _viewerDrawGradients;
            set {
                if (value != _viewerDrawGradients) {
                    _viewerDrawGradients = value;
                    ViewerDrawGradientsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawGradients = true;
        public event EventHandler ViewerDrawGradientsChanged;

        public bool ViewerDrawWireframe {
            get => _viewerDrawWireframe;
            set {
                if (value != _viewerDrawWireframe) {
                    _viewerDrawWireframe = value;
                    ViewerDrawWireframeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawWireframe = false;
        public event EventHandler ViewerDrawWireframeChanged;

        public bool ViewerDrawBoundaries {
            get => _viewerDrawBoundaries;
            set {
                if (value != _viewerDrawBoundaries) {
                    _viewerDrawBoundaries = value;
                    ViewerDrawBoundariesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawBoundaries = false;
        public event EventHandler ViewerDrawBoundariesChanged;

        public bool ViewerDrawCollisionLines {
            get => _viewerDrawCollisionLines;
            set {
                if (value != _viewerDrawCollisionLines) {
                    _viewerDrawCollisionLines = value;
                    ViewerDrawCollisionLinesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawCollisionLines = false;
        public event EventHandler ViewerDrawCollisionLinesChanged;

        public bool ViewerDrawTerrainTypes {
            get => _viewerDrawTerrainTypes;
            set {
                if (value != _viewerDrawTerrainTypes) {
                    _viewerDrawTerrainTypes = value;
                    ViewerDrawTerrainTypesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawTerrainTypes = false;
        public event EventHandler ViewerDrawTerrainTypesChanged;

        public bool ViewerDrawEventIDs {
            get => _viewerDrawEventIDs;
            set {
                if (value != _viewerDrawEventIDs) {
                    _viewerDrawEventIDs = value;
                    ViewerDrawEventIDsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawEventIDs = false;
        public event EventHandler ViewerDrawEventIDsChanged;

        public bool ViewerDrawNormals {
            get => _viewerDrawNormals;
            set {
                if (value != _viewerDrawNormals) {
                    _viewerDrawNormals = value;
                    ViewerDrawNormalsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawNormals = false;
        public event EventHandler ViewerDrawNormalsChanged;

        public bool ViewerRotateSpritesUp {
            get => _viewerRotateSpritesUp;
            set {
                if (value != _viewerRotateSpritesUp) {
                    _viewerRotateSpritesUp = value;
                    ViewerRotateSpritesUpChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerRotateSpritesUp = false;
        public event EventHandler ViewerRotateSpritesUpChanged;

        public bool ViewerDrawHelp {
            get => _viewerDrawHelp;
            set {
                if (value != _viewerDrawHelp) {
                    _viewerDrawHelp = value;
                    ViewerDrawHelpChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _viewerDrawHelp = true;
        public event EventHandler ViewerDrawHelpChanged;

        public int ViewerCursorMode { get; set; } = 0;

        /// <summary>
        /// The current ScenarioType to open, serialized as an int for simplicity.
        /// </summary>
        public int OpenScenario {
            get => _openScenario;
            set {
                if (value != _openScenario) {
                    _openScenario = value;
                    OpenScenarioChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private int _openScenario = -1;
        public event EventHandler OpenScenarioChanged;

        /// <summary>
        /// When 'true', clicking a value with a named value (e.g, an enemy ID that has an associated name like 'Masked Monk')
        /// will raise a dropdown with possible named values. When 'false', a numeric value editor will be raised instead.
        /// </summary>
        public bool UseDropdownsForNamedValues {
            get => _useDropdownsForNamedValues;
            set {
                if (value != _useDropdownsForNamedValues) {
                    _useDropdownsForNamedValues = value;
                    UseDropdownsForNamedValuesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _useDropdownsForNamedValues = true;
        public event EventHandler UseDropdownsForNamedValuesChanged;

        public bool UseVanillaHalfHeightForSurfaceNormalCalculations { get; set; } = true;

        /// <summary>
        /// Enables experimental map editing features that only work on a modified version of FIELD.MPD for the Premium Disk.
        /// You'll never want this unless you're working with the BlankField_V2.MPD template.
        /// </summary>
        public bool EnableExperimentalBlankFieldV2Brushes {
            get => _enableExperimentalBlankFieldV2Brushes;
            set {
                if (value != _enableExperimentalBlankFieldV2Brushes) {
                    _enableExperimentalBlankFieldV2Brushes = value;
                    EnableExperimentalBlankFieldV2BrushesChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _enableExperimentalBlankFieldV2Brushes = false;
        public event EventHandler EnableExperimentalBlankFieldV2BrushesChanged;

        /// <summary>
        /// When enabled, several debugging options appear that aren't usually relevant.
        /// </summary>
        public bool EnableDebugSettings {
            get => _enableDebugSettings;
            set {
                if (value != _enableDebugSettings) {
                    _enableDebugSettings = value;
                    EnableDebugSettingsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _enableDebugSettings = false;
        public event EventHandler EnableDebugSettingsChanged;

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
