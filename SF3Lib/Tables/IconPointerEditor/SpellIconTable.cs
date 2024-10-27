using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.IconPointerEditor;
using static SF3.Utils.Resources;

namespace SF3.Tables.IconPointerEditor {
    public class SpellIconTable : Table<SpellIcon> {
        public override int? MaxSize => 256;

        public SpellIconTable(IIconPointerFileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "SpellIcons.xml");
        }

        private readonly string _resourceFile;
        private readonly IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new SpellIcon[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new SpellIcon(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.ID < 0 || newRow.ID >= MaxSize)
                            throw new IndexOutOfRangeException();
                        //MessageBox.Show("" + _fileEditor.GetDouble(newRow.Address));
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
