using System;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Items
{
    public class ItemList
    {
        private Item[] itemssorted;
        private Item[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool loadItemList()
        {
            if (Globals.scenario == 1)
            {
                r = "Resources/X1List.xml";
            }
            else
                r = "Resources/X1OtherList.xml";

            itemssorted = new Item[0];
            items = new Item[256]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Item[] old;
                //int stop = 0;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Item[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Item[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Item(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ID] = itemssorted[old.Length];
                        /*Console.WriteLine(items[itemssorted[old.Length].ID].EnemyID);
                        //numberTest = items[itemssorted[old.Length].ID].EnemyID;
                        if (items[itemssorted[old.Length].ID].EnemyID == 0xffff)
                        {
                            stop = 1;
                        }*/
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

        public Item[] getItemList()
        {
            return itemssorted;
        }
        public Item getItem(int id)
        {
            return items[id];
        }
    }
}
