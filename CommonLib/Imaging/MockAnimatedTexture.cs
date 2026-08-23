using System;

namespace CommonLib.Imaging {
    public class MockAnimatedTexture : IAnimatedTexture, IDisposable {
        public MockAnimatedTexture(ITexture texture) {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));
            Frames = new IAnimatedTextureFrame[] { new MockAnimatedTextureFrame(texture) };
        }

        protected MockAnimatedTexture(IAnimatedTextureFrame[] frames) {
            if (frames == null)
                throw new ArgumentNullException(nameof(frames));
            Frames = frames;
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    (Frames[0] as IDisposable)?.Dispose();
                _disposedValue = true;
            }
        }

        public IAnimatedTextureFrame GetFrame(int timeFrame) => Frames[0];

        public int TextureCollectionID => Frames[0].TextureCollectionID;
        public int TextureID => Frames[0].TextureID;
        public int FrameTimerStart => 0;
        public IAnimatedTextureFrame[] Frames { get; }

        private bool _disposedValue;
    }
}
