using System;
using System.Collections.Generic;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Scenes;
using SF3.Types;

namespace SF3.Win.App {
    public class AppResources {
        public interface IResource {
            string File { get; }
            string DisplayName { get; }
        }

        public abstract class BaseRegistration : IResource {
            public BaseRegistration(string file, string displayName) {
                File        = file;
                DisplayName = displayName;
            }

            public string File { get; }
            public string DisplayName { get; }
        }

        public class SceneRegistration : BaseRegistration {
            public SceneRegistration(string file, IScene scene)
            : base(file, $"{file} - {scene.SceneName}") {
                Scene = scene;
            }

            public readonly IScene Scene;
        }

        public class CHR_Registration : BaseRegistration {
            public CHR_Registration(string file, ICHR_File chr)
            : base(file, file) {
                CHR  = chr;
            }

            public readonly ICHR_File CHR;
        }

        public class IconCollectionRegistration : BaseRegistration {
            public IconCollectionRegistration(string file, ScenarioType scenario, Table<DAT_FileTextureBase> icons, int spellIconIndex)
            : base($"{file} ({scenario})", $"{file} ({scenario})") {
                Icons          = icons;
                Scenario       = scenario;
                SpellIconIndex = spellIconIndex;
            }

            public readonly Table<DAT_FileTextureBase> Icons;
            public readonly ScenarioType Scenario;
            public readonly int SpellIconIndex;
        }

        private static AppResources _globalAppResources = null;

        public static AppResources Get() {
            if (_globalAppResources == null)
                _globalAppResources = new AppResources();
            return _globalAppResources;
        }

        private AppResources() {}

        public void RegisterScene(string file, IScene scene) {
            ArgumentNullException.ThrowIfNull(scene, nameof(scene));
            var newRegistrartion = new SceneRegistration(file, scene);
            _scenes.Add(newRegistrartion);
            ActiveScene ??= newRegistrartion;
        }

        public void UnregisterScene(IScene scene) {
            ArgumentNullException.ThrowIfNull(scene, nameof(scene));
            var index = _scenes.FindIndex(x => x.Scene == scene);
            if (index == -1)
                return;

            _scenes.RemoveAt(index);
            if (ActiveScene.Scene == scene)
                ActiveScene = (_scenes.Count == 0) ? null : _scenes[0];
        }

        public void RegisterCHR(string file, ICHR_File chr) {
            ArgumentNullException.ThrowIfNull(chr, nameof(chr));
            var newRegistrartion = new CHR_Registration(file, chr);
            _chrs.Add(newRegistrartion);
            ActiveCHR ??= newRegistrartion;
        }

        public void UnregisterCHR(ICHR_File chr) {
            ArgumentNullException.ThrowIfNull(chr, nameof(chr));
            var index = _chrs.FindIndex(x => x.CHR == chr);
            if (index == -1)
                return;

            _chrs.RemoveAt(index);
            if (ActiveCHR.CHR == chr)
                ActiveCHR = (_chrs.Count == 0) ? null : _chrs[0];
        }

        public void RegisterIconCollection(string file, ScenarioType scenario, Table<DAT_FileTextureBase> icons, int spellIconIndex) {
            ArgumentNullException.ThrowIfNull(icons, nameof(icons));
            var newRegistrartion = new IconCollectionRegistration(file, scenario, icons, spellIconIndex);
            _iconCollections.Add(newRegistrartion);
            ActiveIconCollection ??= newRegistrartion;
        }

        public void UnregisterIconCollection(Table<DAT_FileTextureBase> icons) {
            ArgumentNullException.ThrowIfNull(icons, nameof(icons));
            var index = _iconCollections.FindIndex(x => x.Icons == icons);
            if (index == -1)
                return;

            _iconCollections.RemoveAt(index);
            if (ActiveIconCollection.Icons == icons)
                ActiveIconCollection = (_iconCollections.Count == 0) ? null : _iconCollections[0];
        }

        private List<SceneRegistration> _scenes = new List<SceneRegistration>();
        public IEnumerable<SceneRegistration> Scenes => _scenes;

        private SceneRegistration _activeScene = null;
        public SceneRegistration ActiveScene {
            get => _activeScene;
            set {
                if (_activeScene != value) {
                    _activeScene = value;
                    ActiveSceneChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveSceneChanged;

        private List<CHR_Registration> _chrs = new List<CHR_Registration>();
        public IEnumerable<CHR_Registration> CHRs => _chrs;

        private CHR_Registration _activeCHR = null;
        public CHR_Registration ActiveCHR {
            get => _activeCHR;
            set {
                if (_activeCHR != value) {
                    _activeCHR = value;
                    ActiveCHRChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveCHRChanged;

        private List<IconCollectionRegistration> _iconCollections = new List<IconCollectionRegistration>();
        public IEnumerable<IconCollectionRegistration> IconCollections => _iconCollections;

        private IconCollectionRegistration _activeIconCollection = null;
        public IconCollectionRegistration ActiveIconCollection {
            get => _activeIconCollection;
            set {
                if (_activeIconCollection != value) {
                    _activeIconCollection = value;
                    ActiveIconCollectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveIconCollectionChanged;
    }
}
