using System.Linq;
using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationFrameView : SGL_ModelInstanceViewBase<PCAnimationFrameStruct, PCAnimationFrameTable> {
        public PCAnimationFrameView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc)
        : base(name, texCollection, table, ngc, forceLighting: true, size: 1.5f, zoom: 1.25f) {
        }

        public override Control Create() {
            Control ctrl;
            if ((ctrl = base.Create()) != null) {
                ModelView.Control.RenderOptions.DrawWireframe = false;
                ModelView.Control.Pitch = PCModelViewConstants.Pitch;
                ModelView.Control.FrameTick += UpdateModelMatrixTick;
            }
            return ctrl;
        }

        public override void Destroy() {
            if (ModelView?.Control != null)
                ModelView.Control.FrameTick -= UpdateModelMatrixTick;
            base.Destroy();
        }

        protected override void ModelInstanceSetter(SGL_ModelInstance3DView view, ITextureMetaCollection texCollection, PCAnimationFrameStruct model) {
            var polyChar = (PolyChar) texCollection;
            if (polyChar == _lastPolyChar)
                return;
            _lastPolyChar = polyChar;

            SGL_ModelInstance[] instances = [];
            if (Table?.Count >= 1) {
                // TODO: We shouldn't have to do this stupid hack!!!
                var originalInstances = ((PCAnimationFrameTable) Table)[0].ToArray();
                instances = originalInstances
                    .Select(x => {
                        var model = x.GetModel(0);
                        return new SGL_ModelInstance((_, _) => model) {
                            ModelCollectionID = x.ModelCollectionID,
                            ModelID           = x.ModelID,
                            ModelInstanceID   = x.ModelInstanceID
                        };
                    }).ToArray();
            }

            _lastFrameIdx = -1;
            _frame      = 0;
            _frameCount = ((PCAnimationFrameTable) Table).LastOrDefault()?.Frame ?? 0;
            _instances  = instances;
            ModelView.Control.Update(texCollection, _instances);

            UpdateModelMatrix();
        }

        private void UpdateModelMatrixTick(object sender, float delta) {
            _frame += delta * 15.0f / 1000.0f;
            _frame %= _frameCount;
            UpdateModelMatrix();
        }

        private void UpdateModelMatrix() {
            // TODO: big dumb hack! boo this table!!!
            int frameIdx = (int) (_frame * 2.0f);
            if (_lastFrameIdx != frameIdx) {
                _lastFrameIdx = frameIdx;
                if (frameIdx < Table.Count) {
                    int idx = 0;
                    foreach (var tableInst in ((PCAnimationFrameTable) Table)[frameIdx])
                        _instances[idx++].Matrix = tableInst.Matrix;
                }
            }
        }

        private float _frame = 0;
        private float _frameCount = 0;
        private int _lastFrameIdx = -1;
        private PolyChar _lastPolyChar = null;
        private SGL_ModelInstance[] _instances = [];
    }
}
