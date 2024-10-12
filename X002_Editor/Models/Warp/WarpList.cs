using System;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using BrightIdeasSoftware;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Warps
{
    public class WarpList : IModelArray<Warp>
    {
        public WarpList(IX002_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        public ScenarioType Scenario => _fileEditor.Scenario;

        private Warp[] itemssorted;
        private Warp[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/X002Warp.xml";

            itemssorted = new Warp[0];
            items = new Warp[1000]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

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
                string myName = "WarpIndex " + myCount;
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
                            //itemssorted[old.Length] = new Warp(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));

                            itemssorted[old.Length] = new Warp(_fileEditor, myCount, myName);

                            myCount++;
                            myName = "WarpIndex ";
                            myName = myName + myCount;

                            items[itemssorted[old.Length].WarpID] = itemssorted[old.Length];
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
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
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
using SF3.X002_Editor.Forms.frmX002_Editor;

namespace SF3.X002_Editor.Models.Treasures
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
            if (Globals.scenario == ScenarioType.Scenario1)
            {
                r = "Resources/X1Treasure.xml";
            }
            if (Globals.scenario == ScenarioType.Scenario2)
            {
                r = "Resources/scenario2Spells.xml";
            }
            if (Globals.scenario == ScenarioType.Scenario3)
            {
                r = "Resources/scenario3Spells.xml";
            }
            if (Globals.scenario == ScenarioType.PremiumDisk)
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


