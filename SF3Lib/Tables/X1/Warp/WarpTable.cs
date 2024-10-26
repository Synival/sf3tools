using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Tables.X1.Warp {
    public class WarpTable : Table<Warp> {
        public int MaxSize { get; } = 255;

        public WarpTable(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1Warp.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Warp[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].Searched != 0xffff))

                while (!xml.EOF && (_rows.Length == 0 || (_rows[_rows.Length - 1].WarpType != 0x01 && _rows[_rows.Length - 1].WarpType != 0xff)))
                //while (!xml.EOF && (_rows.Length == 0 || (_rows[_rows.Length - 1].Searched != 0xffff || _rows[_rows.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (_rows.Length == 0 || myCount <= 2))
                {
                    {
                        _ = xml.Read();
                        if (xml.HasAttributes) {
                            var newRow = new Warp(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                            _rows = _rows.ExpandedWith(newRow);
                            if (newRow.WarpID < 0 || newRow.WarpID >= MaxSize)
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
