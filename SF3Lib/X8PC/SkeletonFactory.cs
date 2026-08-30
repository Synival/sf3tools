using System.Collections.Generic;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.X8PC {
    public class SkeletonFactory {
        public SkeletonFactory() {
        }

        public Skeleton CreateSkeleton(IByteData data, int offset, int? maxUntil = null) {
            if (!maxUntil.HasValue)
                maxUntil = data.Length;
            var maxUntilValue = maxUntil.Value;

            var nextBoneId  = -1;
            var nextModelId = 0;

            return new Skeleton(CreateBone(data, ref offset, maxUntilValue, ref nextBoneId, ref nextModelId));
        }

        private Bone CreateBone(IByteData data, ref int offset, int maxUntil, ref int nextBoneId, ref int nextModelId) {
            var thisBoneId = nextBoneId++;

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
                valueRead = data.GetFIXED(offset2);
                offset2 += 4;
                return offset2 < maxUntil;
            }

            while (offset < maxUntil) {
                var skelArg = data.GetUInt8(offset++);

                // End-of-bone
                if (skelArg == 0xFE)
                    break;
                // Create child bone
                else if (skelArg == 0xFD) {
                    children.Add(CreateBone(data, ref offset, maxUntil, ref nextBoneId, ref nextModelId));
                    if (offset >= maxUntil)
                        break;
                }
                // Model or tag placement.
                else {
                    Bone newModel = null;

                    // New model for rendering.
                    if (skelArg == 0 || skelArg == 0x80) {
                        newModel = new Bone(skelArg, nextModelId++);
                    }
                    // Tag or weapon placement.
                    else {
                        if (!AlignToFour(ref offset))
                            break;

                        if (!AppendFIXED(ref offset, out var posX)) break;
                        if (!AppendFIXED(ref offset, out var posY)) break;
                        if (!AppendFIXED(ref offset, out var posZ)) break;
                        var pos = new VECTOR(posX, posY, posZ);

                        // Weapon placements have rotation and scale.
                        if (skelArg == 0x30 || skelArg == 0x81) {
                            if (!AppendFIXED(ref offset, out var rotX)) break;
                            if (!AppendFIXED(ref offset, out var rotY)) break;
                            if (!AppendFIXED(ref offset, out var rotZ)) break;
                            if (!AppendFIXED(ref offset, out var rotW)) break;
                            var rot = new QUATERNION(rotX, rotY, rotZ, rotW);

                            if (!AppendFIXED(ref offset, out var scaleX)) break;
                            if (!AppendFIXED(ref offset, out var scaleY)) break;
                            if (!AppendFIXED(ref offset, out var scaleZ)) break;
                            var scale = new VECTOR(scaleX, scaleY, scaleZ);

                            newModel = new Bone(skelArg, pos, rot, scale);
                        }
                        // Not a weapon; this tag is just a position.
                        else {
                            newModel = new Bone(skelArg, pos);
                        }
                    }

                    // Append to end of child bone/model list.
                    children.Add(newModel);
                }
            }

            // Create the bone with all its children. This is either root bone or real one, depending on the ID.
            var childrenArray = children.ToArray();
            return (thisBoneId >= 0) ? new Bone(thisBoneId, childrenArray) : new Bone(childrenArray);
        }
    }
}
