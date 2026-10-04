using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using CommonLib.Extensions;
using SF3.Models.Structs.X8PC;
using CommonLib.Rigging;
using System.Collections.Generic;
using CommonLib.Imaging;
using OpenTK.Mathematics;

namespace SF3.Win.Views.X8PC {
    public class PCAnimationView : SGL_ModelInstance3DView {
        public PCAnimationView(string name, PolyChar polyChar, PCAnimationDefStruct animation)
        : base(name, polyChar, sglModelInstance: null, forceLighting: true) {
            _polyChar  = polyChar;
            _animation = animation;
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
                Control.LightDirection = PCModelViewConstants.OutdoorLightDirection;
                Control.LightPalette = PCModelViewConstants.DaytimePalette;
                Control.Yaw = 30;
                Control.BackgroundColor = _backgroundColor;
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
            if (_polyChar?.Rig?.RootBone != null) {
                instsWithBones = _polyChar.Rig.RootBone.Flatten()
                    .Where(x => x.ModelID.HasValue || ((x.Tag == 0x30 || x.Tag == 0x81) && _polyChar.WeaponXPData != null))
                    .Select((x, i) => {
                        var model = _polyChar.GetModel(x.ModelID ?? _polyChar.WeaponXPData.ModelID, 0);
                        return (model == null) ? (Bone: x, Instance: null) : (Bone: x, Instance: new SGL_ModelInstance((_, _) => model) {
                            ModelCollectionID = model.ModelCollectionID,
                            ModelID           = model.ModelID,
                            ModelInstanceID   = instanceId++
                        });
                    })
                    .Where(x => x.Instance != null)
                    .ToArray();
            }

            _instBones = new IBone[instsWithBones.Length];
            _instances = new SGL_ModelInstance[instsWithBones.Length];
            for (int i = 0; i < instsWithBones.Length; i++) {
                _instances[i] = instsWithBones[i].Instance;
                _instBones[i] = instsWithBones[i].Bone;
            }

            _lastFrame = -1;

            if (_animation != null) {
                _minFrame = _animation.StartFrame;
                _maxFrame = _animation.StartFrame + _animation.FrameCount - 1;
            }
            else {
                _minFrame = PolyChar?.BoneKeyframesTable?.GetEarliestKeyframe() ?? 0;
                _maxFrame = PolyChar?.BoneKeyframesTable?.GetLatestKeyframe() ?? 0;
            }
            _frame = _minFrame;
            _keyframes = _animation?.BoneKeyframes ?? PolyChar?.BoneKeyframesTable;
            UpdateKeyframeInfo();

            if (Control != null)
                Control.Update(_polyChar, _instances);

            UpdateModelInstancesState();
        }

        private void OnFrameTick(object sender, float delta) {
            Control.Zoom = Math.Min(Control.Width / (float) Control.Height, Control.Height / (float) Control.Width) * 1.25f;

            if (_maxFrame <= _minFrame)
                _frame = _minFrame;
            else {
                _frame += Math.Min(60, delta) * 30.0f / 1000.0f;
                while (_frame >= _maxFrame)
                    _frame -= Math.Max(1, _maxFrame - _minFrame);
            }

            UpdateModelInstancesState();
        }

        private void UpdateModelInstancesState() {
            UpdateKeyframeInfo();

            if (_animation == null) {
                var framesUntilNextKeyframe = GetFramesUntilNextKeyframe();
                if (framesUntilNextKeyframe >= 30) {
                    if (_frame + framesUntilNextKeyframe >= _maxFrame)
                        _frame -= Math.Max(1, _maxFrame - _minFrame);
                    else
                        _frame += framesUntilNextKeyframe;
                    UpdateKeyframeInfo();
                }
            }

            UpdateModelMatrix();
        }

        private void UpdateKeyframeInfo()
            => _keyframeInfo = _keyframes?.GetAnimationBoneKeyframeInfos(_frame) ?? [];

        private float GetFramesUntilNextKeyframe() {
            if (_keyframeInfo.Length == 0)
                return 0;

            float posFrames = 1000000;
            float rotFrames = 1000000;
            float scaleFrames = 1000000;

            foreach (var kfi in _keyframeInfo) {
                if (kfi.Pos.FramesLeft < posFrames)
                    posFrames = kfi.Pos.FramesLeft;
                if (kfi.Rot.FramesLeft < rotFrames)
                    rotFrames = kfi.Rot.FramesLeft;
                if (kfi.Scale.FramesLeft < scaleFrames)
                    scaleFrames = kfi.Scale.FramesLeft;
            }

            return Math.Min(posFrames, Math.Min(rotFrames, scaleFrames));
        }

        private void UpdateModelMatrix() {
            if (_lastFrame != _frame) {
                _lastFrame = _frame;
                for (int i = 0; i < _instances.Length; i++) {
                    var inst = _instances[i];
                    inst.Matrix = _keyframes.GetModelInstanceMatrixInAnimation(_keyframeInfo, _instBones[i]);
                }
            }
        }

        private PolyChar _polyChar = null;
        private IModelAnimation _animation = null;
        private IReadOnlyList<PCAnimationCmdStruct> _animCmds;

        public PolyChar PolyChar => _polyChar;
        public IModelAnimation Animation => _animation;

        private Color4 _backgroundColor = new(0.2f, 0.3f, 0.3f, 1.0f);
        public Color4 BackgroundColor {
            get => _backgroundColor;
            set {
                if (_backgroundColor != value) {
                    _backgroundColor = value;
                    if (IsCreated)
                        Control.BackgroundColor = value;
                }
            }
        }

        public void SetAnimation(PolyChar polyChar, PCAnimationDefStruct animation, IReadOnlyList<PCAnimationCmdStruct> animCmds) {
            if (_polyChar != polyChar || _animation != animation) {
                _polyChar  = polyChar;
                _animation = animation;
                _animCmds  = animCmds;
                UpdateModelInstances();
            }
        }

        private float _lastFrame = -1;
        private float _frame = 0;
        private float _minFrame = 0;
        private float _maxFrame = 0;

        private SGL_ModelInstance[] _instances = [];
        private IBone[] _instBones = [];
        private BoneKeyframeIndices[] _keyframeInfo;
        private IReadOnlyList<IBoneKeyframe> _keyframes;
    }
}
