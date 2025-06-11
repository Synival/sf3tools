using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Models.Files.X024 {
    public class X024_File : ScenarioTableFile, IX024_File {
        protected X024_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X024_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X024_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        private int GetBlacksmithTableAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario2:   return 0x2d2c; // Same in both versions
                default: return 0;
            }
        }

        public override IEnumerable<ITable> MakeTables() {            
            var tables = new List<ITable>();

            var blacksmithAddr = GetBlacksmithTableAddr();
            if (blacksmithAddr > 0)
                tables.Add(BlacksmithTable = BlacksmithTable.Create(Data, nameof(BlacksmithTable), blacksmithAddr));

            return tables;
        }

        public BlacksmithTable BlacksmithTable { get; private set; }
    }
}
