using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class LoadedOverride : IModel {
        private readonly int mapID;
        private readonly int synMusic;
        private readonly int medMusic;
        private readonly int julMusic;
        private readonly int extraMusic;
        private readonly int synMpd;
        private readonly int medMpd;
        private readonly int julMpd;
        private readonly int extraMpd;
        private readonly int synChr;
        private readonly int medChr;
        private readonly int julChr;
        private readonly int extraChr;
        private readonly int offset;
        private readonly int checkVersion2;

        public LoadedOverride(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x28;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x0000527a; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x000058be; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00006266; //scn3
            }
            else {
                offset = 0x00005aa2; //pd. assumed location if it were to exist
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = name;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x28);
            mapID = start; //2 bytes
            synMusic = start + 0x02; //1 byte
            medMusic = start + 0x03; //1 byte
            julMusic = start + 0x04; //1 byte
            extraMusic = start + 0x05; //1 byte
            synMpd = start + 0x06; //4 bytes mpd synbios?
            medMpd = start + 0x0a; //4 bytes mpd medion
            julMpd = start + 0x0e; //4 bytes mpd julian?
            extraMpd = start + 0x12; //4 bytes mpd extra?
            synChr = start + 0x16; //4 bytes chr synbios?
            medChr = start + 0x1a; //4 bytes chr medion
            julChr = start + 0x1e; //4 bytes chr julian?
            extraChr = start + 0x22; //4 bytes chr extra?
            Address = offset + (id * 0x28);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int MOMapID {
            get => Editor.GetWord(mapID);
            set => Editor.SetWord(mapID, value);
        }

        [BulkCopy]
        public int SynMusic {
            get => Editor.GetByte(synMusic);
            set => Editor.SetByte(synMusic, (byte) value);
        }

        [BulkCopy]
        public int MedMusic {
            get => Editor.GetByte(medMusic);
            set => Editor.SetByte(medMusic, (byte) value);
        }

        [BulkCopy]
        public int JulMusic {
            get => Editor.GetByte(julMusic);
            set => Editor.SetByte(julMusic, (byte) value);
        }

        [BulkCopy]
        public int ExtraMusic {
            get => Editor.GetByte(extraMusic);
            set => Editor.SetByte(extraMusic, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMpd {
            get => Editor.GetDouble(synMpd);
            set => Editor.SetDouble(synMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMpd {
            get => Editor.GetDouble(medMpd);
            set => Editor.SetDouble(medMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMpd {
            get => Editor.GetDouble(julMpd);
            set => Editor.SetDouble(julMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMpd {
            get => Editor.GetDouble(extraMpd);
            set => Editor.SetDouble(extraMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynChr {
            get => Editor.GetDouble(synChr);
            set => Editor.SetDouble(synChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedChr {
            get => Editor.GetDouble(medChr);
            set => Editor.SetDouble(medChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulChr {
            get => Editor.GetDouble(julChr);
            set => Editor.SetDouble(julChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraChr {
            get => Editor.GetDouble(extraChr);
            set => Editor.SetDouble(extraChr, value);
        }
    }
}
