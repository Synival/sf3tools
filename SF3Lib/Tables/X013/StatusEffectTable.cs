using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X013;
using static SF3.Utils.Resources;

namespace SF3.Tables.X013 {
    public class StatusEffectTable : Table<StatusEffect> {
        public override int? MaxSize => 1000;

        public StatusEffectTable(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/StatusGroupList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new StatusEffect[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                //string myName = "WarpIndex " + myCount;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_rows.Length == 0 || newRow.Searched != 0xffff))

                while (!xml.EOF)
                //while (!xml.EOF && (_rows.Length == 0 || (newRow.Searched != 0xffff || newRow.EventNumber != 0xffff)))
                //while (!xml.EOF && (_rows.Length == 0 || myCount <= 2))
                {
                    {
                        _ = xml.Read();
                        if (xml.HasAttributes) {
                            var newRow = new StatusEffect(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                            _rows = _rows.ExpandedWith(newRow);

                            if (newRow.StatusEffectID < 0 || newRow.StatusEffectID >= MaxSize)
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
