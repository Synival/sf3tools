using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;

namespace SF3.Models.X002.StatBoost
{
    public class StatList : ModelArray<StatBoost>
    {
        public int MaxSize { get; } = 300;

        public StatList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        private StatBoost[] models;

        public override string ResourceFile => "Resources/X002StatList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new StatBoost[0];
            models = new StatBoost[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                StatBoost[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        _models = _models.ExpandedWith(new StatBoost(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1)));
                        models[_models[_models.Length - 1].StatID] = _models[_models.Length - 1];
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
