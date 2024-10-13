using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;

namespace SF3.X013_Editor.Models.SupportTypes
{
    public class SupportTypeList : ModelArray<SupportType>
    {
        public SupportTypeList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "RSc1/charactersS1.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "RSc2/charactersS2.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/characters.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "RPD/charactersPD.xml";
            }
        }

        private string _resourceFile;
        private IX013_FileEditor _fileEditor;
        private SupportType[] models;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new SupportType[0];
            models = new SupportType[120]; //max size of spellList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                SupportType[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new SupportType[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new SupportType[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new SupportType(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].SpellID] = _models[old.Length];
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
