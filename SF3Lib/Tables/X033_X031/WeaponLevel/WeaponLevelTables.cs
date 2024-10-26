using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Tables.X033_X031.WeaponLevel {
    public class WeaponLevelTables : Table<WeaponLevel> {
        public int MaxSize { get; } = 2;

        public WeaponLevelTables(IX033_X031_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly IX033_X031_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/WeaponLevel.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new WeaponLevel[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new WeaponLevel(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.WeaponLevelID < 0 || newRow.WeaponLevelID >= MaxSize) {
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
