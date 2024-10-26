using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Tables.X013.Specials {
    public class SpecialTable : Table<Special> {
        public int MaxSize { get; } = 256;

        public SpecialTable(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "Specials.xml");
        }

        private readonly string _resourceFile;
        private readonly IX013_FileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Special[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new Special(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.ID < 0 || newRow.ID >= MaxSize) {
                            throw new IndexOutOfRangeException();
                        }
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
