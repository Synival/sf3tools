using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X013.HealExp
{
    public class HealExpList : ModelArray<HealExp>
    {
        public int MaxSize { get; } = 2;

        public HealExpList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        private HealExp[] items;

        public override string ResourceFile => "Resources/HealExpList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new HealExp[0];
            items = new HealExp[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                HealExp[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new HealExp[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new HealExp[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new HealExp(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[_models.Length - 1].HealExpID] = _models[_models.Length - 1];
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
