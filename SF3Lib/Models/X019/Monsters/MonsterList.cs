using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X019.Monsters
{
    public class MonsterList : ModelArray<Monster>
    {
        public int MaxSize { get; } = 256;

        public MonsterList(IX019_FileEditor fileEditor, bool isX044) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _isX044 = isX044;
            _resourceFile = (Scenario == ScenarioType.PremiumDisk && isX044)
                ? "Resources/PD/Monsters_X044.xml"
                : ResourceFileForScenario(_fileEditor.Scenario, "Monsters.xml");
        }

        private string _resourceFile;
        private IX019_FileEditor _fileEditor;
        private bool _isX044;
        private Monster[] models;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Monster[0];
            models = new Monster[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Monster[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Monster[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Monster[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Monster(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[_models.Length - 1].ID] = _models[_models.Length - 1];
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
