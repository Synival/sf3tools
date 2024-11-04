using System;
using SF3.FileEditors;
using SF3.Models;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class StatusEffectTable : Table<StatusEffect> {
        public StatusEffectTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFile("StatusGroupList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new StatusEffect(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 1000;
    }
}
