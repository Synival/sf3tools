using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class TreasureTable : Table<Treasure> {
        /// <summary>
        /// TODO: what does this do when set?
        /// </summary>
        public static bool Debug { get; set; } = false;

        public TreasureTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Treasure.xml");
            Address = address;
        }

        public override bool Load() {
            return LoadFromResourceFile(
                (id, name, address) => new Treasure(FileEditor, id, name, address),
                Debug ? new ContinueReadingPredicate((rows, prev, cur) => rows.Count <= 2)
                      : new ContinueReadingPredicate((rows, prev, cur) => prev == null || prev.Searched != 0xffff));
        }

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 255;
    }
}
