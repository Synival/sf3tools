using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.SGL;

namespace CommonLib.TexturePacker {
    public class ModelTexturePackerStrategy : TightTexturePackerStrategy {
        public ModelTexturePackerStrategy(ISGL_Model model, IReadOnlyDictionary<int, IAnimatableTexture> texCollection, int padding = 0)
        : base(textures: GetTextures(model, texCollection), padding: padding)
        {}

        private static IAnimatableTexture[] GetTextures(ISGL_Model model, IReadOnlyDictionary<int, IAnimatableTexture> texCollection) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (texCollection == null)
                throw new ArgumentNullException(nameof(texCollection));

            var allTextures = model.Faces
                .Where(x => x.Attributes.UseTexture)
                .Select(x => x.Attributes.TextureNo)
                .Distinct()
                .Select(x => texCollection.TryGetValue(x, out var texture) ? texture : null)
                .Where(x => x != null)
                .ToArray();

            return allTextures;
        }
    }
}
