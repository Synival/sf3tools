using System;
using System.Xml;
using System.IO;
using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X1.Slots
{
    public class SlotList : ModelArray<Slot>
    {
        public int MaxSize { get; } = 256;

        public SlotList(IX1_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                _resourceFile = "Resources/X1List.xml";
            }
            else
            {
                _resourceFile = "Resources/X1OtherList.xml";
            }
        }

        private string _resourceFile;
        private Slot[] models;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Slot[0];
            models = new Slot[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Slot[] old;
                //int stop = 0;
                while (!xml.EOF)
                {
                    xml.Read();
                    if (xml.HasAttributes)
                    {
                        old = new Slot[_models.Length];
                        _models.CopyTo(old, 0);
                        _models = new Slot[old.Length + 1];
                        old.CopyTo(_models, 0);
                        _models[old.Length] = new Slot(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        models[_models[_models.Length - 1].ID] = _models[_models.Length - 1];
                        /*Console.WriteLine(items[itemssorted[old.Length].ID].EnemyID);
                        //numberTest = items[itemssorted[old.Length].ID].EnemyID;
                        if (items[itemssorted[old.Length].ID].EnemyID == 0xffff)
                        {
                            stop = 1;
                        }*/
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
