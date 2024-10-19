using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;
using SF3.X002_Editor.FileEditors;

namespace SF3.X002_Editor.Models.Presets
{
    public class PresetList : ModelArray<Preset>
    {
        public PresetList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/S1/SpellIndexList.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/S2/SpellIndexList.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/S3/SpellIndexList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/PD/SpellIndexList.xml";
            }
        }

        private string _resourceFile;
        private IX002_FileEditor _fileEditor;
        private Preset[] presets;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Preset[0];
            presets = new Preset[31]; //max size of spellIndexList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
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
                        old = new Preset[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Preset[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Preset(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        presets[_models[old.Length].PresetID] = _models[old.Length];
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
    }
}
