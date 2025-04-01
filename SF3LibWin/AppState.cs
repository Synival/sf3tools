using System;
using System.IO;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public string AppName { get; private set; } = null;

        [JsonIgnore]
        public string FileFullPath { get; private set; } = null;

        public bool ViewerDrawSurfaceModel { get; set; } = true;
        public bool ViewerDrawModels { get; set; } = true;
        public bool ViewerDrawGround { get; set; } = true;
        public bool ViewerDrawSkyBox { get; set; } = true;
        public bool ViewerRunAnimations { get; set; } = true;
        public bool ViewerApplyLighting { get; set; } = true;
        public bool ViewerDrawGradients { get; set; } = true;

        public bool ViewerDrawWireframe { get; set; } = false;
        public bool ViewerDrawBoundaries { get; set; } = false;
        public bool ViewerDrawCollisionLines { get; set; } = false;
        public bool ViewerDrawTerrainTypes { get; set; } = false;
        public bool ViewerDrawEventIDs { get; set; } = false;
        public bool ViewerDrawNormals { get; set; } = false;

        public bool ViewerRotateSpritesUp { get; set; } = false;

        public bool ViewerDrawHelp { get; set; } = true;

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
    }
}
