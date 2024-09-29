using System;
//using System.Text;
using System.Xml;
using System.IO;
using static STHAEditor.Forms.frmMain;

namespace STHAEditor.Models.ExpLimit
{
    public static class ExpLimitList
    {
        private static ExpLimit[] itemssorted;
        private static ExpLimit[] items;


        private static string r = "";




        /// <summary>
        /// Initialises static class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public static bool loadExpLimitList()
        {




            
                r = "Resources/ExpLimitList.xml";
            


            itemssorted = new ExpLimit[0];
            items = new ExpLimit[2]; //max size of itemList
            try {
                FileStream stream = new FileStream(r, FileMode.Open);
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                ExpLimit[] old;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        old = new ExpLimit[itemssorted.Length];
                        itemssorted.CopyTo(old, 0);
                        itemssorted = new ExpLimit[old.Length + 1];
                        old.CopyTo(itemssorted, 0);
                        itemssorted[old.Length] = new ExpLimit(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].ExpLimitID] = itemssorted[old.Length];
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

        public static ExpLimit[] getExpLimitList()
        {
            return itemssorted;
        }
        public static ExpLimit getExpLimit(int id)
        {
            return items[id];
        }
    }
}
