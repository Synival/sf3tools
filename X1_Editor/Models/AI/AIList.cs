using System;
using System.Xml;
using System.IO;
using SF3.Models;
using SF3.Types;
using SF3.X1_Editor.FileEditors;

namespace SF3.X1_Editor.Models.AI
{
    public class AIList : ModelArray<AI>
    {
        public AIList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            /*if (Globals.scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/X1AIScn1.xml";
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else if (Globals.scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }
            else
            {
                _resourceFile = "Resources/X1AIOther.xml";
            }*/
        }

        private AI[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1AI.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new AI[0];
            models = new AI[130]; //max size of spellList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                AI[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new AI[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new AI[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new AI(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].AIID] = _models[old.Length];
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
