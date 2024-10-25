using System;
using System.IO;
using SF3.Extensions;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.Slots {
    public class SlotList : ModelArray<Slot> {
        public int MaxSize { get; } = 256;

        public SlotList(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1) {
                _resourceFile = "Resources/X1List.xml";
            }
            else {
                _resourceFile = "Resources/X1OtherList.xml";
            }
        }

        private string _resourceFile;
        private IX1_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new Slot[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open);

                var xml = MakeXmlReader(stream);
                xml.Read();
                //int stop = 0;
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        var newModel = new Slot(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                        if (newModel.ID < 0 || newModel.ID >= MaxSize) {
                            throw new IndexOutOfRangeException();
                        }
                        /*Console.WriteLine(items[itemssorted[old.Length].ID].EnemyID);
                        //numberTest = items[itemssorted[old.Length].ID].EnemyID;
                        if (items[itemssorted[old.Length].ID].EnemyID == 0xffff)
                        {
                            stop = 1;
                        }*/
                    }
                }
            }
            catch (FileLoadException) {
                return false;
            }
            catch (FileNotFoundException) {
                return false;
            }
            finally {
                if (stream != null) {
                    stream.Close();
                }
            }
            return true;
        }
    }
}
