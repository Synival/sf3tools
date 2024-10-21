using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X013.ExpLimit
{
    public class ExpLimitList : ModelArray<ExpLimit>
    {
        public int MaxSize { get; } = 2;

        public ExpLimitList(IX013_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;
        private ExpLimit[] items;

        public override string ResourceFile => "Resources/ExpLimitList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new ExpLimit[0];
            items = new ExpLimit[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                ExpLimit[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new ExpLimit[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new ExpLimit[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new ExpLimit(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[_models.Length - 1].ExpLimitID] = _models[_models.Length - 1];
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
