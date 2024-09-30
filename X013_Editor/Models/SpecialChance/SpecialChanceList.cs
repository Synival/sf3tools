using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.SpecialChance
{
    public static class SpecialChanceList
    {
        private static SpecialChance[] itemssorted;
        private static SpecialChance[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadSpecialChanceList()
        {




            r = "Resources/SpecialChanceList.xml";



            itemssorted = new SpecialChance[0];
            items = new SpecialChance[1]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                SpecialChance[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new SpecialChance[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new SpecialChance[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new SpecialChance(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].SpecialChanceID] = itemssorted[old.Length];
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

        public static SpecialChance[] getSpecialChanceList()
        {
            return itemssorted;
        }
        public static SpecialChance getSpecialChance(int id)
        {
            return items[id];
        }
    }
}
