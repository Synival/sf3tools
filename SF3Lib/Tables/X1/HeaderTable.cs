using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class HeaderTable : Table<Header> {
        public override int? MaxSize => 31;

        public HeaderTable(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFile("X1Top.xml");
        }

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile { get; }
        public override int Address => throw new NotImplementedException();

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Header[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                while (!xml.EOF) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new Header(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.SizeID < 0 || newRow.SizeID >= MaxSize)
                            throw new IndexOutOfRangeException();
                    }
                }
            }
            catch (FileLoadException) {
                return false;
                //} catch (FileNotFoundException) {
                //  return false;
            }
            finally {
                stream?.Close();
            }
            return true;
        }
    }
}
