using System;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;
using System.Runtime.InteropServices.ComTypes;
using SF3.Models;

namespace SF3.X1_Editor.Models.Warps
{
    public class WarpList : IModelArray<Warp>
    {
        private Warp[] itemssorted;
        private Warp[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/X1Warp.xml";

            itemssorted = new Warp[0];
            items = new Warp[255]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Warp[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))

                while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].WarpType != 0x01 && itemssorted[itemssorted.Length - 1].WarpType != 0xff)))
                //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes)
                        {
                            old = new Warp[itemssorted.Length];
                            itemssorted.CopyTo(old, 0);
                            itemssorted = new Warp[old.Length + 1];
                            old.CopyTo(itemssorted, 0);
                            itemssorted[old.Length] = new Warp(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                            items[itemssorted[old.Length].WarpID] = itemssorted[old.Length];
                        }
                    }
                }

                stream.Close();
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

        public Warp[] Models => itemssorted;
    }
}

/*
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Treasures
{
    public class TreasureList
    {
        private List<Item> itemssorted;
        private Dictionary<int, Item> items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool loadTreasureList()
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

        public Treasure[] getItemList()
        {
            return itemssorted.ToArray();
        }
        public Treasure getItem(int id)
        {
            return items[id];
        }
    }
}
*/


