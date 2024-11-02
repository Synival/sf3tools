using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X013;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class SpecialChanceTable : Table<SpecialChance> {
        public override int? MaxSize => 1;

        public SpecialChanceTable(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFile("SpecialChanceList.xml");
        }

        private readonly IX013_FileEditor _fileEditor;

        public override string ResourceFile { get; }
        public override int Address => throw new NotImplementedException();

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new SpecialChance[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new SpecialChance(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1), 0, Scenario >= ScenarioType.Scenario3);
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.ID < 0 || newRow.ID >= MaxSize)
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
