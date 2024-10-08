using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X1_Editor.Models.Presets
{
    public class PresetList : IModelArray<Preset>
    {
        public PresetList(IFileEditor fileEditor, ScenarioType scenario)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;
        }

        public ScenarioType Scenario { get; }

        private Preset[] presetssorted;
        private Preset[] presets;
        private IFileEditor _fileEditor;

        private string r = "";

        /// <summary>
        /// Initialises class
        /// </summary>
        /// <returns>True or False if abilityList.xml does not exist/is in use</returns>
        public bool Load()
        {
            r = "Resources/X1Top.xml";

            presetssorted = new Preset[0];
            presets = new Preset[31]; //max size of spellIndexList
            FileStream stream = null;
            try
            {
                stream = new FileStream(r, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Preset[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Preset[presetssorted.Length];
                        presetssorted.CopyTo(old, 0);
                        presetssorted = new Preset[old.Length + 1];
                        old.CopyTo(presetssorted, 0);
                        presetssorted[old.Length] = new Preset(_fileEditor, Scenario, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        presets[presetssorted[old.Length].SizeID] = presetssorted[old.Length];
                    }
                }
            }
            catch (FileLoadException)
            {
                return false;
                //} catch (FileNotFoundException) {
                //  return false;
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

        public Preset[] Models => presetssorted;
    }
}
