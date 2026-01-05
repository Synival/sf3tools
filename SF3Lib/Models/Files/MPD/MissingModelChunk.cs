using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.SGL;
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

        public IEnumerable<ISGL_Model> Models => new ISGL_Model[0];

        public IEnumerable<IMPD_ModelInstance> ModelInstances => new IMPD_ModelInstance[0];

        public ISGL_Model GetModel(int id) => null;

        private IMPD_Texture[] _textures = null;
        public IEnumerable<IMPD_Texture> Textures {
            get {
                if (_textures == null) {
                    _textures = MPD_File.TextureChunks
                        .Where(x => x.Collection == Collection)
                        .Select(x => x.TextureTable)
                        .SelectMany(x => x)
                        .ToArray();
                }
                return _textures;
            }
        }

        public IMPD_File MPD_File { get; }
        public bool IsUnreferenced { get; set; } = true;
    }
}
