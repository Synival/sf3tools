using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using static SF3.Utils.Utils;

namespace SF3.Models.X033_X031.Stats
{
    public class StatsList : ModelArray<Stats>
    {
        public int MaxSize { get; } = 300;

        public StatsList(IX033_X031_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "ClassList.xml");
        }

        private string _resourceFile;
        private IX033_X031_FileEditor _fileEditor;
        private Stats[] stats;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Stats[0];
            stats = new Stats[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
                        old = new Stats[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Stats[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Stats(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        stats[_models[old.Length].ID] = _models[old.Length];
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
