﻿using System;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    public class UnknownInt16Table : TerminatedTable<UnknownInt16Struct> {
        protected UnknownInt16Table(IByteData data, string name, int address, int? count, int? readUntil)
        : base(data, name, address, readUntil.HasValue ? 2 : 0, count) {
            if (!count.HasValue && !readUntil.HasValue)
                throw new ArgumentNullException(nameof(count) + ", " + nameof(readUntil));

            ReadUntil = readUntil;
            FormatString = "X" + MaxSize.ToString().Length;
        }

        public static UnknownInt16Table Create(IByteData data, string name, int address, int? count, int? readUntil) {
            var newTable = new UnknownInt16Table(data, name, address, count, readUntil);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        private bool ContinueReadingPred(Dictionary<int, UnknownInt16Struct> currentRows, UnknownInt16Struct newModel)
            => newModel.Value != ReadUntil.Value;

        public override bool Load() {
            var pred = ReadUntil.HasValue ? ContinueReadingPred : (ContinueReadingPredicate) null;
            return Load((id, address) => new UnknownInt16Struct(Data, id, "Unknown Int16 " + id.ToString(FormatString), address), pred, false);
        }

        public int? ReadUntil { get; }
        private string FormatString { get; }
    }
}
