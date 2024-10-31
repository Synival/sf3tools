using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class TileMovement {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int noEntry;
        private readonly int unknown01;
        private readonly int grassland;
        private readonly int dirt;
        private readonly int darkGrass;
        private readonly int forest;
        private readonly int brownMountain;
        private readonly int desert;
        private readonly int greyMountain;

        private readonly int unknown09;
        private readonly int unknown0a;
        private readonly int unknown0b;
        private readonly int unknown0c;
        private readonly int unknown0d;
        private readonly int unknown0e;
        private readonly int unknown0f;

        private readonly int offset;
        private readonly int sub;

        public TileMovement(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario2) {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer

                /*offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);

                offset = offset - sub + 0x7c; //second pointer*/

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
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always (except for btl330-339
                //Console.WriteLine(offset);
                offset = _fileEditor.GetDouble(offset);

                if (offset < 0x06070000 && offset > 0)                     //Console.WriteLine("finishing normal proceedure");
{
                    offset -= sub; //second pointer
                }
                else //work around for x1btl330-339 not being consistant with everything else
                {
                    //Console.WriteLine("initiating 330-339 workaround");
                    offset = 0x00000024;
                    //sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset += 0x14; //value we want is 0xac bytes later always
                }

                /*
                offset = 0x000001c4;
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = offset + 0xac; //value we want is 0xac bytes later always
                                            //Console.WriteLine(offset);
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //second pointer
                /*

                /*
                //work around for x1btl330-339 not being consistant with everything else
                offset = 0x00000024;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = offset + 0x14; //value we want is 0xac bytes later always
                //Console.WriteLine(offset);
                //offset = _fileEditor.GetDouble(offset);
                //offset = offset - sub; //second pointer
                */

                /*
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);

                offset = offset - sub + 0x7c; //second pointer*/
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always
                offset = _fileEditor.GetDouble(offset);

                if (offset < 0x06070000 && offset > 0) //a valid pointer in this fille will always be positive up to 0x06070000
{
                    //Console.WriteLine("finishing normal proceedure");
                    offset -= sub; //second pointer
                }
                else //work around for x1btlP05 not being consistant with everything else
                {
                    //Console.WriteLine("P05");
                    offset = 0x00000024;
                    //sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset += 0x14;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            TileID = id;
            TileName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x10);

            noEntry = start;
            unknown01 = start + 1;
            grassland = start + 2;
            dirt = start + 3;
            darkGrass = start + 4;
            forest = start + 5;
            brownMountain = start + 6;
            desert = start + 7;
            greyMountain = start + 8;

            unknown09 = start + 9;
            unknown0a = start + 0xa;
            unknown0b = start + 0xb;
            unknown0c = start + 0xc;
            unknown0d = start + 0xd;
            unknown0e = start + 0xe;
            unknown0f = start + 0xf;

            TileAddress = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int TileID { get; }

        [BulkCopyRowName]
        public string TileName { get; }

        public int TileNoEntry {
            get => _fileEditor.GetByte(noEntry);
            set => _fileEditor.SetByte(noEntry, (byte) value);
        }

        public int TileUnknown1 {
            get => _fileEditor.GetByte(unknown01);
            set => _fileEditor.SetByte(unknown01, (byte) value);
        }

        public int TileGrassland {
            get => _fileEditor.GetByte(grassland);
            set => _fileEditor.SetByte(grassland, (byte) value);
        }

        public int TileDirt {
            get => _fileEditor.GetByte(dirt);
            set => _fileEditor.SetByte(dirt, (byte) value);
        }

        public int TileDarkGrass {
            get => _fileEditor.GetByte(darkGrass);
            set => _fileEditor.SetByte(darkGrass, (byte) value);
        }

        public int TileForest {
            get => _fileEditor.GetByte(forest);
            set => _fileEditor.SetByte(forest, (byte) value);
        }

        public int TileBrownMountain {
            get => _fileEditor.GetByte(brownMountain);
            set => _fileEditor.SetByte(brownMountain, (byte) value);
        }

        public int TileDesert {
            get => _fileEditor.GetByte(desert);
            set => _fileEditor.SetByte(desert, (byte) value);
        }

        public int TileGreyMountain {
            get => _fileEditor.GetByte(greyMountain);
            set => _fileEditor.SetByte(greyMountain, (byte) value);
        }

        public int TileUnknown9 {
            get => _fileEditor.GetByte(unknown09);
            set => _fileEditor.SetByte(unknown09, (byte) value);
        }

        public int TileUnknownA {
            get => _fileEditor.GetByte(unknown0a);
            set => _fileEditor.SetByte(unknown0a, (byte) value);
        }

        public int TileUnknownB {
            get => _fileEditor.GetByte(unknown0b);
            set => _fileEditor.SetByte(unknown0b, (byte) value);
        }

        public int TileUnknownC {
            get => _fileEditor.GetByte(unknown0c);
            set => _fileEditor.SetByte(unknown0c, (byte) value);
        }

        public int TileUnknownD {
            get => _fileEditor.GetByte(unknown0d);
            set => _fileEditor.SetByte(unknown0d, (byte) value);
        }

        public int TileUnknownE {
            get => _fileEditor.GetByte(unknown0e);
            set => _fileEditor.SetByte(unknown0e, (byte) value);
        }

        public int TileUnknownF {
            get => _fileEditor.GetByte(unknown0f);
            set => _fileEditor.SetByte(unknown0f, (byte) value);
        }

        // public int Map => Globals.map;

        public int TileAddress { get; }
    }
}
