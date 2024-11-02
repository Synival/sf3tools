using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class TreasureTable : Table<Treasure> {
        public override int? MaxSize => 255;

        /// <summary>
        /// TODO: what does this do when set?
        /// </summary>
        public static bool Debug { get; set; } = false;

        public TreasureTable(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFile("X1Treasure.xml");
        }

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile { get; }
        public override int Address => throw new NotImplementedException();

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
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                var myCount = 0;
                //Debug = true;
                //while (!xml.EOF && (_rows.Length == 0 || newRow.Searched != 0xffff))

                if (Debug == true) {
                    //while (!xml.EOF && (_rows.Length == 0 || (newRow.Searched != 0xffff || newRow.EventNumber != 0xffff)))
                    while (!xml.EOF && (_rows.Length == 0 || myCount <= 2)) {
                        {
                            _ = xml.Read();
                            if (xml.HasAttributes) {
                                var newRow = new Treasure(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1), 0);
                                _rows = _rows.ExpandedWith(newRow);
                                if (newRow.ID < 0 || newRow.ID >= MaxSize)
                                    throw new IndexOutOfRangeException();
                                if (newRow.Searched == 0xffff)
                                    myCount = 1 + myCount;
                            }
                        }
                    }
                }
                else {
                    while (!xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].Searched != 0xffff))
                    //while (!xml.EOF && (_rows.Length == 0 || (_rows[_rows.Length - 1].Searched != 0xffff || _rows[_rows.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (_rows.Length == 0 || myCount <= 2))
                    {
                        {
                            _ = xml.Read();
                            if (xml.HasAttributes) {
                                var newRow = new Treasure(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1), 0);
                                _rows = _rows.ExpandedWith(newRow);
                                if (newRow.ID < 0 || newRow.ID >= MaxSize)
                                    throw new IndexOutOfRangeException();
                                if (newRow.Searched == 0xffff)
                                    myCount = 1 + myCount;
                            }
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
