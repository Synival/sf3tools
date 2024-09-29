using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Soulmate
{
    public static class SoulmateList
    {
        private static Soulmate[] itemssorted;
        private static Soulmate[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadSoulmateList()
        {


            
                r = "Resources/SoulmateList.xml";


            itemssorted = new Soulmate[0];
            items = new Soulmate[1771]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Soulmate[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Soulmate[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Soulmate[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Soulmate(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].SoulmateID] = itemssorted[old.Length];
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

        public static Soulmate[] getSoulmateList()
        {
            return itemssorted;
        }
        public static Soulmate getSoulmate(int id)
        {
            return items[id];
        }
    }
}
