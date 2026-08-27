using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.SGL;
using Newtonsoft.Json.Linq;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_ModelCollection : IMPD_ModelCollection, IDisposable {
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
                ModelInstances = new MPD_ModelCollectionModelInstances(original.ModelInstances, this);
            if (original.Textures != null)
                Textures = new MPD_ModelCollectionTextures(original.Textures);
            if (original.DataAfterInstances != null)
                DataAfterInstances = original.DataAfterInstances.ToArray();
        }

        public static IMPD_ModelCollection FromJToken(JToken token, MPD_CollectionType collection, IPalette indexedTexturePalette)
            => new MPD_ModelCollection(token, collection, indexedTexturePalette);
        public MPD_ModelCollection(JToken token, MPD_CollectionType collection, IPalette indexedTexturePalette) {
            Collection = collection;

            var jObject = (JObject) token;

            Models             = jObject.GetValueIfExists("Models",             t => t.Select(x => (IMPD_Model) MPD_Model.FromJToken(x, collection)).ToArray());
            ModelInstances     = jObject.GetValueIfExists("ModelInstances",     t => t.Select(x => (IMPD_ModelInstance) MPD_ModelInstance.FromJToken(x, this)).ToArray());
            Textures           = jObject.GetValueIfExists("Textures",           t => t.Select(x => (IMPD_AnimatableTexture) MPD_AnimatableTexture.FromJToken(x, collection, indexedTexturePalette)).ToArray());
            DataAfterInstances = jObject.GetValueIfExists("DataAfterInstances", t => t.Select(x => (byte) x).ToArray());

            if (Collection.IsHeaderModelCollection())
                IsUnreferenced = (bool) jObject["IsUnreferenced"];
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    if (Textures != null)
                        foreach (var tex in Textures)
                            (tex as IDisposable)?.Dispose();

                _disposedValue = true;
            }
        }

        ISGL_Model ISGL_ModelCollection.GetModel(int id, int lod) => GetModel(id, lod);
        public IMPD_ModelLoD GetModel(int id, int lod) {
            var model = Models.FirstOrDefault(x => x.ModelID == id);
            if (model == null || lod < 0 || lod >= model.LevelsOfDetail)
                return null;
            return model.ModelLoDs[lod];
        }

        ISGL_Model[] ISGL_ModelCollection.GetAllModels() => GetAllModels();
        public IMPD_ModelLoD[] GetAllModels()
            => Models.SelectMany(x => x.ModelLoDs).ToArray();

        public MPD_CollectionType Collection { get; }
        public bool IsUnreferenced { get; set; }
        public bool HasMissingModels => Models == null;

        public IReadOnlyList<IMPD_ModelInstance> ModelInstances { get; }
        public IReadOnlyList<IMPD_AnimatableTexture> Textures { get; }
        public IReadOnlyList<byte> DataAfterInstances { get; }
        public IReadOnlyList<IMPD_Model> Models { get; }

        private bool _disposedValue;
    }
}
