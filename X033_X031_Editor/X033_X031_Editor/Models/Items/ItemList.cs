using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Items
{
    public static class ItemList
    {
        private static Item[] itemssorted;
        private static Item[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadItemList()
        {




            if (Globals.scenario == 1)
            {
                r = "RSc1/classListS1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "RSc2/classListS2.xml";
            }
            if (Globals.scenario == 3)
            {
                r = "Resources/classList.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "RPD/classListPD.xml";
            }


            itemssorted = new Item[0];
            items = new Item[300]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Item[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Item[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Item[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Item(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ID] = itemssorted[old.Length];
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

        public static Item[] getItemList()
        {
            return itemssorted;
        }
        public static Item getItem(int id)
        {
            return items[id];
        }
    }
}
