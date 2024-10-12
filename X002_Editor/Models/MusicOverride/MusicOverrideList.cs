using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X002_Editor.Models.MusicOverride
{
    public class MusicOverrideList : IModelArray<MusicOverride>
    {
        public MusicOverrideList(IX002_FileEditor fileEditor)
        {
            _fileEditor = fileEditor;
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        private MusicOverride[] itemssorted;
        private MusicOverride[] items;
        private IX002_FileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            if (Scenario == ScenarioType.Scenario1)
            {
                r = "RSc1/musicOverrideListS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                r = "RSc2/musicOverrideListS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                r = "Resources/musicOverrideList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                r = "RPD/musicOverrideListPD.xml";
            }

            itemssorted = new MusicOverride[0];
            items = new MusicOverride[300]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

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
                        itemssorted[old.Length] = new MusicOverride(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[itemssorted[old.Length].MusicOverrideID] = itemssorted[old.Length];
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

        public MusicOverride[] Models => itemssorted;
    }
}
