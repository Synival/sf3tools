using System;
using CommonLib.Utils;
using SF3.Models.Files.MPD;

namespace SF3.Win.OpenGL.MPD_File {
    public class SurfaceModelResources : IDisposable {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;

        public SurfaceModelResources() {
            var numBlocks = 16 * 16;
            Blocks = new SurfaceModelBlockResources[numBlocks];
            for (var i = 0; i < numBlocks; i++)
                Blocks[i] = new SurfaceModelBlockResources(i);
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

        ~SurfaceModelResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public SurfaceModelBlockResources[] Blocks { get; }
    }
}
