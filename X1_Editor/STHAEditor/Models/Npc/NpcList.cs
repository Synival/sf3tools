﻿using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;
using System.Runtime.InteropServices.ComTypes;

namespace STHAEditor.Models.Npcs
{
    public static class NpcList
    {
        private static Npc[] itemssorted;
        private static Npc[] items;


        private static string r = "";


        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadNpcList()
        {



            r = "Resources/X1Npc.xml";
            


            itemssorted = new Npc[0];
            items = new Npc[100]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Npc[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))

                /*if(Globals.treasureDebug == true)
                {
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {

                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Npc[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Npc[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Npc(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].NpcID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].SpriteID == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }


                            }

                        }
                    }
                }

                else*/
                {
                    while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].SpriteID != 0xffff))
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {

                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Npc[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Npc[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Npc(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].NpcID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].SpriteID == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }


                            }

                        }
                    }
                }






                
                stream.Close();
            } catch (FileLoadException) {
                return false;
            } catch (FileNotFoundException) {
                return false;
            }
            return true;
        }

        public static Npc[] getNpcList()
        {
            return itemssorted;
        }
        public static Npc getNpcs(int id)
        {
            return items[id];
        }
    }
}




/*
using System;
//using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Treasures
{
    public static class TreasureList
    {
        private static List<Item> itemssorted;
        private static Dictionary<int, Item> items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadTreasureList()
        {



            if (Globals.scenario == 1)
            {

                r = "Resources/X1Treasure.xml";
            }
            if (Globals.scenario == 2)
            {

                r = "Resources/scenario2Spells.xml";
            }
            if (Globals.scenario == 3)
            {

                r = "Resources/scenario3Spells.xml";
            }
            if (Globals.scenario == 4)
            {

                r = "Resources/PDSpells.xml";
            }


            itemssorted = new List<Item>(256);
            items = new Dictionary<int, Item>(256); //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Treasure[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        Treasure newItem = new Treasure(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        itemssorted.Add(newItem);
                        items[newItem.ID] = newItem;
                        if (newItem.EnemyID == 0xFFFF)
                        {
                            break;
                        }
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public static Treasure[] getItemList()
        {
            return itemssorted.ToArray();
        }
        public static Treasure getItem(int id)
        {
            return items[id];
        }
    }
}
*/


