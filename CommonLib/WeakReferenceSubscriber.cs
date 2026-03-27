using System;
using CommonLib.Extensions;

namespace CommonLib {
    public class WeakReferenceSubscriber<TArgs, TEventHandler> where TEventHandler : Delegate {
        public WeakReferenceSubscriber(object publisher) {
            Publisher = publisher;
            _publisherType = Publisher.GetType();
        }

        public void Subscribe(string eventName, Action<object, TArgs> eventHandler)
            => Publisher.SubscribeWithWeakReference<TArgs, TEventHandler>(eventName, eventHandler, _publisherType);

        public object Publisher { get; private set; }

        private readonly Type _publisherType;
    }
}
