using System;
using System.Xml;
using System.IO;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.ExpLimit
{
    public class ExpLimitList
    {
        private ExpLimit[] itemssorted;
        private ExpLimit[] items;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool loadExpLimitList()
        {
            r = "Resources/ExpLimitList.xml";

            itemssorted = new ExpLimit[0];
            items = new ExpLimit[2]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                ExpLimit[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new ExpLimit[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new ExpLimit[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new ExpLimit(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ExpLimitID] = itemssorted[old.Length];
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

        public ExpLimit[] getExpLimitList()
        {
            return itemssorted;
        }
        public ExpLimit getExpLimit(int id)
        {
            return items[id];
        }
    }
}
