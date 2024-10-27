using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables {
    public class SpellIconTable : Table<SpellIcon> {
        public SpellIconTable(IIconPointerFileEditor fileEditor, bool isX026) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(Scenario, "SpellIcons.xml");
            IsX026       = isX026;

            switch (Scenario) {
                case ScenarioType.Scenario1: {
                    int offset, sub;
                    if (IsX026) {
                        offset = 0x0a30; //scn1 initial pointer
                        sub = 0x06078000;
                    }
                    else {
                        offset = 0x00000030; //scn1 initial pointer
                        sub = 0x06068000;
                    }

                    Address = FileEditor.GetDouble(offset) - sub;
                    RealOffsetStart = 0xFF8E;
                    break;
                }

                case ScenarioType.Scenario2: {
                    int offset, sub;
                    if (IsX026) {
                        offset = 0x0a1c; //scn2 x026 initial pointer
                        sub = 0x06078000;
                    }
                    else {
                        offset = 0x00000030; //scn2 initial pointer
                        sub = 0x06068000;
                    }

                    Address = FileEditor.GetDouble(offset) - sub;
                    RealOffsetStart = 0xFC86;
                    break;
                }

                case ScenarioType.Scenario3: {
                    int offset, sub;
                    if (IsX026) {
                        offset = 0x09cc; //scn2 x026 initial pointer
                        sub = 0x06078000;
                    }
                    else {
                        offset = 0x00000030; //scn3 initial pointer
                        sub = 0x06068000;
                    }

                    Address = FileEditor.GetDouble(offset) - sub;
                    RealOffsetStart = 0x12A48;
                    break;
                }

                case ScenarioType.PremiumDisk: {
                    int offset, sub;
                    if (IsX026) {
                        offset = 0x07a0; //pd x026 initial pointer
                        sub = 0x06078000;
                    }
                    else {
                        offset = 0x00000030; //pd initial pointer
                        sub = 0x06068000;
                    }

                    Address = FileEditor.GetDouble(offset) - sub; //pointer
                    RealOffsetStart = 0x12A32;
                    break;
                }

                default:
                    throw new ArgumentException(nameof(Scenario));
            }
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpellIcon(FileEditor, id, name, address, Scenario, IsX026, RealOffsetStart));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 256;

        public bool IsX026 { get; }
        public int RealOffsetStart { get; }
    }
}
