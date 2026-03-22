using System;
using System.Collections.Generic;

namespace CommonLib {
    public class DisposableEventHandlerCollection<T> : IDisposable where T : Delegate {
        public DisposableEventHandlerCollection(object publisher) {
            Publisher = publisher;
            _publisherType = Publisher.GetType();
        }

        public void Subscribe(string eventName, T eventHandler) {
            var eventInfo = _publisherType.GetEvent(eventName);
            if (eventInfo == null)
                return;

            eventInfo.AddEventHandler(Publisher, eventHandler);
            _unsubscribeActions.Add(() => eventInfo.RemoveEventHandler(Publisher, eventHandler));
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    foreach (var action in _unsubscribeActions)
                        action();
                    _unsubscribeActions.Clear();
                    Publisher = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public object Publisher { get; private set; }

        private bool _disposedValue;
        private readonly List<Action> _unsubscribeActions = new List<Action>();
        private readonly Type _publisherType;
    }
}
