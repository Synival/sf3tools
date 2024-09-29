using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Stats
{
    public static class StatsList
    {
        private static Stat[] itemssorted;
        private static Stat[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadStatList()
        {




                r = "Resources/StatList.xml";
            


            itemssorted = new Stat[0];
            items = new Stat[256]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Stat[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Stat[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Stat[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Stat(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].StatID] = itemssorted[old.Length];
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

        public static Stat[] getStatList()
        {
            return itemssorted;
        }
        public static Stat getStat(int id)
        {
            return items[id];
        }
    }
}
