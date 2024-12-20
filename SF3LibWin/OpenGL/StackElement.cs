using System;

namespace SF3.Win.OpenGL {
    public class StackElement : IDisposable {
        public StackElement() {
            _disposalAction = null;
        }

        public StackElement(Action init, Action disposal) {
            init?.Invoke();
            _disposalAction = disposal;
        }

        private bool _disposed;
        private readonly Action _disposalAction;

        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;
            if (disposing)
                _disposalAction?.Invoke();
            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StackElement() {
            if (!_disposed)
                System.Diagnostics.Debug.WriteLine("StackElement: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }
    }
}
