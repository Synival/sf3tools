using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X014 {
    public class X014_File : ScenarioTableFile, IX014_File {
        public readonly int c_ramOffset = 0x06070000;

        protected X014_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X014_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X014_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
            };

            return tables;
        }

        public override void Dispose() {
        }
    }
}
