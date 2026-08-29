using System.Collections.Generic;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.X8PC {
    public class SkeletonParser {
        public SkeletonParser(IByteData data) {
            Data = data;
        }

        public Skeleton Parse(int offset, int? maxUntil = null) {
            if (!maxUntil.HasValue)
                maxUntil = Data.Length;
            var maxUntilValue = maxUntil.Value;

            var skeleton = new Skeleton();
            var nextBoneId = 0;
            var nextModelId = 0;

            ParseBone(skeleton, ref offset, maxUntilValue, ref nextBoneId, ref nextModelId);
            return skeleton;
        }

        private void ParseBone(Bone bone, ref int offset, int maxUntil, ref int nextBoneId, ref int nextModelId) {
            var children = new List<Bone>();
            bool AlignToFour(ref int offset2) {
                offset2 = (offset2 + 3) / 4 * 4;
                return offset2 < maxUntil;
            }

            bool AppendFIXED(ref int offset2, out FIXED valueRead) {
                if (offset2 + 4 > maxUntil) {
                    valueRead = new FIXED();
                    return false;
                }
                valueRead = Data.GetFIXED(offset2);
                offset2 += 4;
                return offset2 < maxUntil;
            }

            while (offset < maxUntil) {
                var skelArg = Data.GetUInt8(offset++);
                if (skelArg == 0xFE)
                    break;

                if (skelArg == 0xFD) {
                    var newBone = new Bone {
                        BoneID = nextBoneId++
                    };

                    children.Add(newBone);
                    ParseBone(newBone, ref offset, maxUntil, ref nextBoneId, ref nextModelId);
                    if (offset >= maxUntil)
                        break;
                }
                else {
                    var newModel = new Bone();
                    children.Add(newModel);
                    newModel.Tag = skelArg;

                    if (skelArg == 0 || skelArg == 0x80)
                        newModel.ModelID = nextModelId++;
                    else {
                        if (!AlignToFour(ref offset))
                            break;

                        if (!AppendFIXED(ref offset, out var posX)) break;
                        if (!AppendFIXED(ref offset, out var posY)) break;
                        if (!AppendFIXED(ref offset, out var posZ)) break;
                        newModel.Position = new VECTOR(posX, posY, posZ);

                        if (skelArg == 0x30 || skelArg == 0x81) {
                            if (!AppendFIXED(ref offset, out var rotX)) break;
                            if (!AppendFIXED(ref offset, out var rotY)) break;
                            if (!AppendFIXED(ref offset, out var rotZ)) break;
                            if (!AppendFIXED(ref offset, out var rotW)) break;
                            newModel.Rotation = new QUATERNION(rotX, rotY, rotZ, rotW);

                            if (!AppendFIXED(ref offset, out var scaleX)) break;
                            if (!AppendFIXED(ref offset, out var scaleY)) break;
                            if (!AppendFIXED(ref offset, out var scaleZ)) break;
                            newModel.Scale = new VECTOR(scaleX, scaleY, scaleZ);
                        }
                    }
                }
            }

            bone.Children = children.ToArray();
        }

        public IByteData Data { get; }
    }
}
