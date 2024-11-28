using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.MPD.TextureChunk;

namespace SF3.FileModels.MPD {
    public class TextureChunkEditor : TableEditor {
        protected TextureChunkEditor(IRawEditor editor, INameGetterContext nameContext, int address, string name)
        : base(editor, nameContext) {
            Address = address;
            Name    = name;
        }

        public static TextureChunkEditor Create(IRawEditor editor, INameGetterContext nameContext, int address, string name) {
            var newEditor = new TextureChunkEditor(editor, nameContext, address, name);
            newEditor.Init();
            return newEditor;
        }

        public override IEnumerable<ITable> MakeTables() {
            HeaderTable  = new HeaderTable(Editor, 0x00);
            HeaderTable.Load();
            var header = HeaderTable.Rows[0];

            return new List<ITable>() {
                HeaderTable,
                (TextureTable = new TextureTable(Editor, 0x04, header.NumTextures, header.TextureIdStart)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }


        [BulkCopyRecurse]
        public HeaderTable HeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
