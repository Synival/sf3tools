using System;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using SF3.Models;
using SF3.Types;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Treasures
{
    public class TreasureList : IModelArray<Treasure>
    {
        public TreasureList(IFileEditor fileEditor, ScenarioType scenario)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        private Treasure[] itemssorted;
        private Treasure[] items;
        private IFileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/X1Treasure.xml";

            itemssorted = new Treasure[0];
            items = new Treasure[255]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Treasure[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))

                if (Globals.treasureDebug == true)
                {
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Treasure[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Treasure[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Treasure(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].TreasureID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].Searched == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
                        }
                    }
                }

                else
                {
                    while (!xml.EOF && (itemssorted.Length == 0 || itemssorted[itemssorted.Length - 1].Searched != 0xffff))
                    //while (!xml.EOF && (itemssorted.Length == 0 || (itemssorted[itemssorted.Length - 1].Searched != 0xffff || itemssorted[itemssorted.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (itemssorted.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                old = new Treasure[itemssorted.Length];
                                itemssorted.CopyTo(old, 0);
                                itemssorted = new Treasure[old.Length + 1];
                                old.CopyTo(itemssorted, 0);
                                itemssorted[old.Length] = new Treasure(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                items[itemssorted[old.Length].TreasureID] = itemssorted[old.Length];
                                if (itemssorted[itemssorted.Length - 1].Searched == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
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

        public Treasure[] Models => itemssorted;
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


