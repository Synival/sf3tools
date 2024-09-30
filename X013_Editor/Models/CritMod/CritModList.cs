﻿using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.CritMod
{
    public static class CritModList
    {
        private static CritMod[] itemssorted;
        private static CritMod[] items;

        private static string r = "";

        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadCritModList()
        {
            r = "Resources/CritModList.xml";

            itemssorted = new CritMod[0];
            items = new CritMod[1]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                CritMod[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new CritMod[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new CritMod[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new CritMod(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].CritModID] = itemssorted[old.Length];
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

        public static CritMod[] getCritModList()
        {
            return itemssorted;
        }
        public static CritMod getCritMod(int id)
        {
            return items[id];
        }
    }
}
