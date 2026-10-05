using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;
using SF3.Types;

namespace SF3.Models.Files.X8AN {
    public class X8AN_File : ScenarioTableFile, IX8AN_File {
        public override int RamAddress => 0x00200000; // TODO: Where?
        public override int RamAddressLimit => 0x002FFFFF; // TODO: Where?

        protected X8AN_File(IByteData data, INameGetterContext nameContext) : base(data, nameContext, ScenarioType.Scenario1) {
            Discoveries = null;
        }

        public static X8AN_File Create(IByteData data, INameGetterContext nameContext) {
            var newFile = new X8AN_File(data, nameContext);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X8AN_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {};

            // TODO: Actual table!
            AttackAnimBoneKeyframesTables = new PCBoneKeyframesTable[0];

            tables.AddRange(AttackAnimBoneKeyframesTables);
            return tables;
        }

        public IReadOnlyList<PCBoneKeyframesTable> AttackAnimBoneKeyframesTables { get; private set; }
    }
}
