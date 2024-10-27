using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class CustomMovement {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int unknown00;
        private readonly int xPos1;
        private readonly int zPos1;
        private readonly int xPos2;
        private readonly int zPos2;
        private readonly int xPos3;
        private readonly int zPos3;
        private readonly int xPos4;
        private readonly int zPos4;
        private readonly int ending;
        private readonly int offset;
        private readonly int sub;

        public CustomMovement(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //third pointer

                offset += 10;
                offset += 0xe9a;
                offset += 0x126;
                offset += 0x84; //size of AITargetPosition
            }
            else if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xe9a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xe9a; //size of the enemy spawn table
                    offset += 0x126; //size of SpawnZones
                    offset += 0x84; //size of AITargetPosition
                    //we're now at our offset after adding these
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;

                    offset += 0xa8a;//size of the enemy spawn table
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;

                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer
                offset = _fileEditor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            CustomMovementID = id;
            CustomMovementName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x16);
            unknown00 = start; //2 bytes
            xPos1 = start + 2;
            zPos1 = start + 4;
            xPos2 = start + 6;
            zPos2 = start + 8;
            xPos3 = start + 10;
            zPos3 = start + 12;
            xPos4 = start + 14;
            zPos4 = start + 16;
            ending = start + 18; //4 bytes

            CustomMovementAddress = offset + (id * 0x16);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int CustomMovementID { get; }
        public string CustomMovementName { get; }

        public int CustomMovementUnknown {
            get => _fileEditor.GetWord(unknown00);
            set => _fileEditor.SetWord(unknown00, value);
        }

        public int CustomMovementX1 {
            get => _fileEditor.GetWord(xPos1);
            set => _fileEditor.SetWord(xPos1, value);
        }

        public int CustomMovementZ1 {
            get => _fileEditor.GetWord(zPos1);
            set => _fileEditor.SetWord(zPos1, value);
        }

        public int CustomMovementX2 {
            get => _fileEditor.GetWord(xPos2);
            set => _fileEditor.SetWord(xPos2, value);
        }

        public int CustomMovementZ2 {
            get => _fileEditor.GetWord(zPos2);
            set => _fileEditor.SetWord(zPos2, value);
        }

        public int CustomMovementX3 {
            get => _fileEditor.GetWord(xPos3);
            set => _fileEditor.SetWord(xPos3, value);
        }

        public int CustomMovementZ3 {
            get => _fileEditor.GetWord(zPos3);
            set => _fileEditor.SetWord(zPos3, value);
        }

        public int CustomMovementX4 {
            get => _fileEditor.GetWord(xPos4);
            set => _fileEditor.SetWord(xPos4, value);
        }

        public int CustomMovementZ4 {
            get => _fileEditor.GetWord(zPos4);
            set => _fileEditor.SetWord(zPos4, value);
        }

        public int CustomMovementEnd {
            get => _fileEditor.GetDouble(ending);
            set => _fileEditor.SetDouble(ending, value);
        }

        public int CustomMovementAddress { get; }
    }
}
