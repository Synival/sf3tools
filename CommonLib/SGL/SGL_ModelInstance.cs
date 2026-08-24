using System;
using Newtonsoft.Json.Linq;

namespace CommonLib.SGL {
    public class SGL_ModelInstance : SGL_ModelInstanceBase {
        public SGL_ModelInstance(Func<SGL_ModelInstance, int /*lod*/, ISGL_Model> modelGetter) : base() {
            ModelGetter = modelGetter;
        }

        public SGL_ModelInstance(ISGL_ModelInstance original, Func<SGL_ModelInstance, int /*lod*/, ISGL_Model> modelGetter) : base(original) {
            ModelGetter = modelGetter;
        }

        public static SGL_ModelInstance FromJToken(JToken token, Func<SGL_ModelInstance, int /*lod*/, ISGL_Model> modelGetter) => new SGL_ModelInstance(token, modelGetter);
        protected SGL_ModelInstance(JToken token, Func<SGL_ModelInstance, int /*lod*/, ISGL_Model> modelGetter) : base(token) {
            ModelGetter = modelGetter;
        }

        public override ISGL_Model GetModel(int lod)
            => ModelGetter?.Invoke(this, lod);

        public Func<SGL_ModelInstance, int /*lod*/, ISGL_Model> ModelGetter { get; set; }
    }
}
