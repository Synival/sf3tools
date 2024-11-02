using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class TileMovement : Model {
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

        public TileMovement(IX1_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x10) {
            int offset = 0;
            int sub;

            if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer

                /*offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = Editor.GetDouble(offset);

                offset = offset - sub + 0x7c; //second pointer*/

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = Editor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = Editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always (except for btl330-339
                //Console.WriteLine(offset);
                offset = Editor.GetDouble(offset);

                if (offset < 0x06070000 && offset > 0)                     //Console.WriteLine("finishing normal proceedure");
{
                    offset -= sub; //second pointer
                }
                else //work around for x1btl330-339 not being consistant with everything else
                {
                    //Console.WriteLine("initiating 330-339 workaround");
                    offset = 0x00000024;
                    //sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset += 0x14; //value we want is 0xac bytes later always
                }

                /*
                offset = 0x000001c4;
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = offset + 0xac; //value we want is 0xac bytes later always
                                            //Console.WriteLine(offset);
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub; //second pointer
                /*

                /*
                //work around for x1btl330-339 not being consistant with everything else
                offset = 0x00000024;
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = offset + 0x14; //value we want is 0xac bytes later always
                //Console.WriteLine(offset);
                //offset = Editor.GetDouble(offset);
                //offset = offset - sub; //second pointer
                */

                /*
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = Editor.GetDouble(offset);

                offset = offset - sub + 0x7c; //second pointer*/
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset += 0xac; //value we want is 0xac bytes later always
                offset = Editor.GetDouble(offset);

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
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset += 0x14;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

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

            Address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int TileNoEntry {
            get => Editor.GetByte(noEntry);
            set => Editor.SetByte(noEntry, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown1 {
            get => Editor.GetByte(unknown01);
            set => Editor.SetByte(unknown01, (byte) value);
        }

        [BulkCopy]
        public int TileGrassland {
            get => Editor.GetByte(grassland);
            set => Editor.SetByte(grassland, (byte) value);
        }

        [BulkCopy]
        public int TileDirt {
            get => Editor.GetByte(dirt);
            set => Editor.SetByte(dirt, (byte) value);
        }

        [BulkCopy]
        public int TileDarkGrass {
            get => Editor.GetByte(darkGrass);
            set => Editor.SetByte(darkGrass, (byte) value);
        }

        [BulkCopy]
        public int TileForest {
            get => Editor.GetByte(forest);
            set => Editor.SetByte(forest, (byte) value);
        }

        [BulkCopy]
        public int TileBrownMountain {
            get => Editor.GetByte(brownMountain);
            set => Editor.SetByte(brownMountain, (byte) value);
        }

        [BulkCopy]
        public int TileDesert {
            get => Editor.GetByte(desert);
            set => Editor.SetByte(desert, (byte) value);
        }

        [BulkCopy]
        public int TileGreyMountain {
            get => Editor.GetByte(greyMountain);
            set => Editor.SetByte(greyMountain, (byte) value);
        }

        [BulkCopy]
        public int TileUnknown9 {
            get => Editor.GetByte(unknown09);
            set => Editor.SetByte(unknown09, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownA {
            get => Editor.GetByte(unknown0a);
            set => Editor.SetByte(unknown0a, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownB {
            get => Editor.GetByte(unknown0b);
            set => Editor.SetByte(unknown0b, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownC {
            get => Editor.GetByte(unknown0c);
            set => Editor.SetByte(unknown0c, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownD {
            get => Editor.GetByte(unknown0d);
            set => Editor.SetByte(unknown0d, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownE {
            get => Editor.GetByte(unknown0e);
            set => Editor.SetByte(unknown0e, (byte) value);
        }

        [BulkCopy]
        public int TileUnknownF {
            get => Editor.GetByte(unknown0f);
            set => Editor.SetByte(unknown0f, (byte) value);
        }
    }
}
