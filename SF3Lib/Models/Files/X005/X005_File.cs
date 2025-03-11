using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X005 {
    public class X005_File : ScenarioTableFile, IX005_File {
        public readonly int c_ramOffset = 0; // TODO: what's the ram offset?

        protected X005_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X005_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X005_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
            };

            // TODO: what tables?
            return tables;
        }
    }
}
