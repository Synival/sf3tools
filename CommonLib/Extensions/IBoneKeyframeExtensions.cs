using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CommonLib.Rigging;
using CommonLib.SGL;

namespace CommonLib.Extensions {
    public static class IBoneKeyframeExtensions {
        public static int GetEarliestKeyframe(this IReadOnlyList<IBoneKeyframe> keyframes) {
            return keyframes.Count > 0
                ? keyframes.Min(x => Math.Min(
                    x.PosTable.Count > 0 ? x.PosTable.Min(y => y.FrameNum) : 0,
                    Math.Min(
                        x.RotTable.Count   > 0 ? x.RotTable  .Min(y => y.FrameNum) : 0,
                        x.ScaleTable.Count > 0 ? x.ScaleTable.Min(y => y.FrameNum) : 0
                    )
                ))
                : 0;
        }

        public static int GetLatestKeyframe(this IReadOnlyList<IBoneKeyframe> keyframes) {
            return keyframes.Count > 0
                ? keyframes.Max(x => Math.Max(
                    x.PosTable.Count > 0 ? x.PosTable.Max(y => y.FrameNum) : 0,
                    Math.Max(
                        x.RotTable.Count   > 0 ? x.RotTable  .Max(y => y.FrameNum) : 0,
                        x.ScaleTable.Count > 0 ? x.ScaleTable.Max(y => y.FrameNum) : 0
                    )
                ))
                : 0;
        }

        private static BoneKeyframeComponentIndices GetAnimationKeyframe<T>(IReadOnlyList<T> list, Func<T, int> frameGetter, float frame, int totalFrames) {
            int max = list.Count;
            var lastF = 0;

            for (int i = 0; i < max; i++) {
                var element = list[i];
                var nextF = frameGetter(element);
                if ((frame >= lastF && frame < nextF) || i == max - 1) {
                    if (i == 0)
                        return new BoneKeyframeComponentIndices(0, 0, 0, nextF, 0);
                    else if (frame < nextF)
                        return new BoneKeyframeComponentIndices(i - 1, i, lastF, nextF, frame);
                    else
                        return new BoneKeyframeComponentIndices(i, 0, nextF, totalFrames, frame);
                }
                lastF = nextF;
            }
            return new BoneKeyframeComponentIndices(0, 0, 0, 0, 0.0f);
        }

        public static BoneKeyframeIndices[] GetAnimationBoneKeyframeInfos(this IReadOnlyList<IBoneKeyframe> keyframes, float frame) {
            var totalFrames = keyframes.GetLatestKeyframe();

            var numBones = keyframes.Count;
            var pos   = keyframes.Select(x => GetAnimationKeyframe(x.PosTable,   y => y.FrameNum, frame, totalFrames)).ToArray();
            var rot   = keyframes.Select(x => GetAnimationKeyframe(x.RotTable,   y => y.FrameNum, frame, totalFrames)).ToArray();
            var scale = keyframes.Select(x => GetAnimationKeyframe(x.ScaleTable, y => y.FrameNum, frame, totalFrames)).ToArray();

            var boneKeyframes = new BoneKeyframeIndices[numBones];
            for (int i = 0; i < numBones; i++)
                boneKeyframes[i] = new BoneKeyframeIndices { Pos = pos[i], Rot = rot[i], Scale = scale[i] };

            return boneKeyframes;
        }

        public static Matrix4x4 GetModelInstanceMatrixInAnimation(this IReadOnlyList<IBoneKeyframe> keyframes, IBone bone, BoneKeyframeIndices? boneFrame) {
            var matrix = Matrix4x4.Identity;

            if (bone.BoneID.HasValue && boneFrame.HasValue) {
                var bId = bone.BoneID.Value;

                var posFrame   = boneFrame.Value.Pos;
                var rotFrame   = boneFrame.Value.Rot;
                var scaleFrame = boneFrame.Value.Scale;

                var posTable = keyframes[bId].PosTable;
                var rotTable = keyframes[bId].RotTable;
                var scaleTable = keyframes[bId].ScaleTable;

                var pos1   = posTable.Count   > posFrame.IndexA   ? posTable[posFrame.IndexA].Vector     : new VECTOR(0, 0, 0);
                var rot1   = rotTable.Count   > rotFrame.IndexA   ? rotTable[rotFrame.IndexA].Quaternion : new QUATERNION(0, 0, 0, 1);
                var scale1 = scaleTable.Count > scaleFrame.IndexA ? scaleTable[scaleFrame.IndexA].Vector : new VECTOR(1, 1, 1);

                var pos2   = posTable.Count   > posFrame.IndexB   ? posTable[posFrame.IndexB].Vector     : new VECTOR(0, 0, 0);
                var rot2   = rotTable.Count   > rotFrame.IndexB   ? rotTable[rotFrame.IndexB].Quaternion : new QUATERNION(0, 0, 0, 1);
                var scale2 = scaleTable.Count > scaleFrame.IndexB ? scaleTable[scaleFrame.IndexB].Vector : new VECTOR(1, 1, 1);

                // A keyframe that is 1 frame away is just a snap. If the lerp distance looks extreme, disable interpolation.
                // Don't interpolate position or scale either if they're the same span of frames.
                var noRotInterp = rotFrame.TotalFrames <= 1 && rot1.GetLerpDist(rot2) > 1.00f;
                matrix *= IBoneExtensions.CreateMatrix(
                    pos1,   pos2,   (noRotInterp && posFrame.SharesKeyframe(rotFrame)) ? 0 : posFrame.Mix,
                    rot1,   rot2,   noRotInterp ? 0 : rotFrame.Mix,
                    scale1, scale2, (noRotInterp && scaleFrame.SharesKeyframe(rotFrame)) ? 0 : scaleFrame.Mix
                );
            }
            else if (bone.Tag == 0x30 || bone.Tag == 0x81)
                matrix *= bone.CreateMatrix();

            return matrix;
        }

        public static Matrix4x4 GetModelInstanceMatrixInAnimation(this IReadOnlyList<IBoneKeyframe> keyframes, BoneKeyframeIndices[] keyframeInfo, IBone bone) {
            var matrix = Matrix4x4.Identity;

            void ApplyMatrices(IBone b) {
                matrix *= GetModelInstanceMatrixInAnimation(keyframes, b, (b.BoneID.HasValue) ? keyframeInfo[b.BoneID.Value] : (BoneKeyframeIndices?) null);
                if (b.Parent != null)
                    ApplyMatrices(b.Parent);
            }

            ApplyMatrices(bone);

            return matrix;
        }
    }
}
