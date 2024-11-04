using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class ArrowTable : Table<Arrow> {
        public ArrowTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("X1Arrow.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Arrow(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.ArrowUnknown0 != 0xffff);

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 100;
    }
}
