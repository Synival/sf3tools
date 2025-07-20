using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.CHP {
    public class CHP_File : ScenarioTableFile, ICHP_File {
        public override int RamAddress => 0x002E8000;
        public override int RamAddressLimit => 0x00300000;

        protected CHP_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static CHP_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new CHP_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var addr = 0;
            int globalId = 0;

            CHR_EntriesByOffset = new Dictionary<int, ICHR_File>();

            var dataEof = Data.Length;
            while (addr + 0x18 < dataEof) {
                var header = new SpriteHeader(Data, 0, "", addr, (uint) addr);
                if (header.IsValid()) {
                    var newFile = CHR_File.Create(Data, NameGetterContext, Scenario, globalId, (uint) addr);
                    globalId += newFile.SpriteTable.Length;
                    CHR_EntriesByOffset[addr] = newFile;
                }
                addr += 0x800;
            };

            return CHR_EntriesByOffset.Values.SelectMany(x => x.Tables).ToArray();
        }

        public CHP_Def ToCHP_Def() {
            return new CHP_Def() {
                CHRsByOffset = CHR_EntriesByOffset.ToDictionary(x => x.Key, x => x.Value.ToCHR_Def())
            };
        }

        public Dictionary<int, ICHR_File> CHR_EntriesByOffset { get; private set; }
    }
}
