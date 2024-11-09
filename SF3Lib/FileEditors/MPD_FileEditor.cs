using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.Attributes;
using MPDLib;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public class MPD_FileEditor : SF3FileEditor, IMPD_FileEditor {
        public MPD_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override bool LoadFile(string filename, Stream stream) {
            var mpdFile = new MPDFile();
            mpdFile.FetchChunkDefinitions(stream);
            var chunk5Data = mpdFile.Chunks[5].Decompress(stream);

            using (var chunk5Stream = new MemoryStream(chunk5Data))
                return base.LoadFile(filename, chunk5Stream);
        }

        public override bool SaveFile(string filename)
            => throw new NotImplementedException();

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (TileRows     = new TileRowTable    (this, 0x4000)),
                (ItemTileRows = new ItemTileRowTable(this, 0x6000)),
            };
        }

        public override void DestroyTables() {
            TileRows     = null;
            ItemTileRows = null;
        }

        [BulkCopyRecurse]
        public TileRowTable TileRows { get; private set; }

        [BulkCopyRecurse]
        public ItemTileRowTable ItemTileRows { get; private set; }
    }
}
