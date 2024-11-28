using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Exceptions;
using SF3.TableModels;
using SF3.TableModels.IconPointer;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.FileModels.IconPointer {
    public class IconPointerEditor : ScenarioTableEditor, IIconPointerEditor {
        protected IconPointerEditor(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static IconPointerEditor Create(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new IconPointerEditor(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
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
                    spellIconAddress_X021 = Editor.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Editor.GetDouble(0x0a30) - sub_X026;
                    itemIconAddress_X021  = Editor.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Editor.GetDouble(0x08f0) - sub_X026;
                    spellIconRealOffsetStart = 0xFF8E;
                    break;

                case ScenarioType.Scenario2:
                    spellIconAddress_X021 = Editor.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Editor.GetDouble(0x0a1c) - sub_X026;
                    itemIconAddress_X021  = Editor.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Editor.GetDouble(0x0a08) - sub_X026;
                    spellIconRealOffsetStart = 0xFC86;
                    break;

                case ScenarioType.Scenario3:
                    spellIconAddress_X021 = Editor.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Editor.GetDouble(0x09cc) - sub_X026;
                    itemIconAddress_X021  = Editor.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Editor.GetDouble(0x09b4) - sub_X026;
                    spellIconRealOffsetStart = 0x12A48;
                    break;

                case ScenarioType.PremiumDisk:
                    spellIconAddress_X021 = Editor.GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = Editor.GetDouble(0x07a0) - sub_X026;
                    itemIconAddress_X021  = Editor.GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = Editor.GetDouble(0x072c) - sub_X026;
                    spellIconRealOffsetStart = 0x12A32;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            var isX021 = spellIconAddress_X021 >= 0 && spellIconAddress_X021 < Editor.Size &&
                           itemIconAddress_X021  >= 0 && itemIconAddress_X021  < Editor.Size;
            var isX026 = spellIconAddress_X026 >= 0 && spellIconAddress_X026 < Editor.Size &&
                           itemIconAddress_X026  >= 0 && itemIconAddress_X026  < Editor.Size;

            if (!(isX021 || isX026))
                throw new ModelFileLoaderException("This doesn't look like an X021 or X026 file");
            else if (isX021 && isX026)
                throw new ModelFileLoaderException("This looks like both an X021 and X026 file");

            var spellIconAddress = isX026 ? spellIconAddress_X026 : spellIconAddress_X021;
            var itemIconAddress  = isX026 ? itemIconAddress_X026  : itemIconAddress_X021;
            var has16BitIconAddr = Scenario == ScenarioType.Scenario1 && isX026;

            return new List<ITable>() {
                (SpellIconTable = new SpellIconTable(Editor, ResourceFileForScenario(Scenario, "SpellIcons.xml"), spellIconAddress, has16BitIconAddr, spellIconRealOffsetStart)),
                (ItemIconTable  = new ItemIconTable(Editor, ResourceFileForScenario(Scenario, "Items.xml"), itemIconAddress, has16BitIconAddr))
            };
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }
    }
}
