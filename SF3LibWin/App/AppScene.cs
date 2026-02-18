using System;
using System.Collections.Generic;
using SF3.Actors;
using SF3.Models.Files.CHR;

namespace SF3.Win.App {
    public class AppScene {
        public interface IResource {
            string DisplayName { get; }
        }

        public class ActorCollectionRegistration : IResource {
            public ActorCollectionRegistration(string file, IActorCollection actors) {
                File   = file;
                Actors = actors;
                DisplayName = $"{File} - {Actors.ActorCollectionName}";
            }

            public readonly string File;
            public readonly IActorCollection Actors;
            public string DisplayName { get; }
        }

        public class CHR_Registration : IResource {
            public CHR_Registration(string file, ICHR_File chr) {
                File = file;
                CHR  = chr;
            }

            public readonly string File;
            public readonly ICHR_File CHR;
            public string DisplayName => File;
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
    }
}
