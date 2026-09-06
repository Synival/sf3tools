using System;
using System.Collections.Generic;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.Models.Structs.X8PC;
using SF3.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCBoneWrapperTable : Table<PCBoneWrapperStruct> {
        protected PCBoneWrapperTable(string name, IBone rootBone, PolyChar polyChar)
        : base(null /*N/A*/, name, 0 /*dummy value*/) {
            RootBone = rootBone;
            PolyChar = polyChar;
        }

        public static PCBoneWrapperTable Create(string name, IBone rootBone, PolyChar polyChar)
            => Create(() => new PCBoneWrapperTable(name, rootBone, polyChar));

        public override bool Load() {
            var rows = new List<PCBoneWrapperStruct>();
            try {
                void AddBone(IBone bone, PCBoneWrapperStruct parent) {
                    var newBone = new PCBoneWrapperStruct(bone == RootBone ? "Root" : $"Bone_{bone.BoneID:D2}", bone, PolyChar, parent);
                    if (newBone.BoneID.HasValue || bone == RootBone)
                        rows.Add(newBone);
                    if (bone.Children != null)
                        foreach (var b in bone.Children)
                            AddBone(b, newBone);
                }
                AddBone(RootBone, null);
                _rows = rows.ToArray();
            }
            catch (Exception e) {
                _rows = new PCBoneWrapperStruct[0];
                Logger.WriteLine($"Error loading table '{this.GetType().Name}':", LogType.Error);
                using (Logger.IndentedSection())
                    Logger.LogException(e);
            }
            return true;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public IBone RootBone { get; }
        public PolyChar PolyChar { get; }
    }
}
