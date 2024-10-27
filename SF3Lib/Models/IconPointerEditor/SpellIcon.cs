using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.IconPointerEditor {
    public class SpellIcon : IModel {
        //SPELLS
        private readonly int theSpellIcon;
        private readonly int realOffset;

        public SpellIcon(IIconPointerFileEditor fileEditor, int id, string text) {
            FileEditor = fileEditor;
            Scenario = fileEditor.Scenario;
            IsX026 = fileEditor.IsX026;
            ID = id;
            Name = text;

            int offset;
            int sub;

            if (Scenario == ScenarioType.Scenario1) {
                if (IsX026) {
                    offset = 0x0a30; //scn1 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn1 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0xFF8E;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                if (IsX026) {
                    offset = 0x0a1c; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0xFC86;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                if (IsX026) {
                    offset = 0x09cc; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0x12A48;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                if (IsX026) {
                    offset = 0x07a0; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0x12A32;
            }
            else
                throw new ArgumentException(nameof(fileEditor) + ".Scenario");

            if (IsSc1X026) {
                Size = 2;
                Address = offset + (id * Size);
                theSpellIcon = Address; //1 byte
            }
            else {
                Size = 4;
                Address = offset + (id * Size);
                theSpellIcon = Address; //2 bytes  
            }
        }

        public IFileEditor FileEditor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public string SpellName => new SpellValue(Scenario, ID).Name;

        public ScenarioType Scenario { get; }
        public bool IsX026 { get; }
        public bool IsSc1X026 => IsX026 && Scenario == ScenarioType.Scenario1;

        [BulkCopy]
        public int TheSpellIcon {
            get {
                return IsSc1X026
                    ? FileEditor.GetWord(theSpellIcon)
                    : FileEditor.GetDouble(theSpellIcon);
            }
            set {
                if (IsSc1X026)
                    FileEditor.SetWord(theSpellIcon, value);
                else
                    FileEditor.SetDouble(theSpellIcon, value);
            }
        }

        [BulkCopy]
        public int RealOffset {
            get => TheSpellIcon + realOffset;
            set => TheSpellIcon = value - realOffset;
        }
    }
}
