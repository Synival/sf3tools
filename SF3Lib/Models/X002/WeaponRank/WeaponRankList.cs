using System;
using System.Xml;
using System.IO;
using SF3.FileEditors;

namespace SF3.Models.X002.WeaponRank
{
    public class WeaponRankList : ModelArray<WeaponRank>
    {
        public int MaxSize { get; } = 5;

        public WeaponRankList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        private WeaponRank[] items;

        public override string ResourceFile => "Resources/WeaponRankList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new WeaponRank[0];
            items = new WeaponRank[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                WeaponRank[] old;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new WeaponRank[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new WeaponRank[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new WeaponRank(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        items[_models[old.Length].WeaponRankID] = _models[old.Length];
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
