using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class SGL_ModelInstanceTableView<TStruct, TTable> : SGL_ModelInstanceViewBase<TStruct, TTable>
    where TStruct : ISGL_ModelInstance
    where TTable : ITable {
        public SGL_ModelInstanceTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc, bool? forceLighting = null, float size = 1.0f, float zoom = 1.0f)
        : base(name, texCollection, table, ngc, forceLighting, size, zoom) {
        }

        protected override void ModelInstanceSetter(SGL_ModelInstance3DView view, ITextureMetaCollection texCollection, TStruct model)
            => view.SetModelInstance(texCollection, model);
    }
}
