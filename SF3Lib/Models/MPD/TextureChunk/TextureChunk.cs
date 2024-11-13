using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.MPD.TextureChunk;

namespace SF3.Models.MPD.TextureChunk {
    public class TextureChunk {
        public TextureChunk(IRawEditor editor, int address, string name) {
            Editor  = editor;
            Address = address;
            Name    = name;

            HeaderTable  = new HeaderTable(Editor, 0x00);
            HeaderTable.Load();
            var header = HeaderTable.Rows[0];
            TextureTable = new TextureTable(Editor, 0x04, header.NumTextures, header.TextureIdStart);

            Tables = new List<ITable>() {
                HeaderTable,
                TextureTable
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public IRawEditor Editor { get; }
        public int Address { get; }

        public List<ITable> Tables { get; }

        [BulkCopyRecurse]
        public HeaderTable HeaderTable { get; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; }
    }
}
