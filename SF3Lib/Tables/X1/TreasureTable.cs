using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class TreasureTable : Table<Treasure> {
        /// <summary>
        /// TODO: what does this do when set?
        /// </summary>
        public static bool Debug { get; set; } = false;

        public TreasureTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Treasure.xml");
            Address = address;
        }

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Treasure[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                var myCount = 0;

                var whilePred = Debug
                    ? new Func<bool>(() => !xml.EOF && (_rows.Length == 0 || myCount <= 2))
                    : new Func<bool>(() => !xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].Searched != 0xffff));

                int address = Address;
                while (whilePred()) {
                    _ = xml.Read();
                    if (xml.HasAttributes) {
                        var newRow = new Treasure(FileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1), address);
                        address += newRow.Size;
                        _rows = _rows.ExpandedWith(newRow);
                        if (newRow.ID < 0 || newRow.ID >= MaxSize)
                            throw new IndexOutOfRangeException();
                        if (newRow.Searched == 0xffff)
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
        public override int? MaxSize => 255;
    }
}
