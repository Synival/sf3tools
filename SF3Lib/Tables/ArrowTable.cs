using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class ArrowTable : Table<Arrow> {
        public ArrowTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Arrow.xml");
            Address = address;
        }

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Arrow[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                var myCount = 0;

                var address = Address;
                while (!xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].ArrowUnknown0 != 0xffff)) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new Arrow(FileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1), address);
                        address += newRow.Size;
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.ID < 0 || newRow.ID >= MaxSize)
                            throw new IndexOutOfRangeException();
                        if (newRow.ArrowUnknown0 == 0xffff)
                            myCount = 1 + myCount;
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

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 100;
    }
}
