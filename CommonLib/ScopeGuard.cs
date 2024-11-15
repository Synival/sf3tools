using System;

namespace CommonLib {
    public class ScopeGuard : IDisposable {
        public delegate void InitDelegate();
        public delegate void DisposeDelegate();

        public ScopeGuard(InitDelegate init, DisposeDelegate dispose) {
            if (init != null)
                init();
            _dispose = dispose;
        }

        ~ScopeGuard() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;

            if (disposing) {
                if (_dispose != null) {
                    _dispose();
                    _dispose = null;
                }
            }

            _disposed = true;
        }

        private DisposeDelegate _dispose;
        private bool _disposed = false;
    }
}