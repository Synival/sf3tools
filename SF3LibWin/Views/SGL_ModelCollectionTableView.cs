using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class SGL_ModelCollectionTableView<TStruct, TTable> : SGL_ModelTableViewBase<TStruct, TTable>
    where TStruct : ISGL_ModelCollection
    where TTable : ITable {
        public SGL_ModelCollectionTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc, bool? forceLighting = null, float size = 1.0f)
        : base(name, texCollection, table, ngc, forceLighting, size) {
        }

        protected override void ModelSetter(SGL_Model3DView view, ITextureMetaCollection texCollection, TStruct model)
            => view.SetModels(texCollection, model?.GetAllModels());
    }
}
