using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X1;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X1 {
    public class EnterTable : Table<Enter> {
        public EnterTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Enter.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Enter(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.Entered != 0xffff);

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 100;
    }
}
