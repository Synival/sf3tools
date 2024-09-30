using System;
//using System.Text;
using System.Xml;
using System.IO;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public static class MusicOverrideList
    {
        private static MusicOverride[] itemssorted;
        private static MusicOverride[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadMusicOverrideList()
        {




            if (Globals.scenario == 1)
            {
                r = "RSc1/musicOverrideListS1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "RSc2/musicOverrideListS2.xml";
            }
            if (Globals.scenario == 3)
            {
                r = "Resources/musicOverrideList.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "RPD/musicOverrideListPD.xml";
            }


            itemssorted = new MusicOverride[0];
            items = new MusicOverride[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                MusicOverride[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new MusicOverride[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new MusicOverride[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new MusicOverride(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].MusicOverrideID] = itemssorted[old.Length];
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

        public static MusicOverride[] getMusicOverrideList()
        {
            return itemssorted;
        }
        public static MusicOverride getMusicOverride(int id)
        {
            return items[id];
        }
    }
}
