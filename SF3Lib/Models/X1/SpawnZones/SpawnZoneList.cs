using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.SpawnZones
{
    public class SpawnZoneList : ModelArray<SpawnZone>
    {
        public SpawnZoneList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private SpawnZone[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/UnknownAIList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new SpawnZone[0];
            models = new SpawnZone[30]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                SpawnZone[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new SpawnZone[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new SpawnZone[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new SpawnZone(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].UnknownAIID] = _models[old.Length];
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
