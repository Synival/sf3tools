﻿using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.TextureChunk;
using SF3.RawData;

namespace SF3.Models.Files.MPD {
    public class MPD_FileTextureChunk : TableFile {
        protected MPD_FileTextureChunk(IRawData data, INameGetterContext nameContext, int address, string name)
        : base(data, nameContext) {
            Address = address;
            Name    = name;
        }

        public static MPD_FileTextureChunk Create(IRawData data, INameGetterContext nameContext, int address, string name) {
            var newFile = new MPD_FileTextureChunk(data, nameContext, address, name);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            TextureHeaderTable  = new TextureHeaderTable(Data, 0x00);
            TextureHeaderTable.Load();
            var header = TextureHeaderTable.Rows[0];

            return new List<ITable>() {
                TextureHeaderTable,
                (TextureTable = new TextureTable(Data, 0x04, header.NumTextures, header.TextureIdStart)),
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