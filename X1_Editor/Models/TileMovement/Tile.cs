﻿using SF3.Editor;
using SF3.Types;
using SF3.X1_Editor.Models.UnknownAI;
using System;

namespace SF3.X1_Editor.Models.Tiles
{
    public class Tile
    {
        private IFileEditor _fileEditor;

        private int noEntry;
        private int unknown01;
        private int grassland;
        private int dirt;
        private int darkGrass;
        private int forest;
        private int brownMountain;
        private int desert;
        private int greyMountain;

        private int unknown08;
        private int unknown09;
        private int unknown0a;
        private int unknown0b;
        private int unknown0c;
        private int unknown0d;
        private int unknown0e;
        private int unknown0f;

        private int offset;
        private int address;

        private int index;
        private string name;
        private int sub;

        public Tile(IFileEditor fileEditor, ScenarioType scenario, int id, string text)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = offset + 0xac; //value we want is 0xac bytes later always
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer

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
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = offset + 0xac; //value we want is 0xac bytes later always (except for btl330-339
                                        //Console.WriteLine(offset);
                offset = _fileEditor.GetDouble(offset);

                if (offset < 0x06070000 && offset > 0) //a valid pointer in this fille will always be positive up to 0x06070000
                {
                    //Console.WriteLine("finishing normal proceedure");
                    offset = offset - sub; //second pointer
                }
                else //work around for x1btl330-339 not being consistant with everything else
                {
                    //Console.WriteLine("initiating 330-339 workaround");
                    offset = 0x00000024;
                    //sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = offset + 0x14; //value we want is 0xac bytes later always
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

            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x000001c4;
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = offset + 0xac; //value we want is 0xac bytes later always
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer

                /*
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);

                offset = offset - sub + 0x7c; //second pointer
                */
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x10);

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

            address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int TileID => index;
        public string TileName => name;

        public int TileNoEntry
        {
            get => FileEditor.GetByte(noEntry);
            set => FileEditor.SetByte(noEntry, (byte)value);
        }
        public int TileUnknown1
        {
            get => FileEditor.GetByte(unknown01);
            set => FileEditor.SetByte(unknown01, (byte)value);
        }
        public int TileGrassland
        {
            get => FileEditor.GetByte(grassland);
            set => FileEditor.SetByte(grassland, (byte)value);
        }
        public int TileDirt
        {
            get => FileEditor.GetByte(dirt);
            set => FileEditor.SetByte(dirt, (byte)value);
        }
        public int TileDarkGrass
        {
            get => FileEditor.GetByte(darkGrass);
            set => FileEditor.SetByte(darkGrass, (byte)value);
        }

        public int TileForest
        {
            get => FileEditor.GetByte(forest);
            set => FileEditor.SetByte(forest, (byte)value);
        }

        public int TileBrownMountain
        {
            get => FileEditor.GetByte(brownMountain);
            set => FileEditor.SetByte(brownMountain, (byte)value);
        }

        public int TileDesert
        {
            get => FileEditor.GetByte(desert);
            set => FileEditor.SetByte(desert, (byte)value);
        }

        public int TileGreyMountain
        {
            get => FileEditor.GetByte(greyMountain);
            set => FileEditor.SetByte(greyMountain, (byte)value);
        }

        public int TileUnknown9
        {
            get => FileEditor.GetByte(unknown09);
            set => FileEditor.SetByte(unknown09, (byte)value);
        }

        public int TileUnknownA
        {
            get => FileEditor.GetByte(unknown0a);
            set => FileEditor.SetByte(unknown0a, (byte)value);
        }

        public int TileUnknownB
        {
            get => FileEditor.GetByte(unknown0b);
            set => FileEditor.SetByte(unknown0b, (byte)value);
        }

        public int TileUnknownC
        {
            get => FileEditor.GetByte(unknown0c);
            set => FileEditor.SetByte(unknown0c, (byte)value);
        }

        public int TileUnknownD
        {
            get => FileEditor.GetByte(unknown0d);
            set => FileEditor.SetByte(unknown0d, (byte)value);
        }

        public int TileUnknownE
        {
            get => FileEditor.GetByte(unknown0e);
            set => FileEditor.SetByte(unknown0e, (byte)value);
        }

        public int TileUnknownF
        {
            get => FileEditor.GetByte(unknown0f);
            set => FileEditor.SetByte(unknown0f, (byte)value);
        }

        // public int Map => Globals.map;

        public int TileAddress => (address);
    }
}
