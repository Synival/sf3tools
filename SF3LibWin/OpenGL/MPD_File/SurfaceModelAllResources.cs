using System;
using SF3.Models.Files.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelAllResources : IDisposable {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        public SurfaceModelAllResources() {
            Blocks = [
                // TODO: all the blocks!
                new SurfaceModelBlockResources(0)
            ];
        }

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            foreach (var block in Blocks)
                block.Init();
        }

        public void Reset() {
            foreach (var block in Blocks)
                block.Reset();
        }

        public void Update(IMPD_File mpdFile) {
            foreach (var block in Blocks)
                block.Update(mpdFile);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                foreach (var block in Blocks)
                    block.Dispose();
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SurfaceModelAllResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public SurfaceModelBlockResources[] Blocks { get; }
    }
}
