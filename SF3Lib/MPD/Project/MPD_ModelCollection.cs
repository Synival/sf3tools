using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CommonLib;
using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelCollection : IMPD_ModelCollection {
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

        public static IMPD_ModelCollection FromJToken(JToken token, MPD_CollectionType collection)
            => new MPD_ModelCollection(token, collection);
        public MPD_ModelCollection(JToken token, MPD_CollectionType collection) {
            Collection = collection;

            var jObject = (JObject) token;

            Models             = jObject.GetValueIfExists("Models",             t => new IMPD_Model[0].ToEnumerableWithLength());
            ModelInstances     = jObject.GetValueIfExists("ModelInstances",     t => new IMPD_ModelInstance[0].ToEnumerableWithLength());
            Textures           = jObject.GetValueIfExists("Textures",           t => new IMPD_AnimatableTexture[0].ToEnumerableWithLength());
            DataAfterInstances = jObject.GetValueIfExists("DataAfterInstances", t => t.Select(x => (byte) x).ToArray().ToEnumerableWithLength());

            if (Collection.IsHeaderModelCollection())
                IsUnreferenced = (bool) jObject["IsUnreferenced"];
        }

        public IMPD_Model GetModel(int id, int lod) => Models?.FirstOrDefault(x => x.ModelID == id && x.LevelOfDetail == lod);

        public MPD_CollectionType Collection { get; }
        public bool IsUnreferenced { get; set; }
        public bool HasMissingModels => Models == null;

        public IEnumerableWithLength<IMPD_Model> Models { get; }
        public IEnumerableWithLength<IMPD_ModelInstance> ModelInstances { get; }
        public IEnumerableWithLength<IMPD_AnimatableTexture> Textures { get; }
        public IIndexedEnumerableWithLength<byte> DataAfterInstances { get; }
    }
}
