using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.CustomMovement
{
    public class CustomMovementList : ModelArray<CustomMovement>
    {
        public int MaxSize { get; } = 130;

        public CustomMovementList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            /*
            if (Globals.scenario == ScenarioType.Scenario1)
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

        private CustomMovement[] spells;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1AI.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new CustomMovement[0];
            spells = new CustomMovement[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                CustomMovement[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new CustomMovement[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new CustomMovement[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new CustomMovement(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        spells[_models[old.Length].CustomMovementID] = _models[old.Length];
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
