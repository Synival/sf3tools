using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using SF3.Models.Structs.X8PC;
using SF3.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationView : SGL_ModelInstance3DView {
        public PCAnimationView(string name, PolyChar polyChar)
        : base(name, polyChar, sglModelInstance: null, forceLighting: true) {
            _polyChar = polyChar;
            UpdateModelInstances();
        }

        public override Control Create() {
            Control ctrl;
            if ((ctrl = base.Create()) != null) {
                Control.MinimumSize = new System.Drawing.Size(0, 0);
                Control.MaximumSize = new System.Drawing.Size(0, 0);
                Control.Dock = DockStyle.Fill;
                Control.RenderOptions.DrawWireframe = false;
                Control.Pitch = PCModelViewConstants.Pitch;
                Control.FrameTick += OnFrameTick;
                Control.Update(_polyChar, _instances);
            }
            return ctrl;
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            if (Control != null)
                Control.FrameTick -= OnFrameTick;

            if (Control.IsHandleCreated)
                base.Destroy();
        }

        private void UpdateModelInstances() {
            var instanceId = 0;

            (IBone Bone, SGL_ModelInstance Instance)[] instsWithBones = [];
            if (_polyChar?.Skeleton?.RootBone != null) {
                instsWithBones = _polyChar.Skeleton.RootBone.Flatten()
                    .Where(x => x.ModelID.HasValue || ((x.Tag == 0x30 || x.Tag == 0x81) && _polyChar.WeaponXPData != null))
                    .Select((x, i) => {
                        var model = _polyChar.GetModel(x.ModelID ?? _polyChar.WeaponXPData.ModelID, 0);
                        return (Bone: x, Instance: new SGL_ModelInstance((_, _) => model) {
                            ModelCollectionID = model.ModelCollectionID,
                            ModelID           = model.ModelID,
                            ModelInstanceID   = instanceId++
                        });
                    }).ToArray();
            }

            _instBones = new IBone[instsWithBones.Length];
            _instances = new SGL_ModelInstance[instsWithBones.Length];
            for (int i = 0; i < instsWithBones.Length; i++) {
                _instances[i] = instsWithBones[i].Instance;
                _instBones[i] = instsWithBones[i].Bone;
            }

            _lastFrameIdx = -1;
            _frame = 0;

            if (PolyChar != null) {
                _maxFrame = Math.Max(
                    PolyChar.BoneKeyframePosTables.Max(x => x.Max(y => y.Frame)),
                    Math.Max(
                        PolyChar.BoneKeyframeRotTables.Max(x => x.Max(y => y.Frame)),
                        PolyChar.BoneKeyframeScaleTables.Max(x => x.Max(y => y.Frame))
                    )
                );
            }

            if (Control != null)
                Control.Update(_polyChar, _instances);

            UpdateModelMatrix();
        }

        private void OnFrameTick(object sender, float delta) {
            Control.Zoom = Math.Min(Control.Width / (float) Control.Height, Control.Height / (float) Control.Width) * 1.25f;

            _frame += delta * 15.0f / 1000.0f;
            _frame %= _maxFrame;
            UpdateModelMatrix();
        }

        private void UpdateModelMatrix() {
            int frameIdx = (int) (_frame * 4.0f);
            if (_lastFrameIdx != frameIdx) {
                _lastFrameIdx = frameIdx;
                for (int i = 0; i < _instances.Length; i++) {
                    var inst = _instances[i];
                    inst.Matrix = _polyChar.GetModelInstanceMatrixInAnimation(inst, _instBones[i], _frame);
                }
            }
        }

        private PolyChar _polyChar = null;
        public PolyChar PolyChar {
            get => _polyChar;
            set {
                if (_polyChar != value) {
                    _polyChar = value;
                    UpdateModelInstances();
                }
            }
        }

        private float _frame = 0;
        private float _maxFrame = 0;
        private int _lastFrameIdx = -1;

        private SGL_ModelInstance[] _instances = [];
        private IBone[] _instBones = [];
    }
}
