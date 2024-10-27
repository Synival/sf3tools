using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X033_X031;
using static SF3.Utils.Resources;

namespace SF3.Tables.X033_X031 {
    public class InitialInfoTable : Table<InitialInfo> {
        public override int? MaxSize => 100;

        public InitialInfoTable(IX033_X031_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "ClassEquip.xml");
        }

        private readonly string _resourceFile;
        private readonly IX033_X031_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new InitialInfo[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);
                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new InitialInfo(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.PresetID < 0 || newRow.PresetID >= MaxSize)
                            throw new IndexOutOfRangeException();
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
