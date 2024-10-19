using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X002.MusicOverride
{
    public class MusicOverrideList : ModelArray<MusicOverride>
    {
        public MusicOverrideList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/S1/MusicOverrideList.xml";
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                _resourceFile = "Resources/S2/MusicOverrideList.xml";
            }
            if (Scenario == ScenarioType.Scenario3)
            {
                _resourceFile = "Resources/S3/MusicOverrideList.xml";
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                _resourceFile = "Resources/PD/MusicOverrideList.xml";
            }
        }

        private string _resourceFile;
        private MusicOverride[] items;
        private IX002_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new MusicOverride[0];
            items = new MusicOverride[300]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

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
                        old = new MusicOverride[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new MusicOverride[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new MusicOverride(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].MusicOverrideID] = _models[old.Length];
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
