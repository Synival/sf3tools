using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.Types;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Texture : InMemoryTextureData, IMPD_Texture, IDisposable {
        public MPD_Texture(IMPD_Texture original) : base(original, null, ImageDataCanSet.CanSet8Or16Bit, IndexedColorUpdateStrategy.MatchToExistingPalette) {
            TextureID  = original.TextureID;
            Collection = original.Collection;
            IsIgnored  = original.IsIgnored;

            if (original.Tags != null) {
                Tags = original.Tags
                    .Select(x => new KeyValuePair<TagKey, TagValue>(new TagKey(x.Key), new TagValue(x.Value)))
                    .ToDictionary(x => x.Key, x => x.Value);
            }
        }

        protected MPD_Texture(JObject jObject, MPD_CollectionType collection, IPalette palette)
        : base(jObject, zeroIsTransparent: true, palette, ImageDataCanSet.CanSet8Or16Bit, IndexedColorUpdateStrategy.MatchToExistingPalette) {
            TextureID  = (int) jObject["ID"];
            Collection = collection;

            // 'IsIgnored' is only serialized for the primary collection.
            if (collection == MPD_CollectionType.Primary)
                IsIgnored  = (bool) jObject["IsIgnored"];

            // Tags are not serialized.
        }

        protected MPD_Texture(JToken token, IMPD_Texture texture)
        : base(token, texture.Width, texture.Height, texture.PixelFormat, true, texture.Palette, ImageDataCanSet.CanSet8Or16Bit, IndexedColorUpdateStrategy.MatchToExistingPalette) {
            TextureID  = texture.TextureID;
            Collection = texture.Collection;
            IsIgnored  = texture.IsIgnored;

            // Tags are not serialized.
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                OnDispose(disposing);
                _disposedValue = true;
            }
        }

        protected virtual void OnDispose(bool disposing) {}

        public void Dispose() {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        public int TextureCollectionID => (int) Collection;
        public int TextureID { get; }
        public MPD_CollectionType Collection { get; }
        public bool IsIgnored { get; set; }

        public Dictionary<TagKey, TagValue> Tags { get; }

        private bool _disposedValue;
    }
}
