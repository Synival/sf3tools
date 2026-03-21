using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_ModelCollectionTextures : IEnumerableWithLength<IMPD_AnimatableTexture> {
        public MPD_ModelCollectionTextures() {
            _textures = new List<MPD_AnimatableTexture>();
        }

        public MPD_ModelCollectionTextures(IEnumerableWithLength<IMPD_AnimatableTexture> original) {
            _textures = original.Select(x => new MPD_AnimatableTexture(x)).ToList();
        }

        private List<MPD_AnimatableTexture> _textures;

        public int Count => _textures.Count;
        public IEnumerator<IMPD_AnimatableTexture> GetEnumerator() => _textures.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _textures.GetEnumerator();
    }
}
