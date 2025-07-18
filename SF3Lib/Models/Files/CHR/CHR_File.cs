using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.CHR;
using SF3.Sprites;
using SF3.Types;

namespace SF3.Models.Files.CHR {
    public class CHR_File : ScenarioTableFile, ICHR_File {
        public override int RamAddress => 0x00210000;
        public override int RamAddressLimit => 0x00290000;

        protected CHR_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario, int startId, uint dataOffset, bool isInCHP) : base(data, nameContext, scenario) {
            StartID = startId;
            DataOffset = dataOffset;
            IsInCHP = isInCHP;
        }

        public static CHR_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new CHR_File(data, nameContext, scenario, 0, 0, false);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public static CHR_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario, int startId, uint dataOffset) {
            var newFile = new CHR_File(data, nameContext, scenario, startId, dataOffset, true);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new ITable[] {
                (SpriteTable = SpriteTable.Create(Data, nameof(SpriteTable), (int) DataOffset, StartID, DataOffset, NameGetterContext, IsInCHP))
            };
        }

        public CHR_Def ToCHR_Def() {
            return new CHR_Def() {
                Sprites = SpriteTable.Select(x => x.ToCHR_SpriteDef()).ToArray()
            };
        }

        public int StartID { get; }
        public uint DataOffset { get; }
        public bool IsInCHP { get; }
        public SpriteTable SpriteTable { get; private set; }
    }
}
