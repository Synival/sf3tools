using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Exceptions;
using SF3.Types;
using static SF3.Utils.ResourceUtils;
using SF3.Models.Tables;
using SF3.Models.Tables.IconPointer;
using SF3.RawData;

namespace SF3.Models.Files.IconPointer {
    public class IconPointerFile : ScenarioTableFile, IIconPointerFile {
        protected IconPointerFile(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static IconPointerFile Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new IconPointerFile(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            const int sub_X021 = 0x06068000;
            const int sub_X026 = 0x06078000;

            int spellIconAddress_X021;
            int spellIconAddress_X026;
            int itemIconAddress_X021;
            int itemIconAddress_X026;
            int spellIconRealOffsetStart;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    spellIconAddress_X021 = Data.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Data.GetDouble(0x0a30) - sub_X026;
                    itemIconAddress_X021  = Data.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Data.GetDouble(0x08f0) - sub_X026;
                    spellIconRealOffsetStart = 0xFF8E;
                    break;

                case ScenarioType.Scenario2:
                    spellIconAddress_X021 = Data.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Data.GetDouble(0x0a1c) - sub_X026;
                    itemIconAddress_X021  = Data.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Data.GetDouble(0x0a08) - sub_X026;
                    spellIconRealOffsetStart = 0xFC86;
                    break;

                case ScenarioType.Scenario3:
                    spellIconAddress_X021 = Data.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Data.GetDouble(0x09cc) - sub_X026;
                    itemIconAddress_X021  = Data.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Data.GetDouble(0x09b4) - sub_X026;
                    spellIconRealOffsetStart = 0x12A48;
                    break;

                case ScenarioType.PremiumDisk:
                    spellIconAddress_X021 = Data.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Data.GetDouble(0x07a0) - sub_X026;
                    itemIconAddress_X021  = Data.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Data.GetDouble(0x072c) - sub_X026;
                    spellIconRealOffsetStart = 0x12A32;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            var isX021 = spellIconAddress_X021 >= 0 && spellIconAddress_X021 < Data.Length &&
                         itemIconAddress_X021  >= 0 && itemIconAddress_X021  < Data.Length;
            var isX026 = spellIconAddress_X026 >= 0 && spellIconAddress_X026 < Data.Length &&
                         itemIconAddress_X026  >= 0 && itemIconAddress_X026  < Data.Length;

            if (!(isX021 || isX026))
                throw new ModelFileLoaderException("This doesn't look like an X021 or X026 file");
            else if (isX021 && isX026)
                throw new ModelFileLoaderException("This looks like both an X021 and X026 file");

            var spellIconAddress = isX026 ? spellIconAddress_X026 : spellIconAddress_X021;
            var itemIconAddress  = isX026 ? itemIconAddress_X026  : itemIconAddress_X021;
            var has16BitIconAddr = Scenario == ScenarioType.Scenario1 && isX026;

            return new List<ITable>() {
                (SpellIconTable = SpellIconTable.Create(Data, ResourceFileForScenario(Scenario, "SpellIcons.xml"), spellIconAddress, has16BitIconAddr, spellIconRealOffsetStart)),
                (ItemIconTable  = ItemIconTable.Create(Data, ResourceFileForScenario(Scenario, "Items.xml"), itemIconAddress, has16BitIconAddr))
            };
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }
    }
}
