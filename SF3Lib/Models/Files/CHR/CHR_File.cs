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
            uint[] GetDataOffsets(SpriteTable st)
                => st.Select(x => x.DataOffset).ToArray();

            SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), 0x00, IsCHP);

            SpriteOffset1SetTable = SpriteOffset1SetTable.Create(Data, nameof(SpriteOffset1SetTable),
                GetOffset1Addresses(SpriteTable), GetDataOffsets(SpriteTable));
            SpriteFrameTablesByFileAddr = SpriteTable
                .Select(x => SpriteFrameTable.Create(Data, $"{nameof(SpriteFrameTable)}_{x.ID:D2}", (int) (x.DataOffset + x.Offset1), x.DataOffset, x.Width, x.Height))
                .ToDictionary(x => x.Address, x => x);

            SpriteOffset2SetTable = SpriteOffset2SetTable.Create(Data, nameof(SpriteOffset2SetTable),
                GetOffset2Addresses(SpriteTable), GetDataOffsets(SpriteTable));
            SpriteOffset2SubTablesByFileAddr = SpriteOffset2SetTable
                .SelectMany(x => x.Select((y, i) => new { SpriteOff2 = x, FileAddr = (int) (y + x.DataOffset), Index = i, Offset = y }))
                .Where(x => x.Offset != 0)
                .Select(x => SpriteOffset2SubTable.Create(Data, x.SpriteOff2.Name + $"_{x.Index:D2}", x.FileAddr))
                .ToDictionary(x => x.Address, x => x);

            var tables = new List<ITable>() {
                SpriteTable,
                SpriteOffset1SetTable,
                SpriteOffset2SetTable
            };
            tables.AddRange(SpriteFrameTablesByFileAddr.Values);
            tables.AddRange(SpriteOffset2SubTablesByFileAddr.Values);

            return tables;
        }

        public bool IsCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
        public SpriteOffset1SetTable SpriteOffset1SetTable { get; private set; }
        public Dictionary<int, SpriteFrameTable> SpriteFrameTablesByFileAddr { get; private set; }
        public SpriteOffset2SetTable SpriteOffset2SetTable { get; private set; }
        public Dictionary<int, SpriteOffset2SubTable> SpriteOffset2SubTablesByFileAddr { get; private set; }
    }
}
