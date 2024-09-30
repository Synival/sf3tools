using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.HealExp
{
    public static class HealExpList
    {
        private static HealExp[] itemssorted;
        private static HealExp[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadHealExpList()
        {





            r = "Resources/HealExpList.xml";



            itemssorted = new HealExp[0];
            items = new HealExp[2]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                HealExp[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new HealExp[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new HealExp[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new HealExp(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].HealExpID] = itemssorted[old.Length];
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

        public static HealExp[] getHealExpList()
        {
            return itemssorted;
        }
        public static HealExp getHealExp(int id)
        {
            return items[id];
        }
    }
}
