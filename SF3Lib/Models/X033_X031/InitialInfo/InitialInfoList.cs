using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using static SF3.Utils.Utils;

namespace SF3.Models.X033_X031.InitialInfos
{
    public class InitialInfoList : ModelArray<InitialInfo>
    {
        public int MaxSize { get; } = 100;

        public InitialInfoList(IX033_X031_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "ClassEquip.xml");
        }

        private string _resourceFile;
        private IX033_X031_FileEditor _fileEditor;
        private InitialInfo[] models;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new InitialInfo[0];
            models = new InitialInfo[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                InitialInfo[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new InitialInfo[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new InitialInfo[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new InitialInfo(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[old.Length].PresetID] = _models[old.Length];
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
