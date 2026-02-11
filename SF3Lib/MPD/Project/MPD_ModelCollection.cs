using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelCollection : IMPD_ModelCollection {
        public MPD_ModelCollection(MPD_CollectionType collection) {
            Collection = collection;
            IsUnreferenced = false;

            Models         = new MPD_ModelCollectionModels();
            ModelInstances = new MPD_ModelCollectionModelInstances();
            Textures       = new MPD_ModelCollectionTextures();
        }

        public MPD_ModelCollection(IMPD_ModelCollection original) {
            Collection = original.Collection;
            IsUnreferenced = original.IsUnreferenced;

            if (original.Models != null)
                Models = new MPD_ModelCollectionModels(original.Models);
            if (original.ModelInstances != null)
                ModelInstances = new MPD_ModelCollectionModelInstances(original.ModelInstances);
            if (original.Textures != null)
                Textures = new MPD_ModelCollectionTextures(original.Textures);
            if (original.DataAfterInstances != null)
                DataAfterInstances = ((byte[]) (original.DataAfterInstances.AsArray().Clone())).ToEnumerableWithLength();
        }

        public static IMPD_ModelCollection FromJToken(JToken token, MPD_CollectionType collection, Palette indexedTexturePalette)
            => new MPD_ModelCollection(token, collection, indexedTexturePalette);
        public MPD_ModelCollection(JToken token, MPD_CollectionType collection, Palette indexedTexturePalette) {
            Collection = collection;

            var jObject = (JObject) token;

            Models             = jObject.GetValueIfExists("Models",             t => t.Select(x => (IMPD_Model) MPD_Model.FromJToken(x, collection)).ToArray().ToEnumerableWithLength());
            ModelInstances     = jObject.GetValueIfExists("ModelInstances",     t => t.Select(x => (IMPD_ModelInstance) MPD_ModelInstance.FromJToken(x, collection)).ToArray().ToEnumerableWithLength());
            Textures           = jObject.GetValueIfExists("Textures",           t => t.Select(x => (IMPD_AnimatableTexture) MPD_AnimatableTexture.FromJToken(x, collection, indexedTexturePalette)).ToArray().ToEnumerableWithLength());
            DataAfterInstances = jObject.GetValueIfExists("DataAfterInstances", t => t.Select(x => (byte) x).ToArray().ToEnumerableWithLength());

            if (Collection.IsHeaderModelCollection())
                IsUnreferenced = (bool) jObject["IsUnreferenced"];
        }

        public IMPD_ModelLoD GetModel(int id, int lod) {
            var model = Models.FirstOrDefault(x => x.ModelID == id);
            if (model == null || lod < 0 || lod >= model.LevelsOfDetail)
                return null;
            return model.ModelLoDs[lod];
        }

        public MPD_CollectionType Collection { get; }
        public bool IsUnreferenced { get; set; }
        public bool HasMissingModels => Models == null;

        public IEnumerableWithLength<IMPD_ModelInstance> ModelInstances { get; }
        public IEnumerableWithLength<IMPD_AnimatableTexture> Textures { get; }
        public IIndexedEnumerableWithLength<byte> DataAfterInstances { get; }
        public IEnumerableWithLength<IMPD_Model> Models { get; }
    }
}
