using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8AN;
using SF3.Models.Tables.X8PC;
using SF3.Types;

namespace SF3.Models.Files.X8AN {
    public class X8AN_File : ScenarioTableFile, IX8AN_File {
        public override int RamAddress => 0x00200000; // TODO: Where?
        public override int RamAddressLimit => 0x002FFFFF; // TODO: Where?

        protected X8AN_File(IByteData data, INameGetterContext nameContext, int? fileId)
        : base(data, nameContext, ScenarioType.Scenario1, fileId) {
            Discoveries = null;
        }

        public static X8AN_File Create(IByteData data, INameGetterContext nameContext, int? fileId) {
            var newFile = new X8AN_File(data, nameContext, fileId);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X8AN_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            AttackAnimChunkTable = X8ANAttackAnimChunkTable.Create(Data, nameof(AttackAnimChunkTable), 0, hasFixedRotations: false);
            AttackAnimBoneKeyframesTables = AttackAnimChunkTable;

            var tables = new List<ITable>() {};
            tables.AddRange(AttackAnimBoneKeyframesTables);
            tables.AddRange(AttackAnimBoneKeyframesTables.SelectMany(x => x.SelectMany(y => y.Tables)).ToArray());
            return tables;
        }

        public X8ANAttackAnimChunkTable AttackAnimChunkTable { get; private set; }
        public IReadOnlyList<PCBoneKeyframesTable> AttackAnimBoneKeyframesTables { get; private set; }
    }
}
