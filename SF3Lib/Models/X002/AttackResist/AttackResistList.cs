using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X002.AttackResist
{
    public class AttackResistList : ModelArray<AttackResist>
    {
        public int MaxSize { get; } = 2;

        public AttackResistList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        private AttackResist[] items;

        public override string ResourceFile => "Resources/AttackResistList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new AttackResist[0];
            items = new AttackResist[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                AttackResist[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new AttackResist[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new AttackResist[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new AttackResist(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[_models.Length - 1].AttackResistID] = _models[_models.Length - 1];
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
