using System;
using System.Collections.Generic;
using SF3.Actors;

namespace SF3.Win.App {
    public class AppResources {
        public class ActorCollectionRegistration {
            public ActorCollectionRegistration(string file, IActorCollection actors) {
                File   = file;
                Actors = actors;
            }

            public readonly string File;
            public readonly IActorCollection Actors;
        }

        private static AppResources _globalAppResources = null;

        public static AppResources Get() {
            if (_globalAppResources == null)
                _globalAppResources = new AppResources();
            return _globalAppResources;
        }

        private AppResources() {}

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

        private List<ActorCollectionRegistration> _actorCollections = new List<ActorCollectionRegistration>();
        public IEnumerable<ActorCollectionRegistration> ActorCollections => _actorCollections;
        public ActorCollectionRegistration ActiveActorCollection { get; set; }
    }
}
