using System;

namespace SF3.Win.OpenGL.MPD_File {
    public abstract class ResourcesBase : IResources {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing) {
                Reset();
                DeInit();
            }
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ResourcesBase() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;
            PerformInit();
        }

        protected abstract void PerformInit();

        public abstract void DeInit();
        public abstract void Reset();
    }
}
