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
            int[] GetFrameTableOffsets(SpriteTable st)
                => st.Select(x => (int) (x.DataOffset + x.FrameTableOffset)).ToArray();
            int[] GetAnimationTableOffsets(SpriteTable st)
                => st.Select(x => (int) (x.DataOffset + x.AnimationTableOffset)).ToArray();
            uint[] GetDataOffsets(SpriteTable st)
                => st.Select(x => x.DataOffset).ToArray();

            SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), 0x00, IsCHP);

            FrameDataOffsetsTable = FrameDataOffsetsTable.Create(Data, nameof(FrameDataOffsetsTable),
                GetFrameTableOffsets(SpriteTable), GetDataOffsets(SpriteTable));
            FrameTablesByFileAddr = SpriteTable
                .Select(x => FrameTable.Create(Data, $"{nameof(FrameTable)}_{x.ID:D2}", (int) (x.DataOffset + x.FrameTableOffset), x.DataOffset, x.Width, x.Height))
                .ToDictionary(x => x.Address, x => x);

            AnimationOffsetsTable = AnimationOffsetsTable.Create(Data, nameof(AnimationOffsetsTable),
                GetAnimationTableOffsets(SpriteTable), GetDataOffsets(SpriteTable));
            AnimationFrameTablesByAddr = AnimationOffsetsTable
                .SelectMany(x => x.Select((y, i) => new { SpriteOff2 = x, FileAddr = (int) (y + x.DataOffset), Index = i, Offset = y }))
                .Where(x => x.Offset != 0)
                .Select(x => AnimationFrameTable.Create(Data, x.SpriteOff2.Name + $"_{x.Index:D2}", x.FileAddr))
                .ToDictionary(x => x.Address, x => x);

            var tables = new List<ITable>() {
                SpriteTable,
                FrameDataOffsetsTable,
                AnimationOffsetsTable
            };
            tables.AddRange(FrameTablesByFileAddr.Values);
            tables.AddRange(AnimationFrameTablesByAddr.Values);

            return tables;
        }

        public bool IsCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
        public FrameDataOffsetsTable FrameDataOffsetsTable { get; private set; }
        public Dictionary<int, FrameTable> FrameTablesByFileAddr { get; private set; }
        public AnimationOffsetsTable AnimationOffsetsTable { get; private set; }
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByAddr { get; private set; }
    }
}
