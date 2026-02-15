using System;
using System.Collections.Generic;
using SF3.Actors;

namespace SF3.Win.App {
    public class AppResources {
        private static AppResources _globalAppResources = null;

        public static AppResources Get() {
            if (_globalAppResources == null)
                _globalAppResources = new AppResources();
            return _globalAppResources;
        }

        private AppResources() {}

        public void RegisterActorCollection(IActorCollection collection) {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            _actorCollections.Add(collection);
            ActiveActorCollection ??= collection;
        }

        public void UnregisterActorCollection(IActorCollection collection) {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            if (_actorCollections.Remove(collection) && ActiveActorCollection == collection)
                ActiveActorCollection = (_actorCollections.Count == 0) ? null : _actorCollections[0];
        }

        private List<IActorCollection> _actorCollections = new List<IActorCollection>();
        public IEnumerable<IActorCollection> ActorCollections => _actorCollections;
        public IActorCollection ActiveActorCollection { get; set; }
    }
}
