using System;
using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8AN {
    public class X8ANAttackAnimChunkStruct : Struct, ITableContainer {
        private readonly int _boneKeyframesAddr;

        public X8ANAttackAnimChunkStruct(IByteData data, int id, string name, int address, bool hasFixedRotations)
        : base(data, id, name, address, 0x0) {
            HasFixedRotations = hasFixedRotations;

            _boneKeyframesAddr = Address; // 4 bytes

            Tables = new List<ITable>() {
                (BoneKeyframesTable = PCBoneKeyframesTable.Create(data, $"{name}_KeyFrames", Address, Address + Data.GetInt32(_boneKeyframesAddr), hasFixedRotations))
            };

            var firstAddr = (uint) Address;
            var lastAddr = (uint) (BoneKeyframesTable.Address + BoneKeyframesTable.SizeInBytesPlusTerminator);

            var bones = BoneKeyframesTable.Select(x => x).ToArray();
            if (bones.Length > 0) {
                uint MinOrMaxAddr(PCBoneKeyframesStruct bone, uint start, Func<uint, uint, uint> oper) {
                    var value = start;
                    void ApplyOperForArray(uint offset, uint count, uint elemSize)
                        => oper(start, (uint) Address + oper(offset, offset + count * elemSize));

                    ApplyOperForArray(bone.PosFramesOffset,   bone.NumPosKeyFrames, 2);
                    ApplyOperForArray(bone.PosXPtr,           bone.NumPosKeyFrames, 4);
                    ApplyOperForArray(bone.PosYPtr,           bone.NumPosKeyFrames, 4);
                    ApplyOperForArray(bone.PosZPtr,           bone.NumPosKeyFrames, 4);

                    var rotValueSize = hasFixedRotations ? 4u : 2u;
                    ApplyOperForArray(bone.RotFramesOffset,   bone.NumRotKeyFrames, 2);
                    ApplyOperForArray(bone.RotXPtr,           bone.NumRotKeyFrames, rotValueSize);
                    ApplyOperForArray(bone.RotYPtr,           bone.NumRotKeyFrames, rotValueSize);
                    ApplyOperForArray(bone.RotZPtr,           bone.NumRotKeyFrames, rotValueSize);
                    ApplyOperForArray(bone.RotWPtr,           bone.NumRotKeyFrames, rotValueSize);

                    ApplyOperForArray(bone.ScaleFramesOffset, bone.NumScaleKeyFrames, 2);
                    ApplyOperForArray(bone.ScaleXPtr,         bone.NumScaleKeyFrames, 4);
                    ApplyOperForArray(bone.ScaleYPtr,         bone.NumScaleKeyFrames, 4);
                    ApplyOperForArray(bone.ScaleZPtr,         bone.NumScaleKeyFrames, 4);

                    return value;
                }

                firstAddr = Math.Min(firstAddr, bones.Min(x => MinOrMaxAddr(x, (uint) x.Address,            (a, b) => Math.Min(a, b))));
                lastAddr  = Math.Max(lastAddr,  bones.Max(x => MinOrMaxAddr(x, (uint) (x.Address + x.Size), (a, b) => Math.Max(a, b))));
            }

            Size = (int) (lastAddr - firstAddr);
        }

        public IEnumerable<ITable> Tables { get; }
        public bool HasFixedRotations { get; }
        public PCBoneKeyframesTable BoneKeyframesTable { get; }
    }
}
