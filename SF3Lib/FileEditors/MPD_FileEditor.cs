using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.Attributes;
using MPDLib;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.MPD;
using SF3.Types;

namespace SF3.FileEditors {
    public class MPD_FileEditor : SF3FileEditor, IMPD_FileEditor {
        public MPD_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override bool LoadFile(string filename, Stream stream) {
            // Load MPDFile data
            var pos = stream.Position;
            Chunks = new ChunkCollection(stream);
            stream.Position = pos;

            // TODO: we need a ByteEditor!! Because this is not a file!!
            var chunk5Data = Chunks[5].Decompress();
            Chunk5Editor = new FileEditor(this.NameContext);
            using (var memoryStream = new MemoryStream(chunk5Data))
                ((FileEditor) Chunk5Editor).LoadFile(this.Filename + " (Chunk5)", memoryStream);

            return base.LoadFile(filename, stream);
        }

        public override bool SaveFile(string filename)
            => throw new NotImplementedException();

        public override IEnumerable<ITable> MakeTables() {
            var headerAddrPtr = GetDouble(0x0000) - 0x290000;
            var headerAddr = GetDouble(headerAddrPtr) - 0x290000;

            return new List<ITable>() {
                (Header       = new HeaderTable     (this, headerAddr)),
                (TileRows     = new TileRowTable    (Chunk5Editor, 0x4000)),
                (ItemTileRows = new ItemTileRowTable(Chunk5Editor, 0x6000)),
            };
        }

        public override void DestroyTables() {
            Header       = null;
            TileRows     = null;
            ItemTileRows = null;
        }

        public ChunkCollection Chunks { get; private set; }

        public IByteEditor Chunk5Editor { get; private set; }

        [BulkCopyRecurse]
        public HeaderTable Header { get; private set; }

        [BulkCopyRecurse]
        public TileRowTable TileRows { get; private set; }

        [BulkCopyRecurse]
        public ItemTileRowTable ItemTileRows { get; private set; }
    }
}
