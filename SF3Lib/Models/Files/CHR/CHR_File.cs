using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.CHR;
using SF3.Types;

namespace SF3.Models.Files.CHR {
    public class CHR_File : ScenarioTableFile, ICHR_File {
        public override int RamAddress => 0; // TODO: where is this loaded? is it even relevant?
        public override int RamAddressLimit => 0; // TODO: where is this loaded? is it even relevant?

        protected CHR_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario, bool isCHP) : base(data, nameContext, scenario) {
            IsCHP = isCHP;
        }

        public static CHR_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario, bool isCHP) {
            var newFile = new CHR_File(data, nameContext, scenario, isCHP);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), 0x00, IsCHP))
            };
        }

        public bool IsCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
    }
}
