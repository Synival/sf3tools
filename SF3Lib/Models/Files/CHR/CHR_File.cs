using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.CHR;
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
            int[] GetFrameTableOffsets(SpriteHeaderTable st)
                => st.Select(x => (int) (x.DataOffset + x.FrameTableOffset)).ToArray();
            int[] GetAnimationTableOffsets(SpriteHeaderTable st)
                => st.Select(x => (int) (x.DataOffset + x.AnimationTableOffset)).ToArray();
            uint[] GetDataOffsets(SpriteHeaderTable st)
                => st.Select(x => x.DataOffset).ToArray();

            SpriteHeaderTable = SpriteHeaderTable.Create(Data, nameof(SpriteHeaderTable), 0x00, IsCHP);
            var spriteIdPropInfo = typeof(SpriteHeader).GetProperty(nameof(SpriteHeader.SpriteID));
            var spriteIds = SpriteHeaderTable.Select(x => x.SpriteID).ToArray();
            var spriteNames = SpriteHeaderTable.Select(x => NameGetterContext.GetName(x, spriteIdPropInfo, x.SpriteID, new object[] { NamedValueType.Sprite })).ToArray();

            FrameDataOffsetsTable = FrameDataOffsetsTable.Create(Data, nameof(FrameDataOffsetsTable),
                GetFrameTableOffsets(SpriteHeaderTable), GetDataOffsets(SpriteHeaderTable), spriteIds);
            FrameTablesByFileAddr = SpriteHeaderTable
                .Select(x => FrameTable.Create(
                    Data,
                    $"Sprite{x.ID:D2}_Frames ({spriteNames[x.ID]})",
                    (int) (x.DataOffset + x.FrameTableOffset),
                    x.DataOffset, x.Width, x.Height,
                    $"Sprite{x.ID:D2}_",
                    x.SpriteID,
                    x.Directions))
                .ToDictionary(x => x.Address, x => x);

            AnimationOffsetsTable = AnimationOffsetsTable.Create(Data, nameof(AnimationOffsetsTable),
                GetAnimationTableOffsets(SpriteHeaderTable), GetDataOffsets(SpriteHeaderTable), spriteIds);
            AnimationFrameTablesByAddr = AnimationOffsetsTable
                .SelectMany(x => x.Select((y, i) => new {
                    AnimOffsets = x, FileAddr = (int) (y + x.DataOffset), Index = i, Offset = y })
                    .Where(y => y.Offset != 0)
                )
                .Select(x => AnimationFrameTable.Create(
                    Data,
                    $"Sprite{x.AnimOffsets.ID:D2}_Animation{x.Index:D2} ({spriteNames[x.AnimOffsets.ID]})",
                    x.FileAddr,
                    $"Sprite{x.AnimOffsets.ID:D2}_Animation{x.Index:D2}_",
                    x.AnimOffsets.SpriteID,
                    x.Index))
                .ToDictionary(x => x.Address, x => x);

            var tables = new List<ITable>() {
                SpriteHeaderTable,
                FrameDataOffsetsTable,
                AnimationOffsetsTable
            };
            tables.AddRange(FrameTablesByFileAddr.Values);
            tables.AddRange(AnimationFrameTablesByAddr.Values);

            return tables;
        }

        public bool IsCHP { get; }
        public SpriteHeaderTable SpriteHeaderTable { get; private set; }
        public FrameDataOffsetsTable FrameDataOffsetsTable { get; private set; }
        public Dictionary<int, FrameTable> FrameTablesByFileAddr { get; private set; }
        public AnimationOffsetsTable AnimationOffsetsTable { get; private set; }
        public Dictionary<int, AnimationFrameTable> AnimationFrameTablesByAddr { get; private set; }
    }
}
