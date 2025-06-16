using System;
using System.Collections.Generic;
using System.Linq;
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
            int[] GetOffset1Addresses(SpriteTable st)
                => st.Select(x => (int) (x.DataOffset + x.Offset1)).ToArray();
            int[] GetOffset2Addresses(SpriteTable st)
                => st.Select(x => (int) (x.DataOffset + x.Offset2)).ToArray();
            int[] GetOffsetTable(SpriteTable st)
                => st.Select(x => x.DataOffset).ToArray();

            return new List<ITable>() {
                (SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), 0x00, IsCHP)),
                (SpriteOffset1SetTable = SpriteOffset1SetTable.Create(Data, nameof(SpriteOffset1SetTable),
                    GetOffset1Addresses(SpriteTable), GetOffsetTable(SpriteTable))),
                (SpriteOffset2SetTable = SpriteOffset2SetTable.Create(Data, nameof(SpriteOffset2SetTable),
                    GetOffset2Addresses(SpriteTable), GetOffsetTable(SpriteTable))),
            };
        }

        public bool IsCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
        public SpriteOffset1SetTable SpriteOffset1SetTable { get; private set; }
        public SpriteOffset2SetTable SpriteOffset2SetTable { get; private set; }
    }
}
