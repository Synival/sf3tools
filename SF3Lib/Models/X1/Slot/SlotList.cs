using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Models.X1.Slot {
    public class SlotList : Table<Slot> {
        public int MaxSize { get; } = 256;

        public SlotList(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;

            _resourceFile =Scenario == ScenarioType.Scenario1 ? "Resources/X1List.xml" : "Resources/X1OtherList.xml";
        }

        private readonly string _resourceFile;
        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new Slot[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                //int stop = 0;
                while (!xml.EOF) {
                    _ = xml.Read();
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
                stream?.Close();
            }
            return true;
        }
    }
}