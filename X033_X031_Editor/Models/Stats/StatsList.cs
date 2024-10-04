using System;
using System.Xml;
using System.IO;
using static SF3.X033_X031_Editor.Forms.frmMain;
using SF3.Models;

namespace SF3.X033_X031_Editor.Models.Stats
{
    public class StatsList : IModelArray<Stats>
    {
        private Stats[] statsSorted;
        private Stats[] stats;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public bool Load()
        {
            if (Globals.scenario == 1)
            {
                r = "RSc1/classListS1.xml";
            }
            else if (Globals.scenario == 2)
            {
                r = "RSc2/classListS2.xml";
            }
            if (Globals.scenario == 3)
            {
                r = "Resources/classList.xml";
            }
            else if (Globals.scenario == 4)
            {
                r = "RPD/classListPD.xml";
            }

            statsSorted = new Stats[0];
            stats = new Stats[300]; //max size of itemList
            try
            {
                FileStream stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Stats[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Stats[statsSorted.Length];
                        statsSorted.CopyTo(old, 0);
                        statsSorted = new Stats[old.Length + 1];
                        old.CopyTo(statsSorted, 0);
                        statsSorted[old.Length] = new Stats(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        stats[statsSorted[old.Length].ID] = statsSorted[old.Length];
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

        public Stats[] Models => statsSorted;

    }
}
