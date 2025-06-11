using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Models.Files.X027 {
    public class X027_File : ScenarioTableFile, IX027_File {
        protected X027_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X027_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X027_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int[] GetBlacksmithTableAddrs() {
            switch (Scenario) {
                case ScenarioType.Scenario3:   return new int[] { 0x4ee4, 0x5830 };
                case ScenarioType.PremiumDisk: return new int[] { 0x4eec, 0x5838 };
                default: return new int[] {};
            }
        }

        public override IEnumerable<ITable> MakeTables() {            
            var tables = new List<ITable>();

            var blacksmithAddrs = GetBlacksmithTableAddrs();
            var blacksmithTables = new List<BlacksmithTable>();
            foreach (var addr in blacksmithAddrs)
                blacksmithTables.Add(BlacksmithTable.Create(Data, $"{nameof(BlacksmithTable)}{blacksmithTables.Count + 1:D2} (@0x{addr:X4})", addr));
            BlacksmithTables = blacksmithTables.ToArray();

            tables.AddRange(blacksmithTables);
            return tables;
        }

        public BlacksmithTable[] BlacksmithTables { get; private set; }
    }
}
