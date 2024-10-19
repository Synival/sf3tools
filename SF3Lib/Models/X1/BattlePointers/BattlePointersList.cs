using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X1.BattlePointers
{
    public class BattlePointersList : ModelArray<BattlePointers>
    {
        public BattlePointersList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private BattlePointers[] items;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/BattlePointersList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new BattlePointers[0];
            items = new BattlePointers[5]; //max size of itemList
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                BattlePointers[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new BattlePointers[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new BattlePointers[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new BattlePointers(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].BattleID] = _models[old.Length];
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
