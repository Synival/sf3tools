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

        public static bool Initialized() => _globalAppState != null;

        public static AppState RetrieveAppState() {
            if (_globalAppState == null)
                throw new InvalidOperationException("AppState not initialized");
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

        public static string GetFileFullPath(string appName)
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SF3Tools", appName + " State.json");

        public static AppState Deserialize(string appName) {
            var fullPath = GetFileFullPath(appName);
            if (!File.Exists(fullPath))
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
            if (!File.Exists(FileFullPath))
                Directory.CreateDirectory(Path.GetDirectoryName(FileFullPath));
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FileFullPath, text);
        }

        [JsonIgnore]
        public string AppName { get; private set; }

        [JsonIgnore]
        public string FileFullPath { get; private set; }

        public bool ViewerDrawSurfaceModel { get; set; } = true;
        public bool ViewerDrawModels { get; set; } = true;
        public bool ViewerDrawGround { get; set; } = true;
        public bool ViewerDrawSkyBox { get; set; } = true;
        public bool ViewerRunAnimations { get; set; } = true;
        public bool ViewerApplyLighting { get; set; } = true;
        public bool ViewerDrawGradients { get; set; } = true;

        public bool ViewerDrawWireframe { get; set; } = false;
        public bool ViewerDrawBoundaries { get; set; } = false;
        public bool ViewerDrawTerrainTypes { get; set; } = false;
        public bool ViewerDrawEventIDs { get; set; } = false;
        public bool ViewerDrawNormals { get; set; } = false;

        public bool ViewerRotateSpritesUp { get; set; } = false;

        public bool ViewerDrawHelp { get; set; } = true;

        public int ViewerCursorMode { get; set; } = 0;
    }
}
