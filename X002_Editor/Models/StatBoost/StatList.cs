using System;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.StatBoost
{
    public static class StatList
    {
        private static StatBoost[] itemssorted;
        private static StatBoost[] items;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadStatList()
        {
            r = "Resources/statList.xml";

            itemssorted = new StatBoost[0];
            items = new StatBoost[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                StatBoost[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new StatBoost[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new StatBoost[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new StatBoost(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].StatID] = itemssorted[old.Length];
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

        public static StatBoost[] getStatList()
        {
            return itemssorted;
        }
        public static StatBoost getStat(int id)
        {
            return items[id];
        }
    }
}
