using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.TableModels;
using SF3.TableModels.MPD.TextureChunk;

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
            TextureHeaderTable  = new TextureHeaderTable(Editor, 0x00);
            TextureHeaderTable.Load();
            var header = TextureHeaderTable.Rows[0];

            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = new TextureTable(Editor, 0x04, header.NumTextures, header.TextureIdStart)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }


        [BulkCopyRecurse]
        public TextureHeaderTable TextureHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public TextureTable TextureTable { get; private set; }
    }
}
