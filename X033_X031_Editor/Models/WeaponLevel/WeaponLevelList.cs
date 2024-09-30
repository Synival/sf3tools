using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.WeaponLevel
{
    public static class WeaponLevelList
    {
        private static WeaponLevel[] itemssorted;
        private static WeaponLevel[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadWeaponLevelList()
        {





            r = "Resources/WeaponLevel.xml";



            itemssorted = new WeaponLevel[0];
            items = new WeaponLevel[2]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponLevel[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponLevel[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new WeaponLevel[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new WeaponLevel(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].WeaponLevelID] = itemssorted[old.Length];
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

        public static WeaponLevel[] getWeaponLevelList()
        {
            return itemssorted;
        }
        public static WeaponLevel getWeaponLevel(int id)
        {
            return items[id];
        }
    }
}
