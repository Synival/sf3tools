using System.Collections.Generic;
using System.Linq;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MissingModelChunk : IMPD_ModelCollection {
        public MissingModelChunk(IMPD_File mpdFile, MPD_CollectionType collection) {
            MPD_File = mpdFile;
            Collection = collection;
        }

        public MPD_CollectionType Collection { get; }

        public IReadOnlyList<IMPD_Model> Models => new IMPD_Model[0];

        public IReadOnlyList<IMPD_ModelInstance> ModelInstances => new IMPD_ModelInstance[0];

        public IMPD_ModelLoD GetModel(int id, int lod) => null;

        private bool _gotTextures = false;
        private IReadOnlyList<IMPD_AnimatableTexture> _textures = null;
        public IReadOnlyList<IMPD_AnimatableTexture> Textures {
            get {
                if (!_gotTextures) {
                    var textureChunks = MPD_File.TextureChunks
                        .Where(x => x.Collection == Collection)
                        .ToArray();

                    if (textureChunks.Length > 0) {
                        _textures = textureChunks
                            .SelectMany(x => x.TextureTable.Rows)
                            .Cast<IMPD_AnimatableTexture>()
                            .ToArray();
                    }
                    _gotTextures = true;
                }
                return _textures;
            }
        }

        public IReadOnlyList<byte> DataAfterInstances => null;

        public IMPD_File MPD_File { get; }
        public bool IsUnreferenced { get; set; } = true;
        public bool HasMissingModels => true;
    }
}
