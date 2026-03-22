using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_ModelCollectionTextures : IReadOnlyList<IMPD_AnimatableTexture>, IDisposable {
        public MPD_ModelCollectionTextures() {
            _textures = new List<MPD_AnimatableTexture>();
        }

        public MPD_ModelCollectionTextures(IReadOnlyList<IMPD_AnimatableTexture> original) {
            _textures = original.Select(x => new MPD_AnimatableTexture(x)).ToList();
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    foreach (var texture in _textures)
                        texture.Dispose();
                _textures.Clear();
                _disposedValue = true;
            }
        }

        private List<MPD_AnimatableTexture> _textures;
        private bool _disposedValue;

        public int Count => _textures.Count;
        public IMPD_AnimatableTexture this[int index] => _textures[index];
        public IEnumerator<IMPD_AnimatableTexture> GetEnumerator() => _textures.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _textures.GetEnumerator();
    }
}
