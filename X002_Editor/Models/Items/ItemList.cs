using System;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.Items
{
    public class ItemList : IModelArray<Item>
    {
        private Item[] itemssorted;
        private Item[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load(ScenarioType scenario)
        {
            if (scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/itemListS1.xml";
            }
            else if (scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/itemListS2.xml";
            }
            if (scenario == ScenarioType.Scenario3)
            {
                r = "Resources/itemList.xml";
            }
            else if (scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/itemListPD.xml";
            }

            itemssorted = new Item[0];
            items = new Item[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Item[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Item[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Item[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Item(scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ID] = itemssorted[old.Length];
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

        public Item[] Models => itemssorted;
    }
}
