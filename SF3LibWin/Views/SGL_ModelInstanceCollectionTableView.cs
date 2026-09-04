using System.Linq;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class SGL_ModelInstanceCollectionTableView<TStruct, TTable> : SGL_ModelInstanceViewBase<TStruct, TTable>
    where TStruct : ISGL_ModelInstanceCollection
    where TTable : ITable {
        public SGL_ModelInstanceCollectionTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc, bool? forceLighting = null, float size = 1.0f, float zoom = 1.0f)
        : base(name, texCollection, table, ngc, forceLighting, size, zoom) {
        }

        protected override void ModelInstanceSetter(SGL_ModelInstance3DView view, ITextureMetaCollection texCollection, TStruct model)
            => view.SetModelInstances(texCollection, model?.ToArray());
    }
}
