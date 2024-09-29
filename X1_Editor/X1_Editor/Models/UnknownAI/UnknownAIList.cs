using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.UnknownAI
{
    public static class UnknownAIList
    {
        private static UnknownAI[] itemssorted;
        private static UnknownAI[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadUnknownAIList()
        {


                r = "Resources/UnknownAIList.xml";


            itemssorted = new UnknownAI[0];
            items = new UnknownAI[30]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                UnknownAI[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new UnknownAI[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new UnknownAI[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new UnknownAI(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].UnknownAIID] = itemssorted[old.Length];
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

        public static UnknownAI[] getUnknownAIList()
        {
            return itemssorted;
        }
        public static UnknownAI getUnknownAI(int id)
        {
            return items[id];
        }
    }
}
