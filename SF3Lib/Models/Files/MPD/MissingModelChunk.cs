using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using SF3.Imaging;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class MissingModelChunk : IMPD_ModelCollection {
        public MissingModelChunk(IMPD_File mpdFile, MPD_CollectionType collection) {
            MPD_File = mpdFile;
            Collection = collection;
        }

        public MPD_CollectionType Collection { get; }

        public IEnumerableWithLength<IMPD_Model> Models => new IMPD_Model[0].ToEnumerableWithLength();

        public IEnumerableWithLength<IMPD_ModelInstance> ModelInstances => new IMPD_ModelInstance[0].ToEnumerableWithLength();

        public IMPD_Model GetModel(int id) => null;

        private IEnumerableWithLength<IMPD_AnimatableTexture> _textures = null;
        public IEnumerableWithLength<IMPD_AnimatableTexture> Textures {
            get {
                if (_textures == null) {
                    _textures = MPD_File.TextureChunks
                        .Where(x => x.Collection == Collection)
                        .Select(x => x.TextureTable)
                        .SelectMany(x => x)
                        .Cast<IMPD_AnimatableTexture>()
                        .ToArray()
                        .ToEnumerableWithLength();
                }
                return _textures;
            }
        }

        public IMPD_File MPD_File { get; }
        public bool IsUnreferenced { get; set; } = true;
        public bool HasMissingModels => true;
    }
}
