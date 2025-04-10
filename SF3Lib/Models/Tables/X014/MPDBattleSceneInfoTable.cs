﻿using System;
using SF3.ByteData;
using SF3.Models.Structs.X014;

namespace SF3.Models.Tables.X014 {
    public class MPDBattleSceneInfoTable : TerminatedTable<MPDBattleSceneInfo> {
        protected MPDBattleSceneInfoTable(IByteData data, string name, int address) : base(data, name, address, 4, null) {
        }

        public static MPDBattleSceneInfoTable Create(IByteData data, string name, int address) {
            var newTable = new MPDBattleSceneInfoTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new MPDBattleSceneInfo(Data, id, "MPDBattleScene" + id.ToString("D2"), address),
                (rows, lastRow) => (uint) lastRow.MPDFileID != 0xFFFFFFFFu,
                false
            );
        }
    }
}
