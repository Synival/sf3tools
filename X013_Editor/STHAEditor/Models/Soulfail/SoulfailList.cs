using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.Soulfail
{
    public static class SoulfailList
    {
        private static Soulfail[] itemssorted;
        private static Soulfail[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadSoulfailList()
        {


            
                r = "Resources/Soulfail.xml";


            itemssorted = new Soulfail[0];
            items = new Soulfail[1]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Soulfail[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new Soulfail[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new Soulfail[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new Soulfail(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].SoulfailID] = itemssorted[old.Length];
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

        public static Soulfail[] getSoulfailList()
        {
            return itemssorted;
        }
        public static Soulfail getSoulfail(int id)
        {
            return items[id];
        }
    }
}
