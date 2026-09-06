using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCAnimationFrameTable : Table<PCAnimationFrameStruct> {
        protected PCAnimationFrameTable(string name, PolyChar polyChar)
        : base(null /*N/A*/, name, 0 /*dummy value*/) {
            MinFrame = polyChar.BoneKeyframePosTables.Select(x => x.Min(y => y.Frame))
                .Concat(polyChar.BoneKeyframeRotTables.Select(x => x.Min(y => y.Frame)))
                .Concat(polyChar.BoneKeyframeScaleTables.Select(x => x.Min(y => y.Frame)))
                .Min();

            MaxFrame = polyChar.BoneKeyframePosTables.Select(x => x.Max(y => y.Frame))
                .Concat(polyChar.BoneKeyframeRotTables.Select(x => x.Max(y => y.Frame)))
                .Concat(polyChar.BoneKeyframeScaleTables.Select(x => x.Max(y => y.Frame)))
                .Max();

            PolyChar = polyChar;
        }

        public static PCAnimationFrameTable Create(string name, PolyChar polyChar)
            => Create(() => new PCAnimationFrameTable(name, polyChar));

        public override bool Load() {
            try {
                var rows = new List<PCAnimationFrameStruct>();
                int idx = 0;
                for (float f = MinFrame; f <= MaxFrame; f += 0.5f)
                    rows.Add(new PCAnimationFrameStruct(Data, $"Frame_{f:000.0}", idx++, f, PolyChar));
                _rows = rows.ToArray();
            }
            catch (Exception e) {
                _rows = new PCAnimationFrameStruct[0];
                Logger.WriteLine($"Error loading table '{this.GetType().Name}':", LogType.Error);
                using (Logger.IndentedSection())
                    Logger.LogException(e);
            }
            return true;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public PolyChar PolyChar { get; }
        public int MinFrame { get; }
        public ushort MaxFrame { get; }
    }
}
