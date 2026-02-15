using System;
using System.Collections.Generic;
using SF3.Actors;

namespace SF3.Win.App {
    public class AppResources {
        private static AppResources _globalAppResources = null;

        public static AppResources RetrieveAppState() {
            if (_globalAppResources == null)
                _globalAppResources = new AppResources();
            return _globalAppResources;
        }

        private AppResources() {}

        public void RegisterActorCollection(IActorCollection collection) {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            _actorCollection.Add(collection);
            ActiveActorCollection ??= collection;
        }

        public void UnregisterActorCollection(IActorCollection collection) {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            if (_actorCollection.Remove(collection) && ActiveActorCollection == collection)
                ActiveActorCollection = (_actorCollection.Count == 0) ? null : _actorCollection[0];
        }

        private List<IActorCollection> _actorCollection = new List<IActorCollection>();
        public IActorCollection ActiveActorCollection { get; private set; }
    }
}
