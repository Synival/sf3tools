using System;
using System.Collections.Generic;
using SF3.Actors;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Win.App {
    public class AppScene {
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

        public class ActorCollectionRegistration : BaseRegistration {
            public ActorCollectionRegistration(string file, IActorCollection actors)
            : base(file, $"{file} - {actors.ActorCollectionName}") {
                Actors = actors;
            }

            public readonly IActorCollection Actors;
        }

        public class CHR_Registration : BaseRegistration {
            public CHR_Registration(string file, ICHR_File chr)
            : base(file, file) {
                CHR  = chr;
            }

            public readonly ICHR_File CHR;
        }

        public class IconCollectionRegistration : BaseRegistration {
            public IconCollectionRegistration(string file, ScenarioType scenario, Table<FixedSizeTextureStructBase> icons)
            : base($"{file} ({scenario})", $"{file} ({scenario})") {
                Icons = icons;
                Scenario = scenario;
            }

            public readonly Table<FixedSizeTextureStructBase> Icons;
            public readonly ScenarioType Scenario;
        }

        private static AppScene _globalAppResources = null;

        public static AppScene Get() {
            if (_globalAppResources == null)
                _globalAppResources = new AppScene();
            return _globalAppResources;
        }

        private AppScene() {}

        public void RegisterActorCollection(string file, IActorCollection actors) {
            ArgumentNullException.ThrowIfNull(actors, nameof(actors));
            var newRegistrartion = new ActorCollectionRegistration(file, actors);
            _actorCollections.Add(newRegistrartion);
            ActiveActorCollection ??= newRegistrartion;
        }

        public void UnregisterActorCollection(IActorCollection actors) {
            ArgumentNullException.ThrowIfNull(actors, nameof(actors));
            var index = _actorCollections.FindIndex(x => x.Actors == actors);
            if (index == -1)
                return;

            _actorCollections.RemoveAt(index);
            if (ActiveActorCollection.Actors == actors)
                ActiveActorCollection = (_actorCollections.Count == 0) ? null : _actorCollections[0];
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

        public void RegisterIconCollection(string file, ScenarioType scenario, Table<FixedSizeTextureStructBase> icons) {
            ArgumentNullException.ThrowIfNull(icons, nameof(icons));
            var newRegistrartion = new IconCollectionRegistration(file, scenario, icons);
            _iconCollections.Add(newRegistrartion);
            ActiveIconCollection ??= newRegistrartion;
        }

        public void UnregisterIconCollection(Table<FixedSizeTextureStructBase> icons) {
            ArgumentNullException.ThrowIfNull(icons, nameof(icons));
            var index = _iconCollections.FindIndex(x => x.Icons == icons);
            if (index == -1)
                return;

            _iconCollections.RemoveAt(index);
            if (ActiveIconCollection.Icons == icons)
                ActiveIconCollection = (_iconCollections.Count == 0) ? null : _iconCollections[0];
        }

        private List<ActorCollectionRegistration> _actorCollections = new List<ActorCollectionRegistration>();
        public IEnumerable<ActorCollectionRegistration> ActorCollections => _actorCollections;

        private ActorCollectionRegistration _activeActorCollection = null;
        public ActorCollectionRegistration ActiveActorCollection {
            get => _activeActorCollection;
            set {
                if (_activeActorCollection != value) {
                    _activeActorCollection = value;
                    ActiveActorCollectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler ActiveActorCollectionChanged;

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
