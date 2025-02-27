using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X012 {
    public class X012_File : ScenarioTableFile, IX012_File {
        protected X012_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X012_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X012_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();
            return tables;
        }

        public override void Dispose() {
        }
    }
}
