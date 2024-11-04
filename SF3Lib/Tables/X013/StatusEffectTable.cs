using System;
using SF3.FileEditors;
using SF3.Models.X013;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables.X013 {
    public class StatusEffectTable : Table<StatusEffect> {
        public StatusEffectTable(IX013_FileEditor fileEditor, int address) : base(fileEditor) {
            _fileEditor = fileEditor;
            ResourceFile = ResourceFile("StatusGroupList.xml");
            Address = address;
        }

        private readonly IX013_FileEditor _fileEditor;

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new StatusEffect(_fileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 1000;
    }
}
