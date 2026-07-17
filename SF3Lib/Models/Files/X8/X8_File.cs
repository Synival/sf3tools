using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X8 {
    public class X8_File : ScenarioTableFile, IX8_File {
        protected X8_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario)
        : base(data, nameContext, scenario) {
        }

        public override int RamAddress => 0; // TODO: what is it??? does it even have one???

        public override int RamAddressLimit => 0; // TODO: what is it??? does it even have one???

        public override IEnumerable<ITable> MakeTables() {
            // TODO: make tables, omg
            return new ITable[0];
        }

        public static X8_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X8_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X8_File");
            return newFile;
        }
    }
}
