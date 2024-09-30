using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Critrate
{
    public static class CritrateList
    {
        private static Critrate[] itemssorted;
        private static Critrate[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadCritrateList()
        {
            r = "Resources/CritrateList.xml";



            itemssorted = new Critrate[0];
            items = new Critrate[3]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Critrate[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Critrate[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Critrate[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Critrate(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].CritrateID] = itemssorted[old.Length];
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

        public static Critrate[] getCritrateList()
        {
            return itemssorted;
        }
        public static Critrate getCritrate(int id)
        {
            return items[id];
        }
    }
}
