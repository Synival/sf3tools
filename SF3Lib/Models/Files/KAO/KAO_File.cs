using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.KAO {
    public class KAO_File : ScenarioTableFile, IKAO_File {
        // Not applicable
        public override int RamAddress => 0x00000000;
        // Not applicable
        public override int RamAddressLimit => 0x00000000;

        protected KAO_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario)
        : base(data, nameGetterContext, scenario) {
        }

        public static KAO_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario) {
            var newFile = new KAO_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();

            // TODO: All the data

            return tables;
        }
    }
}
