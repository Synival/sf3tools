using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PCAnimationFrameStruct : IStruct, ISGL_ModelInstanceCollection {
        public PCAnimationFrameStruct(IByteData data, string name, int id, float frame, PolyChar polyChar) {
            Data     = data;
            Name     = name;
            ID       = id;
            Frame    = frame;
            PolyChar = polyChar;

            _firstPosKeyframes   = PolyChar.BoneKeyframePosTables.Select(x => GetKeyframe(x.AsArray(), y => y.Frame)).ToArray();
            _firstRotKeyframes   = PolyChar.BoneKeyframeRotTables.Select(x => GetKeyframe(x.AsArray(), y => y.Frame)).ToArray();
            _firstScaleKeyframes = PolyChar.BoneKeyframeScaleTables.Select(x => GetKeyframe(x.AsArray(), y => y.Frame)).ToArray();

            _models = polyChar.BoneTable
                .Where(x => x.BoneID.HasValue)
                .OrderBy(x => x.BoneID.Value)
                .SelectMany((x, i) => x
                    .Where(y => !y.ForceTransparency.HasValue)
                    .Select(y => CreateModelInstance(y, x))
                    .ToArray()
                ).ToArray();
        }

        private ISGL_ModelInstance CreateModelInstance(ISGL_ModelInstance modelInstance, IBone bone) {
            var matrix = (modelInstance.Matrix.HasValue) ? modelInstance.Matrix.Value : Matrix4x4.Identity;

            void ApplyMatrices(IBone b) {
                if (b.BoneID.HasValue) {
                    var bId = b.BoneID.Value;

                    var posFrame   = _firstPosKeyframes[bId];
                    var rotFrame   = _firstRotKeyframes[bId];
                    var scaleFrame = _firstScaleKeyframes[bId];

                    var pos1   = PolyChar.BoneKeyframePosTables[bId][posFrame.IndexA].CreateVector();
                    var rot1   = PolyChar.BoneKeyframeRotTables[bId][rotFrame.IndexA].CreateQuaternion();
                    var scale1 = PolyChar.BoneKeyframeScaleTables[bId][scaleFrame.IndexA].CreateVector();

                    var pos2   = PolyChar.BoneKeyframePosTables[bId][posFrame.IndexB].CreateVector();
                    var rot2   = PolyChar.BoneKeyframeRotTables[bId][rotFrame.IndexB].CreateQuaternion();
                    var scale2 = PolyChar.BoneKeyframeScaleTables[bId][scaleFrame.IndexB].CreateVector();

                    matrix *= IBoneExtensions.CreateMatrix(
                        pos1,   pos2,   posFrame.Mix,
                        rot1,   rot2,   rotFrame.Mix,
                        scale1, scale2, scaleFrame.Mix
                    );
                }
                if (b.Parent != null)
                    ApplyMatrices(b.Parent);
            }
            ApplyMatrices(bone);

            var model = modelInstance.GetModel(0);
            return new SGL_ModelInstance((_1, _2) => model) {
                ModelCollectionID = model.ModelCollectionID,
                ModelID = model.ModelID,
                ModelInstanceID = modelInstance.ModelID,
                Matrix = matrix
            };
        }

        private struct KeyframeInfo {
            public int IndexA, IndexB;
            public float Mix;
            public override string ToString() => $"({IndexA}, {IndexB}) ({Mix})";
        }

        private KeyframeInfo GetKeyframe<T>(T[] list, Func<T, int> frameGetter) {
            var frame = Frame;
            int max = list.Length;
            var lastF = 0;

            for (int i = 0; i < max; i++) {
                var element = list[i];
                var f = frameGetter(element);
                if ((frame >= lastF && frame < f) || i == max - 1) {
                    if (i == 0)
                        return new KeyframeInfo() { IndexA = 0, IndexB = 0, Mix = 0.0f };
                    else if (i == max - 1)
                        return new KeyframeInfo() { IndexA = i, IndexB = i, Mix = 1.0f };
                    else
                        return new KeyframeInfo() { IndexA = i - 1, IndexB = i, Mix = (frame - lastF) / (f - lastF) };
                }
                lastF = f;
            }
            return new KeyframeInfo() { IndexA = 0, IndexB = 0, Mix = 0.0f };
        }

        public IByteData Data { get; }

        [TableViewModelColumn(displayOrder: -2, displayFormat: "000.0")]
        [BulkCopy]
        public float Frame { get; }

        [TableViewModelColumn(displayOrder: -1, minWidth: 120)]
        [BulkCopy]
        public string Name { get; }

        public ISGL_ModelInstance GetModelInstance(int id) => _models.FirstOrDefault(x => x.ModelInstanceID == id);
        public IEnumerator<ISGL_ModelInstance> GetEnumerator() => _models.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public PolyChar PolyChar { get; }

        private KeyframeInfo[] _firstPosKeyframes;
        private KeyframeInfo[] _firstRotKeyframes;
        private KeyframeInfo[] _firstScaleKeyframes;
        private ISGL_ModelInstance[] _models;

        public int ID { get; }
        public int Address => 0; /* N/A */
        public int Size => 0; /* N/A */
    }
}
