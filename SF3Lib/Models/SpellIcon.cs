using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models {
    public class SpellIcon : IModel {
        //SPELLS
        private readonly int theSpellIcon;

        public SpellIcon(IByteEditor fileEditor, int id, string name, int address, ScenarioType scenario, bool isX026, int realOffsetStart) {
            Editor          = fileEditor;
            ID              = id;
            Name            = name;
            Address         = address;

            IsSc1X026       = scenario == ScenarioType.Scenario1 && isX026;
            RealOffsetStart = realOffsetStart;
            SpellName       = new SpellValue(scenario, ID).Name;

            if (IsSc1X026) {
                Size = 2;
                theSpellIcon = Address; // 1 byte
            }
            else {
                Size = 4;
                theSpellIcon = Address; // 2 bytes  
            }
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public bool IsSc1X026 { get; }
        public string SpellName { get; }
        public int RealOffsetStart { get; }

        [BulkCopy]
        public int TheSpellIcon {
            get {
                return IsSc1X026
                    ? Editor.GetWord(theSpellIcon)
                    : Editor.GetDouble(theSpellIcon);
            }
            set {
                if (IsSc1X026)
                    Editor.SetWord(theSpellIcon, value);
                else
                    Editor.SetDouble(theSpellIcon, value);
            }
        }

        [BulkCopy]
        public int RealOffset {
            get => TheSpellIcon + RealOffsetStart;
            set => TheSpellIcon = value - RealOffsetStart;
        }
    }
}
