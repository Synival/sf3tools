using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X033_X031_Editor.Models.Stats
{
    public class StatsList : ModelArray<Stats>
    {
        public StatsList(IX033_X031_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX033_X031_FileEditor _fileEditor;
        private Stats[] statsSorted;
        private Stats[] stats;

        private string r = "";

        /// <summary>
        /// Initialises list
        /// </summary>
        /// <returns>'true' on success, 'false' if .xml files do not exist or are in use</returns>
        public override bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/classListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/classListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/classList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/classListPD.xml";
            }

            statsSorted = new Stats[0];
            stats = new Stats[300]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

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
                        statsSorted[old.Length] = new Stats(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        stats[statsSorted[old.Length].ID] = statsSorted[old.Length];
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return true;
        }

        public Stats[] Models => statsSorted;

    }
}
