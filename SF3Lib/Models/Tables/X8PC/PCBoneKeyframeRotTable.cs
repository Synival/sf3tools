using System;
using System.Collections.Generic;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCBoneKeyframeRotTable : Table<PCBoneKeyframeRotStruct> {
        protected PCBoneKeyframeRotTable(IByteData data, string name, int boneId, int keyframeCount, int framesAddr, int xsAddr, int ysAddr, int zsAddr, int wsAddr)
        : base(data, name, 0 /* N/A */) {
            BoneID         = boneId;
            _keyframeCount = keyframeCount;
            _framesAddr    = framesAddr;
            _xsAddr        = xsAddr;
            _ysAddr        = ysAddr;
            _zsAddr        = zsAddr;
            _wsAddr        = wsAddr;
        }

        public static PCBoneKeyframeRotTable Create(IByteData data, string name, int boneId, int keyframeCount, int framesAddr, int xsAddr, int ysAddr, int zsAddr, int wsAddr)
            => Create(() => new PCBoneKeyframeRotTable(data, name, boneId, keyframeCount, framesAddr, xsAddr, ysAddr, zsAddr, wsAddr));

        public override bool Load() {
            var rows = new List<PCBoneKeyframeRotStruct>();
            try {
                var frameAddr = _framesAddr;
                var xAddr     = _xsAddr;
                var yAddr     = _ysAddr;
                var zAddr     = _zsAddr;
                var wAddr     = _wsAddr;

                for (int i = 0; i < _keyframeCount; i++) {
                    rows.Add(new PCBoneKeyframeRotStruct(Data, BoneID, i, $"Bone{BoneID:D2}_KeyframeRot{i:D3}", frameAddr, xAddr, yAddr, zAddr, wAddr));
                    frameAddr += 2;
                    xAddr     += 2;
                    yAddr     += 2;
                    zAddr     += 2;
                    wAddr += 2;
                }
            }
            catch (Exception e) {
                Logger.WriteLine($"Error loading table '{this.GetType().Name}':", LogType.Error);
                using (Logger.IndentedSection())
                    Logger.LogException(e);
            }
            _rows = rows.ToArray();
            return true;
        }

        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;

        public int BoneID { get; }

        private readonly int _keyframeCount;
        private readonly int _framesAddr;
        private readonly int _xsAddr;
        private readonly int _ysAddr;
        private readonly int _zsAddr;
        private readonly int _wsAddr;
    }
}
