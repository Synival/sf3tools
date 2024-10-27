using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static SF3.Utils.Resources;

namespace SF3.Tables.X1 {
    public class NpcTable : Table<Npc> {
        public override int? MaxSize => 100;

        public NpcTable(IX1_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly IX1_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/X1Npc.xml";
        public override int Address => throw new NotImplementedException();

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _rows = new Npc[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                _ = xml.Read();
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                var myCount = 0;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].Searched != 0xffff))

                /*if(Globals.treasureDebug == true)
                {
                    //while (!xml.EOF && (_rows.Length == 0 || (_rows[_rows.Length - 1].Searched != 0xffff || _rows[_rows.Length - 1].EventNumber != 0xffff)))
                    while (!xml.EOF && (_rows.Length == 0 || myCount <= 2))
                    {
                        {
                            xml.Read();
                            if (xml.HasAttributes)
                            {
                                var newRow = new Npc(Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _rows = _rows.ExpandedWith(newRow);
                                if (newRow.NpcID < 0 || newRow.NpcID >= MaxSize) {
throw new IndexOutOfRangeException();
}
                                if (newRow.SpriteID == 0xffff)
                                {
                                    myCount = 1 + myCount;
                                }
                            }
                        }
                    }
                }

                else*/
                {
                    while (!xml.EOF && (_rows.Length == 0 || _rows[_rows.Length - 1].SpriteID != 0xffff))
                    //while (!xml.EOF && (_rows.Length == 0 || (_rows[_rows.Length - 1].Searched != 0xffff || _rows[_rows.Length - 1].EventNumber != 0xffff)))
                    //while (!xml.EOF && (_rows.Length == 0 || myCount <= 2))
                    {
                        {
                            _ = xml.Read();
                            if (xml.HasAttributes) {
                                var newRow = new Npc(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                                _rows = _rows.ExpandedWith(newRow);
                                if (newRow.NpcID < 0 || newRow.NpcID >= MaxSize)
                                    throw new IndexOutOfRangeException();
                                if (newRow.SpriteID == 0xffff)
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
